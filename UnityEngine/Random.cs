using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class for generating random data.</para>
	/// </summary>
	public sealed class Random
	{
		/// <summary>
		///   <para>Sets the seed for the random number generator.</para>
		/// </summary>
		public static extern int seed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns a random float number between and min [inclusive] and max [inclusive] (Read Only).</para>
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Range(float min, float max);

		/// <summary>
		///   <para>Returns a random integer number between min [inclusive] and max [exclusive] (Read Only).</para>
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public static int Range(int min, int max)
		{
			return Random.RandomRangeInt(min, max);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int RandomRangeInt(int min, int max);

		/// <summary>
		///   <para>Returns a random number between 0.0 [inclusive] and 1.0 [inclusive] (Read Only).</para>
		/// </summary>
		public static extern float value { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns a random point inside a sphere with radius 1 (Read Only).</para>
		/// </summary>
		public static Vector3 insideUnitSphere
		{
			get
			{
				Vector3 result;
				Random.INTERNAL_get_insideUnitSphere(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_insideUnitSphere(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRandomUnitCircle(out Vector2 output);

		/// <summary>
		///   <para>Returns a random point inside a circle with radius 1 (Read Only).</para>
		/// </summary>
		public static Vector2 insideUnitCircle
		{
			get
			{
				Vector2 result;
				Random.GetRandomUnitCircle(out result);
				return result;
			}
		}

		/// <summary>
		///   <para>Returns a random point on the surface of a sphere with radius 1 (Read Only).</para>
		/// </summary>
		public static Vector3 onUnitSphere
		{
			get
			{
				Vector3 result;
				Random.INTERNAL_get_onUnitSphere(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_onUnitSphere(out Vector3 value);

		/// <summary>
		///   <para>Returns a random rotation (Read Only).</para>
		/// </summary>
		public static Quaternion rotation
		{
			get
			{
				Quaternion result;
				Random.INTERNAL_get_rotation(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_rotation(out Quaternion value);

		/// <summary>
		///   <para>Returns a random rotation with uniform distribution (Read Only).</para>
		/// </summary>
		public static Quaternion rotationUniform
		{
			get
			{
				Quaternion result;
				Random.INTERNAL_get_rotationUniform(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_rotationUniform(out Quaternion value);

		[Obsolete("Use Random.Range instead")]
		public static float RandomRange(float min, float max)
		{
			return Random.Range(min, max);
		}

		[Obsolete("Use Random.Range instead")]
		public static int RandomRange(int min, int max)
		{
			return Random.Range(min, max);
		}
	}
}
