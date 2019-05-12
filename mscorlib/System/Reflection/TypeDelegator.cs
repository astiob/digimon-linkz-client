using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Wraps a Type object and delegates all methods to that Type.</summary>
	[ComVisible(true)]
	[Serializable]
	public class TypeDelegator : Type
	{
		/// <summary>A value indicating type information.</summary>
		protected Type typeImpl;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TypeDelegator" /> class with default properties.</summary>
		protected TypeDelegator()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TypeDelegator" /> class specifying the encapsulating instance.</summary>
		/// <param name="delegatingType">The instance of the class <see cref="T:System.Type" /> that encapsulates the call to the method of an object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="delegatingType" /> is null. </exception>
		public TypeDelegator(Type delegatingType)
		{
			if (delegatingType == null)
			{
				throw new ArgumentNullException("delegatingType must be non-null");
			}
			this.typeImpl = delegatingType;
		}

		/// <summary>Gets the assembly of the implemented type.</summary>
		/// <returns>An <see cref="T:System.Reflection.Assembly" /> object representing the assembly of the implemented type.</returns>
		public override Assembly Assembly
		{
			get
			{
				return this.typeImpl.Assembly;
			}
		}

		/// <summary>Gets the assembly's fully qualified name.</summary>
		/// <returns>A String containing the assembly's fully qualified name.</returns>
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.typeImpl.AssemblyQualifiedName;
			}
		}

		/// <summary>Gets the base type for the current type.</summary>
		/// <returns>The base type for a type.</returns>
		public override Type BaseType
		{
			get
			{
				return this.typeImpl.BaseType;
			}
		}

		/// <summary>Gets the fully qualified name of the implemented type.</summary>
		/// <returns>A String containing the type's fully qualified name.</returns>
		public override string FullName
		{
			get
			{
				return this.typeImpl.FullName;
			}
		}

		/// <summary>Gets the GUID (globally unique identifier) of the implemented type.</summary>
		/// <returns>A GUID.</returns>
		public override Guid GUID
		{
			get
			{
				return this.typeImpl.GUID;
			}
		}

		/// <summary>Gets the module that contains the implemented type.</summary>
		/// <returns>A <see cref="T:System.Reflection.Module" /> object representing the module of the implemented type.</returns>
		public override Module Module
		{
			get
			{
				return this.typeImpl.Module;
			}
		}

		/// <summary>Gets the name of the implemented type, with the path removed.</summary>
		/// <returns>A String containing the type's non-qualified name.</returns>
		public override string Name
		{
			get
			{
				return this.typeImpl.Name;
			}
		}

		/// <summary>Gets the namespace of the implemented type.</summary>
		/// <returns>A String containing the type's namespace.</returns>
		public override string Namespace
		{
			get
			{
				return this.typeImpl.Namespace;
			}
		}

		/// <summary>Gets a handle to the internal metadata representation of an implemented type.</summary>
		/// <returns>A RuntimeTypeHandle object.</returns>
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this.typeImpl.TypeHandle;
			}
		}

		/// <summary>Gets the underlying <see cref="T:System.Type" /> that represents the implemented type.</summary>
		/// <returns>The underlying type.</returns>
		public override Type UnderlyingSystemType
		{
			get
			{
				return this.typeImpl.UnderlyingSystemType;
			}
		}

		/// <summary>Gets the attributes assigned to the TypeDelegator.</summary>
		/// <returns>A TypeAttributes object representing the implementation attribute flags.</returns>
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.typeImpl.Attributes;
		}

		/// <summary>Gets the constructor that implemented the TypeDelegator.</summary>
		/// <returns>A ConstructorInfo object for the method that matches the specified criteria, or null if a match cannot be found.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects using reflection. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="callConvention">The calling conventions. </param>
		/// <param name="types">An array of type Type containing a list of the parameter number, order, and types. Types cannot be null; use an appropriate GetMethod method or an empty array to search for a method without parameters. </param>
		/// <param name="modifiers">An array of type ParameterModifier having the same length as the <paramref name="types" /> array, whose elements represent the attributes associated with the parameters of the method to get. </param>
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeImpl.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		/// <summary>Returns an array of <see cref="T:System.Reflection.ConstructorInfo" /> objects representing constructors defined for the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
		/// <returns>An array of type ConstructorInfo containing the specified constructors defined for this class. If no constructors are defined, an empty array is returned. Depending on the value of a specified parameter, only public constructors or both public and non-public constructors will be returned.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetConstructors(bindingAttr);
		}

		/// <summary>Returns all the custom attributes defined for this type, specifying whether to search the type's inheritance chain.</summary>
		/// <returns>An array of objects containing all the custom attributes defined for this type.</returns>
		/// <param name="inherit">Specifies whether to search this type's inheritance chain to find the attributes. </param>
		/// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(inherit);
		}

		/// <summary>Returns an array of custom attributes identified by type.</summary>
		/// <returns>An array of objects containing the custom attributes defined in this type that match the <paramref name="attributeType" /> parameter, specifying whether to search the type's inheritance chain, or null if no custom attributes are defined on this type.</returns>
		/// <param name="attributeType">An array of custom attributes identified by type.</param>
		/// <param name="inherit">Specifies whether to search this type's inheritance chain to find the attributes. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="attributeType" /> is null. </exception>
		/// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded. </exception>
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(attributeType, inherit);
		}

		/// <summary>Returns the <see cref="T:System.Type" /> of the object encompassed or referred to by the current array, pointer or ByRef.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the object encompassed or referred to by the current array, pointer or ByRef, or null if the current <see cref="T:System.Type" /> is not an array, a pointer or a ByRef.</returns>
		public override Type GetElementType()
		{
			return this.typeImpl.GetElementType();
		}

		/// <summary>Returns the specified event.</summary>
		/// <returns>An <see cref="T:System.Reflection.EventInfo" /> object representing the event declared or inherited by this type with the specified name. This method returns null if no such event is found.</returns>
		/// <param name="name">The name of the event to get. </param>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvent(name, bindingAttr);
		}

		/// <summary>Returns an array of <see cref="T:System.Reflection.EventInfo" /> objects representing all the public events declared or inherited by the current TypeDelegator.</summary>
		/// <returns>Returns an array of type EventInfo containing all the events declared or inherited by the current type. If there are no events, an empty array is returned.</returns>
		public override EventInfo[] GetEvents()
		{
			return this.GetEvents(BindingFlags.Public);
		}

		/// <summary>Returns the events specified in <paramref name="bindingAttr" /> that are declared or inherited by the current TypeDelegator.</summary>
		/// <returns>An array of type EventInfo containing the events specified in <paramref name="bindingAttr" />. If there are no events, an empty array is returned.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvents(bindingAttr);
		}

		/// <summary>Returns a <see cref="T:System.Reflection.FieldInfo" /> object representing the field with the specified name.</summary>
		/// <returns>A FieldInfo object representing the field declared or inherited by this TypeDelegator with the specified name. Returns null if no such field is found.</returns>
		/// <param name="name">The name of the field to find. </param>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetField(name, bindingAttr);
		}

		/// <summary>Returns an array of <see cref="T:System.Reflection.FieldInfo" /> objects representing the data fields defined for the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
		/// <returns>An array of type FieldInfo containing the fields declared or inherited by the current TypeDelegator. An empty array is returned if there are no matched fields.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetFields(bindingAttr);
		}

		/// <summary>Returns the specified interface implemented by the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
		/// <returns>A Type object representing the interface implemented (directly or indirectly) by the current class with the fully qualified name matching the specified name. If no interface that matches name is found, null is returned.</returns>
		/// <param name="name">The fully qualified name of the interface implemented by the current class. </param>
		/// <param name="ignoreCase">true if the case is to be ignored; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public override Type GetInterface(string name, bool ignoreCase)
		{
			return this.typeImpl.GetInterface(name, ignoreCase);
		}

		/// <summary>Returns an interface mapping for the specified interface type.</summary>
		/// <returns>An <see cref="T:System.Reflection.InterfaceMapping" /> object representing the interface mapping for <paramref name="interfaceType" />.</returns>
		/// <param name="interfaceType">The <see cref="T:System.Type" /> of the interface to retrieve a mapping of. </param>
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			return this.typeImpl.GetInterfaceMap(interfaceType);
		}

		/// <summary>Returns all the interfaces implemented on the current class and its base classes.</summary>
		/// <returns>An array of type Type containing all the interfaces implemented on the current class and its base classes. If none are defined, an empty array is returned.</returns>
		public override Type[] GetInterfaces()
		{
			return this.typeImpl.GetInterfaces();
		}

		/// <summary>Returns members (properties, methods, constructors, fields, events, and nested types) specified by the given <paramref name="name" />, <paramref name="type" />, and <paramref name="bindingAttr" />.</summary>
		/// <returns>An array of type MemberInfo containing all the members of the current class and its base class meeting the specified criteria.</returns>
		/// <param name="name">The name of the member to get. </param>
		/// <param name="type">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		/// <param name="bindingAttr">The type of members to get. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMember(name, type, bindingAttr);
		}

		/// <summary>Returns members specified by <paramref name="bindingAttr" />.</summary>
		/// <returns>An array of type MemberInfo containing all the members of the current class and its base classes that meet the <paramref name="bindingAttr" /> filter.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMembers(bindingAttr);
		}

		/// <summary>Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.</summary>
		/// <returns>A MethodInfoInfo object for the implementation method that matches the specified criteria, or null if a match cannot be found.</returns>
		/// <param name="name">The method name. </param>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects using reflection. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="callConvention">The calling conventions. </param>
		/// <param name="types">An array of type Type containing a list of the parameter number, order, and types. Types cannot be null; use an appropriate GetMethod method or an empty array to search for a method without parameters. </param>
		/// <param name="modifiers">An array of type ParameterModifier having the same length as the <paramref name="types" /> array, whose elements represent the attributes associated with the parameters of the method to get. </param>
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeImpl.GetMethodImplInternal(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		/// <summary>Returns an array of <see cref="T:System.Reflection.MethodInfo" /> objects representing specified methods of the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
		/// <returns>An array of MethodInfo objects representing the methods defined on this TypeDelegator.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMethods(bindingAttr);
		}

		/// <summary>Returns a nested type specified by <paramref name="name" /> and in <paramref name="bindingAttr" /> that are declared or inherited by the type represented by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
		/// <returns>A Type object representing the nested type.</returns>
		/// <param name="name">The nested type's name. </param>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedType(name, bindingAttr);
		}

		/// <summary>Returns the nested types specified in <paramref name="bindingAttr" /> that are declared or inherited by the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
		/// <returns>An array of type Type containing the nested types.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedTypes(bindingAttr);
		}

		/// <summary>Returns an array of <see cref="T:System.Reflection.PropertyInfo" /> objects representing properties of the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
		/// <returns>An array of PropertyInfo objects representing properties defined on this TypeDelegator.</returns>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetProperties(bindingAttr);
		}

		/// <summary>When overridden in a derived class, searches for the specified property whose parameters match the specified argument types and modifiers, using the specified binding constraints.</summary>
		/// <returns>A <see cref="T:System.Reflection.PropertyInfo" /> object for the property that matches the specified criteria, or null if a match cannot be found.</returns>
		/// <param name="name">The property to get. </param>
		/// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects via reflection. If <paramref name="binder" /> is null, the default binder is used. See <see cref="T:System.Reflection.Binder" />. </param>
		/// <param name="returnType">The return type of the property. </param>
		/// <param name="types">A list of parameter types. The list represents the number, order, and types of the parameters. Types cannot be null; use an appropriate GetMethod method or an empty array to search for a method without parameters. </param>
		/// <param name="modifiers">An array of the same length as types with elements that represent the attributes associated with the parameters of the method to get. </param>
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeImpl.GetPropertyImplInternal(name, bindingAttr, binder, returnType, types, modifiers);
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Type" /> encompasses or refers to another type; that is, whether the current <see cref="T:System.Type" /> is an array, a pointer or a ByRef.</summary>
		/// <returns>true if the <see cref="T:System.Type" /> is an array, a pointer or a ByRef; otherwise, false.</returns>
		protected override bool HasElementTypeImpl()
		{
			return this.typeImpl.HasElementType;
		}

		/// <summary>Invokes the specified member. The method that is to be invoked must be accessible and provide the most specific match with the specified argument list, under the constraints of the specified binder and invocation attributes.</summary>
		/// <returns>An Object representing the return value of the invoked member.</returns>
		/// <param name="name">The name of the member to invoke. This may be a constructor, method, property, or field. If an empty string ("") is passed, the default member is invoked. </param>
		/// <param name="invokeAttr">The invocation attribute. This must be one of the following <see cref="T:System.Reflection.BindingFlags" /> : InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty. A suitable invocation attribute must be specified. If a static member is to be invoked, the Static flag must be set. </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects via reflection. If <paramref name="binder" /> is null, the default binder is used. See <see cref="T:System.Reflection.Binder" />. </param>
		/// <param name="target">The object on which to invoke the specified member. </param>
		/// <param name="args">An array of type Object that contains the number, order, and type of the parameters of the member to be invoked. If <paramref name="args" /> contains an uninitialized Object, it is treated as empty, which, with the default binder, can be widened to 0, 0.0 or a string. </param>
		/// <param name="modifiers">An array of type ParameterModifer that is the same length as <paramref name="args" />, with elements that represent the attributes associated with the arguments of the member to be invoked. A parameter has attributes associated with it in the member's signature. For ByRef, use ParameterModifer.ByRef, and for none, use ParameterModifer.None. The default binder does exact matching on these. Attributes such as In and InOut are not used in binding, and can be viewed using ParameterInfo. </param>
		/// <param name="culture">An instance of CultureInfo used to govern the coercion of types. This is necessary, for example, to convert a string that represents 1000 to a Double value, since 1000 is represented differently by different cultures. If <paramref name="culture" /> is null, the CultureInfo for the current thread's CultureInfo is used. </param>
		/// <param name="namedParameters">An array of type String containing parameter names that match up, starting at element zero, with the <paramref name="args" /> array. There must be no holes in the array. If <paramref name="args" />. Length is greater than <paramref name="namedParameters" />. Length, the remaining parameters are filled in order. </param>
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this.typeImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Type" /> is an array.</summary>
		/// <returns>true if the <see cref="T:System.Type" /> is an array; otherwise, false.</returns>
		protected override bool IsArrayImpl()
		{
			return this.typeImpl.IsArray;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Type" /> is passed by reference.</summary>
		/// <returns>true if the <see cref="T:System.Type" /> is passed by reference; otherwise, false.</returns>
		protected override bool IsByRefImpl()
		{
			return this.typeImpl.IsByRef;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Type" /> is a COM object.</summary>
		/// <returns>true if the <see cref="T:System.Type" /> is a COM object; otherwise, false.</returns>
		protected override bool IsCOMObjectImpl()
		{
			return this.typeImpl.IsCOMObject;
		}

		/// <summary>Indicates whether a custom attribute identified by <paramref name="attributeType" /> is defined.</summary>
		/// <returns>true if a custom attribute identified by <paramref name="attributeType" /> is defined; otherwise, false.</returns>
		/// <param name="attributeType">Specifies whether to search this type's inheritance chain to find the attributes. </param>
		/// <param name="inherit">An array of custom attributes identified by type. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="attributeType" /> is null. </exception>
		/// <exception cref="T:System.Reflection.ReflectionTypeLoadException">The custom attribute type cannot be loaded. </exception>
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.typeImpl.IsDefined(attributeType, inherit);
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Type" /> is a pointer.</summary>
		/// <returns>true if the <see cref="T:System.Type" /> is a pointer; otherwise, false.</returns>
		protected override bool IsPointerImpl()
		{
			return this.typeImpl.IsPointer;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Type" /> is one of the primitive types.</summary>
		/// <returns>true if the <see cref="T:System.Type" /> is one of the primitive types; otherwise, false.</returns>
		protected override bool IsPrimitiveImpl()
		{
			return this.typeImpl.IsPrimitive;
		}

		/// <summary>Gets a value indicating whether the type is a value type; that is, not a class or an interface.</summary>
		/// <returns>true if the type is a value type; otherwise, false.</returns>
		protected override bool IsValueTypeImpl()
		{
			return this.typeImpl.IsValueType;
		}

		/// <summary>Gets a value that identifies this entity in metadata.</summary>
		/// <returns>A value which, in combination with the module, uniquely identifies this entity in metadata.</returns>
		public override int MetadataToken
		{
			get
			{
				return this.typeImpl.MetadataToken;
			}
		}
	}
}
