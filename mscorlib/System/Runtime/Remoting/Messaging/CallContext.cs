using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Provides a set of properties that are carried with the execution code path. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class CallContext
	{
		[ThreadStatic]
		private static Header[] Headers;

		[ThreadStatic]
		private static Hashtable datastore;

		private CallContext()
		{
		}

		/// <summary>Gets or sets the host context associated with the current thread.</summary>
		/// <returns>The host context associated with the current thread.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		public static object HostContext
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Empties a data slot with the specified name.</summary>
		/// <param name="name">The name of the data slot to empty. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static void FreeNamedDataSlot(string name)
		{
			CallContext.Datastore.Remove(name);
		}

		/// <summary>Retrieves an object with the specified name from the <see cref="T:System.Runtime.Remoting.Messaging.CallContext" />.</summary>
		/// <returns>The object in the call context associated with the specified name.</returns>
		/// <param name="name">The name of the item in the call context. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static object GetData(string name)
		{
			return CallContext.Datastore[name];
		}

		/// <summary>Stores a given object and associates it with the specified name.</summary>
		/// <param name="name">The name with which to associate the new item in the call context. </param>
		/// <param name="data">The object to store in the call context. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static void SetData(string name, object data)
		{
			CallContext.Datastore[name] = data;
		}

		/// <summary>Retrieves an object with the specified name from the logical call context.</summary>
		/// <returns>The object in the logical call context associated with the specified name.</returns>
		/// <param name="name">The name of the item in the logical call context. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		[MonoTODO]
		public static object LogicalGetData(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>Stores a given object in the logical call context and associates it with the specified name.</summary>
		/// <param name="name">The name with which to associate the new item in the logical call context. </param>
		/// <param name="data">The object to store in the logical call context. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		[MonoTODO]
		public static void LogicalSetData(string name, object data)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns the headers that are sent along with the method call.</summary>
		/// <returns>The headers that are sent along with the method call.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static Header[] GetHeaders()
		{
			return CallContext.Headers;
		}

		/// <summary>Sets the headers that are sent along with the method call.</summary>
		/// <param name="headers">A <see cref="T:System.Runtime.Remoting.Messaging.Header" /> array of the headers that are to be sent along with the method call. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static void SetHeaders(Header[] headers)
		{
			CallContext.Headers = headers;
		}

		internal static LogicalCallContext CreateLogicalCallContext(bool createEmpty)
		{
			LogicalCallContext logicalCallContext = null;
			if (CallContext.datastore != null)
			{
				foreach (object obj in CallContext.datastore)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (dictionaryEntry.Value is ILogicalThreadAffinative)
					{
						if (logicalCallContext == null)
						{
							logicalCallContext = new LogicalCallContext();
						}
						logicalCallContext.SetData((string)dictionaryEntry.Key, dictionaryEntry.Value);
					}
				}
			}
			if (logicalCallContext == null && createEmpty)
			{
				return new LogicalCallContext();
			}
			return logicalCallContext;
		}

		internal static object SetCurrentCallContext(LogicalCallContext ctx)
		{
			object result = CallContext.datastore;
			if (ctx != null && ctx.HasInfo)
			{
				CallContext.datastore = (Hashtable)ctx.Datastore.Clone();
			}
			else
			{
				CallContext.datastore = null;
			}
			return result;
		}

		internal static void UpdateCurrentCallContext(LogicalCallContext ctx)
		{
			Hashtable hashtable = ctx.Datastore;
			foreach (object obj in hashtable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				CallContext.SetData((string)dictionaryEntry.Key, dictionaryEntry.Value);
			}
		}

		internal static void RestoreCallContext(object oldContext)
		{
			CallContext.datastore = (Hashtable)oldContext;
		}

		private static Hashtable Datastore
		{
			get
			{
				Hashtable hashtable = CallContext.datastore;
				if (hashtable == null)
				{
					return CallContext.datastore = new Hashtable();
				}
				return hashtable;
			}
		}
	}
}
