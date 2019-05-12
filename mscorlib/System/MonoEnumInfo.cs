using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System
{
	internal struct MonoEnumInfo
	{
		internal Type utype;

		internal Array values;

		internal string[] names;

		internal Hashtable name_hash;

		[ThreadStatic]
		private static Hashtable cache;

		private static Hashtable global_cache;

		private static object global_cache_monitor;

		internal static MonoEnumInfo.SByteComparer sbyte_comparer = new MonoEnumInfo.SByteComparer();

		internal static MonoEnumInfo.ShortComparer short_comparer = new MonoEnumInfo.ShortComparer();

		internal static MonoEnumInfo.IntComparer int_comparer = new MonoEnumInfo.IntComparer();

		internal static MonoEnumInfo.LongComparer long_comparer = new MonoEnumInfo.LongComparer();

		private MonoEnumInfo(MonoEnumInfo other)
		{
			this.utype = other.utype;
			this.values = other.values;
			this.names = other.names;
			this.name_hash = other.name_hash;
		}

		static MonoEnumInfo()
		{
			MonoEnumInfo.global_cache_monitor = new object();
			MonoEnumInfo.global_cache = new Hashtable();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_enum_info(Type enumType, out MonoEnumInfo info);

		private static Hashtable Cache
		{
			get
			{
				if (MonoEnumInfo.cache == null)
				{
					MonoEnumInfo.cache = new Hashtable();
				}
				return MonoEnumInfo.cache;
			}
		}

		internal static void GetInfo(Type enumType, out MonoEnumInfo info)
		{
			if (MonoEnumInfo.Cache.ContainsKey(enumType))
			{
				info = (MonoEnumInfo)MonoEnumInfo.cache[enumType];
				return;
			}
			object obj = MonoEnumInfo.global_cache_monitor;
			lock (obj)
			{
				if (MonoEnumInfo.global_cache.ContainsKey(enumType))
				{
					object obj2 = MonoEnumInfo.global_cache[enumType];
					MonoEnumInfo.cache[enumType] = obj2;
					info = (MonoEnumInfo)obj2;
					return;
				}
			}
			MonoEnumInfo.get_enum_info(enumType, out info);
			IComparer comparer = null;
			if (!(info.values is byte[]) && !(info.values is ushort[]) && !(info.values is uint[]) && !(info.values is ulong[]))
			{
				if (info.values is int[])
				{
					comparer = MonoEnumInfo.int_comparer;
				}
				else if (info.values is short[])
				{
					comparer = MonoEnumInfo.short_comparer;
				}
				else if (info.values is sbyte[])
				{
					comparer = MonoEnumInfo.sbyte_comparer;
				}
				else if (info.values is long[])
				{
					comparer = MonoEnumInfo.long_comparer;
				}
			}
			Array.Sort(info.values, info.names, comparer);
			if (info.names.Length > 50)
			{
				info.name_hash = new Hashtable(info.names.Length);
				for (int i = 0; i < info.names.Length; i++)
				{
					info.name_hash[info.names[i]] = i;
				}
			}
			MonoEnumInfo monoEnumInfo = new MonoEnumInfo(info);
			object obj3 = MonoEnumInfo.global_cache_monitor;
			lock (obj3)
			{
				MonoEnumInfo.global_cache[enumType] = monoEnumInfo;
			}
		}

		internal class SByteComparer : IComparer<sbyte>, IComparer
		{
			public int Compare(object x, object y)
			{
				sbyte b = (sbyte)x;
				sbyte b2 = (sbyte)y;
				return (int)((byte)b - (byte)b2);
			}

			public int Compare(sbyte ix, sbyte iy)
			{
				return (int)((byte)ix - (byte)iy);
			}
		}

		internal class ShortComparer : IComparer<short>, IComparer
		{
			public int Compare(object x, object y)
			{
				short num = (short)x;
				short num2 = (short)y;
				return (int)((ushort)num - (ushort)num2);
			}

			public int Compare(short ix, short iy)
			{
				return (int)((ushort)ix - (ushort)iy);
			}
		}

		internal class IntComparer : IComparer<int>, IComparer
		{
			public int Compare(object x, object y)
			{
				int num = (int)x;
				int num2 = (int)y;
				if (num == num2)
				{
					return 0;
				}
				if (num < num2)
				{
					return -1;
				}
				return 1;
			}

			public int Compare(int ix, int iy)
			{
				if (ix == iy)
				{
					return 0;
				}
				if (ix < iy)
				{
					return -1;
				}
				return 1;
			}
		}

		internal class LongComparer : IComparer<long>, IComparer
		{
			public int Compare(object x, object y)
			{
				long num = (long)x;
				long num2 = (long)y;
				if (num == num2)
				{
					return 0;
				}
				if (num < num2)
				{
					return -1;
				}
				return 1;
			}

			public int Compare(long ix, long iy)
			{
				if (ix == iy)
				{
					return 0;
				}
				if (ix < iy)
				{
					return -1;
				}
				return 1;
			}
		}
	}
}
