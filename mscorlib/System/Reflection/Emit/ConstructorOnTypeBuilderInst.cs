using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	internal class ConstructorOnTypeBuilderInst : ConstructorInfo
	{
		private MonoGenericClass instantiation;

		private ConstructorBuilder cb;

		public ConstructorOnTypeBuilderInst(MonoGenericClass instantiation, ConstructorBuilder cb)
		{
			this.instantiation = instantiation;
			this.cb = cb;
		}

		public override Type DeclaringType
		{
			get
			{
				return this.instantiation;
			}
		}

		public override string Name
		{
			get
			{
				return this.cb.Name;
			}
		}

		public override Type ReflectedType
		{
			get
			{
				return this.instantiation;
			}
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.cb.IsDefined(attributeType, inherit);
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.cb.GetCustomAttributes(inherit);
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.cb.GetCustomAttributes(attributeType, inherit);
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.cb.GetMethodImplementationFlags();
		}

		public override ParameterInfo[] GetParameters()
		{
			if (!((ModuleBuilder)this.cb.Module).assemblyb.IsCompilerContext && !this.instantiation.generic_type.is_created)
			{
				throw new NotSupportedException();
			}
			ParameterInfo[] array = new ParameterInfo[this.cb.parameters.Length];
			for (int i = 0; i < this.cb.parameters.Length; i++)
			{
				Type type = this.instantiation.InflateType(this.cb.parameters[i]);
				array[i] = new ParameterInfo((this.cb.pinfo != null) ? this.cb.pinfo[i] : null, type, this, i + 1);
			}
			return array;
		}

		public override int MetadataToken
		{
			get
			{
				if (!((ModuleBuilder)this.cb.Module).assemblyb.IsCompilerContext)
				{
					return base.MetadataToken;
				}
				return this.cb.MetadataToken;
			}
		}

		internal override int GetParameterCount()
		{
			return this.cb.GetParameterCount();
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.cb.Invoke(obj, invokeAttr, binder, parameters, culture);
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.cb.MethodHandle;
			}
		}

		public override MethodAttributes Attributes
		{
			get
			{
				return this.cb.Attributes;
			}
		}

		public override CallingConventions CallingConvention
		{
			get
			{
				return this.cb.CallingConvention;
			}
		}

		public override Type[] GetGenericArguments()
		{
			return this.cb.GetGenericArguments();
		}

		public override bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		public override bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		public override bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}
	}
}
