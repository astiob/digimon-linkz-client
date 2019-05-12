using System;
using UnityEngine;

public sealed class TutorialMessageWindow : MonoBehaviour
{
	[SerializeField]
	private GUICollider window;

	private Action finishWindowTweenAction;

	[SerializeField]
	private TutorialMessageWindowText windowText;

	private float autoFeedTime;

	private Action finishedDisplayTextAction;

	private TutorialMessageWindow.Status windowStatus;

	[SerializeField]
	private GameObject tapIcon;

	private Action pushTapButtonAction;

	private bool isWindowEnabled;

	private bool isOpened;

	public bool IsOpened
	{
		get
		{
			return this.isOpened;
		}
	}

	public void DisplayWindow(int xFromCenter, int yFromCenter, Action completed, bool isAnimation = true)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = (float)xFromCenter;
		localPosition.y = (float)yFromCenter;
		base.transform.localPosition = localPosition;
		this.finishWindowTweenAction = completed;
		this.window.activeCollider = false;
		this.isWindowEnabled = true;
		this.isOpened = true;
		TweenScale component = this.window.GetComponent<TweenScale>();
		component.PlayForward();
		if (!isAnimation)
		{
			component.tweenFactor = 1f;
		}
	}

	public void DeleteWindow(Action completed, bool isAnimation = true)
	{
		this.windowText.ClearText();
		this.finishWindowTweenAction = delegate()
		{
			this.isOpened = false;
			if (completed != null)
			{
				completed();
			}
		};
		this.SetEnableTapIcon(false, null);
		this.isWindowEnabled = false;
		TweenScale component = this.window.GetComponent<TweenScale>();
		component.PlayReverse();
		if (!isAnimation)
		{
			component.tweenFactor = 0f;
		}
	}

	public void SkipWindowAnimation()
	{
		TweenScale component = this.window.GetComponent<TweenScale>();
		if (0f < component.tweenFactor && component.tweenFactor < 1f)
		{
			if (this.isWindowEnabled)
			{
				component.tweenFactor = 1f;
			}
			else
			{
				component.tweenFactor = 0f;
			}
		}
	}

	public void OnFinishedWindowTween()
	{
		if (this.finishWindowTweenAction != null)
		{
			this.finishWindowTweenAction();
			this.finishWindowTweenAction = null;
		}
	}

	public void SetMessage(string message)
	{
		this.windowText.ClearText();
		this.windowText.SetText(message);
		this.SetEnableTapIcon(false, null);
	}

	public void SetDisplayMessage(string message)
	{
		this.windowText.ClearText();
		this.windowText.SetText(message);
		this.windowText.DisplayText();
	}

	public void SkipDisplayMessage(string message)
	{
		this.windowText.SetText(message);
		this.windowText.DisplayText();
		this.windowStatus = TutorialMessageWindow.Status.NONE;
		this.window.activeCollider = false;
		this.autoFeedTime = 0f;
		this.finishedDisplayTextAction = null;
	}

	public void StartDisplayMessage(Action completed)
	{
		this.windowStatus = TutorialMessageWindow.Status.MESSAGE_DRAW;
		this.window.activeCollider = true;
		this.finishedDisplayTextAction = completed;
	}

	public void StartAutoFeedCountDown(float autoFeedSeconds)
	{
		this.windowStatus = TutorialMessageWindow.Status.AUTO_FEED_COUND_DOWN;
		this.autoFeedTime = Time.realtimeSinceStartup + autoFeedSeconds;
	}

	private void FinishedDisplayText()
	{
		this.windowStatus = TutorialMessageWindow.Status.NONE;
		if (this.finishedDisplayTextAction != null)
		{
			this.finishedDisplayTextAction();
			this.finishedDisplayTextAction = null;
		}
	}

	private void Update()
	{
		TutorialMessageWindow.Status status = this.windowStatus;
		if (status != TutorialMessageWindow.Status.MESSAGE_DRAW)
		{
			if (status == TutorialMessageWindow.Status.AUTO_FEED_COUND_DOWN)
			{
				if (this.autoFeedTime <= Time.realtimeSinceStartup)
				{
					this.PageFeed();
				}
			}
		}
		else if (this.windowText.UpdateDisplayText())
		{
			this.FinishedDisplayText();
		}
	}

	public void SetEnableTapIcon(bool enable, Action actionTouched)
	{
		this.window.activeCollider = enable;
		if (enable)
		{
			this.pushTapButtonAction = actionTouched;
		}
		this.tapIcon.SetActive(enable);
		if (enable)
		{
			TweenAlpha component = this.tapIcon.GetComponent<TweenAlpha>();
			component.PlayForward();
		}
	}

	private void OnPushedTapButton()
	{
		this.window.activeCollider = false;
		if (this.windowStatus == TutorialMessageWindow.Status.MESSAGE_DRAW)
		{
			this.windowText.DisplayText();
			this.FinishedDisplayText();
		}
		else
		{
			this.PageFeed();
		}
	}

	private void PageFeed()
	{
		this.windowStatus = TutorialMessageWindow.Status.NONE;
		if (this.pushTapButtonAction != null)
		{
			this.pushTapButtonAction();
			this.pushTapButtonAction = null;
		}
	}

	private enum Status
	{
		NONE,
		MESSAGE_DRAW,
		AUTO_FEED_COUND_DOWN
	}
}
