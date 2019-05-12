using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Provides a base class for Win32 critical handle implementations in which the value of either 0 or -1 indicates an invalid handle.</summary>
	public abstract class CriticalHandleZeroOrMinusOneIsInvalid : CriticalHandle, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.CriticalHandleZeroOrMinusOneIsInvalid" /> class. </summary>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected CriticalHandleZeroOrMinusOneIsInvalid() : base((IntPtr)(-1))
		{
		}

		/// <summary>Gets a value that indicates whether the handle is invalid.</summary>
		/// <returns>true if the handle is not valid; otherwise, false.</returns>
		public override bool IsInvalid
		{
			get
			{
				return this.handle == (IntPtr)(-1) || this.handle == IntPtr.Zero;
			}
		}
	}
}
