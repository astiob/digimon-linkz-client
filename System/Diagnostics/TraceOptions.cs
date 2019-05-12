using System;

namespace System.Diagnostics
{
	/// <summary>Specifies trace data options to be written to the trace output.</summary>
	/// <filterpriority>2</filterpriority>
	[Flags]
	public enum TraceOptions
	{
		/// <summary>Do not write any elements.</summary>
		None = 0,
		/// <summary>Write the logical operation stack, which is represented by the return value of the <see cref="P:System.Diagnostics.CorrelationManager.LogicalOperationStack" /> property.</summary>
		LogicalOperationStack = 1,
		/// <summary>Write the date and time. </summary>
		DateTime = 2,
		/// <summary>Write the timestamp, which is represented by the return value of the <see cref="M:System.Diagnostics.Stopwatch.GetTimestamp" /> method.</summary>
		Timestamp = 4,
		/// <summary>Write the process identity, which is represented by the return value of the <see cref="P:System.Diagnostics.Process.Id" /> property.</summary>
		ProcessId = 8,
		/// <summary>Write the thread identity, which is represented by the return value of the <see cref="P:System.Threading.Thread.ManagedThreadId" /> property for the current thread.</summary>
		ThreadId = 16,
		/// <summary>Write the call stack, which is represented by the return value of the <see cref="P:System.Environment.StackTrace" /> property.</summary>
		Callstack = 32
	}
}
