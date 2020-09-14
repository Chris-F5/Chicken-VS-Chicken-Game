using System;
using System.Collections.Generic;
using System.Threading;
using SharedClassLibrary.Logging;
using SharedClassLibrary.ECS;
using SharedClassLibrary.GameLogic.Systems;
using SharedClassLibrary.GameLogic.Components;

namespace SharedClassLibrary.GameLogic
{
    public class GameLogic
    {
        public static GameLogic Instance { get; private set; }

        static GameLogic()
        {
            Instance = new GameLogic();
        }

        private int pendingRollback = 0;

        private Action afterTickUpdate;
        private Action beforeTickUpdate;

        private List<GameSystem> gameLogicSystems = new List<GameSystem>();

        public int rollbackLimit = 0;

        private bool gameThreadRunning = false;
        public int GameTick { get; private set; } = 0;
        public DateTime startTime { get; private set; }
        public World world { get; set; }

        public void StartGameLogicThread(Action _afterTickUpdate, Action _beforeTickUpdate, Action _worldInit)
        {
            StartGameLogicThread(_afterTickUpdate, _beforeTickUpdate, _worldInit, DateTime.Now);
        }
        public void StartGameLogicThread(Action _afterTickUpdate, Action _beforeTickUpdate, Action _worldInit, DateTime _startTime)
        {
            startTime = _startTime;

            afterTickUpdate = _afterTickUpdate;
            beforeTickUpdate = _beforeTickUpdate;

            InitWorld();
            _worldInit();

            if (!gameThreadRunning) 
            {
                gameThreadRunning = true;
                Thread _mainThread = new Thread(new ThreadStart(() => { GameThread(_startTime); }));
                _mainThread.Start();
            }
            else
            {
                throw new Exception("GameLogic thread is already running");
            }
        }

        private void InitWorld()
        {
            world = new World();

            gameLogicSystems.Add(new ApplyVelocitySystem(world));

            EntityHandler entity = world.CreateEntity();
            entity.AddComponent(new TransformComponent(-500, 400));
            entity.AddComponent(new VelocityComponent(150f, -75f));
            entity.AddComponent(new SpriteComponent());

            entity = world.CreateEntity();
            entity.AddComponent(new TransformComponent(500, 400));
            entity.AddComponent(new VelocityComponent(-150f, -95f));
            entity.AddComponent(new SpriteComponent());
        }

        private void GameThread(DateTime _startTime)
        {
            gameThreadRunning = true;
            Logger.LogDebug($"Game thread started. Running at {Constants.TICKS_PER_SECOND} ticks per second");
            DateTime _nextLoop = _startTime;

            while (gameThreadRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    MainLoop();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        // TODO: the following line occasionally throws error
                        // System.ArgumentOutOfRangeException: 'Number must be either non-negative and less than or equal to Int32.MaxValue or -1. Parameter name: timeout'

                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }

        private void MainLoop()
        {
            beforeTickUpdate();

            UpdateToNewTick();

            afterTickUpdate();
        }

        private void UpdateToNewTick()
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
            foreach (GameSystem system in gameLogicSystems)
            {
                system.Update();
            }
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
            // TODO: Rollback _tick number of ticks
            for (int i = 0; i < _ticks; i++)
            {
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
