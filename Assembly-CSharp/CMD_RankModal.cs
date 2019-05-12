using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_RankModal : CMD
{
	[SerializeField]
	private GUISelectPanelRank guiSelectPanelRank;

	[SerializeField]
	private GUIListPartsRank guiListPartsRank;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel pointTitleLabel;

	[SerializeField]
	private UILabel pointLabel;

	[SerializeField]
	private UISprite rankSprite;

	private void Start()
	{
		this.titleLabel.text = StringMaster.GetString("ColosseumRankListTitle");
		this.pointTitleLabel.text = StringMaster.GetString("ColosseumRankListTotalWin");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.moveBehavior = CommonDialog.MOVE_BEHAVIOUR.NONE;
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.guiSelectPanelRank != null)
		{
			this.guiSelectPanelRank.FadeOutAllListParts(null, false);
			this.guiSelectPanelRank.SetHideScrollBarAllWays(true);
		}
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void Initialize(List<GUIListPartsRank.RankData> RankDataList, int UserRankID, int UserScore, bool IsAggregate)
	{
		this.guiSelectPanelRank.selectParts = this.guiListPartsRank.gameObject;
		List<GUIListPartsRank.RankData> list = new List<GUIListPartsRank.RankData>();
		foreach (GUIListPartsRank.RankData rankData in RankDataList)
		{
			if (rankData.id <= 4)
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
