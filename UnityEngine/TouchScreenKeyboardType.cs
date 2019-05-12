using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes the type of keyboard.</para>
	/// </summary>
	public enum TouchScreenKeyboardType
	{
		/// <summary>
		///   <para>Default keyboard for the current input method.</para>
		/// </summary>
		Default,
		/// <summary>
		///   <para>Keyboard displays standard ASCII characters.</para>
		/// </summary>
		ASCIICapable,
		/// <summary>
		///   <para>Keyboard with numbers and punctuation.</para>
		/// </summary>
		NumbersAndPunctuation,
		/// <summary>
		///   <para>Keyboard optimized for URL entry.</para>
		/// </summary>
		URL,
		/// <summary>
		///   <para>Numeric keypad designed for PIN entry.</para>
		/// </summary>
		NumberPad,
		/// <summary>
		///   <para>Keypad designed for entering telephone numbers.</para>
		/// </summary>
		PhonePad,
		/// <summary>
		///   <para>Keypad designed for entering a person's name or phone number.</para>
		/// </summary>
		NamePhonePad,
		/// <summary>
		///   <para>Keyboard optimized for specifying email addresses.</para>
		/// </summary>
		EmailAddress,
		/// <summary>
		///   <para>Keyboard designed for Nintendo Network Accounts (available on Wii U only).</para>
		/// </summary>
		NintendoNetworkAccount
	}
}
