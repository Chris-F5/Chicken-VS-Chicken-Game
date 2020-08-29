using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    public class PlayerController
    {
        private static Dictionary<byte, PlayerController> controllers = new Dictionary<byte, PlayerController>();
        private static int newestTick = -1;

        private List<InputState> inputStates = new List<InputState>();
        private InputState pendingState;
        private byte id;

        internal InputState inputState 
        { 
            get 
            {
                return GetState(GameLogic.Instance.GameTick).Value;
            } 
        }

        public static PlayerController GetPlayerController(byte _id)
        {
            if (controllers.ContainsKey(_id))
            {
                return controllers[_id];
            }
            else
            {
                return null;
            }
        }

        public PlayerController(byte _playerId)
        {
            id = _playerId;

            controllers.Add(id, this);
            pendingState = new InputState();
            // TODO: Set inputStates capacity
            // inputStates.Capacity = 
        }

        public void Dispose()
        {
            controllers.Remove(id);
        }

        internal static void UpdateAllToNewTick()
        {
            foreach (PlayerController controller in controllers.Values)
            {
                controller.UpdateToNewTick();
            }
            newestTick += 1;
        }

        public InputState? GetState(int _tick)
        {
            int index = inputStates.Count - 1 - (newestTick - _tick);
            if (index < 0 || index >= inputStates.Count)
            {
                return inputStates[index];
            }
            else
            {
                throw null;
            }
        }

        private void UpdateToNewTick()
        {
            inputStates.Add(pendingState);
            pendingState = pendingState.predictNextState();
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
                if (inputStates[index] != _state) {
                    inputStates[index] = _state;

                    GameLogic.Instance.RequestRollBack(_tick);
                }
            }
        }
    }
    public struct InputState
    {
        public bool rightKey;
        public bool leftKey;
        public bool upKey;
        public bool downKey;

        public InputState predictNextState()
        {
            InputState newState = new InputState();
            newState.rightKey = rightKey;
            newState.leftKey = leftKey;
            newState.upKey = upKey;
            newState.downKey = downKey;
            return newState;
        }

        public static bool operator ==(InputState first, InputState second)
        {
            return Equals(first, second);
        }
        public static bool operator !=(InputState first, InputState second)
        {
            return !(first == second);
        }
    }
}
