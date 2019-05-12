using System;

namespace System.Reflection.Emit
{
	internal class DynamicMethodTokenGenerator : TokenGenerator
	{
		private DynamicMethod m;

		public DynamicMethodTokenGenerator(DynamicMethod m)
		{
			this.m = m;
		}

		public int GetToken(string str)
		{
			return this.m.AddRef(str);
		}

		public int GetToken(MethodInfo method, Type[] opt_param_types)
		{
			throw new InvalidOperationException();
		}

		public int GetToken(MemberInfo member)
		{
			return this.m.AddRef(member);
		}

		public int GetToken(SignatureHelper helper)
		{
			return this.m.AddRef(helper);
		}
	}
}
