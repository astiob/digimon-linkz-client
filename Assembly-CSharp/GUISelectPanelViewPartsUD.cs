using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelViewPartsUD : GUISelectPanelViewUD
{
	[SerializeField]
	private GameObject _selectParts;

	[SerializeField]
	private GameObject _scrollBar;

	private UISprite NGScrollBarPart;

	[SerializeField]
	private GameObject _scrollBarBG;

	private UISprite NGScrollBarBGPart;

	private GUICollider scrollBarBGCollider;

	private BoxCollider scrollBarBGBox;

	private GUICollider _selectCollider;

	[Header("リサイクルビュー X方向 パーツカウント")]
	[SerializeField]
	protected int PARTS_CT_MN = 1;

	[Header("リサイクルビュー Y MAX")]
	[SerializeField]
	protected float fRecycleViewMaxY = 500f;

	[Header("リサイクルビュー Y MIN")]
	[SerializeField]
	protected float fRecycleViewMinY = -500f;

	[Header("リサイクルビュー セクターサイズ")]
	[SerializeField]
	protected int RecycleViewSectorSize = 4;

	[SerializeField]
	private GameObject goSelectPanelParam;

	private SelectPanelParamUD selectPanelParam;

	private GUISelectPanelViewPartsUD.PanelBuildData _pbd = new GUISelectPanelViewPartsUD.PanelBuildData();

	public float verticalBorder = 32f;

	public float verticalMargin = 8f;

	public float horizontalBorder = 32f;

	public float horizontalMargin = 8f;

	private bool hideNonMoveScrollBar = true;

	private string prefName = string.Empty;

	private float zpos = -5f;

	protected int partsCount;

	private float scrollBarPosX;

	private float scrollBarBGPosX;

	private Vector3 v3tmp = new Vector3(1f, 1f, 1f);

	private bool refreshPanel = true;

	private bool hideScrollBarAllWays;

	public virtual GameObject selectParts
	{
		get
		{
			return this._selectParts;
		}
		set
		{
			this._selectParts = value;
		}
	}

	protected GameObject scrollBar
	{
		get
		{
			return this._scrollBar;
		}
		set
		{
			this._scrollBar = value;
		}
	}

	protected GameObject scrollBarBG
	{
		get
		{
			return this._scrollBarBG;
		}
		set
		{
			this._scrollBarBG = value;
		}
	}

	protected GUICollider selectCollider
	{
		get
		{
			if (this._selectCollider == null)
			{
				this.GetSelectCollider();
			}
			return this._selectCollider;
		}
	}

	private GUICollider GetSelectCollider()
	{
		if (this._selectParts != null)
		{
			this._selectCollider = this._selectParts.GetComponent<GUICollider>();
			return this._selectCollider;
		}
		this._selectCollider = null;
		return null;
	}

	public CMD InstanceCMD { get; set; }

	protected void SetSelectPanelParam()
	{
		if (this.goSelectPanelParam != null)
		{
			this.selectPanelParam = this.goSelectPanelParam.GetComponent<SelectPanelParamUD>();
		}
		if (this.selectPanelParam != null)
		{
			Rect listWindowViewRect = new Rect(base.ListWindowViewRect.xMin, base.ListWindowViewRect.yMin, base.ListWindowViewRect.xMax, base.ListWindowViewRect.yMax);
			if (this.selectPanelParam.adjustBorder)
			{
				listWindowViewRect.yMin = this.selectPanelParam.minY - GUIMain.VerticalSpaceSize;
				listWindowViewRect.yMax = this.selectPanelParam.maxY + GUIMain.VerticalSpaceSize;
			}
			else
			{
				listWindowViewRect.yMin = this.selectPanelParam.minY;
				listWindowViewRect.yMax = this.selectPanelParam.maxY;
			}
			base.ListWindowViewRect = listWindowViewRect;
			this.verticalBorder = this.selectPanelParam.verticalBorder;
			this.verticalMargin = this.selectPanelParam.verticalMargin;
			this.horizontalMargin = this.selectPanelParam.horizontalMargin;
			Vector3 localPosition = base.gameObject.transform.localPosition;
			localPosition.x = this.selectPanelParam.listPosX;
			base.gameObject.transform.localPosition = localPosition;
			float scrollPosX = this.selectPanelParam.scrollPosX;
			this.scrollBarPosX = scrollPosX;
			this.scrollBarBGPosX = scrollPosX;
		}
	}

	public GUISelectPanelViewPartsUD.PanelBuildData GetPanelBuildData()
	{
		return this._pbd;
	}

	protected GUISelectPanelViewPartsUD.PanelBuildData CalcBuildData(int wCount, int hCount, float sclX = 1f, float sclY = 1f, List<float> sizeYList = null)
	{
		this._sclX = sclX;
		this._sclY = sclY;
		if (sizeYList != null)
		{
			this.useVariableY = true;
			float num = this.selectCollider.width * sclX;
			this._pbd.pitchW = num + this.horizontalMargin;
			float num2 = this.selectCollider.height * sclY;
			this._pbd.pitchH = num2 + this.verticalMargin;
			this._pbd.lenH = this.verticalBorder * 2f - this.verticalMargin;
			for (int i = 0; i < sizeYList.Count; i++)
			{
				this._pbd.lenH += sizeYList[i] * sclY + this.verticalMargin;
			}
			this._pbd.startY = this._pbd.lenH / 2f - sizeYList[0] * sclY / 2f - this.verticalBorder;
			this._pbd.lenW = this._pbd.pitchW * (float)wCount - this.horizontalMargin;
			this._pbd.startX = -(this._pbd.lenW / 2f) + num / 2f;
		}
		else
		{
			float num3 = this.selectCollider.width * sclX;
			float num4 = this.selectCollider.height * sclY;
			this._pbd.pitchW = num3 + this.horizontalMargin;
			this._pbd.pitchH = num4 + this.verticalMargin;
			this._pbd.lenH = (float)hCount * this._pbd.pitchH + this.verticalBorder * 2f - this.verticalMargin;
			this._pbd.startY = this._pbd.lenH / 2f - num4 / 2f - this.verticalBorder;
			this._pbd.lenW = this._pbd.pitchW * (float)wCount - this.horizontalMargin;
			this._pbd.startX = -(this._pbd.lenW / 2f) + num3 / 2f;
		}
		return this._pbd;
	}

	public bool initLocation { get; set; }

	public bool initMaxLocation { get; set; }

	public bool initEffectFlg { get; set; }

	public bool useLocationRecord { get; set; }

	public void InitMinMaxLocation(int adjustIdx = -1, float adjustOfs = 0f)
	{
		this.listViewRect = base.boundingRect;
		base.minLocate = this.listViewRect.yMin + base.ListWindowViewRect.yMax;
		base.maxLocate = base.minLocate + base.height - base.ListWindowViewRect.height;
		if (base.maxLocate <= base.minLocate)
		{
			base.maxLocate = base.minLocate;
			this.selectLoc = base.minLocate;
			base.EnableScroll = false;
		}
		else
		{
			if (adjustIdx > -1)
			{
				if (this.useVariableY)
				{
					float num = base.minLocate;
					for (int i = 0; i < adjustIdx; i++)
					{
						num += this.partObjs[i].sizeY * this._sclY + this.verticalMargin;
					}
					this.selectLoc = num;
				}
				else
				{
					this.selectLoc = base.minLocate + this._pbd.pitchH * (float)adjustIdx;
				}
				this.selectLoc -= adjustOfs;
				if (this.selectLoc > base.maxLocate)
				{
					this.selectLoc = base.maxLocate;
				}
			}
			else if (base.minLocate > this.selectLoc || this.initLocation)
			{
				if (this.initLocation)
				{
					if (this.useLocationRecord)
					{
						GameObject parentObject = GUIManager.GetParentObject(base.gameObject);
						if (parentObject != null)
						{
							this.prefName = parentObject.name + "_SCROLL_POS";
							if (PlayerPrefs.HasKey(this.prefName))
							{
								this.selectLoc = PlayerPrefs.GetFloat(this.prefName);
								if (this.selectLoc > base.maxLocate)
								{
									this.selectLoc = base.maxLocate;
								}
								else if (this.selectLoc < base.minLocate)
								{
									this.selectLoc = base.minLocate;
								}
							}
							else
							{
								this.selectLoc = base.minLocate;
							}
						}
						else
						{
							this.selectLoc = base.minLocate;
						}
						this.initLocation = false;
					}
					else
					{
						this.selectLoc = base.minLocate;
						this.initLocation = false;
					}
				}
				else
				{
					this.selectLoc = base.minLocate;
					this.initLocation = false;
				}
			}
			else if (base.maxLocate < this.selectLoc || this.initMaxLocation)
			{
				this.selectLoc = base.maxLocate;
			}
			if (Mathf.Abs(base.maxLocate - base.minLocate) < 1f)
			{
				base.EnableScroll = false;
			}
			else
			{
				base.EnableScroll = true;
			}
		}
		base.transform.SetLocalY(this.selectLoc);
		base.SetScrollSpeed(0f);
	}

	public void SetLocationByIDX(int adjustIdx = 0, float adjustOfs = 0f)
	{
		if (this.useVariableY)
		{
			float num = base.minLocate;
			for (int i = 0; i < adjustIdx; i++)
			{
				num += this.partObjs[i].sizeY * this._sclY + this.verticalMargin;
			}
			this.selectLoc = num;
		}
		else
		{
			this.selectLoc = base.minLocate + this._pbd.pitchH * (float)adjustIdx;
		}
		this.selectLoc -= adjustOfs;
		if (this.selectLoc > base.maxLocate)
		{
			this.selectLoc = base.maxLocate;
		}
	}

	public float GetCenterOfs()
	{
		float yMax = base.ListWindowViewRect.yMax;
		return yMax - this._pbd.pitchH;
	}

	public void ResetMinMaxLocation()
	{
		base.minLocate = 0f;
		base.maxLocate = 0f;
		if (this._scrollBar != null && this._scrollBarBG != null)
		{
			bool active = !this.hideNonMoveScrollBar;
			this._scrollBar.SetActive(active);
			this._scrollBarBG.SetActive(active);
		}
	}

	private void KickEffect()
	{
		if (this.initEffectFlg)
		{
			this.initEffectFlg = false;
		}
		else
		{
			base.InitEfcAllListParts();
		}
	}

	public float ZPos
	{
		get
		{
			return this.zpos;
		}
		set
		{
			this.zpos = value;
		}
	}

	public void AllBuild(int count, bool initLoc = true, float sclX = 1f, float sclY = 1f, List<float> sizeYList = null, CMD instCMD = null)
	{
		if (this._selectParts != null)
		{
			this.GetSelectCollider();
			if (!this._selectParts.activeSelf)
			{
				this._selectParts.SetActive(true);
			}
		}
		this.initLocation = initLoc;
		this.InstanceCMD = instCMD;
		this.InitBuild();
		this.partsCount = count;
		int num = this.partsCount / this.PARTS_CT_MN;
		if (this.partsCount % this.PARTS_CT_MN > 0)
		{
			num++;
		}
		if (this.selectCollider != null)
		{
			GUISelectPanelViewPartsUD.PanelBuildData panelBuildData = this.CalcBuildData(this.PARTS_CT_MN, num, sclX, sclY, sizeYList);
			float num2 = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num3 = 0;
			for (int i = 0; i < this.partsCount; i++)
			{
				GUISelectPanelViewControlUD.ListPartsData listPartsData = this.AddBuildPart();
				if (sizeYList != null)
				{
					listPartsData.sizeY = sizeYList[num3];
				}
				float x = startX + panelBuildData.pitchW * (float)(num3 % this.PARTS_CT_MN);
				listPartsData.vPos = new Vector3(x, num2, this.ZPos);
				if (num3 == this.partsCount - 1)
				{
					break;
				}
				num3++;
				if (num3 % this.PARTS_CT_MN == 0)
				{
					if (sizeYList != null)
					{
						num2 -= sizeYList[num3 - 1] / 2f + sizeYList[num3] / 2f + this.verticalMargin;
					}
					else
					{
						num2 -= panelBuildData.pitchH;
					}
				}
			}
			base.height = panelBuildData.lenH;
			this.InitMinMaxLocation(-1, 0f);
			base.InitViewControl(this.selectParts, panelBuildData.pitchH, this.fRecycleViewMinY, this.fRecycleViewMaxY, this.PARTS_CT_MN, this.RecycleViewSectorSize);
			this.KickEffect();
		}
		if (this._selectParts != null)
		{
			this._selectParts.SetActive(false);
		}
	}

	public void RemovePartAndAdjust(int idx)
	{
		if (idx < 0)
		{
			idx = 0;
		}
		if (idx >= this.partObjs.Count)
		{
			idx = this.partObjs.Count - 1;
		}
		float num;
		if (this.useVariableY)
		{
			num = this.partObjs[idx].sizeY * this._sclY + this.verticalMargin;
		}
		else
		{
			num = this._pbd.pitchH;
		}
		base.maxLocate -= num;
		this._pbd.lenH -= num;
		Vector3 vector = this.boxCollider.size;
		vector.y -= num;
		this.boxCollider.size = vector;
		vector = this.boxCollider.center;
		vector.y += num / 2f;
		this.boxCollider.center = vector;
		this.RemoveAtPart(idx, false);
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			if (i >= idx)
			{
				this.partObjs[i].idx--;
				vector = this.partObjs[i].vPos;
				vector.y += num;
				this.partObjs[i].vPos = vector;
			}
			this.partObjs[i].csParts = null;
		}
		for (int i = 0; i < this.csPartsList.Count; i++)
		{
			this.csPartsList[i].csParts.gameObject.SetActive(false);
			this.csPartsList[i].csParts.InactiveParts();
			this.csPartsList[i].csParts.IDX = -1;
		}
		this.prvStartIDX = -1;
		base.RefreshView();
	}

	public void AddPartAndAdjust(bool moveToMax = true, float sizeY = 0f)
	{
		float num3;
		float num4;
		if (this.useVariableY)
		{
			float num = this.partObjs[this.partObjs.Count - 1].sizeY * this._sclY;
			float num2 = sizeY * this._sclY;
			num3 = num2 + this.verticalMargin;
			num4 = num / 2f + num2 / 2f + this.verticalMargin;
		}
		else
		{
			num3 = this._pbd.pitchH;
			num4 = this._pbd.pitchH;
		}
		base.maxLocate += num3;
		this._pbd.lenH += num3;
		Vector3 vector = this.boxCollider.size;
		vector.y += num3;
		this.boxCollider.size = vector;
		vector = this.boxCollider.center;
		vector.y -= num3 / 2f;
		this.boxCollider.center = vector;
		GUISelectPanelViewControlUD.ListPartsData listPartsData = new GUISelectPanelViewControlUD.ListPartsData();
		listPartsData.idx = this.partObjs.Count;
		vector = this.partObjs[this.partObjs.Count - 1].vPos;
		vector.y -= num4;
		listPartsData.vPos = vector;
		listPartsData.sizeY = sizeY;
		this.partObjs.Add(listPartsData);
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			this.partObjs[i].csParts = null;
		}
		if (moveToMax)
		{
			this.selectLoc = base.maxLocate;
		}
		for (int i = 0; i < this.csPartsList.Count; i++)
		{
			this.csPartsList[i].csParts.gameObject.SetActive(false);
			this.csPartsList[i].csParts.InactiveParts();
			this.csPartsList[i].csParts.IDX = -1;
		}
		this.prvStartIDX = -1;
		base.RefreshView();
	}

	public void InsertPartAndAdjust(int idx, float sizeY = 0f)
	{
		if (idx < 0)
		{
			idx = 0;
		}
		if (idx >= this.partObjs.Count)
		{
			idx = this.partObjs.Count - 1;
		}
		float num;
		float y;
		if (this.useVariableY)
		{
			num = sizeY * this._sclY + this.verticalMargin;
			if (idx == 0)
			{
				float num2 = (sizeY * this._sclY - this.partObjs[0].sizeY * this._sclY) / 2f;
				y = this.partObjs[0].vPos.y - num2;
			}
			else
			{
				y = this.partObjs[idx - 1].vPos.y - (this.partObjs[idx - 1].sizeY * this._sclY / 2f + sizeY * this._sclY / 2f + this.verticalMargin);
			}
		}
		else
		{
			num = this._pbd.pitchH;
			if (idx == 0)
			{
				y = this.partObjs[0].vPos.y;
			}
			else
			{
				y = this.partObjs[idx - 1].vPos.y * this._sclY - this._pbd.pitchH;
			}
		}
		base.maxLocate += num;
		this._pbd.lenH += num;
		Vector3 vector = this.boxCollider.size;
		vector.y += num;
		this.boxCollider.size = vector;
		vector = this.boxCollider.center;
		vector.y -= num / 2f;
		this.boxCollider.center = vector;
		GUISelectPanelViewControlUD.ListPartsData listPartsData = new GUISelectPanelViewControlUD.ListPartsData();
		listPartsData.idx = idx;
		vector = this.partObjs[idx].vPos;
		vector.y = y;
		listPartsData.vPos = vector;
		listPartsData.sizeY = sizeY;
		this.partObjs.Insert(idx, listPartsData);
		for (int i = idx + 1; i < this.partObjs.Count; i++)
		{
			this.partObjs[i].idx++;
			vector = this.partObjs[i].vPos;
			vector.y -= num;
			this.partObjs[i].vPos = vector;
		}
	}

	public void RefreshListParts()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			this.partObjs[i].csParts = null;
		}
		for (int i = 0; i < this.csPartsList.Count; i++)
		{
			this.csPartsList[i].csParts.gameObject.SetActive(false);
			this.csPartsList[i].csParts.InactiveParts();
			this.csPartsList[i].csParts.IDX = -1;
		}
		this.prvStartIDX = -1;
		base.RefreshView();
	}

	public bool AddPartAndAdjustForXY(bool moveToMax = true, float sizeY = 0f, float posZ = -5f)
	{
		bool flag = false;
		if (this.partObjs.Count > 0 && this.partObjs.Count % this.PARTS_CT_MN == 0)
		{
			flag = true;
		}
		float num = (float)(this.partObjs.Count % this.PARTS_CT_MN);
		float x = this._pbd.startX + this._pbd.pitchW * num;
		float num2 = (float)(this.partObjs.Count / this.PARTS_CT_MN);
		float y = this._pbd.startY - this._pbd.pitchH * num2;
		if (flag)
		{
			float pitchH = this._pbd.pitchH;
			this._pbd.lenH += pitchH;
			base.maxLocate = base.minLocate + this._pbd.lenH - base.ListWindowViewRect.height;
			if (base.maxLocate <= base.minLocate)
			{
				base.maxLocate = base.minLocate;
			}
			else
			{
				base.EnableScroll = true;
			}
			Vector3 vector = this.boxCollider.size;
			vector.y += pitchH;
			this.boxCollider.size = vector;
			vector = this.boxCollider.center;
			vector.y -= pitchH / 2f;
			this.boxCollider.center = vector;
		}
		GUISelectPanelViewControlUD.ListPartsData listPartsData = new GUISelectPanelViewControlUD.ListPartsData();
		listPartsData.idx = this.partObjs.Count;
		listPartsData.vPos = new Vector3(x, y, posZ);
		listPartsData.sizeY = sizeY;
		this.partObjs.Add(listPartsData);
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			this.partObjs[i].csParts = null;
		}
		if (moveToMax)
		{
			this.selectLoc = base.maxLocate;
		}
		for (int i = 0; i < this.csPartsList.Count; i++)
		{
			this.csPartsList[i].csParts.gameObject.SetActive(false);
			this.csPartsList[i].csParts.InactiveParts();
			this.csPartsList[i].csParts.IDX = -1;
		}
		this.prvStartIDX = -1;
		base.RefreshView();
		return flag;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.prefName != string.Empty)
		{
			PlayerPrefs.SetFloat(this.prefName, this.selectLoc);
		}
	}

	public GameObject GetOnePart(int idx)
	{
		if (this.partObjs[idx].csParts != null)
		{
			return this.partObjs[idx].csParts.gameObject;
		}
		return null;
	}

	protected override void Awake()
	{
		this.GetSelectCollider();
		base.Awake();
	}

	protected void InitBuild()
	{
		if (this.partObjs != null)
		{
			foreach (GUISelectPanelViewControlUD.ListPartsData listPartsData in this.partObjs)
			{
				if (listPartsData.csParts != null)
				{
				}
			}
			this.partObjs = null;
		}
		this.partObjs = new List<GUISelectPanelViewControlUD.ListPartsData>();
		this.InitScrollBar();
		this.SetSelectPanelParam();
	}

	public void ReleaseBuild()
	{
		if (this.partObjs != null)
		{
			foreach (GUISelectPanelViewControlUD.ListPartsData listPartsData in this.partObjs)
			{
				if (listPartsData.csParts != null)
				{
					UnityEngine.Object.Destroy(listPartsData.csParts.gameObject);
				}
			}
			this.partObjs = null;
		}
		this.partObjs = new List<GUISelectPanelViewControlUD.ListPartsData>();
	}

	public float ScrollBarPosX
	{
		get
		{
			return this.scrollBarPosX;
		}
		set
		{
			this.scrollBarPosX = value;
		}
	}

	public float ScrollBarBGPosX
	{
		get
		{
			return this.scrollBarBGPosX;
		}
		set
		{
			this.scrollBarBGPosX = value;
		}
	}

	protected void InitScrollBar()
	{
		if (this._scrollBar != null && this.NGScrollBarPart == null)
		{
			this.scrollBarPosX = this._scrollBar.transform.localPosition.x;
			Vector3 localScale = this._scrollBar.transform.localScale;
			this._scrollBar.transform.parent = base.gameObject.transform.parent;
			this._scrollBar.transform.localScale = localScale;
			Vector3 localPosition = this._scrollBar.transform.localPosition;
			localPosition.x = this.scrollBarPosX;
			this._scrollBar.transform.localPosition = localPosition;
			this.NGScrollBarPart = this._scrollBar.GetComponent<UISprite>();
		}
		if (this._scrollBarBG != null && this.NGScrollBarBGPart == null)
		{
			this.scrollBarBGPosX = this._scrollBarBG.transform.localPosition.x;
			Vector3 localScale = this._scrollBar.transform.localScale;
			this._scrollBarBG.transform.parent = base.gameObject.transform.parent;
			this._scrollBar.transform.localScale = localScale;
			Vector3 localPosition = this._scrollBarBG.transform.localPosition;
			localPosition.x = this.scrollBarBGPosX;
			this._scrollBarBG.transform.localPosition = localPosition;
			this.NGScrollBarBGPart = this._scrollBarBG.GetComponent<UISprite>();
			this.scrollBarBGCollider = this._scrollBarBG.GetComponent<GUICollider>();
			this.scrollBarBGBox = this._scrollBarBG.GetComponent<BoxCollider>();
			if (this.scrollBarBGBox != null && this.scrollBarBGCollider != null)
			{
				this.scrollBarBGCollider.onTouchBegan += delegate(Touch touch, Vector2 pos)
				{
					this.OnTouchMoveListScroll(pos);
				};
				this.scrollBarBGCollider.onTouchMoved += delegate(Touch touch, Vector2 pos)
				{
					this.OnTouchMoveListScroll(pos);
				};
			}
		}
	}

	protected override GUISelectPanelViewControlUD.ListPartsData AddBuildPart()
	{
		base.AddBuildPart();
		if (this.partObjs != null)
		{
			GUISelectPanelViewControlUD.ListPartsData listPartsData = new GUISelectPanelViewControlUD.ListPartsData();
			listPartsData.idx = this.partObjs.Count;
			this.partObjs.Add(listPartsData);
			return listPartsData;
		}
		return null;
	}

	protected GUISelectPanelViewControlUD.ListPartsData InsertBuildPart(int loc)
	{
		if (this.partObjs != null)
		{
			if (loc < 0)
			{
				loc = 0;
			}
			if (loc > this.partObjs.Count)
			{
				loc = this.partObjs.Count;
			}
			GUISelectPanelViewControlUD.ListPartsData listPartsData = new GUISelectPanelViewControlUD.ListPartsData();
			listPartsData.idx = this.partObjs.Count;
			this.partObjs.Insert(loc, listPartsData);
			return listPartsData;
		}
		return null;
	}

	protected void RemoveAtPart(int loc, bool isImmediate = false)
	{
		if (this.partObjs != null)
		{
			if (loc < 0)
			{
				loc = 0;
			}
			if (loc >= this.partObjs.Count)
			{
				loc = this.partObjs.Count - 1;
			}
			this.partObjs.RemoveAt(loc);
		}
	}

	public override void ShowGUI()
	{
		this.GetSelectCollider();
		base.ShowGUI();
		this.refreshPanel = true;
	}

	protected override void Update()
	{
		base.Update();
		if (this.refreshPanel || base.panelSpeed >= 0.01f || base.panelSpeed <= -0.01f)
		{
			this.refreshPanel = false;
		}
		float num = base.ListWindowViewRect.yMax - this.verticalBorder;
		float num2 = base.ListWindowViewRect.yMin + this.verticalBorder;
		float num3 = num - num2;
		float num4 = base.maxLocate - base.minLocate + num3;
		float num5 = num3 / num4 * num3;
		float num6 = (this.selectLoc - base.minLocate) / num4;
		float num7 = num - num6 * num3 - num5 / 2f;
		float num8 = num - num5 / 2f;
		float num9 = num2 + num5 / 2f;
		if (num7 > num8)
		{
			num7 = num8;
		}
		else if (num7 < num9)
		{
			num7 = num9;
		}
		if (this.NGScrollBarPart != null)
		{
			this.NGScrollBarPart.height = (int)num5;
			this.NGScrollBarPart.transform.SetLocalY(num7);
		}
		num3 = num - num2;
		if (this.NGScrollBarBGPart != null)
		{
			float num10 = 0f;
			if (this.NGScrollBarBGPart != null)
			{
				this.NGScrollBarBGPart.height = (int)num3;
				this.NGScrollBarBGPart.transform.SetLocalY((num + num2) / 2f);
				num10 = (float)this.NGScrollBarBGPart.height;
			}
			if (this.scrollBarBGBox != null)
			{
				this.v3tmp.x = this.scrollBarBGBox.size.x;
				this.v3tmp.y = num10;
				this.v3tmp.z = this.scrollBarBGBox.size.z;
				this.scrollBarBGBox.size = this.v3tmp;
				if (this.scrollBarBGCollider != null)
				{
					this.scrollBarBGCollider.height = num10;
				}
			}
		}
		if (this._scrollBar != null)
		{
			this.v3tmp = this._scrollBar.transform.localPosition;
			this.v3tmp.x = base.transform.localPosition.x + this.scrollBarPosX;
			this._scrollBar.transform.localPosition = this.v3tmp;
		}
		if (this._scrollBarBG != null)
		{
			this.v3tmp = this._scrollBarBG.transform.localPosition;
			this.v3tmp.x = base.transform.localPosition.x + this.scrollBarBGPosX;
			this._scrollBarBG.transform.localPosition = this.v3tmp;
		}
		if (this._scrollBar != null && this._scrollBarBG != null && this.NGScrollBarBGPart != null && this.NGScrollBarPart != null)
		{
			if (base.isActiveAndEnabled)
			{
				bool flag = !this.hideNonMoveScrollBar || base.maxLocate != base.minLocate;
				this._scrollBar.SetActive(flag);
				this._scrollBarBG.SetActive(flag);
				if (flag)
				{
					this.UpdateHideScrollBarAllWays();
					if (this.hideScrollBarAllWays)
					{
						this._scrollBar.SetActive(false);
						this._scrollBarBG.SetActive(false);
					}
				}
			}
			else
			{
				this._scrollBar.SetActive(false);
				this._scrollBarBG.SetActive(false);
			}
		}
		if (this._scrollBar != null && this._scrollBarBG && (this.partObjs == null || this.partObjs.Count == 0))
		{
			this._scrollBar.SetActive(false);
			this._scrollBarBG.SetActive(false);
		}
	}

	private void OnTouchMoveListScroll(Vector2 pos)
	{
		float num = 0f;
		float num2 = 0f;
		if (this.NGScrollBarBGPart != null)
		{
			num = this._scrollBarBG.transform.localPosition.y + (float)this.NGScrollBarBGPart.height / 2f;
			num2 = this._scrollBarBG.transform.localPosition.y - (float)this.NGScrollBarBGPart.height / 2f;
		}
		float num3 = base.ListWindowViewRect.yMax - this.verticalBorder;
		float num4 = base.ListWindowViewRect.yMin + this.verticalBorder;
		if (this.NGScrollBarPart != null && base.maxLocate > base.minLocate)
		{
			float num5 = num3 - num4;
			float num6 = base.maxLocate - base.minLocate + num5;
			float num7 = num5 / num6 * num5;
			num -= num7 / 2f;
			num2 += num7 / 2f;
			float num8 = pos.y;
			if (num8 > num)
			{
				num8 = num;
			}
			if (num8 < num2)
			{
				num8 = num2;
			}
			float num9 = (num - num8) / (num - num2);
			this.selectLoc = base.minLocate + (base.maxLocate - base.minLocate) * num9;
			float num10 = (this.selectLoc - base.minLocate) / num6;
			num8 = num3 - num10 * num5 - num7 / 2f;
			if (this.NGScrollBarPart != null)
			{
				this.NGScrollBarPart.transform.SetLocalY(num8);
			}
			global::Debug.Log("=======================POS_Y = " + pos.y.ToString());
		}
	}

	public void SetHideScrollBarAllWays(bool flg)
	{
		this.hideScrollBarAllWays = flg;
	}

	private void UpdateHideScrollBarAllWays()
	{
		if (base.StartFadeEfcCT > 0)
		{
			this.hideScrollBarAllWays = true;
		}
		else
		{
			this.hideScrollBarAllWays = false;
		}
	}

	public void SetScrollBarParam(GameObject slider, GameObject background)
	{
		this.scrollBar = slider;
		this.scrollBarBG = background;
	}

	public void SetRecycleViewParam(float viewTop, float viewBottom, int xPartsNum, int sectorSize)
	{
		this.fRecycleViewMaxY = viewTop;
		this.fRecycleViewMinY = viewBottom;
		this.PARTS_CT_MN = xPartsNum;
		this.RecycleViewSectorSize = sectorSize;
	}

	public void RefreshList(int partsCount, int horizontalPartsCount, List<float> sizeYList = null, bool initLoc = true)
	{
		int num = partsCount / horizontalPartsCount;
		if (partsCount % horizontalPartsCount > 0)
		{
			num++;
		}
		Vector3 localScale = this.selectParts.transform.localScale;
		GUISelectPanelViewPartsUD.PanelBuildData panelBuildData = this.CalcBuildData(horizontalPartsCount, num, localScale.x, localScale.y, sizeYList);
		float num2 = panelBuildData.startY;
		float startX = panelBuildData.startX;
		this.partObjs.Clear();
		int num3 = 0;
		for (int i = 0; i < partsCount; i++)
		{
			GUISelectPanelViewControlUD.ListPartsData listPartsData = this.AddBuildPart();
			if (sizeYList != null)
			{
				listPartsData.sizeY = sizeYList[num3];
			}
			float x = startX + panelBuildData.pitchW * (float)(num3 % horizontalPartsCount);
			listPartsData.vPos = new Vector3(x, num2, -5f);
			if (num3 == partsCount - 1)
			{
				break;
			}
			num3++;
			if (num3 % horizontalPartsCount == 0)
			{
				if (sizeYList != null)
				{
					num2 -= sizeYList[num3 - 1] / 2f + sizeYList[num3] / 2f + this.verticalMargin;
				}
				else
				{
					num2 -= panelBuildData.pitchH;
				}
			}
		}
		base.height = panelBuildData.lenH;
		this.initLocation = initLoc;
		this.InitMinMaxLocation(-1, 0f);
		base.InitializeView();
	}

	public class PanelBuildData
	{
		public float pitchW;

		public float pitchH;

		public float lenH;

		public float lenW;

		public float startY;

		public float startX;
	}
}
