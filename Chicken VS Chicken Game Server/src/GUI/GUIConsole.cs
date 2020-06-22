using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace GameServer.GUI
{
    public static class GUIConsole
    {
        private static RichTextBox consoleControl;
        private static List<ConsoleCommands.BaseConsoleCommand> commands;
        public static void Init(RichTextBox _consoleControl)
        {
            consoleControl = _consoleControl;
            PopulateCommandList();
        }
        private static void PopulateCommandList()
        {
            // Gets list of objects that inherit from BaseConsoleCommand class
            List<ConsoleCommands.BaseConsoleCommand> _commands = new List<ConsoleCommands.BaseConsoleCommand>();
            
            object[] _constructorArgs = new object[] { };

            foreach (Type _type in
                Assembly.GetAssembly(typeof(ConsoleCommands.BaseConsoleCommand)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ConsoleCommands.BaseConsoleCommand))))
            {
                _commands.Add((ConsoleCommands.BaseConsoleCommand)Activator.CreateInstance(_type,null));
            }
            commands = _commands;
            
        }
        public static void ProcessCommand(string _text)
        {
            _text = _text.ToLower();
            string _commandWord = _text.Split(' ')[0];
            string _peramsString = _text.Replace(_commandWord, "").Trim();
            foreach (ConsoleCommands.BaseConsoleCommand _command in commands)
            {
                if (_command.commandWord.ToLower() == _commandWord)
                {
                    WriteLine($"/{_text}");
                    string _out = _command.Process(_peramsString);
                    WriteLine(_out);
                    return;
                }
            }

            WriteLine($"Could not find command: \"{_commandWord}\"");
        }
        public static void WriteLine(string _value)
        {
            if (consoleControl != null) {
                // TODO: color text accordingly. E.g if contains "Error", make this line red.
                consoleControl.Invoke(
                    (Action)delegate
                    {
                        consoleControl.Text += _value + "\n";
                        consoleControl.ScrollToCaret();
                    }
                );
            }
        }
    }
}
