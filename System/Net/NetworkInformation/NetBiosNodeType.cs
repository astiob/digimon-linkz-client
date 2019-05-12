using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Specifies the Network Basic Input/Output System (NetBIOS) node type.</summary>
	public enum NetBiosNodeType
	{
		/// <summary>An unknown node type.</summary>
		Unknown,
		/// <summary>A broadcast node.</summary>
		Broadcast,
		/// <summary>A peer-to-peer node.</summary>
		Peer2Peer,
		/// <summary>A mixed node.</summary>
		Mixed = 4,
		/// <summary>A hybrid node.</summary>
		Hybrid = 8
	}
}
