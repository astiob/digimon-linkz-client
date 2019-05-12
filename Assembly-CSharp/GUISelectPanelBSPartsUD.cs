using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelBSPartsUD : GUISelectPanelBSUD
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

	[SerializeField]
	private GameObject goSelectPanelParam;

	private SelectPanelParamUD selectPanelParam;

	private GUISelectPanelBSPartsUD.PanelBuildData _pbd = new GUISelectPanelBSPartsUD.PanelBuildData();

	public float verticalBorder = 32f;

	public float verticalMargin = 8f;

	public float horizontalBorder = 32f;

	public float horizontalMargin = 8f;

	private bool hideNonMoveScrollBar = true;

	private string prefName = string.Empty;

	protected int partsCount;

	private float scrollBarPosX;

	private float scrollBarBGPosX;

	private Vector3 v3tmp = new Vector3(1f, 1f, 1f);

	private bool refreshPanel = true;

	private bool hideScrollBarAllWays;

	public GameObject selectParts
	{
		get
		{
			return this._selectParts;
		}
		set
		{
			this._selectParts = value;
			this.GetSelectCollider();
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

	public void SetSelectPanelParam(GameObject go)
	{
		this.goSelectPanelParam = go;
	}

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

	public GUISelectPanelBSPartsUD.PanelBuildData CalcBuildData(int wCount, int hCount, float sclX = 1f, float sclY = 1f)
	{
		if (this._selectParts != null && !this._selectParts.activeSelf)
		{
			this._selectParts.SetActive(true);
		}
		float num = this.selectCollider.width * sclX;
		float num2 = this.selectCollider.height * sclY;
		this._pbd.pitchW = num + this.horizontalMargin;
		this._pbd.pitchH = num2 + this.verticalMargin;
		this._pbd.lenH = (float)hCount * this._pbd.pitchH + this.verticalBorder * 2f - this.verticalMargin;
		this._pbd.startY = this._pbd.lenH / 2f - num2 / 2f - this.verticalBorder;
		this._pbd.lenW = this._pbd.pitchW * (float)wCount - this.horizontalMargin;
		this._pbd.startX = -(this._pbd.lenW / 2f) + num / 2f;
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
				this.selectLoc = base.minLocate + (this.selectCollider.height + this.verticalMargin) * (float)adjustIdx;
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
		if (this.initEffectFlg)
		{
			this.initEffectFlg = false;
		}
		else
		{
			base.InitEfcAllListParts();
		}
		base.SetScrollSpeed(0f);
	}

	public void SetLocationByIDX(int adjustIdx = 0, float adjustOfs = 0f)
	{
		this.selectLoc = base.minLocate + (this.selectCollider.height + this.verticalMargin) * (float)adjustIdx;
		this.selectLoc -= adjustOfs;
		if (this.selectLoc > base.maxLocate)
		{
			this.selectLoc = base.maxLocate;
		}
	}

	public float GetCenterOfs()
	{
		float yMax = base.ListWindowViewRect.yMax;
		return yMax - (this.selectCollider.height + this.verticalMargin);
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
		return this.partObjs[idx].gameObject;
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
			foreach (GUIListPartBS guilistPartBS in this.partObjs)
			{
				UnityEngine.Object.Destroy(guilistPartBS.gameObject);
			}
			this.partObjs = null;
		}
		this.partObjs = new List<GUIListPartBS>();
		this.InitScrollBar();
		this.SetSelectPanelParam();
	}

	public void ReleaseBuild()
	{
		if (this.partObjs != null)
		{
			foreach (GUIListPartBS guilistPartBS in this.partObjs)
			{
				UnityEngine.Object.Destroy(guilistPartBS.gameObject);
			}
			this.partObjs = null;
		}
		this.partObjs = new List<GUIListPartBS>();
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

	protected new GameObject AddBuildPart()
	{
		base.AddBuildPart();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.selectParts);
		Vector3 localScale = gameObject.transform.localScale;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = localScale;
		if (this.partObjs != null)
		{
			GUIListPartBS component = gameObject.GetComponent<GUIListPartBS>();
			if (component != null)
			{
				component.IDX = this.partObjs.Count;
				this.partObjs.Add(component);
			}
		}
		return gameObject;
	}

	protected GameObject InsertBuildPart(int loc)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.selectParts);
		Vector3 localScale = gameObject.transform.localScale;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = localScale;
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
			GUIListPartBS component = gameObject.GetComponent<GUIListPartBS>();
			if (component != null)
			{
				this.partObjs.Insert(loc, component);
			}
		}
		return gameObject;
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
			if (isImmediate)
			{
				UnityEngine.Object.DestroyImmediate(this.partObjs[loc].gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(this.partObjs[loc].gameObject);
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
			if (this.partObjs != null)
			{
				this.partObjs.ForEach(delegate(GUIListPartBS o)
				{
					o.UpdateShowCard();
				});
			}
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
