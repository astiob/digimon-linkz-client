using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	[Serializable]
	internal class MonoField : FieldInfo, ISerializable
	{
		internal IntPtr klass;

		internal RuntimeFieldHandle fhandle;

		private string name;

		private Type type;

		private FieldAttributes attrs;

		public override FieldAttributes Attributes
		{
			get
			{
				return this.attrs;
			}
		}

		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return this.fhandle;
			}
		}

		public override Type FieldType
		{
			get
			{
				return this.type;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type GetParentType(bool declaring);

		public override Type ReflectedType
		{
			get
			{
				return this.GetParentType(false);
			}
		}

		public override Type DeclaringType
		{
			get
			{
				return this.GetParentType(true);
			}
		}

		public override string Name
		{
			get
			{
				return this.name;
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
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal override extern int GetFieldOffset();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object GetValueInternal(object obj);

		public override object GetValue(object obj)
		{
			if (!this.IsStatic && obj == null)
			{
				throw new TargetException("Non-static field requires a target");
			}
			if (!this.IsLiteral)
			{
				this.CheckGeneric();
			}
			return this.GetValueInternal(obj);
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", this.type, this.name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetValueInternal(FieldInfo fi, object obj, object value);

		public override void SetValue(object obj, object val, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			if (!this.IsStatic && obj == null)
			{
				throw new TargetException("Non-static field requires a target");
			}
			if (this.IsLiteral)
			{
				throw new FieldAccessException("Cannot set a constant field");
			}
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			this.CheckGeneric();
			if (val != null)
			{
				object obj2 = binder.ChangeType(val, this.type, culture);
				if (obj2 == null)
				{
					throw new ArgumentException(string.Concat(new object[]
					{
						"Object type ",
						val.GetType(),
						" cannot be converted to target type: ",
						this.type
					}), "val");
				}
				val = obj2;
			}
			MonoField.SetValueInternal(this, obj, val);
		}

		internal MonoField Clone(string newName)
		{
			return new MonoField
			{
				name = newName,
				type = this.type,
				attrs = this.attrs,
				klass = this.klass,
				fhandle = this.fhandle
			};
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			MemberInfoSerializationHolder.Serialize(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Field);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern object GetRawConstantValue();

		private void CheckGeneric()
		{
			if (this.DeclaringType.ContainsGenericParameters)
			{
				throw new InvalidOperationException("Late bound operations cannot be performed on fields with types for which Type.ContainsGenericParameters is true.");
			}
		}
	}
}
