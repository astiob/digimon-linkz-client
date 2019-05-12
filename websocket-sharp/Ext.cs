using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;

namespace WebSocketSharp
{
	public static class Ext
	{
		private const string _tspecials = "()<>@,;:\\\"/[]?={} \t";

		private static byte[] compress(this byte[] value)
		{
			if (value.LongLength == 0L)
			{
				return value;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(value))
			{
				result = memoryStream.compressToArray();
			}
			return result;
		}

		private static MemoryStream compress(this Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream();
			if (stream.Length == 0L)
			{
				return memoryStream;
			}
			stream.Position = 0L;
			MemoryStream result;
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
			{
				stream.CopyTo(deflateStream);
				deflateStream.Close();
				memoryStream.Position = 0L;
				result = memoryStream;
			}
			return result;
		}

		private static byte[] compressToArray(this Stream stream)
		{
			byte[] result;
			using (MemoryStream memoryStream = stream.compress())
			{
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		private static byte[] decompress(this byte[] value)
		{
			if (value.LongLength == 0L)
			{
				return value;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(value))
			{
				result = memoryStream.decompressToArray();
			}
			return result;
		}

		private static MemoryStream decompress(this Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream();
			if (stream.Length == 0L)
			{
				return memoryStream;
			}
			stream.Position = 0L;
			MemoryStream result;
			using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
			{
				deflateStream.CopyTo(memoryStream, true);
				result = memoryStream;
			}
			return result;
		}

		private static byte[] decompressToArray(this Stream stream)
		{
			byte[] result;
			using (MemoryStream memoryStream = stream.decompress())
			{
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		private static byte[] readBytes(this Stream stream, byte[] buffer, int offset, int length)
		{
			int i = stream.Read(buffer, offset, length);
			if (i < 1)
			{
				return buffer.SubArray(0, offset);
			}
			while (i < length)
			{
				int num = stream.Read(buffer, offset + i, length - i);
				if (num < 1)
				{
					break;
				}
				i += num;
			}
			return (i >= length) ? buffer : buffer.SubArray(0, offset + i);
		}

		private static bool readBytes(this Stream stream, byte[] buffer, int offset, int length, Stream dest)
		{
			byte[] array = stream.readBytes(buffer, offset, length);
			int num = array.Length;
			dest.Write(array, 0, num);
			return num == offset + length;
		}

		private static void times(this ulong n, Action act)
		{
			for (ulong num = 0UL; num < n; num += 1UL)
			{
				act();
			}
		}

		internal static byte[] Append(this ushort code, string reason)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] array = code.ToByteArrayInternally(ByteOrder.Big);
				memoryStream.Write(array, 0, 2);
				if (reason != null && reason.Length > 0)
				{
					array = Encoding.UTF8.GetBytes(reason);
					memoryStream.Write(array, 0, array.Length);
				}
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		internal static string CheckIfCanRead(this Stream stream)
		{
			return (stream != null) ? (stream.CanRead ? null : "'stream' cannot be read.") : "'stream' must not be null.";
		}

		internal static string CheckIfClosable(this WebSocketState state)
		{
			return (state != WebSocketState.Closing) ? ((state != WebSocketState.Closed) ? null : "The WebSocket connection has already been closed.") : "While closing the WebSocket connection.";
		}

		internal static string CheckIfConnectable(this WebSocketState state)
		{
			return (state != WebSocketState.Open && state != WebSocketState.Closing) ? null : "A WebSocket connection has already been established.";
		}

		internal static string CheckIfOpen(this WebSocketState state)
		{
			return (state != WebSocketState.Connecting) ? ((state != WebSocketState.Closing) ? ((state != WebSocketState.Closed) ? null : "The WebSocket connection has already been closed.") : "While closing the WebSocket connection.") : "A WebSocket connection isn't established.";
		}

		internal static string CheckIfStart(this ServerState state)
		{
			return (state != ServerState.Ready) ? ((state != ServerState.ShuttingDown) ? ((state != ServerState.Stop) ? null : "The server has already stopped.") : "The server is shutting down.") : "The server hasn't yet started.";
		}

		internal static string CheckIfStartable(this ServerState state)
		{
			return (state != ServerState.Start) ? ((state != ServerState.ShuttingDown) ? null : "The server is shutting down.") : "The server has already started.";
		}

		internal static string CheckIfValidCloseStatusCode(this ushort code)
		{
			return code.IsCloseStatusCode() ? null : "Invalid close status code.";
		}

		internal static string CheckIfValidControlData(this byte[] data, string paramName)
		{
			return (data.Length <= 125) ? null : string.Format("'{0}' length must be less.", paramName);
		}

		internal static string CheckIfValidProtocols(this string[] protocols)
		{
			return (!protocols.Contains((string protocol) => protocol == null || protocol.Length == 0 || !protocol.IsToken())) ? ((!protocols.ContainsTwice()) ? null : "Contains a value twice.") : "Contains an invalid value.";
		}

		internal static string CheckIfValidSendData(this byte[] data)
		{
			return (data != null) ? null : "'data' must not be null.";
		}

		internal static string CheckIfValidSendData(this FileInfo file)
		{
			return (file != null) ? null : "'file' must not be null.";
		}

		internal static string CheckIfValidSendData(this string data)
		{
			return (data != null) ? null : "'data' must not be null.";
		}

		internal static string CheckIfValidServicePath(this string servicePath)
		{
			return (servicePath != null && servicePath.Length != 0) ? ((servicePath[0] == '/') ? ((servicePath.IndexOfAny(new char[]
			{
				'?',
				'#'
			}) == -1) ? null : "'servicePath' must not contain either or both query and fragment components.") : "'servicePath' not absolute path.") : "'servicePath' must not be null or empty.";
		}

		internal static string CheckIfValidSessionID(this string id)
		{
			return (id != null && id.Length != 0) ? null : "'id' must not be null or empty.";
		}

		internal static void Close(this WebSocketSharp.Net.HttpListenerResponse response, WebSocketSharp.Net.HttpStatusCode code)
		{
			response.StatusCode = (int)code;
			response.OutputStream.Close();
		}

		internal static void CloseWithAuthChallenge(this WebSocketSharp.Net.HttpListenerResponse response, string challenge)
		{
			response.Headers.SetInternal("WWW-Authenticate", challenge, true);
			response.Close(WebSocketSharp.Net.HttpStatusCode.Unauthorized);
		}

		internal static byte[] Compress(this byte[] value, CompressionMethod method)
		{
			return (method != CompressionMethod.Deflate) ? value : value.compress();
		}

		internal static Stream Compress(this Stream stream, CompressionMethod method)
		{
			return (method != CompressionMethod.Deflate) ? stream : stream.compress();
		}

		internal static byte[] CompressToArray(this Stream stream, CompressionMethod method)
		{
			return (method != CompressionMethod.Deflate) ? stream.ToByteArray() : stream.compressToArray();
		}

		internal static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> comparer)
		{
			foreach (T arg in source)
			{
				if (comparer(arg))
				{
					return true;
				}
			}
			return false;
		}

		internal static bool ContainsTwice(this string[] values)
		{
			int len = values.Length;
			Func<int, bool> contains = null;
			contains = delegate(int index)
			{
				if (index < len - 1)
				{
					for (int i = index + 1; i < len; i++)
					{
						if (values[i] == values[index])
						{
							return true;
						}
					}
					return contains(++index);
				}
				return false;
			};
			return contains(0);
		}

		internal static T[] Copy<T>(this T[] src, long length)
		{
			T[] array = new T[length];
			Array.Copy(src, 0L, array, 0L, length);
			return array;
		}

		internal static void CopyTo(this Stream src, Stream dest)
		{
			src.CopyTo(dest, false);
		}

		internal static void CopyTo(this Stream src, Stream dest, bool setDefaultPosition)
		{
			int num = 256;
			byte[] buffer = new byte[num];
			int count;
			while ((count = src.Read(buffer, 0, num)) > 0)
			{
				dest.Write(buffer, 0, count);
			}
			if (setDefaultPosition)
			{
				dest.Position = 0L;
			}
		}

		internal static byte[] Decompress(this byte[] value, CompressionMethod method)
		{
			return (method != CompressionMethod.Deflate) ? value : value.decompress();
		}

		internal static Stream Decompress(this Stream stream, CompressionMethod method)
		{
			return (method != CompressionMethod.Deflate) ? stream : stream.decompress();
		}

		internal static byte[] DecompressToArray(this Stream stream, CompressionMethod method)
		{
			return (method != CompressionMethod.Deflate) ? stream.ToByteArray() : stream.decompressToArray();
		}

		internal static bool EqualsWith(this int value, char c, Action<int> action)
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			action(value);
			return value == (int)c;
		}

		internal static string GetAbsolutePath(this Uri uri)
		{
			if (uri.IsAbsoluteUri)
			{
				return uri.AbsolutePath;
			}
			string originalString = uri.OriginalString;
			if (originalString[0] != '/')
			{
				return null;
			}
			int num = originalString.IndexOfAny(new char[]
			{
				'?',
				'#'
			});
			return (num <= 0) ? originalString : originalString.Substring(0, num);
		}

		internal static string GetMessage(this CloseStatusCode code)
		{
			return (code != CloseStatusCode.ProtocolError) ? ((code != CloseStatusCode.IncorrectData) ? ((code != CloseStatusCode.Abnormal) ? ((code != CloseStatusCode.InconsistentData) ? ((code != CloseStatusCode.PolicyViolation) ? ((code != CloseStatusCode.TooBig) ? ((code != CloseStatusCode.IgnoreExtension) ? ((code != CloseStatusCode.ServerError) ? ((code != CloseStatusCode.TlsHandshakeFailure) ? string.Empty : "An error has occurred while handshaking.") : "WebSocket server got an internal error.") : "WebSocket client did not receive expected extension(s).") : "A too big data has been received.") : "A policy violation has occurred.") : "An inconsistent data has been received.") : "An exception has occurred.") : "An incorrect data has been received.") : "A WebSocket protocol error has occurred.";
		}

		internal static string GetNameInternal(this string nameAndValue, string separator)
		{
			int num = nameAndValue.IndexOf(separator);
			return (num <= 0) ? null : nameAndValue.Substring(0, num).Trim();
		}

		internal static string GetValueInternal(this string nameAndValue, string separator)
		{
			int num = nameAndValue.IndexOf(separator);
			return (num < 0 || num >= nameAndValue.Length - 1) ? null : nameAndValue.Substring(num + 1).Trim();
		}

		internal static TcpListenerWebSocketContext GetWebSocketContext(this TcpClient client, X509Certificate cert, bool secure, Logger logger)
		{
			return new TcpListenerWebSocketContext(client, cert, secure, logger);
		}

		internal static bool IsCompressionExtension(this string value)
		{
			return value.StartsWith("permessage-");
		}

		internal static bool IsPortNumber(this int value)
		{
			return value > 0 && value < 65536;
		}

		internal static bool IsReserved(this ushort code)
		{
			return code == 1004 || code == 1005 || code == 1006 || code == 1015;
		}

		internal static bool IsReserved(this CloseStatusCode code)
		{
			return code == CloseStatusCode.Undefined || code == CloseStatusCode.NoStatusCode || code == CloseStatusCode.Abnormal || code == CloseStatusCode.TlsHandshakeFailure;
		}

		internal static bool IsText(this string value)
		{
			int length = value.Length;
			for (int i = 0; i < length; i++)
			{
				char c = value[i];
				if (c < ' ' && !"\r\n\t".Contains(new char[]
				{
					c
				}))
				{
					return false;
				}
				if (c == '\u007f')
				{
					return false;
				}
				if (c == '\n' && ++i < length)
				{
					c = value[i];
					if (!" \t".Contains(new char[]
					{
						c
					}))
					{
						return false;
					}
				}
			}
			return true;
		}

		internal static bool IsToken(this string value)
		{
			foreach (char c in value)
			{
				if (c < ' ' || c >= '\u007f' || "()<>@,;:\\\"/[]?={} \t".Contains(new char[]
				{
					c
				}))
				{
					return false;
				}
			}
			return true;
		}

		internal static string Quote(this string value)
		{
			return (!value.IsToken()) ? string.Format("\"{0}\"", value.Replace("\"", "\\\"")) : value;
		}

		internal static NameValueCollection ParseBasicAuthResponseParams(this string value)
		{
			string @string = Encoding.Default.GetString(Convert.FromBase64String(value));
			int num = @string.IndexOf(':');
			string text = @string.Substring(0, num);
			string value2 = (num >= @string.Length - 1) ? string.Empty : @string.Substring(num + 1);
			num = text.IndexOf('\\');
			if (num > 0)
			{
				text = text.Substring(num + 1);
			}
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["username"] = text;
			nameValueCollection["password"] = value2;
			return nameValueCollection;
		}

		internal static NameValueCollection ParseAuthParams(this string value)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			foreach (string text in value.SplitHeaderValue(new char[]
			{
				','
			}))
			{
				int num = text.IndexOf('=');
				string name;
				string val;
				if (num > 0)
				{
					name = text.Substring(0, num).Trim();
					val = ((num >= text.Length - 1) ? string.Empty : text.Substring(num + 1).Trim().Trim(new char[]
					{
						'"'
					}));
				}
				else
				{
					name = text;
					val = string.Empty;
				}
				nameValueCollection.Add(name, val);
			}
			return nameValueCollection;
		}

		internal static byte[] ReadBytes(this Stream stream, int length)
		{
			return stream.readBytes(new byte[length], 0, length);
		}

		internal static byte[] ReadBytes(this Stream stream, long length, int bufferLength)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				long num = length / (long)bufferLength;
				int num2 = (int)(length % (long)bufferLength);
				byte[] buffer = new byte[bufferLength];
				bool flag = false;
				for (long num3 = 0L; num3 < num; num3 += 1L)
				{
					if (!stream.readBytes(buffer, 0, bufferLength, memoryStream))
					{
						flag = true;
						break;
					}
				}
				if (!flag && num2 > 0)
				{
					stream.readBytes(new byte[num2], 0, num2, memoryStream);
				}
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		internal static void ReadBytesAsync(this Stream stream, int length, Action<byte[]> completed, Action<Exception> error)
		{
			byte[] buffer = new byte[length];
			stream.BeginRead(buffer, 0, length, delegate(IAsyncResult ar)
			{
				try
				{
					int num = stream.EndRead(ar);
					byte[] obj = (num >= 1) ? ((num >= length) ? buffer : stream.readBytes(buffer, num, length - num)) : new byte[0];
					if (completed != null)
					{
						completed(obj);
					}
				}
				catch (Exception obj2)
				{
					if (error != null)
					{
						error(obj2);
					}
				}
			}, null);
		}

		internal static string RemovePrefix(this string value, params string[] prefixes)
		{
			int num = 0;
			foreach (string text in prefixes)
			{
				if (value.StartsWith(text))
				{
					num = text.Length;
					break;
				}
			}
			return (num <= 0) ? value : value.Substring(num);
		}

		internal static T[] Reverse<T>(this T[] array)
		{
			int num = array.Length;
			T[] array2 = new T[num];
			int num2 = num - 1;
			for (int i = 0; i <= num2; i++)
			{
				array2[i] = array[num2 - i];
			}
			return array2;
		}

		internal static IEnumerable<string> SplitHeaderValue(this string value, params char[] separator)
		{
			int len = value.Length;
			string separators = new string(separator);
			StringBuilder buffer = new StringBuilder(32);
			bool quoted = false;
			bool escaped = false;
			int i = 0;
			while (i < len)
			{
				char c = value[i];
				if (c == '"')
				{
					if (escaped)
					{
						escaped = !escaped;
					}
					else
					{
						quoted = !quoted;
					}
					goto IL_168;
				}
				if (c == '\\')
				{
					if (i < len - 1 && value[i + 1] == '"')
					{
						escaped = true;
					}
					goto IL_168;
				}
				if (!separators.Contains(new char[]
				{
					c
				}))
				{
					goto IL_168;
				}
				if (quoted)
				{
					goto IL_168;
				}
				yield return buffer.ToString();
				buffer.Length = 0;
				IL_17A:
				i++;
				continue;
				IL_168:
				buffer.Append(c);
				goto IL_17A;
			}
			if (buffer.Length > 0)
			{
				yield return buffer.ToString();
			}
			yield break;
		}

		internal static byte[] ToByteArray(this Stream stream)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				stream.Position = 0L;
				stream.CopyTo(memoryStream);
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		internal static byte[] ToByteArrayInternally(this ushort value, ByteOrder order)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!order.IsHostOrder())
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		internal static byte[] ToByteArrayInternally(this ulong value, ByteOrder order)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!order.IsHostOrder())
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		internal static CompressionMethod ToCompressionMethod(this string value)
		{
			IEnumerator enumerator = Enum.GetValues(typeof(CompressionMethod)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					CompressionMethod compressionMethod = (CompressionMethod)obj;
					if (compressionMethod.ToExtensionString() == value)
					{
						return compressionMethod;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return CompressionMethod.None;
		}

		internal static string ToExtensionString(this CompressionMethod method)
		{
			return (method == CompressionMethod.None) ? string.Empty : string.Format("permessage-{0}", method.ToString().ToLower());
		}

		internal static IPAddress ToIPAddress(this string hostNameOrAddress)
		{
			IPAddress result;
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostNameOrAddress);
				result = hostAddresses[0];
			}
			catch
			{
				result = null;
			}
			return result;
		}

		internal static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			return new List<TSource>(source);
		}

		internal static ushort ToUInt16(this byte[] src, ByteOrder srcOrder)
		{
			return BitConverter.ToUInt16(src.ToHostOrder(srcOrder), 0);
		}

		internal static ulong ToUInt64(this byte[] src, ByteOrder srcOrder)
		{
			return BitConverter.ToUInt64(src.ToHostOrder(srcOrder), 0);
		}

		internal static string TrimEndSlash(this string value)
		{
			value = value.TrimEnd(new char[]
			{
				'/'
			});
			return (value.Length <= 0) ? "/" : value;
		}

		internal static bool TryCreateWebSocketUri(this string uriString, out Uri result, out string message)
		{
			result = null;
			if (uriString.Length == 0)
			{
				message = "Must not be empty.";
				return false;
			}
			Uri uri = uriString.ToUri();
			if (!uri.IsAbsoluteUri)
			{
				message = "Must be the absolute URI: " + uriString;
				return false;
			}
			string scheme = uri.Scheme;
			if (scheme != "ws" && scheme != "wss")
			{
				message = "The scheme part must be 'ws' or 'wss': " + scheme;
				return false;
			}
			string fragment = uri.Fragment;
			if (fragment.Length > 0)
			{
				message = "Must not contain the fragment component: " + uriString;
				return false;
			}
			int num = uri.Port;
			if (num > 0)
			{
				if (num > 65535)
				{
					message = "The port part must be between 1 and 65535: " + num;
					return false;
				}
				if ((scheme == "ws" && num == 443) || (scheme == "wss" && num == 80))
				{
					message = string.Format("Invalid pair of scheme and port: {0}, {1}", scheme, num);
					return false;
				}
			}
			else
			{
				num = ((!(scheme == "ws")) ? 443 : 80);
				string uriString2 = string.Format("{0}://{1}:{2}{3}", new object[]
				{
					scheme,
					uri.Host,
					num,
					uri.PathAndQuery
				});
				uri = uriString2.ToUri();
			}
			result = uri;
			message = string.Empty;
			return true;
		}

		internal static string Unquote(this string value)
		{
			int num = value.IndexOf('"');
			int num2 = value.LastIndexOf('"');
			if (num < num2)
			{
				value = value.Substring(num + 1, num2 - num - 1).Replace("\\\"", "\"");
			}
			return value.Trim();
		}

		internal static void WriteBytes(this Stream stream, byte[] value)
		{
			using (MemoryStream memoryStream = new MemoryStream(value))
			{
				memoryStream.CopyTo(stream);
			}
		}

		public static bool Contains(this string value, params char[] chars)
		{
			return chars == null || chars.Length == 0 || (value != null && value.Length != 0 && value.IndexOfAny(chars) != -1);
		}

		public static bool Contains(this NameValueCollection collection, string name)
		{
			return collection != null && collection.Count != 0 && collection[name] != null;
		}

		public static bool Contains(this NameValueCollection collection, string name, string value)
		{
			if (collection == null || collection.Count == 0)
			{
				return false;
			}
			string text = collection[name];
			if (text == null)
			{
				return false;
			}
			foreach (string text2 in text.Split(new char[]
			{
				','
			}))
			{
				if (text2.Trim().Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public static void Emit(this EventHandler eventHandler, object sender, EventArgs e)
		{
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
		}

		public static void Emit<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
		}

		public static WebSocketSharp.Net.CookieCollection GetCookies(this NameValueCollection headers, bool response)
		{
			string name = (!response) ? "Cookie" : "Set-Cookie";
			return (headers != null && headers.Contains(name)) ? WebSocketSharp.Net.CookieCollection.Parse(headers[name], response) : new WebSocketSharp.Net.CookieCollection();
		}

		public static string GetDescription(this WebSocketSharp.Net.HttpStatusCode code)
		{
			return ((int)code).GetStatusDescription();
		}

		public static string GetName(this string nameAndValue, string separator)
		{
			return (nameAndValue == null || nameAndValue.Length <= 0 || separator == null || separator.Length <= 0) ? null : nameAndValue.GetNameInternal(separator);
		}

		public static KeyValuePair<string, string> GetNameAndValue(this string nameAndValue, string separator)
		{
			string name = nameAndValue.GetName(separator);
			string value = nameAndValue.GetValue(separator);
			return (name == null) ? new KeyValuePair<string, string>(null, null) : new KeyValuePair<string, string>(name, value);
		}

		public static string GetStatusDescription(this int code)
		{
			switch (code)
			{
			case 400:
				return "Bad Request";
			case 401:
				return "Unauthorized";
			case 402:
				return "Payment Required";
			case 403:
				return "Forbidden";
			case 404:
				return "Not Found";
			case 405:
				return "Method Not Allowed";
			case 406:
				return "Not Acceptable";
			case 407:
				return "Proxy Authentication Required";
			case 408:
				return "Request Timeout";
			case 409:
				return "Conflict";
			case 410:
				return "Gone";
			case 411:
				return "Length Required";
			case 412:
				return "Precondition Failed";
			case 413:
				return "Request Entity Too Large";
			case 414:
				return "Request-Uri Too Long";
			case 415:
				return "Unsupported Media Type";
			case 416:
				return "Requested Range Not Satisfiable";
			case 417:
				return "Expectation Failed";
			default:
				switch (code)
				{
				case 200:
					return "OK";
				case 201:
					return "Created";
				case 202:
					return "Accepted";
				case 203:
					return "Non-Authoritative Information";
				case 204:
					return "No Content";
				case 205:
					return "Reset Content";
				case 206:
					return "Partial Content";
				case 207:
					return "Multi-Status";
				default:
					switch (code)
					{
					case 300:
						return "Multiple Choices";
					case 301:
						return "Moved Permanently";
					case 302:
						return "Found";
					case 303:
						return "See Other";
					case 304:
						return "Not Modified";
					case 305:
						return "Use Proxy";
					default:
						switch (code)
						{
						case 500:
							return "Internal Server Error";
						case 501:
							return "Not Implemented";
						case 502:
							return "Bad Gateway";
						case 503:
							return "Service Unavailable";
						case 504:
							return "Gateway Timeout";
						case 505:
							return "Http Version Not Supported";
						default:
							switch (code)
							{
							case 100:
								return "Continue";
							case 101:
								return "Switching Protocols";
							case 102:
								return "Processing";
							default:
								return string.Empty;
							}
							break;
						case 507:
							return "Insufficient Storage";
						}
						break;
					case 307:
						return "Temporary Redirect";
					}
					break;
				}
				break;
			case 422:
				return "Unprocessable Entity";
			case 423:
				return "Locked";
			case 424:
				return "Failed Dependency";
			}
		}

		public static string GetValue(this string nameAndValue, string separator)
		{
			return (nameAndValue == null || nameAndValue.Length <= 0 || separator == null || separator.Length <= 0) ? null : nameAndValue.GetValueInternal(separator);
		}

		public static bool IsCloseStatusCode(this ushort value)
		{
			return value > 999 && value < 5000;
		}

		public static bool IsEnclosedIn(this string value, char c)
		{
			return value != null && value.Length > 1 && value[0] == c && value[value.Length - 1] == c;
		}

		public static bool IsHostOrder(this ByteOrder order)
		{
			return !(BitConverter.IsLittleEndian ^ order == ByteOrder.Little);
		}

		public static bool IsLocal(this IPAddress address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Equals(IPAddress.Any) || IPAddress.IsLoopback(address))
			{
				return true;
			}
			string hostName = Dns.GetHostName();
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			foreach (IPAddress obj in hostAddresses)
			{
				if (address.Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsNullOrEmpty(this string value)
		{
			return value == null || value.Length == 0;
		}

		public static bool IsPredefinedScheme(this string value)
		{
			if (value == null && value.Length < 2)
			{
				return false;
			}
			char c = value[0];
			if (c == 'h')
			{
				return value == "http" || value == "https";
			}
			if (c == 'w')
			{
				return value == "ws" || value == "wss";
			}
			if (c == 'f')
			{
				return value == "file" || value == "ftp";
			}
			if (c == 'n')
			{
				c = value[1];
				return (c != 'e') ? (value == "nntp") : (value == "news" || value == "net.pipe" || value == "net.tcp");
			}
			return (c == 'g' && value == "gopher") || (c == 'm' && value == "mailto");
		}

		public static bool IsUpgradeTo(this WebSocketSharp.Net.HttpListenerRequest request, string protocol)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}
			if (protocol.Length == 0)
			{
				throw new ArgumentException("Must not be empty.", "protocol");
			}
			return request.Headers.Contains("Upgrade", protocol) && request.Headers.Contains("Connection", "Upgrade");
		}

		public static bool MaybeUri(this string value)
		{
			if (value == null || value.Length == 0)
			{
				return false;
			}
			int num = value.IndexOf(':');
			return num != -1 && num < 10 && value.Substring(0, num).IsPredefinedScheme();
		}

		public static T[] SubArray<T>(this T[] array, int startIndex, int length)
		{
			if (array == null || array.Length == 0)
			{
				return new T[0];
			}
			if (startIndex < 0 || length <= 0)
			{
				return new T[0];
			}
			if (startIndex + length > array.Length)
			{
				return new T[0];
			}
			if (startIndex == 0 && array.Length == length)
			{
				return array;
			}
			T[] array2 = new T[length];
			Array.Copy(array, startIndex, array2, 0, length);
			return array2;
		}

		public static void Times(this int n, Action act)
		{
			if (n > 0 && act != null)
			{
				((ulong)((long)n)).times(act);
			}
		}

		public static void Times(this long n, Action act)
		{
			if (n > 0L && act != null)
			{
				((ulong)n).times(act);
			}
		}

		public static void Times(this uint n, Action act)
		{
			if (n > 0u && act != null)
			{
				((ulong)n).times(act);
			}
		}

		public static void Times(this ulong n, Action act)
		{
			if (n > 0UL && act != null)
			{
				n.times(act);
			}
		}

		public static void Times(this int n, Action<int> act)
		{
			if (n > 0 && act != null)
			{
				for (int i = 0; i < n; i++)
				{
					act(i);
				}
			}
		}

		public static void Times(this long n, Action<long> act)
		{
			if (n > 0L && act != null)
			{
				for (long num = 0L; num < n; num += 1L)
				{
					act(num);
				}
			}
		}

		public static void Times(this uint n, Action<uint> act)
		{
			if (n > 0u && act != null)
			{
				for (uint num = 0u; num < n; num += 1u)
				{
					act(num);
				}
			}
		}

		public static void Times(this ulong n, Action<ulong> act)
		{
			if (n > 0UL && act != null)
			{
				for (ulong num = 0UL; num < n; num += 1UL)
				{
					act(num);
				}
			}
		}

		public static T To<T>(this byte[] src, ByteOrder srcOrder) where T : struct
		{
			if (src == null)
			{
				throw new ArgumentNullException("src");
			}
			if (src.Length == 0)
			{
				return default(T);
			}
			Type typeFromHandle = typeof(T);
			byte[] value = src.ToHostOrder(srcOrder);
			return (typeFromHandle != typeof(bool)) ? ((typeFromHandle != typeof(char)) ? ((typeFromHandle != typeof(double)) ? ((typeFromHandle != typeof(short)) ? ((typeFromHandle != typeof(int)) ? ((typeFromHandle != typeof(long)) ? ((typeFromHandle != typeof(float)) ? ((typeFromHandle != typeof(ushort)) ? ((typeFromHandle != typeof(uint)) ? ((typeFromHandle != typeof(ulong)) ? default(T) : ((T)((object)BitConverter.ToUInt64(value, 0)))) : ((T)((object)BitConverter.ToUInt32(value, 0)))) : ((T)((object)BitConverter.ToUInt16(value, 0)))) : ((T)((object)BitConverter.ToSingle(value, 0)))) : ((T)((object)BitConverter.ToInt64(value, 0)))) : ((T)((object)BitConverter.ToInt32(value, 0)))) : ((T)((object)BitConverter.ToInt16(value, 0)))) : ((T)((object)BitConverter.ToDouble(value, 0)))) : ((T)((object)BitConverter.ToChar(value, 0)))) : ((T)((object)BitConverter.ToBoolean(value, 0)));
		}

		public static byte[] ToByteArray<T>(this T value, ByteOrder order) where T : struct
		{
			Type typeFromHandle = typeof(T);
			byte[] array;
			if (typeFromHandle == typeof(bool))
			{
				array = BitConverter.GetBytes((bool)((object)value));
			}
			else if (typeFromHandle == typeof(byte))
			{
				(array = new byte[1])[0] = (byte)((object)value);
			}
			else
			{
				array = ((typeFromHandle != typeof(char)) ? ((typeFromHandle != typeof(double)) ? ((typeFromHandle != typeof(short)) ? ((typeFromHandle != typeof(int)) ? ((typeFromHandle != typeof(long)) ? ((typeFromHandle != typeof(float)) ? ((typeFromHandle != typeof(ushort)) ? ((typeFromHandle != typeof(uint)) ? ((typeFromHandle != typeof(ulong)) ? new byte[0] : BitConverter.GetBytes((ulong)((object)value))) : BitConverter.GetBytes((uint)((object)value))) : BitConverter.GetBytes((ushort)((object)value))) : BitConverter.GetBytes((float)((object)value))) : BitConverter.GetBytes((long)((object)value))) : BitConverter.GetBytes((int)((object)value))) : BitConverter.GetBytes((short)((object)value))) : BitConverter.GetBytes((double)((object)value))) : BitConverter.GetBytes((char)((object)value)));
			}
			byte[] array2 = array;
			if (array2.Length > 1 && !order.IsHostOrder())
			{
				Array.Reverse(array2);
			}
			return array2;
		}

		public static byte[] ToHostOrder(this byte[] src, ByteOrder srcOrder)
		{
			if (src == null)
			{
				throw new ArgumentNullException("src");
			}
			return (src.Length <= 1 || srcOrder.IsHostOrder()) ? src : src.Reverse<byte>();
		}

		public static string ToString<T>(this T[] array, string separator)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = array.Length;
			if (num == 0)
			{
				return string.Empty;
			}
			if (separator == null)
			{
				separator = string.Empty;
			}
			StringBuilder buffer = new StringBuilder(64);
			(num - 1).Times(delegate(int i)
			{
				buffer.AppendFormat("{0}{1}", array[i].ToString(), separator);
			});
			buffer.Append(array[num - 1].ToString());
			return buffer.ToString();
		}

		public static Uri ToUri(this string uriString)
		{
			return (uriString != null && uriString.Length != 0) ? ((!uriString.MaybeUri()) ? new Uri(uriString, UriKind.Relative) : new Uri(uriString)) : null;
		}

		public static string UrlDecode(this string value)
		{
			return (value != null && value.Length != 0) ? HttpUtility.UrlDecode(value) : value;
		}

		public static string UrlEncode(this string value)
		{
			return (value != null && value.Length != 0) ? HttpUtility.UrlEncode(value) : value;
		}

		public static void WriteContent(this WebSocketSharp.Net.HttpListenerResponse response, byte[] content)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			if (content == null || content.Length == 0)
			{
				return;
			}
			Stream outputStream = response.OutputStream;
			response.ContentLength64 = (long)content.Length;
			outputStream.Write(content, 0, content.Length);
			outputStream.Close();
		}
	}
}
