using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelRank : GUISelectPanelBSPartsUD
{
	private int PARTS_CT_MN = 2;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public Vector3 AllBuild(List<GUIListPartsRank.RankData> dts)
	{
		base.InitBuild();
		this.partsCount = dts.Count;
		int num = this.partsCount / this.PARTS_CT_MN;
		if (this.partsCount % this.PARTS_CT_MN > 0)
		{
			num++;
		}
		Vector3 position = new Vector3(0f, 0f, 0f);
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(this.PARTS_CT_MN, num, 1f, 1f);
			float num2 = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num3 = 0;
			foreach (GUIListPartsRank.RankData data in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsRank component = gameObject.GetComponent<GUIListPartsRank>();
				if (component != null)
				{
					float x = startX + panelBuildData.pitchW * (float)(num3 % this.PARTS_CT_MN);
					component.SetOriginalPos(new Vector3(x, num2, -5f));
					if (num3 == 0)
					{
						position = component.transform.position;
					}
					component.Data = data;
					component.ShowGUI();
				}
				num3++;
				if (num3 % this.PARTS_CT_MN == 0)
				{
					num2 -= panelBuildData.pitchH;
				}
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
		return position;
	}
}
