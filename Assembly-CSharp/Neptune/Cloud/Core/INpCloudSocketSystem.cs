using Neptune.Common;
using Neptune.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Neptune.Cloud.Core
{
	public abstract class INpCloudSocketSystem
	{
		private readonly object EXCEPTION_LOCK = new object();

		protected const int MS_TO_MINITSU = 1000;

		protected const int RECEIVE_BUFFER_SIZE = 1024;

		private readonly string HASH_KEY = string.Empty;

		protected NpCloudSetting mSetting;

		protected int mTimeOut = 1;

		private Thread mConnectThread;

		private Thread mReceiveThread;

		protected NpQueue mReceiveQueue;

		protected NpQueue mRequestQueue;

		protected bool mIsPong = true;

		protected int mPongTime = Environment.TickCount;

		protected int mPongCount;

		private uint mRequestCounter;

		private NpCloudException mException;

		public INpCloudSocketSystem(string hashKey)
		{
			this.HASH_KEY = hashKey;
			this.mReceiveQueue = new NpQueue();
			this.mRequestQueue = new NpQueue();
			this.mRequestCounter = 0u;
			this.PongIntervalTime = 1000;
			this.PongEndCount = 10;
		}

		public int PongEndCount { get; set; }

		public int PongIntervalTime { get; set; }

		public uint UserID
		{
			get
			{
				return this.mSetting.UserId;
			}
		}

		public virtual bool IsConnected
		{
			get
			{
				throw new Exception("IsConnected 未作成");
			}
		}

		public void DeleteInstance()
		{
			this.mRequestCounter = 0u;
		}

		public void ConnectAsync(int timeOut, Action<bool, Exception> result, object option)
		{
			this.mTimeOut = timeOut;
			List<object> list = new List<object>();
			list.Add(result);
			list.Add(option);
			this.mConnectThread = new Thread(new ParameterizedThreadStart(this.ConnectThread));
			this.mConnectThread.IsBackground = true;
			this.mConnectThread.Name = "ConnectThread";
			this.mConnectThread.Start(list);
		}

		private void ConnectThread(object obj)
		{
			object exception_LOCK = this.EXCEPTION_LOCK;
			lock (exception_LOCK)
			{
				this.mException = null;
			}
			List<object> list = (List<object>)obj;
			Action<bool, Exception> action = (Action<bool, Exception>)list[0];
			try
			{
				if (this.Connect(this.mTimeOut, list[1]))
				{
					action(true, null);
				}
				else
				{
					action(false, null);
				}
			}
			catch (NpCloudException arg)
			{
				action(false, arg);
			}
			catch (Exception arg2)
			{
				action(false, arg2);
			}
		}

		protected byte[] CreateLoginParameter(NpCloudSetting setting, object option)
		{
			string[] array = setting.ProjectId.Split(new char[]
			{
				'.'
			});
			string str = setting.ProjectId;
			if (array.Length > 1)
			{
				str = array[1];
			}
			NpCloudRequestParameter npCloudRequestParameter = new NpCloudRequestParameter();
			npCloudRequestParameter.content = new Dictionary<string, object>
			{
				{
					"projectId",
					setting.ProjectId
				},
				{
					"projectPass",
					str + "pass"
				},
				{
					"userId",
					setting.UserId
				}
			};
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("value", NpCloudCommandType.login.ToString());
			if (option != null)
			{
				dictionary.Add("option", option);
			}
			npCloudRequestParameter.command = dictionary;
			npCloudRequestParameter.type = "direct";
			MemoryStream memoryStream = new MemoryStream();
			NpMessagePack.Pack<object>(npCloudRequestParameter, memoryStream);
			return this.AddHeaderBuffer(memoryStream.GetBuffer(), NpCloudHeaderType.CommonMsg);
		}

		protected void CreateReceiveLoop()
		{
			this.mReceiveThread = new Thread(new ThreadStart(this.SocketThread));
			this.mReceiveThread.Name = "ReceiveThread";
			this.mReceiveThread.IsBackground = true;
			this.mReceiveThread.Start();
		}

		protected void ExitReceiveLoop()
		{
			if (this.mReceiveThread != null)
			{
				this.mReceiveThread.Join();
			}
			this.mReceiveThread = null;
		}

		protected virtual void SocketThread()
		{
			throw new Exception("ReceiveThread 未作成");
		}

		private NpCloudRequestParameter CreateParameter(NpCloudServerData param)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("value", param.value.ToString());
			if (param.ctrl != null && !param.ctrl.Equals(string.Empty))
			{
				dictionary.Add("ctrl", param.ctrl);
			}
			return new NpCloudRequestParameter
			{
				command = dictionary,
				content = param.content,
				type = param.type.ToString()
			};
		}

		public void Request(NpCloudServerData param)
		{
			byte[] buffer = this.CreateRequestBuffer(param);
			this.Request(buffer);
		}

		private byte[] CreateRequestBuffer(NpCloudServerData param)
		{
			byte[] array = this.CreateSystemBuffer(this.CreateParameter(param), NpCloudHeaderType.CommonMsg);
			if (array.Length <= 0)
			{
				throw new NpCloudException(731, "送信データのバッファが異常でした");
			}
			return array;
		}

		private byte[] CreateSystemBuffer(NpCloudRequestParameter parameter, NpCloudHeaderType type)
		{
			MemoryStream memoryStream = new MemoryStream();
			NpMessagePack.Pack<object>(parameter, memoryStream);
			return this.AddHeaderBuffer(memoryStream.GetBuffer(), type);
		}

		public uint SequenceRequest(uint uid, NpCloudServerData param)
		{
			byte[] buffer = this.CreateRequestBuffer(uid, param);
			this.Request(buffer);
			return (uid != 0u) ? uid : this.mRequestCounter;
		}

		private byte[] CreateRequestBuffer(uint uid, NpCloudServerData param)
		{
			byte[] array = this.CreateSystemBuffer(uid, this.CreateParameter(param), NpCloudHeaderType.CommonMsg);
			if (array.Length <= 0)
			{
				throw new NpCloudException(731, "送信データのバッファが異常でした");
			}
			return array;
		}

		private byte[] CreateSystemBuffer(uint uid, NpCloudRequestParameter parameter, NpCloudHeaderType type)
		{
			MemoryStream memoryStream = new MemoryStream();
			this.Serialize(parameter, uid, memoryStream);
			return this.AddHeaderBuffer(memoryStream.GetBuffer(), type);
		}

		public void Exit(short exitCode, Action action)
		{
			this.Exit(exitCode);
			action();
		}

		public T ReadData<T>(out byte[] buffer) where T : class
		{
			object exception_LOCK = this.EXCEPTION_LOCK;
			lock (exception_LOCK)
			{
				if (this.mException != null)
				{
					throw this.mException;
				}
			}
			NpCloudReceponseQueueType npCloudReceponseQueueType = (NpCloudReceponseQueueType)this.LoadQueu(out buffer);
			if (npCloudReceponseQueueType == NpCloudReceponseQueueType.None)
			{
				return (T)((object)null);
			}
			if (npCloudReceponseQueueType != NpCloudReceponseQueueType.Message)
			{
				throw new NpCloudException(701, "Switchのcaseがdefaultだったよ = {0}");
			}
			return this.ReceiveAction<T>(buffer);
		}

		protected void AddReceiveBuffer(byte[] buffer)
		{
			this.mReceiveQueue.Write(buffer);
		}

		private byte LoadQueu(out byte[] buffer)
		{
			if (!this.mReceiveQueue.Read(out buffer))
			{
				return 0;
			}
			byte result = buffer[0];
			List<byte> list = new List<byte>(buffer);
			list.RemoveAt(0);
			buffer = list.ToArray();
			return result;
		}

		private T ReceiveAction<T>(byte[] tmpBuffer) where T : class
		{
			return NpMessagePack.Unpack<T>(tmpBuffer);
		}

		public virtual bool Connect(int timeOut, object option)
		{
			throw new Exception("Connect 未作成");
		}

		protected virtual void Request(byte[] buffer)
		{
			throw new Exception("Request 未作成");
		}

		protected virtual void Exit(short exitCode)
		{
			throw new Exception("Exit 未作成 " + exitCode);
		}

		private uint GetUId(uint uid)
		{
			if (uid == 0u)
			{
				this.mRequestCounter += 1u;
				return this.mRequestCounter;
			}
			return uid;
		}

		private uint Serialize(NpCloudRequestParameter data, uint uid, MemoryStream stream)
		{
			NpMessagePack.Pack<object>(data.content, stream);
			uint uid2 = this.GetUId(uid);
			byte[] bytes = BitConverter.GetBytes(uid2);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			stream.Write(bytes, 0, bytes.Length);
			HMACSHA512 hmacsha = new HMACSHA512(Encoding.ASCII.GetBytes(this.HASH_KEY));
			byte[] array = hmacsha.ComputeHash(stream.ToArray());
			stream.SetLength(stream.Length - 4L);
			stream.Write(array, 0, array.Length);
			stream.Write(bytes, 0, bytes.Length);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["data"] = Convert.ToBase64String(stream.ToArray());
			data.content = new Dictionary<string, object>
			{
				{
					"param",
					dictionary
				}
			};
			stream.Seek(0L, SeekOrigin.Begin);
			stream.SetLength(0L);
			NpMessagePack.Pack<object>(data, stream);
			return uid2;
		}

		public T Deserialize<T>(string buffer) where T : class
		{
			if (buffer == null || buffer.Equals(string.Empty))
			{
				throw new NpCloudException(780, "データが存在しませんでした。");
			}
			byte[] array = Convert.FromBase64String(buffer);
			int num = array.Length - 64;
			byte[] array2 = new byte[64];
			byte[] array3 = new byte[num];
			Array.Copy(array, num, array2, 0, 64);
			Array.Copy(array, array3, num);
			HMACSHA512 hmacsha = new HMACSHA512(Encoding.ASCII.GetBytes(this.HASH_KEY));
			string text = BitConverter.ToString(hmacsha.ComputeHash(array3));
			string value = BitConverter.ToString(array2);
			if (!text.Equals(value))
			{
				throw new NpCloudException(781, "ハッシュの値が違います、改ざんされている可能性があります。");
			}
			return NpMessagePack.Unpack<T>(array3);
		}

		protected uint Header(MemoryStream stream)
		{
			byte[] array = new byte[10];
			int num = stream.Read(array, 0, 10);
			if (num < 10)
			{
				throw new NpCloudException(754, "ヘッダーサイズ異常です");
			}
			if (array[4] == 254)
			{
				this.mIsPong = true;
			}
			byte[] array2 = new byte[4];
			Array.Copy(array, array2, 4);
			return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(array2, 0));
		}

		private byte[] AddHeaderBuffer(byte[] byteData, NpCloudHeaderType type)
		{
			int num = byteData.Length + 10;
			MemoryStream memoryStream = new MemoryStream(num);
			byte[] bytes = BitConverter.GetBytes(num);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			memoryStream.Write(bytes, 0, 4);
			memoryStream.Write(new byte[]
			{
				(byte)type
			}, 0, 1);
			memoryStream.Write(new byte[5], 0, 5);
			memoryStream.Write(byteData, 0, byteData.Length);
			return memoryStream.ToArray();
		}

		protected void ReceiveException(NpCloudException e)
		{
			object exception_LOCK = this.EXCEPTION_LOCK;
			lock (exception_LOCK)
			{
				this.mException = null;
				this.mException = e;
			}
		}
	}
}
