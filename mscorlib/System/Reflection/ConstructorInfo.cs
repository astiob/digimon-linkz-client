using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Discovers the attributes of a class constructor and provides access to constructor metadata. </summary>
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ConstructorInfo))]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public abstract class ConstructorInfo : MethodBase, _ConstructorInfo
	{
		/// <summary>Represents the name of the class constructor method as it is stored in metadata. This name is always ".ctor". This field is read-only.</summary>
		[ComVisible(true)]
		public static readonly string ConstructorName = ".ctor";

		/// <summary>Represents the name of the type constructor method as it is stored in metadata. This name is always ".cctor". This property is read-only.</summary>
		[ComVisible(true)]
		public static readonly string TypeConstructorName = ".cctor";

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ConstructorInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ConstructorInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ConstructorInfo.GetTypeInfoCount(out uint pcTInfo)
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
		void _ConstructorInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Runtime.InteropServices._ConstructorInfo.Invoke_2(System.Object,System.Reflection.BindingFlags,System.Reflection.Binder,System.Object[],System.Globalization.CultureInfo)" />.</summary>
		/// <returns>An instance of the type.</returns>
		/// <param name="obj">The instance that created this method.</param>
		/// <param name="invokeAttr">One of the <see cref="T:System.Reflection.BindingFlags" /> values that specifies the type of binding.</param>
		/// <param name="binder">A <see cref="T:System.Reflection.Binder" /> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If <paramref name="binder" /> is null, then <see cref="P:System.Type.DefaultBinder" /> is used.</param>
		/// <param name="parameters">An array of objects used to match the number, order, and type of the parameters for this constructor, under the constraints of <paramref name="binder" />. If this constructor does not require parameters, pass an array with zero elements. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> used to govern the coercion of types. If this is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		object _ConstructorInfo.Invoke_2(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(obj, invokeAttr, binder, parameters, culture);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Runtime.InteropServices._ConstructorInfo.Invoke_3(System.Object,System.Object[])" />.</summary>
		/// <returns>An instance of the type.</returns>
		/// <param name="obj">The instance that created this method.</param>
		/// <param name="parameters">An array of objects used to match the number, order, and type of the parameters for this constructor, under the constraints of <paramref name="binder" />. If this constructor does not require parameters, pass an array with zero elements. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type.</param>
		object _ConstructorInfo.Invoke_3(object obj, object[] parameters)
		{
			return base.Invoke(obj, parameters);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Runtime.InteropServices._ConstructorInfo.Invoke_4(System.Reflection.BindingFlags,System.Reflection.Binder,System.Object[],System.Globalization.CultureInfo)" />.</summary>
		/// <returns>An instance of the type.</returns>
		/// <param name="invokeAttr">One of the <see cref="T:System.Reflection.BindingFlags" /> values that specifies the type of binding.</param>
		/// <param name="binder">A <see cref="T:System.Reflection.Binder" /> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If <paramref name="binder" /> is null, then <see cref="P:System.Type.DefaultBinder" /> is used.</param>
		/// <param name="parameters">An array of objects used to match the number, order, and type of the parameters for this constructor, under the constraints of <paramref name="binder" />. If this constructor does not require parameters, pass an array with zero elements. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> used to govern the coercion of types. If this is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		object _ConstructorInfo.Invoke_4(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(invokeAttr, binder, parameters, culture);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Runtime.InteropServices._ConstructorInfo.Invoke_5(System.Object[])" />.</summary>
		/// <returns>An instance of the type.</returns>
		/// <param name="parameters">An array of objects used to match the number, order, and type of the parameters for this constructor, under the constraints of <paramref name="binder" />. If this constructor does not require parameters, pass an array with zero elements. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type.</param>
		object _ConstructorInfo.Invoke_5(object[] parameters)
		{
			return this.Invoke(parameters);
		}

		/// <summary>Gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a constructor.</summary>
		/// <returns>A <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a constructor.</returns>
		[ComVisible(true)]
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Constructor;
			}
		}

		/// <summary>Invokes the constructor reflected by the instance that has the specified parameters, providing default values for the parameters not commonly used.</summary>
		/// <returns>An instance of the class associated with the constructor.</returns>
		/// <param name="parameters">An array of values that matches the number, order and type (under the constraints of the default binder) of the parameters for this constructor. If this constructor takes no parameters, then use either an array with zero elements or null, as in Object[] parameters = new Object[0]. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type. </param>
		/// <exception cref="T:System.MemberAccessException">The class is abstract.-or- The constructor is a class initializer. </exception>
		/// <exception cref="T:System.MethodAccessException">The constructor is private or protected, and the caller lacks <see cref="F:System.Security.Permissions.ReflectionPermissionFlag.MemberAccess" />. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="parameters" /> array does not contain values that match the types accepted by this constructor. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The invoked constructor throws an exception. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">An incorrect number of parameters was passed. </exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, and <see cref="T:System.RuntimeArgumentHandle" /> types is not supported.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the necessary code access permission.</exception>
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object Invoke(object[] parameters)
		{
			if (parameters == null)
			{
				parameters = new object[0];
			}
			return this.Invoke(BindingFlags.CreateInstance, null, parameters, null);
		}

		/// <summary>When implemented in a derived class, invokes the constructor reflected by this ConstructorInfo with the specified arguments, under the constraints of the specified Binder.</summary>
		/// <returns>An instance of the class associated with the constructor.</returns>
		/// <param name="invokeAttr">One of the BindingFlags values that specifies the type of binding. </param>
		/// <param name="binder">A Binder that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If <paramref name="binder" /> is null, then Binder.DefaultBinding is used. </param>
		/// <param name="parameters">An array of type Object used to match the number, order and type of the parameters for this constructor, under the constraints of <paramref name="binder" />. If this constructor does not require parameters, pass an array with zero elements, as in Object[] parameters = new Object[0]. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> used to govern the coercion of types. If this is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="parameters" /> array does not contain values that match the types accepted by this constructor, under the constraints of the <paramref name="binder" />. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The invoked constructor throws an exception. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">An incorrect number of parameters was passed. </exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, and <see cref="T:System.RuntimeArgumentHandle" /> types is not supported.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the necessary code access permissions.</exception>
		/// <exception cref="T:System.MemberAccessException">The class is abstract.-or- The constructor is a class initializer. </exception>
		/// <exception cref="T:System.MethodAccessException">The constructor is private or protected, and the caller lacks <see cref="F:System.Security.Permissions.ReflectionPermissionFlag.MemberAccess" />. </exception>
		public abstract object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		/// <summary>Gets a <see cref="T:System.Type" /> object representing the <see cref="T:System.Reflection.ConstructorInfo" /> type.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing the <see cref="T:System.Reflection.ConstructorInfo" /> type.</returns>
		virtual Type GetType()
		{
			return base.GetType();
		}
	}
}
