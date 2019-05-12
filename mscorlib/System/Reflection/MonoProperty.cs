using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace System.Reflection
{
	[Serializable]
	internal class MonoProperty : PropertyInfo, ISerializable
	{
		internal IntPtr klass;

		internal IntPtr prop;

		private MonoPropertyInfo info;

		private PInfo cached;

		private MonoProperty.GetterAdapter cached_getter;

		private void CachePropertyInfo(PInfo flags)
		{
			if ((this.cached & flags) != flags)
			{
				MonoPropertyInfo.get_property_info(this, ref this.info, flags);
				this.cached |= flags;
			}
		}

		public override PropertyAttributes Attributes
		{
			get
			{
				this.CachePropertyInfo(PInfo.Attributes);
				return this.info.attrs;
			}
		}

		public override bool CanRead
		{
			get
			{
				this.CachePropertyInfo(PInfo.GetMethod);
				return this.info.get_method != null;
			}
		}

		public override bool CanWrite
		{
			get
			{
				this.CachePropertyInfo(PInfo.SetMethod);
				return this.info.set_method != null;
			}
		}

		public override Type PropertyType
		{
			get
			{
				this.CachePropertyInfo(PInfo.GetMethod | PInfo.SetMethod);
				if (this.info.get_method != null)
				{
					return this.info.get_method.ReturnType;
				}
				ParameterInfo[] parameters = this.info.set_method.GetParameters();
				return parameters[parameters.Length - 1].ParameterType;
			}
		}

		public override Type ReflectedType
		{
			get
			{
				this.CachePropertyInfo(PInfo.ReflectedType);
				return this.info.parent;
			}
		}

		public override Type DeclaringType
		{
			get
			{
				this.CachePropertyInfo(PInfo.DeclaringType);
				return this.info.parent;
			}
		}

		public override string Name
		{
			get
			{
				this.CachePropertyInfo(PInfo.Name);
				return this.info.name;
			}
		}

		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			int num = 0;
			int num2 = 0;
			this.CachePropertyInfo(PInfo.GetMethod | PInfo.SetMethod);
			if (this.info.set_method != null && (nonPublic || this.info.set_method.IsPublic))
			{
				num2 = 1;
			}
			if (this.info.get_method != null && (nonPublic || this.info.get_method.IsPublic))
			{
				num = 1;
			}
			MethodInfo[] array = new MethodInfo[num + num2];
			int num3 = 0;
			if (num2 != 0)
			{
				array[num3++] = this.info.set_method;
			}
			if (num != 0)
			{
				array[num3++] = this.info.get_method;
			}
			return array;
		}

		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			this.CachePropertyInfo(PInfo.GetMethod);
			if (this.info.get_method != null && (nonPublic || this.info.get_method.IsPublic))
			{
				return this.info.get_method;
			}
			return null;
		}

		public override ParameterInfo[] GetIndexParameters()
		{
			this.CachePropertyInfo(PInfo.GetMethod | PInfo.SetMethod);
			ParameterInfo[] array;
			if (this.info.get_method != null)
			{
				array = this.info.get_method.GetParameters();
			}
			else
			{
				if (this.info.set_method == null)
				{
					return new ParameterInfo[0];
				}
				ParameterInfo[] parameters = this.info.set_method.GetParameters();
				array = new ParameterInfo[parameters.Length - 1];
				Array.Copy(parameters, array, array.Length);
			}
			for (int i = 0; i < array.Length; i++)
			{
				ParameterInfo pinfo = array[i];
				array[i] = new ParameterInfo(pinfo, this);
			}
			return array;
		}

		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			this.CachePropertyInfo(PInfo.SetMethod);
			if (this.info.set_method != null && (nonPublic || this.info.set_method.IsPublic))
			{
				return this.info.set_method;
			}
			return null;
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(this, attributeType, false);
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, false);
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, false);
		}

		private static object GetterAdapterFrame<T, R>(MonoProperty.Getter<T, R> getter, object obj)
		{
			return getter((T)((object)obj));
		}

		private static object StaticGetterAdapterFrame<R>(MonoProperty.StaticGetter<R> getter, object obj)
		{
			return getter();
		}

		private static MonoProperty.GetterAdapter CreateGetterDelegate(MethodInfo method)
		{
			Type[] typeArguments;
			Type typeFromHandle;
			string name;
			if (method.IsStatic)
			{
				typeArguments = new Type[]
				{
					method.ReturnType
				};
				typeFromHandle = typeof(MonoProperty.StaticGetter<>);
				name = "StaticGetterAdapterFrame";
			}
			else
			{
				typeArguments = new Type[]
				{
					method.DeclaringType,
					method.ReturnType
				};
				typeFromHandle = typeof(MonoProperty.Getter<, >);
				name = "GetterAdapterFrame";
			}
			Type type = typeFromHandle.MakeGenericType(typeArguments);
			object obj = Delegate.CreateDelegate(type, method, false);
			if (obj == null)
			{
				throw new MethodAccessException();
			}
			MethodInfo methodInfo = typeof(MonoProperty).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
			methodInfo = methodInfo.MakeGenericMethod(typeArguments);
			return (MonoProperty.GetterAdapter)Delegate.CreateDelegate(typeof(MonoProperty.GetterAdapter), obj, methodInfo, true);
		}

		public override object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Default, null, index, null);
		}

		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			object result = null;
			MethodInfo getMethod = this.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("Get Method not found for '" + this.Name + "'");
			}
			try
			{
				if (index == null || index.Length == 0)
				{
					result = getMethod.Invoke(obj, invokeAttr, binder, null, culture);
				}
				else
				{
					result = getMethod.Invoke(obj, invokeAttr, binder, index, culture);
				}
			}
			catch (SecurityException inner)
			{
				throw new TargetInvocationException(inner);
			}
			return result;
		}

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo setMethod = this.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new ArgumentException("Set Method not found for '" + this.Name + "'");
			}
			object[] array;
			if (index == null || index.Length == 0)
			{
				array = new object[]
				{
					value
				};
			}
			else
			{
				int num = index.Length;
				array = new object[num + 1];
				index.CopyTo(array, 0);
				array[num] = value;
			}
			setMethod.Invoke(obj, invokeAttr, binder, array, culture);
		}

		public override string ToString()
		{
			return this.PropertyType.ToString() + " " + this.Name;
		}

		public override Type[] GetOptionalCustomModifiers()
		{
			Type[] typeModifiers = MonoPropertyInfo.GetTypeModifiers(this, true);
			if (typeModifiers == null)
			{
				return Type.EmptyTypes;
			}
			return typeModifiers;
		}

		public override Type[] GetRequiredCustomModifiers()
		{
			Type[] typeModifiers = MonoPropertyInfo.GetTypeModifiers(this, false);
			if (typeModifiers == null)
			{
				return Type.EmptyTypes;
			}
			return typeModifiers;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			MemberInfoSerializationHolder.Serialize(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Property);
		}

		private delegate object GetterAdapter(object _this);

		private delegate R Getter<T, R>(T _this);

		private delegate R StaticGetter<R>();
	}
}
