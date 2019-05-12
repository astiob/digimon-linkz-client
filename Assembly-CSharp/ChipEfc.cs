using System;
using UnityEngine;

public sealed class ChipEfc : MonoBehaviour
{
	[SerializeField]
	[Header("サムネイル OBJ")]
	public GameObject goCHIP_THUMB;

	[Header("アニメーション コンポーネント")]
	[SerializeField]
	private Animation anim;

	[Header("アニメーション OBJ")]
	[SerializeField]
	private GameObject goANIM;

	[SerializeField]
	[Header("パーティクル OBJ")]
	private GameObject goPARTICLE;

	[Header("NEW スプライト")]
	[SerializeField]
	public UISprite spNew;

	private int frameCT;

	private bool reservePlay;

	private void Awake()
	{
		this.frameCT = 0;
		this.reservePlay = false;
		this.goPARTICLE.SetActive(false);
		this.goCHIP_THUMB.SetActive(false);
		this.goANIM.transform.localScale = Vector3.one;
	}

	private void Update()
	{
		if (this.frameCT == 1)
		{
			this.anim.Stop();
			this.goANIM.transform.localScale = Vector3.one;
		}
		this.frameCT++;
		if (2 <= this.frameCT && this.reservePlay)
		{
			this.play();
		}
		if (8000 < this.frameCT)
		{
			this.frameCT = 8000;
		}
	}

	public void Play()
	{
		if (2 > this.frameCT)
		{
			this.reservePlay = true;
		}
		else
		{
			this.play();
		}
	}

	private void play()
	{
		this.goPARTICLE.SetActive(true);
		this.goCHIP_THUMB.SetActive(true);
		this.anim.Play();
	}
}
