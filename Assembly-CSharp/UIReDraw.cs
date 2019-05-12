using System;
using UnityEngine;

public class UIReDraw : MonoBehaviour
{
	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private UIWidget widget;

	private void OnEnable()
	{
		this.panel = base.GetComponent<UIPanel>();
		this.widget = base.GetComponent<UIWidget>();
	}

	public void Repaint()
	{
		if (this.widget != null)
		{
			this.widget.Update();
		}
		if (this.panel != null)
		{
			this.panel.RebuildAllDrawCalls();
		}
	}

	public void RunOnPress()
	{
		base.SendMessage("OnPress", true);
	}

	public void RunTweenScale()
	{
		Vector3 b = new Vector3(1.1f, 1.1f, 1.1f);
		Vector3 b2 = new Vector3(1.05f, 1.05f, 1.05f);
		Vector3 localScale = base.gameObject.transform.localScale;
		bool flag = true;
		TweenScale.Begin(base.gameObject, 0f, (!flag) ? ((!UICamera.IsHighlighted(base.gameObject)) ? localScale : Vector3.Scale(localScale, b)) : Vector3.Scale(localScale, b2)).method = UITweener.Method.EaseInOut;
	}

	public void RunTweenScale2()
	{
		Vector3 b = new Vector3(1.05f, 1.05f, 1.05f);
		Vector3 localScale = base.gameObject.transform.localScale;
		TweenScale.Begin(base.gameObject, 0f, Vector3.Scale(localScale, b)).method = UITweener.Method.EaseInOut;
	}

	public void RunTweenScale3()
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(base.gameObject, 0f);
		tweenScale.from = tweenScale.value;
		tweenScale.to = Vector3.one;
	}
}
