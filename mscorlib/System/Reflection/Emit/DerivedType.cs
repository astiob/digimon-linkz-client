using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	internal abstract class DerivedType : Type
	{
		internal Type elementType;

		internal DerivedType(Type elementType)
		{
			this.elementType = elementType;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void create_unmanaged_type(Type type);

		internal abstract string FormatName(string elementName);

		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException();
		}

		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException();
		}

		public override Type GetElementType()
		{
			return this.elementType;
		}

		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.elementType.Attributes;
		}

		protected override bool HasElementTypeImpl()
		{
			return true;
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
			return false;
		}

		protected override bool IsPointerImpl()
		{
			return false;
		}

		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException();
		}

		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException();
		}

		public override bool IsInstanceOfType(object o)
		{
			return false;
		}

		public override bool IsAssignableFrom(Type c)
		{
			return false;
		}

		public override bool ContainsGenericParameters
		{
			get
			{
				return this.elementType.ContainsGenericParameters;
			}
		}

		public override Type MakeGenericType(params Type[] typeArguments)
		{
			throw new NotSupportedException();
		}

		public override Type MakeArrayType()
		{
			return new ArrayType(this, 0);
		}

		public override Type MakeArrayType(int rank)
		{
			if (rank < 1)
			{
				throw new IndexOutOfRangeException();
			}
			return new ArrayType(this, rank);
		}

		public override Type MakeByRefType()
		{
			return new ByRefType(this);
		}

		public override Type MakePointerType()
		{
			return new PointerType(this);
		}

		public override string ToString()
		{
			return this.FormatName(this.elementType.ToString());
		}

		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override StructLayoutAttribute StructLayoutAttribute
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override Assembly Assembly
		{
			get
			{
				return this.elementType.Assembly;
			}
		}

		public override string AssemblyQualifiedName
		{
			get
			{
				string text = this.FormatName(this.elementType.FullName);
				if (text == null)
				{
					return null;
				}
				return text + ", " + this.elementType.Assembly.FullName;
			}
		}

		public override string FullName
		{
			get
			{
				return this.FormatName(this.elementType.FullName);
			}
		}

		public override string Name
		{
			get
			{
				return this.FormatName(this.elementType.Name);
			}
		}

		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override Module Module
		{
			get
			{
				return this.elementType.Module;
			}
		}

		public override string Namespace
		{
			get
			{
				return this.elementType.Namespace;
			}
		}

		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override Type UnderlyingSystemType
		{
			get
			{
				DerivedType.create_unmanaged_type(this);
				return this;
			}
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}
	}
}
