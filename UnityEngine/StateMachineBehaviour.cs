using System;
using UnityEngine.Experimental.Director;

namespace UnityEngine
{
	/// <summary>
	///   <para>StateMachineBehaviour is a component that can be added to a state machine state. It's the base class every script on a state derives from.</para>
	/// </summary>
	public abstract class StateMachineBehaviour : ScriptableObject
	{
		public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		public virtual void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		public virtual void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		public virtual void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		/// <summary>
		///   <para>Called on the first Update frame when a transition from a state from another statemachine transition to one of this statemachine's state.</para>
		/// </summary>
		/// <param name="animator">The Animator playing this state machine.</param>
		/// <param name="stateMachinePathHash">The full path hash for this state machine.</param>
		public virtual void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
		}

		/// <summary>
		///   <para>Called on the last Update frame when one of the statemachine's state is transitionning toward another state in another state machine.</para>
		/// </summary>
		/// <param name="animator">The Animator playing this state machine.</param>
		/// <param name="stateMachinePathHash">The full path hash for this state machine.</param>
		public virtual void OnStateMachineExit(Animator animator, int stateMachinePathHash)
		{
		}

		public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
		}

		public virtual void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
		}

		public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
		}

		public virtual void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
		}

		public virtual void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
		}

		public virtual void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
		{
		}

		public virtual void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
		{
		}
	}
}
