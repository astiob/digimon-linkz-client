using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Represents a class member, such as a property or event. This is an abstract base class.</summary>
	[ComVisible(true)]
	public abstract class MemberDescriptor
	{
		private string name;

		private Attribute[] attrs;

		private AttributeCollection attrCollection;

		private static IComparer default_comparer;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.MemberDescriptor" /> class with the specified name of the member and an array of attributes.</summary>
		/// <param name="name">The name of the member. </param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that contains the member attributes. </param>
		/// <exception cref="T:System.ArgumentException">The name is an empty string ("") or null. </exception>
		protected MemberDescriptor(string name, Attribute[] attrs)
		{
			this.name = name;
			this.attrs = attrs;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.MemberDescriptor" /> class with the name in the specified <see cref="T:System.ComponentModel.MemberDescriptor" /> and the attributes in both the old <see cref="T:System.ComponentModel.MemberDescriptor" /> and the <see cref="T:System.Attribute" /> array.</summary>
		/// <param name="oldMemberDescriptor">A <see cref="T:System.ComponentModel.MemberDescriptor" /> that has the name of the member and its attributes. </param>
		/// <param name="newAttributes">An array of <see cref="T:System.Attribute" /> objects with the attributes you want to add to the member. </param>
		protected MemberDescriptor(MemberDescriptor reference, Attribute[] attrs)
		{
			this.name = reference.name;
			this.attrs = attrs;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.MemberDescriptor" /> class with the specified name of the member.</summary>
		/// <param name="name">The name of the member. </param>
		/// <exception cref="T:System.ArgumentException">The name is an empty string ("") or null.</exception>
		protected MemberDescriptor(string name)
		{
			this.name = name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.MemberDescriptor" /> class with the specified <see cref="T:System.ComponentModel.MemberDescriptor" />.</summary>
		/// <param name="descr">A <see cref="T:System.ComponentModel.MemberDescriptor" /> that contains the name of the member and its attributes. </param>
		protected MemberDescriptor(MemberDescriptor reference)
		{
			this.name = reference.name;
			this.attrs = reference.AttributeArray;
		}

		/// <summary>Gets or sets an array of attributes.</summary>
		/// <returns>An array of type <see cref="T:System.Attribute" /> that contains the attributes of this member. </returns>
		protected virtual Attribute[] AttributeArray
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				if (this.attrs != null)
				{
					arrayList.AddRange(this.attrs);
				}
				this.FillAttributes(arrayList);
				Hashtable hashtable = new Hashtable();
				foreach (object obj in arrayList)
				{
					Attribute attribute = (Attribute)obj;
					hashtable[attribute.TypeId] = attribute;
				}
				Attribute[] array = new Attribute[hashtable.Values.Count];
				hashtable.Values.CopyTo(array, 0);
				return array;
			}
			set
			{
				this.attrs = value;
			}
		}

		/// <summary>When overridden in a derived class, adds the attributes of the inheriting class to the specified list of attributes in the parent class.</summary>
		/// <param name="attributeList">An <see cref="T:System.Collections.IList" /> that lists the attributes in the parent class. Initially, this is empty. </param>
		protected virtual void FillAttributes(IList attributeList)
		{
		}

		/// <summary>Gets the collection of attributes for this member.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.AttributeCollection" /> that provides the attributes for this member, or an empty collection if there are no attributes in the <see cref="P:System.ComponentModel.MemberDescriptor.AttributeArray" />.</returns>
		public virtual AttributeCollection Attributes
		{
			get
			{
				if (this.attrCollection == null)
				{
					this.attrCollection = this.CreateAttributeCollection();
				}
				return this.attrCollection;
			}
		}

		/// <summary>Creates a collection of attributes using the array of attributes passed to the constructor.</summary>
		/// <returns>A new <see cref="T:System.ComponentModel.AttributeCollection" /> that contains the <see cref="P:System.ComponentModel.MemberDescriptor.AttributeArray" /> attributes.</returns>
		protected virtual AttributeCollection CreateAttributeCollection()
		{
			return new AttributeCollection(this.AttributeArray);
		}

		/// <summary>Gets the name of the category to which the member belongs, as specified in the <see cref="T:System.ComponentModel.CategoryAttribute" />.</summary>
		/// <returns>The name of the category to which the member belongs. If there is no <see cref="T:System.ComponentModel.CategoryAttribute" />, the category name is set to the default category, Misc.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual string Category
		{
			get
			{
				return ((CategoryAttribute)this.Attributes[typeof(CategoryAttribute)]).Category;
			}
		}

		/// <summary>Gets the description of the member, as specified in the <see cref="T:System.ComponentModel.DescriptionAttribute" />.</summary>
		/// <returns>The description of the member. If there is no <see cref="T:System.ComponentModel.DescriptionAttribute" />, the property value is set to the default, which is an empty string ("").</returns>
		public virtual string Description
		{
			get
			{
				foreach (Attribute attribute in this.AttributeArray)
				{
					if (attribute is DescriptionAttribute)
					{
						return ((DescriptionAttribute)attribute).Description;
					}
				}
				return string.Empty;
			}
		}

		/// <summary>Gets whether this member should be set only at design time, as specified in the <see cref="T:System.ComponentModel.DesignOnlyAttribute" />.</summary>
		/// <returns>true if this member should be set only at design time; false if the member can be set during run time.</returns>
		public virtual bool DesignTimeOnly
		{
			get
			{
				foreach (Attribute attribute in this.AttributeArray)
				{
					if (attribute is DesignOnlyAttribute)
					{
						return ((DesignOnlyAttribute)attribute).IsDesignOnly;
					}
				}
				return false;
			}
		}

		/// <summary>Gets the name that can be displayed in a window, such as a Properties window.</summary>
		/// <returns>The name to display for the member.</returns>
		public virtual string DisplayName
		{
			get
			{
				foreach (Attribute attribute in this.AttributeArray)
				{
					if (attribute is DisplayNameAttribute)
					{
						return ((DisplayNameAttribute)attribute).DisplayName;
					}
				}
				return this.name;
			}
		}

		/// <summary>Gets the name of the member.</summary>
		/// <returns>The name of the member.</returns>
		public virtual string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets a value indicating whether the member is browsable, as specified in the <see cref="T:System.ComponentModel.BrowsableAttribute" />.</summary>
		/// <returns>true if the member is browsable; otherwise, false. If there is no <see cref="T:System.ComponentModel.BrowsableAttribute" />, the property value is set to the default, which is true.</returns>
		public virtual bool IsBrowsable
		{
			get
			{
				foreach (Attribute attribute in this.AttributeArray)
				{
					if (attribute is BrowsableAttribute)
					{
						return ((BrowsableAttribute)attribute).Browsable;
					}
				}
				return true;
			}
		}

		/// <summary>Gets the hash code for the name of the member, as specified in <see cref="M:System.String.GetHashCode" />.</summary>
		/// <returns>The hash code for the name of the member.</returns>
		protected virtual int NameHashCode
		{
			get
			{
				return this.name.GetHashCode();
			}
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A hash code for the current <see cref="T:System.ComponentModel.MemberDescriptor" />.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Compares this instance to the given object to see if they are equivalent.</summary>
		/// <returns>true if equivalent; otherwise, false.</returns>
		/// <param name="obj">The object to compare to the current instance. </param>
		public override bool Equals(object obj)
		{
			MemberDescriptor memberDescriptor = obj as MemberDescriptor;
			return memberDescriptor != null && memberDescriptor.name == this.name;
		}

		/// <summary>Gets a component site for the given component.</summary>
		/// <returns>The site of the component, or null if a site does not exist.</returns>
		/// <param name="component">The component for which you want to find a site. </param>
		protected static ISite GetSite(object component)
		{
			if (component is Component)
			{
				return ((Component)component).Site;
			}
			return null;
		}

		/// <summary>Gets the component on which to invoke a method.</summary>
		/// <returns>An instance of the component to invoke. This method returns a visual designer when the property is attached to a visual designer.</returns>
		/// <param name="componentClass">A <see cref="T:System.Type" /> representing the type of component this <see cref="T:System.ComponentModel.MemberDescriptor" /> is bound to. For example, if this <see cref="T:System.ComponentModel.MemberDescriptor" /> describes a property, this parameter should be the class that the property is declared on. </param>
		/// <param name="component">An instance of the object to call. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="componentClass" /> or <paramref name="component" /> is null.</exception>
		[Obsolete("Use GetInvocationTarget")]
		protected static object GetInvokee(Type componentClass, object component)
		{
			if (component is IComponent)
			{
				ISite site = ((IComponent)component).Site;
				if (site != null && site.DesignMode)
				{
					System.ComponentModel.Design.IDesignerHost designerHost = site.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) as System.ComponentModel.Design.IDesignerHost;
					if (designerHost != null)
					{
						System.ComponentModel.Design.IDesigner designer = designerHost.GetDesigner((IComponent)component);
						if (designer != null && componentClass.IsInstanceOfType(designer))
						{
							component = designer;
						}
					}
				}
			}
			return component;
		}

		/// <summary>Retrieves the object that should be used during invocation of members.</summary>
		/// <returns>The object to be used during member invocations.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of the invocation target.</param>
		/// <param name="instance">The potential invocation target.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="instance" /> is null.</exception>
		protected virtual object GetInvocationTarget(Type type, object instance)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return MemberDescriptor.GetInvokee(type, instance);
		}

		/// <summary>Finds the given method through reflection, searching only for public methods.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> that represents the method, or null if the method is not found.</returns>
		/// <param name="componentClass">The component that contains the method. </param>
		/// <param name="name">The name of the method to find. </param>
		/// <param name="args">An array of parameters for the method, used to choose between overloaded methods. </param>
		/// <param name="returnType">The type to return for the method. </param>
		protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType)
		{
			return MemberDescriptor.FindMethod(componentClass, name, args, returnType, true);
		}

		/// <summary>Finds the given method through reflection, with an option to search only public methods.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> that represents the method, or null if the method is not found.</returns>
		/// <param name="componentClass">The component that contains the method. </param>
		/// <param name="name">The name of the method to find. </param>
		/// <param name="args">An array of parameters for the method, used to choose between overloaded methods. </param>
		/// <param name="returnType">The type to return for the method. </param>
		/// <param name="publicOnly">Whether to restrict search to public methods. </param>
		protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType, bool publicOnly)
		{
			BindingFlags bindingAttr;
			if (publicOnly)
			{
				bindingAttr = BindingFlags.Public;
			}
			else
			{
				bindingAttr = (BindingFlags.Public | BindingFlags.NonPublic);
			}
			return componentClass.GetMethod(name, bindingAttr, null, CallingConventions.Any, args, null);
		}

		internal static IComparer DefaultComparer
		{
			get
			{
				if (MemberDescriptor.default_comparer == null)
				{
					MemberDescriptor.default_comparer = new MemberDescriptor.MemberDescriptorComparer();
				}
				return MemberDescriptor.default_comparer;
			}
		}

		private class MemberDescriptorComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return string.Compare(((MemberDescriptor)x).Name, ((MemberDescriptor)y).Name, false, CultureInfo.InvariantCulture);
			}
		}
	}
}
