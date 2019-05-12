using Mono.Security.Protocol.Tls.Handshake;
using System;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Tls
{
	internal abstract class Context
	{
		internal const short MAX_FRAGMENT_SIZE = 16384;

		internal const short TLS1_PROTOCOL_CODE = 769;

		internal const short SSL3_PROTOCOL_CODE = 768;

		internal const long UNIX_BASE_TICKS = 621355968000000000L;

		private SecurityProtocolType securityProtocol;

		private byte[] sessionId;

		private SecurityCompressionType compressionMethod;

		private TlsServerSettings serverSettings;

		private TlsClientSettings clientSettings;

		private SecurityParameters current;

		private SecurityParameters negotiating;

		private SecurityParameters read;

		private SecurityParameters write;

		private CipherSuiteCollection supportedCiphers;

		private HandshakeType lastHandshakeMsg;

		private HandshakeState handshakeState;

		private bool abbreviatedHandshake;

		private bool receivedConnectionEnd;

		private bool sentConnectionEnd;

		private bool protocolNegotiated;

		private ulong writeSequenceNumber;

		private ulong readSequenceNumber;

		private byte[] clientRandom;

		private byte[] serverRandom;

		private byte[] randomCS;

		private byte[] randomSC;

		private byte[] masterSecret;

		private byte[] clientWriteKey;

		private byte[] serverWriteKey;

		private byte[] clientWriteIV;

		private byte[] serverWriteIV;

		private TlsStream handshakeMessages;

		private RandomNumberGenerator random;

		private RecordProtocol recordProtocol;

		public Context(SecurityProtocolType securityProtocolType)
		{
			this.SecurityProtocol = securityProtocolType;
			this.compressionMethod = SecurityCompressionType.None;
			this.serverSettings = new TlsServerSettings();
			this.clientSettings = new TlsClientSettings();
			this.handshakeMessages = new TlsStream();
			this.sessionId = null;
			this.handshakeState = HandshakeState.None;
			this.random = RandomNumberGenerator.Create();
		}

		public bool AbbreviatedHandshake
		{
			get
			{
				return this.abbreviatedHandshake;
			}
			set
			{
				this.abbreviatedHandshake = value;
			}
		}

		public bool ProtocolNegotiated
		{
			get
			{
				return this.protocolNegotiated;
			}
			set
			{
				this.protocolNegotiated = value;
			}
		}

		public SecurityProtocolType SecurityProtocol
		{
			get
			{
				if ((this.securityProtocol & SecurityProtocolType.Tls) == SecurityProtocolType.Tls || (this.securityProtocol & SecurityProtocolType.Default) == SecurityProtocolType.Default)
				{
					return SecurityProtocolType.Tls;
				}
				if ((this.securityProtocol & SecurityProtocolType.Ssl3) == SecurityProtocolType.Ssl3)
				{
					return SecurityProtocolType.Ssl3;
				}
				throw new NotSupportedException("Unsupported security protocol type");
			}
			set
			{
				this.securityProtocol = value;
			}
		}

		public SecurityProtocolType SecurityProtocolFlags
		{
			get
			{
				return this.securityProtocol;
			}
		}

		public short Protocol
		{
			get
			{
				SecurityProtocolType securityProtocolType = this.SecurityProtocol;
				if (securityProtocolType != SecurityProtocolType.Default)
				{
					if (securityProtocolType != SecurityProtocolType.Ssl2)
					{
						if (securityProtocolType == SecurityProtocolType.Ssl3)
						{
							return 768;
						}
						if (securityProtocolType == SecurityProtocolType.Tls)
						{
							return 769;
						}
					}
					throw new NotSupportedException("Unsupported security protocol type");
				}
				return 769;
			}
		}

		public byte[] SessionId
		{
			get
			{
				return this.sessionId;
			}
			set
			{
				this.sessionId = value;
			}
		}

		public SecurityCompressionType CompressionMethod
		{
			get
			{
				return this.compressionMethod;
			}
			set
			{
				this.compressionMethod = value;
			}
		}

		public TlsServerSettings ServerSettings
		{
			get
			{
				return this.serverSettings;
			}
		}

		public TlsClientSettings ClientSettings
		{
			get
			{
				return this.clientSettings;
			}
		}

		public HandshakeType LastHandshakeMsg
		{
			get
			{
				return this.lastHandshakeMsg;
			}
			set
			{
				this.lastHandshakeMsg = value;
			}
		}

		public HandshakeState HandshakeState
		{
			get
			{
				return this.handshakeState;
			}
			set
			{
				this.handshakeState = value;
			}
		}

		public bool ReceivedConnectionEnd
		{
			get
			{
				return this.receivedConnectionEnd;
			}
			set
			{
				this.receivedConnectionEnd = value;
			}
		}

		public bool SentConnectionEnd
		{
			get
			{
				return this.sentConnectionEnd;
			}
			set
			{
				this.sentConnectionEnd = value;
			}
		}

		public CipherSuiteCollection SupportedCiphers
		{
			get
			{
				return this.supportedCiphers;
			}
			set
			{
				this.supportedCiphers = value;
			}
		}

		public TlsStream HandshakeMessages
		{
			get
			{
				return this.handshakeMessages;
			}
		}

		public ulong WriteSequenceNumber
		{
			get
			{
				return this.writeSequenceNumber;
			}
			set
			{
				this.writeSequenceNumber = value;
			}
		}

		public ulong ReadSequenceNumber
		{
			get
			{
				return this.readSequenceNumber;
			}
			set
			{
				this.readSequenceNumber = value;
			}
		}

		public byte[] ClientRandom
		{
			get
			{
				return this.clientRandom;
			}
			set
			{
				this.clientRandom = value;
			}
		}

		public byte[] ServerRandom
		{
			get
			{
				return this.serverRandom;
			}
			set
			{
				this.serverRandom = value;
			}
		}

		public byte[] RandomCS
		{
			get
			{
				return this.randomCS;
			}
			set
			{
				this.randomCS = value;
			}
		}

		public byte[] RandomSC
		{
			get
			{
				return this.randomSC;
			}
			set
			{
				this.randomSC = value;
			}
		}

		public byte[] MasterSecret
		{
			get
			{
				return this.masterSecret;
			}
			set
			{
				this.masterSecret = value;
			}
		}

		public byte[] ClientWriteKey
		{
			get
			{
				return this.clientWriteKey;
			}
			set
			{
				this.clientWriteKey = value;
			}
		}

		public byte[] ServerWriteKey
		{
			get
			{
				return this.serverWriteKey;
			}
			set
			{
				this.serverWriteKey = value;
			}
		}

		public byte[] ClientWriteIV
		{
			get
			{
				return this.clientWriteIV;
			}
			set
			{
				this.clientWriteIV = value;
			}
		}

		public byte[] ServerWriteIV
		{
			get
			{
				return this.serverWriteIV;
			}
			set
			{
				this.serverWriteIV = value;
			}
		}

		public RecordProtocol RecordProtocol
		{
			get
			{
				return this.recordProtocol;
			}
			set
			{
				this.recordProtocol = value;
			}
		}

		public int GetUnixTime()
		{
			return (int)((DateTime.UtcNow.Ticks - 621355968000000000L) / 10000000L);
		}

		public byte[] GetSecureRandomBytes(int count)
		{
			byte[] array = new byte[count];
			this.random.GetNonZeroBytes(array);
			return array;
		}

		public virtual void Clear()
		{
			this.compressionMethod = SecurityCompressionType.None;
			this.serverSettings = new TlsServerSettings();
			this.clientSettings = new TlsClientSettings();
			this.handshakeMessages = new TlsStream();
			this.sessionId = null;
			this.handshakeState = HandshakeState.None;
			this.ClearKeyInfo();
		}

		public virtual void ClearKeyInfo()
		{
			if (this.masterSecret != null)
			{
				Array.Clear(this.masterSecret, 0, this.masterSecret.Length);
				this.masterSecret = null;
			}
			if (this.clientRandom != null)
			{
				Array.Clear(this.clientRandom, 0, this.clientRandom.Length);
				this.clientRandom = null;
			}
			if (this.serverRandom != null)
			{
				Array.Clear(this.serverRandom, 0, this.serverRandom.Length);
				this.serverRandom = null;
			}
			if (this.randomCS != null)
			{
				Array.Clear(this.randomCS, 0, this.randomCS.Length);
				this.randomCS = null;
			}
			if (this.randomSC != null)
			{
				Array.Clear(this.randomSC, 0, this.randomSC.Length);
				this.randomSC = null;
			}
			if (this.clientWriteKey != null)
			{
				Array.Clear(this.clientWriteKey, 0, this.clientWriteKey.Length);
				this.clientWriteKey = null;
			}
			if (this.clientWriteIV != null)
			{
				Array.Clear(this.clientWriteIV, 0, this.clientWriteIV.Length);
				this.clientWriteIV = null;
			}
			if (this.serverWriteKey != null)
			{
				Array.Clear(this.serverWriteKey, 0, this.serverWriteKey.Length);
				this.serverWriteKey = null;
			}
			if (this.serverWriteIV != null)
			{
				Array.Clear(this.serverWriteIV, 0, this.serverWriteIV.Length);
				this.serverWriteIV = null;
			}
			this.handshakeMessages.Reset();
			if (this.securityProtocol != SecurityProtocolType.Ssl3)
			{
			}
		}

		public SecurityProtocolType DecodeProtocolCode(short code)
		{
			if (code == 768)
			{
				return SecurityProtocolType.Ssl3;
			}
			if (code != 769)
			{
				throw new NotSupportedException("Unsupported security protocol type");
			}
			return SecurityProtocolType.Tls;
		}

		public void ChangeProtocol(short protocol)
		{
			SecurityProtocolType securityProtocolType = this.DecodeProtocolCode(protocol);
			if ((securityProtocolType & this.SecurityProtocolFlags) == securityProtocolType || (this.SecurityProtocolFlags & SecurityProtocolType.Default) == SecurityProtocolType.Default)
			{
				this.SecurityProtocol = securityProtocolType;
				this.SupportedCiphers.Clear();
				this.SupportedCiphers = null;
				this.SupportedCiphers = CipherSuiteFactory.GetSupportedCiphers(securityProtocolType);
				return;
			}
			throw new TlsException(AlertDescription.ProtocolVersion, "Incorrect protocol version received from server");
		}

		public SecurityParameters Current
		{
			get
			{
				if (this.current == null)
				{
					this.current = new SecurityParameters();
				}
				if (this.current.Cipher != null)
				{
					this.current.Cipher.Context = this;
				}
				return this.current;
			}
		}

		public SecurityParameters Negotiating
		{
			get
			{
				if (this.negotiating == null)
				{
					this.negotiating = new SecurityParameters();
				}
				if (this.negotiating.Cipher != null)
				{
					this.negotiating.Cipher.Context = this;
				}
				return this.negotiating;
			}
		}

		public SecurityParameters Read
		{
			get
			{
				return this.read;
			}
		}

		public SecurityParameters Write
		{
			get
			{
				return this.write;
			}
		}

		public void StartSwitchingSecurityParameters(bool client)
		{
			if (client)
			{
				this.write = this.negotiating;
				this.read = this.current;
			}
			else
			{
				this.read = this.negotiating;
				this.write = this.current;
			}
			this.current = this.negotiating;
		}

		public void EndSwitchingSecurityParameters(bool client)
		{
			SecurityParameters securityParameters;
			if (client)
			{
				securityParameters = this.read;
				this.read = this.current;
			}
			else
			{
				securityParameters = this.write;
				this.write = this.current;
			}
			if (securityParameters != null)
			{
				securityParameters.Clear();
			}
			this.negotiating = securityParameters;
		}
	}
}
