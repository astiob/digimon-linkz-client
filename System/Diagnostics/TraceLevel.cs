using System;

namespace System.Diagnostics
{
	/// <summary>Specifies what messages to output for the <see cref="T:System.Diagnostics.Debug" />, <see cref="T:System.Diagnostics.Trace" /> and <see cref="T:System.Diagnostics.TraceSwitch" /> classes.</summary>
	/// <filterpriority>2</filterpriority>
	public enum TraceLevel
	{
		/// <summary>Output no tracing and debugging messages.</summary>
		Off,
		/// <summary>Output error-handling messages.</summary>
		Error,
		/// <summary>Output warnings and error-handling messages.</summary>
		Warning,
		/// <summary>Output informational messages, warnings, and error-handling messages.</summary>
		Info,
		/// <summary>Output all debugging and tracing messages.</summary>
		Verbose
	}
}
