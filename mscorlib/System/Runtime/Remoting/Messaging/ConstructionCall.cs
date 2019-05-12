using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Implements the <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" /> interface to create a request message that constitutes a constructor call on a remote object.</summary>
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public class ConstructionCall : MethodCall, IConstructionCallMessage, IMessage, IMethodCallMessage, IMethodMessage
	{
		private IActivator _activator;

		private object[] _activationAttributes;

		private IList _contextProperties;

		private Type _activationType;

		private string _activationTypeName;

		private bool _isContextOk;

		[NonSerialized]
		private RemotingProxy _sourceProxy;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Messaging.ConstructionCall" /> class by copying an existing message. </summary>
		/// <param name="m">A remoting message.</param>
		public ConstructionCall(IMessage m) : base(m)
		{
			this._activationTypeName = this.TypeName;
			this._isContextOk = true;
		}

		internal ConstructionCall(Type type)
		{
			this._activationType = type;
			this._activationTypeName = type.AssemblyQualifiedName;
			this._isContextOk = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Messaging.ConstructionCall" /> class from an array of remoting headers. </summary>
		/// <param name="headers">An array of remoting headers that contain key-value pairs. This array is used to initialize <see cref="T:System.Runtime.Remoting.Messaging.ConstructionCall" /> fields for those headers that belong to the namespace "http://schemas.microsoft.com/clr/soap/messageProperties".</param>
		public ConstructionCall(Header[] headers) : base(headers)
		{
		}

		internal ConstructionCall(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal override void InitDictionary()
		{
			ConstructionCallDictionary constructionCallDictionary = new ConstructionCallDictionary(this);
			this.ExternalProperties = constructionCallDictionary;
			this.InternalProperties = constructionCallDictionary.GetInternalProperties();
		}

		internal bool IsContextOk
		{
			get
			{
				return this._isContextOk;
			}
			set
			{
				this._isContextOk = value;
			}
		}

		/// <summary>Gets the type of the remote object to activate. </summary>
		/// <returns>The <see cref="T:System.Type" /> of the remote object to activate.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public Type ActivationType
		{
			get
			{
				if (this._activationType == null)
				{
					this._activationType = Type.GetType(this._activationTypeName);
				}
				return this._activationType;
			}
		}

		/// <summary>Gets the full type name of the remote object to activate. </summary>
		/// <returns>A <see cref="T:System.String" /> containing the full type name of the remote object to activate.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string ActivationTypeName
		{
			get
			{
				return this._activationTypeName;
			}
		}

		/// <summary>Gets or sets the activator that activates the remote object. </summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.Activation.IActivator" /> that activates the remote object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public IActivator Activator
		{
			get
			{
				return this._activator;
			}
			set
			{
				this._activator = value;
			}
		}

		/// <summary>Gets the call site activation attributes for the remote object. </summary>
		/// <returns>An array of type <see cref="T:System.Object" /> containing the call site activation attributes for the remote object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object[] CallSiteActivationAttributes
		{
			get
			{
				return this._activationAttributes;
			}
		}

		internal void SetActivationAttributes(object[] attributes)
		{
			this._activationAttributes = attributes;
		}

		/// <summary>Gets a list of properties that define the context in which the remote object is to be created. </summary>
		/// <returns>A <see cref="T:System.Collections.IList" /> that contains a list of properties that define the context in which the remote object is to be created.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public IList ContextProperties
		{
			get
			{
				if (this._contextProperties == null)
				{
					this._contextProperties = new ArrayList();
				}
				return this._contextProperties;
			}
		}

		internal override void InitMethodProperty(string key, object value)
		{
			switch (key)
			{
			case "__Activator":
				this._activator = (IActivator)value;
				return;
			case "__CallSiteActivationAttributes":
				this._activationAttributes = (object[])value;
				return;
			case "__ActivationType":
				this._activationType = (Type)value;
				return;
			case "__ContextProperties":
				this._contextProperties = (IList)value;
				return;
			case "__ActivationTypeName":
				this._activationTypeName = (string)value;
				return;
			}
			base.InitMethodProperty(key, value);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			IList list = this._contextProperties;
			if (list != null && list.Count == 0)
			{
				list = null;
			}
			info.AddValue("__Activator", this._activator);
			info.AddValue("__CallSiteActivationAttributes", this._activationAttributes);
			info.AddValue("__ActivationType", null);
			info.AddValue("__ContextProperties", list);
			info.AddValue("__ActivationTypeName", this._activationTypeName);
		}

		/// <summary>Gets an <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties. </summary>
		/// <returns>An <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public override IDictionary Properties
		{
			get
			{
				return base.Properties;
			}
		}

		internal RemotingProxy SourceProxy
		{
			get
			{
				return this._sourceProxy;
			}
			set
			{
				this._sourceProxy = value;
			}
		}
	}
}
