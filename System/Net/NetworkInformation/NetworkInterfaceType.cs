using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Specifies types of network interfaces.</summary>
	public enum NetworkInterfaceType
	{
		/// <summary>The interface type is not known.</summary>
		Unknown = 1,
		/// <summary>The network interface uses an Ethernet connection. Ethernet is defined in IEEE standard 802.3.</summary>
		Ethernet = 6,
		/// <summary>The network interface uses a Token-Ring connection. Token-Ring is defined in IEEE standard 802.5.</summary>
		TokenRing = 9,
		/// <summary>The network interface uses a Fiber Distributed Data Interface (FDDI) connection. FDDI is a set of standards for data transmission on fiber optic lines in a local area network.</summary>
		Fddi = 15,
		/// <summary>The network interface uses a basic rate interface Integrated Services Digital Network (ISDN) connection. ISDN is a set of standards for data transmission over telephone lines.</summary>
		BasicIsdn = 20,
		/// <summary>The network interface uses a primary rate interface Integrated Services Digital Network (ISDN) connection. ISDN is a set of standards for data transmission over telephone lines.</summary>
		PrimaryIsdn,
		/// <summary>The network interface uses a Point-To-Point protocol (PPP) connection. PPP is a protocol for data transmission using a serial device.</summary>
		Ppp = 23,
		/// <summary>The network interface is a loopback adapter. Such interfaces are often used for testing; no traffic is sent over the wire.</summary>
		Loopback,
		/// <summary>The network interface uses an Ethernet 3 megabit/second connection. This version of Ethernet is defined in IETF RFC 895.</summary>
		Ethernet3Megabit = 26,
		/// <summary>The network interface uses a Serial Line Internet Protocol (SLIP) connection. SLIP is defined in IETF RFC 1055.</summary>
		Slip = 28,
		/// <summary>The network interface uses asynchronous transfer mode (ATM) for data transmission.</summary>
		Atm = 37,
		/// <summary>The network interface uses a modem.</summary>
		GenericModem = 48,
		/// <summary>The network interface uses a Fast Ethernet connection over twisted pair and provides a data rate of 100 megabits per second. This type of connection is also known as 100Base-T.</summary>
		FastEthernetT = 62,
		/// <summary>The network interface uses a connection configured for ISDN and the X.25 protocol. X.25 allows computers on public networks to communicate using an intermediary computer.</summary>
		Isdn,
		/// <summary>The network interface uses a Fast Ethernet connection over optical fiber and provides a data rate of 100 megabits per second. This type of connection is also known as 100Base-FX.</summary>
		FastEthernetFx = 69,
		/// <summary>The network interface uses a wireless LAN connection (IEEE 802.11 standard).</summary>
		Wireless80211 = 71,
		/// <summary>The network interface uses an Asymmetric Digital Subscriber Line (ADSL).</summary>
		AsymmetricDsl = 94,
		/// <summary>The network interface uses a Rate Adaptive Digital Subscriber Line (RADSL).</summary>
		RateAdaptDsl,
		/// <summary>The network interface uses a Symmetric Digital Subscriber Line (SDSL).</summary>
		SymmetricDsl,
		/// <summary>The network interface uses a Very High Data Rate Digital Subscriber Line (VDSL).</summary>
		VeryHighSpeedDsl,
		/// <summary>The network interface uses the Internet Protocol (IP) in combination with asynchronous transfer mode (ATM) for data transmission.</summary>
		IPOverAtm = 114,
		/// <summary>The network interface uses a gigabit Ethernet connection and provides a data rate of 1,000 megabits per second (1 gigabit per second).</summary>
		GigabitEthernet = 117,
		/// <summary>The network interface uses a tunnel connection.</summary>
		Tunnel = 131,
		/// <summary>The network interface uses a Multirate Digital Subscriber Line.</summary>
		MultiRateSymmetricDsl = 143,
		/// <summary>The network interface uses a High Performance Serial Bus.</summary>
		HighPerformanceSerialBus
	}
}
