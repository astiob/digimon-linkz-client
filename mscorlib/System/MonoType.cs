using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	[Serializable]
	internal class MonoType : Type, ISerializable
	{
		[NonSerialized]
		private MonoTypeInfo type_info;

		internal MonoType(object obj)
		{
			MonoType.type_from_obj(this, obj);
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void type_from_obj(MonoType type, object obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern TypeAttributes get_attributes(Type type);

		internal ConstructorInfo GetDefaultConstructor()
		{
			if (this.type_info == null)
			{
				this.type_info = new MonoTypeInfo();
			}
			ConstructorInfo result;
			if ((result = this.type_info.default_ctor) == null)
			{
				result = (this.type_info.default_ctor = this.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, Type.EmptyTypes, null));
			}
			return result;
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return MonoType.get_attributes(this);
		}

		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (bindingAttr == BindingFlags.Default)
			{
				bindingAttr = (BindingFlags.Instance | BindingFlags.Public);
			}
			ConstructorInfo[] constructors = this.GetConstructors(bindingAttr);
			ConstructorInfo constructorInfo = null;
			int num = 0;
			foreach (ConstructorInfo constructorInfo2 in constructors)
			{
				if (callConvention == CallingConventions.Any || (constructorInfo2.CallingConvention & callConvention) == callConvention)
				{
					constructorInfo = constructorInfo2;
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
					array2[0] = constructorInfo;
				}
				else
				{
					num = 0;
					foreach (ConstructorInfo constructorInfo3 in constructors)
					{
						if (callConvention == CallingConventions.Any || (constructorInfo3.CallingConvention & callConvention) == callConvention)
						{
							array2[num++] = constructorInfo3;
						}
					}
				}
				if (binder == null)
				{
					binder = Binder.DefaultBinder;
				}
				return (ConstructorInfo)this.CheckMethodSecurity(binder.SelectMethod(bindingAttr, array2, types, modifiers));
			}
			if (num > 1)
			{
				throw new AmbiguousMatchException();
			}
			return (ConstructorInfo)this.CheckMethodSecurity(constructorInfo);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern ConstructorInfo[] GetConstructors_internal(BindingFlags bindingAttr, Type reflected_type);

		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.GetConstructors_internal(bindingAttr, this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern EventInfo InternalGetEvent(string name, BindingFlags bindingAttr);

		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.InternalGetEvent(name, bindingAttr);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern EventInfo[] GetEvents_internal(BindingFlags bindingAttr, Type reflected_type);

		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.GetEvents_internal(bindingAttr, this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern FieldInfo GetField(string name, BindingFlags bindingAttr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern FieldInfo[] GetFields_internal(BindingFlags bindingAttr, Type reflected_type);

		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.GetFields_internal(bindingAttr, this);
		}

		public override Type GetInterface(string name, bool ignoreCase)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			Type[] interfaces = this.GetInterfaces();
			foreach (Type type in interfaces)
			{
				Type type2 = (!type.IsGenericType) ? type : type.GetGenericTypeDefinition();
				if (string.Compare(type2.Name, name, ignoreCase, CultureInfo.InvariantCulture) == 0)
				{
					return type;
				}
				if (string.Compare(type2.FullName, name, ignoreCase, CultureInfo.InvariantCulture) == 0)
				{
					return type;
				}
			}
			return null;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern Type[] GetInterfaces();

		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this.FindMembers(MemberTypes.All, bindingAttr, null, null);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern MethodInfo[] GetMethodsByName(string name, BindingFlags bindingAttr, bool ignoreCase, Type reflected_type);

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.GetMethodsByName(null, bindingAttr, false, this);
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			bool ignoreCase = (bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default;
			MethodInfo[] methodsByName = this.GetMethodsByName(name, bindingAttr, ignoreCase, this);
			MethodInfo methodInfo = null;
			int num = 0;
			foreach (MethodInfo methodInfo2 in methodsByName)
			{
				if (callConvention == CallingConventions.Any || (methodInfo2.CallingConvention & callConvention) == callConvention)
				{
					methodInfo = methodInfo2;
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1 && types == null)
			{
				return (MethodInfo)this.CheckMethodSecurity(methodInfo);
			}
			MethodBase[] array2 = new MethodBase[num];
			if (num == 1)
			{
				array2[0] = methodInfo;
			}
			else
			{
				num = 0;
				foreach (MethodInfo methodInfo3 in methodsByName)
				{
					if (callConvention == CallingConventions.Any || (methodInfo3.CallingConvention & callConvention) == callConvention)
					{
						array2[num++] = methodInfo3;
					}
				}
			}
			if (types == null)
			{
				return (MethodInfo)this.CheckMethodSecurity(Binder.FindMostDerivedMatch(array2));
			}
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			return (MethodInfo)this.CheckMethodSecurity(binder.SelectMethod(bindingAttr, array2, types, modifiers));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern MethodInfo GetCorrespondingInflatedMethod(MethodInfo generic);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ConstructorInfo GetCorrespondingInflatedConstructor(ConstructorInfo generic);

		internal override MethodInfo GetMethod(MethodInfo fromNoninstanciated)
		{
			if (fromNoninstanciated == null)
			{
				throw new ArgumentNullException("fromNoninstanciated");
			}
			return this.GetCorrespondingInflatedMethod(fromNoninstanciated);
		}

		internal override ConstructorInfo GetConstructor(ConstructorInfo fromNoninstanciated)
		{
			if (fromNoninstanciated == null)
			{
				throw new ArgumentNullException("fromNoninstanciated");
			}
			return this.GetCorrespondingInflatedConstructor(fromNoninstanciated);
		}

		internal override FieldInfo GetField(FieldInfo fromNoninstanciated)
		{
			BindingFlags bindingFlags = (!fromNoninstanciated.IsStatic) ? BindingFlags.Instance : BindingFlags.Static;
			bindingFlags |= ((!fromNoninstanciated.IsPublic) ? BindingFlags.NonPublic : BindingFlags.Public);
			return this.GetField(fromNoninstanciated.Name, bindingFlags);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern Type GetNestedType(string name, BindingFlags bindingAttr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern Type[] GetNestedTypes(BindingFlags bindingAttr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern PropertyInfo[] GetPropertiesByName(string name, BindingFlags bindingAttr, bool icase, Type reflected_type);

		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.GetPropertiesByName(null, bindingAttr, false, this);
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			bool icase = (bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default;
			PropertyInfo[] propertiesByName = this.GetPropertiesByName(name, bindingAttr, icase, this);
			int num = propertiesByName.Length;
			if (num == 0)
			{
				return null;
			}
			if (num == 1 && (types == null || types.Length == 0) && (returnType == null || returnType == propertiesByName[0].PropertyType))
			{
				return propertiesByName[0];
			}
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			return binder.SelectProperty(bindingAttr, propertiesByName, returnType, types, modifiers);
		}

		protected override bool HasElementTypeImpl()
		{
			return this.IsArrayImpl() || this.IsByRefImpl() || this.IsPointerImpl();
		}

		protected override bool IsArrayImpl()
		{
			return Type.IsArrayImpl(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		protected override extern bool IsByRefImpl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		protected override extern bool IsCOMObjectImpl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		protected override extern bool IsPointerImpl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		protected override extern bool IsPrimitiveImpl();

		public override bool IsSubclassOf(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return base.IsSubclassOf(type);
		}

		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default)
			{
				if ((invokeAttr & (BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty)) != BindingFlags.Default)
				{
					throw new ArgumentException("bindingFlags");
				}
			}
			else if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if ((invokeAttr & BindingFlags.GetField) != BindingFlags.Default && (invokeAttr & BindingFlags.SetField) != BindingFlags.Default)
			{
				throw new ArgumentException("Cannot specify both Get and Set on a field.", "bindingFlags");
			}
			if ((invokeAttr & BindingFlags.GetProperty) != BindingFlags.Default && (invokeAttr & BindingFlags.SetProperty) != BindingFlags.Default)
			{
				throw new ArgumentException("Cannot specify both Get and Set on a property.", "bindingFlags");
			}
			if ((invokeAttr & BindingFlags.InvokeMethod) != BindingFlags.Default)
			{
				if ((invokeAttr & BindingFlags.SetField) != BindingFlags.Default)
				{
					throw new ArgumentException("Cannot specify Set on a field and Invoke on a method.", "bindingFlags");
				}
				if ((invokeAttr & BindingFlags.SetProperty) != BindingFlags.Default)
				{
					throw new ArgumentException("Cannot specify Set on a property and Invoke on a method.", "bindingFlags");
				}
			}
			if (namedParameters != null && (args == null || args.Length < namedParameters.Length))
			{
				throw new ArgumentException("namedParameters cannot be more than named arguments in number");
			}
			if ((invokeAttr & (BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty)) == BindingFlags.Default)
			{
				throw new ArgumentException("Must specify binding flags describing the invoke operation required.", "bindingFlags");
			}
			if ((invokeAttr & (BindingFlags.Public | BindingFlags.NonPublic)) == BindingFlags.Default)
			{
				invokeAttr |= BindingFlags.Public;
			}
			if ((invokeAttr & (BindingFlags.Instance | BindingFlags.Static)) == BindingFlags.Default)
			{
				invokeAttr |= (BindingFlags.Instance | BindingFlags.Static);
			}
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default)
			{
				invokeAttr |= BindingFlags.DeclaredOnly;
				ConstructorInfo[] constructors = this.GetConstructors(invokeAttr);
				object state = null;
				MethodBase methodBase = binder.BindToMethod(invokeAttr, constructors, ref args, modifiers, culture, namedParameters, out state);
				if (methodBase != null)
				{
					object result = methodBase.Invoke(target, invokeAttr, binder, args, culture);
					binder.ReorderArgumentArray(ref args, state);
					return result;
				}
				if (this.IsValueType && args == null)
				{
					return Activator.CreateInstanceInternal(this);
				}
				throw new MissingMethodException("Constructor on type '" + this.FullName + "' not found.");
			}
			else
			{
				if (name == string.Empty && Attribute.IsDefined(this, typeof(DefaultMemberAttribute)))
				{
					DefaultMemberAttribute defaultMemberAttribute = (DefaultMemberAttribute)Attribute.GetCustomAttribute(this, typeof(DefaultMemberAttribute));
					name = defaultMemberAttribute.MemberName;
				}
				bool flag = (invokeAttr & BindingFlags.IgnoreCase) != BindingFlags.Default;
				string text = null;
				bool flag2 = false;
				if ((invokeAttr & BindingFlags.InvokeMethod) != BindingFlags.Default)
				{
					MethodInfo[] methodsByName = this.GetMethodsByName(name, invokeAttr, flag, this);
					object state2 = null;
					if (args == null)
					{
						args = new object[0];
					}
					MethodBase methodBase2 = binder.BindToMethod(invokeAttr, methodsByName, ref args, modifiers, culture, namedParameters, out state2);
					if (methodBase2 != null)
					{
						ParameterInfo[] parameters = methodBase2.GetParameters();
						for (int i = 0; i < parameters.Length; i++)
						{
							if (System.Reflection.Missing.Value == args[i] && (parameters[i].Attributes & ParameterAttributes.HasDefault) != ParameterAttributes.HasDefault)
							{
								throw new ArgumentException("Used Missing.Value for argument without default value", "parameters");
							}
						}
						bool flag3 = parameters.Length > 0 && Attribute.IsDefined(parameters[parameters.Length - 1], typeof(ParamArrayAttribute));
						if (flag3)
						{
							this.ReorderParamArrayArguments(ref args, methodBase2);
						}
						object result2 = methodBase2.Invoke(target, invokeAttr, binder, args, culture);
						binder.ReorderArgumentArray(ref args, state2);
						return result2;
					}
					if (methodsByName.Length > 0)
					{
						text = "The best match for method " + name + " has some invalid parameter.";
					}
					else
					{
						text = "Cannot find method " + name + ".";
					}
				}
				if ((invokeAttr & BindingFlags.GetField) != BindingFlags.Default)
				{
					FieldInfo field = this.GetField(name, invokeAttr);
					if (field != null)
					{
						return field.GetValue(target);
					}
					if ((invokeAttr & BindingFlags.GetProperty) == BindingFlags.Default)
					{
						flag2 = true;
					}
				}
				else if ((invokeAttr & BindingFlags.SetField) != BindingFlags.Default)
				{
					FieldInfo field2 = this.GetField(name, invokeAttr);
					if (field2 != null)
					{
						if (args == null)
						{
							throw new ArgumentNullException("providedArgs");
						}
						if (args == null || args.Length != 1)
						{
							throw new ArgumentException("Only the field value can be specified to set a field value.", "bindingFlags");
						}
						field2.SetValue(target, args[0]);
						return null;
					}
					else if ((invokeAttr & BindingFlags.SetProperty) == BindingFlags.Default)
					{
						flag2 = true;
					}
				}
				if ((invokeAttr & BindingFlags.GetProperty) != BindingFlags.Default)
				{
					PropertyInfo[] propertiesByName = this.GetPropertiesByName(name, invokeAttr, flag, this);
					object state3 = null;
					int num = 0;
					for (int j = 0; j < propertiesByName.Length; j++)
					{
						if (propertiesByName[j].GetGetMethod(true) != null)
						{
							num++;
						}
					}
					MethodBase[] array = new MethodBase[num];
					num = 0;
					for (int j = 0; j < propertiesByName.Length; j++)
					{
						MethodBase getMethod = propertiesByName[j].GetGetMethod(true);
						if (getMethod != null)
						{
							array[num++] = getMethod;
						}
					}
					MethodBase methodBase3 = binder.BindToMethod(invokeAttr, array, ref args, modifiers, culture, namedParameters, out state3);
					if (methodBase3 != null)
					{
						ParameterInfo[] parameters2 = methodBase3.GetParameters();
						bool flag4 = parameters2.Length > 0 && Attribute.IsDefined(parameters2[parameters2.Length - 1], typeof(ParamArrayAttribute));
						if (flag4)
						{
							this.ReorderParamArrayArguments(ref args, methodBase3);
						}
						object result3 = methodBase3.Invoke(target, invokeAttr, binder, args, culture);
						binder.ReorderArgumentArray(ref args, state3);
						return result3;
					}
					flag2 = true;
				}
				else if ((invokeAttr & BindingFlags.SetProperty) != BindingFlags.Default)
				{
					PropertyInfo[] propertiesByName2 = this.GetPropertiesByName(name, invokeAttr, flag, this);
					object state4 = null;
					int num2 = 0;
					for (int k = 0; k < propertiesByName2.Length; k++)
					{
						if (propertiesByName2[k].GetSetMethod(true) != null)
						{
							num2++;
						}
					}
					MethodBase[] array2 = new MethodBase[num2];
					num2 = 0;
					for (int k = 0; k < propertiesByName2.Length; k++)
					{
						MethodBase setMethod = propertiesByName2[k].GetSetMethod(true);
						if (setMethod != null)
						{
							array2[num2++] = setMethod;
						}
					}
					MethodBase methodBase4 = binder.BindToMethod(invokeAttr, array2, ref args, modifiers, culture, namedParameters, out state4);
					if (methodBase4 != null)
					{
						ParameterInfo[] parameters3 = methodBase4.GetParameters();
						bool flag5 = parameters3.Length > 0 && Attribute.IsDefined(parameters3[parameters3.Length - 1], typeof(ParamArrayAttribute));
						if (flag5)
						{
							this.ReorderParamArrayArguments(ref args, methodBase4);
						}
						object result4 = methodBase4.Invoke(target, invokeAttr, binder, args, culture);
						binder.ReorderArgumentArray(ref args, state4);
						return result4;
					}
					flag2 = true;
				}
				if (text != null)
				{
					throw new MissingMethodException(text);
				}
				if (flag2)
				{
					throw new MissingFieldException("Cannot find variable " + name + ".");
				}
				return null;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern Type GetElementType();

		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		public override extern Assembly Assembly { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override string AssemblyQualifiedName
		{
			get
			{
				return this.getFullName(true, true);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string getFullName(bool full_name, bool assembly_qualified);

		public override extern Type BaseType { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override string FullName
		{
			get
			{
				if (this.type_info == null)
				{
					this.type_info = new MonoTypeInfo();
				}
				string result;
				if ((result = this.type_info.full_name) == null)
				{
					result = (this.type_info.full_name = this.getFullName(true, false));
				}
				return result;
			}
		}

		public override Guid GUID
		{
			get
			{
				object[] customAttributes = this.GetCustomAttributes(typeof(GuidAttribute), true);
				if (customAttributes.Length == 0)
				{
					return Guid.Empty;
				}
				return new Guid(((GuidAttribute)customAttributes[0]).Value);
			}
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, inherit);
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
		}

		public override MemberTypes MemberType
		{
			get
			{
				if (this.DeclaringType != null && !this.IsGenericParameter)
				{
					return MemberTypes.NestedType;
				}
				return MemberTypes.TypeInfo;
			}
		}

		public override extern string Name { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override extern string Namespace { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override extern Module Module { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override extern Type DeclaringType { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override Type ReflectedType
		{
			get
			{
				return this.DeclaringType;
			}
		}

		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this._impl;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern int GetArrayRank();

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			UnitySerializationHolder.GetTypeData(this, info, context);
		}

		public override string ToString()
		{
			return this.getFullName(false, false);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern Type[] GetGenericArguments();

		public override bool ContainsGenericParameters
		{
			get
			{
				if (this.IsGenericParameter)
				{
					return true;
				}
				if (this.IsGenericType)
				{
					foreach (Type type in this.GetGenericArguments())
					{
						if (type.ContainsGenericParameters)
						{
							return true;
						}
					}
				}
				return this.HasElementType && this.GetElementType().ContainsGenericParameters;
			}
		}

		public override extern bool IsGenericParameter { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override extern MethodBase DeclaringMethod { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override Type GetGenericTypeDefinition()
		{
			Type genericTypeDefinition_impl = base.GetGenericTypeDefinition_impl();
			if (genericTypeDefinition_impl == null)
			{
				throw new InvalidOperationException();
			}
			return genericTypeDefinition_impl;
		}

		private MethodBase CheckMethodSecurity(MethodBase mb)
		{
			return mb;
		}

		private void ReorderParamArrayArguments(ref object[] args, MethodBase method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			object[] array = new object[parameters.Length];
			Array array2 = Array.CreateInstance(parameters[parameters.Length - 1].ParameterType.GetElementType(), args.Length - (parameters.Length - 1));
			int num = 0;
			for (int i = 0; i < args.Length; i++)
			{
				if (i < parameters.Length - 1)
				{
					array[i] = args[i];
				}
				else
				{
					array2.SetValue(args[i], num);
					num++;
				}
			}
			array[parameters.Length - 1] = array2;
			args = array;
		}
	}
}
