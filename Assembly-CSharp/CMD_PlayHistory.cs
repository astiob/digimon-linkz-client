using Master;
using System;
using UnityEngine;

public class CMD_PlayHistory : CMD
{
	[SerializeField]
	private UILabel questLabel;

	[SerializeField]
	private UILabel questNumLabel;

	[SerializeField]
	private UILabel meatLabel;

	[SerializeField]
	private UILabel meatNumLabel;

	[SerializeField]
	private UILabel fusionLabel;

	[SerializeField]
	private UILabel fusionNumLabel;

	[SerializeField]
	private UILabel evolutionLabel;

	[SerializeField]
	private UILabel evolutionNumLabel;

	[SerializeField]
	private UILabel InheritanceLabel;

	[SerializeField]
	private UILabel InheritanceNumLabel;

	[SerializeField]
	private UILabel researchLabel;

	[SerializeField]
	private UILabel researchNumLabel;

	[SerializeField]
	private UISprite colosseumRankSprite;

	[SerializeField]
	private GameObject colosseumNoneDataObj;

	[SerializeField]
	private UILabel competitionLabel;

	[SerializeField]
	private UILabel competitionNumLabel;

	[SerializeField]
	private UILabel outComeLabel;

	[SerializeField]
	private UILabel outComeNumLabel;

	[SerializeField]
	private UILabel winRateLabel;

	[SerializeField]
	private UILabel winRate;

	[SerializeField]
	private UILabel pictorialBookLabel;

	[SerializeField]
	private UILabel pictorialBookNumLabel;

	[SerializeField]
	private UILabel continueLoginCountLabel;

	[SerializeField]
	private UILabel continueLoginCount;

	[SerializeField]
	private UILabel totalLoginLabel;

	[SerializeField]
	private UILabel totalLoginNumLabel;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.Initialize();
		base.Show(f, sizeX, sizeY, aT);
	}

	private void Initialize()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("PlayHistoryTitle"));
		this.questLabel.text = StringMaster.GetString("PlayHistory-01");
		this.meatLabel.text = StringMaster.GetString("PlayHistory-02");
		this.fusionLabel.text = StringMaster.GetString("PlayHistory-03");
		this.evolutionLabel.text = StringMaster.GetString("PlayHistory-04");
		this.InheritanceLabel.text = StringMaster.GetString("PlayHistory-05");
		this.researchLabel.text = StringMaster.GetString("PlayHistory-06");
		this.competitionLabel.text = StringMaster.GetString("PlayHistory-07");
		this.outComeLabel.text = StringMaster.GetString("PlayHistory-08");
		this.winRateLabel.text = StringMaster.GetString("PlayHistory-09");
		this.pictorialBookLabel.text = StringMaster.GetString("MyProfile-03");
		this.continueLoginCountLabel.text = StringMaster.GetString("PlayHistory-10");
		this.totalLoginLabel.text = StringMaster.GetString("PlayHistory-11");
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
			this.outComeNumLabel.text = string.Format(StringMaster.GetString("ColosseumScore"), "0", "0");
			this.winRate.text = string.Format(StringMaster.GetString("PlayHistory-13"), 0f);
		}
		else
		{
			this.colosseumRankSprite.spriteName = "Rank_" + ColosseumInfo.colosseumRankId.ToString();
			this.competitionNumLabel.text = (ColosseumInfo.winWeek + ColosseumInfo.loseWeek).ToString();
			this.outComeNumLabel.text = string.Format(StringMaster.GetString("ColosseumScore"), ColosseumInfo.winWeek, ColosseumInfo.loseWeek);
			int num = ColosseumInfo.winWeek + ColosseumInfo.loseWeek;
			if (num == 0)
			{
				this.winRate.text = string.Format(StringMaster.GetString("PlayHistory-13"), 0f);
			}
			else
			{
				this.winRate.text = string.Format(StringMaster.GetString("PlayHistory-13"), (float)ColosseumInfo.winWeek / (float)num * 100f);
			}
		}
	}
}
