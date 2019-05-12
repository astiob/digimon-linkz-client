using System;

namespace System.Diagnostics
{
	internal class TraceSourceInfo
	{
		private string name;

		private SourceLevels levels;

		private TraceListenerCollection listeners;

		public TraceSourceInfo(string name, SourceLevels levels)
		{
			this.name = name;
			this.levels = levels;
			this.listeners = new TraceListenerCollection();
		}

		internal TraceSourceInfo(string name, SourceLevels levels, TraceImplSettings settings)
		{
			this.name = name;
			this.levels = levels;
			this.listeners = new TraceListenerCollection(false);
			this.listeners.Add(new DefaultTraceListener(), settings);
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public SourceLevels Levels
		{
			get
			{
				return this.levels;
			}
		}

		public TraceListenerCollection Listeners
		{
			get
			{
				return this.listeners;
			}
		}
	}
}
