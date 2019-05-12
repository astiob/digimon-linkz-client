using System;
using UnityEngine;

namespace Neptune.Common
{
	public class NpDebugProfiler
	{
		public static double GetGCMemory(NpDebugProfilerType type)
		{
			return 0.0;
		}

		public static float GetFps()
		{
			return 1f / Time.deltaTime;
		}

		public static float GetMonoHeapSize(NpDebugProfilerType type)
		{
			return 0f;
		}

		public static double GetMonoUsedSize(NpDebugProfilerType type)
		{
			return 0.0;
		}

		public static NpDebugProfilerData GetObjectSize<T>(NpDebugProfilerType type) where T : class
		{
			return new NpDebugProfilerData();
		}
	}
}
