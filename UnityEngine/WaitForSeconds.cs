using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Suspends the coroutine execution for the given amount of seconds.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class WaitForSeconds : YieldInstruction
	{
		internal float m_Seconds;

		/// <summary>
		///   <para>Creates a yield instruction to wait for a given number of seconds.</para>
		/// </summary>
		/// <param name="seconds"></param>
		public WaitForSeconds(float seconds)
		{
			this.m_Seconds = seconds;
		}
	}
}
