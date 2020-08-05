using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Client
    {
        private delegate void PacketHandler(Client _fromClient, Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int) ClientPackets.pingRespond, ServerHandle.PingRespond},
            { (int) ClientPackets.welcomeReceived, ServerHandle.WelcomeRecieved},
            { (int) ClientPackets.udpTestRecieve, ServerHandle.UDPTestRecieved},
            { (int) ClientPackets.buttonDown, ServerHandle.ButtonDown},
            { (int) ClientPackets.buttonUp, ServerHandle.ButtonUp},
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
            Console.WriteLine("Player object created.");
            playerController = new PlayerController();
        }

        private void SendPing(byte _id)
        {
            using (Packet _packet = new Packet(ServerPackets.ping))
            {
                _packet.WriteByte(_id);
                _packet.WriteByte(ping);
                udp.Send(_packet);
            }
        }

        public void HandlePacket(Packet _packet)
        {
            // TODO: change client packet id type to byte
            byte _packetId = _packet.ReadByte();
            packetHandlers[_packetId](this, _packet);
        }

        public void Welcome()
        {
            Console.WriteLine($"Sending welcome packet to client id: {id}");
            using (Packet _packet = new Packet(ServerPackets.welcome))
            {
                _packet.WriteByte(id);
                _packet.WriteString("Welcome to the server");

                tcp.Send(_packet);
            }
        }
        public void NetworkSynchroniserStartup()
        {
            Console.WriteLine($"Sending synchroniser startup packet to client id: {id}");
            using (Packet _packet = NetworkObject.GenerateStartupPacket())
            {
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

        public static void SynchroniseClients()
        {
            using (Packet _packet = NetworkObject.GenerateSynchronisationPacket())
            {
                SendUDPToAll(_packet);
            }
        }

        public static void PingAllClients(byte _id)
        {
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                ServerManager.clients[i].SendPing(_id);
            }
        }
    }
}