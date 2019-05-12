using System;
using UnityEngine;

public class BattleExtraEffectUI : MonoBehaviour
{
	[SerializeField]
	[Header("アニメーション")]
	private Animation effectAnimation;

	public bool isPlaying
	{
		get
		{
			return this.effectAnimation.isPlaying;
		}
	}

	public void Play(BattleExtraEffectUI.AnimationType animationType)
	{
		if (animationType == BattleExtraEffectUI.AnimationType.Extra)
		{
			this.effectAnimation.Play("Battle_GimmickBonus");
		}
		else
		{
			this.effectAnimation.Play("Battle_StageEffect");
		}
	}

	public enum AnimationType
	{
		Extra,
		Stage
	}
}
