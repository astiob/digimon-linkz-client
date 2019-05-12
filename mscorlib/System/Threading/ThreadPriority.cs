using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>Specifies the scheduling priority of a <see cref="T:System.Threading.Thread" />.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public enum ThreadPriority
	{
		/// <summary>The <see cref="T:System.Threading.Thread" /> can be scheduled after threads with any other priority.</summary>
		Lowest,
		/// <summary>The <see cref="T:System.Threading.Thread" /> can be scheduled after threads with Normal priority and before those with Lowest priority.</summary>
		BelowNormal,
		/// <summary>The <see cref="T:System.Threading.Thread" /> can be scheduled after threads with AboveNormal priority and before those with BelowNormal priority. Threads have Normal priority by default.</summary>
		Normal,
		/// <summary>The <see cref="T:System.Threading.Thread" /> can be scheduled after threads with Highest priority and before those with Normal priority.</summary>
		AboveNormal,
		/// <summary>The <see cref="T:System.Threading.Thread" /> can be scheduled before threads with any other priority.</summary>
		Highest
	}
}
