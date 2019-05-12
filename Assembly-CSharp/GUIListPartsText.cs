using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIListPartsText : GUIListPartBS
{
	[SerializeField]
	private GameObject goTX_SHOW;

	private UILabel ngTX_SHOW;

	[SerializeField]
	private List<GameObject> goRareIconList;

	private GUIListPartsTextData data;

	private bool isTouchEndFromChild;

	public GUIListPartsTextData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngTX_SHOW = this.goTX_SHOW.GetComponent<UILabel>();
	}

	public void ChangeSprite(string sprName)
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		if (component != null)
		{
			component.spriteName = sprName;
			component.MakePixelPerfect();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.ngTX_SHOW.text = this.data.text;
		this.ShowRare();
	}

	private void ShowRare()
	{
		foreach (GameObject gameObject in this.goRareIconList)
		{
			gameObject.SetActive(false);
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				CMD_QuestTOP.ChangeSelectA_StageL_S(base.IDX, false);
			}
		}
	}

	private void OnClickedBtnSelect()
	{
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
