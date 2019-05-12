using Master;
using System;
using UnityEngine;

public sealed class CMD_PlayHistory : CMD
{
	[SerializeField]
	private UILabel questNumLabel;

	[SerializeField]
	private UILabel meatNumLabel;

	[SerializeField]
	private UILabel fusionNumLabel;

	[SerializeField]
	private UILabel evolutionNumLabel;

	[SerializeField]
	private UILabel InheritanceNumLabel;

	[SerializeField]
	private UILabel researchNumLabel;

	[SerializeField]
	private UISprite colosseumRankSprite;

	[SerializeField]
	private GameObject colosseumNoneDataObj;

	[SerializeField]
	private UILabel competitionNumLabel;

	[SerializeField]
	private UILabel outComeNumLabel;

	[SerializeField]
	private UILabel winRate;

	[SerializeField]
	private UILabel pictorialBookNumLabel;

	[SerializeField]
	private UILabel continueLoginCount;

	[SerializeField]
	private UILabel totalLoginNumLabel;

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		this.Initialize();
		base.Show(closeEvent, sizeX, sizeY, showAnimationTime);
	}

	private void Initialize()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("PlayHistoryTitle"));
		this.questNumLabel.text = DataMng.Instance().RespDataPRF_Profile.playHistory.dungeonClearCount;
		this.meatNumLabel.text = DataMng.Instance().RespDataPRF_Profile.playHistory.useMeatNum;
		this.fusionNumLabel.text = DataMng.Instance().RespDataPRF_Profile.playHistory.fusionCount;
		this.evolutionNumLabel.text = DataMng.Instance().RespDataPRF_Profile.playHistory.evolutionCount;
		this.InheritanceNumLabel.text = DataMng.Instance().RespDataPRF_Profile.playHistory.inheritanceCount;
		this.researchNumLabel.text = DataMng.Instance().RespDataPRF_Profile.playHistory.combinationCount;
		this.pictorialBookNumLabel.text = string.Format(StringMaster.GetString("SystemFraction"), DataMng.Instance().RespDataPRF_Profile.collection.possessionNum, DataMng.Instance().RespDataPRF_Profile.collection.totalNum);
		this.continueLoginCount.text = DataMng.Instance().RespDataPRF_Profile.playHistory.continueLoginCount;
		this.totalLoginNumLabel.text = DataMng.Instance().RespDataPRF_Profile.playHistory.totalLoginCount;
	}

	public void SetColosseumInfo(GameWebAPI.ColosseumUserStatus ColosseumInfo)
	{
		if (ColosseumInfo == null)
		{
			this.colosseumRankSprite.gameObject.SetActive(false);
			this.colosseumNoneDataObj.SetActive(true);
			this.competitionNumLabel.text = "0";
			this.outComeNumLabel.text = string.Format(StringMaster.GetString("ColosseumScore"), "0", "0");
			this.winRate.text = string.Format(StringMaster.GetString("PlayHistory-13"), 0f);
		}
		else
		{
			this.colosseumRankSprite.spriteName = "Rank_" + ColosseumInfo.colosseumRankId.ToString();
			this.competitionNumLabel.text = (ColosseumInfo.winTotal + ColosseumInfo.loseTotal).ToString();
			this.outComeNumLabel.text = string.Format(StringMaster.GetString("ColosseumScore"), ColosseumInfo.winTotal, ColosseumInfo.loseTotal);
			int num = ColosseumInfo.winTotal + ColosseumInfo.loseTotal;
			if (num == 0)
			{
				this.winRate.text = string.Format(StringMaster.GetString("PlayHistory-13"), 0f);
			}
			else
			{
				this.winRate.text = string.Format(StringMaster.GetString("PlayHistory-13"), (float)ColosseumInfo.winTotal / (float)num * 100f);
			}
		}
	}
}
