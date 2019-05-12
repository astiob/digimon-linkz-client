using System;
using UnityEngine;

public class RareUpStar : MonoBehaviour
{
	[SerializeField]
	private GameObject starObj;

	[SerializeField]
	private GameObject starEffect;

	[SerializeField]
	private GameObject starRight;

	private int coun;

	private void Start()
	{
		this.starRight.SetActive(false);
		this.starObj.SetActive(false);
		this.starEffect.SetActive(false);
		base.Invoke("StarMove1", 0.5f);
	}

	private void StarMove1()
	{
		this.starRight.SetActive(true);
		base.Invoke("StarMove2", 1f);
	}

	private void StarMove2()
	{
		this.starObj.SetActive(true);
		this.starEffect.SetActive(true);
		base.Invoke("StarMove3", 0.5f);
		base.InvokeRepeating("StarEffectAlphaOn", 0.01f, 0.1f);
	}

	private void StarMove3()
	{
		this.starRight.SetActive(false);
	}

	private void StarEffectAlphaOn()
	{
		if (this.coun <= 5)
		{
			ParticleSystem.MainModule main = this.starEffect.GetComponent<ParticleSystem>().main;
			ParticleSystem.MinMaxGradient startColor = main.startColor;
			startColor.color += new Color(0f, 0f, 0f, 0.2f);
			main.startColor = startColor;
		}
		else
		{
			ParticleSystem.MainModule main2 = this.starEffect.GetComponent<ParticleSystem>().main;
			ParticleSystem.MinMaxGradient startColor2 = main2.startColor;
			startColor2.color -= new Color(0f, 0f, 0f, 0.1f);
			main2.startColor = startColor2;
			if (this.coun > 15)
			{
				base.CancelInvoke("StarEffectAlphaOn");
			}
		}
		this.coun++;
	}
}
