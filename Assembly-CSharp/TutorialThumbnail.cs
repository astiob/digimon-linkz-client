using System;
using UnityEngine;

public class TutorialThumbnail : MonoBehaviour
{
	[SerializeField]
	private UITexture body;

	[SerializeField]
	private UITexture monitor;

	private Action finishedTweenAction;

	private bool isEnabled;

	private bool isOpened;

	public bool IsOpened
	{
		get
		{
			return this.isOpened;
		}
	}

	private void Awake()
	{
		Vector3 localPosition = this.body.transform.localPosition;
		localPosition.y -= GUIMain.VerticalSpaceSize;
		this.body.transform.localPosition = localPosition;
	}

	public void Display(TutorialThumbnail.ThumbnailType type, Action completed, bool isAnimation)
	{
		this.isOpened = true;
		this.finishedTweenAction = completed;
		this.PlayTween(type, true, isAnimation);
	}

	public void Delete(TutorialThumbnail.ThumbnailType type, Action completed, bool isAnimation = true)
	{
		this.finishedTweenAction = delegate()
		{
			this.isOpened = false;
			if (completed != null)
			{
				completed();
			}
		};
		this.PlayTween(type, false, isAnimation);
	}

	private void PlayTween(TutorialThumbnail.ThumbnailType type, bool isForward, bool isAnimation)
	{
		UITweener component;
		if (type == TutorialThumbnail.ThumbnailType.BODY)
		{
			component = this.body.GetComponent<TweenAlpha>();
		}
		else
		{
			component = this.monitor.GetComponent<TweenScale>();
		}
		this.isEnabled = isForward;
		component.Play(isForward);
		if (!isAnimation)
		{
			component.tweenFactor = ((!isForward) ? 0f : 1f);
		}
	}

	public void SkipWindowAnimation(TutorialThumbnail.ThumbnailType type)
	{
		UITweener component;
		if (type == TutorialThumbnail.ThumbnailType.BODY)
		{
			component = this.body.GetComponent<TweenAlpha>();
		}
		else
		{
			component = this.monitor.GetComponent<TweenScale>();
		}
		if (0f < component.tweenFactor && component.tweenFactor < 1f)
		{
			if (this.isEnabled)
			{
				component.tweenFactor = 1f;
			}
			else
			{
				component.tweenFactor = 0f;
			}
		}
	}

	public void SetMonitorPosition(int yFromCenter)
	{
		Vector3 localPosition = this.monitor.transform.localPosition;
		localPosition.y = (float)yFromCenter;
		this.monitor.transform.localPosition = localPosition;
	}

	public void SetFace(string faceType)
	{
		string texname = "UITexture/Common02_Nabi_" + faceType;
		NGUIUtil.ChangeUITextureFromFile(this.monitor, texname, false);
	}

	public void OnFinishedThumbnailTween()
	{
		if (this.finishedTweenAction != null)
		{
			this.finishedTweenAction();
			this.finishedTweenAction = null;
		}
	}

	public enum ThumbnailType
	{
		BODY,
		MONITOR
	}
}
