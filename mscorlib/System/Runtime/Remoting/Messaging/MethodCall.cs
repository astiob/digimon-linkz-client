using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Implements the <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> interface to create a request message that acts as a method call on a remote object.</summary>
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public class MethodCall : ISerializable, IInternalMessage, IMessage, IMethodCallMessage, IMethodMessage, ISerializationRootObject
	{
		private string _uri;

		private string _typeName;

		private string _methodName;

		private object[] _args;

		private Type[] _methodSignature;

		private MethodBase _methodBase;

		private LogicalCallContext _callContext;

		private ArgInfo _inArgInfo;

		private Identity _targetIdentity;

		private Type[] _genericArguments;

		/// <summary>An <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties. </summary>
		protected IDictionary ExternalProperties;

		/// <summary>An <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties. </summary>
		protected IDictionary InternalProperties;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Messaging.MethodCall" /> class from an array of remoting headers.</summary>
		/// <param name="h1">An array of remoting headers that contains key/value pairs. This array is used to initialize <see cref="T:System.Runtime.Remoting.Messaging.MethodCall" /> fields for headers that belong to the namespace "http://schemas.microsoft.com/clr/soap/messageProperties".</param>
		public MethodCall(Header[] h1)
		{
			this.Init();
			if (h1 == null || h1.Length == 0)
			{
				return;
			}
			foreach (Header header in h1)
			{
				this.InitMethodProperty(header.Name, header.Value);
			}
			this.ResolveMethod();
		}

		internal MethodCall(SerializationInfo info, StreamingContext context)
		{
			this.Init();
			foreach (SerializationEntry serializationEntry in info)
			{
				this.InitMethodProperty(serializationEntry.Name, serializationEntry.Value);
			}
		}

		internal MethodCall(CADMethodCallMessage msg)
		{
			this._uri = string.Copy(msg.Uri);
			ArrayList arguments = msg.GetArguments();
			this._args = msg.GetArgs(arguments);
			this._callContext = msg.GetLogicalCallContext(arguments);
			if (this._callContext == null)
			{
				this._callContext = new LogicalCallContext();
			}
			this._methodBase = msg.GetMethod();
			this.Init();
			if (msg.PropertiesCount > 0)
			{
				CADMessageBase.UnmarshalProperties(this.Properties, msg.PropertiesCount, arguments);
			}
		}

		/// <summary>Initializes  a new instance of the <see cref="T:System.Runtime.Remoting.Messaging.MethodCall" /> class by copying an existing message.</summary>
		/// <param name="msg">A remoting message.</param>
		public MethodCall(IMessage msg)
		{
			if (msg is IMethodMessage)
			{
				this.CopyFrom((IMethodMessage)msg);
			}
			else
			{
				foreach (object obj in msg.Properties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					this.InitMethodProperty((string)dictionaryEntry.Key, dictionaryEntry.Value);
				}
				this.Init();
			}
		}

		internal MethodCall(string uri, string typeName, string methodName, object[] args)
		{
			this._uri = uri;
			this._typeName = typeName;
			this._methodName = methodName;
			this._args = args;
			this.Init();
			this.ResolveMethod();
		}

		internal MethodCall()
		{
		}

		string IInternalMessage.Uri
		{
			get
			{
				return this.Uri;
			}
			set
			{
				this.Uri = value;
			}
		}

		Identity IInternalMessage.TargetIdentity
		{
			get
			{
				return this._targetIdentity;
			}
			set
			{
				this._targetIdentity = value;
			}
		}

		internal void CopyFrom(IMethodMessage call)
		{
			this._uri = call.Uri;
			this._typeName = call.TypeName;
			this._methodName = call.MethodName;
			this._args = call.Args;
			this._methodSignature = (Type[])call.MethodSignature;
			this._methodBase = call.MethodBase;
			this._callContext = call.LogicalCallContext;
			this.Init();
		}

		internal virtual void InitMethodProperty(string key, object value)
		{
			switch (key)
			{
			case "__TypeName":
				this._typeName = (string)value;
				return;
			case "__MethodName":
				this._methodName = (string)value;
				return;
			case "__MethodSignature":
				this._methodSignature = (Type[])value;
				return;
			case "__Args":
				this._args = (object[])value;
				return;
			case "__CallContext":
				this._callContext = (LogicalCallContext)value;
				return;
			case "__Uri":
				this._uri = (string)value;
				return;
			case "__GenericArguments":
				this._genericArguments = (Type[])value;
				return;
			}
			this.Properties[key] = value;
		}

		/// <summary>The <see cref="M:System.Runtime.Remoting.Messaging.MethodCall.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)" /> method is not implemented. </summary>
		/// <param name="info">The data for serializing or deserializing the remote object.</param>
		/// <param name="context">The context of a certain serialized stream.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter, Infrastructure" />
		/// </PermissionSet>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("__TypeName", this._typeName);
			info.AddValue("__MethodName", this._methodName);
			info.AddValue("__MethodSignature", this._methodSignature);
			info.AddValue("__Args", this._args);
			info.AddValue("__CallContext", this._callContext);
			info.AddValue("__Uri", this._uri);
			info.AddValue("__GenericArguments", this._genericArguments);
			if (this.InternalProperties != null)
			{
				foreach (object obj in this.InternalProperties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					info.AddValue((string)dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
		}

		/// <summary>Gets the number of arguments passed to a method. </summary>
		/// <returns>A <see cref="T:System.Int32" /> that represents the number of arguments passed to a method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public int ArgCount
		{
			get
			{
				return this._args.Length;
			}
		}

		/// <summary>Gets an array of arguments passed to a method. </summary>
		/// <returns>An array of type <see cref="T:System.Object" /> that represents the arguments passed to a method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object[] Args
		{
			get
			{
				return this._args;
			}
		}

		/// <summary>Gets a value that indicates whether the method can accept a variable number of arguments. </summary>
		/// <returns>true if the method can accept a variable number of arguments; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public bool HasVarArgs
		{
			get
			{
				return (this.MethodBase.CallingConvention | CallingConventions.VarArgs) != (CallingConventions)0;
			}
		}

		/// <summary>Gets the number of arguments in the method call that are not marked as out parameters.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that represents the number of arguments in the method call that are not marked as out parameters.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public int InArgCount
		{
			get
			{
				if (this._inArgInfo == null)
				{
					this._inArgInfo = new ArgInfo(this._methodBase, ArgInfoType.In);
				}
				return this._inArgInfo.GetInOutArgCount();
			}
		}

		/// <summary>Gets an array of arguments in the method call that are not marked as out parameters. </summary>
		/// <returns>An array of type <see cref="T:System.Object" /> that represents arguments in the method call that are not marked as out parameters.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object[] InArgs
		{
			get
			{
				if (this._inArgInfo == null)
				{
					this._inArgInfo = new ArgInfo(this._methodBase, ArgInfoType.In);
				}
				return this._inArgInfo.GetInOutArgs(this._args);
			}
		}

		/// <summary>Gets the <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> for the current method call. </summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> for the current method call.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				if (this._callContext == null)
				{
					this._callContext = new LogicalCallContext();
				}
				return this._callContext;
			}
		}

		/// <summary>Gets the <see cref="T:System.Reflection.MethodBase" /> of the called method. </summary>
		/// <returns>The <see cref="T:System.Reflection.MethodBase" /> of the called method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public MethodBase MethodBase
		{
			get
			{
				if (this._methodBase == null)
				{
					this.ResolveMethod();
				}
				return this._methodBase;
			}
		}

		/// <summary>Gets the name of the invoked method. </summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name of the invoked method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string MethodName
		{
			get
			{
				if (this._methodName == null)
				{
					this._methodName = this._methodBase.Name;
				}
				return this._methodName;
			}
		}

		/// <summary>Gets an object that contains the method signature. </summary>
		/// <returns>A <see cref="T:System.Object" /> that contains the method signature.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object MethodSignature
		{
			get
			{
				if (this._methodSignature == null && this._methodBase != null)
				{
					ParameterInfo[] parameters = this._methodBase.GetParameters();
					this._methodSignature = new Type[parameters.Length];
					for (int i = 0; i < parameters.Length; i++)
					{
						this._methodSignature[i] = parameters[i].ParameterType;
					}
				}
				return this._methodSignature;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties. </summary>
		/// <returns>An <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual IDictionary Properties
		{
			get
			{
				if (this.ExternalProperties == null)
				{
					this.InitDictionary();
				}
				return this.ExternalProperties;
			}
		}

		internal virtual void InitDictionary()
		{
			MethodCallDictionary methodCallDictionary = new MethodCallDictionary(this);
			this.ExternalProperties = methodCallDictionary;
			this.InternalProperties = methodCallDictionary.GetInternalProperties();
		}

		/// <summary>Gets the full type name of the remote object on which the method call is being made. </summary>
		/// <returns>A <see cref="T:System.String" /> that contains the full type name of the remote object on which the method call is being made.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string TypeName
		{
			get
			{
				if (this._typeName == null)
				{
					this._typeName = this._methodBase.DeclaringType.AssemblyQualifiedName;
				}
				return this._typeName;
			}
		}

		/// <summary>Gets or sets the Uniform Resource Identifier (URI) of the remote object on which the method call is being made.</summary>
		/// <returns>The URI of a remote object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string Uri
		{
			get
			{
				return this._uri;
			}
			set
			{
				this._uri = value;
			}
		}

		/// <summary>Gets a method argument, as an object, at a specified index. </summary>
		/// <returns>The method argument as an object.</returns>
		/// <param name="argNum">The index of the requested argument.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object GetArg(int argNum)
		{
			return this._args[argNum];
		}

		/// <summary>Gets the name of a method argument at a specified index. </summary>
		/// <returns>The name of the method argument.</returns>
		/// <param name="index">The index of the requested argument.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string GetArgName(int index)
		{
			return this._methodBase.GetParameters()[index].Name;
		}

		/// <summary>Gets a method argument at a specified index that is not marked as an out parameter. </summary>
		/// <returns>The method argument that is not marked as an out parameter.</returns>
		/// <param name="argNum">The index of the requested argument.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object GetInArg(int argNum)
		{
			if (this._inArgInfo == null)
			{
				this._inArgInfo = new ArgInfo(this._methodBase, ArgInfoType.In);
			}
			return this._args[this._inArgInfo.GetInOutArgIndex(argNum)];
		}

		/// <summary>Gets the name of a method argument at a specified index that is not marked as an out parameter.</summary>
		/// <returns>The name of the method argument that is not marked as an out parameter.</returns>
		/// <param name="index">The index of the requested argument. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string GetInArgName(int index)
		{
			if (this._inArgInfo == null)
			{
				this._inArgInfo = new ArgInfo(this._methodBase, ArgInfoType.In);
			}
			return this._inArgInfo.GetInOutArgName(index);
		}

		/// <summary>Initializes an internal serialization handler from an array of remoting headers that are applied to a method. </summary>
		/// <returns>An internal serialization handler.</returns>
		/// <param name="h">An array of remoting headers that contain key/value pairs. This array is used to initialize <see cref="T:System.Runtime.Remoting.Messaging.MethodCall" /> fields for headers that belong to the namespace "http://schemas.microsoft.com/clr/soap/messageProperties".</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public virtual object HeaderHandler(Header[] h)
		{
			throw new NotImplementedException();
		}

		/// <summary>Initializes a <see cref="T:System.Runtime.Remoting.Messaging.MethodCall" />. </summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual void Init()
		{
		}

		/// <summary>Sets method information from previously initialized remoting message properties. </summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void ResolveMethod()
		{
			if (this._uri != null)
			{
				Type serverTypeForUri = RemotingServices.GetServerTypeForUri(this._uri);
				if (serverTypeForUri == null)
				{
					string str = (this._typeName == null) ? string.Empty : (" (" + this._typeName + ")");
					throw new RemotingException("Requested service not found" + str + ". No receiver for uri " + this._uri);
				}
				Type type = this.CastTo(this._typeName, serverTypeForUri);
				if (type == null)
				{
					throw new RemotingException(string.Concat(new string[]
					{
						"Cannot cast from client type '",
						this._typeName,
						"' to server type '",
						serverTypeForUri.FullName,
						"'"
					}));
				}
				this._methodBase = RemotingServices.GetMethodBaseFromName(type, this._methodName, this._methodSignature);
				if (this._methodBase == null)
				{
					throw new RemotingException(string.Concat(new object[]
					{
						"Method ",
						this._methodName,
						" not found in ",
						type
					}));
				}
				if (type != serverTypeForUri && type.IsInterface && !serverTypeForUri.IsInterface)
				{
					this._methodBase = RemotingServices.GetVirtualMethod(serverTypeForUri, this._methodBase);
					if (this._methodBase == null)
					{
						throw new RemotingException(string.Concat(new object[]
						{
							"Method ",
							this._methodName,
							" not found in ",
							serverTypeForUri
						}));
					}
				}
			}
			else
			{
				this._methodBase = RemotingServices.GetMethodBaseFromMethodMessage(this);
				if (this._methodBase == null)
				{
					throw new RemotingException("Method " + this._methodName + " not found in " + this.TypeName);
				}
			}
			if (this._methodBase.IsGenericMethod && this._methodBase.ContainsGenericParameters)
			{
				if (this.GenericArguments == null)
				{
					throw new RemotingException("The remoting infrastructure does not support open generic methods.");
				}
				this._methodBase = ((MethodInfo)this._methodBase).MakeGenericMethod(this.GenericArguments);
			}
		}

		private Type CastTo(string clientType, Type serverType)
		{
			clientType = MethodCall.GetTypeNameFromAssemblyQualifiedName(clientType);
			if (clientType == serverType.FullName)
			{
				return serverType;
			}
			for (Type baseType = serverType.BaseType; baseType != null; baseType = baseType.BaseType)
			{
				if (clientType == baseType.FullName)
				{
					return baseType;
				}
			}
			Type[] interfaces = serverType.GetInterfaces();
			foreach (Type type in interfaces)
			{
				if (clientType == type.FullName)
				{
					return type;
				}
			}
			return null;
		}

		private static string GetTypeNameFromAssemblyQualifiedName(string aqname)
		{
			int num = aqname.IndexOf("]]");
			int num2 = aqname.IndexOf(',', (num != -1) ? (num + 2) : 0);
			if (num2 != -1)
			{
				aqname = aqname.Substring(0, num2).Trim();
			}
			return aqname;
		}

		/// <summary>Sets method information from serialization settings. </summary>
		/// <param name="info">The data for serializing or deserializing the remote object.</param>
		/// <param name="ctx">The context of a given serialized stream.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public void RootSetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			throw new NotImplementedException();
		}

		private Type[] GenericArguments
		{
			get
			{
				if (this._genericArguments != null)
				{
					return this._genericArguments;
				}
				return this._genericArguments = this.MethodBase.GetGenericArguments();
			}
		}
	}
}
