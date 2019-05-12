using System;
using System.Security.Cryptography;
using System.Text;

namespace System.Net
{
	internal class DigestSession
	{
		private static RandomNumberGenerator rng = RandomNumberGenerator.Create();

		private DateTime lastUse;

		private int _nc;

		private HashAlgorithm hash;

		private DigestHeaderParser parser;

		private string _cnonce;

		public DigestSession()
		{
			this._nc = 1;
			this.lastUse = DateTime.Now;
		}

		public string Algorithm
		{
			get
			{
				return this.parser.Algorithm;
			}
		}

		public string Realm
		{
			get
			{
				return this.parser.Realm;
			}
		}

		public string Nonce
		{
			get
			{
				return this.parser.Nonce;
			}
		}

		public string Opaque
		{
			get
			{
				return this.parser.Opaque;
			}
		}

		public string QOP
		{
			get
			{
				return this.parser.QOP;
			}
		}

		public string CNonce
		{
			get
			{
				if (this._cnonce == null)
				{
					byte[] array = new byte[15];
					DigestSession.rng.GetBytes(array);
					this._cnonce = Convert.ToBase64String(array);
					Array.Clear(array, 0, array.Length);
				}
				return this._cnonce;
			}
		}

		public bool Parse(string challenge)
		{
			this.parser = new DigestHeaderParser(challenge);
			if (!this.parser.Parse())
			{
				return false;
			}
			if (this.parser.Algorithm == null || this.parser.Algorithm.ToUpper().StartsWith("MD5"))
			{
				this.hash = HashAlgorithm.Create("MD5");
			}
			return true;
		}

		private string HashToHexString(string toBeHashed)
		{
			if (this.hash == null)
			{
				return null;
			}
			this.hash.Initialize();
			byte[] array = this.hash.ComputeHash(Encoding.ASCII.GetBytes(toBeHashed));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		private string HA1(string username, string password)
		{
			string toBeHashed = string.Format("{0}:{1}:{2}", username, this.Realm, password);
			if (this.Algorithm != null && this.Algorithm.ToLower() == "md5-sess")
			{
				toBeHashed = string.Format("{0}:{1}:{2}", this.HashToHexString(toBeHashed), this.Nonce, this.CNonce);
			}
			return this.HashToHexString(toBeHashed);
		}

		private string HA2(HttpWebRequest webRequest)
		{
			string toBeHashed = string.Format("{0}:{1}", webRequest.Method, webRequest.RequestUri.PathAndQuery);
			if (this.QOP == "auth-int")
			{
			}
			return this.HashToHexString(toBeHashed);
		}

		private string Response(string username, string password, HttpWebRequest webRequest)
		{
			string text = string.Format("{0}:{1}:", this.HA1(username, password), this.Nonce);
			if (this.QOP != null)
			{
				text += string.Format("{0}:{1}:{2}:", this._nc.ToString("X8"), this.CNonce, this.QOP);
			}
			text += this.HA2(webRequest);
			return this.HashToHexString(text);
		}

		public Authorization Authenticate(WebRequest webRequest, ICredentials credentials)
		{
			if (this.parser == null)
			{
				throw new InvalidOperationException();
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			this.lastUse = DateTime.Now;
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.RequestUri, "digest");
			if (credential == null)
			{
				return null;
			}
			string userName = credential.UserName;
			if (userName == null || userName == string.Empty)
			{
				return null;
			}
			string password = credential.Password;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Digest username=\"{0}\", ", userName);
			stringBuilder.AppendFormat("realm=\"{0}\", ", this.Realm);
			stringBuilder.AppendFormat("nonce=\"{0}\", ", this.Nonce);
			stringBuilder.AppendFormat("uri=\"{0}\", ", httpWebRequest.Address.PathAndQuery);
			if (this.Algorithm != null)
			{
				stringBuilder.AppendFormat("algorithm=\"{0}\", ", this.Algorithm);
			}
			stringBuilder.AppendFormat("response=\"{0}\", ", this.Response(userName, password, httpWebRequest));
			if (this.QOP != null)
			{
				stringBuilder.AppendFormat("qop=\"{0}\", ", this.QOP);
			}
			lock (this)
			{
				if (this.QOP != null)
				{
					stringBuilder.AppendFormat("nc={0:X8}, ", this._nc);
					this._nc++;
				}
			}
			if (this.CNonce != null)
			{
				stringBuilder.AppendFormat("cnonce=\"{0}\", ", this.CNonce);
			}
			if (this.Opaque != null)
			{
				stringBuilder.AppendFormat("opaque=\"{0}\", ", this.Opaque);
			}
			stringBuilder.Length -= 2;
			return new Authorization(stringBuilder.ToString());
		}

		public DateTime LastUse
		{
			get
			{
				return this.lastUse;
			}
		}
	}
}
