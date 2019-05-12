using System;
using UnityEngine;

public class AnimationFinishEventTrigger : MonoBehaviour
{
	public Action<string> OnFinishAnimation { get; set; }

	private void Awake()
	{
		Animation component = base.GetComponent<Animation>();
		if (component != null)
		{
			this.AddEvent(component);
		}
	}

	private void AddEvent(Animation animation)
	{
		string functionName = "OnFinishAnimationTrigger";
		foreach (object obj in animation)
		{
			AnimationState animationState = (AnimationState)obj;
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.functionName = functionName;
			animationEvent.stringParameter = animationState.clip.name;
			animationEvent.time = animationState.clip.length;
			animationState.clip.AddEvent(animationEvent);
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
