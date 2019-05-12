using System;

namespace System.Diagnostics
{
	/// <summary>Provides the base class for trace filter implementations.</summary>
	/// <filterpriority>2</filterpriority>
	public abstract class TraceFilter
	{
		/// <summary>When overridden in a derived class, determines whether the trace listener should trace the event.</summary>
		/// <returns>true to trace the specified event; otherwise, false. </returns>
		/// <param name="cache">The <see cref="T:System.Diagnostics.TraceEventCache" /> that contains information for the trace event.</param>
		/// <param name="source">The name of the source.</param>
		/// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A trace identifier number.</param>
		/// <param name="formatOrMessage">Either the format to use for writing an array of arguments specified by the <paramref name="args" /> parameter, or a message to write.</param>
		/// <param name="args">An array of argument objects.</param>
		/// <param name="data1">A trace data object.</param>
		/// <param name="data">An array of trace data objects.</param>
		/// <filterpriority>2</filterpriority>
		public abstract bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data);
	}
}
