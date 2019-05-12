using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Holds the stack of client channel sinks that must be invoked during an asynchronous message response decoding.</summary>
	[ComVisible(true)]
	public class ClientChannelSinkStack : IClientChannelSinkStack, IClientResponseChannelSinkStack
	{
		private IMessageSink _replySink;

		private ChanelSinkStackEntry _sinkStack;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Channels.ClientChannelSinkStack" /> class with default values.</summary>
		public ClientChannelSinkStack()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Channels.ClientChannelSinkStack" /> class with the specified reply sink.</summary>
		/// <param name="replySink">The <see cref="T:System.Runtime.Remoting.Messaging.IMessageSink" /> that the current stack can use to reply to messages. </param>
		public ClientChannelSinkStack(IMessageSink replySink)
		{
			this._replySink = replySink;
		}

		/// <summary>Requests asynchronous processing of a method call on the sinks that are in the current sink stack.</summary>
		/// <param name="headers">The headers that are retrieved from the server response stream. </param>
		/// <param name="stream">The stream that is returning from the transport sink. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The current sink stack is empty. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void AsyncProcessResponse(ITransportHeaders headers, Stream stream)
		{
			if (this._sinkStack == null)
			{
				throw new RemotingException("The current sink stack is empty");
			}
			ChanelSinkStackEntry sinkStack = this._sinkStack;
			this._sinkStack = this._sinkStack.Next;
			((IClientChannelSink)sinkStack.Sink).AsyncProcessResponse(this, sinkStack.State, headers, stream);
		}

		/// <summary>Dispatches the specified exception on the reply sink.</summary>
		/// <param name="e">The exception to dispatch to the server. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void DispatchException(Exception e)
		{
			this.DispatchReplyMessage(new ReturnMessage(e, null));
		}

		/// <summary>Dispatches the specified reply message on the reply sink.</summary>
		/// <param name="msg">The <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> to dispatch. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void DispatchReplyMessage(IMessage msg)
		{
			if (this._replySink != null)
			{
				this._replySink.SyncProcessMessage(msg);
			}
		}

		/// <summary>Pops the information that is associated with all the sinks from the sink stack up to and including the specified sink.</summary>
		/// <returns>Information generated on the request side and associated with the specified sink.</returns>
		/// <param name="sink">The sink to remove and return from the sink stack. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The current sink stack is empty, or the specified sink was never pushed onto the current stack. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object Pop(IClientChannelSink sink)
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

		/// <summary>Pushes the specified sink and information that is associated with it onto the sink stack.</summary>
		/// <param name="sink">The sink to push onto the sink stack. </param>
		/// <param name="state">Information generated on the request side that is needed on the response side. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void Push(IClientChannelSink sink, object state)
		{
			this._sinkStack = new ChanelSinkStackEntry(sink, state, this._sinkStack);
		}
	}
}
