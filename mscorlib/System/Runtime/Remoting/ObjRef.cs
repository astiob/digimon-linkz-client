using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	/// <summary>Stores all relevant information required to generate a proxy in order to communicate with a remote object.</summary>
	[ComVisible(true)]
	[Serializable]
	public class ObjRef : ISerializable, IObjectReference
	{
		private IChannelInfo channel_info;

		private string uri;

		private IRemotingTypeInfo typeInfo;

		private IEnvoyInfo envoyInfo;

		private int flags;

		private Type _serverType;

		private static int MarshalledObjectRef = 1;

		private static int WellKnowObjectRef = 2;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class with default values.</summary>
		public ObjRef()
		{
			this.UpdateChannelInfo();
		}

		internal ObjRef(string typeName, string uri, IChannelInfo cinfo)
		{
			this.uri = uri;
			this.channel_info = cinfo;
			this.typeInfo = new TypeInfo(Type.GetType(typeName, true));
		}

		internal ObjRef(ObjRef o, bool unmarshalAsProxy)
		{
			this.channel_info = o.channel_info;
			this.uri = o.uri;
			this.typeInfo = o.typeInfo;
			this.envoyInfo = o.envoyInfo;
			this.flags = o.flags;
			if (unmarshalAsProxy)
			{
				this.flags |= ObjRef.MarshalledObjectRef;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class to reference a specified <see cref="T:System.MarshalByRefObject" /> of a specified <see cref="T:System.Type" />.</summary>
		/// <param name="o">The object that the new <see cref="T:System.Runtime.Remoting.ObjRef" /> instance will reference. </param>
		/// <param name="requestedType">The <see cref="T:System.Type" /> of the object that the new <see cref="T:System.Runtime.Remoting.ObjRef" /> instance will reference. </param>
		public ObjRef(MarshalByRefObject o, Type requestedType)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (requestedType == null)
			{
				throw new ArgumentNullException("requestedType");
			}
			this.uri = RemotingServices.GetObjectUri(o);
			this.typeInfo = new TypeInfo(requestedType);
			if (!requestedType.IsInstanceOfType(o))
			{
				throw new RemotingException("The server object type cannot be cast to the requested type " + requestedType.FullName);
			}
			this.UpdateChannelInfo();
		}

		internal ObjRef(Type type, string url, object remoteChannelData)
		{
			this.uri = url;
			this.typeInfo = new TypeInfo(type);
			if (remoteChannelData != null)
			{
				this.channel_info = new ChannelInfo(remoteChannelData);
			}
			this.flags |= ObjRef.WellKnowObjectRef;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class from serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination of the exception. </param>
		protected ObjRef(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext())
			{
				string name = enumerator.Name;
				switch (name)
				{
				case "uri":
					this.uri = (string)enumerator.Value;
					continue;
				case "typeInfo":
					this.typeInfo = (IRemotingTypeInfo)enumerator.Value;
					continue;
				case "channelInfo":
					this.channel_info = (IChannelInfo)enumerator.Value;
					continue;
				case "envoyInfo":
					this.envoyInfo = (IEnvoyInfo)enumerator.Value;
					continue;
				case "fIsMarshalled":
				{
					object value = enumerator.Value;
					int num2;
					if (value is string)
					{
						num2 = ((IConvertible)value).ToInt32(null);
					}
					else
					{
						num2 = (int)value;
					}
					if (num2 == 0)
					{
						flag = false;
					}
					continue;
				}
				case "objrefFlags":
					this.flags = Convert.ToInt32(enumerator.Value);
					continue;
				}
				throw new NotSupportedException();
			}
			if (flag)
			{
				this.flags |= ObjRef.MarshalledObjectRef;
			}
		}

		internal bool IsPossibleToCAD()
		{
			return false;
		}

		internal bool IsReferenceToWellKnow
		{
			get
			{
				return (this.flags & ObjRef.WellKnowObjectRef) > 0;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Remoting.IChannelInfo" /> for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.IChannelInfo" /> interface for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual IChannelInfo ChannelInfo
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.channel_info;
			}
			set
			{
				this.channel_info = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Remoting.IEnvoyInfo" /> for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.IEnvoyInfo" /> interface for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual IEnvoyInfo EnvoyInfo
		{
			get
			{
				return this.envoyInfo;
			}
			set
			{
				this.envoyInfo = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Remoting.IRemotingTypeInfo" /> for the object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.IRemotingTypeInfo" /> for the object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual IRemotingTypeInfo TypeInfo
		{
			get
			{
				return this.typeInfo;
			}
			set
			{
				this.typeInfo = value;
			}
		}

		/// <summary>Gets or sets the URI of the specific object instance.</summary>
		/// <returns>The URI of the specific object instance.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string URI
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		/// <summary>Populates a specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="context">The contextual information about the source or destination of the serialization. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have serialization formatter permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter, Infrastructure" />
		/// </PermissionSet>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(base.GetType());
			info.AddValue("uri", this.uri);
			info.AddValue("typeInfo", this.typeInfo, typeof(IRemotingTypeInfo));
			info.AddValue("envoyInfo", this.envoyInfo, typeof(IEnvoyInfo));
			info.AddValue("channelInfo", this.channel_info, typeof(IChannelInfo));
			info.AddValue("objrefFlags", this.flags);
		}

		/// <summary>Returns a reference to the remote object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</summary>
		/// <returns>A reference to the remote object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</returns>
		/// <param name="context">The context where the current object resides. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have serialization formatter permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter, RemotingConfiguration, Infrastructure" />
		/// </PermissionSet>
		public virtual object GetRealObject(StreamingContext context)
		{
			if ((this.flags & ObjRef.MarshalledObjectRef) > 0)
			{
				return RemotingServices.Unmarshal(this);
			}
			return this;
		}

		/// <summary>Returns a Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current <see cref="T:System.AppDomain" />.</summary>
		/// <returns>A Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current <see cref="T:System.AppDomain" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public bool IsFromThisAppDomain()
		{
			Identity identityForUri = RemotingServices.GetIdentityForUri(this.uri);
			return identityForUri != null && identityForUri.IsFromThisAppDomain;
		}

		/// <summary>Returns a Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current process.</summary>
		/// <returns>A Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current process.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool IsFromThisProcess()
		{
			foreach (object obj in this.channel_info.ChannelData)
			{
				if (obj is CrossAppDomainData)
				{
					string processID = ((CrossAppDomainData)obj).ProcessID;
					return processID == RemotingConfiguration.ProcessId;
				}
			}
			return true;
		}

		internal void UpdateChannelInfo()
		{
			this.channel_info = new ChannelInfo();
		}

		internal Type ServerType
		{
			get
			{
				if (this._serverType == null)
				{
					this._serverType = Type.GetType(this.typeInfo.TypeName);
				}
				return this._serverType;
			}
		}
	}
}
