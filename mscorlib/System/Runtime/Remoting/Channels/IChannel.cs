using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Provides conduits for messages that cross remoting boundaries.</summary>
	[ComVisible(true)]
	public interface IChannel
	{
		/// <summary>Gets the name of the channel.</summary>
		/// <returns>The name of the channel.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		string ChannelName { get; }

		/// <summary>Gets the priority of the channel.</summary>
		/// <returns>An integer that indicates the priority of the channel.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		int ChannelPriority { get; }

		/// <summary>Returns the object URI as an out parameter, and the URI of the current channel as the return value.</summary>
		/// <returns>The URI of the current channel, or null if the URI does not belong to this channel.</returns>
		/// <param name="url">The URL of the object. </param>
		/// <param name="objectURI">When this method returns, contains a <see cref="T:System.String" /> that holds the object URI. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		string Parse(string url, out string objectURI);
	}
}
