using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLabelToggle : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve m_animaCurve;

	private List<TweenAlpha> m_tweenAlpha = new List<TweenAlpha>();

	private UILabel m_toggleLabel;

	private UISprite m_toggleSprite;

	private List<string> m_toggleTextList = new List<string>();

	private List<string> m_toggleSpriteList = new List<string>();

	private float m_waitTime;

	private float m_toggleTime;

	private int m_counter;

	private List<Color> m_labelColorList = new List<Color>();

	private List<Color> m_effectColorList = new List<Color>();

	public void InitToggleData(UILabel label, List<string> textList, float toggleTime = 1f, float waitTime = 1f, bool spriteReset = true)
	{
		if (label == null || textList.Count == 0)
		{
			return;
		}
		TweenAlpha component = label.gameObject.GetComponent<TweenAlpha>();
		if (component == null)
		{
			this.m_tweenAlpha.Add(label.gameObject.AddComponent<TweenAlpha>());
		}
		else
		{
			this.m_tweenAlpha.Remove(component);
			UnityEngine.Object.Destroy(component);
			this.m_tweenAlpha.Add(label.gameObject.AddComponent<TweenAlpha>());
		}
		for (int i = 0; i < this.m_tweenAlpha.Count; i++)
		{
			this.m_tweenAlpha[i].enabled = false;
		}
		if (spriteReset && this.m_toggleSprite != null)
		{
			TweenAlpha component2 = this.m_toggleSprite.gameObject.GetComponent<TweenAlpha>();
			if (component2 != null)
			{
				this.m_tweenAlpha.Remove(component2);
				UnityEngine.Object.Destroy(component2);
			}
			this.m_toggleSprite.alpha = 1f;
			this.m_toggleSprite = null;
		}
		this.m_toggleTextList = textList;
		this.m_waitTime = waitTime;
		this.m_toggleTime = toggleTime;
		this.m_toggleLabel = label;
		this.m_counter = 0;
		if (this.m_labelColorList != null && this.m_labelColorList.Count > 0)
		{
			this.m_toggleLabel.color = this.m_labelColorList[0];
		}
		if (this.m_effectColorList != null && this.m_effectColorList.Count > 0)
		{
			this.m_toggleLabel.effectColor = this.m_effectColorList[0];
		}
		this.m_toggleLabel.text = this.m_toggleTextList[0];
		this.m_toggleLabel.alpha = 0f;
		for (int j = 0; j < this.m_tweenAlpha.Count; j++)
		{
			this.m_tweenAlpha[j].from = 0f;
			this.m_tweenAlpha[j].to = 1f;
			this.m_tweenAlpha[j].style = UITweener.Style.Once;
			this.m_tweenAlpha[j].duration = this.m_toggleTime;
			this.m_tweenAlpha[j].animationCurve = this.m_animaCurve;
			this.m_tweenAlpha[j].tweenFactor = 0f;
		}
		EventDelegate.Set(this.m_tweenAlpha[0].onFinished, new EventDelegate.Callback(this.FadeInAnimaEnd));
		base.StartCoroutine(this.PlayTweenStart());
	}

	private IEnumerator PlayTweenStart()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < this.m_tweenAlpha.Count; i++)
		{
			this.m_tweenAlpha[i].PlayForward();
		}
		yield break;
	}

	public void InitToggleData(UILabel label, List<string> textList, UISprite sprite, float toggleTime = 1f, float waitTime = 1f)
	{
		if (sprite == null)
		{
			return;
		}
		TweenAlpha component = sprite.gameObject.GetComponent<TweenAlpha>();
		if (component == null)
		{
			this.m_tweenAlpha.Add(sprite.gameObject.AddComponent<TweenAlpha>());
		}
		else
		{
			this.m_tweenAlpha.Remove(component);
			UnityEngine.Object.Destroy(component);
			this.m_tweenAlpha.Add(sprite.gameObject.AddComponent<TweenAlpha>());
		}
		this.m_toggleSprite = sprite;
		this.m_toggleSprite.alpha = 0f;
		this.InitToggleData(label, textList, toggleTime, waitTime, false);
	}

	public void InitToggleData(UILabel label, List<string> textList, UISprite sprite, List<string> spriteList, float toggleTime = 1f, float waitTime = 1f)
	{
		if (sprite == null || spriteList.Count == 0)
		{
			return;
		}
		TweenAlpha component = sprite.gameObject.GetComponent<TweenAlpha>();
		if (component == null)
		{
			this.m_tweenAlpha.Add(sprite.gameObject.AddComponent<TweenAlpha>());
		}
		else
		{
			this.m_tweenAlpha.Remove(component);
			UnityEngine.Object.Destroy(component);
			this.m_tweenAlpha.Add(sprite.gameObject.AddComponent<TweenAlpha>());
		}
		this.m_toggleSprite = sprite;
		this.m_toggleSpriteList = spriteList;
		this.m_toggleSprite.spriteName = this.m_toggleSpriteList[0];
		this.m_toggleSprite.alpha = 0f;
		this.InitToggleData(label, textList, toggleTime, waitTime, false);
	}

	public void InitToggleData(UILabel label, List<string> textList, List<Color> labelColor, List<Color> effectColoer = null, float toggleTime = 1f, float waitTime = 1f, bool spriteReset = true)
	{
		this.m_labelColorList = labelColor;
		this.m_effectColorList = effectColoer;
		this.InitToggleData(label, textList, toggleTime, waitTime, spriteReset);
	}

	public void StopToggleAnima()
	{
		for (int i = 0; i < this.m_tweenAlpha.Count; i++)
		{
			if (this.m_tweenAlpha[i] != null)
			{
				UnityEngine.Object.Destroy(this.m_tweenAlpha[i]);
			}
		}
		this.m_tweenAlpha.Clear();
	}

	private void FadeInAnimaEnd()
	{
		base.StartCoroutine(this.RemoveDelegateFadeIn());
	}

	private IEnumerator RemoveDelegateFadeIn()
	{
		yield return new WaitForEndOfFrame();
		EventDelegate.Remove(this.m_tweenAlpha[0].onFinished, new EventDelegate.Callback(this.FadeInAnimaEnd));
		for (int i = 0; i < this.m_tweenAlpha.Count; i++)
		{
			this.m_tweenAlpha[i].from = 1f;
			this.m_tweenAlpha[i].to = 0f;
			this.m_tweenAlpha[i].style = UITweener.Style.Once;
			this.m_tweenAlpha[i].duration = this.m_toggleTime;
			this.m_tweenAlpha[i].animationCurve = this.m_animaCurve;
			this.m_tweenAlpha[i].delay = this.m_waitTime;
			this.m_tweenAlpha[i].tweenFactor = 0f;
			this.m_tweenAlpha[i].PlayForward();
		}
		EventDelegate.Set(this.m_tweenAlpha[0].onFinished, new EventDelegate.Callback(this.FadeOutAnimeEnd));
		yield break;
	}

	private void FadeOutAnimeEnd()
	{
		this.m_counter++;
		if (this.m_toggleSprite != null && this.m_toggleSpriteList.Count > 0)
		{
			this.m_toggleSprite.spriteName = this.m_toggleSpriteList[this.m_counter % this.m_toggleSpriteList.Count];
		}
		this.m_toggleLabel.text = this.m_toggleTextList[this.m_counter % this.m_toggleTextList.Count];
		if (this.m_labelColorList != null && this.m_labelColorList.Count > 0)
		{
			Color color = new Color(this.m_labelColorList[this.m_counter % this.m_labelColorList.Count].r, this.m_labelColorList[this.m_counter % this.m_labelColorList.Count].g, this.m_labelColorList[this.m_counter % this.m_labelColorList.Count].b, 0f);
			this.m_toggleLabel.color = color;
		}
		if (this.m_effectColorList != null && this.m_effectColorList.Count > 0)
		{
			this.m_toggleLabel.effectColor = this.m_effectColorList[this.m_counter % this.m_effectColorList.Count];
		}
		base.StartCoroutine(this.RemoveDelegateFadeOut());
	}

	private IEnumerator RemoveDelegateFadeOut()
	{
		yield return new WaitForEndOfFrame();
		EventDelegate.Remove(this.m_tweenAlpha[0].onFinished, new EventDelegate.Callback(this.FadeOutAnimeEnd));
		for (int i = 0; i < this.m_tweenAlpha.Count; i++)
		{
			this.m_tweenAlpha[i].from = 0f;
			this.m_tweenAlpha[i].to = 1f;
			this.m_tweenAlpha[i].style = UITweener.Style.Once;
			this.m_tweenAlpha[i].duration = this.m_toggleTime;
			this.m_tweenAlpha[i].animationCurve = this.m_animaCurve;
			this.m_tweenAlpha[i].delay = 0f;
			this.m_tweenAlpha[i].tweenFactor = 0f;
			this.m_tweenAlpha[i].PlayForward();
		}
		EventDelegate.Set(this.m_tweenAlpha[0].onFinished, new EventDelegate.Callback(this.FadeInAnimaEnd));
		yield break;
	}
}
