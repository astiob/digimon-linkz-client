using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelViewControlUD : GUICollider
{
	public List<GUISelectPanelViewControlUD.ListPartsData> partObjs;

	private GameObject goParts;

	private float fY_PTH;

	private float fViewMinY;

	private float fViewMaxY;

	private int x_size;

	private int sector_size;

	protected float _sclX;

	protected float _sclY;

	protected bool useVariableY;

	private int sector_ct;

	private int nowStartIDX;

	protected int prvStartIDX;

	protected List<GUISelectPanelViewControlUD.ListPartsRecycle> csPartsList;

	public GUISelectPanelViewControlUD.ListPartsData GetListPartsData(int idx)
	{
		return this.partObjs[idx];
	}

	public bool IsVariableY()
	{
		return this.useVariableY;
	}

	protected void InitViewControl(GameObject _goParts, float _fY_PTH, float _fViewMinY, float _fViewMaxY, int _x_size, int _sector_size, bool isResizeListArea)
	{
		this.goParts = _goParts;
		this.fY_PTH = _fY_PTH;
		if (isResizeListArea)
		{
			this.fViewMinY = _fViewMinY - GUIMain.VerticalSpaceSize;
			this.fViewMaxY = _fViewMaxY + GUIMain.VerticalSpaceSize;
		}
		else
		{
			this.fViewMinY = _fViewMinY;
			this.fViewMaxY = _fViewMaxY;
		}
		this.x_size = _x_size;
		this.sector_size = _sector_size;
		this.sector_ct = this.CalcSectorCT();
		this.nowStartIDX = -1;
		this.prvStartIDX = -1;
		this.BuildFirstView();
		this.UpdateView();
	}

	private void BuildFirstView()
	{
		if (this.csPartsList == null)
		{
			this.csPartsList = new List<GUISelectPanelViewControlUD.ListPartsRecycle>();
			int num = this.x_size * this.sector_size * this.sector_ct;
			GUIListPartBS component = this.goParts.GetComponent<GUIListPartBS>();
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goParts);
				GUIListPartBS component2 = gameObject.GetComponent<GUIListPartBS>();
				component2.parent = this;
				component2.ReceiveOriginalParts(component);
				Vector3 localScale = component2.transform.localScale;
				component2.transform.parent = base.transform;
				localScale.x *= this._sclX;
				localScale.y *= this._sclY;
				component2.transform.localScale = localScale;
				GUISelectPanelViewControlUD.ListPartsRecycle listPartsRecycle = new GUISelectPanelViewControlUD.ListPartsRecycle();
				listPartsRecycle.csParts = component2;
				listPartsRecycle.csParts.IDX = -1;
				listPartsRecycle.isInit = false;
				this.csPartsList.Add(listPartsRecycle);
				gameObject.SetActive(false);
			}
		}
	}

	protected void RefreshView()
	{
		int num = this.x_size * this.sector_size * this.sector_ct;
		int num2 = this.prvStartIDX + num;
		if (num2 > this.partObjs.Count)
		{
			num2 = this.partObjs.Count;
		}
		int num3 = this.nowStartIDX + num;
		if (num3 > this.partObjs.Count)
		{
			num3 = this.partObjs.Count;
		}
		int i = 0;
		if (this.prvStartIDX == -1)
		{
			for (int j = this.nowStartIDX; j < num3; j++)
			{
				this.UpdateParts(this.csPartsList[i], this.partObjs[j]);
				i++;
			}
		}
		else
		{
			this.ShiftRecycleParts();
			for (int j = 0; j < this.partObjs.Count; j++)
			{
				if (this.nowStartIDX <= j && j < num3)
				{
					if (this.csPartsList[i].isShifted)
					{
						i++;
					}
					else
					{
						this.UpdateParts(this.csPartsList[i], this.partObjs[j]);
						i++;
					}
				}
				else
				{
					this.partObjs[j].csParts = null;
				}
			}
			while (i < this.csPartsList.Count)
			{
				this.csPartsList[i].csParts.gameObject.SetActive(false);
				this.csPartsList[i].csParts.InactiveParts();
				this.csPartsList[i].csParts.IDX = -1;
				i++;
			}
		}
	}

	private void UpdateParts(GUISelectPanelViewControlUD.ListPartsRecycle partsObject, GUISelectPanelViewControlUD.ListPartsData partsData)
	{
		partsObject.csParts.gameObject.SetActive(true);
		if (this.useVariableY)
		{
			BoxCollider component = partsObject.csParts.gameObject.GetComponent<BoxCollider>();
			if (component != null)
			{
				Vector3 size = component.size;
				size.y = partsData.sizeY;
				component.size = size;
			}
		}
		partsObject.csParts.SetOriginalPos(partsData.vPos);
		partsData.csParts = partsObject.csParts;
		partsObject.csParts.IDX = partsData.idx;
		partsObject.csParts.SetData();
		if (!partsObject.isInit)
		{
			partsObject.isInit = true;
			partsObject.csParts.InitParts();
		}
		else
		{
			partsObject.csParts.RefreshParts();
		}
		partsObject.csParts.ShowGUI();
	}

	private void ShiftRecycleParts()
	{
		int num = this.x_size * this.sector_size * this.sector_ct;
		int num2 = this.prvStartIDX + num - 1;
		if (num2 > this.partObjs.Count - 1)
		{
			num2 = this.partObjs.Count - 1;
		}
		int num3 = this.nowStartIDX + num - 1;
		if (num3 > this.partObjs.Count - 1)
		{
			num3 = this.partObjs.Count - 1;
		}
		for (int i = 0; i < this.csPartsList.Count; i++)
		{
			this.csPartsList[i].isShifted = false;
		}
		if (num3 >= this.prvStartIDX && num2 >= this.nowStartIDX)
		{
			int num4;
			int num5;
			if (this.prvStartIDX < this.nowStartIDX)
			{
				num4 = this.nowStartIDX;
				num5 = 0;
			}
			else
			{
				num4 = this.prvStartIDX;
				num5 = this.prvStartIDX - this.nowStartIDX;
			}
			int num6;
			if (num2 < num3)
			{
				num6 = num2;
			}
			else
			{
				num6 = num3;
			}
			int num7 = 0;
			int num8 = 0;
			int i;
			for (i = 0; i < this.csPartsList.Count; i++)
			{
				if (this.csPartsList[i].csParts.IDX == num4)
				{
					num7 = i;
				}
				if (this.csPartsList[i].csParts.IDX == num6)
				{
					num8 = i;
				}
			}
			for (i = num7; i <= num8; i++)
			{
				this.csPartsList[i].isShifted = true;
			}
			i = num7;
			int num9 = num5;
			for (int j = this.csPartsList.Count; j > 0; j--)
			{
				this.csPartsList[i].sortTmp = num9;
				num9 = (num9 + 1) % this.csPartsList.Count;
				i = (i + 1) % this.csPartsList.Count;
			}
			this.csPartsList.Sort(new Comparison<GUISelectPanelViewControlUD.ListPartsRecycle>(this.CompareTMP));
		}
	}

	private int CompareTMP(GUISelectPanelViewControlUD.ListPartsRecycle x, GUISelectPanelViewControlUD.ListPartsRecycle y)
	{
		if (x.sortTmp < y.sortTmp)
		{
			return -1;
		}
		if (x.sortTmp > y.sortTmp)
		{
			return 1;
		}
		return 0;
	}

	private int CalcStartIDX()
	{
		int num = this.x_size * this.sector_size;
		int num2 = 0;
		int i = num2 + num - 1;
		int num3 = this.partObjs.Count - 1;
		while (i <= num3)
		{
			float num4 = this.partObjs[num2].vPos.y + base.transform.localPosition.y;
			float num5 = this.partObjs[i].vPos.y + base.transform.localPosition.y;
			if (this.fViewMaxY > num4 || this.fViewMaxY > num5)
			{
				return num2;
			}
			num2 += num;
			i = num2 + num - 1;
		}
		return num2;
	}

	private int CalcSectorCT()
	{
		int num = (int)(this.fViewMaxY - this.fViewMinY);
		int num2 = (int)this.fY_PTH * this.sector_size;
		int num3 = num / num2;
		if (num % num2 == 0)
		{
			num3++;
		}
		else
		{
			num3 += 2;
		}
		return num3;
	}

	private void UpdateView()
	{
		if (this.partObjs == null)
		{
			return;
		}
		this.nowStartIDX = this.CalcStartIDX();
		if (this.nowStartIDX != this.prvStartIDX)
		{
			this.RefreshView();
			this.prvStartIDX = this.nowStartIDX;
		}
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateView();
	}

	protected void InitializeView()
	{
		for (int i = 0; i < this.csPartsList.Count; i++)
		{
			this.csPartsList[i].csParts.gameObject.SetActive(false);
			this.csPartsList[i].csParts.InactiveParts();
			this.csPartsList[i].csParts.IDX = -1;
		}
		this.nowStartIDX = -1;
		this.prvStartIDX = -1;
		this.UpdateView();
	}

	public class ListPartsData
	{
		public GUIListPartBS csParts;

		public int idx;

		public Vector3 vPos;

		public float sizeY;
	}

	public class ListPartsRecycle
	{
		public GUIListPartBS csParts;

		public bool isInit;

		public bool isShifted;

		public int sortTmp;
	}
}
