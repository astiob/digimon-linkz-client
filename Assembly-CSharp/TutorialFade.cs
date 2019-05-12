using System;
using UnityEngine;

public sealed class TutorialFade : MonoBehaviour
{
	private Action finishedTweenAction;

	public void StartFade(TutorialFade.FadeType type, bool enable, float time, Action completed)
	{
		Color color = Color.black;
		if (type == TutorialFade.FadeType.WHITE)
		{
			color = Color.white;
		}
		if (enable)
		{
			color.a = 1f;
		}
		else
		{
			color.a = 0f;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.color = color;
		this.finishedTweenAction = completed;
		TweenAlpha component2 = base.GetComponent<TweenAlpha>();
		component2.duration = time;
		if (enable)
		{
			component2.PlayForward();
			if (0f >= time)
			{
				component2.tweenFactor = 1f;
			}
		}
		else
		{
			component2.PlayReverse();
			if (0f >= time)
			{
				component2.tweenFactor = 0f;
			}
		}
	}

	public void OnFinishedTween()
	{
		if (this.finishedTweenAction != null)
		{
			this.finishedTweenAction();
			this.finishedTweenAction = null;
		}
	}

	public enum FadeType
	{
		WHITE,
		BLACK
	}
}
