using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace System.Reflection
{
	[Serializable]
	internal class MonoMethod : MethodInfo, ISerializable
	{
		internal IntPtr mhandle;

		private string name;

		private Type reftype;

		internal MonoMethod()
		{
		}

		internal MonoMethod(RuntimeMethodHandle mhandle)
		{
			this.mhandle = mhandle.Value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string get_name(MethodBase method);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MonoMethod get_base_definition(MonoMethod method);

		public override MethodInfo GetBaseDefinition()
		{
			return MonoMethod.get_base_definition(this);
		}

		public override ParameterInfo ReturnParameter
		{
			get
			{
				return MonoMethodInfo.GetReturnParameterInfo(this);
			}
		}

		public override Type ReturnType
		{
			get
			{
				return MonoMethodInfo.GetReturnType(this.mhandle);
			}
		}

		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return MonoMethodInfo.GetReturnParameterInfo(this);
			}
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return MonoMethodInfo.GetMethodImplementationFlags(this.mhandle);
		}

		public override ParameterInfo[] GetParameters()
		{
			ParameterInfo[] parametersInfo = MonoMethodInfo.GetParametersInfo(this.mhandle, this);
			ParameterInfo[] array = new ParameterInfo[parametersInfo.Length];
			parametersInfo.CopyTo(array, 0);
			return array;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object InternalInvoke(object obj, object[] parameters, out Exception exc);

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			ParameterInfo[] parametersInfo = MonoMethodInfo.GetParametersInfo(this.mhandle, this);
			if ((parameters == null && parametersInfo.Length != 0) || (parameters != null && parameters.Length != parametersInfo.Length))
			{
				throw new TargetParameterCountException("parameters do not match signature");
			}
			if ((invokeAttr & BindingFlags.ExactBinding) == BindingFlags.Default)
			{
				if (!Binder.ConvertArgs(binder, parameters, parametersInfo, culture))
				{
					throw new ArgumentException("failed to convert parameters");
				}
			}
			else
			{
				for (int i = 0; i < parametersInfo.Length; i++)
				{
					if (parameters[i].GetType() != parametersInfo[i].ParameterType)
					{
						throw new ArgumentException("parameters do not match signature");
					}
				}
			}
			if (this.ContainsGenericParameters)
			{
				throw new InvalidOperationException("Late bound operations cannot be performed on types or methods for which ContainsGenericParameters is true.");
			}
			object result = null;
			Exception ex;
			try
			{
				result = this.InternalInvoke(obj, parameters, out ex);
			}
			catch (ThreadAbortException)
			{
				throw;
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
			return result;
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

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern DllImportAttribute GetDllImportAttribute(IntPtr mhandle);

		internal object[] GetPseudoCustomAttributes()
		{
			int num = 0;
			MonoMethodInfo methodInfo = MonoMethodInfo.GetMethodInfo(this.mhandle);
			if ((methodInfo.iattrs & MethodImplAttributes.PreserveSig) != MethodImplAttributes.IL)
			{
				num++;
			}
			if ((methodInfo.attrs & MethodAttributes.PinvokeImpl) != MethodAttributes.PrivateScope)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			object[] array = new object[num];
			num = 0;
			if ((methodInfo.iattrs & MethodImplAttributes.PreserveSig) != MethodImplAttributes.IL)
			{
				array[num++] = new PreserveSigAttribute();
			}
			if ((methodInfo.attrs & MethodAttributes.PinvokeImpl) != MethodAttributes.PrivateScope)
			{
				DllImportAttribute dllImportAttribute = MonoMethod.GetDllImportAttribute(this.mhandle);
				if ((methodInfo.iattrs & MethodImplAttributes.PreserveSig) != MethodImplAttributes.IL)
				{
					dllImportAttribute.PreserveSig = true;
				}
				array[num++] = dllImportAttribute;
			}
			return array;
		}

		private static bool ShouldPrintFullName(Type type)
		{
			return type.IsClass && (!type.IsPointer || (!type.GetElementType().IsPrimitive && !type.GetElementType().IsNested));
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Type returnType = this.ReturnType;
			if (MonoMethod.ShouldPrintFullName(returnType))
			{
				stringBuilder.Append(returnType.ToString());
			}
			else
			{
				stringBuilder.Append(returnType.Name);
			}
			stringBuilder.Append(" ");
			stringBuilder.Append(this.Name);
			if (this.IsGenericMethod)
			{
				Type[] genericArguments = this.GetGenericArguments();
				stringBuilder.Append("[");
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(genericArguments[i].Name);
				}
				stringBuilder.Append("]");
			}
			stringBuilder.Append("(");
			ParameterInfo[] parameters = this.GetParameters();
			for (int j = 0; j < parameters.Length; j++)
			{
				if (j > 0)
				{
					stringBuilder.Append(", ");
				}
				Type type = parameters[j].ParameterType;
				bool isByRef = type.IsByRef;
				if (isByRef)
				{
					type = type.GetElementType();
				}
				if (MonoMethod.ShouldPrintFullName(type))
				{
					stringBuilder.Append(type.ToString());
				}
				else
				{
					stringBuilder.Append(type.Name);
				}
				if (isByRef)
				{
					stringBuilder.Append(" ByRef");
				}
			}
			if ((this.CallingConvention & CallingConventions.VarArgs) != (CallingConventions)0)
			{
				if (parameters.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("...");
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Type[] genericArguments = (!this.IsGenericMethod || this.IsGenericMethodDefinition) ? null : this.GetGenericArguments();
			MemberInfoSerializationHolder.Serialize(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Method, genericArguments);
		}

		public override MethodInfo MakeGenericMethod(Type[] methodInstantiation)
		{
			if (methodInstantiation == null)
			{
				throw new ArgumentNullException("methodInstantiation");
			}
			for (int i = 0; i < methodInstantiation.Length; i++)
			{
				if (methodInstantiation[i] == null)
				{
					throw new ArgumentNullException();
				}
			}
			MethodInfo methodInfo = this.MakeGenericMethod_impl(methodInstantiation);
			if (methodInfo == null)
			{
				throw new ArgumentException(string.Format("The method has {0} generic parameter(s) but {1} generic argument(s) were provided.", this.GetGenericArguments().Length, methodInstantiation.Length));
			}
			return methodInfo;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern MethodInfo MakeGenericMethod_impl(Type[] types);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern Type[] GetGenericArguments();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern MethodInfo GetGenericMethodDefinition_impl();

		public override MethodInfo GetGenericMethodDefinition()
		{
			MethodInfo genericMethodDefinition_impl = this.GetGenericMethodDefinition_impl();
			if (genericMethodDefinition_impl == null)
			{
				throw new InvalidOperationException();
			}
			return genericMethodDefinition_impl;
		}

		public override extern bool IsGenericMethodDefinition { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override extern bool IsGenericMethod { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override bool ContainsGenericParameters
		{
			get
			{
				if (this.IsGenericMethod)
				{
					foreach (Type type in this.GetGenericArguments())
					{
						if (type.ContainsGenericParameters)
						{
							return true;
						}
					}
				}
				return this.DeclaringType.ContainsGenericParameters;
			}
		}

		public override MethodBody GetMethodBody()
		{
			return MethodBase.GetMethodBody(this.mhandle);
		}
	}
}
