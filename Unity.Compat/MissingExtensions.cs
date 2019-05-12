using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
	internal static class MissingExtensions
	{
		internal static bool HasFlag(this Enum enumValue, Enum flag)
		{
			long num = Convert.ToInt64(enumValue);
			long num2 = Convert.ToInt64(flag);
			return (num & num2) == num2;
		}

		internal static T GetCustomAttribute<T>(this PropertyInfo prop, bool inherit) where T : Attribute
		{
			return (T)((object)prop.GetCustomAttributes(typeof(T), inherit).FirstOrDefault<object>());
		}

		internal static T GetCustomAttribute<T>(this PropertyInfo prop) where T : Attribute
		{
			return prop.GetCustomAttribute(true);
		}

		internal static T GetCustomAttribute<T>(this MemberInfo member, bool inherit) where T : Attribute
		{
			return (T)((object)member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault<object>());
		}

		internal static T GetCustomAttribute<T>(this MemberInfo member) where T : Attribute
		{
			return member.GetCustomAttribute(true);
		}

		internal static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> list1, IEnumerable<T2> list2, Func<T1, T2, TResult> zipper)
		{
			IEnumerator<T1> e = list1.GetEnumerator();
			IEnumerator<T2> e2 = list2.GetEnumerator();
			while (e.MoveNext() && e2.MoveNext())
			{
				yield return zipper(e.Current, e2.Current);
			}
			yield break;
		}
	}
}
