using Master;
using System;
using UnityEngine;

public class RoundLimitStart : RoundStart
{
	[SerializeField]
	private Animation roundLimitAnima;

	[SerializeField]
	private UILabel roundNowLabel;

	[SerializeField]
	private UILabel roundLimitLabel;

	private const string animationName = "Battle_Limit_Round";

	private void Awake()
	{
		base.SetupLocalize();
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.roundLimitAnima.Play("Battle_Limit_Round", true, null));
	}

	public void ApplyWaveAndRound(int round, int limitRound)
	{
		string text = string.Format(StringMaster.GetString("BattleUI-53"), round);
		string text2 = string.Format(StringMaster.GetString("BattleUI-51"), limitRound);
		this.roundLocalize.text = text;
		this.roundNowLabel.text = text;
		this.roundLimitLabel.text = text2;
	}

	public bool AnimationIsPlaying()
	{
		return this.roundLimitAnima.isPlaying;
	}

	public bool AnimationTimeCheck()
	{
		return this.roundLimitAnima["Battle_Limit_Round"].normalizedTime <= 0.7f;
	}
}
