using System;
using UnityEngine;

public class GUISelectPanelViewUD : GUISelectPanelViewControlUD
{
	private float verticalMoveBorder_;

	private float maxLocate_ = 1000f;

	private float minLocate_ = -1000f;

	protected float borderMaxLocate = 1000f;

	protected float borderMinLocate = -1000f;

	protected float selectLoc;

	protected Vector2 startLoc = new Vector2(0f, 0f);

	protected Rect listViewRect;

	private Rect listWindowViewRect;

	private Rect viewRect_;

	private bool enableScroll = true;

	private bool overScroll = true;

	private GUICollider parent;

	private bool touch_begin;

	private bool stillScroll;

	private bool bShowTop = true;

	private float scrollSpeed;

	private float panelSpeed_;

	private float activeMargin = 400f;

	private Action<GUIListPartBS> panelAreaOverTopAction;

	private Action<GUIListPartBS> panelAreaOverBottomAction;

	private Action<int> actCBFadeAll;

	private int actFadeAllCT;

	private int startFadeEfcCT;

	public float verticalMoveBorder
	{
		get
		{
			return this.verticalMoveBorder_;
		}
		set
		{
			this.verticalMoveBorder_ = value;
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
		this.borderMaxLocate = this.maxLocate_ + this.verticalMoveBorder_;
		this.borderMinLocate = this.minLocate_ - this.verticalMoveBorder_;
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
			this.viewRect_ = new Rect(this.listWindowViewRect.xMin, this.listWindowViewRect.yMin, this.listWindowViewRect.width, this.listWindowViewRect.height);
		}
	}

	public Rect viewRect
	{
		get
		{
			return this.viewRect_;
		}
	}

	public float SelectLoc
	{
		get
		{
			return this.selectLoc;
		}
		set
		{
			this.selectLoc = value;
		}
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

	public GUICollider Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.listViewRect = base.boundingRect;
		this.verticalMoveBorder = 160f;
	}

	protected virtual GUISelectPanelViewControlUD.ListPartsData AddBuildPart()
	{
		return null;
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
		if (this.parent != null)
		{
			this.parent.OnTouchBegan(touch, pos);
		}
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
		if (Mathf.Abs(this.startLoc.y - pos.y) > Mathf.Abs(this.scrollSpeed))
		{
		}
		if (this.EnableScroll)
		{
			this.scrollSpeed = this.startLoc.y - pos.y;
		}
		else
		{
			this.scrollSpeed = 0f;
		}
		this.selectLoc -= this.scrollSpeed;
		this.selectLoc = Mathf.Max(Mathf.Min(this.selectLoc, this.borderMaxLocate), this.borderMinLocate);
		this.startLoc = pos;
		if (this.parent != null)
		{
			this.parent.OnTouchMoved(touch, pos);
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
		base.OnTouchEnded(touch, pos, flag);
		this.touch_begin = false;
		if (this.parent != null)
		{
			this.parent.OnTouchEnded(touch, pos, flag);
		}
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

	public float panelSpeed
	{
		get
		{
			return this.panelSpeed_;
		}
	}

	protected override void Update()
	{
		if (this.partObjs == null)
		{
			return;
		}
		base.Update();
		this.panelSpeed_ = this.scrollSpeed;
		if (!this.isTouchMoved)
		{
			this.panelSpeed_ = this.selectLoc;
			if (this.EnableScroll)
			{
				if (this.selectLoc > this.maxLocate_)
				{
					float num = (this.selectLoc - this.maxLocate_) / this.verticalMoveBorder_;
					this.selectLoc -= num * 40f + 1f;
					if (this.selectLoc >= this.borderMaxLocate)
					{
						this.scrollSpeed = 0f;
					}
				}
				else if (this.selectLoc < this.minLocate_)
				{
					float num2 = (this.selectLoc - this.minLocate_) / this.verticalMoveBorder_;
					this.selectLoc -= num2 * 40f + 1f;
					if (this.selectLoc <= this.borderMinLocate)
					{
						this.scrollSpeed = 0f;
					}
				}
				this.selectLoc -= this.scrollSpeed;
			}
			if (!this.stillScroll)
			{
				this.scrollSpeed = (float)Math.Sign(this.scrollSpeed) * Math.Max(Math.Abs(this.scrollSpeed) - (float)Math.Pow((double)Math.Abs(this.scrollSpeed), 1.2000000476837158) * 0.05f, 0f);
				if (Math.Abs(this.scrollSpeed) < 1f)
				{
					this.scrollSpeed = 0f;
				}
			}
			this.panelSpeed_ -= this.selectLoc;
		}
		if (!this.overScroll)
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
		base.transform.SetLocalY(this.selectLoc);
		this.UpdateActive();
		this.UpdateFadeAll();
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
			if (this.partObjs[i].csParts != null)
			{
				GameObject gameObject = this.partObjs[i].csParts.gameObject;
				Vector3 localPosition = gameObject.transform.localPosition;
				float y = gameObject.transform.position.y;
				if (y >= 600f || y <= -600f)
				{
					localPosition.z = -10000f;
					gameObject.transform.localPosition = localPosition;
				}
			}
		}
	}

	public void ShowOutParts()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			if (this.partObjs[i].csParts != null)
			{
				GameObject gameObject = this.partObjs[i].csParts.gameObject;
				Vector3 localPosition = gameObject.transform.localPosition;
				localPosition.z = this.partObjs[i].csParts.GetOriginalPos().z;
				gameObject.transform.localPosition = localPosition;
			}
		}
	}

	protected void SetActiveMargin(float fd)
	{
		this.activeMargin = fd;
	}

	protected void UpdateActive()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			if (this.partObjs[i].csParts != null)
			{
				GameObject gameObject = this.partObjs[i].csParts.gameObject;
				Vector3 a = gameObject.transform.localPosition;
				a += base.gameObject.transform.localPosition;
				bool flag = false;
				if (this.partObjs[i].csParts.GetActCT() > 0 || this.partObjs[i].csParts.IsFadeDo())
				{
					flag = true;
				}
				if (!flag && (a.y > this.activeMargin + GUIMain.VerticalSpaceSize || a.y < -this.activeMargin - GUIMain.VerticalSpaceSize))
				{
					if (gameObject.activeSelf)
					{
						gameObject.SetActive(false);
						if (a.y > this.activeMargin + GUIMain.VerticalSpaceSize)
						{
							this.DoPanelAreaOverTopAction(this.partObjs[i].csParts);
						}
						else if (a.y < -this.activeMargin - GUIMain.VerticalSpaceSize)
						{
							this.DoPanelAreaOverBottomAction(this.partObjs[i].csParts);
						}
					}
				}
				else if (!gameObject.activeSelf)
				{
					gameObject.SetActive(true);
				}
			}
		}
	}

	protected void SetAreaOverTopAction(Action<GUIListPartBS> action)
	{
		this.panelAreaOverTopAction = action;
	}

	protected void SetAreaOverBottomAction(Action<GUIListPartBS> action)
	{
		this.panelAreaOverBottomAction = action;
	}

	private void DoPanelAreaOverTopAction(GUIListPartBS obj)
	{
		if (this.panelAreaOverTopAction != null)
		{
			this.panelAreaOverTopAction(obj);
		}
	}

	private void DoPanelAreaOverBottomAction(GUIListPartBS obj)
	{
		if (this.panelAreaOverBottomAction != null)
		{
			this.panelAreaOverBottomAction(obj);
		}
	}

	public void FadeOutAllListParts(Action<int> act = null, bool immediate = false)
	{
		this.actCBFadeAll = act;
		this.actFadeAllCT = 0;
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			if (this.partObjs[i].csParts != null)
			{
				this.actFadeAllCT++;
				this.partObjs[i].csParts.FadeOutEfc(new Action<int>(this.FadeAllCB), immediate);
			}
		}
	}

	public void FadeInAllListParts(Action<int> act = null)
	{
		this.actCBFadeAll = act;
		this.actFadeAllCT = 0;
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			if (this.partObjs[i].csParts != null)
			{
				this.actFadeAllCT++;
				this.partObjs[i].csParts.FadeInEfc(new Action<int>(this.FadeAllCB));
			}
		}
	}

	private void FadeAllCB(int i)
	{
		this.UpdateFadeAll();
		this.actFadeAllCT--;
	}

	private void UpdateFadeAll()
	{
		if (this.actCBFadeAll != null && this.actFadeAllCT == 0)
		{
			this.actCBFadeAll(0);
			this.actCBFadeAll = null;
		}
	}

	public void ClearActCBFadeAll()
	{
		this.actCBFadeAll = null;
	}

	protected void InitEfcAllListParts()
	{
		for (int i = 0; i < this.csPartsList.Count; i++)
		{
			this.csPartsList[i].csParts.InitEfc();
		}
	}

	public int StartFadeEfcCT
	{
		get
		{
			return this.startFadeEfcCT;
		}
		set
		{
			this.startFadeEfcCT = value;
			if (this.startFadeEfcCT < 0)
			{
				this.startFadeEfcCT = 0;
			}
		}
	}
}
