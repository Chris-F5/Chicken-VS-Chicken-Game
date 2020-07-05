using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

namespace GameClient
{
    public class Client : MonoBehaviour
    {
        public static Client instance;
        public static int dataBufferSize = 4096;

        public int myId = -1;
        public TCP tcp;
        public UDP udp;

        public string serverIp = "127.0.0.1";
        public int serverPort = 26950;

        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<byte, PacketHandler> packetHandlers;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.LogWarning("Client instance already exists, destroying object!");
                Destroy(this);
            }
        }

        private void Start()
        {
            tcp = new TCP();
            udp = new UDP();
        }

        public void ConnectToServer()
        {
            InitClientData();

            tcp.Connect();
        }

        private void InitClientData()
        {
            packetHandlers = new Dictionary<byte, PacketHandler>()
            {
                { (byte)ServerPackets.welcome, ClientHandle.Welcome },
                { (byte)ServerPackets.synchronise, ClientHandle.Synchronise },
            };
        }

        public class TCP
        {
            public TcpClient socket;
            private NetworkStream stream;
            private Packet recievedData;
            private byte[] recieveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                recieveBuffer = new byte[dataBufferSize];

                socket.BeginConnect(instance.serverIp, instance.serverPort, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                stream = socket.GetStream();

                recievedData = new Packet();

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
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
                    Debug.LogWarning($"Error sendign data to server via TCP: {_ex}");
                }
            }

            private void RecieveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        Debug.LogWarning("Error recieving TCP packet. Byte length smaller or equal to 0.");
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
                    Debug.LogWarning("Error recieving TCP packet.");
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
                            byte _packetId = _packet.ReadByte();
                            packetHandlers[_packetId](_packet);
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

        public class UDP
        {
            public UdpClient socket;
            public IPEndPoint endPoint;

            public UDP()
            {
                endPoint = new IPEndPoint(IPAddress.Parse(instance.serverIp), instance.serverPort);
            }

            public void Connect(int _localPort)
            {
                socket = new UdpClient(_localPort);
                socket.Connect(endPoint);
                socket.BeginReceive(RecieveCallback, null);

                using (Packet _packet = new Packet())
                {
                    SendData(_packet);
                }
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    _packet.InsertInt(instance.myId);
                    if (socket != null)
                    {
                        socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Debug.LogWarning($"Error sending data to the server via UDP: {_ex}");
                }
            }

            private void RecieveCallback(IAsyncResult _result)
            {
                try
                {
                    byte[] _data = socket.EndReceive(_result, ref endPoint);
                    socket.BeginReceive(RecieveCallback, null);

                    if (_data.Length < 4)
                    {
                        // Something went wrong while transporting the packet. For now, it will be ignored.
                        return;
                    }
                    else
                    {
                        HandleData(_data);
                    }
                }
                catch
                {
                    // TODO: disconnect
                }
            }

            private void HandleData(byte[] _data)
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetLength = _packet.ReadInt();
                    _data = _packet.ReadBytes(_packetLength);
                }

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_data))
                    {
                        byte _packetId = _packet.ReadByte();
                        packetHandlers[_packetId](_packet);
                    }
                });
            }
        }
    }
}
