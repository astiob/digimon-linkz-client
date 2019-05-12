using System;

namespace System.Net.Sockets
{
	/// <summary>Specifies the type of protocol that an instance of the <see cref="T:System.Net.Sockets.Socket" /> class can use.</summary>
	public enum ProtocolFamily
	{
		/// <summary>Unknown protocol.</summary>
		Unknown = -1,
		/// <summary>Unspecified protocol.</summary>
		Unspecified,
		/// <summary>Unix local to host protocol.</summary>
		Unix,
		/// <summary>IP version 4 protocol.</summary>
		InterNetwork,
		/// <summary>ARPANET IMP protocol.</summary>
		ImpLink,
		/// <summary>PUP protocol.</summary>
		Pup,
		/// <summary>MIT CHAOS protocol.</summary>
		Chaos,
		/// <summary>IPX or SPX protocol.</summary>
		Ipx,
		/// <summary>ISO protocol.</summary>
		Iso,
		/// <summary>European Computer Manufacturers Association (ECMA) protocol.</summary>
		Ecma,
		/// <summary>DataKit protocol.</summary>
		DataKit,
		/// <summary>CCITT protocol, such as X.25.</summary>
		Ccitt,
		/// <summary>IBM SNA protocol.</summary>
		Sna,
		/// <summary>DECNet protocol.</summary>
		DecNet,
		/// <summary>Direct data link protocol.</summary>
		DataLink,
		/// <summary>LAT protocol.</summary>
		Lat,
		/// <summary>NSC HyperChannel protocol.</summary>
		HyperChannel,
		/// <summary>AppleTalk protocol.</summary>
		AppleTalk,
		/// <summary>NetBIOS protocol.</summary>
		NetBios,
		/// <summary>VoiceView protocol.</summary>
		VoiceView,
		/// <summary>FireFox protocol.</summary>
		FireFox,
		/// <summary>Banyan protocol.</summary>
		Banyan = 21,
		/// <summary>Native ATM services protocol.</summary>
		Atm,
		/// <summary>IP version 6 protocol.</summary>
		InterNetworkV6,
		/// <summary>Microsoft Cluster products protocol.</summary>
		Cluster,
		/// <summary>IEEE 1284.4 workgroup protocol.</summary>
		Ieee12844,
		/// <summary>IrDA protocol.</summary>
		Irda,
		/// <summary>Network Designers OSI gateway enabled protocol.</summary>
		NetworkDesigners = 28,
		/// <summary>MAX protocol.</summary>
		Max,
		/// <summary>Xerox NS protocol.</summary>
		NS = 6,
		/// <summary>OSI protocol.</summary>
		Osi
	}
}
