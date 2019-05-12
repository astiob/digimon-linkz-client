using System;
using UnityEngine;

public sealed class ChipEfc : MonoBehaviour
{
	[Header("サムネイル OBJ")]
	[SerializeField]
	public GameObject goCHIP_THUMB;

	[Header("アニメーション コンポーネント")]
	[SerializeField]
	private Animation anim;

	[Header("パーティクル OBJ")]
	[SerializeField]
	private GameObject goPARTICLE;

	[Header("NEW スプライト")]
	[SerializeField]
	public UISprite spNew;

	private bool enableParticle = true;

	public void Play()
	{
		this.goPARTICLE.SetActive(this.enableParticle);
		this.anim.Play();
	}

	public void EnableParticle(bool enable)
	{
		this.enableParticle = enable;
		if (!enable && this.goPARTICLE.activeSelf)
		{
			this.goPARTICLE.SetActive(false);
		}
	}
}
