using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	/// <summary>Defines an environment for the objects that are resident inside it and for which a policy can be enforced.</summary>
	[ComVisible(true)]
	public class Context
	{
		private int domain_id;

		private int context_id;

		private UIntPtr static_data;

		private static IMessageSink default_server_context_sink;

		private IMessageSink server_context_sink_chain;

		private IMessageSink client_context_sink_chain;

		private object[] datastore;

		private ArrayList context_properties;

		private bool frozen;

		private static int global_count;

		private static Hashtable namedSlots = new Hashtable();

		private static DynamicPropertyCollection global_dynamic_properties;

		private DynamicPropertyCollection context_dynamic_properties;

		private ContextCallbackObject callback_object;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Contexts.Context" /> class.</summary>
		public Context()
		{
			this.domain_id = Thread.GetDomainID();
			this.context_id = 1 + Context.global_count++;
		}

		/// <summary>Cleans up the backing objects for the nondefault contexts.</summary>
		~Context()
		{
		}

		/// <summary>Gets the default context for the current application domain.</summary>
		/// <returns>The default context for the <see cref="T:System.AppDomain" /> namespace.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static Context DefaultContext
		{
			get
			{
				return AppDomain.InternalGetDefaultContext();
			}
		}

		/// <summary>Gets the context ID for the current context.</summary>
		/// <returns>The context ID for the current context.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual int ContextID
		{
			get
			{
				return this.context_id;
			}
		}

		/// <summary>Gets the array of the current context properties.</summary>
		/// <returns>The current context properties array; otherwise, null if the context does not have any properties attributed to it.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual IContextProperty[] ContextProperties
		{
			get
			{
				if (this.context_properties == null)
				{
					return new IContextProperty[0];
				}
				return (IContextProperty[])this.context_properties.ToArray(typeof(IContextProperty[]));
			}
		}

		internal bool IsDefaultContext
		{
			get
			{
				return this.context_id == 0;
			}
		}

		internal bool NeedsContextSink
		{
			get
			{
				return this.context_id != 0 || (Context.global_dynamic_properties != null && Context.global_dynamic_properties.HasProperties) || (this.context_dynamic_properties != null && this.context_dynamic_properties.HasProperties);
			}
		}

		/// <summary>Registers a dynamic property implementing the <see cref="T:System.Runtime.Remoting.Contexts.IDynamicProperty" /> interface with the remoting service.</summary>
		/// <returns>true if the property was successfully registered; otherwise, false.</returns>
		/// <param name="prop">The dynamic property to register. </param>
		/// <param name="obj">The object/proxy for which the <paramref name="property" /> is registered. </param>
		/// <param name="ctx">The context for which the <paramref name="property" /> is registered. </param>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="prop" /> or its name is null, or it is not dynamic (it does not implement <see cref="T:System.Runtime.Remoting.Contexts.IDynamicProperty" />). </exception>
		/// <exception cref="T:System.ArgumentException">Both an object as well as a context are specified (both <paramref name="obj" /> and <paramref name="ctx" /> are not null). </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static bool RegisterDynamicProperty(IDynamicProperty prop, ContextBoundObject obj, Context ctx)
		{
			DynamicPropertyCollection dynamicPropertyCollection = Context.GetDynamicPropertyCollection(obj, ctx);
			return dynamicPropertyCollection.RegisterDynamicProperty(prop);
		}

		/// <summary>Unregisters a dynamic property implementing the <see cref="T:System.Runtime.Remoting.Contexts.IDynamicProperty" /> interface.</summary>
		/// <returns>true if the object was successfully unregistered; otherwise, false.</returns>
		/// <param name="name">The name of the dynamic property to unregister. </param>
		/// <param name="obj">The object/proxy for which the <paramref name="property" /> is registered. </param>
		/// <param name="ctx">The context for which the <paramref name="property" /> is registered. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">Both an object as well as a context are specified (both <paramref name="obj" /> and <paramref name="ctx" /> are not null). </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static bool UnregisterDynamicProperty(string name, ContextBoundObject obj, Context ctx)
		{
			DynamicPropertyCollection dynamicPropertyCollection = Context.GetDynamicPropertyCollection(obj, ctx);
			return dynamicPropertyCollection.UnregisterDynamicProperty(name);
		}

		private static DynamicPropertyCollection GetDynamicPropertyCollection(ContextBoundObject obj, Context ctx)
		{
			if (ctx == null && obj != null)
			{
				if (RemotingServices.IsTransparentProxy(obj))
				{
					RealProxy realProxy = RemotingServices.GetRealProxy(obj);
					return realProxy.ObjectIdentity.ClientDynamicProperties;
				}
				return obj.ObjectIdentity.ServerDynamicProperties;
			}
			else
			{
				if (ctx != null && obj == null)
				{
					if (ctx.context_dynamic_properties == null)
					{
						ctx.context_dynamic_properties = new DynamicPropertyCollection();
					}
					return ctx.context_dynamic_properties;
				}
				if (ctx == null && obj == null)
				{
					if (Context.global_dynamic_properties == null)
					{
						Context.global_dynamic_properties = new DynamicPropertyCollection();
					}
					return Context.global_dynamic_properties;
				}
				throw new ArgumentException("Either obj or ctx must be null");
			}
		}

		internal static void NotifyGlobalDynamicSinks(bool start, IMessage req_msg, bool client_site, bool async)
		{
			if (Context.global_dynamic_properties != null && Context.global_dynamic_properties.HasProperties)
			{
				Context.global_dynamic_properties.NotifyMessage(start, req_msg, client_site, async);
			}
		}

		internal static bool HasGlobalDynamicSinks
		{
			get
			{
				return Context.global_dynamic_properties != null && Context.global_dynamic_properties.HasProperties;
			}
		}

		internal void NotifyDynamicSinks(bool start, IMessage req_msg, bool client_site, bool async)
		{
			if (this.context_dynamic_properties != null && this.context_dynamic_properties.HasProperties)
			{
				this.context_dynamic_properties.NotifyMessage(start, req_msg, client_site, async);
			}
		}

		internal bool HasDynamicSinks
		{
			get
			{
				return this.context_dynamic_properties != null && this.context_dynamic_properties.HasProperties;
			}
		}

		internal bool HasExitSinks
		{
			get
			{
				return !(this.GetClientContextSinkChain() is ClientContextTerminatorSink) || this.HasDynamicSinks || Context.HasGlobalDynamicSinks;
			}
		}

		/// <summary>Returns a specific context property, specified by name.</summary>
		/// <returns>The specified context property.</returns>
		/// <param name="name">The name of the property. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual IContextProperty GetProperty(string name)
		{
			if (this.context_properties == null)
			{
				return null;
			}
			foreach (object obj in this.context_properties)
			{
				IContextProperty contextProperty = (IContextProperty)obj;
				if (contextProperty.Name == name)
				{
					return contextProperty;
				}
			}
			return null;
		}

		/// <summary>Sets a specific context property by name.</summary>
		/// <param name="prop">The actual context property. </param>
		/// <exception cref="T:System.InvalidOperationException">There is an attempt to add properties to the default context. </exception>
		/// <exception cref="T:System.InvalidOperationException">The context is frozen. </exception>
		/// <exception cref="T:System.ArgumentNullException">The property or the property name is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual void SetProperty(IContextProperty prop)
		{
			if (prop == null)
			{
				throw new ArgumentNullException("IContextProperty");
			}
			if (this == Context.DefaultContext)
			{
				throw new InvalidOperationException("Can not add properties to default context");
			}
			if (this.frozen)
			{
				throw new InvalidOperationException("Context is Frozen");
			}
			if (this.context_properties == null)
			{
				this.context_properties = new ArrayList();
			}
			this.context_properties.Add(prop);
		}

		/// <summary>Freezes the context, making it impossible to add or remove context properties from the current context.</summary>
		/// <exception cref="T:System.InvalidOperationException">The context is already frozen. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual void Freeze()
		{
			if (this.context_properties != null)
			{
				foreach (object obj in this.context_properties)
				{
					IContextProperty contextProperty = (IContextProperty)obj;
					contextProperty.Freeze(this);
				}
			}
		}

		/// <summary>Returns a <see cref="T:System.String" /> class representation of the current context.</summary>
		/// <returns>A <see cref="T:System.String" /> class representation of the current context.</returns>
		public override string ToString()
		{
			return "ContextID: " + this.context_id;
		}

		internal IMessageSink GetServerContextSinkChain()
		{
			if (this.server_context_sink_chain == null)
			{
				if (Context.default_server_context_sink == null)
				{
					Context.default_server_context_sink = new ServerContextTerminatorSink();
				}
				this.server_context_sink_chain = Context.default_server_context_sink;
				if (this.context_properties != null)
				{
					for (int i = this.context_properties.Count - 1; i >= 0; i--)
					{
						IContributeServerContextSink contributeServerContextSink = this.context_properties[i] as IContributeServerContextSink;
						if (contributeServerContextSink != null)
						{
							this.server_context_sink_chain = contributeServerContextSink.GetServerContextSink(this.server_context_sink_chain);
						}
					}
				}
			}
			return this.server_context_sink_chain;
		}

		internal IMessageSink GetClientContextSinkChain()
		{
			if (this.client_context_sink_chain == null)
			{
				this.client_context_sink_chain = new ClientContextTerminatorSink(this);
				if (this.context_properties != null)
				{
					foreach (object obj in this.context_properties)
					{
						IContextProperty contextProperty = (IContextProperty)obj;
						IContributeClientContextSink contributeClientContextSink = contextProperty as IContributeClientContextSink;
						if (contributeClientContextSink != null)
						{
							this.client_context_sink_chain = contributeClientContextSink.GetClientContextSink(this.client_context_sink_chain);
						}
					}
				}
			}
			return this.client_context_sink_chain;
		}

		internal IMessageSink CreateServerObjectSinkChain(MarshalByRefObject obj, bool forceInternalExecute)
		{
			IMessageSink messageSink = new StackBuilderSink(obj, forceInternalExecute);
			messageSink = new ServerObjectTerminatorSink(messageSink);
			messageSink = new LeaseSink(messageSink);
			if (this.context_properties != null)
			{
				for (int i = this.context_properties.Count - 1; i >= 0; i--)
				{
					IContextProperty contextProperty = (IContextProperty)this.context_properties[i];
					IContributeObjectSink contributeObjectSink = contextProperty as IContributeObjectSink;
					if (contributeObjectSink != null)
					{
						messageSink = contributeObjectSink.GetObjectSink(obj, messageSink);
					}
				}
			}
			return messageSink;
		}

		internal IMessageSink CreateEnvoySink(MarshalByRefObject serverObject)
		{
			IMessageSink messageSink = EnvoyTerminatorSink.Instance;
			if (this.context_properties != null)
			{
				foreach (object obj in this.context_properties)
				{
					IContextProperty contextProperty = (IContextProperty)obj;
					IContributeEnvoySink contributeEnvoySink = contextProperty as IContributeEnvoySink;
					if (contributeEnvoySink != null)
					{
						messageSink = contributeEnvoySink.GetEnvoySink(serverObject, messageSink);
					}
				}
			}
			return messageSink;
		}

		internal static Context SwitchToContext(Context newContext)
		{
			return AppDomain.InternalSetContext(newContext);
		}

		internal static Context CreateNewContext(IConstructionCallMessage msg)
		{
			Context context = new Context();
			foreach (object obj in msg.ContextProperties)
			{
				IContextProperty contextProperty = (IContextProperty)obj;
				if (context.GetProperty(contextProperty.Name) == null)
				{
					context.SetProperty(contextProperty);
				}
			}
			context.Freeze();
			foreach (object obj2 in msg.ContextProperties)
			{
				IContextProperty contextProperty2 = (IContextProperty)obj2;
				if (!contextProperty2.IsNewContextOK(context))
				{
					throw new RemotingException("A context property did not approve the candidate context for activating the object");
				}
			}
			return context;
		}

		/// <summary>Executes code in another context.</summary>
		/// <param name="deleg">The delegate used to request the callback. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public void DoCallBack(CrossContextDelegate deleg)
		{
			lock (this)
			{
				if (this.callback_object == null)
				{
					Context newContext = Context.SwitchToContext(this);
					this.callback_object = new ContextCallbackObject();
					Context.SwitchToContext(newContext);
				}
			}
			this.callback_object.DoCallBack(deleg);
		}

		/// <summary>Allocates an unnamed data slot.</summary>
		/// <returns>A local data slot.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static LocalDataStoreSlot AllocateDataSlot()
		{
			return new LocalDataStoreSlot(false);
		}

		/// <summary>Allocates a named data slot.</summary>
		/// <returns>A local data slot object.</returns>
		/// <param name="name">The required name for the data slot. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			object syncRoot = Context.namedSlots.SyncRoot;
			LocalDataStoreSlot result;
			lock (syncRoot)
			{
				LocalDataStoreSlot localDataStoreSlot = Context.AllocateDataSlot();
				Context.namedSlots.Add(name, localDataStoreSlot);
				result = localDataStoreSlot;
			}
			return result;
		}

		/// <summary>Frees a named data slot on all the contexts.</summary>
		/// <param name="name">The name of the data slot to free. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static void FreeNamedDataSlot(string name)
		{
			object syncRoot = Context.namedSlots.SyncRoot;
			lock (syncRoot)
			{
				Context.namedSlots.Remove(name);
			}
		}

		/// <summary>Retrieves the value from the specified slot on the current context.</summary>
		/// <returns>Returns the data associated with <paramref name="slot" />. </returns>
		/// <param name="slot">The data slot that contains the data. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static object GetData(LocalDataStoreSlot slot)
		{
			Context currentContext = Thread.CurrentContext;
			Context obj = currentContext;
			object result;
			lock (obj)
			{
				if (currentContext.datastore != null && slot.slot < currentContext.datastore.Length)
				{
					result = currentContext.datastore[slot.slot];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		/// <summary>Looks up a named data slot.</summary>
		/// <returns>Returns a local data slot.</returns>
		/// <param name="name">The data slot name. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			object syncRoot = Context.namedSlots.SyncRoot;
			LocalDataStoreSlot result;
			lock (syncRoot)
			{
				LocalDataStoreSlot localDataStoreSlot = Context.namedSlots[name] as LocalDataStoreSlot;
				if (localDataStoreSlot == null)
				{
					result = Context.AllocateNamedDataSlot(name);
				}
				else
				{
					result = localDataStoreSlot;
				}
			}
			return result;
		}

		/// <summary>Sets the data in the specified slot on the current context.</summary>
		/// <param name="slot">The data slot where the data is to be added. </param>
		/// <param name="data">The data that is to be added. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			Context currentContext = Thread.CurrentContext;
			Context obj = currentContext;
			lock (obj)
			{
				if (currentContext.datastore == null)
				{
					currentContext.datastore = new object[slot.slot + 2];
				}
				else if (slot.slot >= currentContext.datastore.Length)
				{
					object[] array = new object[slot.slot + 2];
					currentContext.datastore.CopyTo(array, 0);
					currentContext.datastore = array;
				}
				currentContext.datastore[slot.slot] = data;
			}
		}
	}
}
