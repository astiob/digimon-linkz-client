using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Stack trace logging options.</para>
	/// </summary>
	public enum StackTraceLogType
	{
		/// <summary>
		///   <para>No stack trace will be outputed to log.</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>Only managed stack trace will be outputed.</para>
		/// </summary>
		ScriptOnly,
		/// <summary>
		///   <para>Native and managed stack trace will be logged.</para>
		/// </summary>
		Full
	}
}
