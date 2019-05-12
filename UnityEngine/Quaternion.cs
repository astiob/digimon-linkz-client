using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Quaternions are used to represent rotations.</para>
	/// </summary>
	public struct Quaternion
	{
		public const float kEpsilon = 1E-06f;

		/// <summary>
		///   <para>X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
		/// </summary>
		public float x;

		/// <summary>
		///   <para>Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
		/// </summary>
		public float y;

		/// <summary>
		///   <para>Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
		/// </summary>
		public float z;

		/// <summary>
		///   <para>W component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
		/// </summary>
		public float w;

		/// <summary>
		///   <para>Constructs new Quaternion with given x,y,z,w components.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		public Quaternion(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.x;
				case 1:
					return this.y;
				case 2:
					return this.z;
				case 3:
					return this.w;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.x = value;
					break;
				case 1:
					this.y = value;
					break;
				case 2:
					this.z = value;
					break;
				case 3:
					this.w = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
			}
		}

		/// <summary>
		///   <para>Set x, y, z and w components of an existing Quaternion.</para>
		/// </summary>
		/// <param name="new_x"></param>
		/// <param name="new_y"></param>
		/// <param name="new_z"></param>
		/// <param name="new_w"></param>
		public void Set(float new_x, float new_y, float new_z, float new_w)
		{
			this.x = new_x;
			this.y = new_y;
			this.z = new_z;
			this.w = new_w;
		}

		/// <summary>
		///   <para>The identity rotation (Read Only).</para>
		/// </summary>
		public static Quaternion identity
		{
			get
			{
				return new Quaternion(0f, 0f, 0f, 1f);
			}
		}

		/// <summary>
		///   <para>The dot product between two rotations.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static float Dot(Quaternion a, Quaternion b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		/// <summary>
		///   <para>Creates a rotation which rotates angle degrees around axis.</para>
		/// </summary>
		/// <param name="angle"></param>
		/// <param name="axis"></param>
		public static Quaternion AngleAxis(float angle, Vector3 axis)
		{
			return Quaternion.INTERNAL_CALL_AngleAxis(angle, ref axis);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_AngleAxis(float angle, ref Vector3 axis);

		public void ToAngleAxis(out float angle, out Vector3 axis)
		{
			Quaternion.Internal_ToAxisAngleRad(this, out axis, out angle);
			angle *= 57.29578f;
		}

		/// <summary>
		///   <para>Creates a rotation which rotates from fromDirection to toDirection.</para>
		/// </summary>
		/// <param name="fromDirection"></param>
		/// <param name="toDirection"></param>
		public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			return Quaternion.INTERNAL_CALL_FromToRotation(ref fromDirection, ref toDirection);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_FromToRotation(ref Vector3 fromDirection, ref Vector3 toDirection);

		/// <summary>
		///   <para>Creates a rotation which rotates from fromDirection to toDirection.</para>
		/// </summary>
		/// <param name="fromDirection"></param>
		/// <param name="toDirection"></param>
		public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			this = Quaternion.FromToRotation(fromDirection, toDirection);
		}

		/// <summary>
		///   <para>Creates a rotation with the specified forward and upwards directions.</para>
		/// </summary>
		/// <param name="forward">The direction to look in.</param>
		/// <param name="upwards">The vector that defines in which direction up is.</param>
		public static Quaternion LookRotation(Vector3 forward, [DefaultValue("Vector3.up")] Vector3 upwards)
		{
			return Quaternion.INTERNAL_CALL_LookRotation(ref forward, ref upwards);
		}

		/// <summary>
		///   <para>Creates a rotation with the specified forward and upwards directions.</para>
		/// </summary>
		/// <param name="forward">The direction to look in.</param>
		/// <param name="upwards">The vector that defines in which direction up is.</param>
		[ExcludeFromDocs]
		public static Quaternion LookRotation(Vector3 forward)
		{
			Vector3 up = Vector3.up;
			return Quaternion.INTERNAL_CALL_LookRotation(ref forward, ref up);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_LookRotation(ref Vector3 forward, ref Vector3 upwards);

		/// <summary>
		///   <para>Creates a rotation with the specified forward and upwards directions.</para>
		/// </summary>
		/// <param name="view">The direction to look in.</param>
		/// <param name="up">The vector that defines in which direction up is.</param>
		[ExcludeFromDocs]
		public void SetLookRotation(Vector3 view)
		{
			Vector3 up = Vector3.up;
			this.SetLookRotation(view, up);
		}

		/// <summary>
		///   <para>Creates a rotation with the specified forward and upwards directions.</para>
		/// </summary>
		/// <param name="view">The direction to look in.</param>
		/// <param name="up">The vector that defines in which direction up is.</param>
		public void SetLookRotation(Vector3 view, [DefaultValue("Vector3.up")] Vector3 up)
		{
			this = Quaternion.LookRotation(view, up);
		}

		/// <summary>
		///   <para>Spherically interpolates between a and b by t. The parameter t is clamped to the range [0, 1].</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
		{
			return Quaternion.INTERNAL_CALL_Slerp(ref a, ref b, t);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_Slerp(ref Quaternion a, ref Quaternion b, float t);

		/// <summary>
		///   <para>Spherically interpolates between a and b by t. The parameter t is not clamped.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
		{
			return Quaternion.INTERNAL_CALL_SlerpUnclamped(ref a, ref b, t);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_SlerpUnclamped(ref Quaternion a, ref Quaternion b, float t);

		/// <summary>
		///   <para>Interpolates between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1].</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
		{
			return Quaternion.INTERNAL_CALL_Lerp(ref a, ref b, t);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_Lerp(ref Quaternion a, ref Quaternion b, float t);

		/// <summary>
		///   <para>Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
		{
			return Quaternion.INTERNAL_CALL_LerpUnclamped(ref a, ref b, t);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_LerpUnclamped(ref Quaternion a, ref Quaternion b, float t);

		/// <summary>
		///   <para>Rotates a rotation from towards to.</para>
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="maxDegreesDelta"></param>
		public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
		{
			float num = Quaternion.Angle(from, to);
			if (num == 0f)
			{
				return to;
			}
			float t = Mathf.Min(1f, maxDegreesDelta / num);
			return Quaternion.SlerpUnclamped(from, to, t);
		}

		/// <summary>
		///   <para>Returns the Inverse of rotation.</para>
		/// </summary>
		/// <param name="rotation"></param>
		public static Quaternion Inverse(Quaternion rotation)
		{
			return Quaternion.INTERNAL_CALL_Inverse(ref rotation);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_Inverse(ref Quaternion rotation);

		/// <summary>
		///   <para>Returns a nicely formatted string of the Quaternion.</para>
		/// </summary>
		/// <param name="format"></param>
		public override string ToString()
		{
			return UnityString.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", new object[]
			{
				this.x,
				this.y,
				this.z,
				this.w
			});
		}

		/// <summary>
		///   <para>Returns a nicely formatted string of the Quaternion.</para>
		/// </summary>
		/// <param name="format"></param>
		public string ToString(string format)
		{
			return UnityString.Format("({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString(format),
				this.y.ToString(format),
				this.z.ToString(format),
				this.w.ToString(format)
			});
		}

		/// <summary>
		///   <para>Returns the angle in degrees between two rotations a and b.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static float Angle(Quaternion a, Quaternion b)
		{
			float f = Quaternion.Dot(a, b);
			return Mathf.Acos(Mathf.Min(Mathf.Abs(f), 1f)) * 2f * 57.29578f;
		}

		/// <summary>
		///   <para>Returns the euler angle representation of the rotation.</para>
		/// </summary>
		public Vector3 eulerAngles
		{
			get
			{
				return Quaternion.Internal_ToEulerRad(this) * 57.29578f;
			}
			set
			{
				this = Quaternion.Internal_FromEulerRad(value * 0.0174532924f);
			}
		}

		/// <summary>
		///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public static Quaternion Euler(float x, float y, float z)
		{
			return Quaternion.Internal_FromEulerRad(new Vector3(x, y, z) * 0.0174532924f);
		}

		/// <summary>
		///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).</para>
		/// </summary>
		/// <param name="euler"></param>
		public static Quaternion Euler(Vector3 euler)
		{
			return Quaternion.Internal_FromEulerRad(euler * 0.0174532924f);
		}

		private static Vector3 Internal_ToEulerRad(Quaternion rotation)
		{
			return Quaternion.INTERNAL_CALL_Internal_ToEulerRad(ref rotation);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_Internal_ToEulerRad(ref Quaternion rotation);

		private static Quaternion Internal_FromEulerRad(Vector3 euler)
		{
			return Quaternion.INTERNAL_CALL_Internal_FromEulerRad(ref euler);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_Internal_FromEulerRad(ref Vector3 euler);

		private static void Internal_ToAxisAngleRad(Quaternion q, out Vector3 axis, out float angle)
		{
			Quaternion.INTERNAL_CALL_Internal_ToAxisAngleRad(ref q, out axis, out angle);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_ToAxisAngleRad(ref Quaternion q, out Vector3 axis, out float angle);

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public static Quaternion EulerRotation(float x, float y, float z)
		{
			return Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
		}

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public static Quaternion EulerRotation(Vector3 euler)
		{
			return Quaternion.Internal_FromEulerRad(euler);
		}

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public void SetEulerRotation(float x, float y, float z)
		{
			this = Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
		}

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public void SetEulerRotation(Vector3 euler)
		{
			this = Quaternion.Internal_FromEulerRad(euler);
		}

		[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees")]
		public Vector3 ToEuler()
		{
			return Quaternion.Internal_ToEulerRad(this);
		}

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public static Quaternion EulerAngles(float x, float y, float z)
		{
			return Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
		}

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public static Quaternion EulerAngles(Vector3 euler)
		{
			return Quaternion.Internal_FromEulerRad(euler);
		}

		[Obsolete("Use Quaternion.ToAngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
		public void ToAxisAngle(out Vector3 axis, out float angle)
		{
			Quaternion.Internal_ToAxisAngleRad(this, out axis, out angle);
		}

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public void SetEulerAngles(float x, float y, float z)
		{
			this.SetEulerRotation(new Vector3(x, y, z));
		}

		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
		public void SetEulerAngles(Vector3 euler)
		{
			this = Quaternion.EulerRotation(euler);
		}

		[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees")]
		public static Vector3 ToEulerAngles(Quaternion rotation)
		{
			return Quaternion.Internal_ToEulerRad(rotation);
		}

		[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees")]
		public Vector3 ToEulerAngles()
		{
			return Quaternion.Internal_ToEulerRad(this);
		}

		[Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
		public static Quaternion AxisAngle(Vector3 axis, float angle)
		{
			return Quaternion.INTERNAL_CALL_AxisAngle(ref axis, angle);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_AxisAngle(ref Vector3 axis, float angle);

		[Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
		public void SetAxisAngle(Vector3 axis, float angle)
		{
			this = Quaternion.AxisAngle(axis, angle);
		}

		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2 ^ this.w.GetHashCode() >> 1;
		}

		public override bool Equals(object other)
		{
			if (!(other is Quaternion))
			{
				return false;
			}
			Quaternion quaternion = (Quaternion)other;
			return this.x.Equals(quaternion.x) && this.y.Equals(quaternion.y) && this.z.Equals(quaternion.z) && this.w.Equals(quaternion.w);
		}

		public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
		{
			return new Quaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
		}

		public static Vector3 operator *(Quaternion rotation, Vector3 point)
		{
			float num = rotation.x * 2f;
			float num2 = rotation.y * 2f;
			float num3 = rotation.z * 2f;
			float num4 = rotation.x * num;
			float num5 = rotation.y * num2;
			float num6 = rotation.z * num3;
			float num7 = rotation.x * num2;
			float num8 = rotation.x * num3;
			float num9 = rotation.y * num3;
			float num10 = rotation.w * num;
			float num11 = rotation.w * num2;
			float num12 = rotation.w * num3;
			Vector3 result;
			result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
			result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
			result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
			return result;
		}

		public static bool operator ==(Quaternion lhs, Quaternion rhs)
		{
			return Quaternion.Dot(lhs, rhs) > 0.999999f;
		}

		public static bool operator !=(Quaternion lhs, Quaternion rhs)
		{
			return Quaternion.Dot(lhs, rhs) <= 0.999999f;
		}
	}
}
