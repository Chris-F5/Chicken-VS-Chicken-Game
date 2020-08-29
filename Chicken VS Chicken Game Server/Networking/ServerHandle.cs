using System;
using System.Collections.Generic;
using SharedClassLibrary.Logging;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeRecieved(Client _fromClient, PacketReader _packet)
        {
            int clientIdCheck = _packet.ReadInt();
            string playername = _packet.ReadString();
            _fromClient.playerName = playername;

            Console.WriteLine($"\"{playername}\" connected successfully and is now player {_fromClient.id}");
            if (_fromClient.id != clientIdCheck)
            {
                Console.WriteLine($"Player \"{playername}\" (ID: {_fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }
        }

        public static void InputsRecieved(Client _fromClient, PacketReader _packet)
        {
            while (_packet.unreadLength >= 4)
            {
                int tickNumber = _packet.ReadInt();
                if (tickNumber <= GameLogic.Instance.GameTick && tickNumber >= GameLogic.Instance.rollbackLimit) {

                    InputState inputState = _packet.ReadInputState();

                    _fromClient.SetInputState(tickNumber, inputState);
                }
                else
                {
                    Logger.LogWarning("Recieved client input message with invalid tick number.");
                }
            }
        }
    }
}
