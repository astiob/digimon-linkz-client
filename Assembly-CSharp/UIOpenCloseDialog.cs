using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UITweener))]
public class UIOpenCloseDialog : MonoBehaviour
{
	[SerializeField]
	private bool _openIsReversePlay;

	private UITweener _tween;

	private Action _onFinishedDelegate;

	private bool _isPlaying;

	private bool _isInitialized;

	private UITweener tween
	{
		get
		{
			if (this._tween == null)
			{
				this._tween = base.GetComponent<UITweener>();
			}
			return this._tween;
		}
	}

	public bool isPlaying
	{
		get
		{
			return this._isPlaying;
		}
	}

	private void Start()
	{
		this._tween = base.GetComponent<UITweener>();
		EventDelegate.Add(this.tween.onFinished, new EventDelegate.Callback(this.FinishedAnimation));
	}

	public IEnumerator InitializeRoutine()
	{
		if (this._isInitialized)
		{
			yield break;
		}
		UIWidget widgets = base.GetComponent<UIWidget>();
		bool uiWidgetFind = widgets != null;
		float alpha = 1f;
		if (uiWidgetFind)
		{
			alpha = widgets.alpha;
			widgets.alpha = 0f;
		}
		yield return new WaitForEndOfFrame();
		this.tween.ResetToBeginning();
		if (uiWidgetFind)
		{
			widgets.alpha = alpha;
		}
		this._isInitialized = true;
		yield break;
	}

	public void PlayOpenAnimation(Action onFinishedDelegate = null)
	{
		this.PlayAnimation(true, onFinishedDelegate);
	}

	public void PlayCloseAnimation(Action onFinishedDelegate = null)
	{
		this.PlayAnimation(false, onFinishedDelegate);
	}

	private void PlayAnimation(bool isOpen, Action onFinishedDelegate)
	{
		this._isPlaying = true;
		this._onFinishedDelegate = onFinishedDelegate;
		if (!isOpen)
		{
			if (!this._openIsReversePlay)
			{
				this.tween.PlayReverse();
			}
			else
			{
				this.tween.PlayForward();
			}
		}
		else if (this._openIsReversePlay)
		{
			this.tween.PlayReverse();
		}
		else
		{
			this.tween.PlayForward();
		}
	}

	private void FinishedAnimation()
	{
		if (!this._isPlaying)
		{
			return;
		}
		if (this._onFinishedDelegate != null)
		{
			this._onFinishedDelegate();
		}
		this._onFinishedDelegate = null;
		this._isPlaying = false;
	}
}
