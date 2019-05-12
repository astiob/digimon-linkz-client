using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPvPFriendPanel : GUISelectPanelBSPartsUD
{
	private List<GUIListPvPFriendParts> friendPartsList;

	public List<GUIListPvPFriendParts> FriendPartsList
	{
		get
		{
			return this.friendPartsList;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.friendPartsList = new List<GUIListPvPFriendParts>();
	}

	public void AllBuild(List<GameWebAPI.FriendList> data)
	{
		base.InitBuild();
		this.partsCount = data.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			base.height = panelBuildData.lenH;
			foreach (GameWebAPI.FriendList friendData in data)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPvPFriendParts component = gameObject.GetComponent<GUIListPvPFriendParts>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(-20f, num, -5f));
					component.FriendData = friendData;
					this.friendPartsList.Add(component);
				}
				num -= panelBuildData.pitchH;
			}
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	public void UnSelectedAnother(GUIListPvPFriendParts part)
	{
		foreach (GUIListPvPFriendParts guilistPvPFriendParts in this.friendPartsList)
		{
			if (guilistPvPFriendParts != part)
			{
				guilistPvPFriendParts.ForceRelease();
			}
		}
	}
}
