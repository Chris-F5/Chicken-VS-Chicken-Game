using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Logging;

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
        private List<byte> recievedData;

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

            recievedData = new List<byte>();
            recieveBuffer = new byte[dataBufferSize];
            stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
        }

        public void Send(byte[] _bytes)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_bytes, 0, _bytes.Length, null, null);
                }
            }
            catch (Exception _ex)
            {
                Logger.LogWarning($"Error sending packet to player {client.id} via TCP: {_ex}");
                //TODO: disconect client
            }
        }

        private void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                // This line waits for a pending asynchronous read to complete.
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

                    HandleData(_data);

                    stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error recieving TCP data: {_ex}");
                //TODO: disconect client
            }
        }

        private void HandleData(byte[] _data)
        {
            int packetLength = 0;

            //recievedData.SetBytes(_data);
            recievedData.AddRange(_data);

            if (packetLength == 0 && recievedData.Count >= 4)
            {
                byte[] lengthBytes = recievedData.GetRange(0, 4).ToArray();

                packetLength = BitConverter.ToInt32(lengthBytes,0);
                if (packetLength <= 0)
                {
                    Logger.LogWarning("Client sent packet with claimed length of 0 or less.");
                    recievedData.Clear();
                    return;
                }
            }

            while (packetLength > 0 && packetLength <= recievedData.Count)
            {
                byte[] packetBytes = recievedData.GetRange(0, packetLength).ToArray();
                recievedData.RemoveRange(0, packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    PacketReader packet = new PacketReader(packetBytes);
                    client.HandlePacket(packet);
                });

                packetLength = 0;
                if (recievedData.Count >= 4)
                {
                    byte[] lengthBytes = recievedData.GetRange(0, 4).ToArray();

                    packetLength = BitConverter.ToInt32(lengthBytes, 0);
                    if (packetLength <= 0)
                    {
                        Logger.LogWarning("Recieved TCP packet with claimed length of 0 or less.");
                        recievedData.Clear();
                        return;
                    }
                }
            }
            if (packetLength <= 1)
            {
                recievedData.Clear();
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

            if (Client.canAcceptNewClient)
            {
                new Client(_client);
            }
            else 
            {
                Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: server full");
            }
        }
    }
}