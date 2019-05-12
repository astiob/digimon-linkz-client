using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Firebase
{
	internal class FutureVoid : FutureBase
	{
		private HandleRef swigCPtr;

		private static Dictionary<int, FutureVoid.Action> Callbacks;

		private static int CallbackIndex = 0;

		private static object CallbackLock = new object();

		private IntPtr callbackData = IntPtr.Zero;

		private FutureVoid.SWIG_CompletionDelegate SWIG_CompletionCB;

		internal FutureVoid(IntPtr cPtr, bool cMemoryOwn) : base(AppUtilPINVOKE.FutureVoid_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public FutureVoid() : this(AppUtilPINVOKE.new_FutureVoid(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(FutureVoid obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~FutureVoid()
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
						AppUtilPINVOKE.delete_FutureVoid(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				this.SetCompletionData(IntPtr.Zero);
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public static Task GetTask(FutureVoid fu)
		{
			TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
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
						tcs.SetResult(0);
					}
				}
				fu.Dispose();
			});
			return tcs.Task;
		}

		public void SetOnCompletionCallback(FutureVoid.Action userCompletionCallback)
		{
			if (this.SWIG_CompletionCB == null)
			{
				this.SWIG_CompletionCB = new FutureVoid.SWIG_CompletionDelegate(FutureVoid.SWIG_CompletionDispatcher);
			}
			object callbackLock = FutureVoid.CallbackLock;
			int num;
			lock (callbackLock)
			{
				if (FutureVoid.Callbacks == null)
				{
					FutureVoid.Callbacks = new Dictionary<int, FutureVoid.Action>();
				}
				num = ++FutureVoid.CallbackIndex;
				FutureVoid.Callbacks[num] = userCompletionCallback;
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

		[FutureVoid.MonoPInvokeCallbackAttribute(typeof(FutureVoid.SWIG_CompletionDelegate))]
		private static void SWIG_CompletionDispatcher(int key)
		{
			FutureVoid.Action action = null;
			object callbackLock = FutureVoid.CallbackLock;
			lock (callbackLock)
			{
				if (FutureVoid.Callbacks != null && FutureVoid.Callbacks.TryGetValue(key, out action))
				{
					FutureVoid.Callbacks.Remove(key);
				}
			}
			if (action != null)
			{
				action();
			}
		}

		internal IntPtr SWIG_OnCompletion(FutureVoid.SWIG_CompletionDelegate cs_callback, int cs_key)
		{
			IntPtr result = AppUtilPINVOKE.FutureVoid_SWIG_OnCompletion(this.swigCPtr, cs_callback, cs_key);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void SWIG_FreeCompletionData(IntPtr data)
		{
			AppUtilPINVOKE.FutureVoid_SWIG_FreeCompletionData(this.swigCPtr, data);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
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
