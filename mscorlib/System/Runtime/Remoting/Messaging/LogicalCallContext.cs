using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Provides a set of properties that are carried with the execution code path during remote method calls.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class LogicalCallContext : ICloneable, ISerializable
	{
		private Hashtable _data;

		private CallContextRemotingData _remotingData = new CallContextRemotingData();

		internal LogicalCallContext()
		{
		}

		internal LogicalCallContext(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "__RemotingData")
				{
					this._remotingData = (CallContextRemotingData)serializationEntry.Value;
				}
				else
				{
					this.SetData(serializationEntry.Name, serializationEntry.Value);
				}
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> contains information.</summary>
		/// <returns>A Boolean value indicating whether the current <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> contains information.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public bool HasInfo
		{
			get
			{
				return this._data != null && this._data.Count > 0;
			}
		}

		/// <summary>Empties a data slot with the specified name.</summary>
		/// <param name="name">The name of the data slot to empty. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void FreeNamedDataSlot(string name)
		{
			if (this._data != null)
			{
				this._data.Remove(name);
			}
		}

		/// <summary>Retrieves an object associated with the specified name from the current instance.</summary>
		/// <returns>The object in the logical call context associated with the specified name.</returns>
		/// <param name="name">The name of the item in the call context. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object GetData(string name)
		{
			if (this._data != null)
			{
				return this._data[name];
			}
			return null;
		}

		/// <summary>Populates a specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the current <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" />.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="context">The contextual information about the source or destination of the serialization. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have SerializationFormatter permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter, Infrastructure" />
		/// </PermissionSet>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("__RemotingData", this._remotingData);
			if (this._data != null)
			{
				foreach (object obj in this._data)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					info.AddValue((string)dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
		}

		/// <summary>Stores the specified object in the current instance, and associates it with the specified name.</summary>
		/// <param name="name">The name with which to associate the new item in the call context. </param>
		/// <param name="data">The object to store in the call context. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void SetData(string name, object data)
		{
			if (this._data == null)
			{
				this._data = new Hashtable();
			}
			this._data[name] = data;
		}

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object Clone()
		{
			LogicalCallContext logicalCallContext = new LogicalCallContext();
			logicalCallContext._remotingData = (CallContextRemotingData)this._remotingData.Clone();
			if (this._data != null)
			{
				logicalCallContext._data = new Hashtable();
				foreach (object obj in this._data)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					logicalCallContext._data[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
			return logicalCallContext;
		}

		internal Hashtable Datastore
		{
			get
			{
				return this._data;
			}
		}
	}
}
