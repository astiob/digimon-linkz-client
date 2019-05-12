using System;
using System.Collections;
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
		IEnumerator enumerator = animation.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				AnimationState animationState = (AnimationState)obj;
				AnimationEvent animationEvent = new AnimationEvent();
				animationEvent.functionName = functionName;
				animationEvent.stringParameter = animationState.clip.name;
				animationEvent.time = animationState.clip.length;
				animationState.clip.AddEvent(animationEvent);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
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
