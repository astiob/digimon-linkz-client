using System;
using System.Collections.Generic;
using UnityEngine;

public class ChipBarLnvocation : MonoBehaviour
{
	[SerializeField]
	[Header("チップアニメーション")]
	private Animation chipAnimation;

	[Header("アニメーションチップ")]
	[SerializeField]
	private AnimationClip inAnimationClip;

	[SerializeField]
	private AnimationClip alwaysAnimationClip;

	[SerializeField]
	private AnimationClip outAnimationClip;

	[Header("演出の待機時間")]
	[SerializeField]
	private float minWaitTime = 2f;

	[Header("チップアイコン表示位置")]
	[SerializeField]
	private Vector3[] chipEffectOffsetPostion = new Vector3[]
	{
		Vector3.zero,
		new Vector3(-0.15f, 0.15f, 0f),
		new Vector3(0.15f, 0.15f, 0f),
		new Vector3(-0.15f, -0.15f, 0f),
		new Vector3(0.15f, -0.15f, 0f)
	};

	private float m_time;

	private float m_waitTime;

	private Action m_endAction;

	private bool m_isInAnimation;

	private bool m_isOutAnimation;

	public List<Vector3> GetChipEffectOffsetPostion()
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < this.chipEffectOffsetPostion.Length; i++)
		{
			list.Add(new Vector3(this.chipEffectOffsetPostion[i].x, this.chipEffectOffsetPostion[i].y, this.chipEffectOffsetPostion[i].z));
		}
		return list;
	}

	public void PlayAnimation(float waitTime, Action endAction)
	{
		this.m_time = 0f;
		this.m_waitTime = this.minWaitTime;
		if (this.minWaitTime < waitTime)
		{
			this.m_waitTime = waitTime;
		}
		this.m_endAction = endAction;
		this.m_isInAnimation = true;
		this.m_isOutAnimation = false;
		base.enabled = true;
		base.gameObject.SetActive(true);
		this.chipAnimation.clip = this.inAnimationClip;
		this.chipAnimation.PlayQueued(this.inAnimationClip.name, QueueMode.PlayNow);
		this.chipAnimation.CrossFadeQueued(this.alwaysAnimationClip.name, 0.2f, QueueMode.CompleteOthers);
	}

	private void EndAnimation()
	{
		this.m_isOutAnimation = true;
		this.chipAnimation.clip = this.outAnimationClip;
		this.chipAnimation.CrossFadeQueued(this.outAnimationClip.name, 0.5f, QueueMode.PlayNow);
		if (this.m_endAction != null)
		{
			this.m_endAction();
		}
	}

	private void Update()
	{
		this.m_time += Time.deltaTime;
		if (this.m_isInAnimation && this.m_waitTime < this.m_time)
		{
			this.m_isInAnimation = false;
			this.EndAnimation();
		}
		if (this.m_isOutAnimation && !this.chipAnimation.isPlaying)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
		}
	}
}
