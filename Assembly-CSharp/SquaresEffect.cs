using System;
using UnityEngine;

public sealed class SquaresEffect : MonoBehaviour
{
	[SerializeField]
	private Material squaresMat;

	[SerializeField]
	private Shader squareShader;

	[SerializeField]
	private float effectSeconds = 0.7f;

	private float effectVariation;

	private int waitCT = 1;

	private float EffectProgress
	{
		get
		{
			return this.squaresMat.GetFloat("_Progress");
		}
		set
		{
			this.squaresMat.SetFloat("_Progress", value);
		}
	}

	private void Update()
	{
		this.EffectAnimation();
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.squaresMat);
	}

	public void Initialize()
	{
		this.squaresMat.shader = this.squareShader;
		this.EffectProgress = 1f;
		this.effectVariation = 1f / this.effectSeconds;
	}

	private void EffectAnimation()
	{
		if (this.EffectProgress == 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (0 < this.waitCT)
		{
			this.waitCT--;
			return;
		}
		float num = this.effectVariation * Time.deltaTime;
		if (num < this.EffectProgress)
		{
			this.EffectProgress -= num;
		}
		else
		{
			this.EffectProgress = 0f;
		}
	}
}
