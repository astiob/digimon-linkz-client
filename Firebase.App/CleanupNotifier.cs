using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class CleanupNotifier
	{
		private static Dictionary<long, CleanupNotifier.ObjectCleanupContext> cleanupIdToContext = new Dictionary<long, CleanupNotifier.ObjectCleanupContext>();

		[CompilerGenerated]
		private static CleanupNotifierBridge.CleanupDelegate <>f__mg$cache0;

		public static CleanupNotifier.RegistrationToken RegisterForCleanup(IntPtr cleanupObjectCPtr, IntPtr notifyObjectCPtr, object notifyObject, Type notifyObjectType, CleanupNotifier.DisposeObjectDelegate disposeObjectDelegate, CleanupNotifier.DeleteObjectDelegate deleteObjectDelegate)
		{
			object obj = CleanupNotifier.cleanupIdToContext;
			CleanupNotifier.RegistrationToken result;
			lock (obj)
			{
				result = CleanupNotifier.RegistrationToken.Create(CleanupNotifier.cleanupIdToContext);
				CleanupNotifier.cleanupIdToContext[result.Identifier] = new CleanupNotifier.ObjectCleanupContext
				{
					NotifyObjectType = notifyObjectType,
					NotifyObject = notifyObject,
					DisposeObject = disposeObjectDelegate,
					DeleteObject = deleteObjectDelegate,
					CleanupObjectCPtr = cleanupObjectCPtr,
					NotifyObjectCPtr = notifyObjectCPtr
				};
				if (CleanupNotifier.<>f__mg$cache0 == null)
				{
					CleanupNotifier.<>f__mg$cache0 = new CleanupNotifierBridge.CleanupDelegate(CleanupNotifier.PerformCleanup);
				}
				if (!CleanupNotifierBridge.RegisterCleanupDelegate(cleanupObjectCPtr, notifyObjectCPtr, CleanupNotifier.<>f__mg$cache0, (IntPtr)result.Identifier))
				{
					CleanupNotifier.cleanupIdToContext.Remove(result.Identifier);
					throw new NullReferenceException(string.Format("{0} (instance 0x{1:X}) can't be registered for cleanup with 0x{2:X}.", notifyObjectType, notifyObjectCPtr, cleanupObjectCPtr));
				}
				FirebaseApp.LogMessage(LogLevel.Debug, string.Format("{0} (instance 0x{1:X}) registered for cleanup when 0x{2:X} is destroyed", notifyObjectType, (long)notifyObjectCPtr, (long)cleanupObjectCPtr));
			}
			return result;
		}

		private static void DeleteContext(ref CleanupNotifier.RegistrationToken token)
		{
			object obj = CleanupNotifier.cleanupIdToContext;
			lock (obj)
			{
				CleanupNotifier.cleanupIdToContext.Remove(token.Identifier);
				token.Invalidate();
			}
		}

		public static void UnregisterForCleanup(ref CleanupNotifier.RegistrationToken token)
		{
			object obj = CleanupNotifier.cleanupIdToContext;
			lock (obj)
			{
				CleanupNotifier.ObjectCleanupContext objectCleanupContext;
				if (CleanupNotifier.cleanupIdToContext.TryGetValue(token.Identifier, out objectCleanupContext))
				{
					CleanupNotifierBridge.UnregisterCleanupDelegate(objectCleanupContext.CleanupObjectCPtr, objectCleanupContext.NotifyObjectCPtr);
					CleanupNotifier.DeleteContext(ref token);
					FirebaseApp.LogMessage(LogLevel.Debug, string.Format("{0} (instance 0x{1:X}) unregistered for cleanup when 0x{2:X} is destroyed", objectCleanupContext.NotifyObjectType, (long)objectCleanupContext.NotifyObjectCPtr, (long)objectCleanupContext.CleanupObjectCPtr));
				}
			}
		}

		public static void DisposeObject(ref CleanupNotifier.RegistrationToken token, object owner, bool delete)
		{
			object obj = CleanupNotifier.cleanupIdToContext;
			lock (obj)
			{
				CleanupNotifier.ObjectCleanupContext objectCleanupContext;
				if (CleanupNotifier.cleanupIdToContext.TryGetValue(token.Identifier, out objectCleanupContext))
				{
					CleanupNotifier.DeleteContext(ref token);
					if (!CleanupNotifierBridge.GetAndDestroyNotifiedFlag(objectCleanupContext.NotifyObjectCPtr) && delete)
					{
						FirebaseApp.LogMessage(LogLevel.Debug, string.Format("{0} (instance 0x{1:X}) being deleted", objectCleanupContext.NotifyObjectType, (long)objectCleanupContext.NotifyObjectCPtr));
						objectCleanupContext.DeleteObject(new HandleRef(owner, objectCleanupContext.NotifyObjectCPtr));
						FirebaseApp.LogMessage(LogLevel.Debug, string.Format("{0} (instance 0x{1:X}) deleted", objectCleanupContext.NotifyObjectType, (long)objectCleanupContext.NotifyObjectCPtr));
					}
				}
			}
		}

		[CleanupNotifier.MonoPInvokeCallbackAttribute(typeof(CleanupNotifierBridge.CleanupDelegate))]
		private static void PerformCleanup(IntPtr cleanupObjectCPtr, IntPtr notifyObjectCPtr, IntPtr context)
		{
			object obj = CleanupNotifier.cleanupIdToContext;
			lock (obj)
			{
				CleanupNotifier.ObjectCleanupContext objectCleanupContext;
				if (CleanupNotifier.cleanupIdToContext.TryGetValue((long)context, out objectCleanupContext))
				{
					object notifyObject = objectCleanupContext.NotifyObject;
					if (notifyObject != null)
					{
						FirebaseApp.LogMessage(LogLevel.Debug, string.Format("{0} (instance 0x{1:X}) will be disposed", objectCleanupContext.NotifyObjectType, (long)objectCleanupContext.NotifyObjectCPtr));
						CleanupNotifierBridge.GetAndDestroyNotifiedFlag(notifyObjectCPtr);
						objectCleanupContext.DisposeObject(notifyObject);
					}
					else
					{
						FirebaseApp.LogMessage(LogLevel.Debug, string.Format("{0} (instance 0x{1:X}) is being finalized, deleting object now", objectCleanupContext.NotifyObjectType, (long)objectCleanupContext.NotifyObjectCPtr));
						objectCleanupContext.DeleteObject(new HandleRef(objectCleanupContext.NotifyObjectType, notifyObjectCPtr));
					}
				}
			}
		}

		public delegate void DisposeObjectDelegate(object objectToDispose);

		public delegate void DeleteObjectDelegate(HandleRef objectCPtrToDelete);

		private class ObjectCleanupContext
		{
			private WeakReference notifyObjectReference;

			public Type NotifyObjectType { get; set; }

			public object NotifyObject
			{
				get
				{
					return FirebaseApp.WeakReferenceGetTarget(this.notifyObjectReference);
				}
				set
				{
					if (value != null)
					{
						this.notifyObjectReference = new WeakReference(value, false);
					}
					else
					{
						this.notifyObjectReference = null;
					}
				}
			}

			public CleanupNotifier.DisposeObjectDelegate DisposeObject { get; set; }

			public CleanupNotifier.DeleteObjectDelegate DeleteObject { get; set; }

			public IntPtr CleanupObjectCPtr { get; set; }

			public IntPtr NotifyObjectCPtr { get; set; }
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		internal struct RegistrationToken
		{
			private const long INVALID_ID = 0L;

			private static long nextId;

			public static CleanupNotifier.RegistrationToken Create(object lockObject)
			{
				CleanupNotifier.RegistrationToken result;
				lock (lockObject)
				{
					if (CleanupNotifier.RegistrationToken.nextId == 0L)
					{
						CleanupNotifier.RegistrationToken.nextId += 1L;
					}
					CleanupNotifier.RegistrationToken registrationToken = new CleanupNotifier.RegistrationToken
					{
						Identifier = CleanupNotifier.RegistrationToken.nextId
					};
					CleanupNotifier.RegistrationToken.nextId += 1L;
					result = registrationToken;
				}
				return result;
			}

			public long Identifier { get; private set; }

			public bool IsValid
			{
				get
				{
					return this.Identifier != 0L;
				}
			}

			public void Invalidate()
			{
				this.Identifier = 0L;
			}

			public override string ToString()
			{
				return string.Format("{0} ({1})", this.Identifier.ToString(), (!this.IsValid) ? "invalid" : "valid");
			}
		}

		[AttributeUsage(AttributeTargets.Method)]
		private sealed class MonoPInvokeCallbackAttribute : Attribute
		{
			public MonoPInvokeCallbackAttribute(Type t)
			{
			}
		}
	}
}
