using System;

namespace System
{
	/// <summary>Specifies combinations of modifier and console keys that can interrupt the current process.</summary>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	public enum ConsoleSpecialKey
	{
		/// <summary>The <see cref="F:System.ConsoleModifiers.Control" /> modifier key plus the <see cref="F:System.ConsoleKey.C" /> console key.</summary>
		ControlC,
		/// <summary>The <see cref="F:System.ConsoleModifiers.Control" /> modifier key plus the BREAK console key.</summary>
		ControlBreak
	}
}
