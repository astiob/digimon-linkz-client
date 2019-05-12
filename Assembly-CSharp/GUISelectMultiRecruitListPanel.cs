using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUISelectMultiRecruitListPanel : GUISelectPanelBSPartsUD
{
	private GameWebAPI.ResponseData_Common_MultiRoomList nowRoomList;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void AllBuild(GameWebAPI.ResponseData_Common_MultiRoomList data)
	{
		this.nowRoomList = data;
		int num = 0;
		base.InitBuild();
		if (data.multiRoomList != null)
		{
			this.partsCount = data.multiRoomList.Length;
			if (base.selectCollider != null)
			{
				GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
				float startX = panelBuildData.startX;
				float num2 = panelBuildData.startY;
				foreach (GameWebAPI.ResponseData_Common_MultiRoomList.room room in data.multiRoomList)
				{
					GameObject gameObject = base.AddBuildPart();
					GUIListMultiRecruitListParts component = gameObject.GetComponent<GUIListMultiRecruitListParts>();
					if (component != null)
					{
						component.SetOriginalPos(new Vector3(startX, num2, -5f));
						component.Data = room;
					}
					if (room.multiRoomRequestId != null && num < int.Parse(room.multiRoomRequestId))
					{
						num = int.Parse(room.multiRoomRequestId);
					}
					num2 -= panelBuildData.pitchH;
				}
				if (num > 0)
				{
					PlayerPrefs.SetInt("lastMultiReqId", num);
				}
				base.height = panelBuildData.lenH;
				base.InitMinMaxLocation(-1, 0f);
			}
		}
	}

	public void ReBuild(List<string> excludeRoomIdList)
	{
		base.InitBuild();
		if (this.nowRoomList.multiRoomList != null)
		{
			List<GameWebAPI.ResponseData_Common_MultiRoomList.room> list = this.nowRoomList.multiRoomList.ToList<GameWebAPI.ResponseData_Common_MultiRoomList.room>();
			if (excludeRoomIdList.Count > 0)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					foreach (string b in excludeRoomIdList)
					{
						if (list[i].multiRoomId == b)
						{
							list.RemoveAt(i);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				this.nowRoomList.multiRoomList = list.ToArray();
				this.partsCount = this.nowRoomList.multiRoomList.Length;
				if (base.selectCollider != null)
				{
					GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
					float startX = panelBuildData.startX;
					float num = panelBuildData.startY;
					foreach (GameWebAPI.ResponseData_Common_MultiRoomList.room data in list)
					{
						GameObject gameObject = base.AddBuildPart();
						GUIListMultiRecruitListParts component = gameObject.GetComponent<GUIListMultiRecruitListParts>();
						if (component != null)
						{
							component.SetOriginalPos(new Vector3(startX, num, -5f));
							component.Data = data;
						}
						num -= panelBuildData.pitchH;
					}
					base.height = panelBuildData.lenH;
					base.InitMinMaxLocation(-1, 0f);
				}
			}
			else
			{
				CMD_MultiRecruitTop.instance.ClickUpdateBtn();
			}
		}
	}
}
