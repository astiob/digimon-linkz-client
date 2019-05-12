using Master;
using System;
using UnityEngine;

public sealed class GUIListPartsRank : GUIListPartBS
{
	[SerializeField]
	private UILabel pointLabel;

	[SerializeField]
	private UISprite rankSprite;

	private GUIListPartsRank.RankData rankData;

	public GUIListPartsRank.RankData Data
	{
		set
		{
			this.rankData = value;
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		if (!this.rankData.isHideMaximum)
		{
			this.pointLabel.text = string.Format(StringMaster.GetString("ColosseumRankListNeedInfo"), this.rankData.lowerPoint, this.rankData.upperPoint);
		}
		else
		{
			this.pointLabel.text = string.Format(StringMaster.GetString("ColosseumRankListNeedInfo"), this.rankData.lowerPoint, string.Empty);
		}
		if (this.rankData.groupedId > -1)
		{
			this.rankSprite.spriteName = "Rank_" + this.rankData.groupedId.ToString();
		}
		else
		{
			this.rankSprite.spriteName = "Rank_" + this.rankData.id.ToString();
		}
	}

	public class RankData
	{
		public int upperPoint;

		public int lowerPoint;

		public int id;

		public bool isHideMaximum;

		public int groupedId = -1;
	}
}
