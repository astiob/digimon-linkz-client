using System;
using System.ComponentModel;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides data for the <see cref="E:System.Net.NetworkInformation.Ping.PingCompleted" /> event.</summary>
	public class PingCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
	{
		private PingReply reply;

		internal PingCompletedEventArgs(Exception ex, bool cancelled, object userState, PingReply reply) : base(ex, cancelled, userState)
		{
			this.reply = reply;
		}

		/// <summary>Gets an object that contains data that describes an attempt to send an Internet Control Message Protocol (ICMP) echo request message and receive a corresponding ICMP echo reply message.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that describes the results of the ICMP echo request.</returns>
		public PingReply Reply
		{
			get
			{
				return this.reply;
			}
		}
	}
}
