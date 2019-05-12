using Master;
using System;
using UnityEngine;

public class RoundChallengeStart : RoundStart
{
	private const string animationName = "Battle_Limit_Round_SP";

	[SerializeField]
	private Animation roundLimitAnima;

	[SerializeField]
	private UILabel roundLimitSpLabel;

	private void Awake()
	{
		base.SetupLocalize();
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.roundLimitAnima.Play("Battle_Limit_Round_SP", true, null));
	}

	public void ApplyWaveAndRoundSpeed(int spLimitRound)
	{
		string text = string.Format(StringMaster.GetString("BattleUI-52"), spLimitRound);
		this.roundLimitSpLabel.text = text;
	}

	public bool AnimationIsPlaying()
	{
		return this.roundLimitAnima.isPlaying;
	}
}
