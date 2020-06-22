using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Client
    {
        public static int dataBufferSize = 4096;

        public readonly int id;
        public GameObjects.Player playerObject;

        public readonly TCP tcp;
        public readonly UDP udp;

        public Client(int _id)
        {
            id = _id;
            tcp = new TCP(id);
            udp = new UDP(id);
        }

        private void CreatePlayer()
        {
            playerObject = new GameObjects.Player(new Vector2(0,10));
        }
        public void TCPConnect(TcpClient _socket)
        {
            tcp.Connect(_socket);
            CreatePlayer();
        }
        public void UDPConnect(IPEndPoint _endPoint)
        {
            udp.Connect(_endPoint);
        }
        public bool IsInUse()
        {
            return (tcp.socket != null);
        }
        public bool IsUDPConnected()
        {
            return (udp.endPoint != null);
        }
        public bool VerifEndPoint(IPEndPoint _iPEndPoint)
        {
            return (_iPEndPoint.ToString() == udp.endPoint.ToString());
        }
        public class TCP
        {
            private readonly int id;
            public TcpClient socket;
            private NetworkStream stream;
            private byte[] recieveBuffer;
            private Packet recievedData;

            public TCP(int _id)
            {
                id = _id;
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

                
                ServerSend.Welcome(id, "Welcome!");
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending packet to player {id} via TCP: {_ex}");
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
                            int _packetId = _packet.ReadInt();
                            ServerManager.packetHandlers[_packetId](id, _packet);
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
        }
        public class UDP{
            public IPEndPoint endPoint;

            private readonly int id;

            public UDP(int _id)
            {
                id = _id;
            }

            public void Connect(IPEndPoint _endPoint)
            {
                endPoint = _endPoint;

                ServerSend.UDPTest(id);
            }

            public void SendData(Packet _packet)
            {
                ServerManager.SendUDPData(endPoint, _packet);
            }

            public void HandleData(Packet _packetData)
            {
                int _packetLength = _packetData.ReadInt();
                byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        ServerManager.packetHandlers[_packetId](id, _packet);
                    }
                });
            }
        }
    }
}
