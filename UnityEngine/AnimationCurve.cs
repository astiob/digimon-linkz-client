using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A collection of curves form an AnimationClip.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class AnimationCurve
	{
		internal IntPtr m_Ptr;

		/// <summary>
		///   <para>Creates an animation curve from arbitrary number of keyframes.</para>
		/// </summary>
		/// <param name="keys"></param>
		public AnimationCurve(params Keyframe[] keys)
		{
			this.Init(keys);
		}

		/// <summary>
		///   <para>Creates an empty animation curve.</para>
		/// </summary>
		public AnimationCurve()
		{
			this.Init(null);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		~AnimationCurve()
		{
			this.Cleanup();
		}

		/// <summary>
		///   <para>Evaluate the curve at time.</para>
		/// </summary>
		/// <param name="time">The time within the curve you want to evaluate (the horizontal axis in the curve graph).</param>
		/// <returns>
		///   <para>The value of the curve, at the point in time specified.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float Evaluate(float time);

		/// <summary>
		///   <para>All keys defined in the animation curve.</para>
		/// </summary>
		public Keyframe[] keys
		{
			get
			{
				return this.GetKeys();
			}
			set
			{
				this.SetKeys(value);
			}
		}

		/// <summary>
		///   <para>Add a new key to the curve.</para>
		/// </summary>
		/// <param name="time">The time at which to add the key (horizontal axis in the curve graph).</param>
		/// <param name="value">The value for the key (vertical axis in the curve graph).</param>
		/// <returns>
		///   <para>The index of the added key, or -1 if the key could not be added.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int AddKey(float time, float value);

		/// <summary>
		///   <para>Add a new key to the curve.</para>
		/// </summary>
		/// <param name="key">The key to add to the curve.</param>
		/// <returns>
		///   <para>The index of the added key, or -1 if the key could not be added.</para>
		/// </returns>
		public int AddKey(Keyframe key)
		{
			return this.AddKey_Internal(key);
		}

		private int AddKey_Internal(Keyframe key)
		{
			return AnimationCurve.INTERNAL_CALL_AddKey_Internal(this, ref key);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_AddKey_Internal(AnimationCurve self, ref Keyframe key);

		/// <summary>
		///   <para>Removes the keyframe at index and inserts key.</para>
		/// </summary>
		/// <param name="index">The index of the key to move.</param>
		/// <param name="key">The key (with its new time) to insert.</param>
		/// <returns>
		///   <para>The index of the keyframe after moving it.</para>
		/// </returns>
		public int MoveKey(int index, Keyframe key)
		{
			return AnimationCurve.INTERNAL_CALL_MoveKey(this, index, ref key);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_MoveKey(AnimationCurve self, int index, ref Keyframe key);

		/// <summary>
		///   <para>Removes a key.</para>
		/// </summary>
		/// <param name="index">The index of the key to remove.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveKey(int index);

		public Keyframe this[int index]
		{
			get
			{
				return this.GetKey_Internal(index);
			}
		}

		/// <summary>
		///   <para>The number of keys in the curve. (Read Only)</para>
		/// </summary>
		public extern int length { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetKeys(Keyframe[] keys);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Keyframe GetKey_Internal(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Keyframe[] GetKeys();

		/// <summary>
		///   <para>Smooth the in and out tangents of the keyframe at index.</para>
		/// </summary>
		/// <param name="index">The index of the keyframe to be smoothed.</param>
		/// <param name="weight">The smoothing weight to apply to the keyframe's tangents.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SmoothTangents(int index, float weight);

		/// <summary>
		///   <para>A straight Line starting at timeStart, valueStart and ending at timeEnd, valueEnd.</para>
		/// </summary>
		/// <param name="timeStart">The start time for the linear curve.</param>
		/// <param name="valueStart">The start value for the linear curve.</param>
		/// <param name="timeEnd">The end time for the linear curve.</param>
		/// <param name="valueEnd">The end value for the linear curve.</param>
		/// <returns>
		///   <para>The (straight) curve created from the values specified.</para>
		/// </returns>
		public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			float num = (valueEnd - valueStart) / (timeEnd - timeStart);
			Keyframe[] keys = new Keyframe[]
			{
				new Keyframe(timeStart, valueStart, 0f, num),
				new Keyframe(timeEnd, valueEnd, num, 0f)
			};
			return new AnimationCurve(keys);
		}

		/// <summary>
		///   <para>Creates an ease-in and out curve starting at timeStart, valueStart and ending at timeEnd, valueEnd.</para>
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>
		///   <para>The ease-in and out curve generated from the specified values.</para>
		/// </returns>
		public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			Keyframe[] keys = new Keyframe[]
			{
				new Keyframe(timeStart, valueStart, 0f, 0f),
				new Keyframe(timeEnd, valueEnd, 0f, 0f)
			};
			return new AnimationCurve(keys);
		}

		/// <summary>
		///   <para>The behaviour of the animation before the first keyframe.</para>
		/// </summary>
		public extern WrapMode preWrapMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The behaviour of the animation after the last keyframe.</para>
		/// </summary>
		public extern WrapMode postWrapMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init(Keyframe[] keys);
	}
}
