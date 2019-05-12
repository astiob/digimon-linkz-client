using System;
using UnityEngine;

namespace CharacterModelUI
{
	public sealed class ModelViewerOpenAnimationEvent : StateMachineBehaviour
	{
		private const string STATE_NAME_IDLE = "Idle";

		private const string STATE_NAME_SHOW = "Open";

		private Animator changeAnimetor;

		private Action onFinishAnimation;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (1f <= stateInfo.normalizedTime)
			{
				animator.SetTrigger("Idle");
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onFinishAnimation != null)
			{
				this.onFinishAnimation();
			}
			animator.ResetTrigger("Idle");
			RestrictionInput.EndLoad();
		}

		public void Initialize(Animator animation, Action onOpenedAction)
		{
			this.changeAnimetor = animation;
			this.onFinishAnimation = onOpenedAction;
		}

		public void StartAnimation()
		{
			this.changeAnimetor.SetTrigger("Open");
		}
	}
}
