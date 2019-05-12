using System;
using UnityEngine;

namespace AdventureScene
{
	public class AdventureCameraAnimation
	{
		private Animator cameraAnimator;

		private string endStateName;

		public AdventureCameraAnimation(Animator animator, string startStateName, string endStateName)
		{
			this.cameraAnimator = animator;
			this.endStateName = endStateName;
			this.cameraAnimator.Play(startStateName);
		}

		public bool UpdateAnimation()
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.cameraAnimator.GetCurrentAnimatorStateInfo(0);
			return currentAnimatorStateInfo.IsName(this.endStateName) && 1f <= currentAnimatorStateInfo.normalizedTime;
		}
	}
}
