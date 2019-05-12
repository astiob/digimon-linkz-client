using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUISelectPanelEvolutionItemList : GUISelectPanelViewPartsUD
{
	private List<GameWebAPI.UserSoulData> dispSoulList;

	private List<GameWebAPI.UserSoulData> userPluginDataList;

	public List<GameWebAPI.UserSoulData> partsDataList;

	[SerializeField]
	[Header("進化素材パーツ")]
	private GameObject soulSelectParts;

	private int loadingStatus;

	public override GameObject selectParts
	{
		get
		{
			return this.soulSelectParts;
		}
	}

	public void SetData(GameWebAPI.UserSoulData[] data)
	{
		this.partsDataList = data.ToList<GameWebAPI.UserSoulData>();
	}

	public void setStatusLoading()
	{
		this.loadingStatus = 1;
	}

	public bool isLoading()
	{
		return this.loadingStatus == 1;
	}

	public void setStatusLoaded()
	{
		this.loadingStatus = 2;
	}

	public bool isLoaded()
	{
		return this.loadingStatus == 2;
	}
}
