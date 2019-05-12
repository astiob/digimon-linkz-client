using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Asynchronous operation coroutine.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class AsyncOperation : YieldInstruction
	{
		internal IntPtr m_Ptr;

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalDestroy();

		~AsyncOperation()
		{
			this.InternalDestroy();
		}

		/// <summary>
		///   <para>Has the operation finished? (Read Only)</para>
		/// </summary>
		public extern bool isDone { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>What's the operation's progress. (Read Only)</para>
		/// </summary>
		public extern float progress { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Priority lets you tweak in which order async operation calls will be performed.</para>
		/// </summary>
		public extern int priority { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Allow scenes to be activated as soon as it is ready.</para>
		/// </summary>
		public extern bool allowSceneActivation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
