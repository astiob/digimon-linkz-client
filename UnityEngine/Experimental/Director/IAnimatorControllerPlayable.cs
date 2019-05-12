using System;

namespace UnityEngine.Experimental.Director
{
	public interface IAnimatorControllerPlayable
	{
		/// <summary>
		///   <para>Gets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		float GetFloat(string name);

		/// <summary>
		///   <para>Gets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		float GetFloat(int id);

		/// <summary>
		///   <para>Sets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetFloat(string name, float value);

		/// <summary>
		///   <para>Sets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetFloat(int id, float value);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		bool GetBool(string name);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		bool GetBool(int id);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetBool(string name, bool value);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetBool(int id, bool value);

		/// <summary>
		///   <para>Gets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		int GetInteger(string name);

		/// <summary>
		///   <para>Gets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		int GetInteger(int id);

		/// <summary>
		///   <para>Sets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetInteger(string name, int value);

		/// <summary>
		///   <para>Sets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetInteger(int id, int value);

		/// <summary>
		///   <para>Sets a trigger parameter to active.
		/// A trigger parameter is a bool parameter that gets reset to false when it has been used in a transition. For state machines with multiple layers, the trigger will only get reset once all layers have been evaluated, so that the layers can synchronize their transitions on the same parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetTrigger(string name);

		/// <summary>
		///   <para>Sets a trigger parameter to active.
		/// A trigger parameter is a bool parameter that gets reset to false when it has been used in a transition. For state machines with multiple layers, the trigger will only get reset once all layers have been evaluated, so that the layers can synchronize their transitions on the same parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void SetTrigger(int id);

		/// <summary>
		///   <para>Resets the trigger parameter to false.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void ResetTrigger(string name);

		/// <summary>
		///   <para>Resets the trigger parameter to false.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		void ResetTrigger(int id);

		/// <summary>
		///   <para>Returns true if a parameter is controlled by an additional curve on an animation.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		bool IsParameterControlledByCurve(string name);

		/// <summary>
		///   <para>Returns true if a parameter is controlled by an additional curve on an animation.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		bool IsParameterControlledByCurve(int id);

		/// <summary>
		///   <para>The AnimatorController layer count.</para>
		/// </summary>
		int layerCount { get; }

		/// <summary>
		///   <para>Gets name of the layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		string GetLayerName(int layerIndex);

		/// <summary>
		///   <para>Gets the index of the layer with specified name.</para>
		/// </summary>
		/// <param name="layerName">The layer's name.</param>
		int GetLayerIndex(string layerName);

		/// <summary>
		///   <para>Gets the layer's current weight.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		float GetLayerWeight(int layerIndex);

		/// <summary>
		///   <para>Sets the layer's current weight.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		/// <param name="weight">The weight of the layer.</param>
		void SetLayerWeight(int layerIndex, float weight);

		/// <summary>
		///   <para>Gets the current State information on a specified AnimatorController layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the next State information on a specified AnimatorController layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the Transition information on a specified AnimatorController layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the list of AnimatorClipInfo currently played by the current state.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the list of AnimatorClipInfo currently played by the next state.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex);

		/// <summary>
		///   <para>Is the specified AnimatorController layer in a transition.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		bool IsInTransition(int layerIndex);

		/// <summary>
		///   <para>The number of AnimatorControllerParameters used by the AnimatorController.</para>
		/// </summary>
		int parameterCount { get; }

		/// <summary>
		///   <para>Read only access to the AnimatorControllerParameters used by the animator.</para>
		/// </summary>
		/// <param name="index">The index of the parameter.</param>
		AnimatorControllerParameter GetParameter(int index);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.CrossFade, but the duration and offset in the target state are in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in seconds.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer, float fixedTime);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.CrossFade, but the duration and offset in the target state are in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in seconds.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer, float fixedTime);

		/// <summary>
		///   <para>Creates a dynamic transition between the current state and the destination state.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in source state normalized time.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in source state normalized time, should be between 0 and 1.  If no explicit normalizedTime is specified or normalizedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void CrossFade(string stateName, float transitionDuration, int layer, float normalizedTime);

		/// <summary>
		///   <para>Creates a dynamic transition between the current state and the destination state.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in source state normalized time.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in source state normalized time, should be between 0 and 1.  If no explicit normalizedTime is specified or normalizedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void CrossFade(int stateNameHash, float transitionDuration, int layer, float normalizedTime);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.Play, but the offset in the target state is in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void PlayInFixedTime(string stateName, int layer, float fixedTime);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.Play, but the offset in the target state is in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void PlayInFixedTime(int stateNameHash, int layer, float fixedTime);

		/// <summary>
		///   <para>Plays a state.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in normalized time. If no explicit normalizedTime is specified or value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void Play(string stateName, int layer, float normalizedTime);

		/// <summary>
		///   <para>Plays a state.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in normalized time. If no explicit normalizedTime is specified or value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		void Play(int stateNameHash, int layer, float normalizedTime);

		/// <summary>
		///   <para>Returns true if the AnimatorState is present in the Animator's controller.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		/// <param name="stateID">The AnimatorState fullPathHash, nameHash or shortNameHash.</param>
		bool HasState(int layerIndex, int stateID);
	}
}
