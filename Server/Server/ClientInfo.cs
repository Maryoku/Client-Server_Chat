using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class ClientInfo
    {
        public TcpClient Client;
        public List<byte> buffer = new List<byte>();
        public bool IsConnect;

        public ClientInfo(TcpClient Client)
        {
            this.Client = Client;
            IsConnect = true;
        }
    }
}
