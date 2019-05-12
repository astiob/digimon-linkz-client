using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting.Activation
{
	/// <summary>Defines an attribute that can be used at the call site to specify the URL where the activation will happen. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlAttribute : ContextAttribute
	{
		private string url;

		/// <summary>Creates a new instance of the <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> class.</summary>
		/// <param name="callsiteURL">The call site URL. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="callsiteURL" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		public UrlAttribute(string callsiteURL) : base(callsiteURL)
		{
			this.url = callsiteURL;
		}

		/// <summary>Gets the URL value of the <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />.</summary>
		/// <returns>The URL value of the <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string UrlValue
		{
			get
			{
				return this.url;
			}
		}

		/// <summary>Checks whether the specified object refers to the same URL as the current instance.</summary>
		/// <returns>true if the object is a <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> with the same value; otherwise, false.</returns>
		/// <param name="o">The object to compare to the current <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public override bool Equals(object o)
		{
			return o is UrlAttribute && ((UrlAttribute)o).UrlValue == this.url;
		}

		/// <summary>Returns the hash value for the current <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />.</summary>
		/// <returns>The hash value for the current <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			return this.url.GetHashCode();
		}

		/// <summary>Forces the creation of the context and the server object inside the context at the specified URL.</summary>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" /> of the server object to create. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[ComVisible(true)]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
		}

		/// <summary>Returns a Boolean value that indicates whether the specified <see cref="T:System.Runtime.Remoting.Contexts.Context" /> meets <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />'s requirements.</summary>
		/// <returns>true if the passed-in context is acceptable; otherwise, false.</returns>
		/// <param name="ctx">The context to check against the current context attribute. </param>
		/// <param name="msg">The construction call, the parameters of which need to be checked against the current context. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[ComVisible(true)]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return true;
		}
	}
}
