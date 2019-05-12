using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>An exception thrown by the PlayerPrefs class in a  web player build.</para>
	/// </summary>
	public sealed class PlayerPrefsException : Exception
	{
		public PlayerPrefsException(string error) : base(error)
		{
		}
	}
}
