using System;
using System.Collections.Generic;
using System.Text;

namespace SharedClassLibrary.Simulation
{
    class WorldState
    {
        private readonly TickStateMemeory tickStateMemeory;

        public WorldState(ComponentManager[] _componentManagers)
        {
            WorldTickState startTickState = new WorldTickState(_componentManagers);
            tickStateMemeory = new TickStateMemeory(startTickState);
        }

        internal bool RollBackOneTick()
        {
            return tickStateMemeory.RollBackOneTick();
        }

        internal ref ComponentType GetComponent<ComponentType>()
        {
        }

        class TickStateMemeory
        {
            private int newestIndex = 0;
            private int latestIndex = 0;
            private int currentIndex = 0;
            private WorldTickState[] tickStates = new WorldTickState[30];
            public WorldTickState CurrentTickState { get { return tickStates[currentIndex]; } private set { tickStates[currentIndex] = value; } }

            public TickStateMemeory(WorldTickState _startTickState)
            {
                CurrentTickState = _startTickState;
            }

            public bool RollBackOneTick()
            {
                if (currentIndex == latestIndex)
                {
                    return false;
                }
                currentIndex--;
                return true;
            }
            public void RollForwardOneTick()
            {
                RollForwardOneTick(tickStates[currentIndex].NextTickState());
            }
            private void RollForwardOneTick(WorldTickState _worldTickState)
            {
                currentIndex++;
                if (currentIndex == newestIndex + 1)
                {
                    newestIndex++;
                }
                if (latestIndex == newestIndex)
                {
                    latestIndex++;
                }
                CurrentTickState = _worldTickState;
            }
        }
    }
}
