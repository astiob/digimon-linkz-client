using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	/// <summary>Specifies the levels of trace messages filtered by the source switch and event type filter.</summary>
	/// <filterpriority>2</filterpriority>
	[Flags]
	public enum SourceLevels
	{
		/// <summary>Does not allow any events through.</summary>
		Off = 0,
		/// <summary>Allows only <see cref="F:System.Diagnostics.TraceEventType.Critical" /> events through.</summary>
		Critical = 1,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" /> and <see cref="F:System.Diagnostics.TraceEventType.Error" /> events through.</summary>
		Error = 3,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" />, <see cref="F:System.Diagnostics.TraceEventType.Error" />, and <see cref="F:System.Diagnostics.TraceEventType.Warning" /> events through.</summary>
		Warning = 7,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" />, <see cref="F:System.Diagnostics.TraceEventType.Error" />, <see cref="F:System.Diagnostics.TraceEventType.Warning" />, and <see cref="F:System.Diagnostics.TraceEventType.Information" /> events through.</summary>
		Information = 15,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" />, <see cref="F:System.Diagnostics.TraceEventType.Error" />, <see cref="F:System.Diagnostics.TraceEventType.Warning" />, <see cref="F:System.Diagnostics.TraceEventType.Information" />, and <see cref="F:System.Diagnostics.TraceEventType.Verbose" /> events through.</summary>
		Verbose = 31,
		/// <summary>Allows the <see cref="F:System.Diagnostics.TraceEventType.Stop" />, <see cref="F:System.Diagnostics.TraceEventType.Start" />, <see cref="F:System.Diagnostics.TraceEventType.Suspend" />, <see cref="F:System.Diagnostics.TraceEventType.Transfer" />, and <see cref="F:System.Diagnostics.TraceEventType.Resume" /> events through.</summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
		ActivityTracing = 65280,
		/// <summary>Allows all events through.</summary>
		All = -1
	}
}
