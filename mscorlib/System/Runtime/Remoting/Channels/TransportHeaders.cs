using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Stores a collection of headers used in the channel sinks.</summary>
	[ComVisible(true)]
	[MonoTODO("Serialization format not compatible with .NET")]
	[Serializable]
	public class TransportHeaders : ITransportHeaders
	{
		private Hashtable hash_table;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Channels.TransportHeaders" /> class.</summary>
		public TransportHeaders()
		{
			this.hash_table = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
		}

		/// <summary>Gets or sets a transport header that is associated with the given key.</summary>
		/// <returns>A transport header that is associated with the given key, or null if the key was not found.</returns>
		/// <param name="key">The <see cref="T:System.String" /> that the requested header is associated with. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object this[object key]
		{
			get
			{
				return this.hash_table[key];
			}
			set
			{
				this.hash_table[key] = value;
			}
		}

		/// <summary>Returns an enumerator of the stored transport headers.</summary>
		/// <returns>An enumerator of the stored transport headers.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public IEnumerator GetEnumerator()
		{
			return this.hash_table.GetEnumerator();
		}
	}
}
