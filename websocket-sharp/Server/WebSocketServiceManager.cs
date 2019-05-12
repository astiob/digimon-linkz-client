using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;

namespace WebSocketSharp.Server
{
	public class WebSocketServiceManager
	{
		private Dictionary<string, WebSocketServiceHost> _hosts;

		private volatile bool _keepClean;

		private Logger _logger;

		private volatile ServerState _state;

		private object _sync;

		internal WebSocketServiceManager() : this(new Logger())
		{
		}

		internal WebSocketServiceManager(Logger logger)
		{
			this._logger = logger;
			this._hosts = new Dictionary<string, WebSocketServiceHost>();
			this._keepClean = true;
			this._state = ServerState.Ready;
			this._sync = new object();
		}

		public int Count
		{
			get
			{
				object sync = this._sync;
				int count;
				lock (sync)
				{
					count = this._hosts.Count;
				}
				return count;
			}
		}

		public IEnumerable<WebSocketServiceHost> Hosts
		{
			get
			{
				object sync = this._sync;
				IEnumerable<WebSocketServiceHost> result;
				lock (sync)
				{
					result = this._hosts.Values.ToList<WebSocketServiceHost>();
				}
				return result;
			}
		}

		public WebSocketServiceHost this[string path]
		{
			get
			{
				WebSocketServiceHost result;
				this.TryGetServiceHost(path, out result);
				return result;
			}
		}

		public bool KeepClean
		{
			get
			{
				return this._keepClean;
			}
			internal set
			{
				object sync = this._sync;
				lock (sync)
				{
					if (value ^ this._keepClean)
					{
						this._keepClean = value;
						foreach (WebSocketServiceHost webSocketServiceHost in this._hosts.Values)
						{
							webSocketServiceHost.KeepClean = value;
						}
					}
				}
			}
		}

		public IEnumerable<string> Paths
		{
			get
			{
				object sync = this._sync;
				IEnumerable<string> result;
				lock (sync)
				{
					result = this._hosts.Keys.ToList<string>();
				}
				return result;
			}
		}

		public int SessionCount
		{
			get
			{
				int num = 0;
				foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
				{
					if (this._state != ServerState.Start)
					{
						break;
					}
					num += webSocketServiceHost.Sessions.Count;
				}
				return num;
			}
		}

		private void broadcast(Opcode opcode, byte[] data, Action completed)
		{
			Dictionary<CompressionMethod, byte[]> dictionary = new Dictionary<CompressionMethod, byte[]>();
			try
			{
				foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
				{
					if (this._state != ServerState.Start)
					{
						break;
					}
					webSocketServiceHost.Sessions.Broadcast(opcode, data, dictionary);
				}
				if (completed != null)
				{
					completed();
				}
			}
			catch (Exception ex)
			{
				this._logger.Fatal(ex.ToString());
			}
			finally
			{
				dictionary.Clear();
			}
		}

		private void broadcast(Opcode opcode, Stream stream, Action completed)
		{
			Dictionary<CompressionMethod, Stream> dictionary = new Dictionary<CompressionMethod, Stream>();
			try
			{
				foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
				{
					if (this._state != ServerState.Start)
					{
						break;
					}
					webSocketServiceHost.Sessions.Broadcast(opcode, stream, dictionary);
				}
				if (completed != null)
				{
					completed();
				}
			}
			catch (Exception ex)
			{
				this._logger.Fatal(ex.ToString());
			}
			finally
			{
				foreach (Stream stream2 in dictionary.Values)
				{
					stream2.Dispose();
				}
				dictionary.Clear();
			}
		}

		private void broadcastAsync(Opcode opcode, byte[] data, Action completed)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				this.broadcast(opcode, data, completed);
			});
		}

		private void broadcastAsync(Opcode opcode, Stream stream, Action completed)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				this.broadcast(opcode, stream, completed);
			});
		}

		private Dictionary<string, Dictionary<string, bool>> broadping(byte[] frame, int millisecondsTimeout)
		{
			Dictionary<string, Dictionary<string, bool>> dictionary = new Dictionary<string, Dictionary<string, bool>>();
			foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
			{
				if (this._state != ServerState.Start)
				{
					break;
				}
				dictionary.Add(webSocketServiceHost.Path, webSocketServiceHost.Sessions.Broadping(frame, millisecondsTimeout));
			}
			return dictionary;
		}

		internal void Add(string path, WebSocketServiceHost host)
		{
			object sync = this._sync;
			lock (sync)
			{
				WebSocketServiceHost webSocketServiceHost;
				if (this._hosts.TryGetValue(path, out webSocketServiceHost))
				{
					this._logger.Error("A WebSocket service with the specified path already exists.\npath: " + path);
				}
				else
				{
					if (this._state == ServerState.Start)
					{
						host.Sessions.Start();
					}
					this._hosts.Add(path, host);
				}
			}
		}

		internal bool Remove(string path)
		{
			path = HttpUtility.UrlDecode(path).TrimEndSlash();
			object sync = this._sync;
			WebSocketServiceHost webSocketServiceHost;
			lock (sync)
			{
				if (!this._hosts.TryGetValue(path, out webSocketServiceHost))
				{
					this._logger.Error("A WebSocket service with the specified path not found.\npath: " + path);
					return false;
				}
				this._hosts.Remove(path);
			}
			if (webSocketServiceHost.Sessions.State == ServerState.Start)
			{
				webSocketServiceHost.Sessions.Stop(1001.ToByteArrayInternally(ByteOrder.Big), true);
			}
			return true;
		}

		internal void Start()
		{
			object sync = this._sync;
			lock (sync)
			{
				foreach (WebSocketServiceHost webSocketServiceHost in this._hosts.Values)
				{
					webSocketServiceHost.Sessions.Start();
				}
				this._state = ServerState.Start;
			}
		}

		internal void Stop(byte[] data, bool send)
		{
			object sync = this._sync;
			lock (sync)
			{
				this._state = ServerState.ShuttingDown;
				PayloadData payload = new PayloadData(data);
				CloseEventArgs args = new CloseEventArgs(payload);
				byte[] frame = (!send) ? null : WsFrame.CreateCloseFrame(Mask.Unmask, payload).ToByteArray();
				foreach (WebSocketServiceHost webSocketServiceHost in this._hosts.Values)
				{
					webSocketServiceHost.Sessions.Stop(args, frame);
				}
				this._hosts.Clear();
				this._state = ServerState.Stop;
			}
		}

		internal bool TryGetServiceHostInternally(string path, out WebSocketServiceHost host)
		{
			path = HttpUtility.UrlDecode(path).TrimEndSlash();
			object sync = this._sync;
			bool flag;
			lock (sync)
			{
				flag = this._hosts.TryGetValue(path, out host);
			}
			if (!flag)
			{
				this._logger.Error("A WebSocket service with the specified path not found.\npath: " + path);
			}
			return flag;
		}

		public void Broadcast(byte[] data)
		{
			string text = this._state.CheckIfStart() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				return;
			}
			if (data.LongLength <= 1016L)
			{
				this.broadcast(Opcode.Binary, data, null);
			}
			else
			{
				this.broadcast(Opcode.Binary, new MemoryStream(data), null);
			}
		}

		public void Broadcast(string data)
		{
			string text = this._state.CheckIfStart() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			if (bytes.LongLength <= 1016L)
			{
				this.broadcast(Opcode.Text, bytes, null);
			}
			else
			{
				this.broadcast(Opcode.Text, new MemoryStream(bytes), null);
			}
		}

		public void BroadcastAsync(byte[] data, Action completed)
		{
			string text = this._state.CheckIfStart() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				return;
			}
			if (data.LongLength <= 1016L)
			{
				this.broadcastAsync(Opcode.Binary, data, completed);
			}
			else
			{
				this.broadcastAsync(Opcode.Binary, new MemoryStream(data), completed);
			}
		}

		public void BroadcastAsync(string data, Action completed)
		{
			string text = this._state.CheckIfStart() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			if (bytes.LongLength <= 1016L)
			{
				this.broadcastAsync(Opcode.Text, bytes, completed);
			}
			else
			{
				this.broadcastAsync(Opcode.Text, new MemoryStream(bytes), completed);
			}
		}

		public void BroadcastAsync(Stream stream, int length, Action completed)
		{
			string text;
			if ((text = this._state.CheckIfStart()) == null)
			{
				text = (stream.CheckIfCanRead() ?? ((length >= 1) ? null : "'length' must be greater than 0."));
			}
			string text2 = text;
			if (text2 != null)
			{
				this._logger.Error(text2);
				return;
			}
			stream.ReadBytesAsync(length, delegate(byte[] data)
			{
				int num = data.Length;
				if (num == 0)
				{
					this._logger.Error("A data cannot be read from 'stream'.");
					return;
				}
				if (num < length)
				{
					this._logger.Warn(string.Format("A data with 'length' cannot be read from 'stream'.\nexpected: {0} actual: {1}", length, num));
				}
				if (num <= 1016)
				{
					this.broadcast(Opcode.Binary, data, completed);
				}
				else
				{
					this.broadcast(Opcode.Binary, new MemoryStream(data), completed);
				}
			}, delegate(Exception ex)
			{
				this._logger.Fatal(ex.ToString());
			});
		}

		public Dictionary<string, Dictionary<string, bool>> Broadping()
		{
			string text = this._state.CheckIfStart();
			if (text != null)
			{
				this._logger.Error(text);
				return null;
			}
			return this.broadping(WsFrame.EmptyUnmaskPingData, 1000);
		}

		public Dictionary<string, Dictionary<string, bool>> Broadping(string message)
		{
			if (message == null || message.Length == 0)
			{
				return this.Broadping();
			}
			byte[] data = null;
			string text;
			if ((text = this._state.CheckIfStart()) == null)
			{
				text = (data = Encoding.UTF8.GetBytes(message)).CheckIfValidControlData("message");
			}
			string text2 = text;
			if (text2 != null)
			{
				this._logger.Error(text2);
				return null;
			}
			return this.broadping(WsFrame.CreatePingFrame(Mask.Unmask, data).ToByteArray(), 1000);
		}

		public bool TryGetServiceHost(string path, out WebSocketServiceHost host)
		{
			string text = this._state.CheckIfStart() ?? path.CheckIfValidServicePath();
			if (text != null)
			{
				this._logger.Error(text);
				host = null;
				return false;
			}
			return this.TryGetServiceHostInternally(path, out host);
		}
	}
}
