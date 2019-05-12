using System;
using UnityEngine.Scripting;

namespace UnityEngine.Collections
{
	[RequiredByNativeCode]
	[AttributeUsage(AttributeTargets.Field)]
	public class NativeFixedLengthAttribute : Attribute
	{
		public int FixedLength;

		public NativeFixedLengthAttribute(int fixedLength)
		{
			this.FixedLength = fixedLength;
		}
	}
}
