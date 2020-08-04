using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SharedClassLibrary.Logging
{
    public static class Logger
    {
        public delegate void StringHandler(string _text);

        private static volatile Queue<string> warningMessages;
        private static volatile Queue<string> debugMessages;

        static Logger()
        {
            debugMessages = new Queue<string>();
            warningMessages = new Queue<string>();
        }

        public static Queue<string> DebugMessages { get { return debugMessages; } }
        public static Queue<string> WarningMessages { get { return warningMessages; } }

        public static void LogDebug(string _message)
        {
            // TODO: look into using string builder to optomise string concatenation
            DebugMessages.Enqueue(GeneratePostMessageText() + _message);
        }
        public static void LogWarning(string _message)
        {
            WarningMessages.Enqueue(GeneratePostMessageText() + _message);
        }

        private static string GeneratePostMessageText()
        {
            StackFrame _frame = new StackFrame(2, true);
            string _method = _frame.GetMethod().Name;
            string _fileName = Path.GetFileName(_frame.GetFileName());
            int _lineNumber = _frame.GetFileLineNumber();
            string _stackInfo = $"{_fileName} {_method} {_lineNumber}";
            string _dateTime = DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt");
            return _dateTime.PadRight(21) + _stackInfo.PadRight(55) + "  ";
        }
    }
}
