using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ChatProgram
{
    public class Logger
    {
        public static void LogMsg(string message, LogSeverity severity = LogSeverity.Info)
        {
            var stackTrace = new StackTrace(1, true);
            string loc = null;
            foreach(var stackFrame in stackTrace.GetFrames())
            {
                if(!(stackFrame.GetFileName() ?? "Logger.cs").Contains("Logger.cs"))
                {
                    loc = stackFrame.GetFileName() + ":" + stackFrame.GetFileLineNumber();
                }
            }
            if (string.IsNullOrWhiteSpace(loc))
                loc = "N/A";
            var logM = new LogMessage()
            {
                Content = message,
                Location = loc,
                Severity = severity
            };
            LogMsg(logM);
        }

        static object LOCK = new object();
        public static void LogMsg(LogMessage msg)
        {
            lock(LOCK)
            {
                Console.WriteLine(msg.ToString());
                try
                {
                    File.AppendAllText($"{DateTime.Now.Year.ToString("0000")}-{DateTime.Now.Month.ToString("00")}-{DateTime.Now.Day.ToString("00")}.py", msg.ToString() + "\n");
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }

    public class LogMessage
    {
        public string Content { get; set; }
        public string Location { get; set; }
        public LogSeverity Severity { get; set; }
        public Exception Error { get; set; }

        public override string ToString()
        {
            string content;
            if (Error == null)
                content = Content;
            else
                content = Error.ToString();
            return $"{DateTime.Now.ToLongTimeString()}: {Severity} {Location} {content}";

            //    var guild = Program.TheGrandCodingGuild.GetTextChannel(id)
        }
    }

    public enum LogSeverity
    {
        Debug,
        Info,
        Warning,
        Error
    }
}
