using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Character Joints are mainly used for Ragdoll effects.</para>
	/// </summary>
	public sealed class CharacterJoint : Joint
	{
		[Obsolete("TargetRotation not in use for Unity 5 and assumed disabled.", true)]
		public Quaternion targetRotation;

		[Obsolete("TargetAngularVelocity not in use for Unity 5 and assumed disabled.", true)]
		public Vector3 targetAngularVelocity;

		[Obsolete("RotationDrive not in use for Unity 5 and assumed disabled.", true)]
		public JointDrive rotationDrive;

		/// <summary>
		///   <para>The secondary axis around which the joint can rotate.</para>
		/// </summary>
		public Vector3 swingAxis
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_swingAxis(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_swingAxis(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_swingAxis(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_swingAxis(ref Vector3 value);

		/// <summary>
		///   <para>The configuration of the spring attached to the twist limits of the joint.</para>
		/// </summary>
		public SoftJointLimitSpring twistLimitSpring
		{
			get
			{
				SoftJointLimitSpring result;
				this.INTERNAL_get_twistLimitSpring(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_twistLimitSpring(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_twistLimitSpring(out SoftJointLimitSpring value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_twistLimitSpring(ref SoftJointLimitSpring value);

		/// <summary>
		///   <para>The configuration of the spring attached to the swing limits of the joint.</para>
		/// </summary>
		public SoftJointLimitSpring swingLimitSpring
		{
			get
			{
				SoftJointLimitSpring result;
				this.INTERNAL_get_swingLimitSpring(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_swingLimitSpring(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_swingLimitSpring(out SoftJointLimitSpring value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_swingLimitSpring(ref SoftJointLimitSpring value);

		/// <summary>
		///   <para>The lower limit around the primary axis of the character joint.</para>
		/// </summary>
		public SoftJointLimit lowTwistLimit
		{
			get
			{
				SoftJointLimit result;
				this.INTERNAL_get_lowTwistLimit(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_lowTwistLimit(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_lowTwistLimit(out SoftJointLimit value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_lowTwistLimit(ref SoftJointLimit value);

		/// <summary>
		///   <para>The upper limit around the primary axis of the character joint.</para>
		/// </summary>
		public SoftJointLimit highTwistLimit
		{
			get
			{
				SoftJointLimit result;
				this.INTERNAL_get_highTwistLimit(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_highTwistLimit(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_highTwistLimit(out SoftJointLimit value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_highTwistLimit(ref SoftJointLimit value);

		/// <summary>
		///   <para>The angular limit of rotation (in degrees) around the primary axis of the character joint.</para>
		/// </summary>
		public SoftJointLimit swing1Limit
		{
			get
			{
				SoftJointLimit result;
				this.INTERNAL_get_swing1Limit(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_swing1Limit(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_swing1Limit(out SoftJointLimit value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_swing1Limit(ref SoftJointLimit value);

		/// <summary>
		///   <para>The angular limit of rotation (in degrees) around the primary axis of the character joint.</para>
		/// </summary>
		public SoftJointLimit swing2Limit
		{
			get
			{
				SoftJointLimit result;
				this.INTERNAL_get_swing2Limit(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_swing2Limit(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_swing2Limit(out SoftJointLimit value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_swing2Limit(ref SoftJointLimit value);

		/// <summary>
		///   <para>Brings violated constraints back into alignment even when the solver fails.</para>
		/// </summary>
		public extern bool enableProjection { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set the linear tolerance threshold for projection.</para>
		/// </summary>
		public extern float projectionDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set the angular tolerance threshold (in degrees) for projection.</para>
		/// </summary>
		public extern float projectionAngle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
