using System;

namespace System.Net
{
	internal sealed class ListenerPrefix
	{
		private string original;

		private string host;

		private ushort port;

		private string path;

		private bool secure;

		private IPAddress[] addresses;

		public HttpListener Listener;

		public ListenerPrefix(string prefix)
		{
			this.original = prefix;
			this.Parse(prefix);
		}

		public override string ToString()
		{
			return this.original;
		}

		public IPAddress[] Addresses
		{
			get
			{
				return this.addresses;
			}
			set
			{
				this.addresses = value;
			}
		}

		public bool Secure
		{
			get
			{
				return this.secure;
			}
		}

		public string Host
		{
			get
			{
				return this.host;
			}
		}

		public int Port
		{
			get
			{
				return (int)this.port;
			}
		}

		public string Path
		{
			get
			{
				return this.path;
			}
		}

		public override bool Equals(object o)
		{
			ListenerPrefix listenerPrefix = o as ListenerPrefix;
			return listenerPrefix != null && this.original == listenerPrefix.original;
		}

		public override int GetHashCode()
		{
			return this.original.GetHashCode();
		}

		private void Parse(string uri)
		{
			int num = (!uri.StartsWith("http://")) ? -1 : 80;
			if (num == -1)
			{
				int num2 = (!uri.StartsWith("https://")) ? -1 : 443;
				this.secure = true;
			}
			int length = uri.Length;
			int num3 = uri.IndexOf(':') + 3;
			if (num3 >= length)
			{
				throw new ArgumentException("No host specified.");
			}
			int num4 = uri.IndexOf(':', num3, length - num3);
			if (num4 > 0)
			{
				this.host = uri.Substring(num3, num4 - num3);
				int num5 = uri.IndexOf('/', num4, length - num4);
				this.port = (ushort)int.Parse(uri.Substring(num4 + 1, num5 - num4 - 1));
				this.path = uri.Substring(num5);
			}
			else
			{
				int num5 = uri.IndexOf('/', num3, length - num3);
				this.host = uri.Substring(num3, num5 - num3);
				this.path = uri.Substring(num5);
			}
		}

		public static void CheckUri(string uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			int num = (!uri.StartsWith("http://")) ? -1 : 80;
			if (num == -1)
			{
				num = ((!uri.StartsWith("https://")) ? -1 : 443);
			}
			if (num == -1)
			{
				throw new ArgumentException("Only 'http' and 'https' schemes are supported.");
			}
			int length = uri.Length;
			int num2 = uri.IndexOf(':') + 3;
			if (num2 >= length)
			{
				throw new ArgumentException("No host specified.");
			}
			int num3 = uri.IndexOf(':', num2, length - num2);
			if (num2 == num3)
			{
				throw new ArgumentException("No host specified.");
			}
			if (num3 > 0)
			{
				int num4 = uri.IndexOf('/', num3, length - num3);
				if (num4 == -1)
				{
					throw new ArgumentException("No path specified.");
				}
				try
				{
					int num5 = int.Parse(uri.Substring(num3 + 1, num4 - num3 - 1));
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
				int num4 = uri.IndexOf('/', num2, length - num2);
				if (num4 == -1)
				{
					throw new ArgumentException("No path specified.");
				}
			}
			if (uri[uri.Length - 1] != '/')
			{
				throw new ArgumentException("The prefix must end with '/'");
			}
		}
	}
}
