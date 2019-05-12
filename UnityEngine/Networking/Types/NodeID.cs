using System;
using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
	/// <summary>
	///   <para>The NodeID is the ID used in relay matches to track nodes in a network.</para>
	/// </summary>
	[DefaultValue(NodeID.Invalid)]
	public enum NodeID : ushort
	{
		/// <summary>
		///   <para>The invalid case of a NodeID.</para>
		/// </summary>
		Invalid
	}
}
