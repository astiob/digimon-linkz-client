using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GUISelectPanelDigiGarden : GUISelectPanelBSPartsUD
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void AllBuild(List<MonsterData> dts)
	{
		base.InitBuild();
		this.partsCount = dts.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num2 = 0;
			foreach (MonsterData data in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsDigiGarden component = gameObject.GetComponent<GUIListPartsDigiGarden>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.Data = data;
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
