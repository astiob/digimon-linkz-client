using System;
using System.Net;

namespace Network
{
	internal class HttpResponseMessage
	{
		internal HttpResponseMessage(HttpWebResponse response, byte[] content)
		{
			this.Content = content;
			this.Headers = response.Headers;
			this.StatusCode = response.StatusCode;
		}

		public byte[] Content { get; private set; }

		public WebHeaderCollection Headers { get; private set; }

		public HttpStatusCode StatusCode { get; private set; }
	}
}
