using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides information about the status and data resulting from a <see cref="Overload:System.Net.NetworkInformation.Ping.Send" /> or <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" /> operation.</summary>
	public class PingReply
	{
		private IPAddress address;

		private byte[] buffer;

		private PingOptions options;

		private long rtt;

		private IPStatus status;

		internal PingReply(IPAddress address, byte[] buffer, PingOptions options, long roundtripTime, IPStatus status)
		{
			this.address = address;
			this.buffer = buffer;
			this.options = options;
			this.rtt = roundtripTime;
			this.status = status;
		}

		/// <summary>Gets the address of the host that sends the Internet Control Message Protocol (ICMP) echo reply.</summary>
		/// <returns>An <see cref="T:System.Net.IPAddress" /> containing the destination for the ICMP echo message.</returns>
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		/// <summary>Gets the buffer of data received in an Internet Control Message Protocol (ICMP) echo reply message.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the data received in an ICMP echo reply message, or an empty array, if no reply was received.</returns>
		public byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		/// <summary>Gets the options used to transmit the reply to an Internet Control Message Protocol (ICMP) echo request.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingOptions" /> object that contains the Time to Live (TTL) and the fragmentation directive used for transmitting the reply if <see cref="P:System.Net.NetworkInformation.PingReply.Status" /> is <see cref="F:System.Net.NetworkInformation.IPStatus.Success" />; otherwise, null.</returns>
		public PingOptions Options
		{
			get
			{
				return this.options;
			}
		}

		/// <summary>Gets the number of milliseconds taken to send an Internet Control Message Protocol (ICMP) echo request and receive the corresponding ICMP echo reply message.</summary>
		/// <returns>An <see cref="T:System.Int64" /> that specifies the round trip time, in milliseconds. </returns>
		public long RoundtripTime
		{
			get
			{
				return this.rtt;
			}
		}

		/// <summary>Gets the status of an attempt to send an Internet Control Message Protocol (ICMP) echo request and receive the corresponding ICMP echo reply message.</summary>
		/// <returns>An <see cref="T:System.Net.NetworkInformation.IPStatus" /> value indicating the result of the request.</returns>
		public IPStatus Status
		{
			get
			{
				return this.status;
			}
		}
	}
}
