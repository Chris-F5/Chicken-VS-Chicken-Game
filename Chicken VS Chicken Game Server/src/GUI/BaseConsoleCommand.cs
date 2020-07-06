using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GUI.ConsoleCommands
{
    abstract class BaseConsoleCommand
    {
        public string commandWord { get; protected set; }
        public string description { get; protected set; }
        public string peramDescription { get; protected set; }
        public string Process(string _permas)
        {
            if (_permas == "" || _permas == " ")
            {
                return Process(new string[0]);
            }
            string[] _peramsArray = _permas.Split(' ');
            return Process(_peramsArray);
        }
        protected abstract string Process(string[] _perams);
        protected string ThrowError(string _errReason)
        {
            return $"Error processing the command \"{commandWord}\". {_errReason}";
        }
    }
}
