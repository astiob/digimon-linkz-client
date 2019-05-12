using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>Represents a handle that has been registered when calling <see cref="M:System.Threading.ThreadPool.RegisterWaitForSingleObject(System.Threading.WaitHandle,System.Threading.WaitOrTimerCallback,System.Object,System.UInt32,System.Boolean)" />. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public sealed class RegisteredWaitHandle : MarshalByRefObject
	{
		private WaitHandle _waitObject;

		private WaitOrTimerCallback _callback;

		private TimeSpan _timeout;

		private object _state;

		private bool _executeOnlyOnce;

		private WaitHandle _finalEvent;

		private ManualResetEvent _cancelEvent;

		private int _callsInProcess;

		private bool _unregistered;

		internal RegisteredWaitHandle(WaitHandle waitObject, WaitOrTimerCallback callback, object state, TimeSpan timeout, bool executeOnlyOnce)
		{
			this._waitObject = waitObject;
			this._callback = callback;
			this._state = state;
			this._timeout = timeout;
			this._executeOnlyOnce = executeOnlyOnce;
			this._finalEvent = null;
			this._cancelEvent = new ManualResetEvent(false);
			this._callsInProcess = 0;
			this._unregistered = false;
		}

		internal void Wait(object state)
		{
			try
			{
				WaitHandle[] waitHandles = new WaitHandle[]
				{
					this._waitObject,
					this._cancelEvent
				};
				do
				{
					int num = WaitHandle.WaitAny(waitHandles, this._timeout, false);
					if (!this._unregistered)
					{
						lock (this)
						{
							this._callsInProcess++;
						}
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.DoCallBack), num == 258);
					}
				}
				while (!this._unregistered && !this._executeOnlyOnce);
			}
			catch
			{
			}
			lock (this)
			{
				this._unregistered = true;
				if (this._callsInProcess == 0 && this._finalEvent != null)
				{
					NativeEventCalls.SetEvent_internal(this._finalEvent.Handle);
				}
			}
		}

		private void DoCallBack(object timedOut)
		{
			if (this._callback != null)
			{
				this._callback(this._state, (bool)timedOut);
			}
			lock (this)
			{
				this._callsInProcess--;
				if (this._unregistered && this._callsInProcess == 0 && this._finalEvent != null)
				{
					NativeEventCalls.SetEvent_internal(this._finalEvent.Handle);
				}
			}
		}

		/// <summary>Cancels a registered wait operation issued by the <see cref="M:System.Threading.ThreadPool.RegisterWaitForSingleObject(System.Threading.WaitHandle,System.Threading.WaitOrTimerCallback,System.Object,System.UInt32,System.Boolean)" /> method.</summary>
		/// <returns>true if the function succeeds; otherwise, false.</returns>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to be signaled. </param>
		/// <filterpriority>2</filterpriority>
		[ComVisible(true)]
		public bool Unregister(WaitHandle waitObject)
		{
			bool result;
			lock (this)
			{
				if (this._unregistered)
				{
					result = false;
				}
				else
				{
					this._finalEvent = waitObject;
					this._unregistered = true;
					this._cancelEvent.Set();
					result = true;
				}
			}
			return result;
		}
	}
}
