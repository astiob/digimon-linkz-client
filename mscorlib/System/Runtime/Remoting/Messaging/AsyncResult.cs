using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Encapsulates the results of an asynchronous operation on a delegate.</summary>
	[ComVisible(true)]
	public class AsyncResult : IAsyncResult, IMessageSink
	{
		private object async_state;

		private WaitHandle handle;

		private object async_delegate;

		private IntPtr data;

		private object object_data;

		private bool sync_completed;

		private bool completed;

		private bool endinvoke_called;

		private object async_callback;

		private ExecutionContext current;

		private ExecutionContext original;

		private int gchandle;

		private MonoMethodMessage call_message;

		private IMessageCtrl message_ctrl;

		private IMessage reply_message;

		internal AsyncResult()
		{
		}

		/// <summary>Gets the object provided as the last parameter of a BeginInvoke method call.</summary>
		/// <returns>The object provided as the last parameter of a BeginInvoke method call.</returns>
		public virtual object AsyncState
		{
			get
			{
				return this.async_state;
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.WaitHandle" /> that encapsulates Win32 synchronization handles, and allows the implementation of various synchronization schemes.</summary>
		/// <returns>A <see cref="T:System.Threading.WaitHandle" /> that encapsulates Win32 synchronization handles, and allows the implementation of various synchronization schemes.</returns>
		public virtual WaitHandle AsyncWaitHandle
		{
			get
			{
				WaitHandle result;
				lock (this)
				{
					if (this.handle == null)
					{
						this.handle = new ManualResetEvent(this.completed);
					}
					result = this.handle;
				}
				return result;
			}
		}

		/// <summary>Gets a value indicating whether the BeginInvoke call completed synchronously.</summary>
		/// <returns>true if the BeginInvoke call completed synchronously; otherwise, false.</returns>
		public virtual bool CompletedSynchronously
		{
			get
			{
				return this.sync_completed;
			}
		}

		/// <summary>Gets a value indicating whether the server has completed the call.</summary>
		/// <returns>true after the server has completed the call; otherwise, false.</returns>
		public virtual bool IsCompleted
		{
			get
			{
				return this.completed;
			}
		}

		/// <summary>Gets or sets a value indicating whether EndInvoke has been called on the current <see cref="T:System.Runtime.Remoting.Messaging.AsyncResult" />.</summary>
		/// <returns>true if EndInvoke has been called on the current <see cref="T:System.Runtime.Remoting.Messaging.AsyncResult" />; otherwise, false.</returns>
		public bool EndInvokeCalled
		{
			get
			{
				return this.endinvoke_called;
			}
			set
			{
				this.endinvoke_called = value;
			}
		}

		/// <summary>Gets the delegate object on which the asynchronous call was invoked.</summary>
		/// <returns>The delegate object on which the asynchronous call was invoked.</returns>
		public virtual object AsyncDelegate
		{
			get
			{
				return this.async_delegate;
			}
		}

		/// <summary>Gets the next message sink in the sink chain.</summary>
		/// <returns>An <see cref="T:System.Runtime.Remoting.Messaging.IMessageSink" /> interface that represents the next message sink in the sink chain.</returns>
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Remoting.Messaging.IMessageSink" /> interface.</summary>
		/// <returns>No value is returned.</returns>
		/// <param name="msg">The request <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> interface. </param>
		/// <param name="replySink">The response <see cref="T:System.Runtime.Remoting.Messaging.IMessageSink" /> interface. </param>
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		/// <summary>Gets the response message for the asynchronous call.</summary>
		/// <returns>A remoting message that should represent a response to a method call on a remote object.</returns>
		public virtual IMessage GetReplyMessage()
		{
			return this.reply_message;
		}

		/// <summary>Sets an <see cref="T:System.Runtime.Remoting.Messaging.IMessageCtrl" /> for the current remote method call, which provides a way to control asynchronous messages after they have been dispatched.</summary>
		/// <param name="mc">The <see cref="T:System.Runtime.Remoting.Messaging.IMessageCtrl" /> for the current remote method call. </param>
		public virtual void SetMessageCtrl(IMessageCtrl mc)
		{
			this.message_ctrl = mc;
		}

		internal void SetCompletedSynchronously(bool completed)
		{
			this.sync_completed = completed;
		}

		internal IMessage EndInvoke()
		{
			lock (this)
			{
				if (this.completed)
				{
					return this.reply_message;
				}
			}
			this.AsyncWaitHandle.WaitOne();
			return this.reply_message;
		}

		/// <summary>Synchronously processes a response message returned by a method call on a remote object.</summary>
		/// <returns>Returns null.</returns>
		/// <param name="msg">A response message to a method call on a remote object.</param>
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			this.reply_message = msg;
			lock (this)
			{
				this.completed = true;
				if (this.handle != null)
				{
					((ManualResetEvent)this.AsyncWaitHandle).Set();
				}
			}
			if (this.async_callback != null)
			{
				AsyncCallback asyncCallback = (AsyncCallback)this.async_callback;
				asyncCallback(this);
			}
			return null;
		}

		internal MonoMethodMessage CallMessage
		{
			get
			{
				return this.call_message;
			}
			set
			{
				this.call_message = value;
			}
		}
	}
}
