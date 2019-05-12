using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine.Experimental.Director
{
	/// <summary>
	///   <para>Playable that plays a RuntimeAnimatorController. Can be used as an input to an AnimationPlayable.</para>
	/// </summary>
	public sealed class AnimatorControllerPlayable : AnimationPlayable, IAnimatorControllerPlayable
	{
		public AnimatorControllerPlayable(RuntimeAnimatorController controller) : base(false)
		{
			this.m_Ptr = IntPtr.Zero;
			this.InstantiateEnginePlayable(controller);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InstantiateEnginePlayable(RuntimeAnimatorController controller);

		/// <summary>
		///   <para>RuntimeAnimatorController played by this playable.</para>
		/// </summary>
		public extern RuntimeAnimatorController animatorController { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override int AddInput(AnimationPlayable source)
		{
			Debug.LogError("AnimationClipPlayable doesn't support adding inputs");
			return -1;
		}

		public override bool SetInput(AnimationPlayable source, int index)
		{
			Debug.LogError("AnimationClipPlayable doesn't support setting inputs");
			return false;
		}

		public override bool SetInputs(IEnumerable<AnimationPlayable> sources)
		{
			Debug.LogError("AnimationClipPlayable doesn't support setting inputs");
			return false;
		}

		public override bool RemoveInput(int index)
		{
			Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
			return false;
		}

		public override bool RemoveInput(AnimationPlayable playable)
		{
			Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
			return false;
		}

		public override bool RemoveAllInputs()
		{
			Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
			return false;
		}

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
		/// <param name="id"></param>
		public void SetFloat(int id, float value)
		{
			this.SetFloatID(id, value);
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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string ResolveHash(int hash);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.IsInTransition.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsInTransition(int layerIndex);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.parameterCount.</para>
		/// </summary>
		public extern int parameterCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		private extern AnimatorControllerParameter[] parameters { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int StringToHash(string name);

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
			this.CrossFadeInFixedTime(AnimatorControllerPlayable.StringToHash(stateName), transitionDuration, layer, fixedTime);
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
			this.CrossFade(AnimatorControllerPlayable.StringToHash(stateName), transitionDuration, layer, normalizedTime);
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
			this.PlayInFixedTime(AnimatorControllerPlayable.StringToHash(stateName), layer, fixedTime);
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
			this.Play(AnimatorControllerPlayable.StringToHash(stateName), layer, normalizedTime);
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
		///   <para>See IAnimatorControllerPlayable.HasState.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		/// <param name="stateID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasState(int layerIndex, int stateID);

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
	}
}
