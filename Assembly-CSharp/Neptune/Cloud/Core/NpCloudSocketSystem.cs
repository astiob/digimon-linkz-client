using System;

namespace Neptune.Cloud.Core
{
	public class NpCloudSocketSystem
	{
		private INpCloudSocketSystem mSocketSystem;

		private NpCloudSetting mSetting;

		private static NpCloudSocketSystem instance;

		private NpCloudSocketSystem(string url, NpCloudSocketType type, string hashKey)
		{
			this.mSetting = new NpCloudSetting(type, url);
			if (type != NpCloudSocketType.TCP)
			{
				if (type == NpCloudSocketType.Web)
				{
					this.mSocketSystem = new NpCloudWebSocketSystem(this.mSetting, hashKey);
				}
			}
			else
			{
				this.mSocketSystem = new NpCloudTCPSocketSystem(this.mSetting, hashKey);
			}
		}

		public uint UserID
		{
			get
			{
				return this.mSocketSystem.UserID;
			}
		}

		public bool IsConnected
		{
			get
			{
				return this.mSocketSystem.IsConnected;
			}
		}

		public int PongEndCount
		{
			get
			{
				return this.mSocketSystem.PongEndCount;
			}
			set
			{
				this.mSocketSystem.PongEndCount = value;
			}
		}

		public int PongIntervalTime
		{
			get
			{
				return this.mSocketSystem.PongIntervalTime;
			}
			set
			{
				this.mSocketSystem.PongIntervalTime = value;
			}
		}

		public static void CreateInstance(string url, NpCloudSocketType type, string hashKey)
		{
			if (NpCloudSocketSystem.instance != null)
			{
				return;
			}
			NpCloudSocketSystem.instance = new NpCloudSocketSystem(url, type, hashKey);
		}

		public static NpCloudSocketSystem GetInstance()
		{
			return NpCloudSocketSystem.instance;
		}

		public static void DeleteInstance(Action exitAction)
		{
			if (NpCloudSocketSystem.instance != null)
			{
				NpCloudSocketSystem.instance.mSocketSystem.Exit(0, exitAction);
				NpCloudSocketSystem.instance.mSocketSystem.DeleteInstance();
				NpCloudSocketSystem.instance.mSocketSystem = null;
			}
			else
			{
				exitAction();
			}
			NpCloudSocketSystem.instance = null;
		}

		public bool Connect(int timeOut, object option)
		{
			return this.mSocketSystem.Connect(timeOut, option);
		}

		public void ConnectAsync(int timeOut, Action<bool, Exception> result, object option)
		{
			this.mSocketSystem.ConnectAsync(timeOut, result, option);
		}

		public void Request(NpCloudServerData param)
		{
			this.mSocketSystem.Request(param);
		}

		public uint SequenceRequest(uint uid, NpCloudServerData param)
		{
			return this.mSocketSystem.SequenceRequest(uid, param);
		}

		public T GetRequestParameter<T>(out byte[] buffer) where T : class
		{
			return this.mSocketSystem.ReadData<T>(out buffer);
		}

		public T Deserialize<T>(string buffer) where T : class
		{
			return this.mSocketSystem.Deserialize<T>(buffer);
		}
	}
}
