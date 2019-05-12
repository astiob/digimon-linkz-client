using System;

namespace System.Net
{
	internal class FtpRequestCreator : IWebRequestCreate
	{
		public WebRequest Create(System.Uri uri)
		{
			return new FtpWebRequest(uri);
		}
	}
}
