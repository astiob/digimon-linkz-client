using System;
using UnityEngine;

public class PointResultBonus : MonoBehaviour
{
	[SerializeField]
	private UILabel bonusLabel;

	[SerializeField]
	private UILabel pointLabel;

	[SerializeField]
	private UIWidget widget;

	[SerializeField]
	private AnimationCurve tweenCurve;

	[SerializeField]
	private float fadeTime = 1f;

	private bool fadeFlag;

	private TweenAlpha tweenAlpha;

	public void LabelDataSet(string bunusText, string pointText)
	{
		if (this.bonusLabel != null)
		{
			this.bonusLabel.text = bunusText;
		}
		if (this.pointLabel != null)
		{
			this.pointLabel.text = pointText;
		}
	}

	public void BonusLabelFadeIn(Action fadeEnd = null)
	{
		this.fadeFlag = true;
		if (this.tweenAlpha != null)
		{
			UnityEngine.Object.Destroy(this.tweenAlpha);
			this.tweenAlpha = null;
		}
		this.tweenAlpha = base.gameObject.AddComponent<TweenAlpha>();
		this.tweenAlpha.from = 0f;
		this.tweenAlpha.to = 1f;
		this.tweenAlpha.duration = this.fadeTime;
		this.tweenAlpha.animationCurve = this.tweenCurve;
		this.tweenAlpha.Play(true);
	}

	public void BonusLabelFadeOut(Action fadeEnd = null)
	{
		this.fadeFlag = true;
		if (this.tweenAlpha != null)
		{
			UnityEngine.Object.Destroy(this.tweenAlpha);
			this.tweenAlpha = null;
		}
		this.tweenAlpha = base.gameObject.AddComponent<TweenAlpha>();
		this.tweenAlpha.from = 1f;
		this.tweenAlpha.to = 0f;
		this.tweenAlpha.duration = this.fadeTime;
		this.tweenAlpha.animationCurve = this.tweenCurve;
		this.tweenAlpha.Play(true);
	}

	public void SetLabelAlpha(float alpha)
	{
		this.widget.alpha = alpha;
	}

	public bool FadeCheck()
	{
		return this.fadeFlag;
	}
}
