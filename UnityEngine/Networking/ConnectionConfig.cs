using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	/// <summary>
	///   <para>This class defines parameters of connection between two peers, this definition includes various timeouts and sizes as well as channel configuration.</para>
	/// </summary>
	[Serializable]
	public class ConnectionConfig
	{
		private const int g_MinPacketSize = 128;

		[SerializeField]
		private ushort m_PacketSize;

		[SerializeField]
		private ushort m_FragmentSize;

		[SerializeField]
		private uint m_ResendTimeout;

		[SerializeField]
		private uint m_DisconnectTimeout;

		[SerializeField]
		private uint m_ConnectTimeout;

		[SerializeField]
		private uint m_MinUpdateTimeout;

		[SerializeField]
		private uint m_PingTimeout;

		[SerializeField]
		private uint m_ReducedPingTimeout;

		[SerializeField]
		private uint m_AllCostTimeout;

		[SerializeField]
		private byte m_NetworkDropThreshold;

		[SerializeField]
		private byte m_OverflowDropThreshold;

		[SerializeField]
		private byte m_MaxConnectionAttempt;

		[SerializeField]
		private uint m_AckDelay;

		[SerializeField]
		private ushort m_MaxCombinedReliableMessageSize;

		[SerializeField]
		private ushort m_MaxCombinedReliableMessageCount;

		[SerializeField]
		private ushort m_MaxSentMessageQueueSize;

		[SerializeField]
		private bool m_IsAcksLong;

		[SerializeField]
		internal List<ChannelQOS> m_Channels = new List<ChannelQOS>();

		/// <summary>
		///   <para>Will create default connection config or will copy them from another.</para>
		/// </summary>
		/// <param name="config">Connection config.</param>
		public ConnectionConfig()
		{
			this.m_PacketSize = 1500;
			this.m_FragmentSize = 500;
			this.m_ResendTimeout = 1200u;
			this.m_DisconnectTimeout = 2000u;
			this.m_ConnectTimeout = 2000u;
			this.m_MinUpdateTimeout = 10u;
			this.m_PingTimeout = 500u;
			this.m_ReducedPingTimeout = 100u;
			this.m_AllCostTimeout = 20u;
			this.m_NetworkDropThreshold = 5;
			this.m_OverflowDropThreshold = 5;
			this.m_MaxConnectionAttempt = 10;
			this.m_AckDelay = 33u;
			this.m_MaxCombinedReliableMessageSize = 100;
			this.m_MaxCombinedReliableMessageCount = 10;
			this.m_MaxSentMessageQueueSize = 128;
			this.m_IsAcksLong = false;
		}

		/// <summary>
		///   <para>Will create default connection config or will copy them from another.</para>
		/// </summary>
		/// <param name="config">Connection config.</param>
		public ConnectionConfig(ConnectionConfig config)
		{
			if (config == null)
			{
				throw new NullReferenceException("config is not defined");
			}
			this.m_PacketSize = config.m_PacketSize;
			this.m_FragmentSize = config.m_FragmentSize;
			this.m_ResendTimeout = config.m_ResendTimeout;
			this.m_DisconnectTimeout = config.m_DisconnectTimeout;
			this.m_ConnectTimeout = config.m_ConnectTimeout;
			this.m_MinUpdateTimeout = config.m_MinUpdateTimeout;
			this.m_PingTimeout = config.m_PingTimeout;
			this.m_ReducedPingTimeout = config.m_ReducedPingTimeout;
			this.m_AllCostTimeout = config.m_AllCostTimeout;
			this.m_NetworkDropThreshold = config.m_NetworkDropThreshold;
			this.m_OverflowDropThreshold = config.m_OverflowDropThreshold;
			this.m_MaxConnectionAttempt = config.m_MaxConnectionAttempt;
			this.m_AckDelay = config.m_AckDelay;
			this.m_MaxCombinedReliableMessageSize = config.MaxCombinedReliableMessageSize;
			this.m_MaxCombinedReliableMessageCount = config.m_MaxCombinedReliableMessageCount;
			this.m_MaxSentMessageQueueSize = config.m_MaxSentMessageQueueSize;
			this.m_IsAcksLong = config.m_IsAcksLong;
			foreach (ChannelQOS channel in config.m_Channels)
			{
				this.m_Channels.Add(new ChannelQOS(channel));
			}
		}

		/// <summary>
		///   <para>Validate parameters of connection config. Will throw exceptions if parameters are incorrect.</para>
		/// </summary>
		/// <param name="config"></param>
		public static void Validate(ConnectionConfig config)
		{
			if (config.m_PacketSize < 128)
			{
				throw new ArgumentOutOfRangeException("PacketSize should be > " + 128.ToString());
			}
			if (config.m_FragmentSize >= config.m_PacketSize - 128)
			{
				throw new ArgumentOutOfRangeException("FragmentSize should be < PacketSize - " + 128.ToString());
			}
			if (config.m_Channels.Count > 255)
			{
				throw new ArgumentOutOfRangeException("Channels number should be less than 256");
			}
		}

		/// <summary>
		///   <para>What is a maximum packet size (in Bytes) (including payload and all header). Packet can contain multiple messages inside.</para>
		/// </summary>
		public ushort PacketSize
		{
			get
			{
				return this.m_PacketSize;
			}
			set
			{
				this.m_PacketSize = value;
			}
		}

		/// <summary>
		///   <para>What should be maximum fragment size (in Bytes) for fragmented messages.</para>
		/// </summary>
		public ushort FragmentSize
		{
			get
			{
				return this.m_FragmentSize;
			}
			set
			{
				this.m_FragmentSize = value;
			}
		}

		/// <summary>
		///   <para>Minimum timeout (in ms) which library will wait before it will resend reliable message.</para>
		/// </summary>
		public uint ResendTimeout
		{
			get
			{
				return this.m_ResendTimeout;
			}
			set
			{
				this.m_ResendTimeout = value;
			}
		}

		/// <summary>
		///   <para>How long (in ms) library will wait before it will consider connection as disconnected.</para>
		/// </summary>
		public uint DisconnectTimeout
		{
			get
			{
				return this.m_DisconnectTimeout;
			}
			set
			{
				this.m_DisconnectTimeout = value;
			}
		}

		/// <summary>
		///   <para>Timeout in ms which library will wait before it will send another connection request.</para>
		/// </summary>
		public uint ConnectTimeout
		{
			get
			{
				return this.m_ConnectTimeout;
			}
			set
			{
				this.m_ConnectTimeout = value;
			}
		}

		/// <summary>
		///   <para>Minimal send update timeout (in ms) for connection. this timeout could be increased by library if flow control will required.</para>
		/// </summary>
		public uint MinUpdateTimeout
		{
			get
			{
				return this.m_MinUpdateTimeout;
			}
			set
			{
				if (value == 0u)
				{
					throw new ArgumentOutOfRangeException("Minimal update timeout should be > 0");
				}
				this.m_MinUpdateTimeout = value;
			}
		}

		/// <summary>
		///   <para>Timeout in ms between control protocol messages.</para>
		/// </summary>
		public uint PingTimeout
		{
			get
			{
				return this.m_PingTimeout;
			}
			set
			{
				this.m_PingTimeout = value;
			}
		}

		/// <summary>
		///   <para>Timeout in ms for control messages which library will use before it will accumulate statistics.</para>
		/// </summary>
		public uint ReducedPingTimeout
		{
			get
			{
				return this.m_ReducedPingTimeout;
			}
			set
			{
				this.m_ReducedPingTimeout = value;
			}
		}

		/// <summary>
		///   <para>Defines timeout in ms after that message with AllCost deliver qos will force resend without acknowledgement waiting.</para>
		/// </summary>
		public uint AllCostTimeout
		{
			get
			{
				return this.m_AllCostTimeout;
			}
			set
			{
				this.m_AllCostTimeout = value;
			}
		}

		/// <summary>
		///   <para>How many (in %) packet need to be dropped due network condition before library will throttle send rate.</para>
		/// </summary>
		public byte NetworkDropThreshold
		{
			get
			{
				return this.m_NetworkDropThreshold;
			}
			set
			{
				this.m_NetworkDropThreshold = value;
			}
		}

		/// <summary>
		///   <para>How many (in %) packet need to be dropped due lack of internal bufferes before library will throttle send rate.</para>
		/// </summary>
		public byte OverflowDropThreshold
		{
			get
			{
				return this.m_OverflowDropThreshold;
			}
			set
			{
				this.m_OverflowDropThreshold = value;
			}
		}

		/// <summary>
		///   <para>How many attempt library will get before it will consider the connection as disconnected.</para>
		/// </summary>
		public byte MaxConnectionAttempt
		{
			get
			{
				return this.m_MaxConnectionAttempt;
			}
			set
			{
				this.m_MaxConnectionAttempt = value;
			}
		}

		/// <summary>
		///   <para>How long in ms receiver will wait before it will force send acknowledgements back without waiting any payload.</para>
		/// </summary>
		public uint AckDelay
		{
			get
			{
				return this.m_AckDelay;
			}
			set
			{
				this.m_AckDelay = value;
			}
		}

		/// <summary>
		///   <para>Maximum size of reliable message which library will consider as small and will try to combine in one "array of messages" message.</para>
		/// </summary>
		public ushort MaxCombinedReliableMessageSize
		{
			get
			{
				return this.m_MaxCombinedReliableMessageSize;
			}
			set
			{
				this.m_MaxCombinedReliableMessageSize = value;
			}
		}

		/// <summary>
		///   <para>Maximum amount of small reliable messages which will combine in one "array of messages". Useful if you are going to send a lot of small reliable messages.</para>
		/// </summary>
		public ushort MaxCombinedReliableMessageCount
		{
			get
			{
				return this.m_MaxCombinedReliableMessageCount;
			}
			set
			{
				this.m_MaxCombinedReliableMessageCount = value;
			}
		}

		/// <summary>
		///   <para>Defines maximum messages which will wait for sending before user will receive error on Send() call.</para>
		/// </summary>
		public ushort MaxSentMessageQueueSize
		{
			get
			{
				return this.m_MaxSentMessageQueueSize;
			}
			set
			{
				this.m_MaxSentMessageQueueSize = value;
			}
		}

		/// <summary>
		///   <para>If it is true, connection will use 64 bit mask to acknowledge received reliable messages.</para>
		/// </summary>
		public bool IsAcksLong
		{
			get
			{
				return this.m_IsAcksLong;
			}
			set
			{
				this.m_IsAcksLong = value;
			}
		}

		/// <summary>
		///   <para>Return amount of channels for current configuration.</para>
		/// </summary>
		public int ChannelCount
		{
			get
			{
				return this.m_Channels.Count;
			}
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="value">Add new channel to configuration.</param>
		/// <returns>
		///   <para>Channel id, user can use this id to send message via this channel.</para>
		/// </returns>
		public byte AddChannel(QosType value)
		{
			if (this.m_Channels.Count > 255)
			{
				throw new ArgumentOutOfRangeException("Channels Count should be less than 256");
			}
			if (!Enum.IsDefined(typeof(QosType), value))
			{
				throw new ArgumentOutOfRangeException("requested qos type doesn't exist: " + (int)value);
			}
			ChannelQOS item = new ChannelQOS(value);
			this.m_Channels.Add(item);
			return (byte)(this.m_Channels.Count - 1);
		}

		/// <summary>
		///   <para>Return the QoS set for the given channel or throw an out of range exception.</para>
		/// </summary>
		/// <param name="idx">Index in array.</param>
		/// <returns>
		///   <para>Channel QoS.</para>
		/// </returns>
		public QosType GetChannel(byte idx)
		{
			if ((int)idx >= this.m_Channels.Count)
			{
				throw new ArgumentOutOfRangeException("requested index greater than maximum channels count");
			}
			return this.m_Channels[(int)idx].QOS;
		}

		/// <summary>
		///   <para>Allow access to channels list.</para>
		/// </summary>
		public List<ChannelQOS> Channels
		{
			get
			{
				return this.m_Channels;
			}
		}
	}
}
