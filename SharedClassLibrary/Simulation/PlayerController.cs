using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    public class PlayerController
    {
        private static List<PlayerController> controllers = new List<PlayerController>();
        private static int newestTick = -1;

        private List<InputState> inputStates = new List<InputState>();
        private InputState pendingState;


        internal InputState inputState 
        { 
            get 
            {
                return inputStates[inputStates.Count - 1 - (newestTick - GameLogic.Instance.GameTick)];
            } 
        }

        public PlayerController()
        {
            controllers.Add(this);
            pendingState = new InputState();
            // TODO: Set inputStates capacity
            // inputStates.Capacity = 
        }

        public void Dispose()
        {
            controllers.Remove(this);
        }

        internal static void UpdateAllToNewTick()
        {
            foreach (PlayerController controller in controllers)
            {
                controller.UpdateToNewTick();
            }
            newestTick += 1;
        }

        private void UpdateToNewTick()
        {
            inputStates.Add(pendingState);
            pendingState = new InputState();
        }

        public void SetState(InputState _state)
        {
            SetState(GameLogic.Instance.GameTick, _state);
        }

        public void SetState(int _tick, InputState _state)
        {
            int index = inputStates.Count - 1 - (newestTick - _tick);
            if (index < 0)
            {
                throw new ArgumentException("cant set input state on this tick", "_tick");
            }
            else if (_tick == GameLogic.Instance.GameTick)
            {
                pendingState = _state;
            }
            else if (_tick > GameLogic.Instance.GameTick)
            {
                throw new ArgumentException("tick number can not be in future", "_tick");
            }
            else
            {
                inputStates[index] = _state;

                GameLogic.Instance.RequestRollBack(_tick);
            }
        }

        public struct InputState
        {
            public bool rightKey;
            public bool leftKey;
            public bool upKey;
        }
    }
}
