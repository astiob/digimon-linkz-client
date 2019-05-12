using System;

namespace System.Net.Sockets
{
	/// <summary>Specifies the addressing scheme that an instance of the <see cref="T:System.Net.Sockets.Socket" /> class can use.</summary>
	public enum AddressFamily
	{
		/// <summary>Unknown address family.</summary>
		Unknown = -1,
		/// <summary>Unspecified address family.</summary>
		Unspecified,
		/// <summary>Unix local to host address.</summary>
		Unix,
		/// <summary>Address for IP version 4.</summary>
		InterNetwork,
		/// <summary>ARPANET IMP address.</summary>
		ImpLink,
		/// <summary>Address for PUP protocols.</summary>
		Pup,
		/// <summary>Address for MIT CHAOS protocols.</summary>
		Chaos,
		/// <summary>Address for Xerox NS protocols.</summary>
		NS,
		/// <summary>IPX or SPX address.</summary>
		Ipx = 6,
		/// <summary>Address for ISO protocols.</summary>
		Iso,
		/// <summary>Address for OSI protocols.</summary>
		Osi = 7,
		/// <summary>European Computer Manufacturers Association (ECMA) address.</summary>
		Ecma,
		/// <summary>Address for Datakit protocols.</summary>
		DataKit,
		/// <summary>Addresses for CCITT protocols, such as X.25.</summary>
		Ccitt,
		/// <summary>IBM SNA address.</summary>
		Sna,
		/// <summary>DECnet address.</summary>
		DecNet,
		/// <summary>Direct data-link interface address.</summary>
		DataLink,
		/// <summary>LAT address.</summary>
		Lat,
		/// <summary>NSC Hyperchannel address.</summary>
		HyperChannel,
		/// <summary>AppleTalk address.</summary>
		AppleTalk,
		/// <summary>NetBios address.</summary>
		NetBios,
		/// <summary>VoiceView address.</summary>
		VoiceView,
		/// <summary>FireFox address.</summary>
		FireFox,
		/// <summary>Banyan address.</summary>
		Banyan = 21,
		/// <summary>Native ATM services address.</summary>
		Atm,
		/// <summary>Address for IP version 6.</summary>
		InterNetworkV6,
		/// <summary>Address for Microsoft cluster products.</summary>
		Cluster,
		/// <summary>IEEE 1284.4 workgroup address.</summary>
		Ieee12844,
		/// <summary>IrDA address.</summary>
		Irda,
		/// <summary>Address for Network Designers OSI gateway-enabled protocols.</summary>
		NetworkDesigners = 28,
		/// <summary>MAX address.</summary>
		Max
	}
}
