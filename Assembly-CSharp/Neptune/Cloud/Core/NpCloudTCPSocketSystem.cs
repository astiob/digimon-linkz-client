using Neptune.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Neptune.Cloud.Core
{
	public class NpCloudTCPSocketSystem : INpCloudSocketSystem
	{
		private Socket mSocket;

		private bool mIsReceiveThread;

		private readonly object mLockObject = new object();

		private byte[] mReceivebuffer;

		private MemoryStream mReceponseStream;

		private long mWritePoint;

		private long mReadPoint;

		private uint mReceponseSize;

		private byte[] mHeaderBuffer;

		private byte mReceponseType;

		private byte[] mReceponseData;

		private bool mIsReceponse;

		public NpCloudTCPSocketSystem(NpCloudSetting setting, string hashKey) : base(hashKey)
		{
			this.mSetting = setting;
		}

		public override bool IsConnected
		{
			get
			{
				return this.mSocket.Connected;
			}
		}

		public override bool Connect(int timeOut, object option)
		{
			object obj = this.mLockObject;
			lock (obj)
			{
				this.SocketConnect();
				this.LoginRequest(option);
				this.LoginResponse(timeOut);
				this.mIsReceiveThread = true;
				base.CreateReceiveLoop();
			}
			return true;
		}

		private void SocketConnect()
		{
			Exception ex = null;
			for (int i = 0; i < this.mSetting.HostAddress.Length; i++)
			{
				try
				{
					if (this.mSocket == null)
					{
						this.mSocket = new Socket(this.mSetting.HostAddress[i].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					}
					this.mSocket.Connect(this.mSetting.HostAddress, this.mSetting.Port);
					this.mSocket.NoDelay = true;
					ex = null;
					break;
				}
				catch (SocketException ex2)
				{
					this.mSocket = null;
					ex = ex2;
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			if (this.mSocket == null || !this.mSocket.Connected)
			{
				throw new NpCloudException(711, "Socketの接続に失敗しました。");
			}
		}

		private void LoginRequest(object option)
		{
			byte[] buf = base.CreateLoginParameter(this.mSetting, option);
			this.mSocket.Send(buf);
		}

		private void LoginResponse(int timeOut)
		{
			if (!this.mSocket.Poll(timeOut * 1000000, SelectMode.SelectRead))
			{
				throw new NpCloudException(720, "接続リクエストのタイムアウトです。");
			}
			byte[] array = new byte[1024];
			this.mSocket.Receive(array);
			int num = array.Length - 10;
			byte[] array2 = new byte[num];
			Array.Copy(array, 10, array2, 0, num);
			NpCloudResponseParameter<string> npCloudResponseParameter = NpMessagePack.Unpack<NpCloudResponseParameter<string>>(array2);
			if (npCloudResponseParameter.body.IndexOf("200") < 0)
			{
				throw new NpCloudException(721, "サーバーへの接続に失敗しました。");
			}
		}

		protected override void SocketThread()
		{
			try
			{
				this.mReceivebuffer = new byte[1024];
				if (this.mReceponseStream != null)
				{
					this.mReceponseStream.Dispose();
				}
				this.mReceponseStream = new MemoryStream();
				this.mWritePoint = 0L;
				this.mReadPoint = 0L;
				this.mReceponseSize = 0u;
				this.mHeaderBuffer = new byte[10];
				this.mReceponseType = 0;
				this.mIsReceponse = false;
				byte[] tmpBuffer = null;
				while (this.mIsReceiveThread)
				{
					object obj = this.mLockObject;
					lock (obj)
					{
						if (this.mSocket.Poll(0, SelectMode.SelectWrite))
						{
							this.PingPong();
							this.SendLoop(tmpBuffer);
						}
						this.Receive();
					}
					while (this.mIsReceiveThread)
					{
						if (this.Receponse())
						{
							break;
						}
					}
					Thread.Sleep(50);
				}
			}
			catch (NpCloudException e)
			{
				base.ReceiveException(e);
			}
			catch (SocketException ex)
			{
				short exitCode = 752;
				string message = string.Format("Message = {0}\nStackTrace = {1}", ex.Message, ex.StackTrace);
				NpCloudException e2 = new NpCloudException(exitCode, message);
				base.ReceiveException(e2);
			}
			catch (Exception ex2)
			{
				short exitCode2 = 753;
				NpCloudException e3 = new NpCloudException(exitCode2, ex2.Message);
				base.ReceiveException(e3);
			}
		}

		private void SendLoop(byte[] tmpBuffer)
		{
			object obj = this.mLockObject;
			lock (obj)
			{
				tmpBuffer = null;
				while (this.mRequestQueue.Read(out tmpBuffer))
				{
					this.mSocket.Send(tmpBuffer);
				}
			}
		}

		private void Receive()
		{
			if (this.mSocket.Poll(0, SelectMode.SelectRead))
			{
				this.mReceponseStream.Seek(this.mWritePoint, SeekOrigin.Begin);
				while (this.mSocket.Available > 0)
				{
					int count = this.mSocket.Receive(this.mReceivebuffer);
					this.mReceponseStream.Write(this.mReceivebuffer, 0, count);
				}
				this.mWritePoint = this.mReceponseStream.Position;
			}
		}

		private bool Receponse()
		{
			if (!this.ReceponseHeader())
			{
				return true;
			}
			if (!this.ReceponseData())
			{
				return true;
			}
			this.mIsReceponse = false;
			this.mReceponseSize = 0u;
			if (this.mReceponseStream.Position >= this.mReceponseStream.Length)
			{
				this.mReceponseStream.Seek(0L, SeekOrigin.Begin);
				this.mReceponseStream.SetLength(0L);
				this.mReadPoint = 0L;
				this.mWritePoint = 0L;
				return true;
			}
			return false;
		}

		private bool ReceponseHeader()
		{
			if (this.mIsReceponse)
			{
				return true;
			}
			this.mReceponseStream.Seek(this.mReadPoint, SeekOrigin.Begin);
			int num = this.mReceponseStream.Read(this.mHeaderBuffer, 0, 10);
			if (num < 10)
			{
				Array.Clear(this.mHeaderBuffer, 0, 10);
				return false;
			}
			this.mReceponseSize = this.GetDataSize(this.mHeaderBuffer) - 10u;
			this.mReceponseType = this.HandleDataType(this.mHeaderBuffer);
			this.mReadPoint = this.mReceponseStream.Position;
			this.mIsReceponse = true;
			return true;
		}

		private bool ReceponseData()
		{
			if (!this.mIsReceponse)
			{
				return false;
			}
			this.mReceponseStream.Seek(this.mReadPoint, SeekOrigin.Begin);
			if (this.mReceponseType == 254)
			{
				return true;
			}
			this.mReceponseData = new byte[this.mReceponseSize];
			int num = this.mReceponseStream.Read(this.mReceponseData, 0, this.mReceponseData.Length);
			if ((long)num < (long)((ulong)this.mReceponseSize))
			{
				Array.Clear(this.mReceponseData, 0, this.mReceponseData.Length);
				return false;
			}
			this.mReadPoint = this.mReceponseStream.Position;
			List<byte> list = new List<byte>();
			list.Add(1);
			list.AddRange(this.mReceponseData);
			base.AddReceiveBuffer(list.ToArray());
			Array.Clear(this.mReceponseData, 0, this.mReceponseData.Length);
			return true;
		}

		private byte[] GetDataBuffer(MemoryStream memStream, uint dataSize, out int readSize)
		{
			byte[] result;
			try
			{
				readSize = 0;
				if (dataSize <= 0u)
				{
					result = null;
				}
				else
				{
					byte[] array = new byte[dataSize - 10u];
					readSize = memStream.Read(array, 0, array.Length);
					if (readSize <= 0)
					{
						result = null;
					}
					else
					{
						result = array;
					}
				}
			}
			catch (OutOfMemoryException ex)
			{
				readSize = 0;
				throw ex;
			}
			return result;
		}

		private uint GetDataSize(byte[] headerBuffer)
		{
			byte[] array = new byte[4];
			Array.Copy(headerBuffer, array, 4);
			return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array, 0));
		}

		private byte HandleDataType(byte[] header)
		{
			byte b = header[4];
			if (b == 254)
			{
				this.mIsPong = true;
			}
			return b;
		}

		protected override void Request(byte[] buffer)
		{
			this.mRequestQueue.Write(buffer);
		}

		protected override void Exit(short exitCode)
		{
			this.mIsReceiveThread = false;
			base.ExitReceiveLoop();
			this.mReceponseStream.Dispose();
			this.SocketExit();
			this.mSetting = null;
		}

		private void SocketExit()
		{
			if (this.mSocket == null)
			{
				return;
			}
			object obj = this.mLockObject;
			lock (obj)
			{
				if (this.mReceiveQueue != null)
				{
					this.mReceiveQueue.Clear();
				}
				if (this.mRequestQueue != null)
				{
					this.mRequestQueue.Clear();
				}
				if (this.mSocket.Connected)
				{
					try
					{
						this.mSocket.Shutdown(SocketShutdown.Both);
					}
					catch
					{
						this.mSocket = null;
						return;
					}
				}
				this.mSocket.Close();
				this.mSocket = null;
			}
		}

		private void PingPong()
		{
			if (Environment.TickCount < this.mPongTime + base.PongIntervalTime)
			{
				return;
			}
			if (this.mIsPong)
			{
				this.mPongCount = 0;
				this.mSocket.Send(this.CreatePingMessage());
			}
			else
			{
				this.mPongCount++;
			}
			this.mIsPong = false;
			this.mPongTime = Environment.TickCount;
			if (base.PongEndCount < this.mPongCount)
			{
				NpCloudException e = new NpCloudException(750, "Pongにより切断された");
				base.ReceiveException(e);
				this.mIsReceiveThread = false;
			}
		}

		private byte[] CreatePingMessage()
		{
			uint value = (uint)IPAddress.HostToNetworkOrder(10);
			byte[] sourceArray = new byte[4];
			sourceArray = BitConverter.GetBytes(value);
			byte[] array = new byte[10];
			Array.Copy(sourceArray, array, 4);
			array[4] = byte.MaxValue;
			return array;
		}
	}
}
