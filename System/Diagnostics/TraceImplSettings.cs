using System;

namespace System.Diagnostics
{
	internal class TraceImplSettings
	{
		public const string Key = ".__TraceInfoSettingsKey__.";

		public bool AutoFlush;

		public int IndentLevel;

		public int IndentSize = 4;

		public TraceListenerCollection Listeners = new TraceListenerCollection(false);

		public TraceImplSettings()
		{
			this.Listeners.Add(new DefaultTraceListener(), this);
		}
	}
}
