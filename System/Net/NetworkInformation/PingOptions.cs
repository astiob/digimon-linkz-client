using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Used to control how <see cref="T:System.Net.NetworkInformation.Ping" /> data packets are transmitted.</summary>
	public class PingOptions
	{
		private int ttl = 128;

		private bool dont_fragment;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkInformation.PingOptions" /> class.</summary>
		public PingOptions()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkInformation.PingOptions" /> class and sets the Time to Live and fragmentation values.</summary>
		/// <param name="ttl">An <see cref="T:System.Int32" /> value greater than zero that specifies the number of times that the <see cref="T:System.Net.NetworkInformation.Ping" /> data packets can be forwarded.</param>
		/// <param name="dontFragment">true to prevent data sent to the remote host from being fragmented; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="ttl " />is less than or equal to zero.</exception>
		public PingOptions(int ttl, bool dontFragment)
		{
			if (ttl <= 0)
			{
				throw new ArgumentOutOfRangeException("Must be greater than zero.", "ttl");
			}
			this.ttl = ttl;
			this.dont_fragment = dontFragment;
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls fragmentation of the data sent to the remote host.</summary>
		/// <returns>true if the data cannot be sent in multiple packets; otherwise false. The default is false.</returns>
		public bool DontFragment
		{
			get
			{
				return this.dont_fragment;
			}
			set
			{
				this.dont_fragment = value;
			}
		}

		/// <summary>Gets or sets the number of routing nodes that can forward the <see cref="T:System.Net.NetworkInformation.Ping" /> data before it is discarded.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that specifies the number of times the <see cref="T:System.Net.NetworkInformation.Ping" /> data packets can be forwarded. The default is 128.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero.</exception>
		public int Ttl
		{
			get
			{
				return this.ttl;
			}
			set
			{
				this.ttl = value;
			}
		}
	}
}
