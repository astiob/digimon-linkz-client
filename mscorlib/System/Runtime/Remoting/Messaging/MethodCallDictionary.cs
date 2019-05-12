using System;

namespace System.Runtime.Remoting.Messaging
{
	internal class MethodCallDictionary : MethodDictionary
	{
		public static string[] InternalKeys = new string[]
		{
			"__Uri",
			"__MethodName",
			"__TypeName",
			"__MethodSignature",
			"__Args",
			"__CallContext"
		};

		public MethodCallDictionary(IMethodMessage message) : base(message)
		{
			base.MethodKeys = MethodCallDictionary.InternalKeys;
		}
	}
}
