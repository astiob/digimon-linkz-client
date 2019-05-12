using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelBSRT : GUICollider
{
	protected float selectLoc;

	protected Vector2 startLoc = new Vector2(0f, 0f);

	private Vector3 vRotCtr = new Vector3(0f, 0f, 0f);

	private float radius = 170f;

	private float minPitch = 5f;

	private float maxPitch = 180f;

	private bool isRotParts;

	private bool isRotPartsPlus = true;

	private bool isZPosRev;

	private int btnMoveCT = 1;

	public List<GUIListPartBS> partObjs;

	private bool enableScroll = true;

	private bool stopAtEnd;

	private bool touch_begin;

	[SerializeField]
	private GUICollider csMoveLeft;

	[SerializeField]
	private GUICollider csMoveRight;

	private bool stillScroll;

	private bool freeScrollMode = true;

	private int MoveCt;

	private Quaternion lastQt;

	private Vector3 vecTmp = new Vector3(0f, 0f, 0f);

	private Vector3 lastScale = new Vector3(1f, 1f, 1f);

	private float scrollSpeed;

	private float panelSpeed_;

	private bool isAdjusting;

	private float adjustLoc;

	private float adj_per_div;

	private float adj_threshold;

	private float min_threshold;

	private float stp_threshold;

	private float accel;

	private float zero_thrh_l;

	private float zero_thrh_s;

	public Vector3 VRotCtr
	{
		get
		{
			return this.vRotCtr;
		}
		set
		{
			this.vRotCtr = value;
		}
	}

	public float Radius
	{
		get
		{
			return this.radius;
		}
		set
		{
			this.radius = value;
		}
	}

	public float MinPitch
	{
		get
		{
			return this.minPitch;
		}
		set
		{
			this.minPitch = value;
		}
	}

	public float MaxPitch
	{
		get
		{
			return this.maxPitch;
		}
		set
		{
			this.maxPitch = value;
		}
	}

	public bool IsRotParts
	{
		get
		{
			return this.isRotParts;
		}
		set
		{
			this.isRotParts = value;
		}
	}

	public bool IsRotPartsPlus
	{
		get
		{
			return this.isRotPartsPlus;
		}
		set
		{
			this.isRotPartsPlus = value;
		}
	}

	public bool IsZPosRev
	{
		get
		{
			return this.isZPosRev;
		}
		set
		{
			this.isZPosRev = value;
		}
	}

	public int BtnMoveCT
	{
		get
		{
			return this.btnMoveCT;
		}
		set
		{
			this.btnMoveCT = value;
		}
	}

	public float GetRadius(float len, int partsCt)
	{
		float virtualPitch = this.GetVirtualPitch(partsCt);
		return len / Mathf.Sin(0.0174532924f * virtualPitch);
	}

	public GameObject GetOnePart(int idx)
	{
		return this.partObjs[idx].gameObject;
	}

	public Vector2 GetStartLoc()
	{
		return this.startLoc;
	}

	public bool EnableScroll
	{
		get
		{
			return this.enableScroll;
		}
		set
		{
			this.enableScroll = value;
		}
	}

	public bool StopAtEnd
	{
		get
		{
			return this.stopAtEnd;
		}
		set
		{
			this.stopAtEnd = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.onTouchBegan += delegate(Touch touch, Vector2 pos)
		{
			this.isAdjusting = false;
			this.CancelMoveLR();
		};
		this.onTouchMoved += delegate(Touch touch, Vector2 pos)
		{
			this.isAdjusting = false;
			this.CancelMoveLR();
		};
		this.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.isAdjusting = false;
			this.CancelMoveLR();
		};
		this.InitLR();
	}

	protected void Start()
	{
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
		this.touch_begin = true;
		base.OnTouchBegan(touch, pos);
		this.startLoc = pos;
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
		if (!this.touch_begin)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
		if (Mathf.Abs(this.startLoc.x - pos.x) > Mathf.Abs(this.scrollSpeed))
		{
		}
		if (this.EnableScroll)
		{
			this.scrollSpeed = (pos.x - this.startLoc.x) / 4f;
		}
		else
		{
			this.scrollSpeed = 0f;
		}
		this.selectLoc -= this.scrollSpeed * 0.7f;
		this.StopAtEndFunc();
		this.startLoc = pos;
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
		this.touch_begin = false;
	}

	public void SetScrollSpeed(float spd)
	{
		this.scrollSpeed = spd;
	}

	public float GetScrollSpeed()
	{
		return this.scrollSpeed;
	}

	public bool StillScroll
	{
		get
		{
			return this.stillScroll;
		}
		set
		{
			this.stillScroll = value;
		}
	}

	public bool FreeScrollMode
	{
		get
		{
			return this.freeScrollMode;
		}
		set
		{
			this.freeScrollMode = value;
		}
	}

	protected void InitLR()
	{
		if (this.csMoveLeft != null)
		{
			this.csMoveLeft.onTouchBegan += delegate(Touch touch, Vector2 pos)
			{
				this.OnTouchMoveLeft();
			};
		}
		if (this.csMoveRight != null)
		{
			this.csMoveRight.onTouchBegan += delegate(Touch touch, Vector2 pos)
			{
				this.OnTouchMoveRight();
			};
		}
	}

	private void OnTouchMoveLeft()
	{
		this.MoveCt--;
	}

	private void OnTouchMoveRight()
	{
		this.MoveCt++;
	}

	private void CancelMoveLR()
	{
		this.MoveCt = 0;
	}

	private void UpdateMoveLR()
	{
		if (!this.isAdjusting)
		{
			if (this.MoveCt > 0)
			{
				this.isAdjusting = true;
				this.adjustLoc = this.AdjustLocPlus(this.BtnMoveCT - 1);
				this.MoveCt--;
			}
			if (this.MoveCt < 0)
			{
				this.isAdjusting = true;
				this.adjustLoc = this.AdjustLocMinus(this.BtnMoveCT - 1);
				this.MoveCt++;
			}
		}
	}

	private float Adjust360(float rot)
	{
		if (rot > 0f)
		{
			while (rot >= 360f)
			{
				rot -= 360f;
			}
		}
		else if (rot < 0f)
		{
			while (rot < 0f)
			{
				rot += 360f;
			}
		}
		return rot;
	}

	private float AdjustLocPlus(int pct = 0)
	{
		float usePitch = this.GetUsePitch();
		float num = this.selectLoc + 0.001f;
		int num2 = (int)(num / usePitch);
		float num3;
		if (num >= 0f)
		{
			num3 = usePitch * (float)(num2 + 1);
		}
		else
		{
			num3 = usePitch * (float)num2;
		}
		return num3 + usePitch * (float)pct;
	}

	private float AdjustLocMinus(int pct = 0)
	{
		float usePitch = this.GetUsePitch();
		float num = this.selectLoc - 0.001f;
		int num2 = (int)(num / usePitch);
		float num3;
		if (num >= 0f)
		{
			num3 = usePitch * (float)num2;
		}
		else
		{
			num3 = usePitch * (float)(num2 - 1);
		}
		return num3 - usePitch * (float)pct;
	}

	private float AdjustLocNear()
	{
		float usePitch = this.GetUsePitch();
		float num = this.selectLoc;
		int num2 = (int)(num / usePitch);
		float num3 = num - usePitch * (float)num2;
		float result;
		if (num >= 0f)
		{
			if (num3 > usePitch / 2f)
			{
				result = usePitch * (float)(num2 + 1);
			}
			else
			{
				result = usePitch * (float)num2;
			}
		}
		else if (num3 < -(usePitch / 2f))
		{
			result = usePitch * (float)(num2 - 1);
		}
		else
		{
			result = usePitch * (float)num2;
		}
		return result;
	}

	public float GetUsePitch()
	{
		return 360f / (float)this.partObjs.Count;
	}

	public float GetUsePitch(int ct)
	{
		return 360f / (float)ct;
	}

	public int GetSelectedIdx()
	{
		int result = 0;
		float num = 10000f;
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			float num2 = this.GetLocationByIdx(i) + this.selectLoc;
			num2 = this.Adjust360(num2);
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			float num3 = Math.Abs(num2);
			if (num > num3)
			{
				result = i;
				num = num3;
			}
		}
		return result;
	}

	public void SetSelectedIdx(int idx)
	{
		this.selectLoc = -this.GetLocationByIdx(idx);
	}

	public float GetLocationByIdx(int idx)
	{
		float num = 0f + this.GetUsePitch() * (float)idx;
		return -num;
	}

	public float GetVirtualPitch()
	{
		float num = this.MinPitch;
		float num2 = this.MaxPitch;
		float usePitch = this.GetUsePitch();
		if (num2 < num)
		{
			return num;
		}
		if (usePitch < num)
		{
			return num;
		}
		if (usePitch > num2)
		{
			return num2;
		}
		return usePitch;
	}

	public float GetVirtualPitch(int ct)
	{
		float num = this.MinPitch;
		float num2 = this.MaxPitch;
		float usePitch = this.GetUsePitch(ct);
		if (num2 < num)
		{
			return num;
		}
		if (usePitch < num)
		{
			return num;
		}
		if (usePitch > num2)
		{
			return num2;
		}
		return usePitch;
	}

	public bool IsVirtualPitch()
	{
		float num = this.MinPitch;
		float num2 = this.MaxPitch;
		float usePitch = this.GetUsePitch();
		return num2 < num || usePitch < num || usePitch > num2;
	}

	public float GetCurrentRotPosByIdx(int idx)
	{
		float num;
		if (!this.IsVirtualPitch())
		{
			num = this.GetLocationByIdx(idx) + this.selectLoc;
		}
		else
		{
			float num2 = this.GetLocationByIdx(idx) + this.selectLoc;
			num2 = this.Adjust360(num2);
			float num3 = this.GetVirtualPitch() / this.GetUsePitch();
			if (num2 < 180f)
			{
				num = num2 * num3;
				if (num > 180f)
				{
					num = 180f;
				}
			}
			else
			{
				num = (num2 - 360f) * num3;
				if (num < -180f)
				{
					num = -180f;
				}
			}
		}
		return this.Adjust360(num);
	}

	public Vector3 GetCurrentTransPosByIdx(int idx)
	{
		this.vecTmp.x = 0f;
		this.vecTmp.y = 0f;
		this.vecTmp.z = -this.Radius;
		float num = this.GetCurrentRotPosByIdx(idx);
		Quaternion rotation = Quaternion.Euler(0f, num, 0f);
		this.vecTmp = rotation * this.vecTmp;
		if (this.isZPosRev)
		{
			this.vecTmp.z = -this.vecTmp.z;
		}
		this.vecTmp.x = this.vecTmp.x + this.VRotCtr.x;
		this.vecTmp.y = this.vecTmp.y + this.VRotCtr.y;
		this.vecTmp.z = this.vecTmp.z + this.VRotCtr.z;
		if (this.isRotParts)
		{
			if (!this.isRotPartsPlus)
			{
				float num2;
				if (num < 180f)
				{
					if (num > 30f)
					{
						num = 30f;
					}
					num2 = num;
					this.vecTmp.x = this.vecTmp.x - num2;
				}
				else
				{
					if (num < 330f)
					{
						num = 330f;
					}
					num2 = 360f - num;
					this.vecTmp.x = this.vecTmp.x + num2;
				}
				num = -num;
				float num3 = (180f - num2) / 180f;
				this.lastScale.x = num3;
				this.lastScale.y = num3;
			}
			if (this.isZPosRev)
			{
				rotation = Quaternion.Euler(0f, -num, 0f);
			}
			else
			{
				rotation = Quaternion.Euler(0f, num, 0f);
			}
		}
		else
		{
			rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		this.lastQt = rotation;
		return this.vecTmp;
	}

	private void UpdatePartPosAll()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIListPartBS guilistPartBS = this.partObjs[i];
			if (guilistPartBS != null)
			{
				Vector3 currentTransPosByIdx = this.GetCurrentTransPosByIdx(i);
				Vector3 localPosition = guilistPartBS.gameObject.transform.localPosition;
				localPosition.x = currentTransPosByIdx.x;
				localPosition.y = currentTransPosByIdx.y;
				localPosition.z = currentTransPosByIdx.z;
				guilistPartBS.gameObject.transform.localPosition = localPosition;
				guilistPartBS.gameObject.transform.localRotation = this.lastQt;
				if (this.isRotParts && !this.isRotPartsPlus)
				{
					guilistPartBS.gameObject.transform.localScale = this.lastScale;
				}
			}
			if (this.stopAtEnd)
			{
				int selectedIdx = this.GetSelectedIdx();
				int num = guilistPartBS.IDX - selectedIdx;
				if (num < 0)
				{
					num = -num;
				}
				if (num > 5)
				{
					Vector3 localPosition2 = guilistPartBS.gameObject.transform.localPosition;
					localPosition2.y += 2000f;
					guilistPartBS.gameObject.transform.localPosition = localPosition2;
				}
			}
		}
	}

	public float panelSpeed
	{
		get
		{
			return this.panelSpeed_;
		}
	}

	protected override void Update()
	{
		base.Update();
		this.panelSpeed_ = this.scrollSpeed;
		if (!this.isTouchMoved)
		{
			this.panelSpeed_ = this.selectLoc;
			float usePitch = this.GetUsePitch();
			this.adj_per_div = 2f;
			this.adj_threshold = usePitch / this.adj_per_div;
			this.min_threshold = usePitch / 10f;
			this.stp_threshold = usePitch / 40f;
			this.accel = usePitch / 7f;
			this.zero_thrh_l = usePitch / 10f;
			this.zero_thrh_s = usePitch / 100f;
			if (this.FreeScrollMode)
			{
				this.selectLoc -= this.scrollSpeed;
				if (!this.stillScroll)
				{
					this.scrollSpeed = (float)Math.Sign(this.scrollSpeed) * Math.Max(Math.Abs(this.scrollSpeed) - (float)Math.Pow((double)Math.Abs(this.scrollSpeed), 1.2000000476837158) * 0.05f, 0f);
					if (Math.Abs(this.scrollSpeed) < this.stp_threshold)
					{
						this.scrollSpeed = 0f;
					}
				}
			}
			else
			{
				float num = -this.scrollSpeed;
				float num2 = Math.Abs(num);
				if (!this.isAdjusting)
				{
					this.selectLoc -= this.scrollSpeed;
					if (!this.stillScroll)
					{
						if (this.scrollSpeed > 0f)
						{
							this.scrollSpeed -= this.accel;
							if (this.scrollSpeed < 0f)
							{
								this.scrollSpeed = 0f;
							}
						}
						else if (this.scrollSpeed < 0f)
						{
							this.scrollSpeed += this.accel;
							if (this.scrollSpeed > 0f)
							{
								this.scrollSpeed = 0f;
							}
						}
					}
					if (num2 < this.adj_threshold)
					{
						if (num2 > this.min_threshold)
						{
							if (num > 0f)
							{
								this.adjustLoc = this.AdjustLocPlus(0);
								this.isAdjusting = true;
							}
							else
							{
								this.adjustLoc = this.AdjustLocMinus(0);
								this.isAdjusting = true;
							}
						}
						else
						{
							this.adjustLoc = this.AdjustLocNear();
							if (this.selectLoc != this.adjustLoc)
							{
								this.isAdjusting = true;
							}
						}
					}
				}
				else
				{
					this.selectLoc += (this.adjustLoc - this.selectLoc) / this.adj_per_div;
					float num3;
					if (this.MoveCt == 0)
					{
						num3 = this.zero_thrh_s;
					}
					else
					{
						num3 = this.zero_thrh_l;
					}
					if (Math.Abs(this.adjustLoc - this.selectLoc) < num3)
					{
						this.selectLoc = this.adjustLoc;
						this.scrollSpeed = 0f;
						this.isAdjusting = false;
					}
					else if (num2 >= this.adj_threshold)
					{
						this.isAdjusting = false;
					}
				}
			}
		}
		this.StopAtEndFunc();
		this.UpdateMoveLR();
		this.UpdatePartPosAll();
	}

	private void StopAtEndFunc()
	{
		if (this.stopAtEnd)
		{
			float usePitch = this.GetUsePitch();
			float num = 0f;
			float num2 = usePitch * (float)(this.partObjs.Count - 1);
			float num3 = num - usePitch / 3f;
			float num4 = num2 + usePitch / 3f;
			if (this.selectLoc < num)
			{
				this.scrollSpeed = 0f;
				this.adjustLoc = this.AdjustLocPlus(0);
			}
			if (this.selectLoc > num2)
			{
				this.scrollSpeed = 0f;
				this.adjustLoc = this.AdjustLocMinus(0);
			}
			if (this.selectLoc < num3)
			{
				this.selectLoc = num3;
			}
			else if (this.selectLoc > num4)
			{
				this.selectLoc = num4;
			}
		}
	}
}
