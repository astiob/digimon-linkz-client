using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Firebase.Messaging
{
	internal class FirebaseMessagingPINVOKE
	{
		protected static FirebaseMessagingPINVOKE.SWIGExceptionHelper swigExceptionHelper = new FirebaseMessagingPINVOKE.SWIGExceptionHelper();

		protected static FirebaseMessagingPINVOKE.SWIGStringHelper swigStringHelper = new FirebaseMessagingPINVOKE.SWIGStringHelper();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_new_MessagingOptions")]
		public static extern IntPtr new_MessagingOptions();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_MessagingOptions_SuppressNotificationPermissionPrompt_set")]
		public static extern void MessagingOptions_SuppressNotificationPermissionPrompt_set(HandleRef jarg1, bool jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_MessagingOptions_SuppressNotificationPermissionPrompt_get")]
		public static extern bool MessagingOptions_SuppressNotificationPermissionPrompt_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_delete_MessagingOptions")]
		public static extern void delete_MessagingOptions(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_Title_get")]
		public static extern string FirebaseNotification_Title_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_Body_get")]
		public static extern string FirebaseNotification_Body_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_Icon_get")]
		public static extern string FirebaseNotification_Icon_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_Sound_get")]
		public static extern string FirebaseNotification_Sound_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_Badge_get")]
		public static extern string FirebaseNotification_Badge_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_Tag_get")]
		public static extern string FirebaseNotification_Tag_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_Color_get")]
		public static extern string FirebaseNotification_Color_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_ClickAction_get")]
		public static extern string FirebaseNotification_ClickAction_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_BodyLocalizationKey_get")]
		public static extern string FirebaseNotification_BodyLocalizationKey_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_BodyLocalizationArgs_get")]
		public static extern IntPtr FirebaseNotification_BodyLocalizationArgs_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_TitleLocalizationKey_get")]
		public static extern string FirebaseNotification_TitleLocalizationKey_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseNotification_TitleLocalizationArgs_get")]
		public static extern IntPtr FirebaseNotification_TitleLocalizationArgs_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_new_FirebaseNotification")]
		public static extern IntPtr new_FirebaseNotification();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_delete_FirebaseNotification")]
		public static extern void delete_FirebaseNotification(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_new_FirebaseMessage")]
		public static extern IntPtr new_FirebaseMessage();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_delete_FirebaseMessage")]
		public static extern void delete_FirebaseMessage(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_From_get")]
		public static extern string FirebaseMessage_From_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_To_set")]
		public static extern void FirebaseMessage_To_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_To_get")]
		public static extern string FirebaseMessage_To_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_CollapseKey_get")]
		public static extern string FirebaseMessage_CollapseKey_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_Data_set")]
		public static extern void FirebaseMessage_Data_set(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_Data_get")]
		public static extern IntPtr FirebaseMessage_Data_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_RawData_get")]
		public static extern string FirebaseMessage_RawData_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_MessageId_set")]
		public static extern void FirebaseMessage_MessageId_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_MessageId_get")]
		public static extern string FirebaseMessage_MessageId_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_MessageType_get")]
		public static extern string FirebaseMessage_MessageType_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_Priority_get")]
		public static extern string FirebaseMessage_Priority_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_TimeToLiveInternal_get")]
		public static extern int FirebaseMessage_TimeToLiveInternal_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_Error_get")]
		public static extern string FirebaseMessage_Error_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_ErrorDescription_get")]
		public static extern string FirebaseMessage_ErrorDescription_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_NotificationOpened_get")]
		public static extern bool FirebaseMessage_NotificationOpened_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_FirebaseMessage_LinkInternal_get")]
		public static extern string FirebaseMessage_LinkInternal_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_IsTokenRegistrationOnInitEnabledInternal")]
		internal static extern bool IsTokenRegistrationOnInitEnabledInternal();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_SetTokenRegistrationOnInitEnabledInternal")]
		internal static extern void SetTokenRegistrationOnInitEnabledInternal(bool jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_RequestPermission")]
		public static extern IntPtr RequestPermission();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_Send")]
		public static extern void Send(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_Subscribe")]
		public static extern IntPtr Subscribe(string jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_Unsubscribe")]
		public static extern IntPtr Unsubscribe(string jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_SetListenerCallbacks")]
		public static extern void SetListenerCallbacks(FirebaseMessaging.Listener.MessageReceivedDelegate jarg1, FirebaseMessaging.Listener.TokenReceivedDelegate jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_SetListenerCallbacksEnabled")]
		public static extern void SetListenerCallbacksEnabled(bool jarg1, bool jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_SendPendingEvents")]
		public static extern void SendPendingEvents();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_Messaging_CSharp_MessageCopyNotification")]
		public static extern IntPtr MessageCopyNotification(IntPtr jarg1);

		protected class SWIGExceptionHelper
		{
			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingApplicationException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingArithmeticException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingDivideByZeroException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingIndexOutOfRangeException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingInvalidCastException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingInvalidOperationException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingIOException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingNullReferenceException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingOutOfMemoryException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingOverflowException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingSystemException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingArgumentException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingArgumentNullException);

			private static FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate = new FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(FirebaseMessagingPINVOKE.SWIGExceptionHelper.SetPendingArgumentOutOfRangeException);

			static SWIGExceptionHelper()
			{
				FirebaseMessagingPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacks_FirebaseMessaging(FirebaseMessagingPINVOKE.SWIGExceptionHelper.applicationDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.arithmeticDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.divideByZeroDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.indexOutOfRangeDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.invalidCastDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.invalidOperationDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ioDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.nullReferenceDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.outOfMemoryDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.overflowDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.systemDelegate);
				FirebaseMessagingPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacksArgument_FirebaseMessaging(FirebaseMessagingPINVOKE.SWIGExceptionHelper.argumentDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.argumentNullDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.argumentOutOfRangeDelegate);
			}

			[DllImport("FirebaseCppApp-5.2.1")]
			public static extern void SWIGRegisterExceptionCallbacks_FirebaseMessaging(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemExceptionDelegate);

			[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "SWIGRegisterExceptionArgumentCallbacks_FirebaseMessaging")]
			public static extern void SWIGRegisterExceptionCallbacksArgument_FirebaseMessaging(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate, FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate);

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingApplicationException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new ApplicationException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingArithmeticException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new ArithmeticException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingDivideByZeroException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new DivideByZeroException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingIndexOutOfRangeException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new IndexOutOfRangeException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingInvalidCastException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new InvalidCastException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingInvalidOperationException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new InvalidOperationException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingIOException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new IOException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingNullReferenceException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new NullReferenceException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingOutOfMemoryException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new OutOfMemoryException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingOverflowException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new OverflowException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingSystemException(string message)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new SystemException(message, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate))]
			private static void SetPendingArgumentException(string message, string paramName)
			{
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new ArgumentException(message, paramName, FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate))]
			private static void SetPendingArgumentNullException(string message, string paramName)
			{
				Exception ex = FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve();
				if (ex != null)
				{
					message = message + " Inner Exception: " + ex.Message;
				}
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new ArgumentNullException(paramName, message));
			}

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate))]
			private static void SetPendingArgumentOutOfRangeException(string message, string paramName)
			{
				Exception ex = FirebaseMessagingPINVOKE.SWIGPendingException.Retrieve();
				if (ex != null)
				{
					message = message + " Inner Exception: " + ex.Message;
				}
				FirebaseMessagingPINVOKE.SWIGPendingException.Set(new ArgumentOutOfRangeException(paramName, message));
			}

			public delegate void ExceptionDelegate(string message);

			public delegate void ExceptionArgumentDelegate(string message, string paramName);
		}

		public class SWIGPendingException
		{
			[ThreadStatic]
			private static Exception pendingException;

			private static int numExceptionsPending;

			public static bool Pending
			{
				get
				{
					bool result = false;
					if (FirebaseMessagingPINVOKE.SWIGPendingException.numExceptionsPending > 0 && FirebaseMessagingPINVOKE.SWIGPendingException.pendingException != null)
					{
						result = true;
					}
					return result;
				}
			}

			public static void Set(Exception e)
			{
				if (FirebaseMessagingPINVOKE.SWIGPendingException.pendingException != null)
				{
					throw new ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + FirebaseMessagingPINVOKE.SWIGPendingException.pendingException.ToString() + ")", e);
				}
				FirebaseMessagingPINVOKE.SWIGPendingException.pendingException = e;
				object typeFromHandle = typeof(FirebaseMessagingPINVOKE);
				lock (typeFromHandle)
				{
					FirebaseMessagingPINVOKE.SWIGPendingException.numExceptionsPending++;
				}
			}

			public static Exception Retrieve()
			{
				Exception result = null;
				if (FirebaseMessagingPINVOKE.SWIGPendingException.numExceptionsPending > 0 && FirebaseMessagingPINVOKE.SWIGPendingException.pendingException != null)
				{
					result = FirebaseMessagingPINVOKE.SWIGPendingException.pendingException;
					FirebaseMessagingPINVOKE.SWIGPendingException.pendingException = null;
					object typeFromHandle = typeof(FirebaseMessagingPINVOKE);
					lock (typeFromHandle)
					{
						FirebaseMessagingPINVOKE.SWIGPendingException.numExceptionsPending--;
					}
				}
				return result;
			}
		}

		protected class SWIGStringHelper
		{
			private static FirebaseMessagingPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate = new FirebaseMessagingPINVOKE.SWIGStringHelper.SWIGStringDelegate(FirebaseMessagingPINVOKE.SWIGStringHelper.CreateString);

			static SWIGStringHelper()
			{
				FirebaseMessagingPINVOKE.SWIGStringHelper.SWIGRegisterStringCallback_FirebaseMessaging(FirebaseMessagingPINVOKE.SWIGStringHelper.stringDelegate);
			}

			[DllImport("FirebaseCppApp-5.2.1")]
			public static extern void SWIGRegisterStringCallback_FirebaseMessaging(FirebaseMessagingPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate);

			[MonoPInvokeCallback(typeof(FirebaseMessagingPINVOKE.SWIGStringHelper.SWIGStringDelegate))]
			private static string CreateString(string cString)
			{
				return cString;
			}

			public delegate string SWIGStringDelegate(string message);
		}
	}
}
