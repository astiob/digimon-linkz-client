using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Discovers the attributes of a property and provides access to property metadata.</summary>
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_PropertyInfo))]
	[Serializable]
	public abstract class PropertyInfo : MemberInfo, _PropertyInfo
	{
		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _PropertyInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _PropertyInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _PropertyInfo.GetTypeInfoCount(out uint pcTInfo)
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
		void _PropertyInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets the attributes for this property.</summary>
		/// <returns>Attributes of this property.</returns>
		public abstract PropertyAttributes Attributes { get; }

		/// <summary>Gets a value indicating whether the property can be read.</summary>
		/// <returns>true if this property can be read; otherwise, false.</returns>
		public abstract bool CanRead { get; }

		/// <summary>Gets a value indicating whether the property can be written to.</summary>
		/// <returns>true if this property can be written to; otherwise, false.</returns>
		public abstract bool CanWrite { get; }

		/// <summary>Gets a value indicating whether the property is the special name.</summary>
		/// <returns>true if this property is the special name; otherwise, false.</returns>
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & PropertyAttributes.SpecialName) != PropertyAttributes.None;
			}
		}

		/// <summary>Gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a property.</summary>
		/// <returns>A value indicating that this member is a property.</returns>
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		/// <summary>Gets the type of this property.</summary>
		/// <returns>The type of this property.</returns>
		public abstract Type PropertyType { get; }

		/// <summary>Returns an array whose elements reflect the public get, set, and other accessors of the property reflected by the current instance.</summary>
		/// <returns>An array that contains the public get, set, and other accessors of the property reflected by the current instance, if found; otherwise, this method returns an array with 0 (zero) elements.</returns>
		public MethodInfo[] GetAccessors()
		{
			return this.GetAccessors(false);
		}

		/// <summary>Returns an array whose elements reflect the public (and, if specified, non-public) get, set, and other accessors of the property reflected by the current instance.</summary>
		/// <returns>An array that contains the get, set, and other accessors of the property reflected by the current instance. If <paramref name="nonPublic" /> is true, this array contains public and non-public get, set, and other accessors. If <paramref name="nonPublic" /> is false, this array contains only public get, set, and other accessors. If no accessors with the specified visibility are found, this method returns an array with 0 (zero) elements.</returns>
		/// <param name="nonPublic">Indicates whether non-public methods should be returned in the MethodInfo array. true if non-public methods are to be included; otherwise, false. </param>
		public abstract MethodInfo[] GetAccessors(bool nonPublic);

		/// <summary>Returns the public get accessor for this property.</summary>
		/// <returns>The public get accessor for this property, if the get accessor exists and is public; otherwise, null.</returns>
		public MethodInfo GetGetMethod()
		{
			return this.GetGetMethod(false);
		}

		/// <summary>When overridden in a derived class, returns the public or non-public get accessor for this property.</summary>
		/// <returns>The get accessor for this property, if <paramref name="nonPublic" /> is true. Returns null if <paramref name="nonPublic" /> is false and the get accessor is non-public, or if <paramref name="nonPublic" /> is true but no get accessor exists.</returns>
		/// <param name="nonPublic">true to return a non-public accessor; otherwise, false. </param>
		/// <exception cref="T:System.Security.SecurityException">The requested method is non-public and the caller does not have <see cref="T:System.Security.Permissions.ReflectionPermission" /> to reflect on this non-public method. </exception>
		public abstract MethodInfo GetGetMethod(bool nonPublic);

		/// <summary>Gets an array of all the index parameters for the property.</summary>
		/// <returns>An array that contains the index parameters. If the property is not indexed, the array has 0 (zero) elements. </returns>
		public abstract ParameterInfo[] GetIndexParameters();

		/// <summary>Returns the public set accessor for this property.</summary>
		/// <returns>The Set method for this property, if the set accessor exists and is public; otherwise, null.</returns>
		public MethodInfo GetSetMethod()
		{
			return this.GetSetMethod(false);
		}

		/// <summary>When overridden in a derived class, returns the set accessor for this property.</summary>
		/// <returns>Value Condition A <see cref="T:System.Reflection.MethodInfo" /> object representing the Set method for this property. The set accessor is public.-or- <paramref name="nonPublic" /> is true and a set accessor exists. null<paramref name="nonPublic" /> is true, but the property is read-only.-or- <paramref name="nonPublic" /> is false and the set accessor is non-public.-or- There is no set accessor. </returns>
		/// <param name="nonPublic">true to return a non-public accessor; otherwise, false. </param>
		/// <exception cref="T:System.Security.SecurityException">The requested method is non-public and the caller does not have <see cref="T:System.Security.Permissions.ReflectionPermission" /> to reflect on this non-public method. </exception>
		public abstract MethodInfo GetSetMethod(bool nonPublic);

		/// <summary>Returns the value of the property with optional index values for indexed properties.</summary>
		/// <returns>The property value for the <paramref name="obj" /> parameter.</returns>
		/// <param name="obj">The object whose property value will be returned. </param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.-or- The property's get accessor is not found. </exception>
		/// <exception cref="T:System.Reflection.TargetException">The object does not match the target type.-or- The property is an instance property, but <paramref name="obj" /> is null. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes. </exception>
		/// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while retrieving the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Default, null, index, null);
		}

		/// <summary>When overridden in a derived class, returns the value of a property that has the specified binding, index, and <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>The property value for <paramref name="obj" />.</returns>
		/// <param name="obj">The object whose property value will be returned. </param>
		/// <param name="invokeAttr">The invocation attribute. This must be a bit flag from BindingFlags: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty. A suitable invocation attribute must be specified. If a static member is to be invoked, the Static flag of BindingFlags must be set. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects via reflection. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties. </param>
		/// <param name="culture">The culture for which the resource is to be localized. Note that if the resource is not localized for this culture, the CultureInfo.Parent method will be called successively in search of a match. If this value is null, the CultureInfo is obtained from the CultureInfo.CurrentUICulture property. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.-or- The property's get accessor is not found. </exception>
		/// <exception cref="T:System.Reflection.TargetException">The object does not match the target type.-or- The property is an instance property, but <paramref name="obj" /> is null. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes. </exception>
		/// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while retrieving the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
		public abstract object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		/// <summary>Sets the value of the property with optional index values for index properties.</summary>
		/// <param name="obj">The object whose property value will be set. </param>
		/// <param name="value">The new value for this property. </param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.-or- The property's set accessor is not found. </exception>
		/// <exception cref="T:System.Reflection.TargetException">The object does not match the target type.-or-The property is an instance property, but <paramref name="obj" /> is null. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes. </exception>
		/// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while setting the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual void SetValue(object obj, object value, object[] index)
		{
			this.SetValue(obj, value, BindingFlags.Default, null, index, null);
		}

		/// <summary>When overridden in a derived class, sets the property value for the given object to the given value.</summary>
		/// <param name="obj">The object whose property value will be set. </param>
		/// <param name="value">The new value for this property. </param>
		/// <param name="invokeAttr">The invocation attribute. This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" />: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty. A suitable invocation attribute must be specified. If a static member is to be invoked, the Static flag of BindingFlags must be set. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects through reflection. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties. </param>
		/// <param name="culture">The culture for which the resource is to be localized. Note that if the resource is not localized for this culture, the CultureInfo.Parent method will be called successively in search of a match. If this value is null, the CultureInfo is obtained from the CultureInfo.CurrentUICulture property. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.-or- The property's set accessor is not found. </exception>
		/// <exception cref="T:System.Reflection.TargetException">The object does not match the target type.-or-The property is an instance property, but <paramref name="obj" /> is null. </exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes. </exception>
		/// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while setting the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		/// <summary>Returns an array of types representing the optional custom modifiers of the property.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> objects that identify the optional custom modifiers of the current property, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
		public virtual Type[] GetOptionalCustomModifiers()
		{
			return Type.EmptyTypes;
		}

		/// <summary>Returns an array of types representing the required custom modifiers of the property.</summary>
		/// <returns>An array of objects that identify the required custom modifiers of the current property, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
		public virtual Type[] GetRequiredCustomModifiers()
		{
			return Type.EmptyTypes;
		}

		/// <summary>Returns a literal value associated with the property by a compiler. </summary>
		/// <returns>The literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
		/// <exception cref="T:System.InvalidOperationException">The Constant table in unmanaged metadata does not contain a constant value for the current property.</exception>
		/// <exception cref="T:System.FormatException">The type of the value is not one of the types permitted by the Common Language Specification (CLS). See the Standard ECMA-335 - Common Language Infrastructure (CLI) specification, Partition II. </exception>
		[MonoTODO("Not implemented")]
		public virtual object GetConstantValue()
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns a literal value associated with the property by a compiler. </summary>
		/// <returns>The literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
		/// <exception cref="T:System.InvalidOperationException">The Constant table in unmanaged metadata does not contain a constant value for the current property.</exception>
		/// <exception cref="T:System.FormatException">The type of the value is not one of the types permitted by the Common Language Specification (CLS). See the Standard ECMA-335 - Common Language Infrastructure (CLI) specification, Partition II.</exception>
		[MonoTODO("Not implemented")]
		public virtual object GetRawConstantValue()
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets a <see cref="T:System.Type" /> object representing the <see cref="T:System.Reflection.PropertyInfo" /> type. </summary>
		/// <returns>A <see cref="T:System.Type" /> object representing the <see cref="T:System.Reflection.PropertyInfo" /> type.</returns>
		virtual Type GetType()
		{
			return base.GetType();
		}
	}
}
