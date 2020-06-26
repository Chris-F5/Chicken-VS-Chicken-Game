using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class TCP
    {
        public static int dataBufferSize = 4096;
        private static TcpListener tcpListener;

        private readonly Client client;
        public TcpClient socket;
        private NetworkStream stream;
        private byte[] recieveBuffer;
        private Packet recievedData;

        public TCP(Client _client)
        {
            client = _client;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;
            stream = socket.GetStream();

            recievedData = new Packet();
            recieveBuffer = new byte[dataBufferSize];
            stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);


            client.Welcome();
        }

        public void Send(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    _packet.WriteLength();
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending packet to player {client.id} via TCP: {_ex}");
                //TODO: disconect client
            }
        }

        private void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    // TODO: disconect client
                    return;
                }
                else
                {
                    byte[] _data = new byte[_byteLength];
                    Array.Copy(recieveBuffer, _data, _byteLength);

                    recievedData.Reset(HandleData(_data));

                    stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error recieving TCP data: {_ex}");
                //TODO: disconect client
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            recievedData.SetBytes(_data);

            if (recievedData.UnreadLength() >= 4)
            {
                _packetLength = recievedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= recievedData.UnreadLength())
            {
                byte[] _packetBytes = recievedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        client.HandlePacket(_packet);
                    }
                });

                _packetLength = 0;
                if (recievedData.UnreadLength() >= 4)
                {
                    _packetLength = recievedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }
            if (_packetLength <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void StartListening(int _port)
        {
            tcpListener = new TcpListener(IPAddress.Any, _port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCP.ConnectCallback), null);
            Console.WriteLine($"Started listening for TCP on port {_port}");
        }
        public static void ConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(ConnectCallback), null);

            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}");

            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                if (!ServerManager.clients[i].IsInUse())
                {
                    ServerManager.clients[i].TCPConnect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: server full");
        }
        public static void SendToAll(Packet _packet)
        {

        }
    }
}
