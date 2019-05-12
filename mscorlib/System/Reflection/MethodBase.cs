using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Provides information about methods and constructors. </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_MethodBase))]
	[Serializable]
	public abstract class MethodBase : MemberInfo, _MethodBase
	{
		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _MethodBase.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _MethodBase.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _MethodBase.GetTypeInfoCount(out uint pcTInfo)
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
		void _MethodBase.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns a MethodBase object representing the currently executing method.</summary>
		/// <returns>A MethodBase object representing the currently executing method.</returns>
		/// <exception cref="T:System.Reflection.TargetException">This member was invoked with a late-binding mechanism.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MethodBase GetCurrentMethod();

		internal static MethodBase GetMethodFromHandleNoGenericCheck(RuntimeMethodHandle handle)
		{
			return MethodBase.GetMethodFromIntPtr(handle.Value, IntPtr.Zero);
		}

		private static MethodBase GetMethodFromIntPtr(IntPtr handle, IntPtr declaringType)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			MethodBase methodFromHandleInternalType = MethodBase.GetMethodFromHandleInternalType(handle, declaringType);
			if (methodFromHandleInternalType == null)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			return methodFromHandleInternalType;
		}

		/// <summary>Gets method information by using the method's internal metadata representation (handle).</summary>
		/// <returns>A MethodBase containing information about the method.</returns>
		/// <param name="handle">The method's handle. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="handle" /> is invalid.</exception>
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle)
		{
			MethodBase methodFromIntPtr = MethodBase.GetMethodFromIntPtr(handle.Value, IntPtr.Zero);
			Type declaringType = methodFromIntPtr.DeclaringType;
			if (declaringType.IsGenericType || declaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Cannot resolve method because it's declared in a generic class.");
			}
			return methodFromIntPtr;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MethodBase GetMethodFromHandleInternalType(IntPtr method_handle, IntPtr type_handle);

		/// <summary>Gets a <see cref="T:System.Reflection.MethodBase" /> object for the constructor or method represented by the specified handle, for the specified generic type.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodBase" /> object representing the method or constructor specified by <paramref name="handle" />, in the generic type specified by <paramref name="declaringType" />.</returns>
		/// <param name="handle">A handle to the internal metadata representation of a constructor or method.</param>
		/// <param name="declaringType">A handle to the generic type that defines the constructor or method.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="handle" /> is invalid.</exception>
		[ComVisible(false)]
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType)
		{
			return MethodBase.GetMethodFromIntPtr(handle.Value, declaringType.Value);
		}

		/// <summary>When overridden in a derived class, returns the <see cref="T:System.Reflection.MethodImplAttributes" /> flags.</summary>
		/// <returns>The MethodImplAttributes flags.</returns>
		public abstract MethodImplAttributes GetMethodImplementationFlags();

		/// <summary>When overridden in a derived class, gets the parameters of the specified method or constructor.</summary>
		/// <returns>An array of type ParameterInfo containing information that matches the signature of the method (or constructor) reflected by this MethodBase instance.</returns>
		public abstract ParameterInfo[] GetParameters();

		internal virtual int GetParameterCount()
		{
			ParameterInfo[] parameters = this.GetParameters();
			if (parameters == null)
			{
				return 0;
			}
			return parameters.Length;
		}

		/// <summary>Invokes the method or constructor represented by the current instance, using the specified parameters.</summary>
		/// <returns>An object containing the return value of the invoked method, or null in the case of a constructor.</returns>
		/// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor. </param>
		/// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, <paramref name="parameters" /> should be null.If the method or constructor represented by this instance takes a ref parameter (ByRef in Visual Basic), no special attribute is required for that parameter in order to invoke the method or constructor using this function. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type. </param>
		/// <exception cref="T:System.Reflection.TargetException">The <paramref name="obj" /> parameter is null and the method is not static.-or- The method is not declared or inherited by the class of <paramref name="obj" />. -or-A static constructor is invoked, and <paramref name="obj" /> is neither null nor an instance of the class that declared the constructor.</exception>
		/// <exception cref="T:System.ArgumentException">The elements of the <paramref name="parameters" /> array do not match the signature of the method or constructor reflected by this instance. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The invoked method or constructor throws an exception. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">The <paramref name="parameters" /> array does not have the correct number of arguments. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to execute the constructor. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type that declares the method is an open generic type. That is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true for the declaring type.</exception>
		/// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.Reflection.MethodBase" /> is a <see cref="T:System.Reflection.Emit.MethodBuilder" />. </exception>
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object Invoke(object obj, object[] parameters)
		{
			return this.Invoke(obj, BindingFlags.Default, null, parameters, null);
		}

		/// <summary>When overridden in a derived class, invokes the reflected method or constructor with the given parameters.</summary>
		/// <returns>An Object containing the return value of the invoked method, or null in the case of a constructor, or null if the method's return type is void. Before calling the method or constructor, Invoke checks to see if the user has access permission and verify that the parameters are valid.</returns>
		/// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.</param>
		/// <param name="invokeAttr">A bitmask that is a combination of 0 or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. If <paramref name="binder" /> is null, this parameter is assigned the value <see cref="F:System.Reflection.BindingFlags.Default" />; thus, whatever you pass in is ignored. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects via reflection. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, this should be null.If the method or constructor represented by this instance takes a ByRef parameter, there is no special attribute required for that parameter in order to invoke the method or constructor using this function. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type. </param>
		/// <param name="culture">An instance of CultureInfo used to govern the coercion of types. If this is null, the CultureInfo for the current thread is used. (This is necessary to convert a String that represents 1000 to a Double value, for example, since 1000 is represented differently by different cultures.) </param>
		/// <exception cref="T:System.Reflection.TargetException">The <paramref name="obj" /> parameter is null and the method is not static.-or- The method is not declared or inherited by the class of <paramref name="obj" />. -or-A static constructor is invoked, and <paramref name="obj" /> is neither null nor an instance of the class that declared the constructor.</exception>
		/// <exception cref="T:System.ArgumentException">The type of the <paramref name="parameters" /> parameter does not match the signature of the method or constructor reflected by this instance. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">The <paramref name="parameters" /> array does not have the correct number of arguments. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The invoked method or constructor throws an exception. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to execute the constructor. </exception>
		/// <exception cref="T:System.InvalidOperationException">The type that declares the method is an open generic type. That is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true for the declaring type.</exception>
		public abstract object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		/// <summary>Gets a handle to the internal metadata representation of a method.</summary>
		/// <returns>A <see cref="T:System.RuntimeMethodHandle" /> object.</returns>
		public abstract RuntimeMethodHandle MethodHandle { get; }

		/// <summary>Gets the attributes associated with this method.</summary>
		/// <returns>One of the <see cref="T:System.Reflection.MethodAttributes" /> values.</returns>
		public abstract MethodAttributes Attributes { get; }

		/// <summary>Gets a value indicating the calling conventions for this method.</summary>
		/// <returns>The <see cref="T:System.Reflection.CallingConventions" /> for this method.</returns>
		public virtual CallingConventions CallingConvention
		{
			get
			{
				return CallingConventions.Standard;
			}
		}

		/// <summary>Gets a value indicating whether this is a public method.</summary>
		/// <returns>true if this method is public; otherwise, false.</returns>
		public bool IsPublic
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
			}
		}

		/// <summary>Gets a value indicating whether this member is private.</summary>
		/// <returns>true if access to this method is restricted to other members of the class itself; otherwise, false.</returns>
		public bool IsPrivate
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
			}
		}

		/// <summary>Gets a value indicating whether the visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.Family" />; that is, the method or constructor is visible only within its class and derived classes.</summary>
		/// <returns>true if access to this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.Family" />; otherwise, false.</returns>
		public bool IsFamily
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;
			}
		}

		/// <summary>Gets a value indicating whether the potential visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.Assembly" />; that is, the method or constructor is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.</summary>
		/// <returns>true if the visibility of this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.Assembly" />; otherwise, false.</returns>
		public bool IsAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;
			}
		}

		/// <summary>Gets a value indicating whether the visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.FamANDAssem" />; that is, the method or constructor can be called by derived classes, but only if they are in the same assembly.</summary>
		/// <returns>true if access to this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.FamANDAssem" />; otherwise, false.</returns>
		public bool IsFamilyAndAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;
			}
		}

		/// <summary>Gets a value indicating whether the potential visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.FamORAssem" />; that is, the method or constructor can be called by derived classes wherever they are, and by classes in the same assembly.</summary>
		/// <returns>true if access to this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.FamORAssem" />; otherwise, false.</returns>
		public bool IsFamilyOrAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;
			}
		}

		/// <summary>Gets a value indicating whether the method is static.</summary>
		/// <returns>true if this method is static; otherwise, false.</returns>
		public bool IsStatic
		{
			get
			{
				return (this.Attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
			}
		}

		/// <summary>Gets a value indicating whether this method is final.</summary>
		/// <returns>true if this method is final; otherwise, false.</returns>
		public bool IsFinal
		{
			get
			{
				return (this.Attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope;
			}
		}

		/// <summary>Gets a value indicating whether the method is virtual.</summary>
		/// <returns>true if this method is virtual; otherwise, false.</returns>
		public bool IsVirtual
		{
			get
			{
				return (this.Attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope;
			}
		}

		/// <summary>Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class.</summary>
		/// <returns>true if the member is hidden by signature; otherwise, false.</returns>
		public bool IsHideBySig
		{
			get
			{
				return (this.Attributes & MethodAttributes.HideBySig) != MethodAttributes.PrivateScope;
			}
		}

		/// <summary>Gets a value indicating whether the method is abstract.</summary>
		/// <returns>true if the method is abstract; otherwise, false.</returns>
		public bool IsAbstract
		{
			get
			{
				return (this.Attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope;
			}
		}

		/// <summary>Gets a value indicating whether this method has a special name.</summary>
		/// <returns>true if this method has a special name; otherwise, false.</returns>
		public bool IsSpecialName
		{
			get
			{
				int attributes = (int)this.Attributes;
				return (attributes & 2048) != 0;
			}
		}

		/// <summary>Gets a value indicating whether the method is a constructor.</summary>
		/// <returns>true if this method is a constructor represented by a <see cref="T:System.Reflection.ConstructorInfo" /> object (see note in Remarks about <see cref="T:System.Reflection.Emit.ConstructorBuilder" /> objects); otherwise, false.</returns>
		[ComVisible(true)]
		public bool IsConstructor
		{
			get
			{
				int attributes = (int)this.Attributes;
				return (attributes & 4096) != 0 && this.Name == ".ctor";
			}
		}

		internal virtual int get_next_table_index(object obj, int table, bool inc)
		{
			if (this is MethodBuilder)
			{
				MethodBuilder methodBuilder = (MethodBuilder)this;
				return methodBuilder.get_next_table_index(obj, table, inc);
			}
			if (this is ConstructorBuilder)
			{
				ConstructorBuilder constructorBuilder = (ConstructorBuilder)this;
				return constructorBuilder.get_next_table_index(obj, table, inc);
			}
			throw new Exception("Method is not a builder method");
		}

		/// <summary>Returns an array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
		/// <exception cref="T:System.NotSupportedException">The current object is a <see cref="T:System.Reflection.ConstructorInfo" />. Generic constructors are not supported in the .NET Framework version 2.0. This exception is the default behavior if this method is not overridden in a derived class.</exception>
		[ComVisible(true)]
		public virtual Type[] GetGenericArguments()
		{
			throw new NotSupportedException();
		}

		/// <summary>Gets a value indicating whether the generic method contains unassigned generic type parameters.</summary>
		/// <returns>true if the current <see cref="T:System.Reflection.MethodBase" /> object represents a generic method that contains unassigned generic type parameters; otherwise, false.</returns>
		public virtual bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the method is a generic method definition.</summary>
		/// <returns>true if the current <see cref="T:System.Reflection.MethodBase" /> object represents the definition of a generic method; otherwise, false.</returns>
		public virtual bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the method is generic.</summary>
		/// <returns>true if the current <see cref="T:System.Reflection.MethodBase" /> represents a generic method; otherwise, false.</returns>
		public virtual bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MethodBody GetMethodBodyInternal(IntPtr handle);

		internal static MethodBody GetMethodBody(IntPtr handle)
		{
			return MethodBase.GetMethodBodyInternal(handle);
		}

		/// <summary>When overridden in a derived class, gets a <see cref="T:System.Reflection.MethodBody" /> object that provides access to the MSIL stream, local variables, and exceptions for the current method.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodBody" /> object that provides access to the MSIL stream, local variables, and exceptions for the current method.</returns>
		/// <exception cref="T:System.InvalidOperationException">This method is invalid unless overridden in a derived class.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public virtual MethodBody GetMethodBody()
		{
			throw new NotSupportedException();
		}

		/// <summary>Provides COM objects with version-independent access to the <see cref="M:System.Runtime.InteropServices._MethodBase.GetType" /> method.</summary>
		/// <returns>See <see cref="M:System.Runtime.InteropServices._MethodBase.GetType" />.</returns>
		virtual Type GetType()
		{
			return base.GetType();
		}
	}
}
