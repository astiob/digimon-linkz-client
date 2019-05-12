using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Provides the stack functionality for a stack of server channel sinks.</summary>
	[ComVisible(true)]
	public interface IServerChannelSinkStack : IServerResponseChannelSinkStack
	{
		/// <summary>Pops the information associated with all the sinks from the sink stack up to and including the specified sink.</summary>
		/// <returns>Information generated on the request side and associated with the specified sink.</returns>
		/// <param name="sink">The sink to remove and return from the sink stack. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		object Pop(IServerChannelSink sink);

		/// <summary>Pushes the specified sink and information associated with it onto the sink stack.</summary>
		/// <param name="sink">The sink to push onto the sink stack. </param>
		/// <param name="state">Information generated on the request side that is needed on the response side. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		void Push(IServerChannelSink sink, object state);

		/// <summary>Presents a callback delegate to handle a callback after a message has been dispatched asynchronously. </summary>
		/// <param name="ar">The status and state of an asynchronous operation on a remote object.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		void ServerCallback(IAsyncResult ar);

		/// <summary>Stores a message sink and its associated state for later asynchronous processing. </summary>
		/// <param name="sink">A server channel sink.</param>
		/// <param name="state">The state associated with <paramref name="sink" />.</param>
		void Store(IServerChannelSink sink, object state);

		/// <summary>Stores a message sink and its associated state, and then dispatches a message asynchronously, using the sink just stored and any other stored sinks. </summary>
		/// <param name="sink">A server channel sink.</param>
		/// <param name="state">The state associated with <paramref name="sink" />.</param>
		void StoreAndDispatch(IServerChannelSink sink, object state);
	}
}
