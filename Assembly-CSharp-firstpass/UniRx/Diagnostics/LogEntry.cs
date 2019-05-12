using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UniRx.Diagnostics
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct LogEntry
	{
		public LogEntry(string loggerName, LogType logType, DateTime timestamp, string message, UnityEngine.Object context = null, Exception exception = null, string stackTrace = null, object state = null)
		{
			this = default(LogEntry);
			this.LoggerName = loggerName;
			this.LogType = logType;
			this.Timestamp = timestamp;
			this.Message = message;
			this.Context = context;
			this.Exception = exception;
			this.StackTrace = stackTrace;
			this.State = state;
		}

		public string LoggerName { get; private set; }

		public LogType LogType { get; private set; }

		public string Message { get; private set; }

		public DateTime Timestamp { get; private set; }

		public UnityEngine.Object Context { get; private set; }

		public Exception Exception { get; private set; }

		public string StackTrace { get; private set; }

		public object State { get; private set; }

		public override string ToString()
		{
			string text = (this.Exception == null) ? string.Empty : (Environment.NewLine + this.Exception.ToString());
			return string.Concat(new string[]
			{
				"[",
				this.Timestamp.ToString(),
				"][",
				this.LoggerName,
				"][",
				this.LogType.ToString(),
				"]",
				this.Message,
				text
			});
		}
	}
}
