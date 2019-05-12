using System;

namespace UnityEngine.Events
{
	/// <summary>
	///   <para>Controls the scope of UnityEvent callbacks.</para>
	/// </summary>
	public enum UnityEventCallState
	{
		/// <summary>
		///   <para>Callback is not issued.</para>
		/// </summary>
		Off,
		/// <summary>
		///   <para>Callback is always issued.</para>
		/// </summary>
		EditorAndRuntime,
		/// <summary>
		///   <para>Callback is only issued in the Runtime and Editor playmode.</para>
		/// </summary>
		RuntimeOnly
	}
}
