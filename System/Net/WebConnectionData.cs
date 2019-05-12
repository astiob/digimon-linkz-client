using System;
using System.IO;

namespace System.Net
{
	internal class WebConnectionData
	{
		public HttpWebRequest request;

		public int StatusCode;

		public string StatusDescription;

		public WebHeaderCollection Headers;

		public Version Version;

		public Stream stream;

		public string Challenge;

		public void Init()
		{
			this.request = null;
			this.StatusCode = 0;
			this.StatusDescription = null;
			this.Headers = null;
			this.stream = null;
		}
	}
}
