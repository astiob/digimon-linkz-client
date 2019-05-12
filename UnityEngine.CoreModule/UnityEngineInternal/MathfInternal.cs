using System;

namespace UnityEngineInternal
{
	public struct MathfInternal
	{
		public static volatile float FloatMinNormal = 1.17549435E-38f;

		public static volatile float FloatMinDenormal = float.Epsilon;

		public static bool IsFlushToZeroEnabled = MathfInternal.FloatMinDenormal == 0f;
	}
}
