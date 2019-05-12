using Neptune.Common;
using System;
using System.IO;
using System.Threading;
using WebSocketSharp;

namespace Neptune.Cloud.Core
{
	public class NpCloudWebSocketSystem : INpCloudSocketSystem
	{
		private WebSocket mSocket;

		private bool mIsOpen;

		private bool mIsError;

		private bool mIsLogin;

		private string mErrorMsg = string.Empty;

		public NpCloudWebSocketSystem(NpCloudSetting setting, string hashKey) : base(hashKey)
		{
			this.mSetting = setting;
		}

		public override bool IsConnected
		{
			get
			{
				return this.mIsOpen && this.mIsLogin && !this.mIsError;
			}
		}

		public void OnOpen(object sender, EventArgs e)
		{
			this.mIsOpen = true;
		}

		public void OnError(object sender, WebSocketSharp.ErrorEventArgs e)
		{
			this.mIsError = true;
			this.mErrorMsg = e.Message;
		}

		public void OnClose(object sender, CloseEventArgs e)
		{
		}

		public void OnMessage(object sender, MessageEventArgs e)
		{
			try
			{
				if (!this.mIsLogin)
				{
					int num = e.RawData.Length - 10;
					byte[] array = new byte[num];
					Array.Copy(e.RawData, 10, array, 0, num);
					NpCloudResponseParameter<string> npCloudResponseParameter = NpMessagePack.Unpack<NpCloudResponseParameter<string>>(array);
					if (npCloudResponseParameter.body.IndexOf("200") < 0)
					{
						this.mIsError = true;
						this.mErrorMsg = "サーバーへの接続に失敗しました。";
					}
					else
					{
						this.mIsLogin = true;
					}
				}
				else
				{
					this.Receive(e.RawData);
				}
			}
			catch (Exception ex)
			{
				this.mIsError = true;
				this.mErrorMsg = ex.Message + "\n" + ex.StackTrace;
			}
		}

		private void Receive(byte[] buffer)
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(buffer, 0, buffer.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			uint num = base.Header(memoryStream);
			uint num2 = num - 10u;
			byte[] array = new byte[num2];
			if (memoryStream.Read(array, 0, array.Length) == 0 && num2 == 0u)
			{
				return;
			}
		}

		public override bool Connect(int timeOut, object option)
		{
			this.mIsLogin = false;
			this.mIsOpen = false;
			this.mIsError = false;
			this.mErrorMsg = string.Empty;
			if (this.mSocket == null)
			{
				this.mSocket = new WebSocket(this.mSetting.Url, new string[0]);
				this.mSocket.OnMessage += this.OnMessage;
				this.mSocket.OnOpen += this.OnOpen;
				this.mSocket.OnError += this.OnError;
				this.mSocket.OnClose += this.OnClose;
			}
			this.ConnectRequest();
			this.LoginRequest();
			base.CreateReceiveLoop();
			return true;
		}

		private void ConnectRequest()
		{
			this.mSocket.Connect();
			int tickCount = Environment.TickCount;
			while (!this.mIsOpen)
			{
				if (Environment.TickCount > tickCount + base.PongEndCount * 1000 || this.mIsError)
				{
					if (this.mIsError)
					{
						throw new Exception(this.mErrorMsg);
					}
					throw new NpCloudException(720, "接続リクエストのタイムアウトです。");
				}
			}
		}

		private void LoginRequest()
		{
			byte[] data = base.CreateLoginParameter(this.mSetting, null);
			this.mSocket.Send(data);
			int tickCount = Environment.TickCount;
			while (!this.mIsLogin)
			{
				if (Environment.TickCount > tickCount + base.PongEndCount * 1000 || this.mIsError)
				{
					if (this.mIsError)
					{
						throw new Exception(this.mErrorMsg);
					}
					throw new NpCloudException(720, "接続リクエストのタイムアウトです。");
				}
			}
		}

		protected override void SocketThread()
		{
			while (this.IsConnected)
			{
				if (Environment.TickCount < this.mPongTime + base.PongIntervalTime)
				{
					return;
				}
				WebSocket obj = this.mSocket;
				lock (obj)
				{
					if (!this.mSocket.Ping())
					{
						this.mPongCount++;
					}
				}
				this.mPongTime = Environment.TickCount;
				if (base.PongEndCount < this.mPongCount)
				{
					NpCloudException e = new NpCloudException(750, "Pongにより切断された");
					base.ReceiveException(e);
					this.mIsError = false;
				}
				Thread.Sleep(50);
			}
		}

		protected override void Request(byte[] buffer)
		{
			this.mSocket.Send(buffer);
		}

		protected override void Exit(short exitCode)
		{
			base.ExitReceiveLoop();
			WebSocket obj = this.mSocket;
			lock (obj)
			{
				if (this.mSocket != null)
				{
					this.mSocket.CloseAsync();
				}
				this.mSetting = null;
			}
		}
	}
}
