using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;

namespace WebSocketSharp.Server
{
	public class WebSocketSessionManager
	{
		private static readonly Dictionary<string, IWebSocketSession> _emptySessions = new Dictionary<string, IWebSocketSession>();

		private object _forSweep;

		private volatile bool _keepClean;

		private Logger _logger;

		private Dictionary<string, IWebSocketSession> _sessions;

		private volatile ServerState _state;

		private volatile bool _sweeping;

		private System.Timers.Timer _sweepTimer;

		private object _sync;

		internal WebSocketSessionManager() : this(new Logger())
		{
		}

		internal WebSocketSessionManager(Logger logger)
		{
			this._logger = logger;
			this._forSweep = new object();
			this._keepClean = true;
			this._sessions = new Dictionary<string, IWebSocketSession>();
			this._state = ServerState.Ready;
			this._sync = new object();
			this.setSweepTimer(60000.0);
		}

		internal ServerState State
		{
			get
			{
				return this._state;
			}
		}

		public IEnumerable<string> ActiveIDs
		{
			get
			{
				foreach (KeyValuePair<string, bool> result in this.Broadping(WsFrame.EmptyUnmaskPingData, 1000))
				{
					if (result.Value)
					{
						yield return result.Key;
					}
				}
				yield break;
			}
		}

		public int Count
		{
			get
			{
				object sync = this._sync;
				int count;
				lock (sync)
				{
					count = this._sessions.Count;
				}
				return count;
			}
		}

		public IEnumerable<string> IDs
		{
			get
			{
				if (this._state == ServerState.ShuttingDown)
				{
					return WebSocketSessionManager._emptySessions.Keys;
				}
				object sync = this._sync;
				IEnumerable<string> result;
				lock (sync)
				{
					result = this._sessions.Keys.ToList<string>();
				}
				return result;
			}
		}

		public IEnumerable<string> InactiveIDs
		{
			get
			{
				foreach (KeyValuePair<string, bool> result in this.Broadping(WsFrame.EmptyUnmaskPingData, 1000))
				{
					if (!result.Value)
					{
						yield return result.Key;
					}
				}
				yield break;
			}
		}

		public IWebSocketSession this[string id]
		{
			get
			{
				IWebSocketSession result;
				this.TryGetSession(id, out result);
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
				if (!(value ^ this._keepClean))
				{
					return;
				}
				this._keepClean = value;
				if (this._state == ServerState.Start)
				{
					this._sweepTimer.Enabled = value;
				}
			}
		}

		public IEnumerable<IWebSocketSession> Sessions
		{
			get
			{
				if (this._state == ServerState.ShuttingDown)
				{
					return WebSocketSessionManager._emptySessions.Values;
				}
				object sync = this._sync;
				IEnumerable<IWebSocketSession> result;
				lock (sync)
				{
					result = this._sessions.Values.ToList<IWebSocketSession>();
				}
				return result;
			}
		}

		private void broadcast(Opcode opcode, byte[] data, Action completed)
		{
			Dictionary<CompressionMethod, byte[]> dictionary = new Dictionary<CompressionMethod, byte[]>();
			try
			{
				this.Broadcast(opcode, data, dictionary);
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
				this.Broadcast(opcode, stream, dictionary);
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

		private static string createID()
		{
			return Guid.NewGuid().ToString("N");
		}

		private void setSweepTimer(double interval)
		{
			this._sweepTimer = new System.Timers.Timer(interval);
			this._sweepTimer.Elapsed += delegate(object sender, ElapsedEventArgs e)
			{
				this.Sweep();
			};
		}

		private bool tryGetSession(string id, out IWebSocketSession session)
		{
			object sync = this._sync;
			bool flag;
			lock (sync)
			{
				flag = this._sessions.TryGetValue(id, out session);
			}
			if (!flag)
			{
				this._logger.Error("A session with the specified ID not found.\nID: " + id);
			}
			return flag;
		}

		internal string Add(IWebSocketSession session)
		{
			object sync = this._sync;
			string result;
			lock (sync)
			{
				if (this._state != ServerState.Start)
				{
					result = null;
				}
				else
				{
					string text = WebSocketSessionManager.createID();
					this._sessions.Add(text, session);
					result = text;
				}
			}
			return result;
		}

		internal void Broadcast(Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache)
		{
			foreach (IWebSocketSession webSocketSession in this.Sessions)
			{
				if (this._state != ServerState.Start)
				{
					break;
				}
				webSocketSession.Context.WebSocket.Send(opcode, data, cache);
			}
		}

		internal void Broadcast(Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache)
		{
			foreach (IWebSocketSession webSocketSession in this.Sessions)
			{
				if (this._state != ServerState.Start)
				{
					break;
				}
				webSocketSession.Context.WebSocket.Send(opcode, stream, cache);
			}
		}

		internal Dictionary<string, bool> Broadping(byte[] frame, int millisecondsTimeout)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (IWebSocketSession webSocketSession in this.Sessions)
			{
				if (this._state != ServerState.Start)
				{
					break;
				}
				dictionary.Add(webSocketSession.ID, webSocketSession.Context.WebSocket.Ping(frame, millisecondsTimeout));
			}
			return dictionary;
		}

		internal bool Remove(string id)
		{
			object sync = this._sync;
			bool result;
			lock (sync)
			{
				result = this._sessions.Remove(id);
			}
			return result;
		}

		internal void Start()
		{
			this._sweepTimer.Enabled = this._keepClean;
			this._state = ServerState.Start;
		}

		internal void Stop(byte[] data, bool send)
		{
			PayloadData payload = new PayloadData(data);
			CloseEventArgs args = new CloseEventArgs(payload);
			byte[] frame = (!send) ? null : WsFrame.CreateCloseFrame(Mask.Unmask, payload).ToByteArray();
			this.Stop(args, frame);
		}

		internal void Stop(CloseEventArgs args, byte[] frame)
		{
			object sync = this._sync;
			lock (sync)
			{
				this._state = ServerState.ShuttingDown;
				this._sweepTimer.Enabled = false;
				foreach (IWebSocketSession webSocketSession in this._sessions.Values.ToList<IWebSocketSession>())
				{
					webSocketSession.Context.WebSocket.Close(args, frame, 1000);
				}
				this._state = ServerState.Stop;
			}
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

		public Dictionary<string, bool> Broadping()
		{
			string text = this._state.CheckIfStart();
			if (text != null)
			{
				this._logger.Error(text);
				return null;
			}
			return this.Broadping(WsFrame.EmptyUnmaskPingData, 1000);
		}

		public Dictionary<string, bool> Broadping(string message)
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
			return this.Broadping(WsFrame.CreatePingFrame(Mask.Unmask, data).ToByteArray(), 1000);
		}

		public void CloseSession(string id)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.Close();
			}
		}

		public void CloseSession(string id, ushort code, string reason)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.Close(code, reason);
			}
		}

		public void CloseSession(string id, CloseStatusCode code, string reason)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.Close(code, reason);
			}
		}

		public bool PingTo(string id)
		{
			IWebSocketSession webSocketSession;
			return this.TryGetSession(id, out webSocketSession) && webSocketSession.Context.WebSocket.Ping();
		}

		public bool PingTo(string id, string message)
		{
			IWebSocketSession webSocketSession;
			return this.TryGetSession(id, out webSocketSession) && webSocketSession.Context.WebSocket.Ping(message);
		}

		public void SendTo(string id, byte[] data)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.Send(data);
			}
		}

		public void SendTo(string id, string data)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.Send(data);
			}
		}

		public void SendToAsync(string id, byte[] data, Action<bool> completed)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.SendAsync(data, completed);
			}
		}

		public void SendToAsync(string id, string data, Action<bool> completed)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.SendAsync(data, completed);
			}
		}

		public void SendToAsync(string id, Stream stream, int length, Action<bool> completed)
		{
			IWebSocketSession webSocketSession;
			if (this.TryGetSession(id, out webSocketSession))
			{
				webSocketSession.Context.WebSocket.SendAsync(stream, length, completed);
			}
		}

		public void Sweep()
		{
			if (this._state != ServerState.Start || this._sweeping || this.Count == 0)
			{
				return;
			}
			object forSweep = this._forSweep;
			lock (forSweep)
			{
				this._sweeping = true;
				foreach (string key in this.InactiveIDs)
				{
					if (this._state != ServerState.Start)
					{
						break;
					}
					object sync = this._sync;
					lock (sync)
					{
						IWebSocketSession webSocketSession;
						if (this._sessions.TryGetValue(key, out webSocketSession))
						{
							WebSocketState state = webSocketSession.State;
							if (state == WebSocketState.Open)
							{
								webSocketSession.Context.WebSocket.Close(CloseStatusCode.Abnormal);
							}
							else if (state != WebSocketState.Closing)
							{
								this._sessions.Remove(key);
							}
						}
					}
				}
				this._sweeping = false;
			}
		}

		public bool TryGetSession(string id, out IWebSocketSession session)
		{
			string text = this._state.CheckIfStart() ?? id.CheckIfValidSessionID();
			if (text != null)
			{
				this._logger.Error(text);
				session = null;
				return false;
			}
			return this.tryGetSession(id, out session);
		}
	}
}
