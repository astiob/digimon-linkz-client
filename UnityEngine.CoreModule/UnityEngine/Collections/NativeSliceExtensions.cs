using System;

namespace UnityEngine.Collections
{
	public static class NativeSliceExtensions
	{
		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray) where T : struct
		{
			return new NativeSlice<T>(thisArray);
		}

		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int length) where T : struct
		{
			return new NativeSlice<T>(thisArray, length);
		}

		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int start, int length) where T : struct
		{
			return new NativeSlice<T>(thisArray, start, length);
		}
	}
}
