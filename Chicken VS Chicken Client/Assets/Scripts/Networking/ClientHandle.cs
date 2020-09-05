using UnityEngine;
using System;
using System.Collections.Generic;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation;

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

            WelcomeRecievePacketWriter packetWriter = new WelcomeRecievePacketWriter(NetworkManager.instance.myId, "Welcome recieved");
            TCPConnection.SendData(packetWriter.GetGeneratedBytes());
        }

        public static void RecieveClientInputs(PacketReader _packet)
        {
            Dictionary<byte, Dictionary<int, InputState>> inputStates = new Dictionary<byte, Dictionary<int, InputState>>();
            while (_packet.unreadLength >= 4)
            {
                byte clientId = _packet.ReadByte();
                Dictionary<int, InputState> clientInputStates = new Dictionary<int, InputState>();
                while (true)
                {
                    int tickNumber = _packet.ReadInt();
                    if (tickNumber == int.MaxValue)
                    {
                        break;
                    }
                    InputState inputState = _packet.ReadInputState();
                    clientInputStates.Add(tickNumber, inputState);
                }
                inputStates.Add(clientId, clientInputStates);
            }

            foreach (KeyValuePair<byte, Dictionary<int, InputState>> clientInputs in inputStates)
            {
                byte clientId = clientInputs.Key;
                PlayerController playerController = PlayerController.GetPlayerController(clientId);
                foreach (KeyValuePair<int, InputState> inputState in clientInputs.Value)
                {
                    int tickNumber = inputState.Key;
                    InputState state = inputState.Value;
                    playerController.SetState(tickNumber, state);
                }
            }
        }
    }
}
