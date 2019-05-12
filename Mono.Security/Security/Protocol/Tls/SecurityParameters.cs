using System;

namespace Mono.Security.Protocol.Tls
{
	internal class SecurityParameters
	{
		private CipherSuite cipher;

		private byte[] clientWriteMAC;

		private byte[] serverWriteMAC;

		public CipherSuite Cipher
		{
			get
			{
				return this.cipher;
			}
			set
			{
				this.cipher = value;
			}
		}

		public byte[] ClientWriteMAC
		{
			get
			{
				return this.clientWriteMAC;
			}
			set
			{
				this.clientWriteMAC = value;
			}
		}

		public byte[] ServerWriteMAC
		{
			get
			{
				return this.serverWriteMAC;
			}
			set
			{
				this.serverWriteMAC = value;
			}
		}

		public void Clear()
		{
			this.cipher = null;
		}
	}
}
