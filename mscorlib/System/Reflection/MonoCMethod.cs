using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace System.Reflection
{
	internal class MonoCMethod : ConstructorInfo, ISerializable
	{
		internal IntPtr mhandle;

		private string name;

		private Type reftype;

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return MonoMethodInfo.GetMethodImplementationFlags(this.mhandle);
		}

		public override ParameterInfo[] GetParameters()
		{
			return MonoMethodInfo.GetParametersInfo(this.mhandle, this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object InternalInvoke(object obj, object[] parameters, out Exception exc);

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			ParameterInfo[] parameters2 = this.GetParameters();
			if ((parameters == null && parameters2.Length != 0) || (parameters != null && parameters.Length != parameters2.Length))
			{
				throw new TargetParameterCountException("parameters do not match signature");
			}
			if ((invokeAttr & BindingFlags.ExactBinding) == BindingFlags.Default)
			{
				if (!Binder.ConvertArgs(binder, parameters, parameters2, culture))
				{
					throw new ArgumentException("failed to convert parameters");
				}
			}
			else
			{
				for (int i = 0; i < parameters2.Length; i++)
				{
					if (parameters[i].GetType() != parameters2[i].ParameterType)
					{
						throw new ArgumentException("parameters do not match signature");
					}
				}
			}
			if (obj == null && this.DeclaringType.ContainsGenericParameters)
			{
				throw new MemberAccessException("Cannot create an instance of " + this.DeclaringType + " because Type.ContainsGenericParameters is true.");
			}
			if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default && this.DeclaringType.IsAbstract)
			{
				throw new MemberAccessException(string.Format("Cannot create an instance of {0} because it is an abstract class", this.DeclaringType));
			}
			Exception ex = null;
			object obj2 = null;
			try
			{
				obj2 = this.InternalInvoke(obj, parameters, out ex);
			}
			catch (MethodAccessException)
			{
				throw;
			}
			catch (Exception inner)
			{
				throw new TargetInvocationException(inner);
			}
			if (ex != null)
			{
				throw ex;
			}
			return (obj != null) ? null : obj2;
		}

		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(null, invokeAttr, binder, parameters, culture);
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return new RuntimeMethodHandle(this.mhandle);
			}
		}

		public override MethodAttributes Attributes
		{
			get
			{
				return MonoMethodInfo.GetAttributes(this.mhandle);
			}
		}

		public override CallingConventions CallingConvention
		{
			get
			{
				return MonoMethodInfo.GetCallingConvention(this.mhandle);
			}
		}

		public override Type ReflectedType
		{
			get
			{
				return this.reftype;
			}
		}

		public override Type DeclaringType
		{
			get
			{
				return MonoMethodInfo.GetDeclaringType(this.mhandle);
			}
		}

		public override string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return MonoMethod.get_name(this);
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

		public override MethodBody GetMethodBody()
		{
			return MethodBase.GetMethodBody(this.mhandle);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Void ");
			stringBuilder.Append(this.Name);
			stringBuilder.Append("(");
			ParameterInfo[] parameters = this.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(parameters[i].ParameterType.Name);
			}
			if (this.CallingConvention == CallingConventions.Any)
			{
				stringBuilder.Append(", ...");
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			MemberInfoSerializationHolder.Serialize(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Constructor);
		}
	}
}
