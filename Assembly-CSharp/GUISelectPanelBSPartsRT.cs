using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelBSPartsRT : GUISelectPanelBSRT
{
	[SerializeField]
	private GameObject _selectParts;

	[SerializeField]
	private GUICollider _selectCollider;

	protected int partsCount;

	private GameObject goListPartsRoot;

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

	protected override void Awake()
	{
		this.GetSelectCollider();
		base.Awake();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "LIST_PARTS_ROOT")
			{
				this.goListPartsRoot = transform.gameObject;
			}
		}
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
	}

	protected GameObject AddBuildPart()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.selectParts);
		if (this.goListPartsRoot != null)
		{
			gameObject.transform.parent = this.goListPartsRoot.transform;
		}
		else
		{
			Vector3 localScale = gameObject.transform.localScale;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localScale = localScale;
		}
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
		if (this.goListPartsRoot != null)
		{
			gameObject.transform.parent = this.goListPartsRoot.transform;
		}
		else
		{
			Vector3 localScale = gameObject.transform.localScale;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localScale = localScale;
		}
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
				for (int i = 0; i < this.partObjs.Count; i++)
				{
					this.partObjs[i].UpdateShowCard();
				}
			}
		}
	}
}
