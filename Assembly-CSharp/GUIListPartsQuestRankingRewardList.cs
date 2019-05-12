using Master;
using System;
using UnityEngine;

public sealed class GUIListPartsQuestRankingRewardList : GUIListPartBS
{
	[SerializeField]
	private GUIListPartsQuestRankingReward[] rewards;

	[SerializeField]
	private CMD_PointQuestRanking pointQuestRanking;

	private GameWebAPI.RespDataMS_PointQuestRankingList rankingData;

	private GameWebAPI.RespDataMS_PointQuestRankingList.RewardData[] rewardDataList;

	[SerializeField]
	private GUIListPartsQuestRankingRewardList.MODE mode;

	[SerializeField]
	private UILabel title;

	protected override void Awake()
	{
		foreach (GUIListPartsQuestRankingReward guilistPartsQuestRankingReward in this.rewards)
		{
			guilistPartsQuestRankingReward.gameObject.SetActive(false);
		}
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.SetCommonUI();
		base.ShowGUI();
	}

	private void SetCommonUI()
	{
		this.rankingData = CMD_PointQuestRanking.instance.GetPointQuestRankingList();
		GUIListPartsQuestRankingRewardList.MODE mode = this.mode;
		if (mode != GUIListPartsQuestRankingRewardList.MODE.MyRank)
		{
			if (mode == GUIListPartsQuestRankingRewardList.MODE.NextRank)
			{
				this.rewardDataList = this.rankingData.nextRankRewardList;
				if (CMD_PointQuestRanking.instance.GetListIdx() == 0)
				{
					this.title.text = StringMaster.GetString("PointQuestRankingTopRank");
					this.title.pivot = UIWidget.Pivot.Center;
				}
				else
				{
					this.title.pivot = UIWidget.Pivot.Top;
				}
			}
		}
		else
		{
			this.rewardDataList = this.rankingData.myRankRewardList;
			if (this.rewardDataList == null || this.rewardDataList.Length == 0)
			{
				this.title.text = StringMaster.GetString("PointQuestRankingNotReward");
				this.title.pivot = UIWidget.Pivot.Center;
			}
			else
			{
				this.title.pivot = UIWidget.Pivot.Top;
			}
		}
		int num;
		if (this.rewardDataList.Length > 9)
		{
			num = 9;
		}
		else
		{
			num = this.rewardDataList.Length;
		}
		for (int i = 0; i < num; i++)
		{
			this.rewards[i].gameObject.SetActive(true);
			this.rewards[i].SetItem(this.rewardDataList[i].assetCategoryId, this.rewardDataList[i].assetValue, this.rewardDataList[i].assetNum, false, null);
		}
	}

	public enum MODE
	{
		MyRank,
		NextRank
	}
}
