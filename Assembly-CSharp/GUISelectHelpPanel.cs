using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUISelectHelpPanel : GUISelectPanelBSPartsUD
{
	private const int LIST_ITEM_COUNT = 10;

	private int PARTS_CT_MN = 2;

	private int minIdx;

	private int maxIdx;

	private List<GUISelectHelpPanel.helpPartsData> helpPartsDataList;

	protected override void Awake()
	{
		base.Awake();
		this.maxIdx = 10;
		this.helpPartsDataList = new List<GUISelectHelpPanel.helpPartsData>();
	}

	protected override void Update()
	{
		base.Update();
	}

	private void RectSet(float ymin, float ymax)
	{
		base.ListWindowViewRect = new Rect
		{
			yMin = ymin - GUIMain.VerticalSpaceSize,
			yMax = ymax + GUIMain.VerticalSpaceSize
		};
	}

	public void AllBuildCategory(GameWebAPI.RespDataMA_GetHelpCategoryM dts)
	{
		this.RectSet(-270f, 250f);
		base.InitBuild();
		base.initLocation = true;
		if (base.selectCollider != null)
		{
			this.partsCount = dts.helpCategoryM.Length;
			int num = this.partsCount / this.PARTS_CT_MN;
			if (this.partsCount % this.PARTS_CT_MN > 0)
			{
				num++;
			}
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(this.PARTS_CT_MN, num, 1f, 1f);
			float num2 = panelBuildData.startY;
			float num3 = panelBuildData.startX;
			base.height = panelBuildData.lenH;
			foreach (var <>__AnonType in dts.helpCategoryM.Select((GameWebAPI.RespDataMA_GetHelpCategoryM.HelpCategoryM category, int index) => new
			{
				category,
				index
			}))
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListHelpParts component = gameObject.GetComponent<GUIListHelpParts>();
				component.CategoryData = <>__AnonType.category;
				component.SetOriginalPos(new Vector3(num3, num2, -5f));
				gameObject.SetActive(true);
				if (<>__AnonType.index % 2 == 1)
				{
					num2 -= panelBuildData.pitchH;
					num3 -= panelBuildData.pitchW;
				}
				else
				{
					num3 += panelBuildData.pitchW;
				}
			}
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	public void AllBuildList(GameWebAPI.RespDataMA_GetHelpM dts, string helpCategoryId)
	{
		this.RectSet(-300f, 250f);
		base.InitBuild();
		base.initLocation = true;
		if (base.selectCollider != null)
		{
			int num = 0;
			foreach (GameWebAPI.RespDataMA_GetHelpM.HelpM helpM2 in dts.helpM)
			{
				if (helpM2.helpCategoryId == helpCategoryId)
				{
					num++;
				}
			}
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, num, 1f, 1f);
			float num2 = panelBuildData.startY;
			base.height = panelBuildData.lenH;
			int num3 = 0;
			for (int j = 0; j < dts.helpM.Length; j++)
			{
				if (dts.helpM[j].helpCategoryId == helpCategoryId)
				{
					if (num3 <= this.maxIdx)
					{
						GameObject gameObject = base.AddBuildPart();
						GUIListHelpParts component = gameObject.GetComponent<GUIListHelpParts>();
						component.ListData = dts.helpM[j];
						component.SetOriginalPos(new Vector3(0f, num2, -5f));
					}
					GUISelectHelpPanel.helpPartsData helpPartsData = new GUISelectHelpPanel.helpPartsData();
					helpPartsData.listData = dts.helpM[j];
					helpPartsData.index = num3;
					helpPartsData.pos = new Vector3(0f, num2, -5f);
					this.helpPartsDataList.Add(helpPartsData);
					num2 -= panelBuildData.pitchH;
					num3++;
				}
			}
			base.SetAreaOverTopAction(new Action<GUIListPartBS>(this.PanelReCyclingTop));
			base.SetAreaOverBottomAction(new Action<GUIListPartBS>(this.PanelReCyclingBottom));
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	private void PanelReCyclingTop(GUIListPartBS obj)
	{
		if (this.maxIdx + 1 < this.helpPartsDataList.Count)
		{
			GUIListPartBS guilistPartBS = this.partObjs.Where((GUIListPartBS part) => part.IDX == this.minIdx).Min<GUIListPartBS>();
			this.minIdx++;
			this.maxIdx++;
			this.SetListParam((GUIListHelpParts)guilistPartBS, this.maxIdx);
		}
	}

	private void PanelReCyclingBottom(GUIListPartBS obj)
	{
		if (this.minIdx > 0)
		{
			GUIListPartBS guilistPartBS = this.partObjs.Where((GUIListPartBS part) => part.IDX == this.maxIdx).Max<GUIListPartBS>();
			this.minIdx--;
			this.maxIdx--;
			this.SetListParam((GUIListHelpParts)guilistPartBS, this.minIdx);
		}
	}

	private void SetListParam(GUIListHelpParts part, int index)
	{
		part.isUpdate = true;
		part.IDX = this.helpPartsDataList[index].index;
		part.ListData = this.helpPartsDataList[index].listData;
		part.SetOriginalPos(this.helpPartsDataList[index].pos);
	}

	private class helpPartsData
	{
		public GameWebAPI.RespDataMA_GetHelpM.HelpM listData;

		public int index;

		public Vector3 pos;
	}
}
