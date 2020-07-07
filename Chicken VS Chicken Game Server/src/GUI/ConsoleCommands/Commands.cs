using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GUI.ConsoleCommands
{
    class Commands : BaseConsoleCommand
    {
        public Commands()
        {
            commandWord = "commands";
            description = "Displays a list of commands and their descriptions.";
            peramDescription = "()";
        }
        protected override string Process(string[] _perams)
        {
            if(_perams.Length != 0)
            {
                return ThrowError($"Expected 0 peramiters, found {_perams.Length}.");
            }

            string _output = "";
            foreach (BaseConsoleCommand _command in GUIConsole.commands)
            {
                _output += $"{_command.commandWord} - {_command.peramDescription} - {_command.description}";
                _output += "\n";
            }
            return _output;
        }
    }
}
