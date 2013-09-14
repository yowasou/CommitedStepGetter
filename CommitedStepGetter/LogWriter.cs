using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitedStepGetter
{
    public static class LogWriter
    {
        /// <summary>
        /// Windowsログを出力
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void WriteEventLog(string source,string message)
        {
            System.Diagnostics.EventLog elog = new System.Diagnostics.EventLog();
            elog.MachineName = ".";
            elog.Log = string.Empty;
            elog.Source = "Application";
            if (!System.Diagnostics.EventLog.SourceExists(elog.Source, elog.MachineName))
            {
                System.Diagnostics.EventSourceCreationData escd =
                    new System.Diagnostics.EventSourceCreationData(elog.Source, elog.Log);
                escd.MachineName = elog.MachineName;
                System.Diagnostics.EventLog.CreateEventSource(escd);
            }
            elog.WriteEntry(message);
        }
        public static void WriteEventLog(Exception ex)
        {
            WriteEventLog(ex.Source, ex.Message);
        }
    }
}
