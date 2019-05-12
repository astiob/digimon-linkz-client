using System;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Reflection.Emit
{
	/// <summary>Defines and represents a constructor of a dynamic class.</summary>
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ConstructorBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class ConstructorBuilder : ConstructorInfo, _ConstructorBuilder
	{
		private RuntimeMethodHandle mhandle;

		private ILGenerator ilgen;

		internal Type[] parameters;

		private MethodAttributes attrs;

		private MethodImplAttributes iattrs;

		private int table_idx;

		private CallingConventions call_conv;

		private TypeBuilder type;

		internal ParameterBuilder[] pinfo;

		private CustomAttributeBuilder[] cattrs;

		private bool init_locals = true;

		private Type[][] paramModReq;

		private Type[][] paramModOpt;

		private RefEmitPermissionSet[] permissions;

		internal ConstructorBuilder(TypeBuilder tb, MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt)
		{
			this.attrs = (attributes | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
			this.call_conv = callingConvention;
			if (parameterTypes != null)
			{
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					if (parameterTypes[i] == null)
					{
						throw new ArgumentException("Elements of the parameterTypes array cannot be null", "parameterTypes");
					}
				}
				this.parameters = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.parameters, parameterTypes.Length);
			}
			this.type = tb;
			this.paramModReq = paramModReq;
			this.paramModOpt = paramModOpt;
			this.table_idx = this.get_next_table_index(this, 6, true);
			((ModuleBuilder)tb.Module).RegisterToken(this, this.GetToken().Token);
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ConstructorBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ConstructorBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ConstructorBuilder.GetTypeInfoCount(out uint pcTInfo)
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
		void _ConstructorBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets a <see cref="T:System.Reflection.CallingConventions" /> value that depends on whether the declaring type is generic.</summary>
		/// <returns>
		///   <see cref="F:System.Reflection.CallingConventions.HasThis" /> if the declaring type is generic; otherwise, <see cref="F:System.Reflection.CallingConventions.Standard" />. </returns>
		[MonoTODO]
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.call_conv;
			}
		}

		/// <summary>Gets or sets whether the local variables in this constructor should be zero-initialized.</summary>
		/// <returns>Read/write. Gets or sets whether the local variables in this constructor should be zero-initialized.</returns>
		public bool InitLocals
		{
			get
			{
				return this.init_locals;
			}
			set
			{
				this.init_locals = value;
			}
		}

		internal TypeBuilder TypeBuilder
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>Returns the method implementation flags for this constructor.</summary>
		/// <returns>The method implementation flags for this constructor.</returns>
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.iattrs;
		}

		/// <summary>Returns the parameters of this constructor.</summary>
		/// <returns>Returns an array of <see cref="T:System.Reflection.ParameterInfo" /> objects that represent the parameters of this constructor.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> has not been called on this constructor's type, in the .NET Framework versions 1.0 and 1.1. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> has not been called on this constructor's type, in the .NET Framework version 2.0. </exception>
		public override ParameterInfo[] GetParameters()
		{
			if (!this.type.is_created && !this.IsCompilerContext)
			{
				throw this.not_created();
			}
			return this.GetParametersInternal();
		}

		internal ParameterInfo[] GetParametersInternal()
		{
			if (this.parameters == null)
			{
				return new ParameterInfo[0];
			}
			ParameterInfo[] array = new ParameterInfo[this.parameters.Length];
			for (int i = 0; i < this.parameters.Length; i++)
			{
				array[i] = new ParameterInfo((this.pinfo != null) ? this.pinfo[i + 1] : null, this.parameters[i], this, i + 1);
			}
			return array;
		}

		internal override int GetParameterCount()
		{
			if (this.parameters == null)
			{
				return 0;
			}
			return this.parameters.Length;
		}

		/// <summary>Dynamically invokes the constructor reflected by this instance with the specified arguments, under the constraints of the specified Binder.</summary>
		/// <returns>An instance of the class associated with the constructor.</returns>
		/// <param name="obj">The object that needs to be reinitialized. </param>
		/// <param name="invokeAttr">One of the BindingFlags values that specifies the type of binding that is desired. </param>
		/// <param name="binder">A Binder that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If <paramref name="binder" /> is null, then Binder.DefaultBinding is used. </param>
		/// <param name="parameters">An argument list. This is an array of arguments with the same number, order, and type as the parameters of the constructor to be invoked. If there are no parameters, this should be a null reference (Nothing in Visual Basic). </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> used to govern the coercion of types. If this is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. You can retrieve the constructor using <see cref="M:System.Type.GetConstructor(System.Reflection.BindingFlags,System.Reflection.Binder,System.Reflection.CallingConventions,System.Type[],System.Reflection.ParameterModifier[])" /> and call <see cref="M:System.Reflection.ConstructorInfo.Invoke(System.Reflection.BindingFlags,System.Reflection.Binder,System.Object[],System.Globalization.CultureInfo)" /> on the returned <see cref="T:System.Reflection.ConstructorInfo" />. </exception>
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw this.not_supported();
		}

		/// <summary>Invokes the constructor dynamically reflected by this instance on the given object, passing along the specified parameters, and under the constraints of the given binder.</summary>
		/// <returns>Returns an <see cref="T:System.Object" /> that is the return value of the invoked constructor.</returns>
		/// <param name="invokeAttr">This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" />, such as InvokeMethod, NonPublic, and so on. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects using reflection. If binder is null, the default binder is used. See <see cref="T:System.Reflection.Binder" />. </param>
		/// <param name="parameters">An argument list. This is an array of arguments with the same number, order, and type as the parameters of the constructor to be invoked. If there are no parameters this should be null. </param>
		/// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> used to govern the coercion of types. If this is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. (For example, this is necessary to convert a <see cref="T:System.String" /> that represents 1000 to a <see cref="T:System.Double" /> value, since 1000 is represented differently by different cultures.) </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. You can retrieve the constructor using <see cref="M:System.Type.GetConstructor(System.Reflection.BindingFlags,System.Reflection.Binder,System.Reflection.CallingConventions,System.Type[],System.Reflection.ParameterModifier[])" /> and call <see cref="M:System.Reflection.ConstructorInfo.Invoke(System.Reflection.BindingFlags,System.Reflection.Binder,System.Object[],System.Globalization.CultureInfo)" /> on the returned <see cref="T:System.Reflection.ConstructorInfo" />. </exception>
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw this.not_supported();
		}

		/// <summary>Retrieves the internal handle for the method. Use this handle to access the underlying metadata handle.</summary>
		/// <returns>Returns the internal handle for the method. Use this handle to access the underlying metadata handle.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported on this class. </exception>
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw this.not_supported();
			}
		}

		/// <summary>Retrieves the attributes for this constructor.</summary>
		/// <returns>Returns the attributes for this constructor.</returns>
		public override MethodAttributes Attributes
		{
			get
			{
				return this.attrs;
			}
		}

		/// <summary>Holds a reference to the <see cref="T:System.Type" /> object from which this object was obtained.</summary>
		/// <returns>Returns the Type object from which this object was obtained.</returns>
		public override Type ReflectedType
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>Retrieves a reference to the <see cref="T:System.Type" /> object for the type that declares this member.</summary>
		/// <returns>Returns the <see cref="T:System.Type" /> object for the type that declares this member.</returns>
		public override Type DeclaringType
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>Gets null.</summary>
		/// <returns>Returns null.</returns>
		public Type ReturnType
		{
			get
			{
				return null;
			}
		}

		/// <summary>Retrieves the name of this constructor.</summary>
		/// <returns>Returns the name of this constructor.</returns>
		public override string Name
		{
			get
			{
				return ((this.attrs & MethodAttributes.Static) == MethodAttributes.PrivateScope) ? ConstructorInfo.ConstructorName : ConstructorInfo.TypeConstructorName;
			}
		}

		/// <summary>Retrieves the signature of the field in the form of a string.</summary>
		/// <returns>Returns the signature of the field.</returns>
		public string Signature
		{
			get
			{
				return "constructor signature";
			}
		}

		/// <summary>Adds declarative security to this constructor.</summary>
		/// <param name="action">The security action to be taken, such as Demand, Assert, and so on. </param>
		/// <param name="pset">The set of permissions the action applies to. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="action" /> is invalid (RequestMinimum, RequestOptional, and RequestRefuse are invalid). </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been previously created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or- The permission set <paramref name="pset" /> contains an action that was added earlier by AddDeclarativeSecurity. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="pset" /> is null. </exception>
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
		}

		/// <summary>Defines a parameter of this constructor.</summary>
		/// <returns>Returns a ParameterBuilder object that represents the new parameter of this constructor.</returns>
		/// <param name="iSequence">The position of the parameter in the parameter list. Parameters are indexed beginning with the number 1 for the first parameter. </param>
		/// <param name="attributes">The attributes of the parameter. </param>
		/// <param name="strParamName">The name of the parameter. The name can be the null string. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="position" /> is less than or equal to zero, or it is greater than the number of parameters of the constructor. </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public ParameterBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string strParamName)
		{
			if (iSequence < 1 || iSequence > this.GetParameterCount())
			{
				throw new ArgumentOutOfRangeException("iSequence");
			}
			if (this.type.is_created)
			{
				throw this.not_after_created();
			}
			ParameterBuilder parameterBuilder = new ParameterBuilder(this, iSequence, attributes, strParamName);
			if (this.pinfo == null)
			{
				this.pinfo = new ParameterBuilder[this.parameters.Length + 1];
			}
			this.pinfo[iSequence] = parameterBuilder;
			return parameterBuilder;
		}

		/// <summary>Checks if the specified custom attribute type is defined.</summary>
		/// <returns>true if the specified custom attribute type is defined; otherwise, false.</returns>
		/// <param name="attributeType">A custom attribute type. </param>
		/// <param name="inherit">Controls inheritance of custom attributes from base classes. This parameter is ignored. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. You can retrieve the constructor using <see cref="M:System.Type.GetConstructor(System.Reflection.BindingFlags,System.Reflection.Binder,System.Reflection.CallingConventions,System.Type[],System.Reflection.ParameterModifier[])" /> and call <see cref="M:System.Reflection.MemberInfo.IsDefined(System.Type,System.Boolean)" /> on the returned <see cref="T:System.Reflection.ConstructorInfo" />. </exception>
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw this.not_supported();
		}

		/// <summary>Returns all the custom attributes defined for this constructor.</summary>
		/// <returns>Returns an array of objects representing all the custom attributes of the constructor represented by this <see cref="T:System.Reflection.Emit.ConstructorBuilder" /> instance.</returns>
		/// <param name="inherit">Controls inheritance of custom attributes from base classes. This parameter is ignored. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.type.is_created && this.IsCompilerContext)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, inherit);
			}
			throw this.not_supported();
		}

		/// <summary>Returns the custom attributes identified by the given type.</summary>
		/// <returns>Returns an array of type <see cref="T:System.Object" /> representing the attributes of this constructor.</returns>
		/// <param name="attributeType">The custom attribute type. </param>
		/// <param name="inherit">Controls inheritance of custom attributes from base classes. This parameter is ignored. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.type.is_created && this.IsCompilerContext)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
			}
			throw this.not_supported();
		}

		/// <summary>Gets an <see cref="T:System.Reflection.Emit.ILGenerator" /> for this constructor.</summary>
		/// <returns>Returns an <see cref="T:System.Reflection.Emit.ILGenerator" /> object for this constructor.</returns>
		/// <exception cref="T:System.InvalidOperationException">The constructor is a default constructor.-or-The constructor has <see cref="T:System.Reflection.MethodAttributes" /> or <see cref="T:System.Reflection.MethodImplAttributes" /> flags indicating that it should not have a method body.</exception>
		public ILGenerator GetILGenerator()
		{
			return this.GetILGenerator(64);
		}

		/// <summary>Gets an <see cref="T:System.Reflection.Emit.ILGenerator" /> object, with the specified MSIL stream size, that can be used to build a method body for this constructor.</summary>
		/// <returns>An <see cref="T:System.Reflection.Emit.ILGenerator" /> for this constructor.</returns>
		/// <param name="streamSize">The size of the MSIL stream, in bytes.</param>
		/// <exception cref="T:System.InvalidOperationException">The constructor is a default constructor.-or-The constructor has <see cref="T:System.Reflection.MethodAttributes" /> or <see cref="T:System.Reflection.MethodImplAttributes" /> flags indicating that it should not have a method body. </exception>
		public ILGenerator GetILGenerator(int streamSize)
		{
			if (this.ilgen != null)
			{
				return this.ilgen;
			}
			this.ilgen = new ILGenerator(this.type.Module, ((ModuleBuilder)this.type.Module).GetTokenGenerator(), streamSize);
			return this.ilgen;
		}

		/// <summary>Set a custom attribute using a custom attribute builder.</summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="customBuilder" /> is null. </exception>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			string fullName = customBuilder.Ctor.ReflectedType.FullName;
			if (fullName == "System.Runtime.CompilerServices.MethodImplAttribute")
			{
				byte[] data = customBuilder.Data;
				int num = (int)data[2];
				num |= (int)data[3] << 8;
				this.SetImplementationFlags((MethodImplAttributes)num);
				return;
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

		/// <summary>Set a custom attribute using a specified custom attribute blob.</summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="con" /> or <paramref name="binaryAttribute" /> is null. </exception>
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		/// <summary>Sets the method implementation flags for this constructor.</summary>
		/// <param name="attributes">The method implementation flags. </param>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		public void SetImplementationFlags(MethodImplAttributes attributes)
		{
			if (this.type.is_created)
			{
				throw this.not_after_created();
			}
			this.iattrs = attributes;
		}

		/// <summary>Returns a reference to the module that contains this constructor.</summary>
		/// <returns>The module that contains this constructor.</returns>
		public Module GetModule()
		{
			return this.type.Module;
		}

		/// <summary>Returns the <see cref="T:System.Reflection.Emit.MethodToken" /> that represents the token for this constructor.</summary>
		/// <returns>Returns the <see cref="T:System.Reflection.Emit.MethodToken" /> of this constructor.</returns>
		public MethodToken GetToken()
		{
			return new MethodToken(100663296 | this.table_idx);
		}

		/// <summary>Sets this constructor's custom attribute associated with symbolic information.</summary>
		/// <param name="name">The name of the custom attribute. </param>
		/// <param name="data">The value of the custom attribute. </param>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />.-or- The module does not have a symbol writer defined. For example, the module is not a debug module. </exception>
		[MonoTODO]
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			if (this.type.is_created)
			{
				throw this.not_after_created();
			}
		}

		/// <summary>Gets the dynamic module in which this constructor is defined.</summary>
		/// <returns>A <see cref="T:System.Reflection.Module" /> object that represents the dynamic module in which this constructor is defined.</returns>
		public override Module Module
		{
			get
			{
				return base.Module;
			}
		}

		/// <summary>Returns this <see cref="T:System.Reflection.Emit.ConstructorBuilder" /> instance as a <see cref="T:System.String" />.</summary>
		/// <returns>Returns a <see cref="T:System.String" /> containing the name, attributes, and exceptions of this constructor, followed by the current Microsoft intermediate language (MSIL) stream.</returns>
		public override string ToString()
		{
			return "ConstructorBuilder ['" + this.type.Name + "']";
		}

		internal void fixup()
		{
			if ((this.attrs & (MethodAttributes.Abstract | MethodAttributes.PinvokeImpl)) == MethodAttributes.PrivateScope && (this.iattrs & (MethodImplAttributes)4099) == MethodImplAttributes.IL && (this.ilgen == null || ILGenerator.Mono_GetCurrentOffset(this.ilgen) == 0))
			{
				throw new InvalidOperationException("Method '" + this.Name + "' does not have a method body.");
			}
			if (this.ilgen != null)
			{
				this.ilgen.label_fixup();
			}
		}

		internal void GenerateDebugInfo(ISymbolWriter symbolWriter)
		{
			if (this.ilgen != null && this.ilgen.HasDebugInfo)
			{
				SymbolToken symbolToken = new SymbolToken(this.GetToken().Token);
				symbolWriter.OpenMethod(symbolToken);
				symbolWriter.SetSymAttribute(symbolToken, "__name", Encoding.UTF8.GetBytes(this.Name));
				this.ilgen.GenerateDebugInfo(symbolWriter);
				symbolWriter.CloseMethod();
			}
		}

		internal override int get_next_table_index(object obj, int table, bool inc)
		{
			return this.type.get_next_table_index(obj, table, inc);
		}

		private bool IsCompilerContext
		{
			get
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.TypeBuilder.Module;
				AssemblyBuilder assemblyBuilder = (AssemblyBuilder)moduleBuilder.Assembly;
				return assemblyBuilder.IsCompilerContext;
			}
		}

		private void RejectIfCreated()
		{
			if (this.type.is_created)
			{
				throw new InvalidOperationException("Type definition of the method is complete.");
			}
		}

		private Exception not_supported()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		private Exception not_after_created()
		{
			return new InvalidOperationException("Unable to change after type has been created.");
		}

		private Exception not_created()
		{
			return new NotSupportedException("The type is not yet created.");
		}
	}
}
