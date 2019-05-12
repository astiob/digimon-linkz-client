using System;
using UnityEngine;

public class GUISelectPanelFacility : GUISelectPanelBSPartsUD
{
	[SerializeField]
	private int horizontalPartsCount = 2;

	public void AllBuild(int listItemCount)
	{
		base.InitBuild();
		this.partsCount = listItemCount;
		float height = base.selectCollider.height;
		float width = base.selectCollider.width;
		float num = height + this.verticalMargin;
		float num2 = width + this.horizontalMargin;
		int num3 = this.partsCount / this.horizontalPartsCount;
		if (this.partsCount % this.horizontalPartsCount > 0)
		{
			num3++;
		}
		float num4 = (float)num3 * num + this.verticalBorder * 2f;
		num4 -= this.verticalMargin;
		float num5 = base.ListWindowViewRect.xMin + this.horizontalBorder + width * 0.5f;
		float num6 = num4 * 0.5f - this.verticalBorder - height * 0.5f;
		for (int i = 0; i < listItemCount; i++)
		{
			float x = num5 + num2 * (float)(i % this.horizontalPartsCount);
			float y = num6 - num * (float)(i / this.horizontalPartsCount);
			GUIListPartBS component = base.AddBuildPart().GetComponent<GUIListPartBS>();
			component.SetOriginalPos(new Vector3(x, y, -5f));
		}
		base.height = num4;
		base.InitMinMaxLocation(-1, 0f);
	}

	private void Close()
	{
	}

	private void OnDisable()
	{
		base.scrollBar.SetActive(false);
		base.scrollBarBG.SetActive(false);
	}
}
