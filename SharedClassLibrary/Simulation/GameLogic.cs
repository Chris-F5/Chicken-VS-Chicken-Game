using System;

namespace SharedClassLibrary.Simulation
{
    public class GameLogic
    {
        public static GameLogic Instance { get; private set; }

        static GameLogic()
        {
            Instance = new GameLogic();
        }

        private int pendingRollback = 0;

        public int rollbackLimit = 0;
        public int GameTick { get; private set; } = 0;

        public void UpdateToNewTick()
        {
            // If a rollback is required
            if (pendingRollback < GameTick)
            {
                int numberOfTicksToRollBack = GameTick - pendingRollback;
                ResimulateTicks(numberOfTicksToRollBack);
            }

            PlayerController.UpdateAllToNewTick();
            UpdateToNextTick();
        }

        private void UpdateToNextTick()
        {
            Console.WriteLine("Next Tick");
            NetworkObject.UpdateAll();
            GameTick++;

            // By default no rollback is planned for next tick.
            pendingRollback = GameTick;
        }

        /// <summary>
        /// Resimulates the last "_ticks" number of ticks.
        /// </summary>
        /// <param name="_ticks">
        /// The number of ticks to resimulate.
        /// </param>
        private void ResimulateTicks(int _ticks)
        {
            if (_ticks > GameTick)
            {
                throw new ArgumentException("Cant roll back to before the beging of the game", "_ticks");
            }
            if (GameTick - _ticks < rollbackLimit)
            {
                throw new ArgumentException("Cant roll back this far");
            }

            GameTick -= _ticks;
            NetworkObject.RollBackTicks(_ticks);
            Console.WriteLine($"Resimulating {_ticks}");
            for (int i = 0; i < _ticks; i++)
            {
                Console.WriteLine($"X Pos : {NetworkObject.allNetworkObjects[2].GetComponent<Components.PositionComponent>().Position.x}");
                Console.WriteLine($"X Vel : {NetworkObject.allNetworkObjects[2].GetComponent<Components.DynamicPhysicsBehaviour>().Velocity.x}");
                UpdateToNextTick();
            }
        }

        /// <summary>
        /// Tells GameLogic that on the next tick, it must roll back to at least tick number "_tick".
        /// </summary>
        /// <param name="_tick">
        /// The tick number to roll back to.
        /// </param>
        public void RequestRollBack(int _tick)
        {
            // If not already rolling back past this tick
            if (_tick < pendingRollback)
            {
                if (_tick < rollbackLimit)
                {
                    throw new ArgumentException("Cant roll back this far", "_tick");
                }
                pendingRollback = _tick;
            }
        }
    }
}
