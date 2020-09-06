using System;
using System.Windows.Forms;
using System.Threading;
using SharedClassLibrary;
using SharedClassLibrary.Simulation;
using SharedClassLibrary.Simulation.NetworkObjects;
using SharedClassLibrary.Logging;

namespace GameServer
{
    public static class Program
    {
        public delegate void FormReadyCallback();

        static void Main()
        {
            ServerManager.StartServer(25680);

            StartFormThread();
            GameLogic.Instance.StartGameLogicThread(AfterTickUpdate, BeforeTickUpdate);

            new Wall(new Vector2(-10, -10), new Vector2(20, 6));
            new Wall(new Vector2(-4, -2.5f), new Vector2(3, 0.5f));
        }

        private static void BeforeTickUpdate()
        {
            ThreadManager.UpdateMain();
        }

        private static void AfterTickUpdate()
        {
            Client.ShareClientInputsToAllClients();
            while (Logger.DebugMessages.Count > 0)
            {
                Console.WriteLine(Logger.DebugMessages.Dequeue());
            }
            while (Logger.WarningMessages.Count > 0)
            {
                Console.WriteLine("WARNING: " + Logger.WarningMessages.Dequeue());
            }
        }

        [STAThread]
        private static void StartFormThread()
        {
            Thread _formThread = new Thread(new ThreadStart(FormThread));
            _formThread.Start();
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
