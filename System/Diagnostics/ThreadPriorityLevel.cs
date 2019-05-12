using System;

namespace System.Diagnostics
{
	/// <summary>Specifies the priority level of a thread.</summary>
	/// <filterpriority>2</filterpriority>
	public enum ThreadPriorityLevel
	{
		/// <summary>Specifies one step above the normal priority for the associated <see cref="T:System.Diagnostics.ProcessPriorityClass" />.</summary>
		AboveNormal = 1,
		/// <summary>Specifies one step below the normal priority for the associated <see cref="T:System.Diagnostics.ProcessPriorityClass" />.</summary>
		BelowNormal = -1,
		/// <summary>Specifies highest priority. This is two steps above the normal priority for the associated <see cref="T:System.Diagnostics.ProcessPriorityClass" />.</summary>
		Highest = 2,
		/// <summary>Specifies idle priority. This is the lowest possible priority value of all threads, independent of the value of the associated <see cref="T:System.Diagnostics.ProcessPriorityClass" />.</summary>
		Idle = -15,
		/// <summary>Specifies lowest priority. This is two steps below the normal priority for the associated <see cref="T:System.Diagnostics.ProcessPriorityClass" />.</summary>
		Lowest = -2,
		/// <summary>Specifies normal priority for the associated <see cref="T:System.Diagnostics.ProcessPriorityClass" />.</summary>
		Normal = 0,
		/// <summary>Specifies time-critical priority. This is the highest priority of all threads, independent of the value of the associated <see cref="T:System.Diagnostics.ProcessPriorityClass" />.</summary>
		TimeCritical = 15
	}
}
