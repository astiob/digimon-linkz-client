using System;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
	[Serializable]
	internal class MonoGenericMethod : MonoMethod
	{
		internal MonoGenericMethod()
		{
			throw new InvalidOperationException();
		}

		public override extern Type ReflectedType { [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
