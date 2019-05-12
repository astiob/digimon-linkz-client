using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GUISelectPanelDigiGarden : GUISelectPanelBSPartsUD
{
	public void AllBuild(List<MonsterData> monsterDataList, Action<CMD, string, string> bornAction, Action<MonsterData> pushEvolutionButton)
	{
		base.InitBuild();
		this.partsCount = monsterDataList.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num2 = 0;
			foreach (MonsterData monsterData in monsterDataList)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsDigiGarden component = gameObject.GetComponent<GUIListPartsDigiGarden>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.Initialize(monsterData);
					component.SetCallback(bornAction, pushEvolutionButton);
					component.ShowGUI();
				}
				num2++;
				num -= panelBuildData.pitchH;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
			base.EnableScroll = false;
		}
	}
}
