using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Director;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Interface to control the Mecanim animation system.</para>
	/// </summary>
	public sealed class Animator : DirectorPlayer, IAnimatorControllerPlayable
	{
		/// <summary>
		///   <para>Returns true if the current rig is optimizable with AnimatorUtility.OptimizeTransformHierarchy.</para>
		/// </summary>
		public extern bool isOptimizable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns true if the current rig is humanoid, false if it is generic.</para>
		/// </summary>
		public extern bool isHuman { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns true if the current rig has root motion.</para>
		/// </summary>
		public extern bool hasRootMotion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern bool isRootPositionOrRotationControlledByCurves { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the scale of the current Avatar for a humanoid rig, (1 by default if the rig is generic).</para>
		/// </summary>
		public extern float humanScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns whether the animator is initialized successfully.</para>
		/// </summary>
		public extern bool isInitialized { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public float GetFloat(string name)
		{
			return this.GetFloatString(name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public float GetFloat(int id)
		{
			return this.GetFloatID(id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dampTime"></param>
		/// <param name="deltaTime"></param>
		/// <param name="id"></param>
		public void SetFloat(string name, float value)
		{
			this.SetFloatString(name, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dampTime"></param>
		/// <param name="deltaTime"></param>
		/// <param name="id"></param>
		public void SetFloat(string name, float value, float dampTime, float deltaTime)
		{
			this.SetFloatStringDamp(name, value, dampTime, deltaTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dampTime"></param>
		/// <param name="deltaTime"></param>
		/// <param name="id"></param>
		public void SetFloat(int id, float value)
		{
			this.SetFloatID(id, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dampTime"></param>
		/// <param name="deltaTime"></param>
		/// <param name="id"></param>
		public void SetFloat(int id, float value, float dampTime, float deltaTime)
		{
			this.SetFloatIDDamp(id, value, dampTime, deltaTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public bool GetBool(string name)
		{
			return this.GetBoolString(name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public bool GetBool(int id)
		{
			return this.GetBoolID(id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetBool(string name, bool value)
		{
			this.SetBoolString(name, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetBool(int id, bool value)
		{
			this.SetBoolID(id, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public int GetInteger(string name)
		{
			return this.GetIntegerString(name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public int GetInteger(int id)
		{
			return this.GetIntegerID(id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetInteger(string name, int value)
		{
			this.SetIntegerString(name, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetInteger(int id, int value)
		{
			this.SetIntegerID(id, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public void SetTrigger(string name)
		{
			this.SetTriggerString(name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public void SetTrigger(int id)
		{
			this.SetTriggerID(id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.ResetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public void ResetTrigger(string name)
		{
			this.ResetTriggerString(name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.ResetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public void ResetTrigger(int id)
		{
			this.ResetTriggerID(id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.IsParameterControlledByCurve.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public bool IsParameterControlledByCurve(string name)
		{
			return this.IsParameterControlledByCurveString(name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.IsParameterControlledByCurve.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public bool IsParameterControlledByCurve(int id)
		{
			return this.IsParameterControlledByCurveID(id);
		}

		/// <summary>
		///   <para>Gets the avatar delta position for the last evaluated frame.</para>
		/// </summary>
		public Vector3 deltaPosition
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_deltaPosition(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_deltaPosition(out Vector3 value);

		/// <summary>
		///   <para>Gets the avatar delta rotation for the last evaluated frame.</para>
		/// </summary>
		public Quaternion deltaRotation
		{
			get
			{
				Quaternion result;
				this.INTERNAL_get_deltaRotation(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_deltaRotation(out Quaternion value);

		/// <summary>
		///   <para>Gets the avatar velocity  for the last evaluated frame.</para>
		/// </summary>
		public Vector3 velocity
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_velocity(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_velocity(out Vector3 value);

		/// <summary>
		///   <para>Gets the avatar angular velocity for the last evaluated frame.</para>
		/// </summary>
		public Vector3 angularVelocity
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_angularVelocity(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_angularVelocity(out Vector3 value);

		/// <summary>
		///   <para>The root position, the position of the game object.</para>
		/// </summary>
		public Vector3 rootPosition
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_rootPosition(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rootPosition(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rootPosition(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rootPosition(ref Vector3 value);

		/// <summary>
		///   <para>The root rotation, the rotation of the game object.</para>
		/// </summary>
		public Quaternion rootRotation
		{
			get
			{
				Quaternion result;
				this.INTERNAL_get_rootRotation(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rootRotation(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rootRotation(out Quaternion value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rootRotation(ref Quaternion value);

		/// <summary>
		///   <para>Should root motion be applied?</para>
		/// </summary>
		public extern bool applyRootMotion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>When linearVelocityBlending is set to true, the root motion velocity and angular velocity will be blended linearly.</para>
		/// </summary>
		public extern bool linearVelocityBlending { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>When turned on, animations will be executed in the physics loop. This is only useful in conjunction with kinematic rigidbodies.</para>
		/// </summary>
		[Obsolete("Use Animator.updateMode instead")]
		public bool animatePhysics
		{
			get
			{
				return this.updateMode == AnimatorUpdateMode.AnimatePhysics;
			}
			set
			{
				this.updateMode = ((!value) ? AnimatorUpdateMode.Normal : AnimatorUpdateMode.AnimatePhysics);
			}
		}

		/// <summary>
		///   <para>Specifies the update mode of the Animator.</para>
		/// </summary>
		public extern AnimatorUpdateMode updateMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns true if the object has a transform hierarchy.</para>
		/// </summary>
		public extern bool hasTransformHierarchy { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern bool allowConstantClipSamplingOptimization { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The current gravity weight based on current animations that are played.</para>
		/// </summary>
		public extern float gravityWeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The position of the body center of mass.</para>
		/// </summary>
		public Vector3 bodyPosition
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_bodyPosition(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_bodyPosition(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_bodyPosition(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_bodyPosition(ref Vector3 value);

		/// <summary>
		///   <para>The rotation of the body center of mass.</para>
		/// </summary>
		public Quaternion bodyRotation
		{
			get
			{
				Quaternion result;
				this.INTERNAL_get_bodyRotation(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_bodyRotation(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_bodyRotation(out Quaternion value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_bodyRotation(ref Quaternion value);

		/// <summary>
		///   <para>Gets the position of an IK goal.</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is queried.</param>
		/// <returns>
		///   <para>Return the current position of this IK goal in world space.</para>
		/// </returns>
		public Vector3 GetIKPosition(AvatarIKGoal goal)
		{
			this.CheckIfInIKPass();
			return this.GetIKPositionInternal(goal);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Vector3 GetIKPositionInternal(AvatarIKGoal goal);

		/// <summary>
		///   <para>Sets the position of an IK goal.</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is set.</param>
		/// <param name="goalPosition">The position in world space.</param>
		public void SetIKPosition(AvatarIKGoal goal, Vector3 goalPosition)
		{
			this.CheckIfInIKPass();
			this.SetIKPositionInternal(goal, goalPosition);
		}

		internal void SetIKPositionInternal(AvatarIKGoal goal, Vector3 goalPosition)
		{
			Animator.INTERNAL_CALL_SetIKPositionInternal(this, goal, ref goalPosition);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetIKPositionInternal(Animator self, AvatarIKGoal goal, ref Vector3 goalPosition);

		/// <summary>
		///   <para>Gets the rotation of an IK goal.</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is is queried.</param>
		public Quaternion GetIKRotation(AvatarIKGoal goal)
		{
			this.CheckIfInIKPass();
			return this.GetIKRotationInternal(goal);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Quaternion GetIKRotationInternal(AvatarIKGoal goal);

		/// <summary>
		///   <para>Sets the rotation of an IK goal.</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is set.</param>
		/// <param name="goalRotation">The rotation in world space.</param>
		public void SetIKRotation(AvatarIKGoal goal, Quaternion goalRotation)
		{
			this.CheckIfInIKPass();
			this.SetIKRotationInternal(goal, goalRotation);
		}

		internal void SetIKRotationInternal(AvatarIKGoal goal, Quaternion goalRotation)
		{
			Animator.INTERNAL_CALL_SetIKRotationInternal(this, goal, ref goalRotation);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetIKRotationInternal(Animator self, AvatarIKGoal goal, ref Quaternion goalRotation);

		/// <summary>
		///   <para>Gets the translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal).</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is queried.</param>
		public float GetIKPositionWeight(AvatarIKGoal goal)
		{
			this.CheckIfInIKPass();
			return this.GetIKPositionWeightInternal(goal);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetIKPositionWeightInternal(AvatarIKGoal goal);

		/// <summary>
		///   <para>Sets the translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal).</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is set.</param>
		/// <param name="value">The translative weight.</param>
		public void SetIKPositionWeight(AvatarIKGoal goal, float value)
		{
			this.CheckIfInIKPass();
			this.SetIKPositionWeightInternal(goal, value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetIKPositionWeightInternal(AvatarIKGoal goal, float value);

		/// <summary>
		///   <para>Gets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal).</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is queried.</param>
		public float GetIKRotationWeight(AvatarIKGoal goal)
		{
			this.CheckIfInIKPass();
			return this.GetIKRotationWeightInternal(goal);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetIKRotationWeightInternal(AvatarIKGoal goal);

		/// <summary>
		///   <para>Sets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal).</para>
		/// </summary>
		/// <param name="goal">The AvatarIKGoal that is set.</param>
		/// <param name="value">The rotational weight.</param>
		public void SetIKRotationWeight(AvatarIKGoal goal, float value)
		{
			this.CheckIfInIKPass();
			this.SetIKRotationWeightInternal(goal, value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetIKRotationWeightInternal(AvatarIKGoal goal, float value);

		/// <summary>
		///   <para>Gets the position of an IK hint.</para>
		/// </summary>
		/// <param name="hint">The AvatarIKHint that is queried.</param>
		/// <returns>
		///   <para>Return the current position of this IK hint in world space.</para>
		/// </returns>
		public Vector3 GetIKHintPosition(AvatarIKHint hint)
		{
			this.CheckIfInIKPass();
			return this.GetIKHintPositionInternal(hint);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Vector3 GetIKHintPositionInternal(AvatarIKHint hint);

		/// <summary>
		///   <para>Sets the position of an IK hint.</para>
		/// </summary>
		/// <param name="hint">The AvatarIKHint that is set.</param>
		/// <param name="hintPosition">The position in world space.</param>
		public void SetIKHintPosition(AvatarIKHint hint, Vector3 hintPosition)
		{
			this.CheckIfInIKPass();
			this.SetIKHintPositionInternal(hint, hintPosition);
		}

		internal void SetIKHintPositionInternal(AvatarIKHint hint, Vector3 hintPosition)
		{
			Animator.INTERNAL_CALL_SetIKHintPositionInternal(this, hint, ref hintPosition);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetIKHintPositionInternal(Animator self, AvatarIKHint hint, ref Vector3 hintPosition);

		/// <summary>
		///   <para>Gets the translative weight of an IK Hint (0 = at the original animation before IK, 1 = at the hint).</para>
		/// </summary>
		/// <param name="hint">The AvatarIKHint that is queried.</param>
		/// <returns>
		///   <para>Return translative weight.</para>
		/// </returns>
		public float GetIKHintPositionWeight(AvatarIKHint hint)
		{
			this.CheckIfInIKPass();
			return this.GetHintWeightPositionInternal(hint);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetHintWeightPositionInternal(AvatarIKHint hint);

		/// <summary>
		///   <para>Sets the translative weight of an IK hint (0 = at the original animation before IK, 1 = at the hint).</para>
		/// </summary>
		/// <param name="hint">The AvatarIKHint that is set.</param>
		/// <param name="value">The translative weight.</param>
		public void SetIKHintPositionWeight(AvatarIKHint hint, float value)
		{
			this.CheckIfInIKPass();
			this.SetIKHintPositionWeightInternal(hint, value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetIKHintPositionWeightInternal(AvatarIKHint hint, float value);

		/// <summary>
		///   <para>Sets the look at position.</para>
		/// </summary>
		/// <param name="lookAtPosition">The position to lookAt.</param>
		public void SetLookAtPosition(Vector3 lookAtPosition)
		{
			this.CheckIfInIKPass();
			this.SetLookAtPositionInternal(lookAtPosition);
		}

		internal void SetLookAtPositionInternal(Vector3 lookAtPosition)
		{
			Animator.INTERNAL_CALL_SetLookAtPositionInternal(this, ref lookAtPosition);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetLookAtPositionInternal(Animator self, ref Vector3 lookAtPosition);

		/// <summary>
		///   <para>Set look at weights.</para>
		/// </summary>
		/// <param name="weight">(0-1) the global weight of the LookAt, multiplier for other parameters.</param>
		/// <param name="bodyWeight">(0-1) determines how much the body is involved in the LookAt.</param>
		/// <param name="headWeight">(0-1) determines how much the head is involved in the LookAt.</param>
		/// <param name="eyesWeight">(0-1) determines how much the eyes are involved in the LookAt.</param>
		/// <param name="clampWeight">(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).</param>
		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight, float eyesWeight)
		{
			float clampWeight = 0.5f;
			this.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		/// <summary>
		///   <para>Set look at weights.</para>
		/// </summary>
		/// <param name="weight">(0-1) the global weight of the LookAt, multiplier for other parameters.</param>
		/// <param name="bodyWeight">(0-1) determines how much the body is involved in the LookAt.</param>
		/// <param name="headWeight">(0-1) determines how much the head is involved in the LookAt.</param>
		/// <param name="eyesWeight">(0-1) determines how much the eyes are involved in the LookAt.</param>
		/// <param name="clampWeight">(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).</param>
		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			this.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		/// <summary>
		///   <para>Set look at weights.</para>
		/// </summary>
		/// <param name="weight">(0-1) the global weight of the LookAt, multiplier for other parameters.</param>
		/// <param name="bodyWeight">(0-1) determines how much the body is involved in the LookAt.</param>
		/// <param name="headWeight">(0-1) determines how much the head is involved in the LookAt.</param>
		/// <param name="eyesWeight">(0-1) determines how much the eyes are involved in the LookAt.</param>
		/// <param name="clampWeight">(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).</param>
		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			this.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		/// <summary>
		///   <para>Set look at weights.</para>
		/// </summary>
		/// <param name="weight">(0-1) the global weight of the LookAt, multiplier for other parameters.</param>
		/// <param name="bodyWeight">(0-1) determines how much the body is involved in the LookAt.</param>
		/// <param name="headWeight">(0-1) determines how much the head is involved in the LookAt.</param>
		/// <param name="eyesWeight">(0-1) determines how much the eyes are involved in the LookAt.</param>
		/// <param name="clampWeight">(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).</param>
		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			float bodyWeight = 0f;
			this.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		/// <summary>
		///   <para>Set look at weights.</para>
		/// </summary>
		/// <param name="weight">(0-1) the global weight of the LookAt, multiplier for other parameters.</param>
		/// <param name="bodyWeight">(0-1) determines how much the body is involved in the LookAt.</param>
		/// <param name="headWeight">(0-1) determines how much the head is involved in the LookAt.</param>
		/// <param name="eyesWeight">(0-1) determines how much the eyes are involved in the LookAt.</param>
		/// <param name="clampWeight">(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).</param>
		public void SetLookAtWeight(float weight, [DefaultValue("0.00f")] float bodyWeight, [DefaultValue("1.00f")] float headWeight, [DefaultValue("0.00f")] float eyesWeight, [DefaultValue("0.50f")] float clampWeight)
		{
			this.CheckIfInIKPass();
			this.SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetLookAtWeightInternal(float weight, [DefaultValue("0.00f")] float bodyWeight, [DefaultValue("1.00f")] float headWeight, [DefaultValue("0.00f")] float eyesWeight, [DefaultValue("0.50f")] float clampWeight);

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight, float headWeight, float eyesWeight)
		{
			float clampWeight = 0.5f;
			this.SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight, float headWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			this.SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			this.SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			float bodyWeight = 0f;
			this.SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		/// <summary>
		///   <para>Sets local rotation of a human bone during a IK pass.</para>
		/// </summary>
		/// <param name="humanBoneId">The human bone Id.</param>
		/// <param name="rotation">The local rotation.</param>
		public void SetBoneLocalRotation(HumanBodyBones humanBoneId, Quaternion rotation)
		{
			this.CheckIfInIKPass();
			this.SetBoneLocalRotationInternal(humanBoneId, rotation);
		}

		internal void SetBoneLocalRotationInternal(HumanBodyBones humanBoneId, Quaternion rotation)
		{
			Animator.INTERNAL_CALL_SetBoneLocalRotationInternal(this, humanBoneId, ref rotation);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetBoneLocalRotationInternal(Animator self, HumanBodyBones humanBoneId, ref Quaternion rotation);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern ScriptableObject GetBehaviour(Type type);

		public T GetBehaviour<T>() where T : StateMachineBehaviour
		{
			return this.GetBehaviour(typeof(T)) as T;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern ScriptableObject[] GetBehaviours(Type type);

		internal static T[] ConvertStateMachineBehaviour<T>(ScriptableObject[] rawObjects) where T : StateMachineBehaviour
		{
			if (rawObjects == null)
			{
				return null;
			}
			T[] array = new T[rawObjects.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (T)((object)rawObjects[i]);
			}
			return array;
		}

		public T[] GetBehaviours<T>() where T : StateMachineBehaviour
		{
			return Animator.ConvertStateMachineBehaviour<T>(this.GetBehaviours(typeof(T)));
		}

		/// <summary>
		///   <para>Automatic stabilization of feet during transition and blending.</para>
		/// </summary>
		public extern bool stabilizeFeet { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.layerCount.</para>
		/// </summary>
		public extern int layerCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetLayerName.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetLayerName(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetLayerIndex.</para>
		/// </summary>
		/// <param name="layerName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLayerIndex(string layerName);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetLayerWeight.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetLayerWeight(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetLayerWeight.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		/// <param name="weight"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetLayerWeight(int layerIndex, float weight);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetCurrentAnimatorStateInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetNextAnimatorStateInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetAnimatorTransitionInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetCurrentAnimatorClipInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetNextAnimatorClipInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.IsInTransition.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsInTransition(int layerIndex);

		/// <summary>
		///   <para>Read only acces to the AnimatorControllerParameters used by the animator.</para>
		/// </summary>
		public extern AnimatorControllerParameter[] parameters { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.parameterCount.</para>
		/// </summary>
		public extern int parameterCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>See AnimatorController.GetParameter.</para>
		/// </summary>
		/// <param name="index"></param>
		public AnimatorControllerParameter GetParameter(int index)
		{
			AnimatorControllerParameter[] parameters = this.parameters;
			if (index < 0 && index >= this.parameters.Length)
			{
				throw new IndexOutOfRangeException("index");
			}
			return parameters[index];
		}

		/// <summary>
		///   <para>Blends pivot point between body center of mass and feet pivot. At 0%, the blending point is body center of mass. At 100%, the blending point is feet pivot.</para>
		/// </summary>
		public extern float feetPivotActive { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Gets the pivot weight.</para>
		/// </summary>
		public extern float pivotWeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Get the current position of the pivot.</para>
		/// </summary>
		public Vector3 pivotPosition
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_pivotPosition(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_pivotPosition(out Vector3 value);

		/// <summary>
		///   <para>Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress.</para>
		/// </summary>
		/// <param name="matchPosition">The position we want the body part to reach.</param>
		/// <param name="matchRotation">The rotation in which we want the body part to be.</param>
		/// <param name="targetBodyPart">The body part that is involved in the match.</param>
		/// <param name="weightMask">Structure that contains weights for matching position and rotation.</param>
		/// <param name="startNormalizedTime">Start time within the animation clip (0 - beginning of clip, 1 - end of clip).</param>
		/// <param name="targetNormalizedTime">End time within the animation clip (0 - beginning of clip, 1 - end of clip), values greater than 1 can be set to trigger a match after a certain number of loops. Ex: 2.3 means at 30% of 2nd loop.</param>
		public void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget targetBodyPart, MatchTargetWeightMask weightMask, float startNormalizedTime, [DefaultValue("1")] float targetNormalizedTime)
		{
			Animator.INTERNAL_CALL_MatchTarget(this, ref matchPosition, ref matchRotation, targetBodyPart, ref weightMask, startNormalizedTime, targetNormalizedTime);
		}

		[ExcludeFromDocs]
		public void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget targetBodyPart, MatchTargetWeightMask weightMask, float startNormalizedTime)
		{
			float targetNormalizedTime = 1f;
			Animator.INTERNAL_CALL_MatchTarget(this, ref matchPosition, ref matchRotation, targetBodyPart, ref weightMask, startNormalizedTime, targetNormalizedTime);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_MatchTarget(Animator self, ref Vector3 matchPosition, ref Quaternion matchRotation, AvatarTarget targetBodyPart, ref MatchTargetWeightMask weightMask, float startNormalizedTime, float targetNormalizedTime);

		/// <summary>
		///   <para>Interrupts the automatic target matching.</para>
		/// </summary>
		/// <param name="completeMatch"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InterruptMatchTarget([DefaultValue("true")] bool completeMatch);

		[ExcludeFromDocs]
		public void InterruptMatchTarget()
		{
			bool completeMatch = true;
			this.InterruptMatchTarget(completeMatch);
		}

		/// <summary>
		///   <para>If automatic matching is active.</para>
		/// </summary>
		public extern bool isMatchingTarget { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The playback speed of the Animator. 1 is normal playback speed.</para>
		/// </summary>
		public extern float speed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("ForceStateNormalizedTime is deprecated. Please use Play or CrossFade instead.")]
		public void ForceStateNormalizedTime(float normalizedTime)
		{
			this.Play(0, 0, normalizedTime);
		}

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			this.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(string stateName, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			this.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFadeInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		public void CrossFadeInFixedTime(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
		{
			this.CrossFadeInFixedTime(Animator.StringToHash(stateName), transitionDuration, layer, fixedTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFadeInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime);

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			this.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			this.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void CrossFade(string stateName, float transitionDuration, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.CrossFade(stateName, transitionDuration, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void CrossFade(string stateName, float transitionDuration)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.CrossFade(stateName, transitionDuration, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFade.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		public void CrossFade(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			this.CrossFade(Animator.StringToHash(stateName), transitionDuration, layer, normalizedTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFade.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CrossFade(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime);

		[ExcludeFromDocs]
		public void CrossFade(int stateNameHash, float transitionDuration, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.CrossFade(stateNameHash, transitionDuration, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void CrossFade(int stateNameHash, float transitionDuration)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.CrossFade(stateNameHash, transitionDuration, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void PlayInFixedTime(string stateName, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.PlayInFixedTime(stateName, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void PlayInFixedTime(string stateName)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.PlayInFixedTime(stateName, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.PlayInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		public void PlayInFixedTime(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
		{
			this.PlayInFixedTime(Animator.StringToHash(stateName), layer, fixedTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.PlayInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void PlayInFixedTime(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime);

		[ExcludeFromDocs]
		public void PlayInFixedTime(int stateNameHash, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.PlayInFixedTime(stateNameHash, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void PlayInFixedTime(int stateNameHash)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.PlayInFixedTime(stateNameHash, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void Play(string stateName, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.Play(stateName, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void Play(string stateName)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.Play(stateName, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.Play.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		public void Play(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			this.Play(Animator.StringToHash(stateName), layer, normalizedTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.Play.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Play(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime);

		[ExcludeFromDocs]
		public void Play(int stateNameHash, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.Play(stateNameHash, layer, negativeInfinity);
		}

		[ExcludeFromDocs]
		public void Play(int stateNameHash)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.Play(stateNameHash, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>Sets an AvatarTarget and a targetNormalizedTime for the current state.</para>
		/// </summary>
		/// <param name="targetIndex">The avatar body part that is queried.</param>
		/// <param name="targetNormalizedTime">The current state Time that is queried.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTarget(AvatarTarget targetIndex, float targetNormalizedTime);

		/// <summary>
		///   <para>Returns the position of the target specified by SetTarget(AvatarTarget targetIndex, float targetNormalizedTime)).</para>
		/// </summary>
		public Vector3 targetPosition
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_targetPosition(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_targetPosition(out Vector3 value);

		/// <summary>
		///   <para>Returns the rotation of the target specified by SetTarget(AvatarTarget targetIndex, float targetNormalizedTime)).</para>
		/// </summary>
		public Quaternion targetRotation
		{
			get
			{
				Quaternion result;
				this.INTERNAL_get_targetRotation(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_targetRotation(out Quaternion value);

		/// <summary>
		///   <para>Returns true if the transform is controlled by the Animator\.</para>
		/// </summary>
		/// <param name="transform">The transform that is queried.</param>
		[WrapperlessIcall]
		[Obsolete("use mask and layers to control subset of transfroms in a skeleton", true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsControlled(Transform transform);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsBoneTransform(Transform transform);

		internal extern Transform avatarRoot { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns transform mapped to this human bone id.</para>
		/// </summary>
		/// <param name="humanBoneId">The human bone that is queried, see enum HumanBodyBones for a list of possible values.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Transform GetBoneTransform(HumanBodyBones humanBoneId);

		/// <summary>
		///   <para>Controls culling of this Animator component.</para>
		/// </summary>
		public extern AnimatorCullingMode cullingMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Sets the animator in playback mode.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StartPlayback();

		/// <summary>
		///   <para>Stops the animator playback mode. When playback stops, the avatar resumes getting control from game logic.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopPlayback();

		/// <summary>
		///   <para>Sets the playback position in the recording buffer.</para>
		/// </summary>
		public extern float playbackTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Sets the animator in recording mode, and allocates a circular buffer of size frameCount.</para>
		/// </summary>
		/// <param name="frameCount">The number of frames (updates) that will be recorded. If frameCount is 0, the recording will continue until the user calls StopRecording. The maximum value for frameCount is 10000.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StartRecording(int frameCount);

		/// <summary>
		///   <para>Stops animator record mode.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopRecording();

		/// <summary>
		///   <para>Start time of the first frame of the buffer relative to the frame at which StartRecording was called.</para>
		/// </summary>
		public extern float recorderStartTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>End time of the recorded clip relative to when StartRecording was called.</para>
		/// </summary>
		public extern float recorderStopTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Gets the mode of the Animator recorder.</para>
		/// </summary>
		public extern AnimatorRecorderMode recorderMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The runtime representation of AnimatorController that controls the Animator.</para>
		/// </summary>
		public extern RuntimeAnimatorController runtimeAnimatorController { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.HasState.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		/// <param name="stateID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasState(int layerIndex, int stateID);

		/// <summary>
		///   <para>Generates an parameter id from a string.</para>
		/// </summary>
		/// <param name="name">The string to convert to Id.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int StringToHash(string name);

		/// <summary>
		///   <para>Gets/Sets the current Avatar.</para>
		/// </summary>
		public extern Avatar avatar { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetStats();

		private void CheckIfInIKPass()
		{
			if (this.logWarnings && !this.CheckIfInIKPassInternal())
			{
				Debug.LogWarning("Setting and getting IK Goals, Lookat and BoneLocalRotation should only be done in OnAnimatorIK or OnStateIK");
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool CheckIfInIKPassInternal();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFloatString(string name, float value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFloatID(int id, float value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float GetFloatString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float GetFloatID(int id);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetBoolString(string name, bool value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetBoolID(int id, bool value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetBoolString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetBoolID(int id);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetIntegerString(string name, int value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetIntegerID(int id, int value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetIntegerString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetIntegerID(int id);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTriggerString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTriggerID(int id);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResetTriggerString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResetTriggerID(int id);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsParameterControlledByCurveString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsParameterControlledByCurveID(int id);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFloatStringDamp(string name, float value, float dampTime, float deltaTime);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFloatIDDamp(int id, float value, float dampTime, float deltaTime);

		/// <summary>
		///   <para>Additional layers affects the center of mass.</para>
		/// </summary>
		public extern bool layersAffectMassCenter { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Get left foot bottom height.</para>
		/// </summary>
		public extern float leftFeetBottomHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Get right foot bottom height.</para>
		/// </summary>
		public extern float rightFeetBottomHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Evaluates the animator based on deltaTime.</para>
		/// </summary>
		/// <param name="deltaTime">The time delta.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Update(float deltaTime);

		/// <summary>
		///   <para>Rebind all the animated properties and mesh data with the Animator.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Rebind();

		/// <summary>
		///   <para>Apply the default Root Motion.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ApplyBuiltinRootMotion();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string ResolveHash(int hash);

		public extern bool logWarnings { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool fireEvents { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Gets the value of a vector parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		[Obsolete("GetVector is deprecated.")]
		public Vector3 GetVector(string name)
		{
			return Vector3.zero;
		}

		/// <summary>
		///   <para>Gets the value of a vector parameter.</para>
		/// </summary>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		[Obsolete("GetVector is deprecated.")]
		public Vector3 GetVector(int id)
		{
			return Vector3.zero;
		}

		/// <summary>
		///   <para>Sets the value of a vector parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		[Obsolete("SetVector is deprecated.")]
		public void SetVector(string name, Vector3 value)
		{
		}

		/// <summary>
		///   <para>Sets the value of a vector parameter.</para>
		/// </summary>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		/// <param name="value">The new value for the parameter.</param>
		[Obsolete("SetVector is deprecated.")]
		public void SetVector(int id, Vector3 value)
		{
		}

		/// <summary>
		///   <para>Gets the value of a quaternion parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		[Obsolete("GetQuaternion is deprecated.")]
		public Quaternion GetQuaternion(string name)
		{
			return Quaternion.identity;
		}

		/// <summary>
		///   <para>Gets the value of a quaternion parameter.</para>
		/// </summary>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		[Obsolete("GetQuaternion is deprecated.")]
		public Quaternion GetQuaternion(int id)
		{
			return Quaternion.identity;
		}

		/// <summary>
		///   <para>Sets the value of a quaternion parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		[Obsolete("SetQuaternion is deprecated.")]
		public void SetQuaternion(string name, Quaternion value)
		{
		}

		/// <summary>
		///   <para>Sets the value of a quaternion parameter.</para>
		/// </summary>
		/// <param name="id">Of the parameter. The id is generated using Animator::StringToHash.</param>
		/// <param name="value">The new value for the parameter.</param>
		[Obsolete("SetQuaternion is deprecated.")]
		public void SetQuaternion(int id, Quaternion value)
		{
		}
	}
}
