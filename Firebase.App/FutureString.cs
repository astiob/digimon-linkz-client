using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Firebase
{
	internal class FutureString : FutureBase
	{
		private HandleRef swigCPtr;

		private static Dictionary<int, FutureString.Action> Callbacks;

		private static int CallbackIndex = 0;

		private static object CallbackLock = new object();

		private IntPtr callbackData = IntPtr.Zero;

		private FutureString.SWIG_CompletionDelegate SWIG_CompletionCB;

		internal FutureString(IntPtr cPtr, bool cMemoryOwn) : base(AppUtilPINVOKE.FutureString_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public FutureString() : this(AppUtilPINVOKE.new_FutureString(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(FutureString obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~FutureString()
		{
			this.Dispose();
		}

		public override void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AppUtilPINVOKE.delete_FutureString(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				this.SetCompletionData(IntPtr.Zero);
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public static Task<string> GetTask(FutureString fu)
		{
			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
			if (fu.status() == FutureStatus.Invalid)
			{
				tcs.SetException(new FirebaseException(0, "Asynchronous operation was not started."));
				return tcs.Task;
			}
			fu.SetOnCompletionCallback(delegate
			{
				if (fu.status() == FutureStatus.Invalid)
				{
					tcs.SetCanceled();
				}
				else
				{
					int num = fu.error();
					if (num != 0)
					{
						tcs.SetException(new FirebaseException(num, fu.error_message()));
					}
					else
					{
						tcs.SetResult(fu.result());
					}
				}
				fu.Dispose();
			});
			return tcs.Task;
		}

		public void SetOnCompletionCallback(FutureString.Action userCompletionCallback)
		{
			if (this.SWIG_CompletionCB == null)
			{
				this.SWIG_CompletionCB = new FutureString.SWIG_CompletionDelegate(FutureString.SWIG_CompletionDispatcher);
			}
			object callbackLock = FutureString.CallbackLock;
			int num;
			lock (callbackLock)
			{
				if (FutureString.Callbacks == null)
				{
					FutureString.Callbacks = new Dictionary<int, FutureString.Action>();
				}
				num = ++FutureString.CallbackIndex;
				FutureString.Callbacks[num] = userCompletionCallback;
			}
			this.SetCompletionData(this.SWIG_OnCompletion(this.SWIG_CompletionCB, num));
		}

		private void SetCompletionData(IntPtr data)
		{
			if (this.callbackData != IntPtr.Zero)
			{
				this.SWIG_FreeCompletionData(this.callbackData);
			}
			this.callbackData = data;
		}

		[FutureString.MonoPInvokeCallbackAttribute(typeof(FutureString.SWIG_CompletionDelegate))]
		private static void SWIG_CompletionDispatcher(int key)
		{
			FutureString.Action action = null;
			object callbackLock = FutureString.CallbackLock;
			lock (callbackLock)
			{
				if (FutureString.Callbacks != null && FutureString.Callbacks.TryGetValue(key, out action))
				{
					FutureString.Callbacks.Remove(key);
				}
			}
			if (action != null)
			{
				action();
			}
		}

		internal IntPtr SWIG_OnCompletion(FutureString.SWIG_CompletionDelegate cs_callback, int cs_key)
		{
			IntPtr result = AppUtilPINVOKE.FutureString_SWIG_OnCompletion(this.swigCPtr, cs_callback, cs_key);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void SWIG_FreeCompletionData(IntPtr data)
		{
			AppUtilPINVOKE.FutureString_SWIG_FreeCompletionData(this.swigCPtr, data);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public string result()
		{
			string result = AppUtilPINVOKE.FutureString_result(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public delegate void Action();

		[AttributeUsage(AttributeTargets.Method)]
		private sealed class MonoPInvokeCallbackAttribute : Attribute
		{
			public MonoPInvokeCallbackAttribute(Type t)
			{
			}
		}

		internal delegate void SWIG_CompletionDelegate(int index);
	}
}
