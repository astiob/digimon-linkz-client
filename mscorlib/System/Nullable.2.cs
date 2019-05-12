using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Supports a value type that can be assigned null like a reference type. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	public static class Nullable
	{
		/// <summary>Compares the relative values of two <see cref="T:System.Nullable`1" /> objects.</summary>
		/// <returns>An integer that indicates the relative values of the <paramref name="n1" /> and <paramref name="n2" /> parameters.Return ValueDescriptionLess than zeroThe <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n1" /> is false, and the <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n2" /> is true.-or-The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are true, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is less than the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.ZeroThe <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are false.-or-The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are true, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is equal to the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.Greater than zeroThe <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n1" /> is true, and the <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n2" /> is false.-or-The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are true, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is greater than the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.</returns>
		/// <param name="n1">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <param name="n2">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <typeparam name="T">The underlying value type of the <paramref name="n1" /> and <paramref name="n2" /> parameters.</typeparam>
		[ComVisible(false)]
		public static int Compare<T>(T? value1, T? value2) where T : struct
		{
			if (!value1.has_value)
			{
				return (!value2.has_value) ? 0 : -1;
			}
			if (!value2.has_value)
			{
				return 1;
			}
			return Comparer<T>.Default.Compare(value1.value, value2.value);
		}

		/// <summary>Indicates whether two specified <see cref="T:System.Nullable`1" /> objects are equal.</summary>
		/// <returns>true if the <paramref name="n1" /> parameter is equal to the <paramref name="n2" /> parameter; otherwise, false. The return value depends on the <see cref="P:System.Nullable`1.HasValue" /> and <see cref="P:System.Nullable`1.Value" /> properties of the two parameters that are compared.Return ValueDescriptiontrueThe <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are false. -or-The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are true, and the <see cref="P:System.Nullable`1.Value" /> properties of the parameters are equal.falseThe <see cref="P:System.Nullable`1.HasValue" /> property is true for one parameter and false for the other parameter.-or-The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are true, and the <see cref="P:System.Nullable`1.Value" /> properties of the parameters are unequal.</returns>
		/// <param name="n1">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <param name="n2">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <typeparam name="T">The underlying value type of the <paramref name="n1" /> and <paramref name="n2" /> parameters.</typeparam>
		[ComVisible(false)]
		public static bool Equals<T>(T? value1, T? value2) where T : struct
		{
			return value1.has_value == value2.has_value && (!value1.has_value || EqualityComparer<T>.Default.Equals(value1.value, value2.value));
		}

		/// <summary>Returns the underlying type argument of the specified nullable type.</summary>
		/// <returns>The type argument of the <paramref name="nullableType" /> parameter, if the <paramref name="nullableType" /> parameter is a closed generic nullable type; otherwise, null. </returns>
		/// <param name="nullableType">A <see cref="T:System.Type" /> object that describes a closed generic nullable type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="nullableType" /> is null.</exception>
		public static Type GetUnderlyingType(Type nullableType)
		{
			if (nullableType == null)
			{
				throw new ArgumentNullException("nullableType");
			}
			if (nullableType.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return nullableType.GetGenericArguments()[0];
			}
			return null;
		}
	}
}
