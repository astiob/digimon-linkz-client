using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes different levels of log information the network layer supports.</para>
	/// </summary>
	public enum NetworkLogLevel
	{
		/// <summary>
		///   <para>Only report errors, otherwise silent.</para>
		/// </summary>
		Off,
		/// <summary>
		///   <para>Report informational messages like connectivity events.</para>
		/// </summary>
		Informational,
		/// <summary>
		///   <para>Full debug level logging down to each individual message being reported.</para>
		/// </summary>
		Full = 3
	}
}
