using System;
using System.Collections.Specialized;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	internal abstract class HandshakeBase
	{
		protected const string CrLf = "\r\n";

		private byte[] _entity;

		private NameValueCollection _headers;

		private Version _version;

		internal byte[] EntityBodyData
		{
			get
			{
				return this._entity;
			}
			set
			{
				this._entity = value;
			}
		}

		public string EntityBody
		{
			get
			{
				return (this._entity == null || this._entity.LongLength <= 0L) ? string.Empty : HandshakeBase.getEncoding(this._headers["Content-Type"]).GetString(this._entity);
			}
		}

		public NameValueCollection Headers
		{
			get
			{
				NameValueCollection result;
				if ((result = this._headers) == null)
				{
					result = (this._headers = new NameValueCollection());
				}
				return result;
			}
			protected set
			{
				this._headers = value;
			}
		}

		public Version ProtocolVersion
		{
			get
			{
				Version result;
				if ((result = this._version) == null)
				{
					result = (this._version = HttpVersion.Version11);
				}
				return result;
			}
			protected set
			{
				this._version = value;
			}
		}

		private static Encoding getEncoding(string contentType)
		{
			if (contentType == null || contentType.Length == 0)
			{
				return Encoding.UTF8;
			}
			int num = contentType.IndexOf("charset=", StringComparison.Ordinal);
			if (num == -1)
			{
				return Encoding.UTF8;
			}
			string text = contentType.Substring(num + 8);
			num = text.IndexOf(';');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			return Encoding.GetEncoding(text);
		}

		public byte[] ToByteArray()
		{
			return Encoding.UTF8.GetBytes(this.ToString());
		}
	}
}
