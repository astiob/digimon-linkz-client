using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Stores a collection of headers used in the channel sinks.</summary>
	[ComVisible(true)]
	public interface ITransportHeaders
	{
		/// <summary>Gets or sets a transport header associated with the given key.</summary>
		/// <returns>A transport header associated with the given key.</returns>
		/// <param name="key">The key the requested transport header is associated with. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		object this[object key]
		{
			get;
			set;
		}

		/// <summary>Returns a <see cref="T:System.Collections.IEnumerator" /> that iterates over all entries in the <see cref="T:System.Runtime.Remoting.Channels.ITransportHeaders" /> object.</summary>
		/// <returns>A <see cref="T:System.Collections.IEnumerator" /> that iterates over all entries in the <see cref="T:System.Runtime.Remoting.Channels.ITransportHeaders" /> object.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		IEnumerator GetEnumerator();
	}
}
