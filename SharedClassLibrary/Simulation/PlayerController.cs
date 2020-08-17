using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    public class PlayerController
    {
        private List<InputState> inputStates = new List<InputState>();
        private InputState pendingState;

        private static List<PlayerController> controllers = new List<PlayerController>();

        public InputState inputState 
        { 
            get 
            {
                return inputStates[GetStateIndex(GameLogic.Instance.GameTick)];
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

        internal static void UpdateAll()
        {
            foreach (PlayerController controller in controllers)
            {
                controller.UpdateToNextTick();
            }
        }

        private void UpdateToNextTick()
        {
            inputStates.Add(pendingState);
            pendingState = new InputState();
        }

        private int GetStateIndex(int _tick)
        {
            return inputStates.Count - 1 - GameLogic.Instance.GameTick - _tick;
        }

        public void SetState(int _tick, InputState _state)
        {
            if (_tick == inputStates.Count)
            {
                pendingState = _state;
            }
            else if (_tick > inputStates.Count)
            {
                throw new ArgumentException("tick number can not be in future.", "_tick");
            }
            else
            {
                inputStates[GetStateIndex(_tick)] = _state;

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
