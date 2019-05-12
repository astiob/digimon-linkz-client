using System;
using System.Globalization;
using System.Text;

namespace System.Reflection.Emit
{
	internal class MethodOnTypeBuilderInst : MethodInfo
	{
		private MonoGenericClass instantiation;

		internal MethodBuilder mb;

		private Type[] method_arguments;

		private MethodOnTypeBuilderInst generic_method_definition;

		public MethodOnTypeBuilderInst(MonoGenericClass instantiation, MethodBuilder mb)
		{
			this.instantiation = instantiation;
			this.mb = mb;
		}

		internal MethodOnTypeBuilderInst(MethodOnTypeBuilderInst gmd, Type[] typeArguments)
		{
			this.instantiation = gmd.instantiation;
			this.mb = gmd.mb;
			this.method_arguments = new Type[typeArguments.Length];
			typeArguments.CopyTo(this.method_arguments, 0);
			this.generic_method_definition = gmd;
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
				return this.mb.Name;
			}
		}

		public override Type ReflectedType
		{
			get
			{
				return this.instantiation;
			}
		}

		public override Type ReturnType
		{
			get
			{
				if (!((ModuleBuilder)this.mb.Module).assemblyb.IsCompilerContext)
				{
					return this.mb.ReturnType;
				}
				return this.instantiation.InflateType(this.mb.ReturnType, this.method_arguments);
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

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(this.ReturnType.ToString());
			stringBuilder.Append(" ");
			stringBuilder.Append(this.mb.Name);
			stringBuilder.Append("(");
			if (((ModuleBuilder)this.mb.Module).assemblyb.IsCompilerContext)
			{
				ParameterInfo[] parameters = this.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(parameters[i].ParameterType);
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.mb.GetMethodImplementationFlags();
		}

		public override ParameterInfo[] GetParameters()
		{
			if (!((ModuleBuilder)this.mb.Module).assemblyb.IsCompilerContext)
			{
				throw new NotSupportedException();
			}
			ParameterInfo[] array = new ParameterInfo[this.mb.parameters.Length];
			for (int i = 0; i < this.mb.parameters.Length; i++)
			{
				Type type = this.instantiation.InflateType(this.mb.parameters[i], this.method_arguments);
				array[i] = new ParameterInfo((this.mb.pinfo != null) ? this.mb.pinfo[i + 1] : null, type, this, i + 1);
			}
			return array;
		}

		public override int MetadataToken
		{
			get
			{
				if (!((ModuleBuilder)this.mb.Module).assemblyb.IsCompilerContext)
				{
					return base.MetadataToken;
				}
				return this.mb.MetadataToken;
			}
		}

		internal override int GetParameterCount()
		{
			return this.mb.GetParameterCount();
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override MethodAttributes Attributes
		{
			get
			{
				return this.mb.Attributes;
			}
		}

		public override CallingConventions CallingConvention
		{
			get
			{
				return this.mb.CallingConvention;
			}
		}

		public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			if (this.mb.generic_params == null || this.method_arguments != null)
			{
				throw new NotSupportedException();
			}
			if (typeArguments == null)
			{
				throw new ArgumentNullException("typeArguments");
			}
			for (int i = 0; i < typeArguments.Length; i++)
			{
				if (typeArguments[i] == null)
				{
					throw new ArgumentNullException("typeArguments");
				}
			}
			if (this.mb.generic_params.Length != typeArguments.Length)
			{
				throw new ArgumentException("Invalid argument array length");
			}
			return new MethodOnTypeBuilderInst(this, typeArguments);
		}

		public override Type[] GetGenericArguments()
		{
			if (this.mb.generic_params == null)
			{
				return null;
			}
			Type[] array = this.method_arguments ?? this.mb.generic_params;
			Type[] array2 = new Type[array.Length];
			array.CopyTo(array2, 0);
			return array2;
		}

		public override MethodInfo GetGenericMethodDefinition()
		{
			return this.generic_method_definition ?? this.mb;
		}

		public override bool ContainsGenericParameters
		{
			get
			{
				if (this.mb.generic_params == null)
				{
					throw new NotSupportedException();
				}
				if (this.method_arguments == null)
				{
					return true;
				}
				foreach (Type type in this.method_arguments)
				{
					if (type.ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.mb.generic_params != null && this.method_arguments == null;
			}
		}

		public override bool IsGenericMethod
		{
			get
			{
				return this.mb.generic_params != null;
			}
		}

		public override MethodInfo GetBaseDefinition()
		{
			throw new NotSupportedException();
		}

		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}
	}
}
