using System;

namespace System.Net.Sockets
{
	/// <summary>Presents the packet information from a call to <see cref="M:System.Net.Sockets.Socket.ReceiveMessageFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> or <see cref="M:System.Net.Sockets.Socket.EndReceiveMessageFrom(System.IAsyncResult,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" />.</summary>
	public struct IPPacketInformation
	{
		private IPAddress address;

		private int iface;

		internal IPPacketInformation(IPAddress address, int iface)
		{
			this.address = address;
			this.iface = iface;
		}

		/// <summary>Gets the origin information of the packet that was received as a result of calling the <see cref="M:System.Net.Sockets.Socket.ReceiveMessageFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> method or <see cref="M:System.Net.Sockets.Socket.EndReceiveMessageFrom(System.IAsyncResult,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> method.</summary>
		/// <returns>An <see cref="T:System.Net.IPAddress" /> that indicates the origin information of the packet that was received as a result of calling the <see cref="M:System.Net.Sockets.Socket.ReceiveMessageFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> method or <see cref="M:System.Net.Sockets.Socket.EndReceiveMessageFrom(System.IAsyncResult,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> method. For packets that were sent from a unicast address, the <see cref="P:System.Net.Sockets.IPPacketInformation.Address" /> property will return the <see cref="T:System.Net.IPAddress" /> of the sender; for multicast or broadcast packets, the <see cref="P:System.Net.Sockets.IPPacketInformation.Address" /> property will return the multicast or broadcast <see cref="T:System.Net.IPAddress" />.</returns>
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		/// <summary>Gets the network interface information that is associated with a call to <see cref="M:System.Net.Sockets.Socket.ReceiveMessageFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> or <see cref="M:System.Net.Sockets.Socket.EndReceiveMessageFrom(System.IAsyncResult,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value, which represents the index of the network interface. You can use this index with <see cref="M:System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces" /> to get more information about the relevant interface.</returns>
		public int Interface
		{
			get
			{
				return this.iface;
			}
		}

		/// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="comparand" /> is an instance of <see cref="T:System.Net.Sockets.IPPacketInformation" /> and equals the value of the instance; otherwise, false.</returns>
		/// <param name="comparand">The object to compare with this instance.</param>
		public override bool Equals(object comparand)
		{
			if (!(comparand is IPPacketInformation))
			{
				return false;
			}
			IPPacketInformation ippacketInformation = (IPPacketInformation)comparand;
			return ippacketInformation.iface == this.iface && ippacketInformation.address.Equals(this.address);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>An Int32 hash code.</returns>
		public override int GetHashCode()
		{
			return this.address.GetHashCode() + this.iface;
		}

		/// <summary>Tests whether two specified <see cref="T:System.Net.Sockets.IPPacketInformation" /> instances are equivalent.</summary>
		/// <returns>true if <paramref name="packetInformation1" /> and <paramref name="packetInformation2" /> are equal; otherwise, false.</returns>
		/// <param name="packetInformation1">The <see cref="T:System.Net.Sockets.IPPacketInformation" /> instance that is to the left of the equality operator.</param>
		/// <param name="packetInformation2">The <see cref="T:System.Net.Sockets.IPPacketInformation" /> instance that is to the right of the equality operator.</param>
		public static bool operator ==(IPPacketInformation p1, IPPacketInformation p2)
		{
			return p1.Equals(p2);
		}

		/// <summary>Tests whether two specified <see cref="T:System.Net.Sockets.IPPacketInformation" /> instances are not equal.</summary>
		/// <returns>true if <paramref name="packetInformation1" /> and <paramref name="packetInformation2" /> are unequal; otherwise, false.</returns>
		/// <param name="packetInformation1">The <see cref="T:System.Net.Sockets.IPPacketInformation" /> instance that is to the left of the inequality operator.</param>
		/// <param name="packetInformation2">The <see cref="T:System.Net.Sockets.IPPacketInformation" /> instance that is to the right of the inequality operator.</param>
		public static bool operator !=(IPPacketInformation p1, IPPacketInformation p2)
		{
			return !p1.Equals(p2);
		}
	}
}
