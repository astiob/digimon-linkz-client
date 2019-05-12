using System;
using UnityEngine;

public class ResultBase : MonoBehaviour
{
	[SerializeField]
	[Header("TAP NEXTのオブジェクト")]
	private GameObject tapNext;

	public bool isEnd { get; protected set; }

	protected bool isShowTapNext
	{
		get
		{
			return this.tapNext.activeInHierarchy;
		}
	}

	public void UpdateAndroidBackKey()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnTapped();
		}
	}

	public virtual void Init()
	{
		this.HideNextTap();
	}

	public virtual void Show()
	{
	}

	public virtual void OnTapped()
	{
	}

	protected void ShowNextTap()
	{
		NGUITools.SetActiveSelf(this.tapNext, true);
		TweenAlpha[] componentsInChildren = this.tapNext.GetComponentsInChildren<TweenAlpha>(true);
		foreach (TweenAlpha tweenAlpha in componentsInChildren)
		{
			tweenAlpha.ResetToBeginning();
			tweenAlpha.PlayForward();
		}
		UIWidget component = this.tapNext.GetComponent<UIWidget>();
		component.alpha = 0f;
	}

	protected void HideNextTap()
	{
		NGUITools.SetActiveSelf(this.tapNext, false);
	}

	protected void ResetTweenAlpha(GameObject go)
	{
		TweenAlpha component = go.GetComponent<TweenAlpha>();
		if (component != null)
		{
			NGUITools.DestroyImmediate(component);
		}
		UIWidget component2 = go.GetComponent<UIWidget>();
		if (component2 != null)
		{
			component2.alpha = 1f;
		}
	}
}
