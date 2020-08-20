using System;
using System.Windows.Forms;
using System.Threading;
using SharedClassLibrary;
using SharedClassLibrary.Simulation;
using SharedClassLibrary.Simulation.NetworkObjects;
using SharedClassLibrary.Simulation.Components;

namespace GameServer
{
    public static class Program
    {
        private static bool gameThreadRunning = false;
        public delegate void FormReadyCallback();

        static void Main()
        {
            //ServerManager.StartServer(25680, 5);

            StartFormThread();
            StartGameThread();

            new Wall(new Vector2(-10, -10), new Vector2(20, 6));
            new Wall(new Vector2(-4, -2.5f), new Vector2(3, 0.5f));
            PlayerController controller = new PlayerController();
            Player player = new Player(controller, new Vector2(0, 10));
            for (int i = 0; i < 300; i++)
            {
                GameLogic.Instance.UpdateToNewTick();
                Console.WriteLine($"Player position: ({player.GetComponent<PositionComponent>().Position.x}, {player.GetComponent<PositionComponent>().Position.y}) Game tick {GameLogic.Instance.GameTick}");
            }
            Console.WriteLine("--");
            for (int i = 0; i < 3; i++)
            {
                PlayerController.InputState inputState = new PlayerController.InputState();
                inputState.rightKey = true;
                controller.SetState(inputState);

                GameLogic.Instance.UpdateToNewTick();
                Console.WriteLine($"Player position: ({player.GetComponent<PositionComponent>().Position.x}, {player.GetComponent<PositionComponent>().Position.y}) Game tick {GameLogic.Instance.GameTick}");
            }
            for (int i = 0; i < 8; i++)
            {
                GameLogic.Instance.UpdateToNewTick();
                Console.WriteLine($"Player position: ({player.GetComponent<PositionComponent>().Position.x}, {player.GetComponent<PositionComponent>().Position.y}) Game tick {GameLogic.Instance.GameTick}");
            }
            /*controller.SetState(303, new PlayerController.InputState());
            controller.SetState(304, new PlayerController.InputState());
            controller.SetState(305, new PlayerController.InputState());
            controller.SetState(306, new PlayerController.InputState());
            controller.SetState(307, new PlayerController.InputState());
            controller.SetState(308, new PlayerController.InputState());
            controller.SetState(309, new PlayerController.InputState());
            controller.SetState(310, new PlayerController.InputState());
            GameLogic.Instance.UpdateToNewTick();
            Console.WriteLine($"Player position: ({player.GetComponent<PositionComponent>().Position.x}, {player.GetComponent<PositionComponent>().Position.y}) Game tick {GameLogic.Instance.GameTick}");*/
        }

        private static void MainLoop()
        {
            //ThreadManager.UpdateMain();
            //GameLogic.Instance.UpdateToNewTick();
            //Client.SynchroniseClients();
        }

        private static void StartGameThread()
        {
            gameThreadRunning = true;
            Thread _mainThread = new Thread(new ThreadStart(GameThread));
            _mainThread.Start();
        }
        [STAThread]
        private static void StartFormThread()
        {
            Thread _formThread = new Thread(new ThreadStart(FormThread));
            _formThread.Start();
        }
        private static void GameThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SECOND} ticks per second");
            DateTime _nextLoop = DateTime.Now;

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
        private static void FormThread()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI.Form1());

            // Form closed - exit everything else.
            Environment.Exit(1);
        }
    }
}
