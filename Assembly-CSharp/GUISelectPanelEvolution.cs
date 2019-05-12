using Evolution;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelEvolution : GUISelectPanelBSPartsUD
{
	public void AllBuild(List<EvolutionData.MonsterEvolveData> evolveDataList)
	{
		base.InitBuild();
		this.partsCount = evolveDataList.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num2 = 0;
			for (int i = 0; i < evolveDataList.Count; i++)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsEvolution component = gameObject.GetComponent<GUIListPartsEvolution>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.Data = evolveDataList[i];
				}
				num -= panelBuildData.pitchH;
				num2++;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
			base.SetActiveMargin(450f);
		}
	}
}
