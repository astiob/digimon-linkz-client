using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectChatMemberPanel : GUISelectPanelBSPartsUD
{
	private GameWebAPI.ResponseData_ChatUserList.respUserList[] dataList;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void AllBuild(GameWebAPI.ResponseData_ChatUserList data)
	{
		base.InitBuild();
		if (CMD_ChatMenu.instance.openMemberListType == 4)
		{
			this.partsCount = ((data.requestList == null) ? 0 : data.requestList.Length);
			this.dataList = data.requestList;
		}
		else if (CMD_ChatMenu.instance.openMemberListType == 5)
		{
			this.partsCount = ((data.inviteList == null) ? 0 : data.inviteList.Length);
			this.dataList = data.inviteList;
		}
		else if (CMD_ChatMenu.instance.openMemberListType == 2)
		{
			this.partsCount = data.memberList.Length - 1;
			this.dataList = data.memberList;
		}
		else
		{
			this.partsCount = data.memberList.Length;
			this.dataList = data.memberList;
		}
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float startX = panelBuildData.startX;
			float num = panelBuildData.startY;
			if (this.dataList != null)
			{
				foreach (GameWebAPI.ResponseData_ChatUserList.respUserList data2 in this.dataList)
				{
					GameObject gameObject = base.AddBuildPart();
					GUIListChatMemberParts component = gameObject.GetComponent<GUIListChatMemberParts>();
					if (component != null)
					{
						component.SetOriginalPos(new Vector3(startX, num, -5f));
						component.Data = data2;
					}
					num -= panelBuildData.pitchH;
				}
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	public void HandoverListBuild(List<GameWebAPI.ResponseData_ChatUserList.respUserList> data)
	{
		base.InitBuild();
		this.partsCount = data.Count - 1;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float startX = panelBuildData.startX;
			float num = panelBuildData.startY;
			foreach (GameWebAPI.ResponseData_ChatUserList.respUserList respUserList in data)
			{
				if (CMD_ChatMenu.instance.openMemberListType != 2 || !(DataMng.Instance().UserId == respUserList.userId))
				{
					GameObject gameObject = base.AddBuildPart();
					GUIListChatMemberParts component = gameObject.GetComponent<GUIListChatMemberParts>();
					if (component != null)
					{
						component.SetOriginalPos(new Vector3(startX, num, -5f));
						component.Data = respUserList;
					}
					num -= panelBuildData.pitchH;
				}
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	public void AllFriendBuild(List<GameWebAPI.FriendList> data)
	{
		base.InitBuild();
		this.partsCount = data.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float startX = panelBuildData.startX;
			float num = panelBuildData.startY;
			foreach (GameWebAPI.FriendList friendData in data)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListChatMemberParts component = gameObject.GetComponent<GUIListChatMemberParts>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.FriendData = friendData;
				}
				num -= panelBuildData.pitchH;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
			CMD_ChatInvitation.instance.partFriendListObjs = this.partObjs;
		}
	}
}
