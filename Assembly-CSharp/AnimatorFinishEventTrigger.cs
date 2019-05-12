using System;
using UnityEngine;

public class AnimatorFinishEventTrigger : MonoBehaviour
{
	public Action<string> OnFinishAnimation { get; set; }

	private void Awake()
	{
		RuntimeAnimatorController runtimeAnimatorController = base.GetComponent<Animator>().runtimeAnimatorController;
		string functionName = "OnFinishAnimationTrigger";
		foreach (AnimationClip animationClip in runtimeAnimatorController.animationClips)
		{
			animationClip.AddEvent(new AnimationEvent
			{
				functionName = functionName,
				stringParameter = animationClip.name,
				time = animationClip.length
			});
		}
	}

	private void OnFinishAnimationTrigger(string name)
	{
		if (this.OnFinishAnimation != null)
		{
			this.OnFinishAnimation(name);
		}
	}
}
