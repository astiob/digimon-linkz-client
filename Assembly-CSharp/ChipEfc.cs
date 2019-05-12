using System;
using UnityEngine;

public class ChipEfc : MonoBehaviour
{
	[SerializeField]
	[Header("サムネイル OBJ")]
	public GameObject goCHIP_THUMB;

	[SerializeField]
	[Header("アニメーション コンポーネント")]
	public Animation anim;

	[Header("アニメーション OBJ")]
	[SerializeField]
	public GameObject goANIM;

	[SerializeField]
	[Header("パーティクル OBJ")]
	public GameObject goPARTICLE;

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
		if (this.frameCT >= 2 && this.reservePlay)
		{
			this.play();
		}
		if (this.frameCT > 8000)
		{
			this.frameCT = 8000;
		}
	}

	public void Play()
	{
		if (this.frameCT < 2)
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
