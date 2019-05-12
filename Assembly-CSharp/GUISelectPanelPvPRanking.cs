using System;
using UnityEngine;

public class GUISelectPanelPvPRanking : GUISelectPanelViewPartsUD
{
	[Header("PvP用パーツ")]
	[SerializeField]
	private GameObject pvpSelectParts;

	[Header("ポイントクエスト用パーツ")]
	[SerializeField]
	private GameObject pqSelectParts;

	public override GameObject selectParts
	{
		get
		{
			if (CMD_PvPRanking.Mode == CMD_PvPRanking.MODE.PvP)
			{
				return this.pvpSelectParts;
			}
			if (CMD_PvPRanking.Mode == CMD_PvPRanking.MODE.PointQuest)
			{
				return this.pqSelectParts;
			}
			return this.pqSelectParts;
		}
	}
}
