using System;
using UnityEngine;

public class GUISelectPvPListPanel : GUISelectPanelBSPartsUD
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void AllBuild(GameWebAPI.RespData_ColosseumMockBattleRequestListLogic data)
	{
		int num = 0;
		base.InitBuild();
		if (data.memberList != null)
		{
			this.partsCount = data.memberList.Length;
			if (base.selectCollider != null)
			{
				GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
				float startX = panelBuildData.startX;
				float num2 = panelBuildData.startY;
				foreach (GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MemberList memberList2 in data.memberList)
				{
					GameObject gameObject = base.AddBuildPart();
					GUIListPvPListParts component = gameObject.GetComponent<GUIListPvPListParts>();
					if (component != null)
					{
						component.SetOriginalPos(new Vector3(startX, num2, -5f));
						component.Data = memberList2;
						component.mockBattleStatus = data.GetMockBattleStatus;
					}
					if (memberList2.userInfo.requestTime != null && num < int.Parse(memberList2.userInfo.requestTime))
					{
						num = int.Parse(memberList2.userInfo.requestTime);
					}
					num2 -= panelBuildData.pitchH;
				}
				if (num > 0)
				{
					PlayerPrefs.SetInt("lastPvPMockTime", num);
				}
				base.height = panelBuildData.lenH;
				base.InitMinMaxLocation(-1, 0f);
			}
		}
	}

	public void AllRelease()
	{
		base.InitBuild();
	}
}
