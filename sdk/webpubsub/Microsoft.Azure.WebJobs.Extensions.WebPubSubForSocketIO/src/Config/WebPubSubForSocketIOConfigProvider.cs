// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Azure.WebPubSub.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    [Extension("WebPubSubForSocketIO", "webpubsubforsocketio")]
    internal class WebPubSubForSocketIOConfigProvider : IExtensionConfigProvider, IAsyncConverter<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly IConfiguration _configuration;
        private readonly INameResolver _nameResolver;
        private readonly ILogger _logger;
        private readonly WebPubSubFunctionsOptions _options;
        private readonly IWebPubSubTriggerDispatcher _dispatcher;
        private readonly SocketLifetimeStore _socketLifetimeStore;

        public WebPubSubForSocketIOConfigProvider(
            IOptions<WebPubSubFunctionsOptions> options,
            INameResolver nameResolver,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _options = options.Value;
            _logger = loggerFactory.CreateLogger(LogCategories.CreateTriggerCategory("WebPubSubForSocketIO"));
            _nameResolver = nameResolver;
            _configuration = configuration;
            _dispatcher = new WebPubSubForSocketIOTriggerDispatcher(_logger, _options);
            _socketLifetimeStore = new SocketLifetimeStore();
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrEmpty(_options.ConnectionString))
            {
                _options.ConnectionString = _nameResolver.Resolve(Constants.WebPubSubConnectionStringName);
            }

            if (string.IsNullOrEmpty(_options.Hub))
            {
                _options.Hub = _nameResolver.Resolve(Constants.HubNameStringName);
            }

            Exception webhookException = null;
            try
            {
#pragma warning disable CS0618 // Type or member is obsolete preview
                var url = context.GetWebhookHandler();
#pragma warning restore CS0618 // Type or member is obsolete preview
                _logger.LogInformation($"Registered Web PubSub for Socket.IO negotiate Endpoint = {url?.GetLeftPart(UriPartial.Path)}");
            }
            catch (Exception ex)
            {
                // disable trigger.
                webhookException = ex;
            }

            // register JsonConverters
            RegisterJsonConverter();

            // bindings
            context
                .AddConverter<WebPubSubConnection, JObject>(JObject.FromObject)
                .AddConverter<WebPubSubContext, JObject>(JObject.FromObject)
                .AddConverter<JObject, WebPubSubForSocketIOAction>(ConvertToWebPubSubOperation)
                .AddConverter<JArray, WebPubSubForSocketIOAction[]>(ConvertToWebPubSubOperationArray);

            // Trigger binding
            context.AddBindingRule<WebPubSubTriggerAttribute>()
                .BindToTrigger(new WebPubSubTriggerBindingProvider(_dispatcher, _nameResolver, _options, webhookException));

            // Input binding -- For negotiation token
            var webpubsubConnectionAttributeRule = context.AddBindingRule<WebPubSubConnectionAttribute>();
            webpubsubConnectionAttributeRule.AddValidator(ValidateWebPubSubConnectionAttributeBinding);
            webpubsubConnectionAttributeRule.BindToInput(GetClientConnection);

            // Output binding
            var webPubSubAttributeRule = context.AddBindingRule<WebPubSubForSocketIOAttribute>();
            webPubSubAttributeRule.AddValidator(ValidateWebPubSubAttributeBinding);
            webPubSubAttributeRule.BindToCollector(CreateCollector);

            _logger.LogInformation("Azure Web PubSub for Socket.IO binding initialized");
        }

        public Task<HttpResponseMessage> ConvertAsync(HttpRequestMessage input, CancellationToken cancellationToken)
        {
            return _dispatcher.ExecuteAsync(input, cancellationToken);
        }

        private void ValidateWebPubSubConnectionAttributeBinding(WebPubSubConnectionAttribute attribute, Type type)
        {
            ValidateConnectionString(
                attribute.Connection,
                $"{nameof(WebPubSubConnectionAttribute)}.{nameof(WebPubSubConnectionAttribute.Connection)}");
        }

        private void ValidateWebPubSubAttributeBinding(WebPubSubForSocketIOAttribute attribute, Type type)
        {
            ValidateConnectionString(
                attribute.Connection,
                $"{nameof(WebPubSubForSocketIOAttribute)}.{nameof(WebPubSubForSocketIOAttribute.Connection)}");
        }

        internal WebPubSubService GetService(WebPubSubForSocketIOAttribute attribute)
        {
            var connectionString = Utilities.FirstOrDefault(attribute.Connection, _options.ConnectionString);
            var hubName = Utilities.FirstOrDefault(attribute.Hub, _options.Hub);
            return new WebPubSubService(connectionString, hubName);
        }

        private IAsyncCollector<WebPubSubForSocketIOAction> CreateCollector(WebPubSubForSocketIOAttribute attribute)
        {
            return new WebPubSubForSocketIOAsyncCollector(GetService(attribute), _socketLifetimeStore);
        }

        private WebPubSubConnection GetClientConnection(WebPubSubConnectionAttribute attribute)
        {
            var hub = Utilities.FirstOrDefault(attribute.Hub, _options.Hub);
            var service = new WebPubSubService(attribute.Connection, hub);
            return service.GetClientConnection();
        }

        private void ValidateConnectionString(string attributeConnectionString, string attributeConnectionStringName)
        {
            var connectionString = Utilities.FirstOrDefault(attributeConnectionString, _options.ConnectionString);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"The Service connection string must be set either via an '{Constants.WebPubSubConnectionStringName}' app setting, via an '{Constants.WebPubSubConnectionStringName}' environment variable, or directly in code via {nameof(WebPubSubFunctionsOptions)}.{nameof(WebPubSubFunctionsOptions.ConnectionString)} or {attributeConnectionStringName}.");
            }
        }

        internal static void RegisterJsonConverter()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new BinaryDataJsonConverter(),
                    new ConnectionStatesNewtonsoftConverter(),
                },
            };
        }

        internal static WebPubSubForSocketIOAction ConvertToWebPubSubOperation(JObject input)
        {
            if (input.TryGetValue("actionName", StringComparison.OrdinalIgnoreCase, out var kind))
            {
                var opeartions = typeof(WebPubSubForSocketIOAction).Assembly.GetTypes().Where(t => t.BaseType == typeof(WebPubSubForSocketIOAction));
                foreach (var item in opeartions)
                {
                    if (TryToWebPubSubOperation(input, kind.ToString() + "Action", item, out var operation))
                    {
                        return operation;
                    }
                }
            }
            throw new ArgumentException($"Not supported WebPubSubOperation: {kind}.");
        }

        internal static WebPubSubForSocketIOAction[] ConvertToWebPubSubOperationArray(JArray input)
        {
            var result = new List<WebPubSubForSocketIOAction>();
            foreach (var item in input)
            {
                result.Add(ConvertToWebPubSubOperation((JObject)item));
            }
            return result.ToArray();
        }

        private static bool TryToWebPubSubOperation(JObject input, string actionName, Type operationType, out WebPubSubForSocketIOAction operation)
        {
            if (actionName.Equals(operationType.Name, StringComparison.OrdinalIgnoreCase))
            {
                operation = input.ToObject(operationType) as WebPubSubForSocketIOAction;
                return true;
            }
            operation = null;
            return false;
        }
    }
}
