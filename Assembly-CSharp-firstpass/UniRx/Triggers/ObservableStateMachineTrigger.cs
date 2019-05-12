using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableStateMachineTrigger : StateMachineBehaviour
	{
		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateExit;

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateEnter;

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateIK;

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateUpdate;

		private Subject<ObservableStateMachineTrigger.OnStateMachineInfo> onStateMachineEnter;

		private Subject<ObservableStateMachineTrigger.OnStateMachineInfo> onStateMachineExit;

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateExit != null)
			{
				this.onStateExit.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateExitAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> result;
			if ((result = this.onStateExit) == null)
			{
				result = (this.onStateExit = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return result;
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateEnter != null)
			{
				this.onStateEnter.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateEnterAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> result;
			if ((result = this.onStateEnter) == null)
			{
				result = (this.onStateEnter = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return result;
		}

		public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateIK != null)
			{
				this.onStateIK.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateIKAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> result;
			if ((result = this.onStateIK) == null)
			{
				result = (this.onStateIK = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return result;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateUpdate != null)
			{
				this.onStateUpdate.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateUpdateAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> result;
			if ((result = this.onStateUpdate) == null)
			{
				result = (this.onStateUpdate = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return result;
		}

		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			if (this.onStateMachineEnter != null)
			{
				this.onStateMachineEnter.OnNext(new ObservableStateMachineTrigger.OnStateMachineInfo(animator, stateMachinePathHash));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateMachineInfo> OnStateMachineEnterAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateMachineInfo> result;
			if ((result = this.onStateMachineEnter) == null)
			{
				result = (this.onStateMachineEnter = new Subject<ObservableStateMachineTrigger.OnStateMachineInfo>());
			}
			return result;
		}

		public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
		{
			if (this.onStateMachineExit != null)
			{
				this.onStateMachineExit.OnNext(new ObservableStateMachineTrigger.OnStateMachineInfo(animator, stateMachinePathHash));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateMachineInfo> OnStateMachineExitAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateMachineInfo> result;
			if ((result = this.onStateMachineExit) == null)
			{
				result = (this.onStateMachineExit = new Subject<ObservableStateMachineTrigger.OnStateMachineInfo>());
			}
			return result;
		}

		public class OnStateInfo
		{
			public OnStateInfo(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			{
				this.Animator = animator;
				this.StateInfo = stateInfo;
				this.LayerIndex = layerIndex;
			}

			public Animator Animator { get; private set; }

			public AnimatorStateInfo StateInfo { get; private set; }

			public int LayerIndex { get; private set; }
		}

		public class OnStateMachineInfo
		{
			public OnStateMachineInfo(Animator animator, int stateMachinePathHash)
			{
				this.Animator = animator;
				this.StateMachinePathHash = stateMachinePathHash;
			}

			public Animator Animator { get; private set; }

			public int StateMachinePathHash { get; private set; }
		}
	}
}
