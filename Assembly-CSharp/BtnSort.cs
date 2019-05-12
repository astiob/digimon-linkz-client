using System;
using System.Collections.Generic;
using UnityEngine;

public class BtnSort : GUICollider
{
	[SerializeField]
	private GameObject goTX_SORT;

	private UILabel ngTX_SORT;

	public Action OnChangeSortType { private get; set; }

	public List<MonsterData> SortTargetMonsterList { private get; set; }

	public Action ActCallBackEnd { get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.AwakeSort();
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
		this.beganPostion = pos;
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
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f)
			{
				CMD_ModalSort cmd_ModalSort = GUIMain.ShowCommonDialog(new Action<int>(this.EndSort), "CMD_ModalSort", null) as CMD_ModalSort;
				cmd_ModalSort.SetTargetMonsterList(this.SortTargetMonsterList);
				cmd_ModalSort.SetChangeSettingAction(this.OnChangeSortType);
				cmd_ModalSort.Initialize();
			}
		}
	}

	private void AwakeSort()
	{
		this.ngTX_SORT = this.goTX_SORT.GetComponent<UILabel>();
		this.ngTX_SORT.text = CMD_ModalSort.GetSortName(CMD_BaseSelect.IconSortType);
	}

	private void EndSort(int i)
	{
		this.ngTX_SORT.text = CMD_ModalSort.GetSortName(CMD_BaseSelect.IconSortType);
		if (this.ActCallBackEnd != null)
		{
			this.ActCallBackEnd();
		}
	}
}
