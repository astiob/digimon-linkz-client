using System;
using UnityEngine;

public class BattleFadeout : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[Header("Fadeoutのスキナー")]
	[SerializeField]
	public UIComponentSkinner fadeoutSkinner;

	private UISprite sprite;

	private float m_time;

	private float m_fadeTime;

	private float m_alphaValue;

	private float m_targetAlpah;

	public void Fade(Color color, float fadeTime = 0f, float alpha = 0f)
	{
		this.m_targetAlpah = alpha;
		this.m_fadeTime = fadeTime;
		this.m_time = 0f;
		base.enabled = true;
		int currentSkin = this.fadeoutSkinner.currentSkin;
		this.fadeoutSkinner.SetSkins(1);
		if (this.sprite == null)
		{
			this.sprite = base.GetComponentInChildren<UISprite>();
		}
		if (this.sprite != null)
		{
			if (currentSkin == 0)
			{
				this.sprite.alpha = 0f;
			}
			this.sprite.color = new Color(color.r, color.g, color.b, this.sprite.alpha);
			this.m_alphaValue = alpha - this.sprite.alpha;
		}
		if (fadeTime == 0f)
		{
			base.enabled = false;
			if (this.sprite != null)
			{
				this.sprite.alpha = this.m_targetAlpah;
			}
			if (alpha == 0f)
			{
				this.fadeoutSkinner.SetSkins(0);
			}
		}
	}

	public void FadeIn(Color color, float fadeTime = 0f)
	{
		this.Fade(color, fadeTime, 0f);
	}

	public void FadeOut(Color color, float fadeTime = 0f)
	{
		this.Fade(color, fadeTime, 1f);
	}

	private void Update()
	{
		if (this.sprite == null)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		this.m_time += deltaTime;
		this.sprite.alpha += this.m_alphaValue * deltaTime;
		if (this.m_time > this.m_fadeTime)
		{
			this.sprite.alpha = this.m_targetAlpah;
			base.enabled = false;
			this.m_time = 0f;
		}
	}
}
