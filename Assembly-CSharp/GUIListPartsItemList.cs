using System;
using UnityEngine;

public sealed class GUIListPartsItemList : GUIListPartBS
{
	[SerializeField]
	private PresentBoxItem csItem;

	[SerializeField]
	private GameObject goEfc;

	private GameWebAPI.RespData_AreaEventResult.Reward data;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
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
		if (flag)
		{
			base.OnTouchEnded(touch, pos, flag);
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f)
			{
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void SetData()
	{
		CMD_PointResult cmd_PointResult = (CMD_PointResult)base.GetInstanceCMD();
		this.data = cmd_PointResult.GetDataByIDX(base.IDX);
	}

	public override void InitParts()
	{
		if (this.goEfc != null && this.goEfc.transform.parent == base.transform)
		{
			this.goEfc.transform.parent = base.transform.parent;
		}
		AppCoroutine.Start(this.csItem.SetItemWithWaitASync(this.data.assetCategoryId, this.data.assetValue, "1", true, delegate
		{
		}), false);
	}

	public override void RefreshParts()
	{
		AppCoroutine.Start(this.csItem.SetItemWithWaitASync(this.data.assetCategoryId, this.data.assetValue, "1", true, delegate
		{
		}), false);
	}

	public override void InactiveParts()
	{
	}
}
