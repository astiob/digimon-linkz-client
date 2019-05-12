using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIListChipParts : GUIListPartBS
{
	private GUIListChipParts.Data data;

	private Vector2 beganPosition = Vector2.zero;

	private bool isTouchEndFromChild;

	private bool isTouching_mi;

	private bool isLongTouched;

	private float touchBeganTime;

	[SerializeField]
	private ChipIcon chipIcon;

	private bool _LongTouch = true;

	private bool isNotShortTouchable;

	public bool LongTouch
	{
		get
		{
			return this._LongTouch;
		}
		set
		{
			this._LongTouch = value;
		}
	}

	public override void ShowGUI()
	{
		this.Init();
		base.ShowGUI();
	}

	private void Init()
	{
		this.chipIcon.SetData(this.data.masterChip, -1, -1);
		this.SetSelectColor(this.data.isSelect);
		this.SetSelectMessage(this.data.selectMessage);
		if (this.data.shouldDim)
		{
			bool isAlreadyAttached = this.data.userChip.userMonsterId > 0;
			this.isNotShortTouchable = this.chipIcon.SetEquipmentIconWithDim(isAlreadyAttached, this.data.myDigimonChipGroupIds, this.data.userChip.chipId);
		}
		else
		{
			this.chipIcon.SetEquipmentIcon(this.data.userChip.userMonsterId > 0);
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
		this.beganPosition = pos;
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
		this.isTouching_mi = true;
		this.isLongTouched = false;
		this.touchBeganTime = Time.realtimeSinceStartup;
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
		float magnitude = (this.beganPosition - pos).magnitude;
		if (magnitude > 40f)
		{
			this.isTouching_mi = false;
		}
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
		if (this.data == null)
		{
			return;
		}
		this.isTouching_mi = false;
		if (this.isLongTouched)
		{
			this.isLongTouched = false;
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPosition - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				if (this.isNotShortTouchable)
				{
					return;
				}
				if (this.data.actTouchShort != null)
				{
					this.data.actTouchShort(this.data);
				}
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.data == null)
		{
			return;
		}
		if (this.isTouching_mi && this.LongTouch && Time.realtimeSinceStartup - this.touchBeganTime >= 0.5f)
		{
			if (this.data.actTouchLong != null)
			{
				this.data.actTouchLong(this.data);
			}
			base.isTouching = false;
			this.isLongTouched = true;
			this.isTouching_mi = false;
		}
	}

	public void SetSelectColor(bool isSelect)
	{
		this.chipIcon.SetSelectColor(isSelect);
	}

	public void SetNowSelectMessage(bool isSelect)
	{
		this.chipIcon.SetNowSelectMessage(isSelect);
	}

	public void SetSelectMessage(string value)
	{
		this.chipIcon.SetSelectMessage(value);
	}

	public void SetData(GUIListChipParts.Data dt)
	{
		this.data = dt;
	}

	public override void SetData()
	{
		this.data = GUISelectPanelChipList.partsDataList[base.IDX];
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	public class Data
	{
		public int index;

		public GameWebAPI.RespDataMA_ChipM.Chip masterChip;

		public GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip;

		public Action<GUIListChipParts.Data> actTouchShort;

		public Action<GUIListChipParts.Data> actTouchLong;

		public bool isSelect;

		public bool shouldDim;

		public List<string> myDigimonChipGroupIds;

		public string selectMessage = string.Empty;
	}
}
