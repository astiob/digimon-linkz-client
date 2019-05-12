using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.Experimental.Networking
{
	/// <summary>
	///   <para>The UnityWebRequest object is used to communicate with web servers.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UnityWebRequest : IDisposable
	{
		/// <summary>
		///   <para>The string "GET", commonly used as the verb for an HTTP GET request.</para>
		/// </summary>
		public const string kHttpVerbGET = "GET";

		/// <summary>
		///   <para>The string "HEAD", commonly used as the verb for an HTTP HEAD request.</para>
		/// </summary>
		public const string kHttpVerbHEAD = "HEAD";

		/// <summary>
		///   <para>The string "POST", commonly used as the verb for an HTTP POST request.</para>
		/// </summary>
		public const string kHttpVerbPOST = "POST";

		/// <summary>
		///   <para>The string "PUT", commonly used as the verb for an HTTP PUT request.</para>
		/// </summary>
		public const string kHttpVerbPUT = "PUT";

		/// <summary>
		///   <para>The string "CREATE", commonly used as the verb for an HTTP CREATE request.</para>
		/// </summary>
		public const string kHttpVerbCREATE = "CREATE";

		/// <summary>
		///   <para>The string "DELETE", commonly used as the verb for an HTTP DELETE request.</para>
		/// </summary>
		public const string kHttpVerbDELETE = "DELETE";

		[NonSerialized]
		internal IntPtr m_Ptr;

		private static readonly string[] forbiddenHeaderKeys = new string[]
		{
			"accept-charset",
			"accept-encoding",
			"access-control-request-headers",
			"access-control-request-method",
			"connection",
			"content-length",
			"cookie",
			"cookie2",
			"date",
			"dnt",
			"expect",
			"host",
			"keep-alive",
			"origin",
			"referer",
			"te",
			"trailer",
			"transfer-encoding",
			"upgrade",
			"user-agent",
			"via",
			"x-unity-version"
		};

		public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			byte[] bytes = Encoding.UTF8.GetBytes("\r\n");
			int num = 0;
			foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
			{
				num += 64 + multipartFormSection.sectionData.Length;
			}
			List<byte> list = new List<byte>(num);
			foreach (IMultipartFormSection multipartFormSection2 in multipartFormSections)
			{
				string str = "form-data";
				string sectionName = multipartFormSection2.sectionName;
				string fileName = multipartFormSection2.fileName;
				if (!string.IsNullOrEmpty(fileName))
				{
					str = "file";
				}
				string text = "Content-Disposition: " + str;
				if (!string.IsNullOrEmpty(sectionName))
				{
					text = text + "; name=\"" + sectionName + "\"";
				}
				if (!string.IsNullOrEmpty(fileName))
				{
					text = text + "; filename=\"" + fileName + "\"";
				}
				text += "\r\n";
				string contentType = multipartFormSection2.contentType;
				if (!string.IsNullOrEmpty(contentType))
				{
					text = text + "Content-Type: " + contentType + "\r\n";
				}
				list.AddRange(boundary);
				list.AddRange(bytes);
				list.AddRange(Encoding.UTF8.GetBytes(text));
				list.AddRange(bytes);
				list.AddRange(multipartFormSection2.sectionData);
			}
			list.TrimExcess();
			return list.ToArray();
		}

		/// <summary>
		///   <para>Generate a random 40-byte array for use as a multipart form boundary.</para>
		/// </summary>
		/// <returns>
		///   <para>40 random bytes, guaranteed to contain only printable ASCII values.</para>
		/// </returns>
		public static byte[] GenerateBoundary()
		{
			byte[] array = new byte[40];
			for (int i = 0; i < 40; i++)
			{
				int num = Random.Range(48, 110);
				if (num > 57)
				{
					num += 7;
				}
				if (num > 90)
				{
					num += 6;
				}
				array[i] = (byte)num;
			}
			return array;
		}

		public static byte[] SerializeSimpleForm(Dictionary<string, string> formFields)
		{
			string text = string.Empty;
			foreach (KeyValuePair<string, string> keyValuePair in formFields)
			{
				if (text.Length > 0)
				{
					text += "&";
				}
				text = text + Uri.EscapeDataString(keyValuePair.Key) + "=" + Uri.EscapeDataString(keyValuePair.Value);
			}
			return Encoding.UTF8.GetBytes(text);
		}

		/// <summary>
		///   <para>If true, any DownloadHandler attached to this UnityWebRequest will have DownloadHandler.Dispose called automatically when UnityWebRequest.Dispose is called.</para>
		/// </summary>
		public bool disposeDownloadHandlerOnDispose { get; set; }

		/// <summary>
		///   <para>If true, any UploadHandler attached to this UnityWebRequest will have UploadHandler.Dispose called automatically when UnityWebRequest.Dispose is called.</para>
		/// </summary>
		public bool disposeUploadHandlerOnDispose { get; set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InternalDestroy();

		private void InternalSetDefaults()
		{
			this.disposeDownloadHandlerOnDispose = true;
			this.disposeUploadHandlerOnDispose = true;
		}

		~UnityWebRequest()
		{
			this.InternalDestroy();
		}

		/// <summary>
		///   <para>Signals that this [UnityWebRequest] is no longer being used, and should clean up any resources it is using.</para>
		/// </summary>
		public void Dispose()
		{
		}

		private static bool ContainsForbiddenCharacters(string s, int firstAllowedCharCode)
		{
			foreach (char c in s)
			{
				if ((int)c < firstAllowedCharCode || c == '\u007f')
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsHeaderNameLegal(string headerName)
		{
			if (string.IsNullOrEmpty(headerName))
			{
				return false;
			}
			headerName = headerName.ToLower();
			if (UnityWebRequest.ContainsForbiddenCharacters(headerName, 33))
			{
				return false;
			}
			if (headerName.StartsWith("sec-") || headerName.StartsWith("proxy-"))
			{
				return false;
			}
			foreach (string b in UnityWebRequest.forbiddenHeaderKeys)
			{
				if (string.Equals(headerName, b))
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsHeaderValueLegal(string headerValue)
		{
			return !string.IsNullOrEmpty(headerValue) && !UnityWebRequest.ContainsForbiddenCharacters(headerValue, 32);
		}

		private static string GetErrorDescription(UnityWebRequest.UnityWebRequestError errorCode)
		{
			switch (errorCode)
			{
			case UnityWebRequest.UnityWebRequestError.OK:
				return "No Error";
			case UnityWebRequest.UnityWebRequestError.SDKError:
				return "Internal Error With Transport Layer";
			case UnityWebRequest.UnityWebRequestError.UnsupportedProtocol:
				return "Specified Transport Protocol is Unsupported";
			case UnityWebRequest.UnityWebRequestError.MalformattedUrl:
				return "URL is Malformatted";
			case UnityWebRequest.UnityWebRequestError.CannotResolveProxy:
				return "Unable to resolve specified proxy server";
			case UnityWebRequest.UnityWebRequestError.CannotResolveHost:
				return "Unable to resolve host specified in URL";
			case UnityWebRequest.UnityWebRequestError.CannotConnectToHost:
				return "Unable to connect to host specified in URL";
			case UnityWebRequest.UnityWebRequestError.AccessDenied:
				return "Remote server denied access to the specified URL";
			case UnityWebRequest.UnityWebRequestError.GenericHTTPError:
				return "Unknown/Generic HTTP Error - Check HTTP Error code";
			case UnityWebRequest.UnityWebRequestError.WriteError:
				return "Error when transmitting request to remote server - transmission terminated prematurely";
			case UnityWebRequest.UnityWebRequestError.ReadError:
				return "Error when reading response from remote server - transmission terminated prematurely";
			case UnityWebRequest.UnityWebRequestError.OutOfMemory:
				return "Out of Memory";
			case UnityWebRequest.UnityWebRequestError.Timeout:
				return "Timeout occurred while waiting for response from remote server";
			case UnityWebRequest.UnityWebRequestError.HTTPPostError:
				return "Error while transmitting HTTP POST body data";
			case UnityWebRequest.UnityWebRequestError.SSLCannotConnect:
				return "Unable to connect to SSL server at remote host";
			case UnityWebRequest.UnityWebRequestError.Aborted:
				return "Request was manually aborted by local code";
			case UnityWebRequest.UnityWebRequestError.TooManyRedirects:
				return "Redirect limit exceeded";
			case UnityWebRequest.UnityWebRequestError.ReceivedNoData:
				return "Received an empty response from remote host";
			case UnityWebRequest.UnityWebRequestError.SSLNotSupported:
				return "SSL connections are not supported on the local machine";
			case UnityWebRequest.UnityWebRequestError.FailedToSendData:
				return "Failed to transmit body data";
			case UnityWebRequest.UnityWebRequestError.FailedToReceiveData:
				return "Failed to receive response body data";
			case UnityWebRequest.UnityWebRequestError.SSLCertificateError:
				return "Failure to authenticate SSL certificate of remote host";
			case UnityWebRequest.UnityWebRequestError.SSLCipherNotAvailable:
				return "SSL cipher received from remote host is not supported on the local machine";
			case UnityWebRequest.UnityWebRequestError.SSLCACertError:
				return "Failure to authenticate Certificate Authority of the SSL certificate received from the remote host";
			case UnityWebRequest.UnityWebRequestError.UnrecognizedContentEncoding:
				return "Remote host returned data with an unrecognized/unparseable content encoding";
			case UnityWebRequest.UnityWebRequestError.LoginFailed:
				return "HTTP authentication failed";
			case UnityWebRequest.UnityWebRequestError.SSLShutdownFailed:
				return "Failure while shutting down SSL connection";
			}
			return "Unknown error";
		}

		internal enum UnityWebRequestMethod
		{
			Get,
			Post,
			Put,
			Head,
			Custom
		}

		internal enum UnityWebRequestError
		{
			OK,
			Unknown,
			SDKError,
			UnsupportedProtocol,
			MalformattedUrl,
			CannotResolveProxy,
			CannotResolveHost,
			CannotConnectToHost,
			AccessDenied,
			GenericHTTPError,
			WriteError,
			ReadError,
			OutOfMemory,
			Timeout,
			HTTPPostError,
			SSLCannotConnect,
			Aborted,
			TooManyRedirects,
			ReceivedNoData,
			SSLNotSupported,
			FailedToSendData,
			FailedToReceiveData,
			SSLCertificateError,
			SSLCipherNotAvailable,
			SSLCACertError,
			UnrecognizedContentEncoding,
			LoginFailed,
			SSLShutdownFailed
		}
	}
}
