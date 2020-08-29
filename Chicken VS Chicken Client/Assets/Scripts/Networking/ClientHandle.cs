using UnityEngine;
using System;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public class ClientHandle : MonoBehaviour
    {
        public static void RecieveWelcome(PacketReader _packet)
        {
            byte myId = _packet.ReadByte();
            long startTick = _packet.ReadLong();
            string msg = _packet.ReadString();

            Debug.Log($"Welcome message form server: {msg}");

            NetworkManager.instance.TcpConnectionConfirmed(myId);

            DateTime gameLogicStartTime = new DateTime(startTick);
            GameLogicManager.instance.StartGameLogic(gameLogicStartTime);

            ClientSend.WelcomeRecieved();
        }

        public static void RecieveClientInputs(PacketReader _packet)
        {
            
        }
    }
}
