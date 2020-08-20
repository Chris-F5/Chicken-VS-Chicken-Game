using GameServer.GUI.ConsoleCommands;
using SharedClassLibrary.Simulation;

namespace GameServer.src.GUI.ConsoleCommands
{
    class GetGameTick : BaseConsoleCommand
    {
        public GetGameTick()
        {
            commandWord = "get_game_tick";
            description = "Outputs the current game tick and the max game tick.";
            peramDescription = "()";
        }
        protected override string Process(string[] _perams)
        {
            if (_perams.Length != 0)
            {
                return ThrowError($"Expected 0 peramiters, found {_perams.Length}.");
            }

            return $"Current game tick: {GameLogic.Instance.GameTick}. Max game tick: {int.MaxValue}.";
        }
    }
}
