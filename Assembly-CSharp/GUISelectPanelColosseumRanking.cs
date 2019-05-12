using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUISelectPanelColosseumRanking : GUISelectPanelViewPartsUD
{
	public static List<GameWebAPI.RespDataCL_Ranking.RankingData> partsDataList;

	[Header("コロシアム用パーツ")]
	[SerializeField]
	private GameObject coSelectParts;

	private float panelUpdateTime;

	private float beforeMaxLocate;

	protected override void Update()
	{
		base.Update();
		this.panelUpdateTime += Time.deltaTime;
		if (base.gameObject.transform.localPosition.y > base.maxLocate + 150f && this.panelUpdateTime > 1f)
		{
			CMD_ColosseumRanking.instance.UpdateRankingList();
			this.panelUpdateTime = 0f;
		}
	}

	public override GameObject selectParts
	{
		get
		{
			return this.coSelectParts;
		}
	}

	public void SetData(GameWebAPI.RespDataCL_Ranking data)
	{
		if (data.rankingMember != null)
		{
			GUISelectPanelColosseumRanking.partsDataList = data.rankingMember.ToList<GameWebAPI.RespDataCL_Ranking.RankingData>();
		}
		else
		{
			GUISelectPanelColosseumRanking.partsDataList = new List<GameWebAPI.RespDataCL_Ranking.RankingData>();
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

	public void SetBeforeMaxLocate()
	{
		this.beforeMaxLocate = base.maxLocate * 2f;
	}

	public void SetSelectLocate()
	{
		if (this.beforeMaxLocate > 0f)
		{
			this.selectLoc = base.minLocate + this.beforeMaxLocate + base.GetPanelBuildData().pitchH * 2f;
		}
	}
}
