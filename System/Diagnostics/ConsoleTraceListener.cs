using System;

namespace System.Diagnostics
{
	/// <summary>Directs tracing or debugging output to either the standard output or the standard error stream.</summary>
	/// <filterpriority>2</filterpriority>
	public class ConsoleTraceListener : TextWriterTraceListener
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.ConsoleTraceListener" /> class with trace output written to the standard output stream.</summary>
		public ConsoleTraceListener() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.ConsoleTraceListener" /> class with an option to write trace output to the standard output stream or the standard error stream.</summary>
		/// <param name="useErrorStream">true to write tracing and debugging output to the standard error stream; false to write tracing and debugging output to the standard output stream.</param>
		public ConsoleTraceListener(bool useErrorStream) : base((!useErrorStream) ? Console.Out : Console.Error)
		{
		}

		internal ConsoleTraceListener(string data) : this(Convert.ToBoolean(data))
		{
		}
	}
}
