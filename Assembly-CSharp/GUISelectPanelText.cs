using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelText : GUISelectPanelBSPartsUD
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

	public void AllBuild(List<GUIListPartsTextData> dts)
	{
		base.InitBuild();
		this.partsCount = dts.Count;
		int num = this.partsCount / this.PARTS_CT_MN;
		if (this.partsCount % this.PARTS_CT_MN > 0)
		{
			num++;
		}
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(this.PARTS_CT_MN, num, 1f, 1f);
			float num2 = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num3 = 0;
			foreach (GUIListPartsTextData data in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsText component = gameObject.GetComponent<GUIListPartsText>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num2, -5f));
					component.Data = data;
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
	}
}
