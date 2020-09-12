using System;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Logging;

namespace GameServer
{
    class UDP
    {
        private static UdpClient udpListener;

        public IPEndPoint endPoint;
        private readonly Client client;

        public UDP(Client _client)
        {
            client = _client;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
        }

        public void Send(byte[] _bytes)
        {
            if (endPoint != null)
            {
                udpListener.BeginSend(_bytes, _bytes.Length, endPoint, null, null);
            }
        }

        public void HandlePacket(PacketReader _packet)
        {
            ThreadManager.ExecuteOnMainThread(() =>
            {
                client.HandlePacket(_packet);
            });
        }

        public static void StartListening(int _port)
        {
            udpListener = new UdpClient(_port);
            udpListener.BeginReceive(UDP.RecieveCallback, null);
            Logger.LogDebug($"Started listening for UDP on port {_port}");
        }

        public static void RecieveCallback(IAsyncResult _result)
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(RecieveCallback, null);

            if (_data.Length < 4)
            {
                // Something went wrong while transporting the packet. For now, it will be ignored.
                Logger.LogWarning("Inbound UDP packet was too short for length to be read. Ignoring.");
                return;
            }
            else
            {
                // TODO: Some extra precautions need to be put in place here to prevent DOS attacks.

                PacketReader _packet = new PacketReader(_data);
                int packetLength = _packet.ReadInt();

                if (packetLength != _data.Length - sizeof(int))
                    Logger.LogWarning("Recieved packet with invalid length.");

                byte claimedClientId = _packet.ReadByte();

                if (claimedClientId <= 0 || claimedClientId > Client.maxPlayerCount)
                {
                    Logger.LogWarning($"A clients claimed id {claimedClientId} does not exist. Either they are using a broken client or something went very wrong.");
                }
                else {
                    Client claimedClient = Client.GetClient(claimedClientId);
                    if (claimedClient == null)
                    {
                        Logger.LogWarning("Client tryed to udp connect to a client id that has not been taken yet.");
                    }
                    if (!claimedClient.IsUDPConnected)
                    {
                        Logger.LogDebug("New UDP connection established");
                        claimedClient.UDPConnect(_clientEndPoint);
                    }
                    else if (claimedClient.VerifyEndPoint(_clientEndPoint))
                    {
                        claimedClient.HandleUdpPacket(_packet);
                    }
                    else
                    {
                        Logger.LogWarning("Client end point did not match with end point of claimed client.");
                    }
                }
            }
        }
    }
}