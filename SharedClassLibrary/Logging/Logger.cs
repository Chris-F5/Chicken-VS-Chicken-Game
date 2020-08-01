using System;
using System.Diagnostics;
using System.IO;

namespace SharedClassLibrary.Logging
{
    public static class Logger
    {
        private static volatile LoggerStream warning;
        private static volatile LoggerStream defaultLogger;

        static Logger()
        {
            warning = new LoggerStream();
            defaultLogger = new LoggerStream();
        }

        public static LoggerStream Warning { get { return warning; } }
        public static LoggerStream DefaultLogger { get { return defaultLogger; } }

        public static void WriteLine(string _message)
        {
            defaultLogger.WriteLine(_message);
        }

        public class LoggerStream
        {
            private TextWriter writer;

            public LoggerStream()
            {
                writer = TextWriter.Null;
            }

            public void SetOut(TextWriter _writer)
            {
                writer = _writer;
            }

            public void WriteLine(string _message)
            {
                StackFrame _frame = new StackFrame(1, true);
                string _method = _frame.GetMethod().Name;
                string _fileName = _frame.GetFileName();
                int _lineNumber = _frame.GetFileLineNumber();
                string _stackInfo = $"{_fileName} {_method} {_lineNumber}";
                string _fullMessage = _stackInfo.PadRight(30) + _message;

                writer.WriteLine(_fullMessage);
            }
        }
    }
}
