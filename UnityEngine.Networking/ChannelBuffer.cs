using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal class ChannelBuffer : IDisposable
	{
		private NetworkConnection m_Connection;

		private ChannelPacket m_CurrentPacket;

		private float m_LastFlushTime;

		private byte m_ChannelId;

		private int m_MaxPacketSize;

		private bool m_IsReliable;

		private bool m_AllowFragmentation;

		private bool m_IsBroken;

		private int m_MaxPendingPacketCount;

		private const int k_MaxFreePacketCount = 512;

		public const int MaxPendingPacketCount = 16;

		public const int MaxBufferedPackets = 512;

		private Queue<ChannelPacket> m_PendingPackets;

		private static List<ChannelPacket> s_FreePackets;

		internal static int pendingPacketCount;

		public float maxDelay = 0.01f;

		private float m_LastBufferedMessageCountTimer = Time.realtimeSinceStartup;

		private static NetworkWriter s_SendWriter = new NetworkWriter();

		private static NetworkWriter s_FragmentWriter = new NetworkWriter();

		private const int k_PacketHeaderReserveSize = 100;

		private bool m_Disposed;

		internal NetBuffer fragmentBuffer = new NetBuffer();

		private bool readingFragment = false;

		public ChannelBuffer(NetworkConnection conn, int bufferSize, byte cid, bool isReliable, bool isSequenced)
		{
			this.m_Connection = conn;
			this.m_MaxPacketSize = bufferSize - 100;
			this.m_CurrentPacket = new ChannelPacket(this.m_MaxPacketSize, isReliable);
			this.m_ChannelId = cid;
			this.m_MaxPendingPacketCount = 16;
			this.m_IsReliable = isReliable;
			this.m_AllowFragmentation = (isReliable && isSequenced);
			if (isReliable)
			{
				this.m_PendingPackets = new Queue<ChannelPacket>();
				if (ChannelBuffer.s_FreePackets == null)
				{
					ChannelBuffer.s_FreePackets = new List<ChannelPacket>();
				}
			}
		}

		public int numMsgsOut { get; private set; }

		public int numBufferedMsgsOut { get; private set; }

		public int numBytesOut { get; private set; }

		public int numMsgsIn { get; private set; }

		public int numBytesIn { get; private set; }

		public int numBufferedPerSecond { get; private set; }

		public int lastBufferedPerSecond { get; private set; }

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_Disposed)
			{
				if (disposing)
				{
					if (this.m_PendingPackets != null)
					{
						while (this.m_PendingPackets.Count > 0)
						{
							ChannelBuffer.pendingPacketCount--;
							ChannelPacket item = this.m_PendingPackets.Dequeue();
							if (ChannelBuffer.s_FreePackets.Count < 512)
							{
								ChannelBuffer.s_FreePackets.Add(item);
							}
						}
						this.m_PendingPackets.Clear();
					}
				}
			}
			this.m_Disposed = true;
		}

		public bool SetOption(ChannelOption option, int value)
		{
			bool result;
			if (option != ChannelOption.MaxPendingBuffers)
			{
				if (option != ChannelOption.AllowFragmentation)
				{
					if (option != ChannelOption.MaxPacketSize)
					{
						result = false;
					}
					else if (!this.m_CurrentPacket.IsEmpty() || this.m_PendingPackets.Count > 0)
					{
						if (LogFilter.logError)
						{
							Debug.LogError("Cannot set MaxPacketSize after sending data.");
						}
						result = false;
					}
					else if (value <= 0)
					{
						if (LogFilter.logError)
						{
							Debug.LogError("Cannot set MaxPacketSize less than one.");
						}
						result = false;
					}
					else if (value > this.m_MaxPacketSize)
					{
						if (LogFilter.logError)
						{
							Debug.LogError("Cannot set MaxPacketSize to greater than the existing maximum (" + this.m_MaxPacketSize + ").");
						}
						result = false;
					}
					else
					{
						this.m_CurrentPacket = new ChannelPacket(value, this.m_IsReliable);
						this.m_MaxPacketSize = value;
						result = true;
					}
				}
				else
				{
					this.m_AllowFragmentation = (value != 0);
					result = true;
				}
			}
			else if (!this.m_IsReliable)
			{
				result = false;
			}
			else if (value < 0 || value >= 512)
			{
				if (LogFilter.logError)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Invalid MaxPendingBuffers for channel ",
						this.m_ChannelId,
						". Must be greater than zero and less than ",
						512
					}));
				}
				result = false;
			}
			else
			{
				this.m_MaxPendingPacketCount = value;
				result = true;
			}
			return result;
		}

		public void CheckInternalBuffer()
		{
			if (Time.realtimeSinceStartup - this.m_LastFlushTime > this.maxDelay && !this.m_CurrentPacket.IsEmpty())
			{
				this.SendInternalBuffer();
				this.m_LastFlushTime = Time.realtimeSinceStartup;
			}
			if (Time.realtimeSinceStartup - this.m_LastBufferedMessageCountTimer > 1f)
			{
				this.lastBufferedPerSecond = this.numBufferedPerSecond;
				this.numBufferedPerSecond = 0;
				this.m_LastBufferedMessageCountTimer = Time.realtimeSinceStartup;
			}
		}

		public bool SendWriter(NetworkWriter writer)
		{
			return this.SendBytes(writer.AsArraySegment().Array, writer.AsArraySegment().Count);
		}

		public bool Send(short msgType, MessageBase msg)
		{
			ChannelBuffer.s_SendWriter.StartMessage(msgType);
			msg.Serialize(ChannelBuffer.s_SendWriter);
			ChannelBuffer.s_SendWriter.FinishMessage();
			this.numMsgsOut++;
			return this.SendWriter(ChannelBuffer.s_SendWriter);
		}

		internal bool HandleFragment(NetworkReader reader)
		{
			bool result;
			if (reader.ReadByte() == 0)
			{
				if (!this.readingFragment)
				{
					this.fragmentBuffer.SeekZero();
					this.readingFragment = true;
				}
				byte[] array = reader.ReadBytesAndSize();
				this.fragmentBuffer.WriteBytes(array, (ushort)array.Length);
				result = false;
			}
			else
			{
				this.readingFragment = false;
				result = true;
			}
			return result;
		}

		internal bool SendFragmentBytes(byte[] bytes, int bytesToSend)
		{
			int num = 0;
			while (bytesToSend > 0)
			{
				int num2 = Math.Min(bytesToSend, this.m_MaxPacketSize - 32);
				byte[] array = new byte[num2];
				Array.Copy(bytes, num, array, 0, num2);
				ChannelBuffer.s_FragmentWriter.StartMessage(17);
				ChannelBuffer.s_FragmentWriter.Write(0);
				ChannelBuffer.s_FragmentWriter.WriteBytesFull(array);
				ChannelBuffer.s_FragmentWriter.FinishMessage();
				this.SendWriter(ChannelBuffer.s_FragmentWriter);
				num += num2;
				bytesToSend -= num2;
			}
			ChannelBuffer.s_FragmentWriter.StartMessage(17);
			ChannelBuffer.s_FragmentWriter.Write(1);
			ChannelBuffer.s_FragmentWriter.FinishMessage();
			this.SendWriter(ChannelBuffer.s_FragmentWriter);
			return true;
		}

		internal bool SendBytes(byte[] bytes, int bytesToSend)
		{
			bool result;
			if (bytesToSend >= 65535)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ChannelBuffer:SendBytes cannot send packet larger than " + ushort.MaxValue + " bytes");
				}
				result = false;
			}
			else if (bytesToSend <= 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ChannelBuffer:SendBytes cannot send zero bytes");
				}
				result = false;
			}
			else if (bytesToSend > this.m_MaxPacketSize)
			{
				if (this.m_AllowFragmentation)
				{
					result = this.SendFragmentBytes(bytes, bytesToSend);
				}
				else
				{
					if (LogFilter.logError)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Failed to send big message of ",
							bytesToSend,
							" bytes. The maximum is ",
							this.m_MaxPacketSize,
							" bytes on channel:",
							this.m_ChannelId
						}));
					}
					result = false;
				}
			}
			else if (!this.m_CurrentPacket.HasSpace(bytesToSend))
			{
				if (this.m_IsReliable)
				{
					if (this.m_PendingPackets.Count == 0)
					{
						if (!this.m_CurrentPacket.SendToTransport(this.m_Connection, (int)this.m_ChannelId))
						{
							this.QueuePacket();
						}
						this.m_CurrentPacket.Write(bytes, bytesToSend);
						result = true;
					}
					else if (this.m_PendingPackets.Count >= this.m_MaxPendingPacketCount)
					{
						if (!this.m_IsBroken)
						{
							if (LogFilter.logError)
							{
								Debug.LogError("ChannelBuffer buffer limit of " + this.m_PendingPackets.Count + " packets reached.");
							}
						}
						this.m_IsBroken = true;
						result = false;
					}
					else
					{
						this.QueuePacket();
						this.m_CurrentPacket.Write(bytes, bytesToSend);
						result = true;
					}
				}
				else if (!this.m_CurrentPacket.SendToTransport(this.m_Connection, (int)this.m_ChannelId))
				{
					if (LogFilter.logError)
					{
						Debug.Log("ChannelBuffer SendBytes no space on unreliable channel " + this.m_ChannelId);
					}
					result = false;
				}
				else
				{
					this.m_CurrentPacket.Write(bytes, bytesToSend);
					result = true;
				}
			}
			else
			{
				this.m_CurrentPacket.Write(bytes, bytesToSend);
				result = (this.maxDelay != 0f || this.SendInternalBuffer());
			}
			return result;
		}

		private void QueuePacket()
		{
			ChannelBuffer.pendingPacketCount++;
			this.m_PendingPackets.Enqueue(this.m_CurrentPacket);
			this.m_CurrentPacket = this.AllocPacket();
		}

		private ChannelPacket AllocPacket()
		{
			ChannelPacket result;
			if (ChannelBuffer.s_FreePackets.Count == 0)
			{
				result = new ChannelPacket(this.m_MaxPacketSize, this.m_IsReliable);
			}
			else
			{
				ChannelPacket channelPacket = ChannelBuffer.s_FreePackets[ChannelBuffer.s_FreePackets.Count - 1];
				ChannelBuffer.s_FreePackets.RemoveAt(ChannelBuffer.s_FreePackets.Count - 1);
				channelPacket.Reset();
				result = channelPacket;
			}
			return result;
		}

		private static void FreePacket(ChannelPacket packet)
		{
			if (ChannelBuffer.s_FreePackets.Count < 512)
			{
				ChannelBuffer.s_FreePackets.Add(packet);
			}
		}

		public bool SendInternalBuffer()
		{
			bool result;
			if (this.m_IsReliable && this.m_PendingPackets.Count > 0)
			{
				while (this.m_PendingPackets.Count > 0)
				{
					ChannelPacket channelPacket = this.m_PendingPackets.Dequeue();
					if (!channelPacket.SendToTransport(this.m_Connection, (int)this.m_ChannelId))
					{
						this.m_PendingPackets.Enqueue(channelPacket);
						break;
					}
					ChannelBuffer.pendingPacketCount--;
					ChannelBuffer.FreePacket(channelPacket);
					if (this.m_IsBroken && this.m_PendingPackets.Count < this.m_MaxPendingPacketCount / 2)
					{
						if (LogFilter.logWarn)
						{
							Debug.LogWarning("ChannelBuffer recovered from overflow but data was lost.");
						}
						this.m_IsBroken = false;
					}
				}
				result = true;
			}
			else
			{
				result = this.m_CurrentPacket.SendToTransport(this.m_Connection, (int)this.m_ChannelId);
			}
			return result;
		}
	}
}
