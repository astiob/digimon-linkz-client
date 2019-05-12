using System;

namespace UnityEngine.Networking
{
	internal class FloatConversion
	{
		public static float ToSingle(uint value)
		{
			UIntFloat uintFloat = default(UIntFloat);
			uintFloat.intValue = value;
			return uintFloat.floatValue;
		}

		public static double ToDouble(ulong value)
		{
			UIntFloat uintFloat = default(UIntFloat);
			uintFloat.longValue = value;
			return uintFloat.doubleValue;
		}
	}
}
