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
    class Server
    {
        public TcpListener Listener;
        public List<ClientInfo> clients = new List<ClientInfo>();
        public List<ClientInfo> newClients = new List<ClientInfo>();
        public static Server server;
        static TextWriter Out;

        public Server(int Port, TextWriter _Out)
        {
            Out = _Out;
            Server.server = this;

            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();
        }

        public void Work()
        {
            Thread clientListener = new Thread(ListenerClients);
            clientListener.Start();

            while (true)
            {
                foreach (ClientInfo client in clients)
                {
                    if (client.IsConnect)
                    {
                        NetworkStream stream = client.Client.GetStream();
                        while (stream.DataAvailable)
                        {
                            int ReadByte = stream.ReadByte();
                            if (ReadByte != -1)
                            {
                                client.buffer.Add((byte)ReadByte);
                            }
                        }
                        if (client.buffer.Count > 0)
                        {
                            Out.WriteLine("Resend");
                            foreach (ClientInfo otherClient in clients)
                            {
                                byte[] msg = client.buffer.ToArray();
                                client.buffer.Clear();
                                foreach (ClientInfo _otherClient in clients)
                                {
                                        try
                                        {
                                            _otherClient.Client.GetStream().Write(msg, 0, msg.Length);
                                        }
                                        catch
                                        {
                                            _otherClient.IsConnect = false;
                                            _otherClient.Client.Close();
                                        }
                                }
                            }
                        }
                    }
                }

                clients.RemoveAll(delegate (ClientInfo CI)
                {
                    if (!CI.IsConnect)
                    {
                        Server.Out.WriteLine("Client disconnect");
                        return true;
                    }
                    return false;
                });

                if(newClients.Count > 0)
                {
                    clients.AddRange(newClients);
                    newClients.Clear();
                }
            }
        }

        ~Server()
        {
            if(Listener != null)
            {
                Listener.Stop();
            }
            foreach (ClientInfo client in clients)
            {
                client.Client.Close();
            }
        }

        static void ListenerClients()
        {
            while (true)
            {
                server.newClients.Add(new ClientInfo(server.Listener.AcceptTcpClient()));
                Out.WriteLine("New Client");
            }
        }
    }
}
