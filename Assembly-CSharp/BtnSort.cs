using System;
using UnityEngine;

public class BtnSort : GUICollider
{
	public GameObject goTX_SORT;

	private UILabel ngTX_SORT;

	public bool IsEvolvePage { get; set; }

	public Action ActCallBackEnd { get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.AwakeSort();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
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
				CMD_ModalSort cmd_ModalSort = GUIMain.ShowCommonDialog(new Action<int>(this.EndSort), "CMD_ModalSort") as CMD_ModalSort;
				cmd_ModalSort.IsEvolvePage = this.IsEvolvePage;
			}
		}
	}

	private void AwakeSort()
	{
		this.ngTX_SORT = this.goTX_SORT.GetComponent<UILabel>();
		this.ngTX_SORT.text = MonsterDataMng.Instance().GetSortName();
	}

	private void EndSort(int i)
	{
		this.ngTX_SORT.text = MonsterDataMng.Instance().GetSortName();
		if (this.ActCallBackEnd != null)
		{
			this.ActCallBackEnd();
		}
	}
}
