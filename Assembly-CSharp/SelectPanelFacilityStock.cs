using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectPanelFacilityStock : GUISelectPanelBSPartsUD
{
	private int PARTS_CT_MN = 2;

	public void AllBuild(List<ListPartsFacilityStock.FacilityStockListData> dts, Action<int> _actCallBackPlace)
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
			foreach (ListPartsFacilityStock.FacilityStockListData stockFacility in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				ListPartsFacilityStock component = gameObject.GetComponent<ListPartsFacilityStock>();
				if (component != null)
				{
					float x = startX + panelBuildData.pitchW * (float)(num3 % this.PARTS_CT_MN);
					component.SetOriginalPos(new Vector3(x, num2, -5f));
					component.StockFacility = stockFacility;
					component.SetDetail(_actCallBackPlace);
					component.parent = this;
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
