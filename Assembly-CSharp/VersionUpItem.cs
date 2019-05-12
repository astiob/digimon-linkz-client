using Evolution;
using System;
using UnityEngine;

public class VersionUpItem : GUICollider
{
	[SerializeField]
	[Header("アイコン")]
	public UISprite spIcon;

	[Header("アイコン")]
	[SerializeField]
	public UITexture texIcon;

	[SerializeField]
	[Header("素材選択")]
	public UILabel lbSelect;

	[Header("個数プレート")]
	[SerializeField]
	public UISprite spNumPlate;

	[SerializeField]
	[Header("個数表示")]
	public UILabel lbNum;

	private Action actTouchShort;

	private Action actTouchLong;

	private Vector2 beganPosition;

	private float touchBeganTime;

	private bool isTouching_mi;

	private bool isLongTouched;

	private bool _LongTouch = true;

	public HaveSoulData baseSoulData { get; set; }

	public int NeedNum { get; set; }

	public HaveSoulData AlmightySoulData { get; set; }

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

	public void SetTouchAct_L(Action act)
	{
		this.actTouchLong = act;
	}

	public void SetTouchAct_S(Action act)
	{
		this.actTouchShort = act;
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
			if (magnitude < 40f && this.actTouchShort != null)
			{
				this.actTouchShort();
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.isTouching_mi && this.LongTouch && Time.realtimeSinceStartup - this.touchBeganTime >= 0.5f)
		{
			if (this.actTouchLong != null)
			{
				this.actTouchLong();
			}
			base.isTouching = false;
			this.isLongTouched = true;
			this.isTouching_mi = false;
		}
	}
}
