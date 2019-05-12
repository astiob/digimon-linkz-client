using System;

namespace System.Net.Sockets
{
	/// <summary>Encapsulates the information that is necessary to duplicate a <see cref="T:System.Net.Sockets.Socket" />.</summary>
	[Serializable]
	public struct SocketInformation
	{
		private SocketInformationOptions options;

		private byte[] protocol_info;

		/// <summary>Gets or sets the options for a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketInformationOptions" /> instance.</returns>
		public SocketInformationOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		/// <summary>Gets or sets the protocol information for a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An array of type <see cref="T:System.Byte" />.</returns>
		public byte[] ProtocolInformation
		{
			get
			{
				return this.protocol_info;
			}
			set
			{
				this.protocol_info = value;
			}
		}
	}
}
