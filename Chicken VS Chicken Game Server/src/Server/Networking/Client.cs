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
            { (int) ClientPackets.welcomeReceived, ServerHandle.WelcomeRecieved},
            { (int) ClientPackets.udpTestRecieve, ServerHandle.UDPTestRecieved},
            { (int) ClientPackets.buttonDown, ServerHandle.ButtonDown},
            { (int) ClientPackets.buttonUp, ServerHandle.ButtonUp},
        };

        public readonly int id;
        public readonly TCP tcp;
        public readonly UDP udp;
        public GameObjects.Player playerObject;

        public Client(int _id)
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
            playerObject = new GameObjects.Player(new Vector2(0, 10));
        }
        public void Welcome()
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.WriteString("Welcome to the server");
                _packet.WriteInt(id);

                tcp.Send(_packet);
            }
        }
        /// <summary>
        /// Sends all game objects to a given client as if the objects had just been created.
        /// This be used for situations where the client knows nothing about any of the game objects. E.g when a new client joins.
        /// </summary>
        public void SendAllObjectsAsNew()
        {
            foreach (KeyValuePair<short, GameObject> _gameObjectKeyPair in GameObject.allObjects)
            {
                GameObject _gameObject = _gameObjectKeyPair.Value;
                _gameObject.SendNewObjectPacket(this);
            }
        }

        public void SendUDPTest()
        {
            using (Packet _packet = new Packet((int)ServerPackets.udpTest))
            {
                _packet.WriteString("UDP Test!");

                udp.Send(_packet);
            }
        }
        public void HandlePacket(Packet _packet)
        {
            int _packetId = _packet.ReadInt();
            packetHandlers[_packetId](this, _packet);
        }
        public static void SendTCPToAll(Packet _packet)
        {
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                ServerManager.clients[i].tcp.Send(_packet);
            }
        }

        private static void SendUDPToAll(Packet _packet)
        {
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                ServerManager.clients[i].udp.Send(_packet);
            }
        }

        public static void SendObjectUpdatesToAll()
        {
            using (Packet _packet = new Packet((int)ServerPackets.gameObjectUpdates))
            {
                lock (GameObject.allObjects)
                {
                    // TODO: sometimes i get an error where this cant run because the allObjects dictionary was modified. The lock aint working
                    foreach (KeyValuePair<short, GameObject> _gameObject in GameObject.allObjects)
                    {
                        _gameObject.Value.Update(_packet);
                    }
                }
                SendUDPToAll(_packet);
            }
        }
    }
}
