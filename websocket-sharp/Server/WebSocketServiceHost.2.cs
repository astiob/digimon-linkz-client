using System;
using WebSocketSharp.Net;

namespace WebSocketSharp.Server
{
	internal class WebSocketServiceHost<T> : WebSocketServiceHost where T : WebSocketService
	{
		private Func<T> _constructor;

		private string _path;

		private WebSocketSessionManager _sessions;

		internal WebSocketServiceHost(string path, Func<T> constructor, Logger logger)
		{
			this._path = HttpUtility.UrlDecode(path).TrimEndSlash();
			this._constructor = constructor;
			this._sessions = new WebSocketSessionManager(logger);
		}

		public override bool KeepClean
		{
			get
			{
				return this._sessions.KeepClean;
			}
			set
			{
				this._sessions.KeepClean = value;
			}
		}

		public override string Path
		{
			get
			{
				return this._path;
			}
		}

		public override WebSocketSessionManager Sessions
		{
			get
			{
				return this._sessions;
			}
		}

		public override Type Type
		{
			get
			{
				return typeof(T);
			}
		}

		protected override WebSocketService CreateSession()
		{
			return this._constructor();
		}
	}
}
