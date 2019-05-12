using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Holds the stack of server channel sinks.</summary>
	[ComVisible(true)]
	public class ServerChannelSinkStack : IServerChannelSinkStack, IServerResponseChannelSinkStack
	{
		private ChanelSinkStackEntry _sinkStack;

		/// <summary>Returns the <see cref="T:System.IO.Stream" /> onto which the specified message is to be serialized.</summary>
		/// <returns>The <see cref="T:System.IO.Stream" /> onto which the specified message is to be serialized.</returns>
		/// <param name="msg">The message to be serialized onto the requested stream. </param>
		/// <param name="headers">The headers retrieved from the server response stream. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The sink stack is empty. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public Stream GetResponseStream(IMessage msg, ITransportHeaders headers)
		{
			if (this._sinkStack == null)
			{
				throw new RemotingException("The sink stack is empty");
			}
			return ((IServerChannelSink)this._sinkStack.Sink).GetResponseStream(this, this._sinkStack.State, msg, headers);
		}

		/// <summary>Pops the information associated with all the sinks from the sink stack up to and including the specified sink.</summary>
		/// <returns>Information generated on the request side and associated with the specified sink.</returns>
		/// <param name="sink">The sink to remove and return from the sink stack. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The current sink stack is empty, or the specified sink was never pushed onto the current stack. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object Pop(IServerChannelSink sink)
		{
			while (this._sinkStack != null)
			{
				ChanelSinkStackEntry sinkStack = this._sinkStack;
				this._sinkStack = this._sinkStack.Next;
				if (sinkStack.Sink == sink)
				{
					return sinkStack.State;
				}
			}
			throw new RemotingException("The current sink stack is empty, or the specified sink was never pushed onto the current stack");
		}

		/// <summary>Pushes the specified sink and information associated with it onto the sink stack.</summary>
		/// <param name="sink">The sink to push onto the sink stack. </param>
		/// <param name="state">Information generated on the request side that is needed on the response side. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void Push(IServerChannelSink sink, object state)
		{
			this._sinkStack = new ChanelSinkStackEntry(sink, state, this._sinkStack);
		}

		/// <summary>Provides a <see cref="T:System.AsyncCallback" /> delegate to handle a callback after a message has been dispatched asynchronously. </summary>
		/// <param name="ar">The status and state of an asynchronous operation on a remote object.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public void ServerCallback(IAsyncResult ar)
		{
			throw new NotImplementedException();
		}

		/// <summary>Stores a message sink and its associated state for later asynchronous processing. </summary>
		/// <param name="sink">A server channel sink.</param>
		/// <param name="state">The state associated with <paramref name="sink" />.</param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The current sink stack is empty.-or-The specified sink was never pushed onto the current stack. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public void Store(IServerChannelSink sink, object state)
		{
			throw new NotImplementedException();
		}

		/// <summary>Stores a message sink and its associated state, and then dispatches a message asynchronously, using the sink just stored and any other stored sinks. </summary>
		/// <param name="sink">A server channel sink.</param>
		/// <param name="state">The state associated with <paramref name="sink" />.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public void StoreAndDispatch(IServerChannelSink sink, object state)
		{
			throw new NotImplementedException();
		}

		/// <summary>Requests asynchronous processing of a method call on the sinks in the current sink stack.</summary>
		/// <param name="msg">The message to be serialized onto the requested stream.</param>
		/// <param name="headers">The headers retrieved from the server response stream. </param>
		/// <param name="stream">The stream coming back from the transport sink. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The current sink stack is empty. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void AsyncProcessResponse(IMessage msg, ITransportHeaders headers, Stream stream)
		{
			if (this._sinkStack == null)
			{
				throw new RemotingException("The current sink stack is empty");
			}
			ChanelSinkStackEntry sinkStack = this._sinkStack;
			this._sinkStack = this._sinkStack.Next;
			((IServerChannelSink)sinkStack.Sink).AsyncProcessResponse(this, sinkStack.State, msg, headers, stream);
		}
	}
}
