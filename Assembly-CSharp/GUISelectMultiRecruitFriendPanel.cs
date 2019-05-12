using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectMultiRecruitFriendPanel : GUISelectPanelBSPartsUD
{
	public List<GUIListMultiRecruitFriendParts> friendPartsList;

	protected override void Awake()
	{
		base.Awake();
		this.friendPartsList = new List<GUIListMultiRecruitFriendParts>();
	}

	protected override void Update()
	{
		base.Update();
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
				GUIListMultiRecruitFriendParts component = gameObject.GetComponent<GUIListMultiRecruitFriendParts>();
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
}
