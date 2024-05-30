// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    //<packet type>[<# of binary attachments>-][<namespace>,][<acknowledgment id>][JSON-stringified payload without binary]
    internal class SocketIOProtocol
    {
        public static string EncodePacket(SocketIOPacket packet)
        {
            var sb = new StringBuilder();
            sb.Append(packet.Type.ToString("D"));

            // if we have a namespace other than `/`
            // we append it followed by a comma `,`
            if (packet.Namespace != "/")
            {
                sb.Append(packet.Namespace);
                sb.Append(",");
            }

            // immediately followed by the id
            if (packet.Id != null)
            {
                sb.Append(packet.Id.ToString());
            }

            // json data
            sb.Append(packet.Data);
            return sb.ToString();
        }
    }
}
