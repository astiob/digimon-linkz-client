using System;
using System.Net;

namespace WebSocketSharp.Net
{
	internal sealed class ListenerPrefix
	{
		private IPAddress[] _addresses;

		private string _host;

		private string _original;

		private string _path;

		private ushort _port;

		private bool _secure;

		public HttpListener Listener;

		public ListenerPrefix(string uriPrefix)
		{
			this._original = uriPrefix;
			this.parse(uriPrefix);
		}

		public IPAddress[] Addresses
		{
			get
			{
				return this._addresses;
			}
			set
			{
				this._addresses = value;
			}
		}

		public string Host
		{
			get
			{
				return this._host;
			}
		}

		public string Path
		{
			get
			{
				return this._path;
			}
		}

		public int Port
		{
			get
			{
				return (int)this._port;
			}
		}

		public bool Secure
		{
			get
			{
				return this._secure;
			}
		}

		private void parse(string uriPrefix)
		{
			int num = (!uriPrefix.StartsWith("http://")) ? 443 : 80;
			if (num == 443)
			{
				this._secure = true;
			}
			int length = uriPrefix.Length;
			int num2 = uriPrefix.IndexOf(':') + 3;
			int num3 = uriPrefix.IndexOf(':', num2, length - num2);
			if (num3 > 0)
			{
				int num4 = uriPrefix.IndexOf('/', num3, length - num3);
				this._host = uriPrefix.Substring(num2, num3 - num2);
				this._port = (ushort)int.Parse(uriPrefix.Substring(num3 + 1, num4 - num3 - 1));
				this._path = uriPrefix.Substring(num4);
			}
			else
			{
				int num4 = uriPrefix.IndexOf('/', num2, length - num2);
				this._host = uriPrefix.Substring(num2, num4 - num2);
				this._port = (ushort)num;
				this._path = uriPrefix.Substring(num4);
			}
			if (this._path.Length != 1)
			{
				this._path = this._path.Substring(0, this._path.Length - 1);
			}
		}

		public static void CheckUriPrefix(string uriPrefix)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			int num = (!uriPrefix.StartsWith("http://")) ? -1 : 80;
			if (num == -1)
			{
				num = ((!uriPrefix.StartsWith("https://")) ? -1 : 443);
			}
			if (num == -1)
			{
				throw new ArgumentException("Only 'http' and 'https' schemes are supported.");
			}
			int length = uriPrefix.Length;
			int num2 = uriPrefix.IndexOf(':') + 3;
			if (num2 >= length)
			{
				throw new ArgumentException("No host specified.");
			}
			int num3 = uriPrefix.IndexOf(':', num2, length - num2);
			if (num2 == num3)
			{
				throw new ArgumentException("No host specified.");
			}
			if (num3 > 0)
			{
				int num4 = uriPrefix.IndexOf('/', num3, length - num3);
				if (num4 == -1)
				{
					throw new ArgumentException("No path specified.");
				}
				try
				{
					int num5 = int.Parse(uriPrefix.Substring(num3 + 1, num4 - num3 - 1));
					if (num5 <= 0 || num5 >= 65536)
					{
						throw new Exception();
					}
				}
				catch
				{
					throw new ArgumentException("Invalid port.");
				}
			}
			else
			{
				int num4 = uriPrefix.IndexOf('/', num2, length - num2);
				if (num4 == -1)
				{
					throw new ArgumentException("No path specified.");
				}
			}
			if (uriPrefix[uriPrefix.Length - 1] != '/')
			{
				throw new ArgumentException("The URI prefix must end with '/'.");
			}
		}

		public override bool Equals(object obj)
		{
			ListenerPrefix listenerPrefix = obj as ListenerPrefix;
			return listenerPrefix != null && this._original == listenerPrefix._original;
		}

		public override int GetHashCode()
		{
			return this._original.GetHashCode();
		}

		public override string ToString()
		{
			return this._original;
		}
	}
}
