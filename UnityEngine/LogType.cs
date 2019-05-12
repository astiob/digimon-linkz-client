using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The type of the log message in the delegate registered with Application.RegisterLogCallback.</para>
	/// </summary>
	public enum LogType
	{
		/// <summary>
		///   <para>LogType used for Errors.</para>
		/// </summary>
		Error,
		/// <summary>
		///   <para>LogType used for Asserts. (These could also indicate an error inside Unity itself.)</para>
		/// </summary>
		Assert,
		/// <summary>
		///   <para>LogType used for Warnings.</para>
		/// </summary>
		Warning,
		/// <summary>
		///   <para>LogType used for regular log messages.</para>
		/// </summary>
		Log,
		/// <summary>
		///   <para>LogType used for Exceptions.</para>
		/// </summary>
		Exception
	}
}
