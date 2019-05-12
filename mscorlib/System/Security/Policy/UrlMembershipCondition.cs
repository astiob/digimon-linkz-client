using Mono.Security;
using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Determines whether an assembly belongs to a code group by testing its URL. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlMembershipCondition : IConstantMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		private readonly int version = 1;

		private Url url;

		private string userUrl;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.UrlMembershipCondition" /> class with the URL that determines membership.</summary>
		/// <param name="url">The URL for which to test. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="url" /> parameter is null. </exception>
		public UrlMembershipCondition(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			this.CheckUrl(url);
			this.userUrl = url;
			this.url = new Url(url);
		}

		internal UrlMembershipCondition(Url url, string userUrl)
		{
			this.url = (Url)url.Copy();
			this.userUrl = userUrl;
		}

		/// <summary>Gets or sets the URL for which the membership condition tests.</summary>
		/// <returns>The URL for which the membership condition tests.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt is made to set <see cref="P:System.Security.Policy.UrlMembershipCondition.Url" /> to null. </exception>
		public string Url
		{
			get
			{
				if (this.userUrl == null)
				{
					this.userUrl = this.url.Value;
				}
				return this.userUrl;
			}
			set
			{
				this.url = new Url(value);
			}
		}

		/// <summary>Determines whether the specified evidence satisfies the membership condition.</summary>
		/// <returns>true if the specified evidence satisfies the membership condition; otherwise, false.</returns>
		/// <param name="evidence">The evidence set against which to make the test. </param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Security.Policy.UrlMembershipCondition.Url" /> property is null. </exception>
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			string value = this.url.Value;
			int num = value.LastIndexOf("*");
			if (num == -1)
			{
				num = value.Length;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				if (hostEnumerator.Current is Url && string.Compare(value, 0, (hostEnumerator.Current as Url).Value, 0, num, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Creates an equivalent copy of the membership condition.</summary>
		/// <returns>A new, identical copy of the current membership condition.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Security.Policy.UrlMembershipCondition.Url" /> property is null. </exception>
		public IMembershipCondition Copy()
		{
			return new UrlMembershipCondition(this.url, this.userUrl);
		}

		/// <summary>Determines whether the URL from the specified object is equivalent to the URL contained in the current <see cref="T:System.Security.Policy.UrlMembershipCondition" />.</summary>
		/// <returns>true if the URL from the specified object is equivalent to the URL contained in the current <see cref="T:System.Security.Policy.UrlMembershipCondition" />; otherwise, false.</returns>
		/// <param name="o">The object to compare to the current <see cref="T:System.Security.Policy.UrlMembershipCondition" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Security.Policy.UrlMembershipCondition.Url" /> property of the current object or the specified object is null. </exception>
		public override bool Equals(object o)
		{
			UrlMembershipCondition urlMembershipCondition = o as UrlMembershipCondition;
			if (o == null)
			{
				return false;
			}
			string value = this.url.Value;
			int num = value.Length;
			if (value[num - 1] == '*')
			{
				num--;
				if (value[num - 1] == '/')
				{
					num--;
				}
			}
			return string.Compare(value, 0, urlMembershipCondition.Url, 0, num, true, CultureInfo.InvariantCulture) == 0;
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="e" /> parameter is not a valid membership condition element. </exception>
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object. </param>
		/// <param name="level">The policy level context, used to resolve named permission set references. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="e" /> parameter is not a valid membership condition element. </exception>
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
			string text = e.Attribute("Url");
			if (text != null)
			{
				this.CheckUrl(text);
				this.url = new Url(text);
			}
			else
			{
				this.url = null;
			}
			this.userUrl = text;
		}

		/// <summary>Gets the hash code for the current membership condition.</summary>
		/// <returns>The hash code for the current membership condition.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Security.Policy.UrlMembershipCondition.Url" /> property is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			return this.url.GetHashCode();
		}

		/// <summary>Creates and returns a string representation of the membership condition.</summary>
		/// <returns>A string representation of the state of the membership condition.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Security.Policy.UrlMembershipCondition.Url" /> property is null. </exception>
		public override string ToString()
		{
			return "Url - " + this.Url;
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		/// <summary>Creates an XML encoding of the security object and its current state with the specified <see cref="T:System.Security.Policy.PolicyLevel" />.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <param name="level">The policy level context for resolving named permission set references. </param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Security.Policy.UrlMembershipCondition.Url" /> property is null. </exception>
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = MembershipConditionHelper.Element(typeof(UrlMembershipCondition), this.version);
			securityElement.AddAttribute("Url", this.userUrl);
			return securityElement;
		}

		internal void CheckUrl(string url)
		{
			int num = url.IndexOf(Uri.SchemeDelimiter);
			string uriString = (num >= 0) ? url : ("file://" + url);
			Uri uri = new Uri(uriString, false, false);
			if (uri.Host.IndexOf('*') >= 1)
			{
				string text = Locale.GetText("Invalid * character in url");
				throw new ArgumentException(text, "name");
			}
		}
	}
}
