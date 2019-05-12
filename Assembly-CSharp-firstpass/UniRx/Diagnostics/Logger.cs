using System;
using UnityEngine;

namespace UniRx.Diagnostics
{
	public class Logger
	{
		private static bool isInitialized;

		private static bool isDebugBuild;

		protected readonly Action<LogEntry> logPublisher;

		public Logger(string loggerName)
		{
			this.Name = loggerName;
			this.logPublisher = ObservableLogger.RegisterLogger(this);
		}

		public string Name { get; private set; }

		public virtual void Debug(object message, UnityEngine.Object context = null)
		{
			if (!Logger.isInitialized)
			{
				Logger.isInitialized = true;
				Logger.isDebugBuild = UnityEngine.Debug.isDebugBuild;
			}
			if (Logger.isDebugBuild)
			{
				Action<LogEntry> action = this.logPublisher;
				string message2 = (message == null) ? string.Empty : message.ToString();
				action(new LogEntry(this.Name, LogType.Log, DateTime.Now, message2, context, null, null, null));
			}
		}

		public virtual void DebugFormat(string format, params object[] args)
		{
			if (!Logger.isInitialized)
			{
				Logger.isInitialized = true;
				Logger.isDebugBuild = UnityEngine.Debug.isDebugBuild;
			}
			if (Logger.isDebugBuild)
			{
				Action<LogEntry> action = this.logPublisher;
				string message = (format == null) ? string.Empty : string.Format(format, args);
				action(new LogEntry(this.Name, LogType.Log, DateTime.Now, message, null, null, null, null));
			}
		}

		public virtual void Log(object message, UnityEngine.Object context = null)
		{
			Action<LogEntry> action = this.logPublisher;
			string message2 = (message == null) ? string.Empty : message.ToString();
			action(new LogEntry(this.Name, LogType.Log, DateTime.Now, message2, context, null, null, null));
		}

		public virtual void LogFormat(string format, params object[] args)
		{
			Action<LogEntry> action = this.logPublisher;
			string message = (format == null) ? string.Empty : string.Format(format, args);
			action(new LogEntry(this.Name, LogType.Log, DateTime.Now, message, null, null, null, null));
		}

		public virtual void Warning(object message, UnityEngine.Object context = null)
		{
			Action<LogEntry> action = this.logPublisher;
			string message2 = (message == null) ? string.Empty : message.ToString();
			action(new LogEntry(this.Name, LogType.Warning, DateTime.Now, message2, context, null, null, null));
		}

		public virtual void WarningFormat(string format, params object[] args)
		{
			Action<LogEntry> action = this.logPublisher;
			string message = (format == null) ? string.Empty : string.Format(format, args);
			action(new LogEntry(this.Name, LogType.Warning, DateTime.Now, message, null, null, null, null));
		}

		public virtual void Error(object message, UnityEngine.Object context = null)
		{
			Action<LogEntry> action = this.logPublisher;
			string message2 = (message == null) ? string.Empty : message.ToString();
			action(new LogEntry(this.Name, LogType.Error, DateTime.Now, message2, context, null, null, null));
		}

		public virtual void ErrorFormat(string format, params object[] args)
		{
			Action<LogEntry> action = this.logPublisher;
			string message = (format == null) ? string.Empty : string.Format(format, args);
			action(new LogEntry(this.Name, LogType.Error, DateTime.Now, message, null, null, null, null));
		}

		public virtual void Exception(Exception exception, UnityEngine.Object context = null)
		{
			Action<LogEntry> action = this.logPublisher;
			string message = (exception == null) ? string.Empty : exception.ToString();
			action(new LogEntry(this.Name, LogType.Exception, DateTime.Now, message, context, exception, null, null));
		}

		public virtual void Raw(LogEntry logEntry)
		{
			this.logPublisher(logEntry);
		}
	}
}
