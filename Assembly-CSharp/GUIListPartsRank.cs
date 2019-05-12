using Master;
using System;
using UnityEngine;

public class GUIListPartsRank : GUIListPartBS
{
	[SerializeField]
	private UILabel pointTitleLabel;

	[SerializeField]
	private UILabel pointLabel;

	[SerializeField]
	private UISprite rankSprite;

	private GUIListPartsRank.RankData data;

	public GUIListPartsRank.RankData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.pointTitleLabel.text = StringMaster.GetString("ColosseumRankListNeedTitle2");
		if (!this.data.isHideMaximum)
		{
			this.pointLabel.text = string.Format(StringMaster.GetString("ColosseumRankListNeedInfo"), this.data.lowerPoint, this.data.upperPoint);
		}
		else
		{
			this.pointLabel.text = string.Format(StringMaster.GetString("ColosseumRankListNeedInfo"), this.data.lowerPoint, string.Empty);
		}
		this.rankSprite.spriteName = "Rank_" + this.data.id.ToString();
	}

	public class RankData
	{
		public int upperPoint;

		public int lowerPoint;

		public int id;

		public bool isHideMaximum;
	}
}
