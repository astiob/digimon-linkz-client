using System;
using System.ComponentModel;

namespace System.Net
{
	/// <summary>Provides data for the <see cref="E:System.Net.WebClient.UploadProgressChanged" /> event of a <see cref="T:System.Net.WebClient" />.</summary>
	public class UploadProgressChangedEventArgs : System.ComponentModel.ProgressChangedEventArgs
	{
		private long received;

		private long sent;

		private long total_recv;

		private long total_send;

		internal UploadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive, long bytesSent, long totalBytesToSend, int progressPercentage, object userState) : base(progressPercentage, userState)
		{
			this.received = bytesReceived;
			this.total_recv = totalBytesToReceive;
			this.sent = bytesSent;
			this.total_send = totalBytesToSend;
		}

		/// <summary>Gets the number of bytes received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes received.</returns>
		public long BytesReceived
		{
			get
			{
				return this.received;
			}
		}

		/// <summary>Gets the total number of bytes in a <see cref="T:System.Net.WebClient" /> data upload operation.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes that will be received.</returns>
		public long TotalBytesToReceive
		{
			get
			{
				return this.total_recv;
			}
		}

		/// <summary>Gets the number of bytes sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes sent.</returns>
		public long BytesSent
		{
			get
			{
				return this.sent;
			}
		}

		/// <summary>Gets the total number of bytes to send.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes that will be sent.</returns>
		public long TotalBytesToSend
		{
			get
			{
				return this.total_send;
			}
		}
	}
}
