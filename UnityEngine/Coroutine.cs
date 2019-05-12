using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>MonoBehaviour.StartCoroutine returns a Coroutine. Instances of this class are only used to reference these coroutines and do not hold any exposed properties or functions.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Coroutine : YieldInstruction
	{
		internal IntPtr m_Ptr;

		private Coroutine()
		{
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseCoroutine();

		~Coroutine()
		{
			this.ReleaseCoroutine();
		}
	}
}
