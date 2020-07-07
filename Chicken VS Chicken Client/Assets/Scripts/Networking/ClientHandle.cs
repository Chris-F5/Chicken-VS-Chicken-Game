using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;

namespace GameClient
{
    public class ClientHandle : MonoBehaviour
    {
        public static void RecievePing(Packet _packet)
        {
            byte _id = _packet.ReadByte();
            byte _myPing = _packet.ReadByte();
            ClientSend.PingRecieved(_id);
            //Debug.Log($"Ping (measured in game ticks) : {_myPing}");
        }
        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Debug.Log($"Welcome message form server: {_msg}");
            Client.instance.myId = _myId;

            ClientSend.WelcomeRecieved();

            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void Synchronise(Packet _packet)
        {
            while (_packet.UnreadLength() > 0)
            {
                short _synchroniserId = _packet.ReadShort();
                short _typeId = _packet.ReadShort();
                NetworkSynchroniser _synchroniser = SynchroniserManager.FindSynchroniser(_synchroniserId);
                if (_synchroniser == null)
                {
                    SynchroniserManager.instance.HandleNewSynchroniser(_synchroniserId, _typeId, _packet);
                }
                else
                {
                    _synchroniser.HandleSynchronise(_packet);
                }
            }
        }
    }
}
