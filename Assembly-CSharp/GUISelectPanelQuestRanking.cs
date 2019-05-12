using System;
using UnityEngine;

public class GUISelectPanelQuestRanking : GUISelectPanelViewPartsUD
{
	[Header("ポイントクエスト用パーツ")]
	[SerializeField]
	private GameObject pqSelectParts;

	public override GameObject selectParts
	{
		get
		{
			return this.pqSelectParts;
		}
	}
}
