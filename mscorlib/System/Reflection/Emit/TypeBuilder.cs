using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	/// <summary>Defines and creates new instances of classes during run time.</summary>
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_TypeBuilder))]
	public sealed class TypeBuilder : Type, _TypeBuilder
	{
		/// <summary>Represents that total size for the type is not specified.</summary>
		public const int UnspecifiedTypeSize = 0;

		private string tname;

		private string nspace;

		private Type parent;

		private Type nesting_type;

		internal Type[] interfaces;

		internal int num_methods;

		internal MethodBuilder[] methods;

		internal ConstructorBuilder[] ctors;

		internal PropertyBuilder[] properties;

		internal int num_fields;

		internal FieldBuilder[] fields;

		internal EventBuilder[] events;

		private CustomAttributeBuilder[] cattrs;

		internal TypeBuilder[] subtypes;

		internal TypeAttributes attrs;

		private int table_idx;

		private ModuleBuilder pmodule;

		private int class_size;

		private PackingSize packing_size;

		private IntPtr generic_container;

		private GenericTypeParameterBuilder[] generic_params;

		private RefEmitPermissionSet[] permissions;

		private Type created;

		private string fullname;

		private bool createTypeCalled;

		private Type underlying_type;

		internal TypeBuilder(ModuleBuilder mb, TypeAttributes attr, int table_idx)
		{
			this.parent = null;
			this.attrs = attr;
			this.class_size = 0;
			this.table_idx = table_idx;
			this.fullname = (this.tname = ((table_idx != 1) ? ("type_" + table_idx) : "<Module>"));
			this.nspace = string.Empty;
			this.pmodule = mb;
			this.setup_internal_class(this);
		}

		internal TypeBuilder(ModuleBuilder mb, string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packing_size, int type_size, Type nesting_type)
		{
			this.parent = parent;
			this.attrs = attr;
			this.class_size = type_size;
			this.packing_size = packing_size;
			this.nesting_type = nesting_type;
			this.check_name("fullname", name);
			if (parent == null && (attr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic && (attr & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
			{
				throw new InvalidOperationException("Interface must be declared abstract.");
			}
			int num = name.LastIndexOf('.');
			if (num != -1)
			{
				this.tname = name.Substring(num + 1);
				this.nspace = name.Substring(0, num);
			}
			else
			{
				this.tname = name;
				this.nspace = string.Empty;
			}
			if (interfaces != null)
			{
				this.interfaces = new Type[interfaces.Length];
				Array.Copy(interfaces, this.interfaces, interfaces.Length);
			}
			this.pmodule = mb;
			if ((attr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && parent == null && !this.IsCompilerContext)
			{
				this.parent = typeof(object);
			}
			this.table_idx = mb.get_next_table_index(this, 2, true);
			this.setup_internal_class(this);
			this.fullname = this.GetFullName();
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _TypeBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _TypeBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _TypeBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to properties and methods exposed by an object.</summary>
		/// <param name="dispIdMember">Identifies the member.</param>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="lcid">The locale context in which to interpret arguments.</param>
		/// <param name="wFlags">Flags describing the context of the call.</param>
		/// <param name="pDispParams">Pointer to a structure containing an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param>
		/// <param name="pVarResult">Pointer to the location where the result is to be stored.</param>
		/// <param name="pExcepInfo">Pointer to a structure that contains exception information.</param>
		/// <param name="puArgErr">The index of the first argument that has an error.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _TypeBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.attrs;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void setup_internal_class(TypeBuilder tb);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void create_internal_class(TypeBuilder tb);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void setup_generic_class();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void create_generic_class();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern EventInfo get_event_info(EventBuilder eb);

		/// <summary>Retrieves the dynamic assembly that contains this type definition.</summary>
		/// <returns>Read-only. Retrieves the dynamic assembly that contains this type definition.</returns>
		public override Assembly Assembly
		{
			get
			{
				return this.pmodule.Assembly;
			}
		}

		/// <summary>Returns the full name of this type qualified by the display name of the assembly.</summary>
		/// <returns>Read-only. The full name of this type qualified by the display name of the assembly.</returns>
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.fullname + ", " + this.Assembly.FullName;
			}
		}

		/// <summary>Retrieves the base type of this type.</summary>
		/// <returns>Read-only. Retrieves the base type of this type.</returns>
		public override Type BaseType
		{
			get
			{
				return this.parent;
			}
		}

		/// <summary>Returns the type that declared this type.</summary>
		/// <returns>Read-only. The type that declared this type.</returns>
		public override Type DeclaringType
		{
			get
			{
				return this.nesting_type;
			}
		}

		/// <summary>Returns the underlying system type for this TypeBuilder.</summary>
		/// <returns>Read-only. Returns the underlying system type.</returns>
		/// <exception cref="T:System.InvalidOperationException">This type is an enumeration, but there is no underlying system type. </exception>
		public override Type UnderlyingSystemType
		{
			get
			{
				if (this.is_created)
				{
					return this.created.UnderlyingSystemType;
				}
				if (!this.IsEnum || this.IsCompilerContext)
				{
					return this;
				}
				if (this.underlying_type != null)
				{
					return this.underlying_type;
				}
				throw new InvalidOperationException("Enumeration type is not defined.");
			}
		}

		private string GetFullName()
		{
			if (this.nesting_type != null)
			{
				return this.nesting_type.FullName + "+" + this.tname;
			}
			if (this.nspace != null && this.nspace.Length > 0)
			{
				return this.nspace + "." + this.tname;
			}
			return this.tname;
		}

		/// <summary>Retrieves the full path of this type.</summary>
		/// <returns>Read-only. Retrieves the full path of this type.</returns>
		public override string FullName
		{
			get
			{
				return this.fullname;
			}
		}

		/// <summary>Retrieves the GUID of this type.</summary>
		/// <returns>Read-only. Retrieves the GUID of this type </returns>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported for incomplete types. </exception>
		public override Guid GUID
		{
			get
			{
				this.check_created();
				return this.created.GUID;
			}
		}

		/// <summary>Retrieves the dynamic module that contains this type definition.</summary>
		/// <returns>Read-only. Retrieves the dynamic module that contains this type definition.</returns>
		public override Module Module
		{
			get
			{
				return this.pmodule;
			}
		}

		/// <summary>Retrieves the name of this type.</summary>
		/// <returns>Read-only. Retrieves the <see cref="T:System.String" /> name of this type.</returns>
		public override string Name
		{
			get
			{
				return this.tname;
			}
		}

		/// <summary>Retrieves the namespace where this TypeBuilder is defined.</summary>
		/// <returns>Read-only. Retrieves the namespace where this TypeBuilder is defined.</returns>
		public override string Namespace
		{
			get
			{
				return this.nspace;
			}
		}

		/// <summary>Retrieves the packing size of this type.</summary>
		/// <returns>Read-only. Retrieves the packing size of this type.</returns>
		public PackingSize PackingSize
		{
			get
			{
				return this.packing_size;
			}
		}

		/// <summary>Retrieves the total size of a type.</summary>
		/// <returns>Read-only. Retrieves this type’s total size.</returns>
		public int Size
		{
			get
			{
				return this.class_size;
			}
		}

		/// <summary>Returns the type that was used to obtain this type.</summary>
		/// <returns>Read-only. The type that was used to obtain this type.</returns>
		public override Type ReflectedType
		{
			get
			{
				return this.nesting_type;
			}
		}

		/// <summary>Adds declarative security to this type.</summary>
		/// <param name="action">The security action to be taken such as Demand, Assert, and so on. </param>
		/// <param name="pset">The set of permissions the action applies to. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="action" /> is invalid (RequestMinimum, RequestOptional, and RequestRefuse are invalid). </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or- The permission set <paramref name="pset" /> contains an action that was added earlier by AddDeclarativeSecurity. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="pset" /> is null. </exception>
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
		}

		/// <summary>Adds an interface that this type implements.</summary>
		/// <param name="interfaceType">The interface that this type implements. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="interfaceType" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		[ComVisible(true)]
		public void AddInterfaceImplementation(Type interfaceType)
		{
			if (interfaceType == null)
			{
				throw new ArgumentNullException("interfaceType");
			}
			this.check_not_created();
			if (this.interfaces != null)
			{
				foreach (Type type in this.interfaces)
				{
					if (type == interfaceType)
					{
						return;
					}
				}
				Type[] array2 = new Type[this.interfaces.Length + 1];
				this.interfaces.CopyTo(array2, 0);
				array2[this.interfaces.Length] = interfaceType;
				this.interfaces = array2;
			}
			else
			{
				this.interfaces = new Type[1];
				this.interfaces[0] = interfaceType;
			}
		}

		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			this.check_created();
			if (this.created != typeof(object))
			{
				return this.created.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
			}
			if (this.ctors == null)
			{
				return null;
			}
			ConstructorBuilder constructorBuilder = null;
			int num = 0;
			foreach (ConstructorBuilder constructorBuilder2 in this.ctors)
			{
				if (callConvention == CallingConventions.Any || constructorBuilder2.CallingConvention == callConvention)
				{
					constructorBuilder = constructorBuilder2;
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (types != null)
			{
				MethodBase[] array2 = new MethodBase[num];
				if (num == 1)
				{
					array2[0] = constructorBuilder;
				}
				else
				{
					num = 0;
					foreach (ConstructorBuilder constructorInfo in this.ctors)
					{
						if (callConvention == CallingConventions.Any || constructorInfo.CallingConvention == callConvention)
						{
							array2[num++] = constructorInfo;
						}
					}
				}
				if (binder == null)
				{
					binder = Binder.DefaultBinder;
				}
				return (ConstructorInfo)binder.SelectMethod(bindingAttr, array2, types, modifiers);
			}
			if (num > 1)
			{
				throw new AmbiguousMatchException();
			}
			return constructorBuilder;
		}

		/// <summary>Determines whether a custom attribute is applied to the current type.</summary>
		/// <returns>true if one or more instances of <paramref name="attributeType" />, or an attribute derived from <paramref name="attributeType" />, is defined on this type; otherwise, false.</returns>
		/// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported for incomplete types. Retrieve the type using <see cref="M:System.Type.GetType" /> and call <see cref="M:System.Reflection.MemberInfo.IsDefined(System.Type,System.Boolean)" /> on the returned <see cref="T:System.Type" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="attributeType" /> is not defined.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="attributeType" /> is null.</exception>
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (!this.is_created && !this.IsCompilerContext)
			{
				throw new NotSupportedException();
			}
			return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
		}

		/// <summary>Returns all the custom attributes defined for this type.</summary>
		/// <returns>Returns an array of objects representing all the custom attributes of this type.</returns>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported for incomplete types. Retrieve the type using <see cref="M:System.Type.GetType" /> and call <see cref="M:System.Reflection.MemberInfo.GetCustomAttributes(System.Boolean)" /> on the returned <see cref="T:System.Type" />. </exception>
		public override object[] GetCustomAttributes(bool inherit)
		{
			this.check_created();
			return this.created.GetCustomAttributes(inherit);
		}

		/// <summary>Returns all the custom attributes of the current type that are assignable to a specified type.</summary>
		/// <returns>An array of custom attributes defined on the current type.</returns>
		/// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
		/// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported for incomplete types. Retrieve the type using <see cref="M:System.Type.GetType" /> and call <see cref="M:System.Reflection.MemberInfo.GetCustomAttributes(System.Boolean)" /> on the returned <see cref="T:System.Type" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="attributeType" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The type must be a type provided by the underlying runtime system.</exception>
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			this.check_created();
			return this.created.GetCustomAttributes(attributeType, inherit);
		}

		/// <summary>Defines a nested type, given its name.</summary>
		/// <returns>The defined nested type.</returns>
		/// <param name="name">The short name of the type. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		public TypeBuilder DefineNestedType(string name)
		{
			return this.DefineNestedType(name, TypeAttributes.NestedPrivate, this.pmodule.assemblyb.corlib_object_type, null);
		}

		/// <summary>Defines a nested type, given its name and attributes.</summary>
		/// <returns>The defined nested type.</returns>
		/// <param name="name">The short name of the type. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <exception cref="T:System.ArgumentException">The nested attribute is not specified.-or- This type is sealed.-or- This type is an array.-or- This type is an interface, but the nested type is not an interface.-or- The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr)
		{
			return this.DefineNestedType(name, attr, this.pmodule.assemblyb.corlib_object_type, null);
		}

		/// <summary>Defines a nested type, given its name, attributes, and the type that it extends.</summary>
		/// <returns>The defined nested type.</returns>
		/// <param name="name">The short name of the type. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <exception cref="T:System.ArgumentException">The nested attribute is not specified.-or- This type is sealed.-or- This type is an array.-or- This type is an interface, but the nested type is not an interface.-or- The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
		{
			return this.DefineNestedType(name, attr, parent, null);
		}

		private TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packSize, int typeSize)
		{
			if (interfaces != null)
			{
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (interfaces[i] == null)
					{
						throw new ArgumentNullException("interfaces");
					}
				}
			}
			TypeBuilder typeBuilder = new TypeBuilder(this.pmodule, name, attr, parent, interfaces, packSize, typeSize, this);
			typeBuilder.fullname = typeBuilder.GetFullName();
			this.pmodule.RegisterTypeName(typeBuilder, typeBuilder.fullname);
			if (this.subtypes != null)
			{
				TypeBuilder[] array = new TypeBuilder[this.subtypes.Length + 1];
				Array.Copy(this.subtypes, array, this.subtypes.Length);
				array[this.subtypes.Length] = typeBuilder;
				this.subtypes = array;
			}
			else
			{
				this.subtypes = new TypeBuilder[1];
				this.subtypes[0] = typeBuilder;
			}
			return typeBuilder;
		}

		/// <summary>Defines a nested type, given its name, attributes, the type that it extends, and the interfaces that it implements.</summary>
		/// <returns>The defined nested type.</returns>
		/// <param name="name">The short name of the type. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <param name="interfaces">The interfaces that the nested type implements. </param>
		/// <exception cref="T:System.ArgumentException">The nested attribute is not specified.-or- This type is sealed.-or- This type is an array.-or- This type is an interface, but the nested type is not an interface.-or- The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.-or-An element of the <paramref name="interfaces" /> array is null.</exception>
		[ComVisible(true)]
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			return this.DefineNestedType(name, attr, parent, interfaces, PackingSize.Unspecified, 0);
		}

		/// <summary>Defines a nested type, given its name, attributes, the total size of the type, and the type that it extends.</summary>
		/// <returns>The defined nested type.</returns>
		/// <param name="name">The short name of the type. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <param name="typeSize">The total size of the type. </param>
		/// <exception cref="T:System.ArgumentException">The nested attribute is not specified.-or- This type is sealed.-or- This type is an array.-or- This type is an interface, but the nested type is not an interface.-or- The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize)
		{
			return this.DefineNestedType(name, attr, parent, null, PackingSize.Unspecified, typeSize);
		}

		/// <summary>Defines a nested type, given its name, attributes, the type that it extends, and the packing size.</summary>
		/// <returns>The defined nested type.</returns>
		/// <param name="name">The short name of the type. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attr">The attributes of the type. </param>
		/// <param name="parent">The type that the nested type extends. </param>
		/// <param name="packSize">The packing size of the type. </param>
		/// <exception cref="T:System.ArgumentException">The nested attribute is not specified.-or- This type is sealed.-or- This type is an array.-or- This type is an interface, but the nested type is not an interface.-or- The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize)
		{
			return this.DefineNestedType(name, attr, parent, null, packSize, 0);
		}

		/// <summary>Adds a new constructor to the type, with the given attributes and signature.</summary>
		/// <returns>The defined constructor.</returns>
		/// <param name="attributes">The attributes of the constructor. </param>
		/// <param name="callingConvention">The calling convention of the constructor. </param>
		/// <param name="parameterTypes">The parameter types of the constructor. </param>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
		{
			return this.DefineConstructor(attributes, callingConvention, parameterTypes, null, null);
		}

		/// <summary>Adds a new constructor to the type, with the given attributes, signature, and custom modifiers.</summary>
		/// <returns>The defined constructor.</returns>
		/// <param name="attributes">The attributes of the constructor. </param>
		/// <param name="callingConvention">The calling convention of the constructor. </param>
		/// <param name="parameterTypes">The parameter types of the constructor. </param>
		/// <param name="requiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no required custom modifiers, specify null instead of an array of types. If none of the parameters have required custom modifiers, specify null instead of an array of arrays.</param>
		/// <param name="optionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no optional custom modifiers, specify null instead of an array of types. If none of the parameters have optional custom modifiers, specify null instead of an array of arrays.</param>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="requiredCustomModifiers" /> or <paramref name="optionalCustomModifiers" /> does not equal the size of <paramref name="parameterTypes" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false.</exception>
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			this.check_not_created();
			ConstructorBuilder constructorBuilder = new ConstructorBuilder(this, attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
			if (this.ctors != null)
			{
				ConstructorBuilder[] array = new ConstructorBuilder[this.ctors.Length + 1];
				Array.Copy(this.ctors, array, this.ctors.Length);
				array[this.ctors.Length] = constructorBuilder;
				this.ctors = array;
			}
			else
			{
				this.ctors = new ConstructorBuilder[1];
				this.ctors[0] = constructorBuilder;
			}
			return constructorBuilder;
		}

		/// <summary>Defines the default constructor. The constructor defined here will simply call the default constructor of the parent.</summary>
		/// <returns>Returns the constructor.</returns>
		/// <param name="attributes">A MethodAttributes object representing the attributes to be applied to the constructor. </param>
		/// <exception cref="T:System.NotSupportedException">The parent type (base type) does not have a default constructor. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false.</exception>
		[ComVisible(true)]
		public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
		{
			Type corlib_object_type;
			if (this.parent != null)
			{
				corlib_object_type = this.parent;
			}
			else
			{
				corlib_object_type = this.pmodule.assemblyb.corlib_object_type;
			}
			ConstructorInfo constructor = corlib_object_type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			if (constructor == null)
			{
				throw new NotSupportedException("Parent does not have a default constructor. The default constructor must be explicitly defined.");
			}
			ConstructorBuilder constructorBuilder = this.DefineConstructor(attributes, CallingConventions.Standard, Type.EmptyTypes);
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			return constructorBuilder;
		}

		private void append_method(MethodBuilder mb)
		{
			if (this.methods != null)
			{
				if (this.methods.Length == this.num_methods)
				{
					MethodBuilder[] destinationArray = new MethodBuilder[this.methods.Length * 2];
					Array.Copy(this.methods, destinationArray, this.num_methods);
					this.methods = destinationArray;
				}
			}
			else
			{
				this.methods = new MethodBuilder[1];
			}
			this.methods[this.num_methods] = mb;
			this.num_methods++;
		}

		/// <summary>Adds a new method to the type, with the specified name, method attributes, and method signature.</summary>
		/// <returns>The defined method.</returns>
		/// <param name="name">The name of the method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="returnType">The return type of the method. </param>
		/// <param name="parameterTypes">The types of the parameters of the method. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero.-or- The type of the parent of this method is an interface, and this method is not virtual (Overridable in Visual Basic). </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false. </exception>
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		/// <summary>Adds a new method to the type, with the specified name, method attributes, calling convention, and method signature.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.MethodBuilder" /> representing the newly defined method.</returns>
		/// <param name="name">The name of the method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The calling convention of the method. </param>
		/// <param name="returnType">The return type of the method. </param>
		/// <param name="parameterTypes">The types of the parameters of the method. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero.-or- The type of the parent of this method is an interface, and this method is not virtual (Overridable in Visual Basic). </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false. </exception>
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		/// <summary>Adds a new method to the type, with the specified name, method attributes, calling convention, method signature, and custom modifiers.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.MethodBuilder" /> object representing the newly added method.</returns>
		/// <param name="name">The name of the method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The calling convention of the method. </param>
		/// <param name="returnType">The return type of the method. </param>
		/// <param name="returnTypeRequiredCustomModifiers">An array of types representing the required custom modifiers, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />, for the return type of the method. If the return type has no required custom modifiers, specify null.</param>
		/// <param name="returnTypeOptionalCustomModifiers">An array of types representing the optional custom modifiers, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />, for the return type of the method. If the return type has no optional custom modifiers, specify null.</param>
		/// <param name="parameterTypes">The types of the parameters of the method.</param>
		/// <param name="parameterTypeRequiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no required custom modifiers, specify null instead of an array of types. If none of the parameters have required custom modifiers, specify null instead of an array of arrays.</param>
		/// <param name="parameterTypeOptionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no optional custom modifiers, specify null instead of an array of types. If none of the parameters have optional custom modifiers, specify null instead of an array of arrays.</param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero.-or- The type of the parent of this method is an interface, and this method is not virtual (Overridable in Visual Basic). -or-The size of <paramref name="parameterTypeRequiredCustomModifiers" /> or <paramref name="parameterTypeOptionalCustomModifiers" /> does not equal the size of <paramref name="parameterTypes" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false.</exception>
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.check_name("name", name);
			this.check_not_created();
			if (this.IsInterface && ((attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope || (attributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope) && (attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException("Interface method must be abstract and virtual.");
			}
			if (returnType == null)
			{
				returnType = this.pmodule.assemblyb.corlib_void_type;
			}
			MethodBuilder methodBuilder = new MethodBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
			this.append_method(methodBuilder);
			return methodBuilder;
		}

		/// <summary>Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the name of the entry point, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags.</summary>
		/// <returns>The defined PInvoke method.</returns>
		/// <param name="name">The name of the PInvoke method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="dllName">The name of the DLL in which the PInvoke method is defined. </param>
		/// <param name="entryName">The name of the entry point in the DLL. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <param name="nativeCallConv">The native calling convention. </param>
		/// <param name="nativeCharSet">The method's native character set. </param>
		/// <exception cref="T:System.ArgumentException">The method is not static.-or- The parent type is an interface.-or- The method is abstract.-or- The method was previously defined.-or- The length of <paramref name="name" />, <paramref name="dllName" />, or <paramref name="entryName" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" />, <paramref name="dllName" />, or <paramref name="entryName" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
		}

		/// <summary>Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the name of the entry point, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, the PInvoke flags, and custom modifiers for the parameters and return type.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.MethodBuilder" /> representing the defined PInvoke method.</returns>
		/// <param name="name">The name of the PInvoke method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="dllName">The name of the DLL in which the PInvoke method is defined. </param>
		/// <param name="entryName">The name of the entry point in the DLL. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="returnTypeRequiredCustomModifiers">An array of types representing the required custom modifiers, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />, for the return type of the method. If the return type has no required custom modifiers, specify null.</param>
		/// <param name="returnTypeOptionalCustomModifiers">An array of types representing the optional custom modifiers, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />, for the return type of the method. If the return type has no optional custom modifiers, specify null.</param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <param name="parameterTypeRequiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no required custom modifiers, specify null instead of an array of types. If none of the parameters have required custom modifiers, specify null instead of an array of arrays.</param>
		/// <param name="parameterTypeOptionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no optional custom modifiers, specify null instead of an array of types. If none of the parameters have optional custom modifiers, specify null instead of an array of arrays.</param>
		/// <param name="nativeCallConv">The native calling convention. </param>
		/// <param name="nativeCharSet">The method's native character set. </param>
		/// <exception cref="T:System.ArgumentException">The method is not static.-or- The parent type is an interface.-or- The method is abstract.-or- The method was previously defined.-or- The length of <paramref name="name" />, <paramref name="dllName" />, or <paramref name="entryName" /> is zero. -or-The size of <paramref name="parameterTypeRequiredCustomModifiers" /> or <paramref name="parameterTypeOptionalCustomModifiers" /> does not equal the size of <paramref name="parameterTypes" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" />, <paramref name="dllName" />, or <paramref name="entryName" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false.</exception>
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.check_name("name", name);
			this.check_name("dllName", dllName);
			this.check_name("entryName", entryName);
			if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
			{
				throw new ArgumentException("PInvoke methods must be static and native and cannot be abstract.");
			}
			if (this.IsInterface)
			{
				throw new ArgumentException("PInvoke methods cannot exist on interfaces.");
			}
			this.check_not_created();
			MethodBuilder methodBuilder = new MethodBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, dllName, entryName, nativeCallConv, nativeCharSet);
			this.append_method(methodBuilder);
			return methodBuilder;
		}

		/// <summary>Defines a PInvoke method given its name, the name of the DLL in which the method is defined, the attributes of the method, the calling convention of the method, the return type of the method, the types of the parameters of the method, and the PInvoke flags.</summary>
		/// <returns>The defined PInvoke method.</returns>
		/// <param name="name">The name of the PInvoke method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="dllName">The name of the DLL in which the PInvoke method is defined. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The method's calling convention. </param>
		/// <param name="returnType">The method's return type. </param>
		/// <param name="parameterTypes">The types of the method's parameters. </param>
		/// <param name="nativeCallConv">The native calling convention. </param>
		/// <param name="nativeCharSet">The method's native character set. </param>
		/// <exception cref="T:System.ArgumentException">The method is not static.-or- The parent type is an interface.-or- The method is abstract.-or- The method was previously defined.-or- The length of <paramref name="name" /> or <paramref name="dllName" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="dllName" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		/// <summary>Adds a new method to the type, with the specified name and method attributes.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.MethodBuilder" /> representing the newly defined method.</returns>
		/// <param name="name">The name of the method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero.-or- The type of the parent of this method is an interface, and this method is not virtual (Overridable in Visual Basic). </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false. </exception>
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard);
		}

		/// <summary>Adds a new method to the type, with the specified name, method attributes, and calling convention.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.MethodBuilder" /> representing the newly defined method.</returns>
		/// <param name="name">The name of the method. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="callingConvention">The calling convention of the method. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero.-or- The type of the parent of this method is an interface and this method is not virtual (Overridable in Visual Basic). </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false. </exception>
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			return this.DefineMethod(name, attributes, callingConvention, null, null);
		}

		/// <summary>Specifies a given method body that implements a given method declaration, potentially with a different name.</summary>
		/// <param name="methodInfoBody">The method body to be used. This should be a MethodBuilder object. </param>
		/// <param name="methodInfoDeclaration">The method whose declaration is to be used. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="methodInfoBody" /> does not belong to this class. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="methodInfoBody" /> or <paramref name="methodInfoDeclaration" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or- The declaring type of <paramref name="methodInfoBody" /> is not the type represented by this <see cref="T:System.Reflection.Emit.TypeBuilder" />. </exception>
		public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
			if (methodInfoBody == null)
			{
				throw new ArgumentNullException("methodInfoBody");
			}
			if (methodInfoDeclaration == null)
			{
				throw new ArgumentNullException("methodInfoDeclaration");
			}
			this.check_not_created();
			if (methodInfoBody.DeclaringType != this)
			{
				throw new ArgumentException("method body must belong to this type");
			}
			if (methodInfoBody is MethodBuilder)
			{
				MethodBuilder methodBuilder = (MethodBuilder)methodInfoBody;
				methodBuilder.set_override(methodInfoDeclaration);
			}
		}

		/// <summary>Adds a new field to the type, with the given name, attributes, and field type.</summary>
		/// <returns>The defined field.</returns>
		/// <param name="fieldName">The name of the field. <paramref name="fieldName" /> cannot contain embedded nulls. </param>
		/// <param name="type">The type of the field </param>
		/// <param name="attributes">The attributes of the field. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="fieldName" /> is zero.-or- <paramref name="type" /> is System.Void.-or- A total size was specified for the parent class of this field. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fieldName" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.</exception>
		public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
		{
			return this.DefineField(fieldName, type, null, null, attributes);
		}

		/// <summary>Adds a new field to the type, with the given name, attributes, field type, and custom modifiers.</summary>
		/// <returns>The defined field.</returns>
		/// <param name="fieldName">The name of the field. <paramref name="fieldName" /> cannot contain embedded nulls. </param>
		/// <param name="type">The type of the field </param>
		/// <param name="requiredCustomModifiers">An array of types representing the required custom modifiers for the field, such as <see cref="T:Microsoft.VisualC.IsConstModifier" />.</param>
		/// <param name="optionalCustomModifiers">An array of types representing the optional custom modifiers for the field, such as <see cref="T:Microsoft.VisualC.IsConstModifier" />.</param>
		/// <param name="attributes">The attributes of the field. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="fieldName" /> is zero.-or- <paramref name="type" /> is System.Void.-or- A total size was specified for the parent class of this field. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fieldName" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			this.check_name("fieldName", fieldName);
			if (type == typeof(void))
			{
				throw new ArgumentException("Bad field type in defining field.");
			}
			this.check_not_created();
			FieldBuilder fieldBuilder = new FieldBuilder(this, fieldName, type, attributes, requiredCustomModifiers, optionalCustomModifiers);
			if (this.fields != null)
			{
				if (this.fields.Length == this.num_fields)
				{
					FieldBuilder[] destinationArray = new FieldBuilder[this.fields.Length * 2];
					Array.Copy(this.fields, destinationArray, this.num_fields);
					this.fields = destinationArray;
				}
				this.fields[this.num_fields] = fieldBuilder;
				this.num_fields++;
			}
			else
			{
				this.fields = new FieldBuilder[1];
				this.fields[0] = fieldBuilder;
				this.num_fields++;
				this.create_internal_class(this);
			}
			if (this.IsEnum && !this.IsCompilerContext && this.underlying_type == null && (attributes & FieldAttributes.Static) == FieldAttributes.PrivateScope)
			{
				this.underlying_type = type;
			}
			return fieldBuilder;
		}

		/// <summary>Adds a new property to the type, with the given name and property signature.</summary>
		/// <returns>The defined property.</returns>
		/// <param name="name">The name of the property. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the property. </param>
		/// <param name="returnType">The return type of the property. </param>
		/// <param name="parameterTypes">The types of the parameters of the property. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. -or- Any of the elements of the <paramref name="parameterTypes" /> array is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineProperty(name, attributes, returnType, null, null, parameterTypes, null, null);
		}

		/// <summary>Adds a new property to the type, with the given name, property signature, and custom modifiers.</summary>
		/// <returns>The defined property.</returns>
		/// <param name="name">The name of the property. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the property. </param>
		/// <param name="returnType">The return type of the property. </param>
		/// <param name="returnTypeRequiredCustomModifiers">An array of types representing the required custom modifiers, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />, for the return type of the property. If the return type has no required custom modifiers, specify null.</param>
		/// <param name="returnTypeOptionalCustomModifiers">An array of types representing the optional custom modifiers, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />, for the return type of the property. If the return type has no optional custom modifiers, specify null.</param>
		/// <param name="parameterTypes">The types of the parameters of the property. </param>
		/// <param name="parameterTypeRequiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no required custom modifiers, specify null instead of an array of types. If none of the parameters have required custom modifiers, specify null instead of an array of arrays.</param>
		/// <param name="parameterTypeOptionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />. If a particular parameter has no optional custom modifiers, specify null instead of an array of types. If none of the parameters have optional custom modifiers, specify null instead of an array of arrays.</param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null-or- Any of the elements of the <paramref name="parameterTypes" /> array is null</exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.check_name("name", name);
			if (parameterTypes != null)
			{
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					if (parameterTypes[i] == null)
					{
						throw new ArgumentNullException("parameterTypes");
					}
				}
			}
			this.check_not_created();
			PropertyBuilder propertyBuilder = new PropertyBuilder(this, name, attributes, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
			if (this.properties != null)
			{
				PropertyBuilder[] array = new PropertyBuilder[this.properties.Length + 1];
				Array.Copy(this.properties, array, this.properties.Length);
				array[this.properties.Length] = propertyBuilder;
				this.properties = array;
			}
			else
			{
				this.properties = new PropertyBuilder[1];
				this.properties[0] = propertyBuilder;
			}
			return propertyBuilder;
		}

		/// <summary>Defines the initializer for this type.</summary>
		/// <returns>Returns a type initializer.</returns>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		[ComVisible(true)]
		public ConstructorBuilder DefineTypeInitializer()
		{
			return this.DefineConstructor(MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, null);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type create_runtime_class(TypeBuilder tb);

		private bool is_nested_in(Type t)
		{
			while (t != null)
			{
				if (t == this)
				{
					return true;
				}
				t = t.DeclaringType;
			}
			return false;
		}

		private bool has_ctor_method()
		{
			MethodAttributes methodAttributes = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
			for (int i = 0; i < this.num_methods; i++)
			{
				MethodBuilder methodBuilder = this.methods[i];
				if (methodBuilder.Name == ConstructorInfo.ConstructorName && (methodBuilder.Attributes & methodAttributes) == methodAttributes)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Creates a <see cref="T:System.Type" /> object for the class. After defining fields and methods on the class, CreateType is called in order to load its Type object.</summary>
		/// <returns>Returns the new <see cref="T:System.Type" /> object for this class.</returns>
		/// <exception cref="T:System.InvalidOperationException">The enclosing type has not been created.-or- This type is non-abstract and contains an abstract method.-or- This type is not an abstract class or an interface and has a method without a method body. </exception>
		/// <exception cref="T:System.NotSupportedException">The type contains invalid Microsoft intermediate language (MSIL) code.-or- The branch target is specified using a 1-byte offset, but the target is at a distance greater than 127 bytes from the branch. </exception>
		/// <exception cref="T:System.TypeLoadException">The type cannot be loaded. For example, it contains a static method that has the calling convention <see cref="F:System.Reflection.CallingConventions.HasThis" />.</exception>
		public Type CreateType()
		{
			if (this.createTypeCalled)
			{
				return this.created;
			}
			if (!this.IsInterface && this.parent == null && this != this.pmodule.assemblyb.corlib_object_type && this.FullName != "<Module>")
			{
				this.SetParent(this.pmodule.assemblyb.corlib_object_type);
			}
			this.create_generic_class();
			if (this.fields != null)
			{
				foreach (FieldBuilder fieldBuilder in this.fields)
				{
					if (fieldBuilder != null)
					{
						Type fieldType = fieldBuilder.FieldType;
						if (!fieldBuilder.IsStatic && fieldType is TypeBuilder && fieldType.IsValueType && fieldType != this && this.is_nested_in(fieldType))
						{
							TypeBuilder typeBuilder = (TypeBuilder)fieldType;
							if (!typeBuilder.is_created)
							{
								AppDomain.CurrentDomain.DoTypeResolve(typeBuilder);
								if (!typeBuilder.is_created)
								{
								}
							}
						}
					}
				}
			}
			if (this.parent != null && this.parent.IsSealed)
			{
				throw new TypeLoadException(string.Concat(new object[]
				{
					"Could not load type '",
					this.FullName,
					"' from assembly '",
					this.Assembly,
					"' because the parent type is sealed."
				}));
			}
			if (this.parent == this.pmodule.assemblyb.corlib_enum_type && this.methods != null)
			{
				throw new TypeLoadException(string.Concat(new object[]
				{
					"Could not load type '",
					this.FullName,
					"' from assembly '",
					this.Assembly,
					"' because it is an enum with methods."
				}));
			}
			if (this.methods != null)
			{
				bool flag = !this.IsAbstract;
				for (int j = 0; j < this.num_methods; j++)
				{
					MethodBuilder methodBuilder = this.methods[j];
					if (flag && methodBuilder.IsAbstract)
					{
						throw new InvalidOperationException("Type is concrete but has abstract method " + methodBuilder);
					}
					methodBuilder.check_override();
					methodBuilder.fixup();
				}
			}
			if (!this.IsInterface && !this.IsValueType && this.ctors == null && this.tname != "<Module>" && ((this.GetAttributeFlagsImpl() & TypeAttributes.Abstract) | TypeAttributes.Sealed) != (TypeAttributes.Abstract | TypeAttributes.Sealed) && !this.has_ctor_method())
			{
				this.DefineDefaultConstructor(MethodAttributes.Public);
			}
			if (this.ctors != null)
			{
				foreach (ConstructorBuilder constructorBuilder in this.ctors)
				{
					constructorBuilder.fixup();
				}
			}
			this.createTypeCalled = true;
			this.created = this.create_runtime_class(this);
			if (this.created != null)
			{
				return this.created;
			}
			return this;
		}

		internal void GenerateDebugInfo(ISymbolWriter symbolWriter)
		{
			symbolWriter.OpenNamespace(this.Namespace);
			if (this.methods != null)
			{
				for (int i = 0; i < this.num_methods; i++)
				{
					MethodBuilder methodBuilder = this.methods[i];
					methodBuilder.GenerateDebugInfo(symbolWriter);
				}
			}
			if (this.ctors != null)
			{
				foreach (ConstructorBuilder constructorBuilder in this.ctors)
				{
					constructorBuilder.GenerateDebugInfo(symbolWriter);
				}
			}
			symbolWriter.CloseNamespace();
			if (this.subtypes != null)
			{
				for (int k = 0; k < this.subtypes.Length; k++)
				{
					this.subtypes[k].GenerateDebugInfo(symbolWriter);
				}
			}
		}

		/// <summary>Returns an array of <see cref="T:System.Reflection.ConstructorInfo" /> objects representing the public and non-public constructors defined for this class, as specified.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.ConstructorInfo" /> objects representing the specified constructors defined for this class. If no constructors are defined, an empty array is returned.</returns>
		/// <param name="bindingAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" /> as in InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			if (this.is_created)
			{
				return this.created.GetConstructors(bindingAttr);
			}
			if (!this.IsCompilerContext)
			{
				throw new NotSupportedException();
			}
			return this.GetConstructorsInternal(bindingAttr);
		}

		internal ConstructorInfo[] GetConstructorsInternal(BindingFlags bindingAttr)
		{
			if (this.ctors == null)
			{
				return new ConstructorInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (ConstructorBuilder constructorBuilder in this.ctors)
			{
				bool flag = false;
				MethodAttributes attributes = constructorBuilder.Attributes;
				if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
				{
					if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
					{
						flag = true;
					}
				}
				else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
				{
					flag = true;
				}
				if (flag)
				{
					flag = false;
					if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
					{
						if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
						{
							flag = true;
						}
					}
					else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
					{
						flag = true;
					}
					if (flag)
					{
						arrayList.Add(constructorBuilder);
					}
				}
			}
			ConstructorInfo[] array2 = new ConstructorInfo[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		/// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>This method is not supported. No value is returned.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
		public override Type GetElementType()
		{
			throw new NotSupportedException();
		}

		/// <summary>Returns the event with the specified name.</summary>
		/// <returns>An <see cref="T:System.Reflection.EventInfo" /> object representing the event declared or inherited by this type with the specified name, or null if there are no matches.</returns>
		/// <param name="name">The name of the event to search for. </param>
		/// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values that limits the search. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			this.check_created();
			return this.created.GetEvent(name, bindingAttr);
		}

		/// <summary>Returns the public events declared or inherited by this type.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.EventInfo" /> objects representing the public events declared or inherited by this type. An empty array is returned if there are no public events.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		public override EventInfo[] GetEvents()
		{
			return this.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		/// <summary>Returns the public and non-public events that are declared by this type.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.EventInfo" /> objects representing the events declared or inherited by this type that match the specified binding flags. An empty array is returned if there are no matching events.</returns>
		/// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values that limits the search.</param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			if (this.is_created)
			{
				return this.created.GetEvents(bindingAttr);
			}
			if (!this.IsCompilerContext)
			{
				throw new NotSupportedException();
			}
			return new EventInfo[0];
		}

		internal EventInfo[] GetEvents_internal(BindingFlags bindingAttr)
		{
			if (this.events == null)
			{
				return new EventInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (EventBuilder eventBuilder in this.events)
			{
				if (eventBuilder != null)
				{
					EventInfo eventInfo = this.get_event_info(eventBuilder);
					bool flag = false;
					MethodInfo methodInfo = eventInfo.GetAddMethod(true);
					if (methodInfo == null)
					{
						methodInfo = eventInfo.GetRemoveMethod(true);
					}
					if (methodInfo != null)
					{
						MethodAttributes attributes = methodInfo.Attributes;
						if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
						{
							if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
							{
								flag = true;
							}
						}
						else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
						{
							flag = true;
						}
						if (flag)
						{
							flag = false;
							if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
							{
								if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
								{
									flag = true;
								}
							}
							else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
							{
								flag = true;
							}
							if (flag)
							{
								arrayList.Add(eventInfo);
							}
						}
					}
				}
			}
			EventInfo[] array2 = new EventInfo[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		/// <summary>Returns the field specified by the given name.</summary>
		/// <returns>Returns the <see cref="T:System.Reflection.FieldInfo" /> object representing the field declared or inherited by this type with the specified name and public or non-public modifier. If there are no matches then null is returned.</returns>
		/// <param name="name">The name of the field to get. </param>
		/// <param name="bindingAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" /> as in InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (this.created != null)
			{
				return this.created.GetField(name, bindingAttr);
			}
			if (this.fields == null)
			{
				return null;
			}
			foreach (FieldBuilder fieldInfo in this.fields)
			{
				if (fieldInfo != null)
				{
					if (!(fieldInfo.Name != name))
					{
						bool flag = false;
						FieldAttributes attributes = fieldInfo.Attributes;
						if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
						{
							if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
							{
								flag = true;
							}
						}
						else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
						{
							flag = true;
						}
						if (flag)
						{
							flag = false;
							if ((attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
							{
								if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
								{
									flag = true;
								}
							}
							else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
							{
								flag = true;
							}
							if (flag)
							{
								return fieldInfo;
							}
						}
					}
				}
			}
			return null;
		}

		/// <summary>Returns the public and non-public fields that are declared by this type.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.FieldInfo" /> objects representing the public and non-public fields declared or inherited by this type. An empty array is returned if there are no fields, as specified.</returns>
		/// <param name="bindingAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" /> : InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			if (this.created != null)
			{
				return this.created.GetFields(bindingAttr);
			}
			if (this.fields == null)
			{
				return new FieldInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (FieldBuilder fieldInfo in this.fields)
			{
				if (fieldInfo != null)
				{
					bool flag = false;
					FieldAttributes attributes = fieldInfo.Attributes;
					if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
					{
						if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
						{
							flag = true;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
					{
						flag = true;
					}
					if (flag)
					{
						flag = false;
						if ((attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
						{
							if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
							{
								flag = true;
							}
						}
						else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
						{
							flag = true;
						}
						if (flag)
						{
							arrayList.Add(fieldInfo);
						}
					}
				}
			}
			FieldInfo[] array2 = new FieldInfo[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		/// <summary>Returns the interface implemented (directly or indirectly) by this class with the fully qualified name matching the given interface name.</summary>
		/// <returns>Returns a <see cref="T:System.Type" /> object representing the implemented interface. Returns null if no interface matching name is found.</returns>
		/// <param name="name">The name of the interface. </param>
		/// <param name="ignoreCase">If true, the search is case-insensitive. If false, the search is case-sensitive. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		public override Type GetInterface(string name, bool ignoreCase)
		{
			this.check_created();
			return this.created.GetInterface(name, ignoreCase);
		}

		/// <summary>Returns an array of all the interfaces implemented on this type and its base types.</summary>
		/// <returns>Returns an array of <see cref="T:System.Type" /> objects representing the implemented interfaces. If none are defined, an empty array is returned.</returns>
		public override Type[] GetInterfaces()
		{
			if (this.is_created)
			{
				return this.created.GetInterfaces();
			}
			if (this.interfaces != null)
			{
				Type[] array = new Type[this.interfaces.Length];
				this.interfaces.CopyTo(array, 0);
				return array;
			}
			return Type.EmptyTypes;
		}

		/// <summary>Returns all the public and non-public members declared or inherited by this type, as specified.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.MemberInfo" /> objects representing the public and non-public members defined on this type if <paramref name="nonPublic" /> is used; otherwise, only the public members are returned.</returns>
		/// <param name="name">The name of the member. </param>
		/// <param name="type">The type of the member to return. </param>
		/// <param name="bindingAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" />, as in InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			this.check_created();
			return this.created.GetMember(name, type, bindingAttr);
		}

		/// <summary>Returns the members for the public and non-public members declared or inherited by this type.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.MemberInfo" /> objects representing the public and non-public members declared or inherited by this type. An empty array is returned if there are no matching members.</returns>
		/// <param name="bindingAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" />, such as InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			this.check_created();
			return this.created.GetMembers(bindingAttr);
		}

		private MethodInfo[] GetMethodsByName(string name, BindingFlags bindingAttr, bool ignoreCase, Type reflected_type)
		{
			MethodInfo[] array2;
			if ((bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default && this.parent != null)
			{
				MethodInfo[] array = this.parent.GetMethods(bindingAttr);
				ArrayList arrayList = new ArrayList(array.Length);
				bool flag = (bindingAttr & BindingFlags.FlattenHierarchy) != BindingFlags.Default;
				foreach (MethodInfo methodInfo in array)
				{
					MethodAttributes attributes = methodInfo.Attributes;
					if (!methodInfo.IsStatic || flag)
					{
						bool flag2;
						switch (attributes & MethodAttributes.MemberAccessMask)
						{
						case MethodAttributes.Private:
							flag2 = false;
							break;
						case MethodAttributes.FamANDAssem:
						case MethodAttributes.Family:
						case MethodAttributes.FamORAssem:
							goto IL_B6;
						case MethodAttributes.Assembly:
							flag2 = ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default);
							break;
						case MethodAttributes.Public:
							flag2 = ((bindingAttr & BindingFlags.Public) != BindingFlags.Default);
							break;
						default:
							goto IL_B6;
						}
						IL_C6:
						if (flag2)
						{
							arrayList.Add(methodInfo);
							goto IL_D6;
						}
						goto IL_D6;
						IL_B6:
						flag2 = ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default);
						goto IL_C6;
					}
					IL_D6:;
				}
				if (this.methods == null)
				{
					array2 = new MethodInfo[arrayList.Count];
					arrayList.CopyTo(array2);
				}
				else
				{
					array2 = new MethodInfo[this.methods.Length + arrayList.Count];
					arrayList.CopyTo(array2, 0);
					this.methods.CopyTo(array2, arrayList.Count);
				}
			}
			else
			{
				array2 = this.methods;
			}
			if (array2 == null)
			{
				return new MethodInfo[0];
			}
			ArrayList arrayList2 = new ArrayList();
			foreach (MethodInfo methodInfo2 in array2)
			{
				if (methodInfo2 != null)
				{
					if (name == null || string.Compare(methodInfo2.Name, name, ignoreCase) == 0)
					{
						bool flag2 = false;
						MethodAttributes attributes = methodInfo2.Attributes;
						if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
						{
							if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
							{
								flag2 = true;
							}
						}
						else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
						{
							flag2 = true;
						}
						if (flag2)
						{
							flag2 = false;
							if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
							{
								if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
								{
									flag2 = true;
								}
							}
							else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
							{
								flag2 = true;
							}
							if (flag2)
							{
								arrayList2.Add(methodInfo2);
							}
						}
					}
				}
			}
			MethodInfo[] array4 = new MethodInfo[arrayList2.Count];
			arrayList2.CopyTo(array4);
			return array4;
		}

		/// <summary>Returns all the public and non-public methods declared or inherited by this type, as specified.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.MethodInfo" /> objects representing the public and non-public methods defined on this type if <paramref name="nonPublic" /> is used; otherwise, only the public methods are returned.</returns>
		/// <param name="bindingAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" /> as in InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.GetMethodsByName(null, bindingAttr, false, this);
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			this.check_created();
			bool ignoreCase = (bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default;
			MethodInfo[] methodsByName = this.GetMethodsByName(name, bindingAttr, ignoreCase, this);
			MethodInfo methodInfo = null;
			int num = (types == null) ? 0 : types.Length;
			int num2 = 0;
			foreach (MethodInfo methodInfo2 in methodsByName)
			{
				if (callConvention == CallingConventions.Any || (methodInfo2.CallingConvention & callConvention) == callConvention)
				{
					methodInfo = methodInfo2;
					num2++;
				}
			}
			if (num2 == 0)
			{
				return null;
			}
			if (num2 == 1 && num == 0)
			{
				return methodInfo;
			}
			MethodBase[] array2 = new MethodBase[num2];
			if (num2 == 1)
			{
				array2[0] = methodInfo;
			}
			else
			{
				num2 = 0;
				foreach (MethodInfo methodInfo3 in methodsByName)
				{
					if (callConvention == CallingConventions.Any || (methodInfo3.CallingConvention & callConvention) == callConvention)
					{
						array2[num2++] = methodInfo3;
					}
				}
			}
			if (types == null)
			{
				return (MethodInfo)Binder.FindMostDerivedMatch(array2);
			}
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			return (MethodInfo)binder.SelectMethod(bindingAttr, array2, types, modifiers);
		}

		/// <summary>Returns the public and non-public nested types that are declared by this type.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing the nested type that matches the specified requirements, if found; otherwise, null.</returns>
		/// <param name="name">The <see cref="T:System.String" /> containing the name of the nested type to get. </param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags" /> that specify how the search is conducted.-or- Zero, to conduct a case-sensitive search for public methods. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			this.check_created();
			if (this.subtypes == null)
			{
				return null;
			}
			foreach (TypeBuilder typeBuilder in this.subtypes)
			{
				if (typeBuilder.is_created)
				{
					if ((typeBuilder.attrs & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
					{
						if ((bindingAttr & BindingFlags.Public) == BindingFlags.Default)
						{
							goto IL_7C;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
					{
						goto IL_7C;
					}
					if (typeBuilder.Name == name)
					{
						return typeBuilder.created;
					}
				}
				IL_7C:;
			}
			return null;
		}

		/// <summary>Returns the public and non-public nested types that are declared or inherited by this type.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> objects representing all the types nested within the current <see cref="T:System.Type" /> that match the specified binding constraints.An empty array of type <see cref="T:System.Type" />, if no types are nested within the current <see cref="T:System.Type" />, or if none of the nested types match the binding constraints.</returns>
		/// <param name="bindingAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" />, as in InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			if (!this.is_created && !this.IsCompilerContext)
			{
				throw new NotSupportedException();
			}
			ArrayList arrayList = new ArrayList();
			if (this.subtypes == null)
			{
				return Type.EmptyTypes;
			}
			foreach (TypeBuilder typeBuilder in this.subtypes)
			{
				bool flag = false;
				if ((typeBuilder.attrs & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
				{
					if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
					{
						flag = true;
					}
				}
				else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
				{
					flag = true;
				}
				if (flag)
				{
					arrayList.Add(typeBuilder);
				}
			}
			Type[] array2 = new Type[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		/// <summary>Returns all the public and non-public properties declared or inherited by this type, as specified.</summary>
		/// <returns>Returns an array of PropertyInfo objects representing the public and non-public properties defined on this type if <paramref name="nonPublic" /> is used; otherwise, only the public properties are returned.</returns>
		/// <param name="bindingAttr">This invocation attribute. This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" /> : InvokeMethod, NonPublic, and so on. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			if (this.is_created)
			{
				return this.created.GetProperties(bindingAttr);
			}
			if (this.properties == null)
			{
				return new PropertyInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (PropertyBuilder propertyInfo in this.properties)
			{
				bool flag = false;
				MethodInfo methodInfo = propertyInfo.GetGetMethod(true);
				if (methodInfo == null)
				{
					methodInfo = propertyInfo.GetSetMethod(true);
				}
				if (methodInfo != null)
				{
					MethodAttributes attributes = methodInfo.Attributes;
					if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
					{
						if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
						{
							flag = true;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
					{
						flag = true;
					}
					if (flag)
					{
						flag = false;
						if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
						{
							if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
							{
								flag = true;
							}
						}
						else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
						{
							flag = true;
						}
						if (flag)
						{
							arrayList.Add(propertyInfo);
						}
					}
				}
			}
			PropertyInfo[] array2 = new PropertyInfo[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw this.not_supported();
		}

		protected override bool HasElementTypeImpl()
		{
			return this.is_created && this.created.HasElementType;
		}

		/// <summary>Invokes the specified member. The method that is to be invoked must be accessible and provide the most specific match with the specified argument list, under the constraints of the specified binder and invocation attributes.</summary>
		/// <returns>Returns the return value of the invoked member.</returns>
		/// <param name="name">The name of the member to invoke. This can be a constructor, method, property, or field. A suitable invocation attribute must be specified. Note that it is possible to invoke the default member of a class by passing an empty string as the name of the member. </param>
		/// <param name="invokeAttr">The invocation attribute. This must be a bit flag from BindingFlags. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects using reflection. If binder is null, the default binder is used. See <see cref="T:System.Reflection.Binder" />. </param>
		/// <param name="target">The object on which to invoke the specified member. If the member is static, this parameter is ignored. </param>
		/// <param name="args">An argument list. This is an array of Objects that contains the number, order, and type of the parameters of the member to be invoked. If there are no parameters this should be null. </param>
		/// <param name="modifiers">An array of the same length as <paramref name="args" /> with elements that represent the attributes associated with the arguments of the member to be invoked. A parameter has attributes associated with it in the metadata. They are used by various interoperability services. See the metadata specs for more details. </param>
		/// <param name="culture">An instance of CultureInfo used to govern the coercion of types. If this is null, the CultureInfo for the current thread is used. (Note that this is necessary to, for example, convert a String that represents 1000 to a Double value, since 1000 is represented differently by different cultures.) </param>
		/// <param name="namedParameters">Each parameter in the <paramref name="namedParameters" /> array gets the value in the corresponding element in the <paramref name="args" /> array. If the length of <paramref name="args" /> is greater than the length of <paramref name="namedParameters" />, the remaining argument values are passed in order. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported for incomplete types. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			this.check_created();
			return this.created.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		protected override bool IsArrayImpl()
		{
			return false;
		}

		protected override bool IsByRefImpl()
		{
			return false;
		}

		protected override bool IsCOMObjectImpl()
		{
			return (this.GetAttributeFlagsImpl() & TypeAttributes.Import) != TypeAttributes.NotPublic;
		}

		protected override bool IsPointerImpl()
		{
			return false;
		}

		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		protected override bool IsValueTypeImpl()
		{
			return (Type.type_is_subtype_of(this, this.pmodule.assemblyb.corlib_value_type, false) || Type.type_is_subtype_of(this, typeof(ValueType), false)) && this != this.pmodule.assemblyb.corlib_value_type && this != this.pmodule.assemblyb.corlib_enum_type;
		}

		/// <summary>Returns a <see cref="T:System.Type" /> object that represents a one-dimensional array of the current type, with a lower bound of zero.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing a one-dimensional array type whose element type is the current type, with a lower bound of zero.</returns>
		public override Type MakeArrayType()
		{
			return new ArrayType(this, 0);
		}

		/// <summary>Returns a <see cref="T:System.Type" /> object that represents an array of the current type, with the specified number of dimensions.</summary>
		/// <returns>A <see cref="T:System.Type" /> object that represents a one-dimensional array of the current type.</returns>
		/// <param name="rank">The number of dimensions for the array. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="rank" /> is not a valid array dimension.</exception>
		public override Type MakeArrayType(int rank)
		{
			if (rank < 1)
			{
				throw new IndexOutOfRangeException();
			}
			return new ArrayType(this, rank);
		}

		/// <summary>Returns a <see cref="T:System.Type" /> object that represents the current type when passed as a ref parameter (ByRef in Visual Basic).</summary>
		/// <returns>A <see cref="T:System.Type" /> object that represents the current type when passed as a ref parameter (ByRef in Visual Basic).</returns>
		public override Type MakeByRefType()
		{
			return new ByRefType(this);
		}

		/// <summary>Substitutes the elements of an array of types for the type parameters of the current generic type definition, and returns the resulting constructed type.</summary>
		/// <returns>A <see cref="T:System.Type" /> representing the constructed type formed by substituting the elements of <paramref name="typeArguments" /> for the type parameters of the current generic type. </returns>
		/// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic type definition.</param>
		/// <exception cref="T:System.InvalidOperationException">The current type does not represent the definition of a generic type. That is, <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> returns false. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeArguments" /> is null.-or- Any element of <paramref name="typeArguments" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">Any element of <paramref name="typeArguments" /> does not satisfy the constraints specified for the corresponding type parameter of the current generic type. </exception>
		[MonoTODO]
		public override Type MakeGenericType(params Type[] typeArguments)
		{
			return base.MakeGenericType(typeArguments);
		}

		/// <summary>Returns a <see cref="T:System.Type" /> object that represents the type of an unmanaged pointer to the current type.</summary>
		/// <returns>A <see cref="T:System.Type" /> object that represents the type of an unmanaged pointer to the current type.</returns>
		public override Type MakePointerType()
		{
			return new PointerType(this);
		}

		/// <summary>Not supported in dynamic modules.</summary>
		/// <returns>Read-only.</returns>
		/// <exception cref="T:System.NotSupportedException">Not supported in dynamic modules. </exception>
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				this.check_created();
				return this.created.TypeHandle;
			}
		}

		internal void SetCharSet(TypeAttributes ta)
		{
			this.attrs = ta;
		}

		/// <summary>Set a custom attribute using a custom attribute builder.</summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="customBuilder" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false.</exception>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			string fullName = customBuilder.Ctor.ReflectedType.FullName;
			if (fullName == "System.Runtime.InteropServices.StructLayoutAttribute")
			{
				byte[] data = customBuilder.Data;
				int num = (int)data[2];
				num |= (int)data[3] << 8;
				this.attrs &= ~TypeAttributes.LayoutMask;
				switch (num)
				{
				case 0:
					this.attrs |= TypeAttributes.SequentialLayout;
					goto IL_B8;
				case 2:
					this.attrs |= TypeAttributes.ExplicitLayout;
					goto IL_B8;
				case 3:
					this.attrs |= TypeAttributes.NotPublic;
					goto IL_B8;
				}
				throw new Exception("Error in customattr");
				IL_B8:
				string fullName2 = customBuilder.Ctor.GetParameters()[0].ParameterType.FullName;
				int num2 = 6;
				if (fullName2 == "System.Int16")
				{
					num2 = 4;
				}
				int num3 = (int)data[num2++];
				num3 |= (int)data[num2++] << 8;
				int i = 0;
				while (i < num3)
				{
					num2++;
					byte b = data[num2++];
					int num4;
					if (b == 85)
					{
						num4 = CustomAttributeBuilder.decode_len(data, num2, out num2);
						CustomAttributeBuilder.string_from_bytes(data, num2, num4);
						num2 += num4;
					}
					num4 = CustomAttributeBuilder.decode_len(data, num2, out num2);
					string text = CustomAttributeBuilder.string_from_bytes(data, num2, num4);
					num2 += num4;
					int num5 = (int)data[num2++];
					num5 |= (int)data[num2++] << 8;
					num5 |= (int)data[num2++] << 16;
					num5 |= (int)data[num2++] << 24;
					string text2 = text;
					switch (text2)
					{
					case "CharSet":
						switch (num5)
						{
						case 1:
						case 2:
							this.attrs &= ~TypeAttributes.StringFormatMask;
							break;
						case 3:
							this.attrs &= ~TypeAttributes.AutoClass;
							this.attrs |= TypeAttributes.UnicodeClass;
							break;
						case 4:
							this.attrs &= ~TypeAttributes.UnicodeClass;
							this.attrs |= TypeAttributes.AutoClass;
							break;
						}
						break;
					case "Pack":
						this.packing_size = (PackingSize)num5;
						break;
					case "Size":
						this.class_size = num5;
						break;
					}
					IL_2C7:
					i++;
					continue;
					goto IL_2C7;
				}
				return;
			}
			if (fullName == "System.Runtime.CompilerServices.SpecialNameAttribute")
			{
				this.attrs |= TypeAttributes.SpecialName;
				return;
			}
			if (fullName == "System.SerializableAttribute")
			{
				this.attrs |= TypeAttributes.Serializable;
				return;
			}
			if (fullName == "System.Runtime.InteropServices.ComImportAttribute")
			{
				this.attrs |= TypeAttributes.Import;
				return;
			}
			if (fullName == "System.Security.SuppressUnmanagedCodeSecurityAttribute")
			{
				this.attrs |= TypeAttributes.HasSecurity;
			}
			if (this.cattrs != null)
			{
				CustomAttributeBuilder[] array = new CustomAttributeBuilder[this.cattrs.Length + 1];
				this.cattrs.CopyTo(array, 0);
				array[this.cattrs.Length] = customBuilder;
				this.cattrs = array;
			}
			else
			{
				this.cattrs = new CustomAttributeBuilder[1];
				this.cattrs[0] = customBuilder;
			}
		}

		/// <summary>Sets a custom attribute using a specified custom attribute blob.</summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="con" /> or <paramref name="binaryAttribute" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false.</exception>
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		/// <summary>Adds a new event to the type, with the given name, attributes and event type.</summary>
		/// <returns>The defined event.</returns>
		/// <param name="name">The name of the event. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the event. </param>
		/// <param name="eventtype">The type of the event. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.-or- <paramref name="eventtype" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
		{
			this.check_name("name", name);
			if (eventtype == null)
			{
				throw new ArgumentNullException("type");
			}
			this.check_not_created();
			EventBuilder eventBuilder = new EventBuilder(this, name, attributes, eventtype);
			if (this.events != null)
			{
				EventBuilder[] array = new EventBuilder[this.events.Length + 1];
				Array.Copy(this.events, array, this.events.Length);
				array[this.events.Length] = eventBuilder;
				this.events = array;
			}
			else
			{
				this.events = new EventBuilder[1];
				this.events[0] = eventBuilder;
			}
			return eventBuilder;
		}

		/// <summary>Defines initialized data field in the .sdata section of the portable executable (PE) file.</summary>
		/// <returns>A field to reference the data.</returns>
		/// <param name="name">The name used to refer to the data. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="data">The blob of data. </param>
		/// <param name="attributes">The attributes for the field. </param>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="name" /> is zero.-or- The size of the data is less than or equal to zero, or greater than or equal to 0x3f0000. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="data" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> has been previously called. </exception>
		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			FieldBuilder fieldBuilder = this.DefineUninitializedData(name, data.Length, attributes);
			fieldBuilder.SetRVAData(data);
			return fieldBuilder;
		}

		/// <summary>Defines an uninitialized data field in the .sdata section of the portable executable (PE) file.</summary>
		/// <returns>A field to reference the data.</returns>
		/// <param name="name">The name used to refer to the data. <paramref name="name" /> cannot contain embedded nulls. </param>
		/// <param name="size">The size of the data field. </param>
		/// <param name="attributes">The attributes for the field. </param>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="name" /> is zero.-or- <paramref name="size" /> is less than or equal to zero, or greater than or equal to 0x003f0000. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Empty name is not legal", "name");
			}
			if (size <= 0 || size > 4128768)
			{
				throw new ArgumentException("Data size must be > 0 and < 0x3f0000");
			}
			this.check_not_created();
			string text = "$ArrayType$" + size;
			Type type = this.pmodule.GetRegisteredType(this.fullname + "+" + text);
			if (type == null)
			{
				TypeBuilder typeBuilder = this.DefineNestedType(text, TypeAttributes.Public | TypeAttributes.NestedPublic | TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, this.pmodule.assemblyb.corlib_value_type, null, PackingSize.Size1, size);
				typeBuilder.CreateType();
				type = typeBuilder;
			}
			return this.DefineField(name, type, attributes | FieldAttributes.Static | FieldAttributes.HasFieldRVA);
		}

		/// <summary>Returns the type token of this type.</summary>
		/// <returns>Read-only. Returns the TypeToken of this type.</returns>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.</exception>
		public TypeToken TypeToken
		{
			get
			{
				return new TypeToken(33554432 | this.table_idx);
			}
		}

		/// <summary>Sets the base type of the type currently under construction.</summary>
		/// <param name="parent">The new base type. </param>
		/// <exception cref="T:System.InvalidOperationException">The type was previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or-<paramref name="parent" /> is null, and the current instance represents an interface whose attributes do not include <see cref="F:System.Reflection.TypeAttributes.Abstract" />.-or-For the current dynamic type, the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> property is true, but the <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericTypeDefinition" /> property is false. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="parent" /> is an interface. This exception condition is new in the .NET Framework version 2.0. </exception>
		public void SetParent(Type parent)
		{
			this.check_not_created();
			if (parent == null)
			{
				if ((this.attrs & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic)
				{
					if ((this.attrs & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
					{
						throw new InvalidOperationException("Interface must be declared abstract.");
					}
					this.parent = null;
				}
				else
				{
					this.parent = typeof(object);
				}
			}
			else
			{
				this.parent = parent;
			}
			this.setup_internal_class(this);
		}

		internal int get_next_table_index(object obj, int table, bool inc)
		{
			return this.pmodule.get_next_table_index(obj, table, inc);
		}

		/// <summary>Returns an interface mapping for the requested interface.</summary>
		/// <returns>Returns the requested interface mapping.</returns>
		/// <param name="interfaceType">The <see cref="T:System.Type" /> of the interface for which the mapping is to be retrieved. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented for incomplete types. </exception>
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			if (this.created == null)
			{
				throw new NotSupportedException("This method is not implemented for incomplete types.");
			}
			return this.created.GetInterfaceMap(interfaceType);
		}

		internal bool IsCompilerContext
		{
			get
			{
				return this.pmodule.assemblyb.IsCompilerContext;
			}
		}

		internal bool is_created
		{
			get
			{
				return this.created != null;
			}
		}

		private Exception not_supported()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		private void check_not_created()
		{
			if (this.is_created)
			{
				throw new InvalidOperationException("Unable to change after type has been created.");
			}
		}

		private void check_created()
		{
			if (!this.is_created)
			{
				throw this.not_supported();
			}
		}

		private void check_name(string argName, string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException(argName);
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Empty name is not legal", argName);
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException("Illegal name", argName);
			}
		}

		/// <summary>Returns the name of the type excluding the namespace.</summary>
		/// <returns>Read-only. The name of the type excluding the namespace.</returns>
		public override string ToString()
		{
			return this.FullName;
		}

		/// <summary>Determines whether an instance of the current <see cref="T:System.Type" /> can be assigned from an instance of the specified Type.</summary>
		/// <returns>true if the <paramref name="c" /> parameter and the current <see cref="T:System.Type" /> represent the same type, or if the current Type is in the inheritance hierarchy of <paramref name="c" />, or if the current Type is an interface that <paramref name="c" /> supports. false if none of these conditions are the case, or if <paramref name="c" /> is a null reference (Nothing in Visual Basic).</returns>
		/// <param name="c">The Type to compare with the current Type. </param>
		[MonoTODO]
		public override bool IsAssignableFrom(Type c)
		{
			return base.IsAssignableFrom(c);
		}

		/// <summary>Determines whether this type is derived from a specified type.</summary>
		/// <returns>Read-only. Returns true if this type is the same as the type <paramref name="c" />, or is a subtype of type <paramref name="c" />; otherwise, false.</returns>
		/// <param name="c">A <see cref="T:System.Type" /> that is to be checked. </param>
		[ComVisible(true)]
		[MonoTODO]
		public override bool IsSubclassOf(Type c)
		{
			return base.IsSubclassOf(c);
		}

		[MonoTODO("arrays")]
		internal bool IsAssignableTo(Type c)
		{
			if (c == this)
			{
				return true;
			}
			if (c.IsInterface)
			{
				if (this.parent != null && this.is_created && c.IsAssignableFrom(this.parent))
				{
					return true;
				}
				if (this.interfaces == null)
				{
					return false;
				}
				foreach (Type c2 in this.interfaces)
				{
					if (c.IsAssignableFrom(c2))
					{
						return true;
					}
				}
				if (!this.is_created)
				{
					return false;
				}
			}
			if (this.parent == null)
			{
				return c == typeof(object);
			}
			return c.IsAssignableFrom(this.parent);
		}

		/// <summary>Returns a value that indicates whether the current dynamic type has been created.</summary>
		/// <returns>true if the <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> method has been called; otherwise, false. </returns>
		public bool IsCreated()
		{
			return this.is_created;
		}

		/// <summary>Returns an array of <see cref="T:System.Type" /> objects representing the type arguments of a generic type or the type parameters of a generic type definition.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> objects. The elements of the array represent the type arguments of a generic type or the type parameters of a generic type definition.</returns>
		public override Type[] GetGenericArguments()
		{
			if (this.generic_params == null)
			{
				return null;
			}
			Type[] array = new Type[this.generic_params.Length];
			this.generic_params.CopyTo(array, 0);
			return array;
		}

		/// <summary>Returns a <see cref="T:System.Type" /> object that represents a generic type definition from which the current type can be obtained.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing a generic type definition from which the current type can be obtained.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current type is not generic. That is, <see cref="P:System.Reflection.Emit.TypeBuilder.IsGenericType" /> returns false.</exception>
		public override Type GetGenericTypeDefinition()
		{
			if (this.generic_params == null)
			{
				throw new InvalidOperationException("Type is not generic");
			}
			return this;
		}

		public override bool ContainsGenericParameters
		{
			get
			{
				return this.generic_params != null;
			}
		}

		/// <summary>Gets a value indicating whether the current type is a generic type parameter.</summary>
		/// <returns>true if the current <see cref="T:System.Reflection.Emit.TypeBuilder" /> object represents a generic type parameter; otherwise, false.</returns>
		public override extern bool IsGenericParameter { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>Gets a value that indicates the covariance and special constraints of the current generic type parameter. </summary>
		/// <returns>A bitwise combination of <see cref="T:System.Reflection.GenericParameterAttributes" /> values that describes the covariance and special constraints of the current generic type parameter.</returns>
		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				return GenericParameterAttributes.None;
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Reflection.Emit.TypeBuilder" /> represents a generic type definition from which other generic types can be constructed.</summary>
		/// <returns>true if this <see cref="T:System.Reflection.Emit.TypeBuilder" /> object represents a generic type definition; otherwise, false.</returns>
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return this.generic_params != null;
			}
		}

		/// <summary>Gets a value indicating whether the current type is a generic type. </summary>
		/// <returns>true if the type represented by the current <see cref="T:System.Reflection.Emit.TypeBuilder" /> object is generic; otherwise, false.</returns>
		public override bool IsGenericType
		{
			get
			{
				return this.IsGenericTypeDefinition;
			}
		}

		/// <summary>Gets the position of a type parameter in the type parameter list of the generic type that declared the parameter.</summary>
		/// <returns>If the current <see cref="T:System.Reflection.Emit.TypeBuilder" /> object represents a generic type parameter, the position of the type parameter in the type parameter list of the generic type that declared the parameter; otherwise, undefined.</returns>
		[MonoTODO]
		public override int GenericParameterPosition
		{
			get
			{
				return 0;
			}
		}

		/// <summary>Gets the method that declared the current generic type parameter.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodBase" /> that represents the method that declared the current type, if the current type is a generic type parameter; otherwise, null.</returns>
		public override MethodBase DeclaringMethod
		{
			get
			{
				return null;
			}
		}

		/// <summary>Defines the generic type parameters for the current type, specifying their number and their names, and returns an array of <see cref="T:System.Reflection.Emit.GenericTypeParameterBuilder" /> objects that can be used to set their constraints.</summary>
		/// <returns>An array of <see cref="T:System.Reflection.Emit.GenericTypeParameterBuilder" /> objects that can be used to define the constraints of the generic type parameters for the current type.</returns>
		/// <param name="names">An array of names for the generic type parameters.</param>
		/// <exception cref="T:System.InvalidOperationException">Generic type parameters have already been defined for this type.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="names" /> is null.-or-An element of <paramref name="names" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="names" /> is an empty array.</exception>
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			if (names == null)
			{
				throw new ArgumentNullException("names");
			}
			if (names.Length == 0)
			{
				throw new ArgumentException("names");
			}
			this.setup_generic_class();
			this.generic_params = new GenericTypeParameterBuilder[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				string text = names[i];
				if (text == null)
				{
					throw new ArgumentNullException("names");
				}
				this.generic_params[i] = new GenericTypeParameterBuilder(this, null, text, i);
			}
			return this.generic_params;
		}

		/// <summary>Returns the constructor of the specified constructed generic type that corresponds to the specified constructor of the generic type definition. </summary>
		/// <returns>A <see cref="T:System.Reflection.ConstructorInfo" /> object that represents the constructor of <paramref name="type" /> corresponding to <paramref name="constructor" />, which specifies a constructor belonging to the generic type definition of <paramref name="type" />.</returns>
		/// <param name="type">The constructed generic type whose constructor is returned.</param>
		/// <param name="constructor">A constructor on the generic type definition of <paramref name="type" />, which specifies which constructor of <paramref name="type" /> to return.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not represent a generic type. -or-<paramref name="type" /> is not of type <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or-The declaring type of <paramref name="constructor" /> is not a generic type definition. -or-The declaring type of <paramref name="constructor" /> is not the generic type definition of <paramref name="type" />.</exception>
		public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
		{
			if (type == null)
			{
				throw new ArgumentException("Type is not generic", "type");
			}
			ConstructorInfo constructor2 = type.GetConstructor(constructor);
			if (constructor2 == null)
			{
				throw new ArgumentException("constructor not found");
			}
			return constructor2;
		}

		private static bool IsValidGetMethodType(Type type)
		{
			if (type is TypeBuilder || type is MonoGenericClass)
			{
				return true;
			}
			if (type.Module is ModuleBuilder)
			{
				return true;
			}
			if (type.IsGenericParameter)
			{
				return false;
			}
			Type[] genericArguments = type.GetGenericArguments();
			if (genericArguments == null)
			{
				return false;
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (TypeBuilder.IsValidGetMethodType(genericArguments[i]))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Returns the method of the specified constructed generic type that corresponds to the specified method of the generic type definition. </summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object that represents the method of <paramref name="type" /> corresponding to <paramref name="method" />, which specifies a method belonging to the generic type definition of <paramref name="type" />.</returns>
		/// <param name="type">The constructed generic type whose method is returned.</param>
		/// <param name="method">A method on the generic type definition of <paramref name="type" />, which specifies which method of <paramref name="type" /> to return.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is a generic method that is not a generic method definition.-or-<paramref name="type" /> does not represent a generic type.-or-<paramref name="type" /> is not of type <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or-The declaring type of <paramref name="method" /> is not a generic type definition. -or-The declaring type of <paramref name="method" /> is not the generic type definition of <paramref name="type" />.</exception>
		public static MethodInfo GetMethod(Type type, MethodInfo method)
		{
			if (!TypeBuilder.IsValidGetMethodType(type))
			{
				throw new ArgumentException("type is not TypeBuilder but " + type.GetType(), "type");
			}
			if (!type.IsGenericType)
			{
				throw new ArgumentException("type is not a generic type", "type");
			}
			if (!method.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException("method declaring type is not a generic type definition", "method");
			}
			if (method.DeclaringType != type.GetGenericTypeDefinition())
			{
				throw new ArgumentException("method declaring type is not the generic type definition of type", "method");
			}
			MethodInfo method2 = type.GetMethod(method);
			if (method2 == null)
			{
				throw new ArgumentException(string.Format("method {0} not found in type {1}", method.Name, type));
			}
			return method2;
		}

		/// <summary>Returns the field of the specified constructed generic type that corresponds to the specified field of the generic type definition. </summary>
		/// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object that represents the field of <paramref name="type" /> corresponding to <paramref name="field" />, which specifies a field belonging to the generic type definition of <paramref name="type" />.</returns>
		/// <param name="type">The constructed generic type whose field is returned.</param>
		/// <param name="field">A field on the generic type definition of <paramref name="type" />, which specifies which field of <paramref name="type" /> to return.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not represent a generic type. -or-<paramref name="type" /> is not of type <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or-The declaring type of <paramref name="field" /> is not a generic type definition. -or-The declaring type of <paramref name="field" /> is not the generic type definition of <paramref name="type" />.</exception>
		public static FieldInfo GetField(Type type, FieldInfo field)
		{
			FieldInfo field2 = type.GetField(field);
			if (field2 == null)
			{
				throw new Exception("field not found");
			}
			return field2;
		}
	}
}
