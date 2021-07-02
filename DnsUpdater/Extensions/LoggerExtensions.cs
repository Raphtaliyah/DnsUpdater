using System;

namespace DnsUpdater.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogInfo(this Logger logger, string message, Exception ex = null) => 
            logger.Log(LogType.Info, message, ex);

        public static void LogWarning(this Logger logger, string message, Exception ex = null) =>
            logger.Log(LogType.Warning, message, ex);

        public static void LogError(this Logger logger, string message, Exception ex = null) =>
            logger.Log(LogType.Error, message, ex);
    }
}