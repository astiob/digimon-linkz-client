using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_RankModal : CMD
{
	[SerializeField]
	private GUISelectPanelRank guiSelectPanelRank;

	[SerializeField]
	private GUIListPartsRank guiListPartsRank;

	[SerializeField]
	private UILabel pointLabel;

	[SerializeField]
	private UISprite rankSprite;

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.moveBehavior = CommonDialog.MOVE_BEHAVIOUR.NONE;
		base.Show(closeEvent, sizeX, sizeY, showAnimationTime);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (null != this.guiSelectPanelRank)
		{
			this.guiSelectPanelRank.FadeOutAllListParts(null, false);
			this.guiSelectPanelRank.SetHideScrollBarAllWays(true);
		}
		base.ClosePanel(animation);
	}

	public void Initialize(List<GUIListPartsRank.RankData> RankDataList, int UserRankID, int UserScore, bool IsAggregate)
	{
		this.guiSelectPanelRank.selectParts = this.guiListPartsRank.gameObject;
		List<GUIListPartsRank.RankData> list = new List<GUIListPartsRank.RankData>();
		foreach (GUIListPartsRank.RankData rankData in RankDataList)
		{
			if (4 >= rankData.id)
			{
				if (rankData.id == 4)
				{
					rankData.groupedId = 0;
				}
				else
				{
					rankData.groupedId = rankData.id;
				}
				list.Add(rankData);
			}
		}
		this.guiListPartsRank.gameObject.SetActive(true);
		this.guiSelectPanelRank.initLocation = true;
		Vector3 vector = this.guiSelectPanelRank.AllBuild(list);
		this.guiListPartsRank.gameObject.SetActive(false);
		if (0 < RankDataList.Count)
		{
			float num = this.guiListPartsRank.gameObject.transform.position.x - vector.x;
			Vector3 position = this.guiSelectPanelRank.transform.position;
			position.x += num;
			this.guiSelectPanelRank.transform.position = position;
		}
		this.rankSprite.spriteName = string.Empty;
		foreach (GUIListPartsRank.RankData rankData2 in RankDataList)
		{
			if (rankData2.id == UserRankID)
			{
				this.rankSprite.spriteName = "Rank_" + rankData2.id.ToString();
			}
		}
		this.pointLabel.text = ((!IsAggregate) ? UserScore.ToString() : StringMaster.GetString("PvpAggregate"));
		base.ShowDLG();
		RestrictionInput.EndLoad();
	}
}
