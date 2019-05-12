using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelExchange : GUISelectPanelBSPartsUD
{
	private int PARTS_CT_MN = 1;

	public Vector3 AllBuild(int listItemCount, out List<ExchangeItem> itemList)
	{
		base.InitBuild();
		this.partsCount = listItemCount;
		int num = this.partsCount / this.PARTS_CT_MN;
		if (this.partsCount % this.PARTS_CT_MN > 0)
		{
			num++;
		}
		Vector3 position = new Vector3(0f, 0f, 0f);
		itemList = new List<ExchangeItem>();
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(this.PARTS_CT_MN, num, 1f, 1f);
			float num2 = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num3 = 0;
			for (int i = 0; i < this.partsCount; i++)
			{
				GameObject gameObject = base.AddBuildPart();
				ExchangeItem component = gameObject.GetComponent<ExchangeItem>();
				if (component != null)
				{
					float x = startX + panelBuildData.pitchW * (float)(num3 % this.PARTS_CT_MN);
					component.SetOriginalPos(new Vector3(x, num2, -5f));
					if (num3 == 0)
					{
						position = component.transform.position;
					}
					itemList.Add(component);
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
