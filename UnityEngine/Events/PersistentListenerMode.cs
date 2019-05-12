using System;

namespace UnityEngine.Events
{
	/// <summary>
	///   <para>THe mode that a listener is operating in.</para>
	/// </summary>
	[Serializable]
	public enum PersistentListenerMode
	{
		/// <summary>
		///   <para>The listener will use the function binding specified by the even.</para>
		/// </summary>
		EventDefined,
		/// <summary>
		///   <para>The listener will bind to zero argument functions.</para>
		/// </summary>
		Void,
		/// <summary>
		///   <para>The listener will bind to one argument Object functions.</para>
		/// </summary>
		Object,
		/// <summary>
		///   <para>The listener will bind to one argument int functions.</para>
		/// </summary>
		Int,
		/// <summary>
		///   <para>The listener will bind to one argument float functions.</para>
		/// </summary>
		Float,
		/// <summary>
		///   <para>The listener will bind to one argument string functions.</para>
		/// </summary>
		String,
		/// <summary>
		///   <para>The listener will bind to one argument bool functions.</para>
		/// </summary>
		Bool
	}
}
