using System;

namespace UnityEngine.Analytics
{
	internal static class Utils
	{
		public static bool isJSONPrimitive(object value)
		{
			Type type = value.GetType();
			return type == typeof(byte) || type == typeof(sbyte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong) || type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(decimal) || type == typeof(double) || type == typeof(float) || type == typeof(string) || type == typeof(bool) || type == typeof(char);
		}
	}
}
