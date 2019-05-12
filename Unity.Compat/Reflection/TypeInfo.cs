using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
	public class TypeInfo : Type
	{
		private static ConditionalWeakTable<Type, TypeInfo> typeInfoMap = new ConditionalWeakTable<Type, TypeInfo>();

		private Type underlyingType;

		private TypeInfo(Type underlyingType)
		{
			this.underlyingType = underlyingType;
		}

		internal static TypeInfo FromType(Type type)
		{
			return TypeInfo.typeInfoMap.GetValue(type, (Type _) => new TypeInfo(type));
		}

		public T GetCustomAttribute<T>(bool inherit) where T : Attribute
		{
			return (T)((object)this.underlyingType.GetCustomAttributes(typeof(T), inherit).FirstOrDefault<object>());
		}

		public T GetCustomAttribute<T>() where T : Attribute
		{
			return this.GetCustomAttribute<T>(true);
		}

		public IEnumerable<Type> ImplementedInterfaces
		{
			get
			{
				return this.underlyingType.GetInterfaces();
			}
		}

		public override Assembly Assembly
		{
			get
			{
				return this.underlyingType.Assembly;
			}
		}

		public override string AssemblyQualifiedName
		{
			get
			{
				return this.underlyingType.AssemblyQualifiedName;
			}
		}

		public override Type BaseType
		{
			get
			{
				return this.underlyingType.BaseType;
			}
		}

		public override string FullName
		{
			get
			{
				return this.underlyingType.FullName;
			}
		}

		public override Guid GUID
		{
			get
			{
				return this.underlyingType.GUID;
			}
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.underlyingType.Attributes;
		}

		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.underlyingType.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.underlyingType.GetConstructors(bindingAttr);
		}

		public override Type GetElementType()
		{
			return this.underlyingType.GetElementType();
		}

		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			return this.underlyingType.GetEvent(name, bindingAttr);
		}

		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.underlyingType.GetEvents(bindingAttr);
		}

		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return this.underlyingType.GetField(name, bindingAttr);
		}

		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.underlyingType.GetFields(bindingAttr);
		}

		public override Type GetInterface(string name, bool ignoreCase)
		{
			return this.underlyingType.GetInterface(name, ignoreCase);
		}

		public override Type[] GetInterfaces()
		{
			return this.underlyingType.GetInterfaces();
		}

		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this.underlyingType.GetMembers(bindingAttr);
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.underlyingType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.underlyingType.GetMethods(bindingAttr);
		}

		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			return this.underlyingType.GetNestedType(name, bindingAttr);
		}

		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.underlyingType.GetNestedTypes(bindingAttr);
		}

		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.underlyingType.GetProperties(bindingAttr);
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return this.underlyingType.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		protected override bool HasElementTypeImpl()
		{
			return this.underlyingType.HasElementType;
		}

		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this.underlyingType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		protected override bool IsArrayImpl()
		{
			return this.underlyingType.IsArray;
		}

		protected override bool IsByRefImpl()
		{
			return this.underlyingType.IsByRef;
		}

		protected override bool IsCOMObjectImpl()
		{
			return this.underlyingType.IsCOMObject;
		}

		protected override bool IsPointerImpl()
		{
			return this.underlyingType.IsPointer;
		}

		protected override bool IsPrimitiveImpl()
		{
			return this.underlyingType.IsPrimitive;
		}

		public override Module Module
		{
			get
			{
				return this.underlyingType.Module;
			}
		}

		public override string Namespace
		{
			get
			{
				return this.underlyingType.Namespace;
			}
		}

		public override Type UnderlyingSystemType
		{
			get
			{
				return this.underlyingType.UnderlyingSystemType;
			}
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.underlyingType.GetCustomAttributes(attributeType, inherit);
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.underlyingType.GetCustomAttributes(inherit);
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.underlyingType.IsDefined(attributeType, inherit);
		}

		public override string Name
		{
			get
			{
				return this.underlyingType.Name;
			}
		}
	}
}
