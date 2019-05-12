using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal class EnumInfo
	{
		public string[] names;

		public int[] values;

		public string[] annotations;

		public bool isFlags;

		[UsedByNativeCode]
		internal static EnumInfo CreateEnumInfoFromNativeEnum(string[] names, int[] values, string[] annotations, bool isFlags)
		{
			return new EnumInfo
			{
				names = names,
				values = values,
				annotations = annotations,
				isFlags = isFlags
			};
		}
	}
}
