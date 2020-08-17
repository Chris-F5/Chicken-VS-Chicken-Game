using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation.Components
{
    public abstract class RollbackComponent : Component
    {
        private LinkedList<ComponentRollbackState> states = new LinkedList<ComponentRollbackState>();
        private int originTick;

        protected ComponentRollbackState state 
        { 
            get { return states.Last.Value; }
        }

        /// <param name="_rollbackLimit">
        /// Number of gameticks to store a state before discarding it.
        /// </param>
        public RollbackComponent(Component _nextComponent, ComponentRollbackState _state) : base(_nextComponent)
        {
            originTick = GameLogic.Instance.GameTick;
            states.AddLast(_state);
        }

        protected internal override void LateUpdate()
        {
            if (GameLogic.Instance.GameTick == originTick)
            {
                RollforwardPastCreation();
            }
            else if(GameLogic.Instance.GameTick > originTick)
            {
                ComponentRollbackState nextState = state.FinishChanges();
                states.AddLast(nextState);

                if (states.Count > GameLogic.Instance.GameTick - GameLogic.Instance.rollbackLimit)
                {
                    states.RemoveFirst();
                }
            }
            base.LateUpdate();
        }

        internal override void RollBackOneTick()
        {
            if (states.Count > 1) {
                states.RemoveLast();
            }
            else
            {
                RollbackOneTickPastCreation();
            }

            base.RollBackOneTick();
        }

        private protected virtual void RollbackOneTickPastCreation()
        {
            throw new Exception("Cant roll back past creation of component without overriding this function.");
        }
        private protected virtual void RollforwardPastCreation()
        {
            throw new Exception("Cant roll forward past creation of component without overriding this function.");
        }
    }
}
