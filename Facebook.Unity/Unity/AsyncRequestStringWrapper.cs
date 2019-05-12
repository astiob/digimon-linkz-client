using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
	internal class AsyncRequestStringWrapper : IAsyncRequestStringWrapper
	{
		public void Request(Uri url, HttpMethod method, WWWForm query = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, method, query, callback);
		}

		public void Request(Uri url, HttpMethod method, IDictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, method, formData, callback);
		}
	}
}
