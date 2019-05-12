using System;
using UnityEngine;

public sealed class TutorialMask : MonoBehaviour
{
	private bool maskActive;

	[SerializeField]
	private GameObject fullScreen;

	private bool tweenActive;

	private Action actionFinish;

	private TweenAlpha tween;

	private void Awake()
	{
		GameObject gameObject = this.fullScreen;
		this.tween = gameObject.GetComponent<TweenAlpha>();
	}

	public void SetEnable(bool enable, Action completed, bool isAnimation = true)
	{
		if (this.maskActive != enable)
		{
			this.actionFinish = completed;
			this.maskActive = enable;
			if (enable)
			{
				this.tween.PlayForward();
				if (!isAnimation)
				{
					this.tween.tweenFactor = 1f;
				}
			}
			else
			{
				this.tween.PlayReverse();
				if (!isAnimation)
				{
					this.tween.tweenFactor = 0f;
				}
			}
			this.tweenActive = true;
		}
	}

	public void OnFinishedTween()
	{
		this.tweenActive = false;
		if (this.actionFinish != null)
		{
			this.actionFinish();
			this.actionFinish = null;
		}
	}

	public bool IsTweenActive
	{
		get
		{
			return this.tweenActive;
		}
	}

	private void Update()
	{
		if (this.tweenActive && !this.tween.enabled)
		{
			this.OnFinishedTween();
		}
	}
}
