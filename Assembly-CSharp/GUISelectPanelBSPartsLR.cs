using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelBSPartsLR : GUISelectPanelBSLR
{
	[SerializeField]
	private GameObject _selectParts;

	[SerializeField]
	private GameObject _scrollBar;

	private UISprite NGScrollBarPart;

	[SerializeField]
	private GameObject _scrollBarBG;

	private UISprite NGScrollBarBGPart;

	private GUICollider _selectCollider;

	public float verticalBorder = 32f;

	public float verticalMargin = 8f;

	public float horizontalBorder = 32f;

	public float horizontalMargin = 8f;

	protected int partsCount;

	private bool refreshPanel = true;

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

	public GUICollider selectCollider
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

	public bool initLocation { get; set; }

	public void InitMinMaxLocation(bool isRev = false)
	{
		this.listViewRect = base.boundingRect;
		base.minLocate = this.listViewRect.xMin + base.ListWindowViewRect.xMax;
		base.maxLocate = base.minLocate + base.width - base.ListWindowViewRect.width;
		if (base.maxLocate < base.minLocate)
		{
			if (!isRev)
			{
				base.maxLocate = base.minLocate;
			}
			else
			{
				base.minLocate = base.maxLocate;
			}
		}
		if (base.minLocate > this.selectLoc)
		{
			this.selectLoc = base.minLocate;
		}
		else if (this.selectLoc > base.maxLocate)
		{
			this.selectLoc = base.maxLocate;
		}
		if (this.initLocation)
		{
			if (!isRev)
			{
				this.selectLoc = base.minLocate;
			}
			else
			{
				this.selectLoc = base.maxLocate;
			}
			this.initLocation = false;
		}
		if (base.maxLocate < this.selectLoc)
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
		base.transform.SetLocalX(this.selectLoc);
		base.SetScrollSpeed(0f);
		base.CancelMoveLR();
		base.ClearAdjusting();
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
		if (this._scrollBar != null)
		{
			this._scrollBar.transform.parent = base.gameObject.transform.parent;
			this.NGScrollBarPart = this._scrollBar.GetComponent<UISprite>();
		}
		if (this._scrollBarBG != null)
		{
			this._scrollBarBG.transform.parent = base.gameObject.transform.parent;
			this.NGScrollBarBGPart = this._scrollBarBG.GetComponent<UISprite>();
		}
	}

	protected GameObject AddBuildPart()
	{
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

	protected void RemoveAtPart(int loc)
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
			UnityEngine.Object.Destroy(this.partObjs[loc].gameObject);
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
		float num = base.ListWindowViewRect.xMax - this.horizontalBorder;
		float num2 = base.ListWindowViewRect.xMin + this.horizontalBorder;
		float num3 = num - num2;
		float num4 = base.maxLocate - base.minLocate + num3;
		float num5 = num3 / num4 * num3;
		float num6 = (this.selectLoc - base.minLocate) / num4;
		float x = num - num6 * num3 - num5 / 2f;
		if (this.NGScrollBarPart != null)
		{
			this.NGScrollBarPart.width = (int)num5;
			this.NGScrollBarPart.transform.SetLocalX(x);
		}
		num3 = num - num2;
		if (this.NGScrollBarBGPart != null)
		{
			this.NGScrollBarBGPart.width = (int)num3;
			this.NGScrollBarBGPart.transform.SetLocalX((num + num2) / 2f);
		}
	}
}
