using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Used by GUIUtility.GetControlID to inform the UnityGUI system if a given control can get keyboard focus.</para>
	/// </summary>
	public enum FocusType
	{
		/// <summary>
		///   <para>This control can get keyboard focus on Windows, but not on Mac. Used for buttons, checkboxes and other "pressable" things.</para>
		/// </summary>
		Native,
		/// <summary>
		///   <para>This is a proper keyboard control. It can have input focus on all platforms. Used for TextField and TextArea controls.</para>
		/// </summary>
		Keyboard,
		/// <summary>
		///   <para>This control can never recieve keyboard focus.</para>
		/// </summary>
		Passive
	}
}
