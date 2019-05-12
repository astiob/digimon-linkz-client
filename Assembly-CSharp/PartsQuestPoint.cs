using Master;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PartsQuestPoint : MonoBehaviour
{
	[Header("ポイントのスプライト")]
	[SerializeField]
	private List<UISprite> spPointList;

	[SerializeField]
	[Header("ランキングのスプライト")]
	private List<UISprite> spRankingList;

	[Header("ランキングの位の文字 -> 圏外で消し")]
	[SerializeField]
	private UILabel lbTX_Ranking;

	[SerializeField]
	[Header("ランキング圏外")]
	private UILabel lbTX_RankingOut;

	public QuestData.WorldAreaData AreaData;

	public GameWebAPI.RespDataWD_PointQuestInfo PointInfo;

	protected virtual void Awake()
	{
	}

	public virtual void ShowData()
	{
		this.ShowNumber(this.spPointList, this.PointInfo.currentPoint, false);
		if (this.spRankingList != null && this.lbTX_Ranking != null && this.lbTX_RankingOut != null)
		{
			this.ShowNumber(this.spRankingList, this.PointInfo.currentRank, true);
			if (this.PointInfo.currentRank == 0)
			{
				this.lbTX_Ranking.gameObject.SetActive(false);
				this.lbTX_RankingOut.gameObject.SetActive(true);
				this.lbTX_RankingOut.text = StringMaster.GetString("ColosseumRankOutside");
			}
		}
	}

	protected virtual void ShowNumber(List<UISprite> spL, int num, bool dontSHowZero = false)
	{
		TextUtil.ShowNumber(spL, "Common02_FriendshipN_", num, dontSHowZero);
	}

	protected virtual void OnDestroy()
	{
	}

	private void OnClickedRanking()
	{
		CMD_PointQuestRanking.PointQuestInfo = this.PointInfo;
		GUIMain.ShowCommonDialog(null, "CMD_PointQuestRanking");
	}

	private void OnClickedDetail()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("PointQuest-1");
		cmdwebWindow.Url = string.Format(WebAddress.EXT_ADR_POINT_QUEST_DETAIL, this.PointInfo.worldEventId);
	}

	private void OnClickedReward()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("PointQuest-2");
		cmdwebWindow.Url = string.Format(WebAddress.EXT_ADR_POINT_QUEST_REWARD, this.PointInfo.worldEventId);
	}
}
