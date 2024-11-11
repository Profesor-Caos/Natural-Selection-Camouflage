using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LogEventArgs : EventArgs
{
    public DateTime Timestamp { get; }
    public string Message { get; }

    public LogEventArgs(DateTime timestamp, string message)
    {
        Timestamp = timestamp;
        Message = message;
    }
}