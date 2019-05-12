using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OAuth
{
	public static class OAuthTools
	{
		private const string AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

		private const string Digit = "1234567890";

		private const string Lower = "abcdefghijklmnopqrstuvwxyz";

		private const string Unreserved = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-._~";

		private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private static readonly Random _random;

		private static readonly object _randomLock = new object();

		private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

		private static readonly Encoding _encoding = Encoding.UTF8;

		static OAuthTools()
		{
			byte[] array = new byte[4];
			OAuthTools._rng.GetNonZeroBytes(array);
			OAuthTools._random = new Random(BitConverter.ToInt32(array, 0));
		}

		public static string GetNonce()
		{
			char[] array = new char[16];
			object randomLock = OAuthTools._randomLock;
			lock (randomLock)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = "abcdefghijklmnopqrstuvwxyz1234567890"[OAuthTools._random.Next(0, "abcdefghijklmnopqrstuvwxyz1234567890".Length)];
				}
			}
			return new string(array);
		}

		public static string GetTimestamp()
		{
			return OAuthTools.GetTimestamp(DateTime.UtcNow);
		}

		public static string GetTimestamp(DateTime dateTime)
		{
			return OAuthTools.ToUnixTime(dateTime).ToString();
		}

		private static long ToUnixTime(DateTime dateTime)
		{
			return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		public static string UrlEncodeRelaxed(string value)
		{
			string text = Uri.EscapeDataString(value);
			return text.Replace("(", OAuthTools.PercentEncode("(")).Replace(")", OAuthTools.PercentEncode(")"));
		}

		private static string PercentEncode(string s)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in bytes)
			{
				if ((b > 7 && b < 11) || b == 13)
				{
					stringBuilder.Append(string.Format("%0{0:X}", b));
				}
				else
				{
					stringBuilder.Append(string.Format("%{0:X}", b));
				}
			}
			return stringBuilder.ToString();
		}

		public static string UrlEncodeStrict(string value)
		{
			string text = value.Where((char c) => !"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-._~".Contains(c) && c != '%').Aggregate(value, (string current, char c) => current.Replace(c.ToString(), OAuthTools.PercentEncode(c.ToString())));
			return text.Replace("%%", "%25%");
		}

		public static string NormalizeRequestParameters(WebParameterCollection parameters)
		{
			WebParameterCollection collection = OAuthTools.SortParametersExcludingSignature(parameters);
			return OAuthTools.Concatenate(collection, "=", "&");
		}

		private static string Concatenate(ICollection<WebParameter> collection, string separator, string spacer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = collection.Count;
			int num = 0;
			foreach (WebParameter webParameter in collection)
			{
				stringBuilder.Append(webParameter.Name);
				stringBuilder.Append(separator);
				stringBuilder.Append(webParameter.Value);
				num++;
				if (num < count)
				{
					stringBuilder.Append(spacer);
				}
			}
			return stringBuilder.ToString();
		}

		public static WebParameterCollection SortParametersExcludingSignature(WebParameterCollection parameters)
		{
			WebParameterCollection webParameterCollection = new WebParameterCollection(parameters);
			IEnumerable<WebParameter> parameters2 = webParameterCollection.Where((WebParameter n) => OAuthTools.EqualsIgnoreCase(n.Name, "oauth_signature"));
			webParameterCollection.RemoveAll(parameters2);
			foreach (WebParameter webParameter in webParameterCollection)
			{
				webParameter.Value = OAuthTools.UrlEncodeStrict(webParameter.Value);
			}
			webParameterCollection.Sort((WebParameter x, WebParameter y) => (!x.Name.Equals(y.Name)) ? x.Name.CompareTo(y.Name) : x.Value.CompareTo(y.Value));
			return webParameterCollection;
		}

		private static bool EqualsIgnoreCase(string left, string right)
		{
			return string.Compare(left, right, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		public static string ConstructRequestUrl(Uri url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			StringBuilder stringBuilder = new StringBuilder();
			string value = string.Format("{0}://{1}", url.Scheme, url.Host);
			string text = string.Format(":{0}", url.Port);
			bool flag = url.Scheme == "http" && url.Port == 80;
			bool flag2 = url.Scheme == "https" && url.Port == 443;
			stringBuilder.Append(value);
			stringBuilder.Append((flag || flag2) ? string.Empty : text);
			stringBuilder.Append(url.AbsolutePath);
			return stringBuilder.ToString();
		}

		public static string ConcatenateRequestElements(string method, string url, WebParameterCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = method.ToUpper() + "&";
			string value2 = OAuthTools.UrlEncodeRelaxed(OAuthTools.ConstructRequestUrl(new Uri(url))) + "&";
			string value3 = OAuthTools.UrlEncodeRelaxed(OAuthTools.NormalizeRequestParameters(parameters));
			stringBuilder.Append(value);
			stringBuilder.Append(value2);
			stringBuilder.Append(value3);
			return stringBuilder.ToString();
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, string signatureBase, string consumerSecret)
		{
			return OAuthTools.GetSignature(signatureMethod, OAuthSignatureTreatment.Escaped, signatureBase, consumerSecret, null);
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, OAuthSignatureTreatment signatureTreatment, string signatureBase, string consumerSecret)
		{
			return OAuthTools.GetSignature(signatureMethod, signatureTreatment, signatureBase, consumerSecret, null);
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, string signatureBase, string consumerSecret, string tokenSecret)
		{
			return OAuthTools.GetSignature(signatureMethod, OAuthSignatureTreatment.Escaped, consumerSecret, tokenSecret);
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, OAuthSignatureTreatment signatureTreatment, string signatureBase, string consumerSecret, string tokenSecret)
		{
			if (OAuthTools.IsNullOrBlank(tokenSecret))
			{
				tokenSecret = string.Empty;
			}
			consumerSecret = OAuthTools.UrlEncodeRelaxed(consumerSecret);
			tokenSecret = OAuthTools.UrlEncodeRelaxed(tokenSecret);
			if (signatureMethod != OAuthSignatureMethod.HmacSha1)
			{
				throw new NotImplementedException("Only HMAC-SHA1 is currently supported.");
			}
			HMACSHA1 hmacsha = new HMACSHA1();
			string s = consumerSecret + "&" + tokenSecret;
			hmacsha.Key = OAuthTools._encoding.GetBytes(s);
			string text = OAuthTools.HashWith(signatureBase, hmacsha);
			return (signatureTreatment != OAuthSignatureTreatment.Escaped) ? text : OAuthTools.UrlEncodeRelaxed(text);
		}

		private static string HashWith(string input, HashAlgorithm algorithm)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			byte[] inArray = algorithm.ComputeHash(bytes);
			return Convert.ToBase64String(inArray);
		}

		private static bool IsNullOrBlank(string value)
		{
			return string.IsNullOrEmpty(value) || (!string.IsNullOrEmpty(value) && value.Trim() == string.Empty);
		}
	}
}
