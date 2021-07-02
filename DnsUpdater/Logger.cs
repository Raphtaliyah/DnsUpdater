using System;

namespace DnsUpdater
{
    public class Logger
    {
        private static Logger _logger;
        public static Logger Get() => _logger ??= new Logger();

        private Logger()
        {
        }

        public void Log(LogType type, string message, Exception ex = null)
        {
            Console.WriteLine(
                $"({DateTime.Now} | {type}) {message} {(ex == null ? string.Empty : $"\nException: {ex}")}");
        }
    }

    public enum LogType
    {
        Info = 0,
        Warning = 1,
        Error = 2,
    }
}