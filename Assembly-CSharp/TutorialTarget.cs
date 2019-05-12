using System;
using UnityEngine;

public class TutorialTarget : MonoBehaviour
{
	[SerializeField]
	private int frameLineSize;

	[SerializeField]
	private int frameMargin;

	[SerializeField]
	private GameObject frame;

	[SerializeField]
	private GameObject arrow;

	private Action tweenFinishedAction;

	public void StartDisplay(bool activeFrame, bool activeArrow, TutorialTarget.ArrowPositon arrowPosition, GameObject targetUI, Action onFinishedDisplayAnimation)
	{
		this.tweenFinishedAction = onFinishedDisplayAnimation;
		UIWidget component = targetUI.GetComponent<UIWidget>();
		if (null != component)
		{
			base.transform.position = component.worldCenter;
		}
		else
		{
			base.transform.position = targetUI.transform.position;
		}
		Vector2 targetUIPartsSize = this.GetTargetUIPartsSize(targetUI);
		if (activeFrame)
		{
			this.AdjustSizeFrame(targetUIPartsSize);
			TweenAlpha component2 = this.frame.GetComponent<TweenAlpha>();
			component2.PlayForward();
			TweenScale component3 = this.frame.GetComponent<TweenScale>();
			component3.PlayForward();
		}
		if (activeArrow)
		{
			this.AdjustPositionArrow(targetUIPartsSize, activeFrame);
			TweenAlpha component4 = this.arrow.GetComponent<TweenAlpha>();
			component4.PlayForward();
			TweenPosition component5 = this.arrow.GetComponent<TweenPosition>();
			component5.PlayForward();
		}
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		switch (arrowPosition)
		{
		case TutorialTarget.ArrowPositon.TOP:
			localEulerAngles.z = 0f;
			break;
		case TutorialTarget.ArrowPositon.BOTTOM:
			localEulerAngles.z = 180f;
			break;
		case TutorialTarget.ArrowPositon.LEFT:
			localEulerAngles.z = 90f;
			break;
		case TutorialTarget.ArrowPositon.RIGHT:
			localEulerAngles.z = 270f;
			break;
		}
		base.transform.localEulerAngles = localEulerAngles;
	}

	public void StartInvisible(Action onFinishedDisplayAnimation)
	{
		this.tweenFinishedAction = delegate()
		{
			if (onFinishedDisplayAnimation != null)
			{
				onFinishedDisplayAnimation();
			}
			Vector3 localEulerAngles = this.transform.localEulerAngles;
			localEulerAngles.z = 0f;
			this.transform.localEulerAngles = localEulerAngles;
		};
		TweenScale component = this.frame.GetComponent<TweenScale>();
		if (component.enabled)
		{
			component.enabled = false;
			TweenAlpha component2 = this.frame.GetComponent<TweenAlpha>();
			component2.PlayReverse();
		}
		TweenPosition component3 = this.arrow.GetComponent<TweenPosition>();
		if (component3.enabled)
		{
			component3.enabled = false;
			TweenAlpha component4 = this.arrow.GetComponent<TweenAlpha>();
			component4.PlayReverse();
		}
	}

	private Vector2 GetTargetUIPartsSize(GameObject targetUI)
	{
		int num = 0;
		int num2 = 0;
		float num3 = 0f;
		float num4 = 0f;
		Transform transform = targetUI.transform;
		UISprite[] componentsInChildren = targetUI.GetComponentsInChildren<UISprite>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (num < componentsInChildren[i].width)
				{
					num = componentsInChildren[i].width;
					num3 = this.GetScale(transform, componentsInChildren[i].transform).x;
				}
				if (num2 < componentsInChildren[i].height)
				{
					num2 = componentsInChildren[i].height;
					num4 = this.GetScale(transform, componentsInChildren[i].transform).y;
				}
			}
		}
		UITexture[] componentsInChildren2 = targetUI.GetComponentsInChildren<UITexture>();
		if (componentsInChildren2 != null)
		{
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (num < componentsInChildren2[j].width)
				{
					num = componentsInChildren2[j].width;
					num3 = this.GetScale(transform, componentsInChildren[j].transform).x;
				}
				if (num2 < componentsInChildren2[j].height)
				{
					num2 = componentsInChildren2[j].height;
					num4 = this.GetScale(transform, componentsInChildren[j].transform).y;
				}
			}
		}
		return new Vector2((float)Mathf.RoundToInt((float)(num + this.frameLineSize) * num3), (float)Mathf.RoundToInt((float)(num2 + this.frameLineSize) * num4));
	}

	private Vector3 GetScale(Transform parent, Transform child)
	{
		Transform transform = child;
		Vector3 localScale = parent.localScale;
		while (parent != transform)
		{
			localScale.Scale(transform.localScale);
			transform = transform.parent;
		}
		return localScale;
	}

	private void AdjustSizeFrame(Vector2 uiPartsSize)
	{
		UISprite component = this.frame.GetComponent<UISprite>();
		component.width = Mathf.RoundToInt(uiPartsSize.x);
		component.height = Mathf.RoundToInt(uiPartsSize.y);
	}

	private void AdjustPositionArrow(Vector2 uiPartsSize, bool isActiveFrame)
	{
		int num = (!isActiveFrame) ? 0 : this.frameMargin;
		int num2 = Mathf.RoundToInt(uiPartsSize.y / 2f) + num;
		this.arrow.transform.localPosition.y = (float)num2;
		TweenPosition component = this.arrow.GetComponent<TweenPosition>();
		component.from = new Vector3(0f, (float)num2, 0f);
		component.to = new Vector3(0f, (float)num2 + 10f, 0f);
	}

	public void OnFinished()
	{
		if (this.tweenFinishedAction != null)
		{
			this.tweenFinishedAction();
			this.tweenFinishedAction = null;
		}
	}

	public enum ArrowPositon
	{
		TOP,
		BOTTOM,
		LEFT,
		RIGHT
	}
}
