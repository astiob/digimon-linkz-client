using System;

namespace Mono.Security.Protocol.Tls.Handshake
{
	internal abstract class HandshakeMessage : TlsStream
	{
		private Context context;

		private HandshakeType handshakeType;

		private ContentType contentType;

		private byte[] cache;

		public HandshakeMessage(Context context, HandshakeType handshakeType) : this(context, handshakeType, ContentType.Handshake)
		{
		}

		public HandshakeMessage(Context context, HandshakeType handshakeType, ContentType contentType)
		{
			this.context = context;
			this.handshakeType = handshakeType;
			this.contentType = contentType;
		}

		public HandshakeMessage(Context context, HandshakeType handshakeType, byte[] data) : base(data)
		{
			this.context = context;
			this.handshakeType = handshakeType;
		}

		public Context Context
		{
			get
			{
				return this.context;
			}
		}

		public HandshakeType HandshakeType
		{
			get
			{
				return this.handshakeType;
			}
		}

		public ContentType ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		protected abstract void ProcessAsTls1();

		protected abstract void ProcessAsSsl3();

		public void Process()
		{
			SecurityProtocolType securityProtocol = this.Context.SecurityProtocol;
			if (securityProtocol != SecurityProtocolType.Default)
			{
				if (securityProtocol != SecurityProtocolType.Ssl2)
				{
					if (securityProtocol == SecurityProtocolType.Ssl3)
					{
						this.ProcessAsSsl3();
						return;
					}
					if (securityProtocol == SecurityProtocolType.Tls)
					{
						goto IL_37;
					}
				}
				throw new NotSupportedException("Unsupported security protocol type");
			}
			IL_37:
			this.ProcessAsTls1();
		}

		public virtual void Update()
		{
			if (this.CanWrite)
			{
				if (this.cache == null)
				{
					this.cache = this.EncodeMessage();
				}
				this.context.HandshakeMessages.Write(this.cache);
				base.Reset();
				this.cache = null;
			}
		}

		public virtual byte[] EncodeMessage()
		{
			this.cache = null;
			if (this.CanWrite)
			{
				byte[] array = base.ToArray();
				int num = array.Length;
				this.cache = new byte[4 + num];
				this.cache[0] = (byte)this.HandshakeType;
				this.cache[1] = (byte)(num >> 16);
				this.cache[2] = (byte)(num >> 8);
				this.cache[3] = (byte)num;
				Buffer.BlockCopy(array, 0, this.cache, 4, num);
			}
			return this.cache;
		}

		public static bool Compare(byte[] buffer1, byte[] buffer2)
		{
			if (buffer1 == null || buffer2 == null)
			{
				return false;
			}
			if (buffer1.Length != buffer2.Length)
			{
				return false;
			}
			for (int i = 0; i < buffer1.Length; i++)
			{
				if (buffer1[i] != buffer2[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
