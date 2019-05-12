using Master;
using System;
using UnityEngine;

public sealed class GUIListPartsQuestRankingList : GUIListPartBS
{
	[SerializeField]
	[Header("ランク帯の背景")]
	private UISprite spBase;

	[Header("ランク帯の背景装飾")]
	[SerializeField]
	private UISprite spBaseGlow;

	[Header("ランク帯間の線")]
	[SerializeField]
	private UISprite spBaseLine;

	[SerializeField]
	[Header("ランク帯最低ポイントの背景")]
	private UISprite spBaseSab;

	[SerializeField]
	[Header("ランク帯最低ポイント")]
	private UILabel lbTX_DuelPoint;

	[SerializeField]
	[Header("ランキング順位")]
	private UILabel lbTX_RankingNumber;

	[Header("今ココ")]
	[SerializeField]
	private GameObject goIsMine;

	[SerializeField]
	private GUIListPartsQuestRankingList.RankColorList rankColorList;

	[SerializeField]
	private Color rankOutBG;

	[SerializeField]
	private Color rankOutGlow;

	private string[] rankRange = new string[2];

	private int rankRangeMin;

	private bool isMine;

	public override void SetData()
	{
		if (null != CMD_PointQuestRanking.instance)
		{
			this.rankRange = CMD_PointQuestRanking.instance.GetPointRankingKey(base.IDX);
			this.rankRangeMin = CMD_PointQuestRanking.instance.GetPointRankingValue(base.IDX);
			this.isMine = CMD_PointQuestRanking.instance.GetIsMine(base.IDX);
		}
		if (null != CMD_ColosseumRanking.instance)
		{
			this.rankRange = CMD_ColosseumRanking.instance.GetRankingKey(base.IDX);
			this.rankRangeMin = CMD_ColosseumRanking.instance.GetRankingValue(base.IDX);
			this.isMine = CMD_ColosseumRanking.instance.GetIsMine(base.IDX);
		}
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.ShowData();
	}

	private void ShowData()
	{
		GUIListPartsQuestRankingList.RankColorList.RankColor rankColor = this.rankColorList.GetRankColor(base.IDX);
		if (this.isMine)
		{
			if (CMD_ColosseumRanking.instance != null && CMD_ColosseumRanking.instance.dispRankingType != CMD_ColosseumRanking.ColosseumRankingType.THIS_TIME)
			{
				this.goIsMine.SetActive(false);
			}
			else
			{
				this.goIsMine.SetActive(true);
			}
			this.spBaseGlow.gameObject.SetActive(true);
		}
		else
		{
			this.goIsMine.SetActive(false);
			this.spBaseGlow.gameObject.SetActive(false);
		}
		if (int.Parse(this.rankRange[1]) > 0)
		{
			this.lbTX_DuelPoint.text = this.rankRangeMin.ToString();
			if (this.rankRange[0] != this.rankRange[1])
			{
				this.lbTX_RankingNumber.text = string.Format(StringMaster.GetString("QuestRankingList"), this.rankRange[0], this.rankRange[1]);
			}
			else
			{
				this.lbTX_RankingNumber.text = string.Format(StringMaster.GetString("ColosseumRankLabel"), this.rankRange[0]);
			}
			this.lbTX_RankingNumber.effectColor = rankColor.textEffect;
			this.spBaseSab.gameObject.SetActive(true);
			this.spBaseLine.gameObject.SetActive(true);
			this.spBase.color = rankColor.rankBG;
			this.spBaseSab.color = rankColor.minPointBG;
			this.spBaseLine.color = rankColor.textEffect;
			if (this.isMine)
			{
				this.spBaseGlow.color = rankColor.rankBG;
			}
		}
		else
		{
			this.lbTX_DuelPoint.text = string.Empty;
			this.lbTX_RankingNumber.text = StringMaster.GetString("ColosseumRankOutside");
			this.lbTX_RankingNumber.effectColor = this.rankOutBG;
			this.spBase.color = this.rankOutBG;
			this.spBaseSab.gameObject.SetActive(false);
			this.spBaseLine.gameObject.SetActive(false);
			if (this.isMine)
			{
				this.spBaseGlow.color = this.rankOutGlow;
			}
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this.beganPostion = pos;
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f)
			{
				this.OnTouchEndedProcess();
			}
		}
		base.OnTouchEnded(touch, pos, flag);
	}

	private void OnTouchEndedProcess()
	{
	}

	private void OnClickedBtnSelect()
	{
		if (null != CMD_ColosseumRanking.instance)
		{
			CMD_ColosseumRanking.instance.DispRankingList(int.Parse(this.rankRange[0]), int.Parse(this.rankRange[0]) + 99);
		}
	}

	[Serializable]
	private struct RankColorList
	{
		public GUIListPartsQuestRankingList.RankColorList.RankColor rankRange1;

		public GUIListPartsQuestRankingList.RankColorList.RankColor rankRange2;

		public GUIListPartsQuestRankingList.RankColorList.RankColor rankRange3;

		public GUIListPartsQuestRankingList.RankColorList.RankColor rankRange4;

		public GUIListPartsQuestRankingList.RankColorList.RankColor rankRange5;

		public GUIListPartsQuestRankingList.RankColorList.RankColor rankRange6;

		public GUIListPartsQuestRankingList.RankColorList.RankColor rankRange7;

		public GUIListPartsQuestRankingList.RankColorList.RankColor GetRankColor(int rankRangeIndex)
		{
			switch (rankRangeIndex)
			{
			case 0:
				return this.rankRange1;
			case 1:
				return this.rankRange2;
			case 2:
				return this.rankRange3;
			case 3:
				return this.rankRange4;
			case 4:
				return this.rankRange5;
			case 5:
				return this.rankRange6;
			}
			return this.rankRange7;
		}

		[Serializable]
		public struct RankColor
		{
			public Color rankBG;

			public Color minPointBG;

			public Color textEffect;
		}
	}
}
