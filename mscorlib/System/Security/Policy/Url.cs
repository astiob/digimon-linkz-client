using Mono.Security;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Provides the URL from which a code assembly originates as evidence for policy evaluation. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class Url : IBuiltInEvidence, IIdentityPermissionFactory
	{
		private string origin_url;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.Url" /> class with the URL from which a code assembly originates.</summary>
		/// <param name="name">The URL of origin for the associated code assembly. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public Url(string name) : this(name, false)
		{
		}

		internal Url(string name, bool validated)
		{
			this.origin_url = ((!validated) ? this.Prepare(name) : name);
		}

		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return ((!verbose) ? 1 : 3) + this.origin_url.Length;
		}

		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return 0;
		}

		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			return 0;
		}

		/// <summary>Creates a new copy of the evidence object.</summary>
		/// <returns>A new, identical copy of the evidence object.</returns>
		public object Copy()
		{
			return new Url(this.origin_url, true);
		}

		/// <summary>Creates an identity permission corresponding to the current instance of the <see cref="T:System.Security.Policy.Url" /> evidence class.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.UrlIdentityPermission" /> for the specified <see cref="T:System.Security.Policy.Url" /> evidence.</returns>
		/// <param name="evidence">The evidence set from which to construct the identity permission. </param>
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new UrlIdentityPermission(this.origin_url);
		}

		/// <summary>Compares the current <see cref="T:System.Security.Policy.Url" /> evidence object to the specified object for equivalence.</summary>
		/// <returns>true if the two <see cref="T:System.Security.Policy.Url" /> objects are equal; otherwise, false.</returns>
		/// <param name="o">The <see cref="T:System.Security.Policy.Url" /> evidence object to test for equivalence with the current object. </param>
		public override bool Equals(object o)
		{
			Url url = o as Url;
			if (url == null)
			{
				return false;
			}
			string text = url.Value;
			string text2 = this.origin_url;
			if (text.IndexOf(Uri.SchemeDelimiter) < 0)
			{
				text = "file://" + text;
			}
			if (text2.IndexOf(Uri.SchemeDelimiter) < 0)
			{
				text2 = "file://" + text2;
			}
			return string.Compare(text, text2, true, CultureInfo.InvariantCulture) == 0;
		}

		/// <summary>Gets the hash code of the current URL.</summary>
		/// <returns>The hash code of the current URL.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			string text = this.origin_url;
			if (text.IndexOf(Uri.SchemeDelimiter) < 0)
			{
				text = "file://" + text;
			}
			return text.GetHashCode();
		}

		/// <summary>Returns a string representation of the current <see cref="T:System.Security.Policy.Url" />.</summary>
		/// <returns>A representation of the current <see cref="T:System.Security.Policy.Url" />.</returns>
		public override string ToString()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Url");
			securityElement.AddAttribute("version", "1");
			securityElement.AddChild(new SecurityElement("Url", this.origin_url));
			return securityElement.ToString();
		}

		/// <summary>Gets the URL from which the code assembly originates.</summary>
		/// <returns>The URL from which the code assembly originates.</returns>
		public string Value
		{
			get
			{
				return this.origin_url;
			}
		}

		private string Prepare(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("Url");
			}
			if (url == string.Empty)
			{
				throw new FormatException(Locale.GetText("Invalid (empty) Url"));
			}
			int num = url.IndexOf(Uri.SchemeDelimiter);
			if (num > 0)
			{
				if (url.StartsWith("file://"))
				{
					url = "file://" + url.Substring(7);
				}
				Uri uri = new Uri(url, false, false);
				url = uri.ToString();
			}
			int num2 = url.Length - 1;
			if (url[num2] == '/')
			{
				url = url.Substring(0, num2);
			}
			return url;
		}
	}
}
