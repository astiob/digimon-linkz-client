using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUISelectChatGroupPanel : GUISelectPanelBSPartsUD
{
	private float panelUpdateTime;

	public GameWebAPI.ResponseData_ChatGroupList.lists[] dataList;

	public List<GUIListChatGroupParts> chatPartsList;

	protected override void Awake()
	{
		base.Awake();
		this.chatPartsList = new List<GUIListChatGroupParts>();
	}

	protected override void Update()
	{
		base.Update();
		if (CMD_ChatTop.instance != null && CMD_ChatTop.instance.isChatPaging)
		{
			this.panelUpdateTime += Time.deltaTime;
			if (this.panelUpdateTime > 1f && base.gameObject.transform.localPosition.y > base.maxLocate + 150f && !CMD_ChatTop.instance.isGetChatGroupListMax)
			{
				CMD_ChatTop.instance.PagingChatGroupList();
				this.panelUpdateTime = 0f;
			}
		}
	}

	public void AllBuild(GameWebAPI.ResponseData_ChatGroupList data)
	{
		base.InitBuild();
		bool flag = false;
		if (data.inviteList != null)
		{
			this.partsCount = data.inviteList.Length;
			this.dataList = data.inviteList;
			flag = true;
		}
		else if (data.requestList != null)
		{
			this.partsCount = data.requestList.Length;
			this.dataList = data.requestList;
			flag = true;
		}
		else if (data.groupList != null)
		{
			this.partsCount = data.groupList.Length;
			this.dataList = data.groupList;
		}
		else
		{
			this.partsCount = 0;
			this.dataList = data.groupList;
		}
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float startX = panelBuildData.startX;
			float num = panelBuildData.startY;
			if (this.dataList != null)
			{
				GameWebAPI.ResponseData_ChatGroupList.lists[] array = this.dataList;
				for (int i = 0; i < array.Length; i++)
				{
					GameWebAPI.ResponseData_ChatGroupList.lists dt = array[i];
					int num2 = 0;
					if (BlockManager.instance().blockList != null && flag)
					{
						num2 = BlockManager.instance().blockList.Where((GameWebAPI.FriendList item) => item.userData.userId == dt.ownerUserId).ToList<GameWebAPI.FriendList>().Count;
					}
					if (num2 == 0)
					{
						GameObject gameObject = base.AddBuildPart();
						GUIListChatGroupParts component = gameObject.GetComponent<GUIListChatGroupParts>();
						if (component != null)
						{
							component.SetOriginalPos(new Vector3(startX, num, -5f));
							component.Data = dt;
						}
						num -= panelBuildData.pitchH;
						this.chatPartsList.Add(component);
					}
				}
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	public void AddBuild(GameWebAPI.ResponseData_ChatGroupList data)
	{
		if (data.groupList != null)
		{
			this.partsCount += data.groupList.Length;
		}
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float startX = panelBuildData.startX;
			float num = panelBuildData.startY;
			foreach (GameWebAPI.ResponseData_ChatGroupList.lists data2 in data.groupList)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListChatGroupParts component = gameObject.GetComponent<GUIListChatGroupParts>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.Data = data2;
				}
			}
			foreach (GUIListPartBS guilistPartBS in this.partObjs)
			{
				guilistPartBS.gameObject.transform.localPosition = new Vector3(guilistPartBS.gameObject.transform.localPosition.x, num, guilistPartBS.gameObject.transform.localPosition.z);
				num -= panelBuildData.pitchH;
			}
			base.height = panelBuildData.lenH;
			base.initEffectFlg = true;
			this.selectLoc -= panelBuildData.pitchH;
			base.InitMinMaxLocation(-1, 0f);
		}
	}
}
