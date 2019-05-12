using System;

namespace System.Reflection.Emit
{
	internal interface TokenGenerator
	{
		int GetToken(string str);

		int GetToken(MemberInfo member);

		int GetToken(MethodInfo method, Type[] opt_param_types);

		int GetToken(SignatureHelper helper);
	}
}
