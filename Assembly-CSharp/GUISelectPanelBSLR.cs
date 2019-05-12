using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelBSLR : GUICollider
{
	private float horizonMoveBorder_;

	private float maxLocate_ = 1000f;

	private float minLocate_ = -1000f;

	protected float borderMaxLocate = 1000f;

	protected float borderMinLocate = -1000f;

	protected float selectLoc;

	protected Vector2 startLoc = new Vector2(0f, 0f);

	protected Rect listViewRect;

	private Rect listWindowViewRect;

	private Rect viewRect_;

	public bool isLREnabled = true;

	protected float arrowOffestX = 280f;

	public List<GUIListPartBS> partObjs;

	private bool enableScroll = true;

	private bool enableEternalScroll;

	private float accel = 4f;

	private bool touch_begin;

	private GameObject goMoveLeft;

	private GameObject goMoveRight;

	private GUICollider csMoveLeft;

	private GUICollider csMoveRight;

	public Vector3 MoveLeft_pos;

	public Vector3 MoveRight_pos;

	private bool stillScroll;

	private bool bShowTop = true;

	private bool freeScrollMode = true;

	private bool overScroll = true;

	[SerializeField]
	private int maxTouchMoveCount = 3;

	private int MoveCt;

	private bool initSwipeSE;

	private int curSelectIdx;

	private float scrollSpeed;

	private float panelSpeed_;

	private bool isAdjusting;

	private float adjustLoc;

	public float horizonMoveBorder
	{
		get
		{
			return this.horizonMoveBorder_;
		}
		set
		{
			this.horizonMoveBorder_ = value;
			this.calcMoveBorder();
		}
	}

	public float maxLocate
	{
		get
		{
			return this.maxLocate_;
		}
		set
		{
			this.maxLocate_ = value;
			this.calcMoveBorder();
		}
	}

	public float minLocate
	{
		get
		{
			return this.minLocate_;
		}
		set
		{
			this.minLocate_ = value;
			this.calcMoveBorder();
		}
	}

	private void calcMoveBorder()
	{
		this.borderMaxLocate = this.maxLocate_ + this.horizonMoveBorder_;
		this.borderMinLocate = this.minLocate_ - this.horizonMoveBorder_;
	}

	public Rect ListWindowViewRect
	{
		get
		{
			return this.listWindowViewRect;
		}
		set
		{
			this.listWindowViewRect = value;
			this.viewRect_ = new Rect(this.ListWindowViewRect.xMin, this.ListWindowViewRect.yMin, this.ListWindowViewRect.width, this.ListWindowViewRect.height);
		}
	}

	public Rect viewRect
	{
		get
		{
			return this.viewRect_;
		}
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

	public bool EnableEternalScroll
	{
		get
		{
			return this.enableEternalScroll;
		}
		set
		{
			this.enableEternalScroll = value;
		}
	}

	public float Accel
	{
		get
		{
			return this.accel;
		}
		set
		{
			this.accel = value;
		}
	}

	protected virtual void PlaySwipeSE()
	{
	}

	protected override void Awake()
	{
		base.Awake();
		this.listViewRect = base.boundingRect;
		this.horizonMoveBorder = 160f;
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
	}

	protected void Start()
	{
		Transform transform = null;
		Transform transform2 = null;
		foreach (object obj in base.transform)
		{
			Transform transform3 = (Transform)obj;
			if (transform3.name == "MoveLeftBtn")
			{
				this.goMoveLeft = transform3.gameObject;
				this.csMoveLeft = this.goMoveLeft.GetComponent<GUICollider>();
				if (this.csMoveLeft != null)
				{
					this.csMoveLeft.onTouchBegan += delegate(Touch touch, Vector2 pos)
					{
						this.OnTouchMoveLeft();
					};
				}
				transform2 = transform3;
			}
			if (transform3.name == "MoveRightBtn")
			{
				this.goMoveRight = transform3.gameObject;
				this.csMoveRight = this.goMoveRight.GetComponent<GUICollider>();
				if (this.csMoveRight != null)
				{
					this.csMoveRight.onTouchBegan += delegate(Touch touch, Vector2 pos)
					{
						this.OnTouchMoveRight();
					};
				}
				transform = transform3;
			}
		}
		if (base.transform.parent != null)
		{
			if (transform2 != null)
			{
				Vector3 localScale = transform2.localScale;
				transform2.parent = base.transform.parent;
				transform2.localScale = localScale;
			}
			if (transform != null)
			{
				Vector3 localScale = transform.localScale;
				transform.parent = base.transform.parent;
				transform.localScale = localScale;
			}
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
		if (this.EnableScroll && this.partObjs.Count > 1)
		{
			this.scrollSpeed = this.startLoc.x - pos.x;
		}
		else
		{
			this.scrollSpeed = 0f;
		}
		this.selectLoc -= this.scrollSpeed;
		if (!this.enableEternalScroll)
		{
			this.selectLoc = Mathf.Max(Mathf.Min(this.selectLoc, this.borderMaxLocate), this.borderMinLocate);
		}
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

	public bool ShowTop
	{
		get
		{
			return this.bShowTop;
		}
		set
		{
			this.bShowTop = value;
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

	public bool OverScroll
	{
		get
		{
			return this.overScroll;
		}
		set
		{
			this.overScroll = value;
		}
	}

	public void OnTouchMoveLeft()
	{
		this.MoveCt++;
		if (this.MoveCt > this.maxTouchMoveCount)
		{
			this.MoveCt = this.maxTouchMoveCount;
		}
	}

	public void OnTouchMoveRight()
	{
		this.MoveCt--;
		if (this.MoveCt < -this.maxTouchMoveCount)
		{
			this.MoveCt = -this.maxTouchMoveCount;
		}
	}

	protected void CancelMoveLR()
	{
		this.MoveCt = 0;
	}

	public void ShowMoveLR()
	{
		this.ShowMoveL();
		this.ShowMoveR();
	}

	public void ShowMoveL()
	{
		if (this.goMoveLeft != null)
		{
			this.goMoveLeft.transform.localPosition = this.MoveLeft_pos;
			if (!this.goMoveLeft.activeSelf)
			{
				this.goMoveLeft.SetActive(true);
			}
		}
	}

	public void ShowMoveR()
	{
		if (this.goMoveRight != null)
		{
			this.goMoveRight.transform.localPosition = this.MoveRight_pos;
			if (!this.goMoveRight.activeSelf)
			{
				this.goMoveRight.SetActive(true);
			}
		}
	}

	public void HideMoveLR()
	{
		this.HideMoveL();
		this.HideMoveR();
	}

	public void HideMoveL()
	{
		if (this.goMoveLeft != null && this.goMoveLeft.activeSelf)
		{
			this.goMoveLeft.SetActive(false);
		}
	}

	public void HideMoveR()
	{
		if (this.goMoveRight != null && this.goMoveRight.activeSelf)
		{
			this.goMoveRight.SetActive(false);
		}
	}

	private void UpdateMoveLR()
	{
		int count = this.partObjs.Count;
		float usePitch = this.GetUsePitch();
		if (!this.isAdjusting)
		{
			if (this.MoveCt > 0)
			{
				if (!this.enableEternalScroll)
				{
					if (this.selectLoc < this.minLocate)
					{
						this.selectLoc = this.minLocate;
					}
					if (this.selectLoc > this.maxLocate)
					{
						this.selectLoc = this.maxLocate;
						this.CancelMoveLR();
						return;
					}
					for (int i = 0; i < count; i++)
					{
						if (this.minLocate_ + usePitch * (float)i > this.selectLoc + 1f)
						{
							this.isAdjusting = true;
							this.adjustLoc = this.minLocate_ + usePitch * (float)i;
							global::Debug.Log("==================MOVR -> ADJ = " + this.adjustLoc.ToString() + "SL =" + this.selectLoc.ToString());
							break;
						}
					}
				}
				else
				{
					for (int i = -1; i < count + 1; i++)
					{
						if (this.minLocate_ + usePitch * (float)i > this.selectLoc + 1f)
						{
							this.isAdjusting = true;
							this.adjustLoc = this.minLocate_ + usePitch * (float)i;
							global::Debug.Log("==================MOVR -> ADJ = " + this.adjustLoc.ToString() + "SL =" + this.selectLoc.ToString());
							break;
						}
					}
				}
				this.MoveCt--;
			}
			if (this.MoveCt < 0)
			{
				if (!this.enableEternalScroll)
				{
					if (this.selectLoc < this.minLocate)
					{
						this.selectLoc = this.minLocate;
						this.CancelMoveLR();
						return;
					}
					if (this.selectLoc > this.maxLocate)
					{
						this.selectLoc = this.maxLocate;
					}
					for (int i = count - 1; i >= 0; i--)
					{
						if (this.minLocate_ + usePitch * (float)i < this.selectLoc - 1f)
						{
							this.isAdjusting = true;
							this.adjustLoc = this.minLocate_ + usePitch * (float)i;
							global::Debug.Log("==================MOVL -> ADJ = " + this.adjustLoc.ToString() + "SL =" + this.selectLoc.ToString());
							break;
						}
					}
				}
				else
				{
					for (int i = count - 1 + 1; i >= -1; i--)
					{
						if (this.minLocate_ + usePitch * (float)i < this.selectLoc - 1f)
						{
							this.isAdjusting = true;
							this.adjustLoc = this.minLocate_ + usePitch * (float)i;
							global::Debug.Log("==================MOVL -> ADJ = " + this.adjustLoc.ToString() + "SL =" + this.selectLoc.ToString());
							break;
						}
					}
				}
				this.MoveCt++;
			}
		}
		if (!this.enableEternalScroll)
		{
			if (this.GetSelectedIdx() == 0)
			{
				this.HideMoveR();
			}
			else
			{
				this.ShowMoveR();
			}
			float num = usePitch / 2f;
			float num2 = this.maxLocate_ - this.selectLoc;
			if (num2 <= num)
			{
				this.HideMoveL();
			}
			else
			{
				this.ShowMoveL();
			}
		}
		else
		{
			this.ShowMoveLR();
		}
		int selectedIdx = this.GetSelectedIdx();
		if (!this.initSwipeSE)
		{
			this.curSelectIdx = selectedIdx;
			this.initSwipeSE = true;
		}
		if (this.curSelectIdx != selectedIdx)
		{
			this.PlaySwipeSE();
			this.curSelectIdx = selectedIdx;
		}
	}

	public float GetUsePitch()
	{
		GUISelectPanelBSPartsLR guiselectPanelBSPartsLR = (GUISelectPanelBSPartsLR)this;
		return guiselectPanelBSPartsLR.selectCollider.width + guiselectPanelBSPartsLR.horizontalMargin;
	}

	public void SetSelectedIndex(int _index)
	{
		this.selectLoc = this.minLocate_ + this.GetUsePitch() * (float)_index;
	}

	public void SetSelectedIndexRev(int _index)
	{
		this.selectLoc = this.minLocate_ + this.GetUsePitch() * (float)(this.partObjs.Count - (_index + 1));
	}

	public int GetSelectedIdx()
	{
		int result = 0;
		float num = 10000f;
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			float num2 = Math.Abs(this.minLocate_ + this.GetUsePitch() * (float)i - this.selectLoc);
			if (num > num2)
			{
				result = i;
				num = num2;
			}
		}
		return result;
	}

	public float GetLocationByIdx(int idx)
	{
		return this.minLocate + this.GetUsePitch() * (float)idx;
	}

	public int GetSelectedIdxRev()
	{
		int selectedIdx = this.GetSelectedIdx();
		return this.partObjs.Count - 1 - selectedIdx;
	}

	public float GetLocationByIdxRev(int idx)
	{
		return this.minLocate + this.GetUsePitch() * (float)(this.partObjs.Count - 1 - idx);
	}

	public bool IsStopRev()
	{
		int selectedIdxRev = this.GetSelectedIdxRev();
		float locationByIdxRev = this.GetLocationByIdxRev(selectedIdxRev);
		float num = locationByIdxRev - this.selectLoc;
		if (num < 0f)
		{
			num = -num;
		}
		return num <= 40f;
	}

	public float panelSpeed
	{
		get
		{
			return this.panelSpeed_;
		}
	}

	protected void ClearAdjusting()
	{
		this.isAdjusting = false;
	}

	protected override void Update()
	{
		base.Update();
		this.panelSpeed_ = this.scrollSpeed;
		if (!this.isTouchMoved)
		{
			this.panelSpeed_ = this.selectLoc;
			if (!this.enableEternalScroll)
			{
				if (this.selectLoc > this.maxLocate_)
				{
					float num = (this.selectLoc - this.maxLocate_) / this.horizonMoveBorder_;
					this.selectLoc -= num * 40f + 0.5f;
					if (this.selectLoc >= this.borderMaxLocate)
					{
						this.scrollSpeed = 0f;
					}
				}
				else if (this.selectLoc < this.minLocate_)
				{
					float num2 = (this.selectLoc - this.minLocate_) / this.horizonMoveBorder_;
					this.selectLoc -= num2 * 40f + 1f;
					if (this.selectLoc <= this.borderMinLocate)
					{
						this.scrollSpeed = 0f;
					}
				}
			}
			int count = this.partObjs.Count;
			float usePitch = this.GetUsePitch();
			float num3 = 5f;
			float num4 = usePitch / num3;
			float num5 = 2.2f;
			float num6 = 1f;
			float num7 = 100f;
			if (this.FreeScrollMode)
			{
				this.selectLoc -= this.scrollSpeed;
				if (!this.stillScroll)
				{
					this.scrollSpeed = (float)Math.Sign(this.scrollSpeed) * Math.Max(Math.Abs(this.scrollSpeed) - (float)Math.Pow((double)Math.Abs(this.scrollSpeed), 1.2000000476837158) * 0.05f, 0f);
					if (Math.Abs(this.scrollSpeed) < num6)
					{
						this.scrollSpeed = 0f;
					}
				}
			}
			else
			{
				if (this.scrollSpeed > num7)
				{
					this.scrollSpeed = num7;
				}
				else if (this.scrollSpeed < -num7)
				{
					this.scrollSpeed = -num7;
				}
				float num8 = -this.scrollSpeed;
				float num9 = Math.Abs(num8);
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
					if (this.selectLoc <= this.maxLocate_ && this.selectLoc >= this.minLocate_ && num9 < num4)
					{
						if (num9 > num5)
						{
							if (num8 > 0f)
							{
								for (int i = 0; i < count; i++)
								{
									if (this.minLocate_ + usePitch * (float)i > this.selectLoc)
									{
										this.isAdjusting = true;
										this.adjustLoc = this.minLocate_ + usePitch * (float)i;
										break;
									}
								}
							}
							else
							{
								for (int i = count - 1; i >= 0; i--)
								{
									if (this.minLocate_ + usePitch * (float)i < this.selectLoc)
									{
										this.isAdjusting = true;
										this.adjustLoc = this.minLocate_ + usePitch * (float)i;
										break;
									}
								}
							}
						}
						else
						{
							int num10 = 0;
							float num11 = 10000f;
							for (int i = 0; i < count; i++)
							{
								float num12 = Math.Abs(this.minLocate_ + usePitch * (float)i - this.selectLoc);
								if (num11 > num12)
								{
									num10 = i;
									num11 = num12;
								}
							}
							this.adjustLoc = this.minLocate_ + usePitch * (float)num10;
							if (this.selectLoc != this.adjustLoc)
							{
								this.isAdjusting = true;
							}
						}
					}
				}
				else
				{
					this.selectLoc += (this.adjustLoc - this.selectLoc) / num3;
					float num13;
					if (this.MoveCt == 0)
					{
						num13 = 0.5f;
					}
					else
					{
						num13 = 8f;
					}
					if (Math.Abs(this.adjustLoc - this.selectLoc) < num13)
					{
						this.selectLoc = this.adjustLoc;
						this.scrollSpeed = 0f;
						this.isAdjusting = false;
					}
					else if (num9 >= num4)
					{
						this.isAdjusting = false;
					}
				}
			}
			this.panelSpeed_ -= this.selectLoc;
		}
		if (!this.enableEternalScroll && !this.overScroll)
		{
			if (this.selectLoc > this.maxLocate_)
			{
				this.selectLoc = this.maxLocate_;
			}
			else if (this.selectLoc < this.minLocate_)
			{
				this.selectLoc = this.minLocate_;
			}
		}
		if (this.isLREnabled)
		{
			base.transform.SetLocalX(this.selectLoc);
		}
		if (this.enableEternalScroll)
		{
			this.RemakeListForEternalScroll();
		}
		this.UpdateMoveLR();
	}

	private void RemakeListForEternalScroll()
	{
		GUISelectPanelBSPartsLR guiselectPanelBSPartsLR = (GUISelectPanelBSPartsLR)this;
		float num = guiselectPanelBSPartsLR.selectCollider.width + guiselectPanelBSPartsLR.horizontalMargin;
		while (this.selectLoc > this.maxLocate_)
		{
			this.maxLocate_ += num;
			this.minLocate_ += num;
			Vector3 localPosition = this.partObjs[this.partObjs.Count - 1].gameObject.transform.localPosition;
			localPosition.x -= num;
			GUIListPartBS guilistPartBS = this.partObjs[0];
			this.partObjs.RemoveAt(0);
			this.partObjs.Add(guilistPartBS);
			guilistPartBS.gameObject.transform.localPosition = localPosition;
			for (int i = 0; i < this.partObjs.Count; i++)
			{
				this.partObjs[i].IDX = i;
			}
		}
		while (this.selectLoc < this.minLocate_)
		{
			this.maxLocate_ -= num;
			this.minLocate_ -= num;
			Vector3 localPosition = this.partObjs[0].gameObject.transform.localPosition;
			localPosition.x += num;
			GUIListPartBS guilistPartBS = this.partObjs[this.partObjs.Count - 1];
			this.partObjs.RemoveAt(this.partObjs.Count - 1);
			this.partObjs.Insert(0, guilistPartBS);
			guilistPartBS.gameObject.transform.localPosition = localPosition;
			for (int i = 0; i < this.partObjs.Count; i++)
			{
				this.partObjs[i].IDX = i;
			}
		}
	}

	public virtual float GetVerticalScrollPer()
	{
		return (base.gameObject.transform.localPosition.y + this.minLocate) / base.height;
	}

	public virtual float GetHorizontalScrollPer()
	{
		return (base.gameObject.transform.localPosition.x + this.minLocate) / base.width;
	}

	public void HideOutParts()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			Vector3 localPosition = this.partObjs[i].gameObject.transform.localPosition;
			float x = this.partObjs[i].gameObject.transform.position.x;
			if (x >= 640f || x <= -640f)
			{
				localPosition.z = -10000f;
				this.partObjs[i].gameObject.transform.localPosition = localPosition;
			}
		}
	}

	public void ShowOutParts()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			Vector3 localPosition = this.partObjs[i].gameObject.transform.localPosition;
			localPosition.z = this.partObjs[i].GetOriginalPos().z;
			this.partObjs[i].gameObject.transform.localPosition = localPosition;
		}
	}
}
