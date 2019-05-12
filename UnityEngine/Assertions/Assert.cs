using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Assertions.Comparers;

namespace UnityEngine.Assertions
{
	/// <summary>
	///   <para>The Assert class contains assertion methods for setting invariants in the code.</para>
	/// </summary>
	[DebuggerStepThrough]
	public static class Assert
	{
		internal const string UNITY_ASSERTIONS = "UNITY_ASSERTIONS";

		/// <summary>
		///   <para>Should an exception be thrown on a failure.</para>
		/// </summary>
		public static bool raiseExceptions;

		private static readonly Dictionary<Type, object> m_ComparersCache = new Dictionary<Type, object>();

		private static void Fail(string message, string userMessage)
		{
			if (Debugger.IsAttached)
			{
				throw new AssertionException(message, userMessage);
			}
			if (Assert.raiseExceptions)
			{
				throw new AssertionException(message, userMessage);
			}
			if (message == null)
			{
				message = "Assertion has failed\n";
			}
			if (userMessage != null)
			{
				message = userMessage + '\n' + message;
			}
			Debug.LogAssertion(message);
		}

		/// <summary>
		///   <para>Asserts that the condition is true.</para>
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsTrue(bool condition)
		{
			Assert.IsTrue(condition, null);
		}

		/// <summary>
		///   <para>Asserts that the condition is true.</para>
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
			{
				Assert.Fail(AssertionMessageUtil.BooleanFailureMessage(true), message);
			}
		}

		/// <summary>
		///   <para>Asserts that the condition is false.</para>
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsFalse(bool condition)
		{
			Assert.IsFalse(condition, null);
		}

		/// <summary>
		///   <para>Asserts that the condition is false.</para>
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void IsFalse(bool condition, string message)
		{
			if (condition)
			{
				Assert.Fail(AssertionMessageUtil.BooleanFailureMessage(false), message);
			}
		}

		/// <summary>
		///   <para>Asserts that the values are approximately equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreApproximatelyEqual(float expected, float actual)
		{
			Assert.AreEqual<float>(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		/// <summary>
		///   <para>Asserts that the values are approximately equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreApproximatelyEqual(float expected, float actual, string message)
		{
			Assert.AreEqual<float>(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		/// <summary>
		///   <para>Asserts that the values are approximately equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreApproximatelyEqual(float expected, float actual, float tolerance)
		{
			Assert.AreApproximatelyEqual(expected, actual, tolerance, null);
		}

		/// <summary>
		///   <para>Asserts that the values are approximately equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreApproximatelyEqual(float expected, float actual, float tolerance, string message)
		{
			Assert.AreEqual<float>(expected, actual, message, new FloatComparer(tolerance));
		}

		/// <summary>
		///   <para>Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotApproximatelyEqual(float expected, float actual)
		{
			Assert.AreNotEqual<float>(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		/// <summary>
		///   <para>Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotApproximatelyEqual(float expected, float actual, string message)
		{
			Assert.AreNotEqual<float>(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		/// <summary>
		///   <para>Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance)
		{
			Assert.AreNotApproximatelyEqual(expected, actual, tolerance, null);
		}

		/// <summary>
		///   <para>Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check (|a-b| &lt; tolerance). Default tolerance is 0.00001f.</para>
		/// </summary>
		/// <param name="tolerance">Tolerance of approximation.</param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance, string message)
		{
			Assert.AreNotEqual<float>(expected, actual, message, new FloatComparer(tolerance));
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual<T>(T expected, T actual)
		{
			Assert.AreEqual<T>(expected, actual, null);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual<T>(T expected, T actual, string message)
		{
			Assert.AreEqual<T>(expected, actual, message, Assert.GetEqualityComparer<T>(null));
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AreEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
		{
			if (!comparer.Equals(actual, expected))
			{
				Assert.Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, true), message);
			}
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotEqual<T>(T expected, T actual)
		{
			Assert.AreNotEqual<T>(expected, actual, null);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotEqual<T>(T expected, T actual, string message)
		{
			Assert.AreNotEqual<T>(expected, actual, message, Assert.GetEqualityComparer<T>(null));
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void AreNotEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
		{
			if (comparer.Equals(actual, expected))
			{
				Assert.Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, false), message);
			}
		}

		private static IEqualityComparer<T> GetEqualityComparer<T>(params object[] args)
		{
			Type typeFromHandle = typeof(T);
			object @default;
			Assert.m_ComparersCache.TryGetValue(typeFromHandle, out @default);
			if (@default != null)
			{
				return (IEqualityComparer<T>)@default;
			}
			@default = EqualityComparer<T>.Default;
			Assert.m_ComparersCache.Add(typeFromHandle, @default);
			return (IEqualityComparer<T>)@default;
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNull<T>(T value) where T : class
		{
			Assert.IsNull<T>(value, null);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNull<T>(T value, string message) where T : class
		{
			if (value != null)
			{
				Assert.Fail(AssertionMessageUtil.NullFailureMessage(value, true), message);
			}
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNotNull<T>(T value) where T : class
		{
			Assert.IsNotNull<T>(value, null);
		}

		[Conditional("UNITY_ASSERTIONS")]
		public static void IsNotNull<T>(T value, string message) where T : class
		{
			if (value == null)
			{
				Assert.Fail(AssertionMessageUtil.NullFailureMessage(value, false), message);
			}
		}
	}
}
