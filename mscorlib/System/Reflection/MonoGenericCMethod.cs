using System;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
	[Serializable]
	internal class MonoGenericCMethod : MonoCMethod
	{
		internal MonoGenericCMethod()
		{
			throw new InvalidOperationException();
		}

		public override extern Type ReflectedType { [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
