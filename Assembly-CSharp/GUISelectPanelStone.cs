using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelStone : GUISelectPanelBSPartsUD
{
	[SerializeField]
	private GameObject goPackParts;

	public Action Callback { private get; set; }

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public Vector3 AllBuild(List<StoreUtil.StoneStoreData> dts)
	{
		base.InitBuild();
		this.partsCount = dts.Count;
		Vector3 position = new Vector3(0f, 0f, 0f);
		if (base.selectCollider != null)
		{
			GUISelectPanelStone.PanelBuildDataForShop panelBuildDataForShop = this.CalcBuildDataForShop(dts);
			for (int i = 0; i < dts.Count; i++)
			{
				StoreUtil.StoneStoreData stoneStoreData = dts[i];
				GameObject selectParts;
				if (stoneStoreData.packFlg)
				{
					selectParts = this.goPackParts;
				}
				else
				{
					selectParts = base.selectParts;
				}
				GameObject gameObject = base.AddBuildPart(selectParts);
				GUIListPartsStone component = gameObject.GetComponent<GUIListPartsStone>();
				if (component != null)
				{
					component.Callback = this.Callback;
					component.SetOriginalPos(panelBuildDataForShop.posList[i]);
					if (i == 0)
					{
						position = component.transform.position;
					}
					component.Data = stoneStoreData;
				}
			}
			base.height = panelBuildDataForShop.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
		return position;
	}

	public GUISelectPanelStone.PanelBuildDataForShop CalcBuildDataForShop(List<StoreUtil.StoneStoreData> dts)
	{
		GUISelectPanelStone.PanelBuildDataForShop panelBuildDataForShop = new GUISelectPanelStone.PanelBuildDataForShop();
		panelBuildDataForShop.posList = new List<Vector3>();
		if (base.selectParts != null && !base.selectParts.activeSelf)
		{
			base.selectParts.SetActive(true);
		}
		float width = base.selectCollider.width;
		float height = base.selectCollider.height;
		float num = width + this.horizontalMargin;
		GUICollider component = this.goPackParts.GetComponent<GUICollider>();
		float height2 = component.height;
		bool flag = false;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = num * 2f - this.horizontalMargin;
		float num5 = -(num4 / 2f) + width / 2f;
		float y = 0f;
		float z = -5f;
		for (int i = 0; i < dts.Count; i++)
		{
			StoreUtil.StoneStoreData stoneStoreData = dts[i];
			float x;
			if (stoneStoreData.packFlg)
			{
				x = 0f;
				if (i == 0)
				{
					num3 -= this.verticalBorder;
				}
				else
				{
					num3 -= this.verticalMargin;
				}
				num3 -= height2;
				y = num3 + height2 / 2f;
				flag = false;
			}
			else if (!flag)
			{
				x = num5;
				if (i == 0)
				{
					num3 -= this.verticalBorder;
				}
				else
				{
					num3 -= this.verticalMargin;
				}
				num3 -= height;
				y = num3 + height / 2f;
				flag = true;
			}
			else
			{
				x = num5 + num;
				flag = false;
			}
			Vector3 item = new Vector3(x, y, z);
			panelBuildDataForShop.posList.Add(item);
		}
		num3 -= this.verticalBorder;
		panelBuildDataForShop.lenH = Mathf.Abs(num3);
		float num6 = Mathf.Abs(num3) / 2f;
		num2 += num6;
		num3 += num6;
		for (int i = 0; i < dts.Count; i++)
		{
			Vector3 value = panelBuildDataForShop.posList[i];
			value.y += num6;
			panelBuildDataForShop.posList[i] = value;
		}
		return panelBuildDataForShop;
	}

	public class PanelBuildDataForShop
	{
		public float lenH;

		public List<Vector3> posList;
	}
}
