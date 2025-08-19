using System;
using System.IO;
using System.Text;

namespace WerkzeugMobil.Helpers
{
    public static class Logger
    {
        private static readonly string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static readonly string errorLog = Path.Combine(logDir, "error_log.txt");
        private static readonly string startupLog = Path.Combine(logDir, "startup.log");

        public static void LogError(string context, Exception ex)
        {
            Directory.CreateDirectory(logDir);

            var sb = new StringBuilder();
            sb.AppendLine($"[{DateTime.Now}] ERROR in {context}");
            sb.AppendLine(ex.ToString());

            File.AppendAllText(errorLog, sb.ToString() + Environment.NewLine);
        }

        public static void LogStartup(string message)
        {
            Directory.CreateDirectory(logDir);
            File.AppendAllText(startupLog, $"[{DateTime.Now}] {message}{Environment.NewLine}");
        }
    }
}