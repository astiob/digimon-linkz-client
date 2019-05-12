using System;
using System.Diagnostics;

namespace UnityEngine.Assertions.Must
{
	/// <summary>
	///   <para>An extension class that serves as a wrapper for the Assert class.</para>
	/// </summary>
	[DebuggerStepThrough]
	public static class MustExtensions
	{
		/// <summary>
		///   <para>An extension method for Assertions.Assert.IsTrue.</para>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeTrue(this bool value)
		{
			Assert.IsTrue(value);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.IsTrue.</para>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeTrue(this bool value, string message)
		{
			Assert.IsTrue(value, message);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.IsFalse.</para>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeFalse(this bool value)
		{
			Assert.IsFalse(value);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.IsFalse.</para>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeFalse(this bool value, string message)
		{
			Assert.IsFalse(value, message);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeApproximatelyEqual(this float actual, float expected)
		{
			Assert.AreApproximatelyEqual(actual, expected);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeApproximatelyEqual(this float actual, float expected, string message)
		{
			Assert.AreApproximatelyEqual(actual, expected, message);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance)
		{
			Assert.AreApproximatelyEqual(actual, expected, tolerance);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
		{
			Assert.AreApproximatelyEqual(expected, actual, tolerance, message);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreNotApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected)
		{
			Assert.AreNotApproximatelyEqual(expected, actual);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreNotApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected, string message)
		{
			Assert.AreNotApproximatelyEqual(expected, actual, message);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreNotApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance)
		{
			Assert.AreNotApproximatelyEqual(expected, actual, tolerance);
		}

		/// <summary>
		///   <para>An extension method for Assertions.Assert.AreNotApproximatelyEqual.</para>
		/// </summary>
		/// <param name="actual"></param>
		/// <param name="expected"></param>
		/// <param name="message"></param>
		/// <param name="tolerance"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeApproximatelyEqual(this float actual, float expected, float tolerance, string message)
		{
			Assert.AreNotApproximatelyEqual(expected, actual, tolerance, message);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeEqual<T>(this T actual, T expected)
		{
			Assert.AreEqual<T>(actual, expected);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeEqual<T>(this T actual, T expected, string message)
		{
			Assert.AreEqual<T>(expected, actual, message);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeEqual<T>(this T actual, T expected)
		{
			Assert.AreNotEqual<T>(actual, expected);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeEqual<T>(this T actual, T expected, string message)
		{
			Assert.AreNotEqual<T>(expected, actual, message);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeNull<T>(this T expected) where T : class
		{
			Assert.IsNull<T>(expected);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustBeNull<T>(this T expected, string message) where T : class
		{
			Assert.IsNull<T>(expected, message);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeNull<T>(this T expected) where T : class
		{
			Assert.IsNotNull<T>(expected);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void MustNotBeNull<T>(this T expected, string message) where T : class
		{
			Assert.IsNotNull<T>(expected, message);
		}
	}
}
