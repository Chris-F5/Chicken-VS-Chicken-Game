﻿using System;
using System.Windows.Forms;
using System.Threading;
using SharedClassLibrary;
using SharedClassLibrary.Simulation;
using SharedClassLibrary.Simulation.ObjectTemplates;

namespace GameServer
{
    public static class Program
    {
        private static bool gameThreadRunning = false;
        public delegate void FormReadyCallback();

        static void Main()
        {
            ServerManager.StartServer(25680, 5);

            StartFormThread();
            StartGameThread();

            new Wall(new Vector2(-10, -10), new Vector2(20, 6)).CreateObjectInstance();
            new Wall(new Vector2(-4, -2.5f), new Vector2(3, 0.5f)).CreateObjectInstance();
        }
        public static void StartGameThread()
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
                    GameLogic.Update();

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