using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation;
using SharedClassLibrary.Simulation.NetworkObjects;

namespace GameServer
{
    class Client
    {
        private delegate void PacketHandler(Client _fromClient, Packet _packet);
        private static Dictionary<byte, PacketHandler> packetHandlers = new Dictionary<byte, PacketHandler>()
        {
            { (byte) ClientPacketIds.pingRespond, ServerHandle.PingRespond},
            { (byte) ClientPacketIds.welcomeReceived, ServerHandle.WelcomeRecieved},
            { (byte) ClientPacketIds.udpTestRecieve, ServerHandle.UDPTestRecieved},
            { (byte) ClientPacketIds.buttonDown, ServerHandle.ButtonDown},
            { (byte) ClientPacketIds.buttonUp, ServerHandle.ButtonUp},
        };

        public readonly byte id;
        public readonly TCP tcp;
        public readonly UDP udp;
        public PlayerController playerController;
        public byte ping;

        public Client(byte _id)
        {
            id = _id;
            tcp = new TCP(this);
            udp = new UDP(this);
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
        private void CreatePlayer()
        {
            playerController = new PlayerController();
            new Player(playerController, new Vector2(0, 10));
            Console.WriteLine("Player object created.");
        }

        public void HandlePacket(Packet _packet)
        {
            byte _packetId = _packet.ReadByte();
            packetHandlers[_packetId](this, _packet);
        }

        public void Welcome()
        {
            Console.WriteLine($"Sending welcome packet to client id: {id}");
            using (Packet _packet = new Packet((byte)ServerPacketIds.welcome))
            {
                _packet.WriteByte(id);
                _packet.WriteString("Welcome to the server");

                tcp.Send(_packet);
            }
        }

        private static void SendTCPToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                ServerManager.clients[i].tcp.Send(_packet, false);
            }
        }

        private static void SendUDPToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                ServerManager.clients[i].udp.Send(_packet, false);
            }
        }
    }
}