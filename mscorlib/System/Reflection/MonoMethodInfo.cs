using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
	internal struct MonoMethodInfo
	{
		private Type parent;

		private Type ret;

		internal MethodAttributes attrs;

		internal MethodImplAttributes iattrs;

		private CallingConventions callconv;

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_method_info(IntPtr handle, out MonoMethodInfo info);

		internal static MonoMethodInfo GetMethodInfo(IntPtr handle)
		{
			MonoMethodInfo result;
			MonoMethodInfo.get_method_info(handle, out result);
			return result;
		}

		internal static Type GetDeclaringType(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).parent;
		}

		internal static Type GetReturnType(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).ret;
		}

		internal static MethodAttributes GetAttributes(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).attrs;
		}

		internal static CallingConventions GetCallingConvention(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).callconv;
		}

		internal static MethodImplAttributes GetMethodImplementationFlags(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).iattrs;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParameterInfo[] get_parameter_info(IntPtr handle, MemberInfo member);

		internal static ParameterInfo[] GetParametersInfo(IntPtr handle, MemberInfo member)
		{
			return MonoMethodInfo.get_parameter_info(handle, member);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern UnmanagedMarshal get_retval_marshal(IntPtr handle);

		internal static ParameterInfo GetReturnParameterInfo(MonoMethod method)
		{
			return new ParameterInfo(MonoMethodInfo.GetReturnType(method.mhandle), method, MonoMethodInfo.get_retval_marshal(method.mhandle));
		}
	}
}
