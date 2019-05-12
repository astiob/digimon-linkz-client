using System;
using System.Runtime.ConstrainedExecution;

namespace System.Runtime
{
	/// <summary>Specifies the garbage collection settings for the current process. </summary>
	public static class GCSettings
	{
		/// <summary>Gets a value indicating whether server garbage collection is enabled.</summary>
		/// <returns>true if server garbage collection is enabled; otherwise, false.</returns>
		[MonoTODO("Always returns false")]
		public static bool IsServerGC
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the current latency mode for garbage collection.</summary>
		/// <returns>One of the <see cref="T:System.Runtime.GCLatencyMode" /> values. </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="T:System.Runtime.GCLatencyMode" /> is set to an invalid value.</exception>
		[MonoTODO("Always returns GCLatencyMode.Interactive and ignores set (.NET 2.0 SP1 member)")]
		public static GCLatencyMode LatencyMode
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return GCLatencyMode.Interactive;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
			}
		}
	}
}
