using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	internal class FieldOnTypeBuilderInst : FieldInfo
	{
		internal MonoGenericClass instantiation;

		internal FieldBuilder fb;

		public FieldOnTypeBuilderInst(MonoGenericClass instantiation, FieldBuilder fb)
		{
			this.instantiation = instantiation;
			this.fb = fb;
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
				return this.fb.Name;
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
			if (!((ModuleBuilder)this.instantiation.generic_type.Module).assemblyb.IsCompilerContext)
			{
				return this.fb.FieldType.ToString() + " " + this.Name;
			}
			return this.FieldType.ToString() + " " + this.Name;
		}

		public override FieldAttributes Attributes
		{
			get
			{
				return this.fb.Attributes;
			}
		}

		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override int MetadataToken
		{
			get
			{
				if (!((ModuleBuilder)this.instantiation.generic_type.Module).assemblyb.IsCompilerContext)
				{
					throw new InvalidOperationException();
				}
				return this.fb.MetadataToken;
			}
		}

		public override Type FieldType
		{
			get
			{
				if (!((ModuleBuilder)this.instantiation.generic_type.Module).assemblyb.IsCompilerContext)
				{
					throw new NotSupportedException();
				}
				return this.instantiation.InflateType(this.fb.FieldType);
			}
		}

		public override object GetValue(object obj)
		{
			throw new NotSupportedException();
		}

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
