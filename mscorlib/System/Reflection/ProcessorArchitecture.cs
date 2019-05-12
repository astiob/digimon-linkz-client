using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Identifies the processor and bits-per-word of the platform targeted by an executable.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum ProcessorArchitecture
	{
		/// <summary>An unknown or unspecified combination of processor and bits-per-word.</summary>
		None,
		/// <summary>Neutral with respect to processor and bits-per-word.</summary>
		MSIL,
		/// <summary>A 32-bit Intel processor, either native or in the Windows on Windows (WOW) environment on a 64-bit platform.</summary>
		X86,
		/// <summary>A 64-bit Intel processor only.</summary>
		IA64,
		/// <summary>A 64-bit AMD processor only.</summary>
		Amd64
	}
}
