using System;
using System.Collections.Generic;
using SharedClassLibrary.Simulation;
using SharedClassLibrary.Networking;
using UnityEngine;

namespace GameClient
{
    class GameLogicManager
    {
        public const int extraInputsSend = 1;

        private GameLogicManager() { }

        private PlayerController playerController;

        public static GameLogicManager instance;
        public static GameLogicManager Instance 
        { 
            get 
            {
                if (instance == null)
                {
                    instance = new GameLogicManager();
                }
                return instance;
            }
        }

        public void StartGameLogic(DateTime _gameStartTime)
        {
            GameLogic.Instance.StartGameLogicThread(BeforeGameTick, AfterGameTick, _gameStartTime);
        }

        private void BeforeGameTick()
        {
            if (playerController == null) {
                playerController = PlayerController.GetPlayerController(NetworkManager.instance.myId);
            }
            playerController.SetState(InputManager.instance.inputState);

            Dictionary<int, InputState> clientInputStates = new Dictionary<int, InputState>();
            for (int tick = GameLogic.Instance.GameTick - 1; tick >= GameLogic.Instance.GameTick - 1 - extraInputsSend; tick--)
            {
                InputState? input = playerController.GetState(tick);
                if (input != null)
                {
                    clientInputStates.Add(tick, input.Value);
                }
            }
            if (clientInputStates.Count > 0)
            {
                PacketWriter packetWriter = new InputPacketWriter(clientInputStates);
                NetworkManager.instance.SendTcp(packetWriter);
                Debug.Log("Sent Input");
            }
        }

        private void AfterGameTick()
        {
        }
    }
}
