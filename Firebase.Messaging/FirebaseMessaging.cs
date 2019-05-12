using System;
using System.Threading.Tasks;

namespace Firebase.Messaging
{
	public sealed class FirebaseMessaging
	{
		private static FirebaseMessaging.Listener listener = FirebaseMessaging.Listener.Create();

		private FirebaseMessaging()
		{
		}

		internal static event EventHandler<MessageReceivedEventArgs> MessageReceivedInternal;

		internal static event EventHandler<TokenReceivedEventArgs> TokenReceivedInternal;

		internal static void CreateOrDestroyListener()
		{
			object typeFromHandle = typeof(FirebaseMessaging.Listener);
			lock (typeFromHandle)
			{
				bool flag = FirebaseMessaging.MessageReceivedInternal != null;
				bool flag2 = FirebaseMessaging.TokenReceivedInternal != null;
				if (flag || flag2)
				{
					FirebaseMessaging.Listener.Create();
				}
				else
				{
					FirebaseMessaging.Listener.Destroy();
				}
				FirebaseMessaging.SetListenerCallbacksEnabled(flag, flag2);
				if (flag || flag2)
				{
					FirebaseMessaging.SendPendingEvents();
				}
			}
		}

		private static FirebaseApp App
		{
			get
			{
				return FirebaseMessaging.Listener.App;
			}
		}

		public static bool TokenRegistrationOnInitEnabled
		{
			get
			{
				return FirebaseMessaging.IsTokenRegistrationOnInitEnabledInternal();
			}
			set
			{
				FirebaseMessaging.SetTokenRegistrationOnInitEnabledInternal(value);
			}
		}

		public static event EventHandler<MessageReceivedEventArgs> MessageReceived
		{
			add
			{
				object typeFromHandle = typeof(FirebaseMessaging.Listener);
				lock (typeFromHandle)
				{
					FirebaseMessaging.MessageReceivedInternal += value;
					FirebaseMessaging.CreateOrDestroyListener();
				}
			}
			remove
			{
				object typeFromHandle = typeof(FirebaseMessaging.Listener);
				lock (typeFromHandle)
				{
					FirebaseMessaging.MessageReceivedInternal -= value;
					FirebaseMessaging.CreateOrDestroyListener();
				}
			}
		}

		public static event EventHandler<TokenReceivedEventArgs> TokenReceived
		{
			add
			{
				object typeFromHandle = typeof(FirebaseMessaging.Listener);
				lock (typeFromHandle)
				{
					FirebaseMessaging.TokenReceivedInternal += value;
					FirebaseMessaging.CreateOrDestroyListener();
				}
			}
			remove
			{
				object typeFromHandle = typeof(FirebaseMessaging.Listener);
				lock (typeFromHandle)
				{
					FirebaseMessaging.TokenReceivedInternal -= value;
					FirebaseMessaging.CreateOrDestroyListener();
				}
			}
		}

		[Obsolete("FirebaseMessaging.Subscribe is deprecated. Please use FirebaseMessaging.SubscribeAsync() instead")]
		public static void Subscribe(string topic)
		{
			FirebaseMessaging.SubscribeAsync(topic);
		}

		[Obsolete("FirebaseMessaging.Unsubscribe is deprecated. Please use FirebaseMessaging.UnsubscribeAsync() instead")]
		public static void Unsubscribe(string topic)
		{
			FirebaseMessaging.SubscribeAsync(topic);
		}

		internal static bool IsTokenRegistrationOnInitEnabledInternal()
		{
			bool result = FirebaseMessagingPINVOKE.IsTokenRegistrationOnInitEnabledInternal();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static void SetTokenRegistrationOnInitEnabledInternal(bool enable)
		{
			FirebaseMessagingPINVOKE.SetTokenRegistrationOnInitEnabledInternal(enable);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static Task RequestPermissionAsync()
		{
			return FutureVoid.GetTask(new FutureVoid(FirebaseMessagingPINVOKE.RequestPermission(), true));
		}

		public static void Send(FirebaseMessage message)
		{
			FirebaseMessagingPINVOKE.Send(FirebaseMessage.getCPtr(message));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static Task SubscribeAsync(string topic)
		{
			return FutureVoid.GetTask(new FutureVoid(FirebaseMessagingPINVOKE.Subscribe(topic), true));
		}

		public static Task UnsubscribeAsync(string topic)
		{
			return FutureVoid.GetTask(new FutureVoid(FirebaseMessagingPINVOKE.Unsubscribe(topic), true));
		}

		private static void SetListenerCallbacks(FirebaseMessaging.Listener.MessageReceivedDelegate messageCallback, FirebaseMessaging.Listener.TokenReceivedDelegate tokenCallback)
		{
			FirebaseMessagingPINVOKE.SetListenerCallbacks(messageCallback, tokenCallback);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private static void SetListenerCallbacksEnabled(bool message_callback_enabled, bool token_callback_enabled)
		{
			FirebaseMessagingPINVOKE.SetListenerCallbacksEnabled(message_callback_enabled, token_callback_enabled);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private static void SendPendingEvents()
		{
			FirebaseMessagingPINVOKE.SendPendingEvents();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static IntPtr MessageCopyNotification(IntPtr message)
		{
			IntPtr result = FirebaseMessagingPINVOKE.MessageCopyNotification(message);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal class Listener : IDisposable
		{
			private FirebaseMessaging.Listener.MessageReceivedDelegate messageReceivedDelegate = new FirebaseMessaging.Listener.MessageReceivedDelegate(FirebaseMessaging.Listener.MessageReceivedDelegateMethod);

			private FirebaseMessaging.Listener.TokenReceivedDelegate tokenReceivedDelegate = new FirebaseMessaging.Listener.TokenReceivedDelegate(FirebaseMessaging.Listener.TokenReceivedDelegateMethod);

			private FirebaseApp app = FirebaseApp.DefaultInstance;

			private static FirebaseMessaging.Listener listener;

			private Listener()
			{
				FirebaseMessaging.SetListenerCallbacks(this.messageReceivedDelegate, this.tokenReceivedDelegate);
			}

			internal static FirebaseMessaging.Listener Create()
			{
				object typeFromHandle = typeof(FirebaseMessaging.Listener);
				FirebaseMessaging.Listener result;
				lock (typeFromHandle)
				{
					if (FirebaseMessaging.Listener.listener != null)
					{
						result = FirebaseMessaging.Listener.listener;
					}
					else
					{
						FirebaseMessaging.Listener.listener = new FirebaseMessaging.Listener();
						result = FirebaseMessaging.Listener.listener;
					}
				}
				return result;
			}

			internal static void Destroy()
			{
				object typeFromHandle = typeof(FirebaseMessaging.Listener);
				lock (typeFromHandle)
				{
					if (FirebaseMessaging.Listener.listener != null)
					{
						FirebaseMessaging.Listener.listener.Dispose();
					}
				}
			}

			internal static FirebaseApp App
			{
				get
				{
					object typeFromHandle = typeof(FirebaseMessaging.Listener);
					FirebaseApp result;
					lock (typeFromHandle)
					{
						result = ((FirebaseMessaging.Listener.listener == null) ? null : FirebaseMessaging.Listener.listener.app);
					}
					return result;
				}
			}

			~Listener()
			{
				this.Dispose();
			}

			public void Dispose()
			{
				object typeFromHandle = typeof(FirebaseMessaging.Listener);
				lock (typeFromHandle)
				{
					if (FirebaseMessaging.Listener.listener == this)
					{
						FirebaseMessaging.SetListenerCallbacks(null, null);
						FirebaseMessaging.Listener.listener = null;
						this.app = null;
					}
				}
			}

			[FirebaseMessaging.Listener.MonoPInvokeCallbackAttribute(typeof(FirebaseMessaging.Listener.MessageReceivedDelegate))]
			private static int MessageReceivedDelegateMethod(IntPtr message)
			{
				EventHandler<MessageReceivedEventArgs> messageReceivedInternal = FirebaseMessaging.MessageReceivedInternal;
				if (messageReceivedInternal != null)
				{
					messageReceivedInternal(null, new MessageReceivedEventArgs(new FirebaseMessage(message, true)));
					return 1;
				}
				return 0;
			}

			[FirebaseMessaging.Listener.MonoPInvokeCallbackAttribute(typeof(FirebaseMessaging.Listener.TokenReceivedDelegate))]
			private static void TokenReceivedDelegateMethod(string token)
			{
				EventHandler<TokenReceivedEventArgs> tokenReceivedInternal = FirebaseMessaging.TokenReceivedInternal;
				if (tokenReceivedInternal != null)
				{
					tokenReceivedInternal(null, new TokenReceivedEventArgs(token));
				}
			}

			internal delegate int MessageReceivedDelegate(IntPtr message);

			internal delegate void TokenReceivedDelegate(string token);

			[AttributeUsage(AttributeTargets.Method)]
			private sealed class MonoPInvokeCallbackAttribute : Attribute
			{
				public MonoPInvokeCallbackAttribute(Type t)
				{
				}
			}
		}
	}
}
