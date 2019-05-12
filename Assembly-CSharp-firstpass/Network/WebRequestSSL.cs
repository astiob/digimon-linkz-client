using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

namespace Network
{
	public class WebRequestSSL
	{
		private TimeSpan timeout = TimeSpan.FromSeconds(100.0);

		private CookieContainer cookieContainer = new CookieContainer();

		public bool isException;

		private string fingerPrint = string.Empty;

		public void Initialize(string fingerPrint)
		{
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.OnRemoteCertificateValidationCallback);
			this.fingerPrint = fingerPrint;
		}

		private bool OnRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return certificate.GetCertHashString() == this.fingerPrint;
		}

		public IEnumerator PostAsync(string requestUri, byte[] body, Action<string> callback)
		{
			this.isException = false;
			return this.GetResponseAsync(requestUri, "POST", body, delegate(HttpResponseMessage response)
			{
				callback(Encoding.UTF8.GetString(response.Content));
			});
		}

		private IEnumerator GetResponseAsync(string requestUri, string method, byte[] body, Action<HttpResponseMessage> completed)
		{
			HttpWebRequest request = this.CreateRequest(requestUri, method, body);
			float timeoutRealTime = Time.time + (float)((int)this.timeout.TotalSeconds);
			IAsyncResult asyncResult = request.BeginGetResponse(null, null);
			while (!asyncResult.IsCompleted)
			{
				if (Time.time > timeoutRealTime)
				{
					this.isException = true;
					yield break;
				}
				yield return null;
			}
			using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult))
			{
				using (Stream stream = response.GetResponseStream())
				{
					List<byte> buffer = new List<byte>();
					for (;;)
					{
						try
						{
							byte[] array = new byte[1024];
							int num = stream.Read(array, 0, array.Length);
							if (num <= 0)
							{
								break;
							}
							for (int i = 0; i < num; i++)
							{
								buffer.Add(array[i]);
							}
						}
						catch (Exception)
						{
							this.isException = true;
							yield break;
						}
						yield return null;
					}
					completed(new HttpResponseMessage(response, buffer.ToArray()));
				}
			}
			yield break;
		}

		private HttpWebRequest CreateRequest(string requestUri, string method, byte[] body)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
			httpWebRequest.Accept = "text/plain";
			httpWebRequest.Headers[HttpRequestHeader.AcceptCharset] = "utf-8";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.Timeout = (int)this.timeout.TotalMilliseconds;
			httpWebRequest.CookieContainer = this.cookieContainer;
			httpWebRequest.Method = method;
			if (body != null)
			{
				httpWebRequest.ContentLength = (long)body.Length;
				using (Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(body, 0, body.Length);
				}
			}
			return httpWebRequest;
		}
	}
}
