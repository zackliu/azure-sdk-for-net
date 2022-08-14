// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The message error detail.
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        ///  The name of error
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The detailed message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDetail"/> class.
        /// </summary>
        /// <param name="name">The name of error</param>
        /// <param name="message">The detailed message</param>
        public ErrorDetail(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
