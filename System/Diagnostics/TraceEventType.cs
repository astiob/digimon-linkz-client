using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	/// <summary>Identifies the type of event that has caused the trace.</summary>
	/// <filterpriority>2</filterpriority>
	public enum TraceEventType
	{
		/// <summary>Fatal error or application crash.</summary>
		Critical = 1,
		/// <summary>Recoverable error.</summary>
		Error,
		/// <summary>Noncritical problem.</summary>
		Warning = 4,
		/// <summary>Informational message.</summary>
		Information = 8,
		/// <summary>Debugging trace.</summary>
		Verbose = 16,
		/// <summary>Starting of a logical operation.</summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
		Start = 256,
		/// <summary>Stopping of a logical operation.</summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
		Stop = 512,
		/// <summary>Suspension of a logical operation.</summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
		Suspend = 1024,
		/// <summary>Resumption of a logical operation.</summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
		Resume = 2048,
		/// <summary>Changing of correlation identity.</summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
		Transfer = 4096
	}
}
