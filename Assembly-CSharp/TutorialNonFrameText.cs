using System;
using UnityEngine;

public sealed class TutorialNonFrameText : GUICollider
{
	private UILabel message;

	private TweenAlpha messageFadeTweenAlpha;

	private BoxCollider touchBoxCollider;

	private Action finishedTweenAlphaAction;

	private Action pushedTapButtonAction;

	protected override void Awake()
	{
		base.Awake();
		this.message = base.GetComponent<UILabel>();
		this.messageFadeTweenAlpha = base.GetComponent<TweenAlpha>();
		this.touchBoxCollider = base.GetComponent<BoxCollider>();
		base.activeCollider = false;
		this.touchBoxCollider.enabled = false;
	}

	public void Open(NGUIText.Alignment alignment)
	{
		this.message.alignment = alignment;
	}

	public void Close()
	{
		this.messageFadeTweenAlpha.enabled = false;
		base.activeCollider = false;
		this.touchBoxCollider.enabled = false;
		this.message.text = string.Empty;
	}

	public void SetText(string text)
	{
		this.message.text = text;
	}

	public void StartDisplay(float fadeTime, Action onPushedEvent)
	{
		this.pushedTapButtonAction = onPushedEvent;
		this.finishedTweenAlphaAction = delegate()
		{
			base.activeCollider = true;
			this.touchBoxCollider.enabled = true;
		};
		this.messageFadeTweenAlpha.duration = fadeTime;
		this.messageFadeTweenAlpha.PlayForward();
	}

	public void StartInvisible(float fadeTime, Action onFinishedFadeOut)
	{
		base.activeCollider = false;
		this.touchBoxCollider.enabled = false;
		this.finishedTweenAlphaAction = delegate()
		{
			if (onFinishedFadeOut != null)
			{
				onFinishedFadeOut();
			}
		};
		this.messageFadeTweenAlpha.duration = fadeTime;
		this.messageFadeTweenAlpha.PlayReverse();
	}

	public void OnFinishedTweenAlpha()
	{
		if (this.finishedTweenAlphaAction != null)
		{
			this.finishedTweenAlphaAction();
			this.finishedTweenAlphaAction = null;
		}
	}

	public void OnPushed()
	{
		base.activeCollider = false;
		if (this.pushedTapButtonAction != null)
		{
			this.pushedTapButtonAction();
			this.pushedTapButtonAction = null;
		}
	}
}
