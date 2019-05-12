using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Implements the <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> interface to create a request message that acts as a method call on a remote object.</summary>
	[ComVisible(true)]
	public class MethodCallMessageWrapper : InternalMessageWrapper, IMessage, IMethodCallMessage, IMethodMessage
	{
		private object[] _args;

		private ArgInfo _inArgInfo;

		private MethodCallMessageWrapper.DictionaryWrapper _properties;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Messaging.MethodCallMessageWrapper" /> class by wrapping an <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> interface.</summary>
		/// <param name="msg">A message that acts as an outgoing method call on a remote object.</param>
		public MethodCallMessageWrapper(IMethodCallMessage msg) : base(msg)
		{
			this._args = ((IMethodCallMessage)this.WrappedMessage).Args;
			this._inArgInfo = new ArgInfo(msg.MethodBase, ArgInfoType.In);
		}

		/// <summary>Gets the number of arguments passed to the method. </summary>
		/// <returns>A <see cref="T:System.Int32" /> that represents the number of arguments passed to a method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual int ArgCount
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).ArgCount;
			}
		}

		/// <summary>Gets an array of arguments passed to the method. </summary>
		/// <returns>An array of type <see cref="T:System.Object" /> that represents the arguments passed to a method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual object[] Args
		{
			get
			{
				return this._args;
			}
			set
			{
				this._args = value;
			}
		}

		/// <summary>Gets a value indicating whether the method can accept a variable number of arguments. </summary>
		/// <returns>true if the method can accept a variable number of arguments; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual bool HasVarArgs
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).HasVarArgs;
			}
		}

		/// <summary>Gets the number of arguments in the method call that are not marked as out parameters. </summary>
		/// <returns>A <see cref="T:System.Int32" /> that represents the number of arguments in the method call that are not marked as out parameters.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual int InArgCount
		{
			get
			{
				return this._inArgInfo.GetInOutArgCount();
			}
		}

		/// <summary>Gets an array of arguments in the method call that are not marked as out parameters. </summary>
		/// <returns>An array of type <see cref="T:System.Object" /> that represents arguments in the method call that are not marked as out parameters.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual object[] InArgs
		{
			get
			{
				return this._inArgInfo.GetInOutArgs(this._args);
			}
		}

		/// <summary>Gets the <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> for the current method call. </summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> for the current method call.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual LogicalCallContext LogicalCallContext
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).LogicalCallContext;
			}
		}

		/// <summary>Gets the <see cref="T:System.Reflection.MethodBase" /> of the called method. </summary>
		/// <returns>The <see cref="T:System.Reflection.MethodBase" /> of the called method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual MethodBase MethodBase
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).MethodBase;
			}
		}

		/// <summary>Gets the name of the invoked method. </summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name of the invoked method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string MethodName
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).MethodName;
			}
		}

		/// <summary>Gets an object that contains the method signature. </summary>
		/// <returns>A <see cref="T:System.Object" /> that contains the method signature.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual object MethodSignature
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).MethodSignature;
			}
		}

		/// <summary>An <see cref="T:System.Collections.IDictionary" /> that represents a collection of the remoting message's properties. </summary>
		/// <returns>An <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					this._properties = new MethodCallMessageWrapper.DictionaryWrapper(this, this.WrappedMessage.Properties);
				}
				return this._properties;
			}
		}

		/// <summary>Gets the full type name of the remote object on which the method call is being made. </summary>
		/// <returns>A <see cref="T:System.String" /> that contains the full type name of the remote object on which the method call is being made.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string TypeName
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).TypeName;
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the remote object on which the method call is being made. </summary>
		/// <returns>The URI of a remote object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string Uri
		{
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).Uri;
			}
			set
			{
				IInternalMessage internalMessage = this.WrappedMessage as IInternalMessage;
				if (internalMessage != null)
				{
					internalMessage.Uri = value;
				}
				else
				{
					this.Properties["__Uri"] = value;
				}
			}
		}

		/// <summary>Gets a method argument, as an object, at a specified index. </summary>
		/// <returns>The method argument as an object.</returns>
		/// <param name="argNum">The index of the requested argument.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual object GetArg(int argNum)
		{
			return this._args[argNum];
		}

		/// <summary>Gets the name of a method argument at a specified index. </summary>
		/// <returns>The name of the method argument.</returns>
		/// <param name="index">The index of the requested argument.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string GetArgName(int index)
		{
			return ((IMethodCallMessage)this.WrappedMessage).GetArgName(index);
		}

		/// <summary>Gets a method argument at a specified index that is not marked as an out parameter. </summary>
		/// <returns>The method argument that is not marked as an out parameter.</returns>
		/// <param name="argNum">The index of the requested argument.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual object GetInArg(int argNum)
		{
			return this._args[this._inArgInfo.GetInOutArgIndex(argNum)];
		}

		/// <summary>Gets the name of a method argument at a specified index that is not marked as an out parameter. </summary>
		/// <returns>The name of the method argument that is not marked as an out parameter.</returns>
		/// <param name="index">The index of the requested argument.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string GetInArgName(int index)
		{
			return this._inArgInfo.GetInOutArgName(index);
		}

		private class DictionaryWrapper : MethodCallDictionary
		{
			private IDictionary _wrappedDictionary;

			private static string[] _keys = new string[]
			{
				"__Args"
			};

			public DictionaryWrapper(IMethodMessage message, IDictionary wrappedDictionary) : base(message)
			{
				this._wrappedDictionary = wrappedDictionary;
				base.MethodKeys = MethodCallMessageWrapper.DictionaryWrapper._keys;
			}

			protected override IDictionary AllocInternalProperties()
			{
				return this._wrappedDictionary;
			}

			protected override void SetMethodProperty(string key, object value)
			{
				if (key == "__Args")
				{
					((MethodCallMessageWrapper)this._message)._args = (object[])value;
				}
				else
				{
					base.SetMethodProperty(key, value);
				}
			}

			protected override object GetMethodProperty(string key)
			{
				if (key == "__Args")
				{
					return ((MethodCallMessageWrapper)this._message)._args;
				}
				return base.GetMethodProperty(key);
			}
		}
	}
}
