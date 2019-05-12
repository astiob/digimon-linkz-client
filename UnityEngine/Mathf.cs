using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	/// <summary>
	///   <para>A collection of common math functions.</para>
	/// </summary>
	public struct Mathf
	{
		/// <summary>
		///   <para>The infamous 3.14159265358979... value (Read Only).</para>
		/// </summary>
		public const float PI = 3.14159274f;

		/// <summary>
		///   <para>A representation of positive infinity (Read Only).</para>
		/// </summary>
		public const float Infinity = float.PositiveInfinity;

		/// <summary>
		///   <para>A representation of negative infinity (Read Only).</para>
		/// </summary>
		public const float NegativeInfinity = float.NegativeInfinity;

		/// <summary>
		///   <para>Degrees-to-radians conversion constant (Read Only).</para>
		/// </summary>
		public const float Deg2Rad = 0.0174532924f;

		/// <summary>
		///   <para>Radians-to-degrees conversion constant (Read Only).</para>
		/// </summary>
		public const float Rad2Deg = 57.29578f;

		/// <summary>
		///   <para>A tiny floating point value (Read Only).</para>
		/// </summary>
		public static readonly float Epsilon = (!MathfInternal.IsFlushToZeroEnabled) ? MathfInternal.FloatMinDenormal : MathfInternal.FloatMinNormal;

		/// <summary>
		///   <para>Returns the sine of angle f in radians.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Sin(float f)
		{
			return (float)Math.Sin((double)f);
		}

		/// <summary>
		///   <para>Returns the cosine of angle f in radians.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Cos(float f)
		{
			return (float)Math.Cos((double)f);
		}

		/// <summary>
		///   <para>Returns the tangent of angle f in radians.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Tan(float f)
		{
			return (float)Math.Tan((double)f);
		}

		/// <summary>
		///   <para>Returns the arc-sine of f - the angle in radians whose sine is f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Asin(float f)
		{
			return (float)Math.Asin((double)f);
		}

		/// <summary>
		///   <para>Returns the arc-cosine of f - the angle in radians whose cosine is f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Acos(float f)
		{
			return (float)Math.Acos((double)f);
		}

		/// <summary>
		///   <para>Returns the arc-tangent of f - the angle in radians whose tangent is f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Atan(float f)
		{
			return (float)Math.Atan((double)f);
		}

		/// <summary>
		///   <para>Returns the angle in radians whose Tan is y/x.</para>
		/// </summary>
		/// <param name="y"></param>
		/// <param name="x"></param>
		public static float Atan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x);
		}

		/// <summary>
		///   <para>Returns square root of f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		/// <summary>
		///   <para>Returns the absolute value of f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Abs(float f)
		{
			return Math.Abs(f);
		}

		/// <summary>
		///   <para>Returns the absolute value of value.</para>
		/// </summary>
		/// <param name="value"></param>
		public static int Abs(int value)
		{
			return Math.Abs(value);
		}

		/// <summary>
		///   <para>Returns the smallest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static float Min(float a, float b)
		{
			return (a >= b) ? b : a;
		}

		/// <summary>
		///   <para>Returns the smallest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static float Min(params float[] values)
		{
			int num = values.Length;
			if (num == 0)
			{
				return 0f;
			}
			float num2 = values[0];
			for (int i = 1; i < num; i++)
			{
				if (values[i] < num2)
				{
					num2 = values[i];
				}
			}
			return num2;
		}

		/// <summary>
		///   <para>Returns the smallest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static int Min(int a, int b)
		{
			return (a >= b) ? b : a;
		}

		/// <summary>
		///   <para>Returns the smallest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static int Min(params int[] values)
		{
			int num = values.Length;
			if (num == 0)
			{
				return 0;
			}
			int num2 = values[0];
			for (int i = 1; i < num; i++)
			{
				if (values[i] < num2)
				{
					num2 = values[i];
				}
			}
			return num2;
		}

		/// <summary>
		///   <para>Returns largest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static float Max(float a, float b)
		{
			return (a <= b) ? b : a;
		}

		/// <summary>
		///   <para>Returns largest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static float Max(params float[] values)
		{
			int num = values.Length;
			if (num == 0)
			{
				return 0f;
			}
			float num2 = values[0];
			for (int i = 1; i < num; i++)
			{
				if (values[i] > num2)
				{
					num2 = values[i];
				}
			}
			return num2;
		}

		/// <summary>
		///   <para>Returns the largest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static int Max(int a, int b)
		{
			return (a <= b) ? b : a;
		}

		/// <summary>
		///   <para>Returns the largest of two or more values.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="values"></param>
		public static int Max(params int[] values)
		{
			int num = values.Length;
			if (num == 0)
			{
				return 0;
			}
			int num2 = values[0];
			for (int i = 1; i < num; i++)
			{
				if (values[i] > num2)
				{
					num2 = values[i];
				}
			}
			return num2;
		}

		/// <summary>
		///   <para>Returns f raised to power p.</para>
		/// </summary>
		/// <param name="f"></param>
		/// <param name="p"></param>
		public static float Pow(float f, float p)
		{
			return (float)Math.Pow((double)f, (double)p);
		}

		/// <summary>
		///   <para>Returns e raised to the specified power.</para>
		/// </summary>
		/// <param name="power"></param>
		public static float Exp(float power)
		{
			return (float)Math.Exp((double)power);
		}

		/// <summary>
		///   <para>Returns the logarithm of a specified number in a specified base.</para>
		/// </summary>
		/// <param name="f"></param>
		/// <param name="p"></param>
		public static float Log(float f, float p)
		{
			return (float)Math.Log((double)f, (double)p);
		}

		/// <summary>
		///   <para>Returns the natural (base e) logarithm of a specified number.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Log(float f)
		{
			return (float)Math.Log((double)f);
		}

		/// <summary>
		///   <para>Returns the base 10 logarithm of a specified number.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Log10(float f)
		{
			return (float)Math.Log10((double)f);
		}

		/// <summary>
		///   <para>Returns the smallest integer greater to or equal to f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Ceil(float f)
		{
			return (float)Math.Ceiling((double)f);
		}

		/// <summary>
		///   <para>Returns the largest integer smaller to or equal to f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Floor(float f)
		{
			return (float)Math.Floor((double)f);
		}

		/// <summary>
		///   <para>Returns f rounded to the nearest integer.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Round(float f)
		{
			return (float)Math.Round((double)f);
		}

		/// <summary>
		///   <para>Returns the smallest integer greater to or equal to f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static int CeilToInt(float f)
		{
			return (int)Math.Ceiling((double)f);
		}

		/// <summary>
		///   <para>Returns the largest integer smaller to or equal to f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static int FloorToInt(float f)
		{
			return (int)Math.Floor((double)f);
		}

		/// <summary>
		///   <para>Returns f rounded to the nearest integer.</para>
		/// </summary>
		/// <param name="f"></param>
		public static int RoundToInt(float f)
		{
			return (int)Math.Round((double)f);
		}

		/// <summary>
		///   <para>Returns the sign of f.</para>
		/// </summary>
		/// <param name="f"></param>
		public static float Sign(float f)
		{
			return (f < 0f) ? -1f : 1f;
		}

		/// <summary>
		///   <para>Clamps a value between a minimum float and maximum float value.</para>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}
			return value;
		}

		/// <summary>
		///   <para>Clamps value between min and max and returns value.</para>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}
			return value;
		}

		/// <summary>
		///   <para>Clamps value between 0 and 1 and returns value.</para>
		/// </summary>
		/// <param name="value"></param>
		public static float Clamp01(float value)
		{
			if (value < 0f)
			{
				return 0f;
			}
			if (value > 1f)
			{
				return 1f;
			}
			return value;
		}

		/// <summary>
		///   <para>Linearly interpolates between a and b by t.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * Mathf.Clamp01(t);
		}

		/// <summary>
		///   <para>Linearly interpolates between a and b by t.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static float LerpUnclamped(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		/// <summary>
		///   <para>Same as Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static float LerpAngle(float a, float b, float t)
		{
			float num = Mathf.Repeat(b - a, 360f);
			if (num > 180f)
			{
				num -= 360f;
			}
			return a + num * Mathf.Clamp01(t);
		}

		/// <summary>
		///   <para>Moves a value current towards target.</para>
		/// </summary>
		/// <param name="current">The current value.</param>
		/// <param name="target">The value to move towards.</param>
		/// <param name="maxDelta">The maximum change that should be applied to the value.</param>
		public static float MoveTowards(float current, float target, float maxDelta)
		{
			if (Mathf.Abs(target - current) <= maxDelta)
			{
				return target;
			}
			return current + Mathf.Sign(target - current) * maxDelta;
		}

		/// <summary>
		///   <para>Same as MoveTowards but makes sure the values interpolate correctly when they wrap around 360 degrees.</para>
		/// </summary>
		/// <param name="current"></param>
		/// <param name="target"></param>
		/// <param name="maxDelta"></param>
		public static float MoveTowardsAngle(float current, float target, float maxDelta)
		{
			target = current + Mathf.DeltaAngle(current, target);
			return Mathf.MoveTowards(current, target, maxDelta);
		}

		/// <summary>
		///   <para>Interpolates between min and max with smoothing at the limits.</para>
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="t"></param>
		public static float SmoothStep(float from, float to, float t)
		{
			t = Mathf.Clamp01(t);
			t = -2f * t * t * t + 3f * t * t;
			return to * t + from * (1f - t);
		}

		public static float Gamma(float value, float absmax, float gamma)
		{
			bool flag = false;
			if (value < 0f)
			{
				flag = true;
			}
			float num = Mathf.Abs(value);
			if (num > absmax)
			{
				return (!flag) ? num : (-num);
			}
			float num2 = Mathf.Pow(num / absmax, gamma) * absmax;
			return (!flag) ? num2 : (-num2);
		}

		/// <summary>
		///   <para>Compares two floating point values if they are similar.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static bool Approximately(float a, float b)
		{
			return Mathf.Abs(b - a) < Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * 8f);
		}

		[ExcludeFromDocs]
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		[ExcludeFromDocs]
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float positiveInfinity = float.PositiveInfinity;
			return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
		}

		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			smoothTime = Mathf.Max(0.0001f, smoothTime);
			float num = 2f / smoothTime;
			float num2 = num * deltaTime;
			float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
			float num4 = current - target;
			float num5 = target;
			float num6 = maxSpeed * smoothTime;
			num4 = Mathf.Clamp(num4, -num6, num6);
			target = current - num4;
			float num7 = (currentVelocity + num * num4) * deltaTime;
			currentVelocity = (currentVelocity - num * num7) * num3;
			float num8 = target + (num4 + num7) * num3;
			if (num5 - current > 0f == num8 > num5)
			{
				num8 = num5;
				currentVelocity = (num8 - num5) / deltaTime;
			}
			return num8;
		}

		[ExcludeFromDocs]
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return Mathf.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		[ExcludeFromDocs]
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float positiveInfinity = float.PositiveInfinity;
			return Mathf.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
		}

		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			target = current + Mathf.DeltaAngle(current, target);
			return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		/// <summary>
		///   <para>Loops the value t, so that it is never larger than length and never smaller than 0.</para>
		/// </summary>
		/// <param name="t"></param>
		/// <param name="length"></param>
		public static float Repeat(float t, float length)
		{
			return t - Mathf.Floor(t / length) * length;
		}

		/// <summary>
		///   <para>PingPongs the value t, so that it is never larger than length and never smaller than 0.</para>
		/// </summary>
		/// <param name="t"></param>
		/// <param name="length"></param>
		public static float PingPong(float t, float length)
		{
			t = Mathf.Repeat(t, length * 2f);
			return length - Mathf.Abs(t - length);
		}

		/// <summary>
		///   <para>Calculates the linear parameter t that produces the interpolant value within the range [a, b].</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="value"></param>
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
			{
				return Mathf.Clamp01((value - a) / (b - a));
			}
			return 0f;
		}

		/// <summary>
		///   <para>Returns the closest power of two value.</para>
		/// </summary>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ClosestPowerOfTwo(int value);

		/// <summary>
		///   <para>Converts the given value from gamma to linear color space.</para>
		/// </summary>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GammaToLinearSpace(float value);

		/// <summary>
		///   <para>Converts the given value from linear to gamma color space.</para>
		/// </summary>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float LinearToGammaSpace(float value);

		/// <summary>
		///   <para>Returns true if the value is power of two.</para>
		/// </summary>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsPowerOfTwo(int value);

		/// <summary>
		///   <para>Returns the next power of two value.</para>
		/// </summary>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int NextPowerOfTwo(int value);

		/// <summary>
		///   <para>Calculates the shortest difference between two given angles given in degrees.</para>
		/// </summary>
		/// <param name="current"></param>
		/// <param name="target"></param>
		public static float DeltaAngle(float current, float target)
		{
			float num = Mathf.Repeat(target - current, 360f);
			if (num > 180f)
			{
				num -= 360f;
			}
			return num;
		}

		/// <summary>
		///   <para>Generate 2D Perlin noise.</para>
		/// </summary>
		/// <param name="x">X-coordinate of sample point.</param>
		/// <param name="y">Y-coordinate of sample point.</param>
		/// <returns>
		///   <para>Value between 0.0 and 1.0.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float PerlinNoise(float x, float y);

		internal static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = num * num4 - num2 * num3;
			if (num5 == 0f)
			{
				return false;
			}
			float num6 = p3.x - p1.x;
			float num7 = p3.y - p1.y;
			float num8 = (num6 * num4 - num7 * num3) / num5;
			result = new Vector2(p1.x + num8 * num, p1.y + num8 * num2);
			return true;
		}

		internal static bool LineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 result)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			float num3 = p4.x - p3.x;
			float num4 = p4.y - p3.y;
			float num5 = num * num4 - num2 * num3;
			if (num5 == 0f)
			{
				return false;
			}
			float num6 = p3.x - p1.x;
			float num7 = p3.y - p1.y;
			float num8 = (num6 * num4 - num7 * num3) / num5;
			if (num8 < 0f || num8 > 1f)
			{
				return false;
			}
			float num9 = (num6 * num2 - num7 * num) / num5;
			if (num9 < 0f || num9 > 1f)
			{
				return false;
			}
			result = new Vector2(p1.x + num8 * num, p1.y + num8 * num2);
			return true;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort FloatToHalf(float val);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float HalfToFloat(ushort val);

		internal static long RandomToLong(Random r)
		{
			byte[] array = new byte[8];
			r.NextBytes(array);
			return (long)(BitConverter.ToUInt64(array, 0) & 9223372036854775807UL);
		}
	}
}
