using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    /// <summary>
    /// For local tracking the lifetime of sockets.
    /// </summary>
    internal class SocketLifetimeStore
    {
        public bool TryFindConnectionIdBySocketId(string socketId, out string connectionId, out string @namespace)
        {
            throw new NotImplementedException();
        }

        public void AddSocket(string socketId, string @namespace, string connectionId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveSocket(string socketId)
        {
            throw new NotImplementedException();
        }
    }
}
