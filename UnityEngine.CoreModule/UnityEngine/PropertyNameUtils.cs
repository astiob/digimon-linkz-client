using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Runtime/Utilities/PropertyName.h")]
	internal class PropertyNameUtils
	{
		[FreeFunction]
		public static PropertyName PropertyNameFromString([Unmarshalled] string name)
		{
			PropertyName result;
			PropertyNameUtils.PropertyNameFromString_Injected(name, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PropertyNameFromString_Injected(string name, out PropertyName ret);
	}
}
