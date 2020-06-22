using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerSend
    {
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.WriteString(_msg);
                _packet.WriteInt(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        /// <summary>
        /// Sends all game objects to a given client as if the objects had just been created.
        /// This be used for situations where the client knows nothing about any of the game objects. E.g when a new client joins.
        /// </summary>
        public static void SendAllObjectsAsNew(int _toClient)
        {
            foreach (KeyValuePair<short, GameObject> _gameObject in GameObject.allObjects)
            {
                _gameObject.Value.SendNewObjectPacket(_toClient);
            }
        }

        public static void SendObjectUpdates()
        {
            using (Packet _packet = new Packet((int)ServerPackets.gameObjectUpdates))
            {
                lock (GameObject.allObjects) {
                    // TODO: sometimes i get an error where this cant run because the allObjects dictionary was modified. The lock aint working
                    foreach (KeyValuePair<short, GameObject> _gameObject in GameObject.allObjects)
                    {
                        _gameObject.Value.Update(_packet);
                    }
                }
                SendUDPToAll(_packet);
            }
        }

        public static void UDPTest(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.udpTest))
            {
                _packet.WriteString("UDP Test!");

                SendUDPData(_toClient, _packet);
            }
        }

        #region SentTo

        public static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            ServerManager.clients[_toClient].tcp.SendData(_packet);
        }

        public static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            ServerManager.clients[_toClient].udp.SendData(_packet);
        }

        public static void SendTCPToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                ServerManager.clients[i].tcp.SendData(_packet);
            }
        }
        public static void SendTCPToAll(Packet _packet, int _exceptClient)
        {
            _packet.WriteLength();
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                if (i != _exceptClient) {
                    ServerManager.clients[i].tcp.SendData(_packet);
                }
            }
        }

        public static void SendUDPToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                ServerManager.clients[i].udp.SendData(_packet);
            }
        }
        public static void SendUDPToAll(Packet _packet, int _exceptClient)
        {
            _packet.WriteLength();
            for (int i = 1; i <= ServerManager.maxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    ServerManager.clients[i].udp.SendData(_packet);
                }
            }
        }

        #endregion
    }
}
