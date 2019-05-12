using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoundChallengeStart : RoundStart
{
	[SerializeField]
	private Animation roundLimitAnima;

	[SerializeField]
	private UILabel roundLimitSpLabel;

	[SerializeField]
	private UILabel roundLimitRankLabel;

	[SerializeField]
	private UILabel roundLimitRankLabelBack;

	[Header("スピードクリアランク表示のカラー")]
	[SerializeField]
	private Color[] rankGradientTopColor;

	[SerializeField]
	private Color[] rankGradientBottomColor;

	[SerializeField]
	private Color[] rankOutlineColor;

	private const string animationName = "Battle_Limit_Round_SP";

	private void Awake()
	{
		base.SetupLocalize();
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.roundLimitAnima.Play("Battle_Limit_Round_SP", true, null));
	}

	public void ApplyWaveAndRoundSpeed(int spLimitRound, List<GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM.WorldDungeonOptionReward> rewardList)
	{
		spLimitRound--;
		bool flag = false;
		int num = 0;
		GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM.WorldDungeonOptionReward worldDungeonOptionReward = null;
		if (rewardList.Count > 0)
		{
			for (int i = 0; i < rewardList.Count; i++)
			{
				if (int.Parse(rewardList[i].clearValue) > spLimitRound)
				{
					num = int.Parse(rewardList[i].clearValue);
					worldDungeonOptionReward = rewardList[i];
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			int num2 = int.Parse(worldDungeonOptionReward.clearType);
			Vector3 localPosition = new Vector3(-255f, 5f, 0f);
			Vector3 localPosition2 = new Vector3(-240f, 5f, 0f);
			Vector3 localPosition3 = new Vector3(-225f, 4f, 0f);
			this.roundLimitRankLabel.transform.localPosition = localPosition3;
			this.roundLimitRankLabel.gameObject.SetActive(true);
			this.roundLimitRankLabelBack.gameObject.SetActive(false);
			this.roundLimitRankLabel.gradientTop = this.rankGradientTopColor[num2 - 1];
			this.roundLimitRankLabel.gradientBottom = this.rankGradientBottomColor[num2 - 1];
			this.roundLimitRankLabel.effectColor = this.rankOutlineColor[num2 - 1];
			this.roundLimitRankLabel.text = StringMaster.GetString("SpeedClearRank" + num2.ToString());
			this.roundLimitRankLabelBack.text = StringMaster.GetString("SpeedClearRank" + num2.ToString());
			switch (num2)
			{
			case 1:
				this.roundLimitRankLabel.transform.localPosition = localPosition;
				this.roundLimitRankLabelBack.gameObject.SetActive(true);
				break;
			case 2:
				this.roundLimitRankLabel.transform.localPosition = localPosition2;
				this.roundLimitRankLabelBack.gameObject.SetActive(true);
				break;
			case 4:
				this.roundLimitRankLabelBack.gameObject.SetActive(true);
				break;
			case 5:
				this.roundLimitRankLabelBack.gameObject.SetActive(true);
				break;
			case 7:
				this.roundLimitRankLabelBack.gameObject.SetActive(true);
				break;
			case 8:
				this.roundLimitRankLabelBack.gameObject.SetActive(true);
				break;
			}
		}
		else
		{
			this.roundLimitRankLabel.gameObject.SetActive(false);
			this.roundLimitRankLabelBack.gameObject.SetActive(false);
		}
		int num3 = num - spLimitRound;
		string text = string.Format(StringMaster.GetString("BattleUI-52"), num3);
		this.roundLimitSpLabel.text = text;
	}

	public bool AnimationIsPlaying()
	{
		return this.roundLimitAnima.isPlaying;
	}

	public enum ChallengeType
	{
		Solo = 1,
		MultiHost,
		MultiGuest
	}

	public enum SpeedClearRank
	{
		SSS = 1,
		SS,
		S,
		A,
		B,
		C,
		D,
		E,
		F,
		G
	}
}
