using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Contexts
{
	/// <summary>Provides the default implementations of the <see cref="T:System.Runtime.Remoting.Contexts.IContextAttribute" /> and <see cref="T:System.Runtime.Remoting.Contexts.IContextProperty" /> interfaces.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class)]
	[Serializable]
	public class ContextAttribute : Attribute, IContextAttribute, IContextProperty
	{
		/// <summary>Indicates the name of the context attribute.</summary>
		protected string AttributeName;

		/// <summary>Creates an instance of the <see cref="T:System.Runtime.Remoting.Contexts.ContextAttribute" /> class with the specified name.</summary>
		/// <param name="name">The name of the context attribute. </param>
		public ContextAttribute(string name)
		{
			this.AttributeName = name;
		}

		/// <summary>Gets the name of the context attribute.</summary>
		/// <returns>The name of the context attribute.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string Name
		{
			get
			{
				return this.AttributeName;
			}
		}

		/// <summary>Returns a Boolean value indicating whether this instance is equal to the specified object.</summary>
		/// <returns>true if <paramref name="o" /> is not null and if the object names are equivalent; otherwise, false.</returns>
		/// <param name="o">The object to compare with this instance. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (!(o is ContextAttribute))
			{
				return false;
			}
			ContextAttribute contextAttribute = (ContextAttribute)o;
			return !(contextAttribute.AttributeName != this.AttributeName);
		}

		/// <summary>Called when the context is frozen.</summary>
		/// <param name="newContext">The context to freeze. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual void Freeze(Context newContext)
		{
		}

		/// <summary>Returns the hashcode for this instance of <see cref="T:System.Runtime.Remoting.Contexts.ContextAttribute" />.</summary>
		/// <returns>The hashcode for this instance of <see cref="T:System.Runtime.Remoting.Contexts.ContextAttribute" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			if (this.AttributeName == null)
			{
				return 0;
			}
			return this.AttributeName.GetHashCode();
		}

		/// <summary>Adds the current context property to the given message.</summary>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" /> to which to add the context property. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="ctorMsg" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			IList contextProperties = ctorMsg.ContextProperties;
			contextProperties.Add(this);
		}

		/// <summary>Returns a Boolean value indicating whether the context parameter meets the context attribute's requirements.</summary>
		/// <returns>true if the passed in context is okay; otherwise, false.</returns>
		/// <param name="ctx">The context in which to check. </param>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" /> to which to add the context property.</param>
		/// <exception cref="T:System.ArgumentNullException">Either <paramref name="ctx" /> or <paramref name="ctorMsg" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (!ctorMsg.ActivationType.IsContextful)
			{
				return true;
			}
			IContextProperty property = ctx.GetProperty(this.AttributeName);
			return property != null && this == property;
		}

		/// <summary>Returns a Boolean value indicating whether the context property is compatible with the new context.</summary>
		/// <returns>true if the context property is okay with the new context; otherwise, false.</returns>
		/// <param name="newCtx">The new context in which the property has been created. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual bool IsNewContextOK(Context newCtx)
		{
			return true;
		}
	}
}
