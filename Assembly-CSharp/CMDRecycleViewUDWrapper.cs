using System;
using UnityEngine;

public abstract class CMDRecycleViewUDWrapper : MonoBehaviour
{
	[SerializeField]
	protected float listOffsetX;

	[SerializeField]
	protected float listOffsetY;

	[SerializeField]
	protected float verticalBorder;

	[SerializeField]
	protected bool adjustBorder = true;

	[SerializeField]
	protected float verticalMargin;

	[SerializeField]
	protected int horizontalPartsNum;

	[SerializeField]
	protected float horizontalMargin;

	[SerializeField]
	protected float scrollPosAdjX;

	[SerializeField]
	protected GameObject scrollBarSlider;

	[SerializeField]
	protected GameObject scrollBarBackground;

	[SerializeField]
	protected GUIListPartsWrapper listParts;

	protected GUISelectPanelViewPartsUD cmdRecycleView;

	private bool isInitializedList;

	public void InitializeView(int recycleSectorNum)
	{
		this.cmdRecycleView = base.gameObject.GetComponent<GUISelectPanelViewPartsUD>();
		this.cmdRecycleView.touchBehavior = GUICollider.TouchBehavior.None;
		this.cmdRecycleView.dontAddToDialogEvent = true;
		this.cmdRecycleView.selectParts = this.listParts.gameObject;
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		float x = component.size.x * -0.5f;
		float y = component.size.y * -0.5f;
		float x2 = component.size.x;
		float y2 = component.size.y;
		Rect listWindowViewRect = new Rect(x, y, x2, y2);
		if (this.adjustBorder)
		{
			listWindowViewRect.yMin -= GUIMain.VerticalSpaceSize;
			listWindowViewRect.yMax += GUIMain.VerticalSpaceSize;
		}
		this.cmdRecycleView.ListWindowViewRect = listWindowViewRect;
		this.cmdRecycleView.verticalBorder = this.verticalBorder;
		this.cmdRecycleView.verticalMargin = this.verticalMargin;
		this.cmdRecycleView.horizontalMargin = this.horizontalMargin;
		Vector3 vector = base.transform.localPosition;
		vector.x = this.listOffsetX;
		base.transform.localPosition = vector;
		vector = component.center;
		vector.y = this.listOffsetY;
		component.center = vector;
		this.cmdRecycleView.ScrollBarPosX = this.scrollPosAdjX;
		this.cmdRecycleView.ScrollBarBGPosX = this.scrollPosAdjX;
		this.cmdRecycleView.SetScrollBarParam(this.scrollBarSlider, this.scrollBarBackground);
		float num = component.size.y * 0.5f + this.listParts.GetPartsSize().y;
		float viewBottom = num * -1f;
		this.cmdRecycleView.SetRecycleViewParam(num, viewBottom, this.horizontalPartsNum, recycleSectorNum);
	}

	protected void CreateList(int count)
	{
		if (!this.isInitializedList)
		{
			this.isInitializedList = true;
			this.cmdRecycleView.AllBuild(count, true, 1f, 1f, null, null, true);
		}
		else
		{
			this.cmdRecycleView.RefreshList(count, this.horizontalPartsNum, null, true);
		}
	}
}
