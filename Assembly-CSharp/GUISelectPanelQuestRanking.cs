using System;
using System.Linq;
using UnityEngine;

public class GUISelectPanelQuestRanking : GUISelectPanelViewPartsUD
{
	[SerializeField]
	[Header("ポイントクエスト用パーツ")]
	private GameObject pqSelectParts;

	public override GameObject selectParts
	{
		get
		{
			return this.pqSelectParts;
		}
	}

	public void DisableList()
	{
		if (this.partObjs != null && this.partObjs.Count<GUISelectPanelViewControlUD.ListPartsData>() > 0)
		{
			foreach (GUISelectPanelViewControlUD.ListPartsData listPartsData in this.partObjs)
			{
				if (listPartsData != null && listPartsData.csParts != null)
				{
					listPartsData.csParts.gameObject.SetActive(false);
				}
			}
		}
	}
}
