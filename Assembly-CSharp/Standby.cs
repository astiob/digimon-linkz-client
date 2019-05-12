using System;
using UnityEngine;

public class Standby : MonoBehaviour
{
	[SerializeField]
	[Header("自分/仲間/敵コンテンツ(真ん中ターン表示)")]
	private Animator myAnimator;

	public void PlayShowAnimation()
	{
		this.myAnimator.SetTrigger("start");
	}

	public void PlayHideAnimation()
	{
		this.myAnimator.SetTrigger("finish");
	}
}
