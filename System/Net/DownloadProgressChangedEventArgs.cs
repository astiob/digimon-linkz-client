using System;
using System.ComponentModel;

namespace System.Net
{
	/// <summary>Provides data for the <see cref="E:System.Net.WebClient.DownloadProgressChanged" /> event of a <see cref="T:System.Net.WebClient" />.</summary>
	public class DownloadProgressChangedEventArgs : System.ComponentModel.ProgressChangedEventArgs
	{
		private long received;

		private long total;

		internal DownloadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive, object userState) : base((totalBytesToReceive == -1L) ? 0 : ((int)(bytesReceived * 100L / totalBytesToReceive)), userState)
		{
			this.received = bytesReceived;
			this.total = totalBytesToReceive;
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

		/// <summary>Gets the total number of bytes in a <see cref="T:System.Net.WebClient" /> data download operation.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes that will be received.</returns>
		public long TotalBytesToReceive
		{
			get
			{
				return this.total;
			}
		}
	}
}
