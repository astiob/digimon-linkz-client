using NGUI.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CommonDialog : GUICollider
{
	private CommonDialog.ACT_STATUS act_status;

	public float waitingTime = 0.02f;

	private float openWait;

	public bool permanentMode;

	public CommonDialog.DialogLocation dialogLocation;

	private Vector2[] dialogLocationTable = new Vector2[]
	{
		new Vector2(0f, 0f),
		new Vector2(0f, 140f),
		new Vector2(0f, -140f)
	};

	public bool dontManageZPos;

	private int forceReturnVL = -1;

	protected Action<int> finish;

	private Action windowClosedAction;

	protected Action<int> opened;

	public CommonDialog.ReturnOrder returnOrder = CommonDialog.ReturnOrder.SortName;

	private Vector2 maxSize;

	private Vector2 firstSize;

	private float animeTime;

	protected Vector3 startPos;

	private bool isColliderDisable;

	public Color barrierColor = new Color(1f, 1f, 1f, 0.784313738f);

	private int originLeftAnchorAbsolute;

	private Action actCallBackLast;

	private List<GUICollider> touchPanels = new List<GUICollider>();

	protected List<Action<Touch, Vector2, bool>> eventList;

	private bool isScaled;

	protected CommonDialog.MOVE_BEHAVIOUR moveBehavior = CommonDialog.MOVE_BEHAVIOUR.Y_SCALE;

	private float moveLength = 1200f;

	private bool closeDialog_ = true;

	protected int returnVal;

	private bool _opened;

	[SerializeField]
	protected bool enableAndroidBackKey = true;

	public CommonDialog.ACT_STATUS GetActionStatus()
	{
		return this.act_status;
	}

	public void SetForceReturnValue(int vl)
	{
		this.forceReturnVL = vl;
	}

	public void SetCloseAction(Action<int> act)
	{
		this.finish = act;
	}

	public void SetWindowClosedAction(Action action)
	{
		this.windowClosedAction = action;
	}

	public bool ClosePanelRecursive { get; set; }

	public bool CanClosePanelRecursive { get; set; }

	public void AddWidgetDepth(int add)
	{
		this.AddWidgetDepth(base.transform, add);
	}

	private void AddWidgetDepth(Transform tm, int add)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth += add;
		}
		IEnumerator enumerator = tm.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform tm2 = (Transform)obj;
				this.AddWidgetDepth(tm2, add);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action onOpened;

	public List<GUICollider> TouchPanels
	{
		get
		{
			return this.touchPanels;
		}
	}

	protected override void Awake()
	{
		if (base.gameObject.name.EndsWith("(Clone)"))
		{
			base.gameObject.name = base.gameObject.name.Substring(0, base.gameObject.name.Length - 7);
		}
		this.startPos = base.transform.localPosition;
		base.Awake();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform tm = (Transform)obj;
				this.SearchChild(tm);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (this.touchPanels.Count == 0)
		{
			base.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				if (flag && !this.permanentMode)
				{
					this.returnVal = 0;
					this.ClosePanel(true);
				}
			};
		}
		this.touchBehavior = GUICollider.TouchBehavior.None;
		GUIManager.AddCommonDialog(this);
	}

	private void SearchChild(Transform tm)
	{
		GUICollider component = tm.gameObject.GetComponent<GUICollider>();
		if (component != null && !component.dontAddToDialogEvent)
		{
			this.touchPanels.Add(component);
		}
		IEnumerator enumerator = tm.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform tm2 = (Transform)obj;
				this.SearchChild(tm2);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	protected virtual void MakeAnimation(bool open, float atime, iTween.EaseType type = iTween.EaseType.linear)
	{
		float num;
		float num2;
		if (open)
		{
			num = 0f;
			num2 = 1f;
		}
		else
		{
			num = 1f;
			num2 = 0f;
		}
		this.isScaled = false;
		try
		{
			if (base.gameObject == null)
			{
			}
		}
		catch
		{
			this.ScaleEnd(0);
			return;
		}
		if (atime <= 0f)
		{
			base.gameObject.transform.localScale = new Vector3(num2, num2, 1f);
			this.ScaleEnd(0);
			this.Update();
		}
		else
		{
			CommonDialog.MOVE_BEHAVIOUR move_BEHAVIOUR = this.moveBehavior;
			switch (move_BEHAVIOUR + 1)
			{
			case CommonDialog.MOVE_BEHAVIOUR.XY_SCALE:
				base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				this.isScaled = true;
				return;
			case CommonDialog.MOVE_BEHAVIOUR.X_SCALE:
				base.gameObject.transform.localScale = new Vector3(num, num, 1f);
				break;
			case CommonDialog.MOVE_BEHAVIOUR.Y_SCALE:
				base.gameObject.transform.localScale = new Vector3(num, 1f, 1f);
				break;
			case CommonDialog.MOVE_BEHAVIOUR.MOVE_FROM_LEFT:
				base.gameObject.transform.localScale = new Vector3(1f, num, 1f);
				break;
			case CommonDialog.MOVE_BEHAVIOUR.MOVE_FROM_RIGHT:
			{
				Vector3 localPosition = base.gameObject.transform.localPosition;
				localPosition.x = -this.moveLength;
				base.gameObject.transform.localPosition = localPosition;
				break;
			}
			case CommonDialog.MOVE_BEHAVIOUR.XY_SCALE_CLOSE:
			{
				Vector3 localPosition = base.gameObject.transform.localPosition;
				localPosition.x = this.moveLength;
				base.gameObject.transform.localPosition = localPosition;
				break;
			}
			case (CommonDialog.MOVE_BEHAVIOUR)6:
				if (open)
				{
					base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				else
				{
					base.gameObject.transform.localScale = new Vector3(num, num, 1f);
				}
				break;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("from", num);
			hashtable.Add("to", num2);
			hashtable.Add("time", EfcCont.GetFixedTime(atime));
			hashtable.Add("onupdate", "UpdateScale");
			hashtable.Add("easetype", type);
			hashtable.Add("oncomplete", "ScaleEnd");
			hashtable.Add("oncompleteparams", 0);
			iTween.ValueTo(base.gameObject, hashtable);
		}
	}

	private void UpdateScale(float scl)
	{
		switch (this.moveBehavior)
		{
		case CommonDialog.MOVE_BEHAVIOUR.XY_SCALE:
			base.gameObject.transform.localScale = new Vector3(scl, scl, 1f);
			break;
		case CommonDialog.MOVE_BEHAVIOUR.X_SCALE:
			base.gameObject.transform.localScale = new Vector3(scl, 1f, 1f);
			break;
		case CommonDialog.MOVE_BEHAVIOUR.Y_SCALE:
			base.gameObject.transform.localScale = new Vector3(1f, scl, 1f);
			break;
		case CommonDialog.MOVE_BEHAVIOUR.MOVE_FROM_LEFT:
		{
			Vector3 localPosition = base.gameObject.transform.localPosition;
			localPosition.x = -this.moveLength * (1f - scl);
			base.gameObject.transform.localPosition = localPosition;
			break;
		}
		case CommonDialog.MOVE_BEHAVIOUR.MOVE_FROM_RIGHT:
		{
			Vector3 localPosition = base.gameObject.transform.localPosition;
			localPosition.x = this.moveLength * (1f - scl);
			base.gameObject.transform.localPosition = localPosition;
			break;
		}
		case CommonDialog.MOVE_BEHAVIOUR.XY_SCALE_CLOSE:
			base.gameObject.transform.localScale = new Vector3(scl, scl, 1f);
			break;
		}
	}

	protected void ScaleEnd(int id)
	{
		this.isScaled = true;
	}

	protected void SetScaleFlg(bool flg)
	{
		this.isScaled = flg;
	}

	protected bool startShow { get; set; }

	protected bool endShow { get; set; }

	protected bool closeDialog
	{
		get
		{
			return this.closeDialog_;
		}
		set
		{
			this.closeDialog_ = value;
		}
	}

	public virtual void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.finish = f;
		base.gameObject.SetActive(true);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x += this.dialogLocationTable[(int)this.dialogLocation].x;
		localPosition.y += this.dialogLocationTable[(int)this.dialogLocation].y;
		base.transform.localPosition = localPosition;
		this.animeTime = aT;
		int i = 0;
		while (i < this.touchPanels.Count)
		{
			if (null == this.touchPanels[i])
			{
				this.touchPanels.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		this.MakeAnimation(true, this.animeTime, iTween.EaseType.linear);
		switch (this.returnOrder)
		{
		case CommonDialog.ReturnOrder.SortTransform:
			this.touchPanels.Sort((GUICollider x, GUICollider y) => Math.Sign(x.gObject.transform.localPosition.x - y.gObject.transform.localPosition.x));
			break;
		case CommonDialog.ReturnOrder.SortName:
			this.touchPanels.Sort((GUICollider x, GUICollider y) => string.Compare(x.name, y.name));
			break;
		case CommonDialog.ReturnOrder.SortVerticalTransform:
			this.touchPanels.Sort((GUICollider x, GUICollider y) => Math.Sign(-x.gObject.transform.localPosition.y + y.gObject.transform.localPosition.y));
			break;
		}
		this.eventList = new List<Action<Touch, Vector2, bool>>();
		for (int j = 0; j < this.touchPanels.Count; j++)
		{
			this.AddButtonAction(j, this.touchPanels[j]);
		}
		this.openWait = this.waitingTime;
		this.startShow = true;
		this.endShow = false;
		if (!this.isColliderDisable)
		{
			GUICollider.DisableAllCollider("Awake CommonDialog = " + base.gameObject.name);
			this.isColliderDisable = true;
		}
	}

	public virtual void AddButtonAction(int index, GUICollider col)
	{
		Action<Touch, Vector2, bool> action = delegate(Touch touch, Vector2 pos, bool flag)
		{
			if (flag)
			{
				this.returnVal = index;
				if (null == col.CallBackClass)
				{
					this.ClosePanel(true);
				}
			}
		};
		this.eventList.Add(action);
		col.onTouchEnded += action;
	}

	public bool IsClosing()
	{
		return this.endShow;
	}

	public bool IsOpened()
	{
		return this._opened;
	}

	public virtual void ClosePanel(bool animation = true)
	{
		if (!this.isColliderDisable)
		{
			GUICollider.DisableAllCollider("ClosePanel CommonDialog = " + base.gameObject.name);
			this.isColliderDisable = true;
		}
		this.startShow = false;
		this.endShow = true;
		if (animation)
		{
			this.MakeAnimation(false, this.animeTime, iTween.EaseType.linear);
		}
		else
		{
			this.MakeAnimation(false, 0f, iTween.EaseType.linear);
		}
		this.act_status = CommonDialog.ACT_STATUS.CLOSING;
	}

	public virtual void ClosePanelNotEndShow(bool animation = true)
	{
		this.startShow = false;
		if (animation)
		{
			this.MakeAnimation(false, this.animeTime, iTween.EaseType.linear);
		}
		else
		{
			this.MakeAnimation(false, 0f, iTween.EaseType.linear);
		}
	}

	protected void ShowDLG()
	{
		UISafeArea component = base.GetComponent<UISafeArea>();
		if (null != component)
		{
			UIWidget component2 = component.GetComponent<UIWidget>();
			component2.leftAnchor.absolute = this.originLeftAnchorAbsolute;
		}
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.x = 0f;
		base.gameObject.transform.localPosition = localPosition;
	}

	protected void HideDLG()
	{
		UISafeArea component = base.GetComponent<UISafeArea>();
		if (null != component)
		{
			UIWidget component2 = component.GetComponent<UIWidget>();
			this.originLeftAnchorAbsolute = component2.leftAnchor.absolute;
			component2.leftAnchor.absolute = 8000;
		}
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.x = 8000f;
		base.gameObject.transform.localPosition = localPosition;
	}

	public override void HideGUI()
	{
	}

	public void SetOnOpened(Action<int> act)
	{
		this.opened = act;
	}

	protected virtual void WindowOpened()
	{
		if (this.onOpened != null)
		{
			this.onOpened();
		}
		if (this.opened != null)
		{
			this.opened(0);
		}
	}

	protected virtual void WindowClosed()
	{
		this.act_status = CommonDialog.ACT_STATUS.CLOSED;
		if (this.windowClosedAction != null)
		{
			this.windowClosedAction();
		}
	}

	public void SetLastCallBack(Action act)
	{
		this.actCallBackLast = act;
	}

	protected override void Update()
	{
		base.Update();
		if (this.startShow)
		{
			if (this.isScaled)
			{
				float deltaTime = Time.deltaTime;
				if (this.openWait < 0f)
				{
					this._opened = true;
					this.WindowOpened();
					if (this.act_status == CommonDialog.ACT_STATUS.START && this.isColliderDisable)
					{
						GUICollider.EnableAllCollider("WindowOpened CommonDialog = " + base.gameObject.name);
						this.isColliderDisable = false;
					}
					this.act_status = CommonDialog.ACT_STATUS.OPEN;
					this.startShow = false;
					this.closeDialog = false;
				}
				this.openWait -= deltaTime;
			}
		}
		else if (this.endShow)
		{
			if (this.isScaled)
			{
				base.transform.localPosition = this.startPos;
				this.HideGUI();
				GUIManager.HideCommonDialog(this);
				if (this.eventList != null)
				{
					int num = 0;
					foreach (GUICollider guicollider in this.touchPanels)
					{
						guicollider.onTouchEnded -= this.eventList[num];
						num++;
					}
				}
				this.endShow = false;
				this.closeDialog = true;
				base.gameObject.SetActive(false);
				GUIManager.DeleteCommonDialog(this);
				if (this.finish != null)
				{
					if (-1 < this.forceReturnVL)
					{
						this.finish(this.forceReturnVL);
					}
					else
					{
						this.finish(this.returnVal);
					}
				}
				if (this.ClosePanelRecursive)
				{
					CommonDialog topDialog = GUIManager.GetTopDialog(this, false);
					if (topDialog != null && topDialog.CanClosePanelRecursive)
					{
						topDialog.ClosePanelRecursive = true;
						topDialog.ClosePanel(true);
					}
				}
				UnityEngine.Object.Destroy(base.gameObject);
				if (this.isColliderDisable)
				{
					GUICollider.EnableAllCollider("WindowClosed CommonDialog = " + base.gameObject.name);
					this.isColliderDisable = false;
				}
				this.WindowClosed();
				if (this.actCallBackLast != null)
				{
					this.actCallBackLast();
				}
			}
		}
		else if (this.touchPanels.Count == 0 && !this.closeDialog && !this.permanentMode && (GUIManager.someOneTouch || Input.GetKeyDown(KeyCode.Escape)))
		{
			global::Debug.Log("touchPanels");
			this.OnTouchBegan(default(Touch), default(Vector2));
			this.OnTouchEnded(default(Touch), default(Vector2), true);
		}
		if (GUIManager.ExtBackKeyReady && Input.GetKeyDown(KeyCode.Escape))
		{
			CommonDialog topDialog2 = GUIManager.GetTopDialog(null, false);
			if (topDialog2 != null && topDialog2.gameObject.name == base.gameObject.name && this.enableAndroidBackKey && !this.permanentMode && this.act_status == CommonDialog.ACT_STATUS.OPEN && !GUICollider.IsAllColliderDisable())
			{
				this.returnVal = -1;
				this.ClosePanel(true);
				SoundMng.Instance().PlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1, 1f);
			}
		}
	}

	protected override void OnDestroy()
	{
		Resources.UnloadUnusedAssets();
		base.OnDestroy();
	}

	public enum ACT_STATUS
	{
		START,
		OPEN,
		CLOSING,
		CLOSED
	}

	public enum DialogLocation
	{
		Center,
		Top,
		Bottom
	}

	public enum ReturnOrder
	{
		SortTransform,
		SortName,
		SortVerticalTransform
	}

	public enum MOVE_BEHAVIOUR
	{
		NONE = -1,
		XY_SCALE,
		X_SCALE,
		Y_SCALE,
		MOVE_FROM_LEFT,
		MOVE_FROM_RIGHT,
		XY_SCALE_CLOSE
	}
}
