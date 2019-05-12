using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Types of modifier key that can be active during a keystroke event.</para>
	/// </summary>
	[Flags]
	public enum EventModifiers
	{
		/// <summary>
		///   <para>No modifier key pressed during a keystroke event.</para>
		/// </summary>
		None = 0,
		/// <summary>
		///   <para>Shift key.</para>
		/// </summary>
		Shift = 1,
		/// <summary>
		///   <para>Control key.</para>
		/// </summary>
		Control = 2,
		/// <summary>
		///   <para>Alt key.</para>
		/// </summary>
		Alt = 4,
		/// <summary>
		///   <para>Command key (Mac).</para>
		/// </summary>
		Command = 8,
		/// <summary>
		///   <para>Num lock key.</para>
		/// </summary>
		Numeric = 16,
		/// <summary>
		///   <para>Caps lock key.</para>
		/// </summary>
		CapsLock = 32,
		/// <summary>
		///   <para>Function key.</para>
		/// </summary>
		FunctionKey = 64
	}
}
