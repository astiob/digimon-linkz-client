using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD : CommonDialog
{
	private PartsTitleBase partsTitle;

	private GUIMultiTab multiTab;

	public bool requestMenu;

	public bool useCMDAnim;

	public GameObject goEFC_HEADER;

	public GameObject goEFC_FOOTER;

	public GameObject goEFC_RIGHT;

	public GameObject goEFC_LEFT;

	public GameObject goEFC_BG;

	public GameObject goEFC_CTR;

	public GameObject goEFC_PARAM_HEADER;

	public GameObject goEFC_PARAM_FOOTER;

	public GameObject goEFC_PARAM_RIGHT;

	public GameObject goEFC_PARAM_LEFT;

	private EfcCont ecEFC_HEADER;

	private EfcCont ecEFC_FOOTER;

	private EfcCont ecEFC_RIGHT;

	private EfcCont ecEFC_LEFT;

	private EfcCont ecEFC_BG;

	private EfcCont ecEFC_CTR;

	private EfcParam epEFC_PARAM_HEADER;

	private EfcParam epEFC_PARAM_FOOTER;

	private EfcParam epEFC_PARAM_RIGHT;

	private EfcParam epEFC_PARAM_LEFT;

	private Vector3 vORG_HEADER;

	private Vector3 vORG_FOOTER;

	private Vector3 vORG_RIGHT;

	private Vector3 vORG_LEFT;

	private int actCt;

	private bool isActing;

	private bool isReActing;

	private bool isParentActing;

	private Color onCol = new Color(1f, 1f, 1f, 1f);

	private Color ofCol = new Color(1f, 1f, 1f, 0f);

	private Color tempCol = new Color(1f, 1f, 1f, 0f);

	private CMD _CMD_Parent;

	private bool isParentDeactive;

	private Action<int> f_cmdbk;

	private float sizeX_cmdbk;

	private float sizeY_cmdbk;

	private float aT_cmdbk;

	private bool animation_bk;

	private int hideDelayCT_S;

	private int hideDelayCT;

	private bool isBusyHideDelay;

	private Action<int> actCBSetOpendAction;

	private Func<int, bool> actCBSetReOpendAction;

	public PartsTitleBase PartsTitle
	{
		get
		{
			return this.partsTitle;
		}
		set
		{
			this.partsTitle = value;
		}
	}

	public GUIMultiTab MultiTab
	{
		get
		{
			return this.multiTab;
		}
		set
		{
			this.multiTab = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		if (this.useCMDAnim)
		{
			DepthController.RefreshWidgetDrawCall(base.transform);
		}
	}

	protected override void Update()
	{
		this.UpdateScaleFlg();
		this.UpdateWaitParent();
		this.UpdateReacting();
		base.Update();
	}

	private void actEndEfc(int i)
	{
		this.actCt--;
		GUICollider.EnableAllCollider("CMD");
	}

	private void actStartEfc()
	{
		this.actCt++;
		GUICollider.DisableAllCollider("CMD");
	}

	private void ClearActStart()
	{
		while (this.actCt > 0)
		{
			this.actEndEfc(0);
		}
	}

	public bool HideFromChild { get; set; }

	public bool DontLookParent { get; set; }

	public int GetActCount()
	{
		return this.actCt;
	}

	private void InitEFC()
	{
		if (this.goEFC_HEADER != null)
		{
			this.ecEFC_HEADER = this.goEFC_HEADER.GetComponent<EfcCont>();
			if (this.ecEFC_HEADER == null)
			{
				this.ecEFC_HEADER = this.goEFC_HEADER.AddComponent<EfcCont>();
			}
			this.vORG_HEADER = this.goEFC_HEADER.transform.localPosition;
		}
		if (this.goEFC_FOOTER != null)
		{
			this.ecEFC_FOOTER = this.goEFC_FOOTER.GetComponent<EfcCont>();
			if (this.ecEFC_FOOTER == null)
			{
				this.ecEFC_FOOTER = this.goEFC_FOOTER.AddComponent<EfcCont>();
			}
			this.vORG_FOOTER = this.goEFC_FOOTER.transform.localPosition;
		}
		if (this.goEFC_RIGHT != null)
		{
			this.ecEFC_RIGHT = this.goEFC_RIGHT.GetComponent<EfcCont>();
			if (this.ecEFC_RIGHT == null)
			{
				this.ecEFC_RIGHT = this.goEFC_RIGHT.AddComponent<EfcCont>();
			}
			this.vORG_RIGHT = this.goEFC_RIGHT.transform.localPosition;
		}
		if (this.goEFC_LEFT != null)
		{
			this.ecEFC_LEFT = this.goEFC_LEFT.GetComponent<EfcCont>();
			if (this.ecEFC_LEFT == null)
			{
				this.ecEFC_LEFT = this.goEFC_LEFT.AddComponent<EfcCont>();
			}
			this.vORG_LEFT = this.goEFC_LEFT.transform.localPosition;
		}
		if (this.goEFC_BG != null)
		{
			foreach (object obj in this.goEFC_BG.transform)
			{
				Transform transform = (Transform)obj;
				this.ecEFC_BG = transform.gameObject.GetComponent<EfcCont>();
				if (this.ecEFC_BG == null)
				{
					this.ecEFC_BG = transform.gameObject.AddComponent<EfcCont>();
				}
			}
		}
		if (this.goEFC_CTR != null)
		{
			foreach (object obj2 in this.goEFC_CTR.transform)
			{
				Transform transform2 = (Transform)obj2;
				this.ecEFC_CTR = transform2.gameObject.GetComponent<EfcCont>();
				if (this.ecEFC_CTR == null)
				{
					this.ecEFC_CTR = transform2.gameObject.AddComponent<EfcCont>();
				}
			}
		}
		if (this.goEFC_PARAM_HEADER != null)
		{
			this.epEFC_PARAM_HEADER = this.goEFC_PARAM_HEADER.GetComponent<EfcParam>();
		}
		if (this.goEFC_PARAM_FOOTER != null)
		{
			this.epEFC_PARAM_FOOTER = this.goEFC_PARAM_FOOTER.GetComponent<EfcParam>();
		}
		if (this.goEFC_PARAM_RIGHT != null)
		{
			this.epEFC_PARAM_RIGHT = this.goEFC_PARAM_RIGHT.GetComponent<EfcParam>();
		}
		if (this.goEFC_PARAM_LEFT != null)
		{
			this.epEFC_PARAM_LEFT = this.goEFC_PARAM_LEFT.GetComponent<EfcParam>();
		}
	}

	private void EFC_FadeIn(bool flg, EfcCont ec, Vector3 vORG, EfcParam param)
	{
		if (ec != null)
		{
			this.actStartEfc();
			if (flg)
			{
				Vector3 pos = vORG + param.vOffset;
				ec.SetPos(pos);
				Vector2 vP = new Vector2(vORG.x, vORG.y);
				ec.MoveTo(vP, param.time, new Action<int>(this.actEndEfc), param.type, param.delay);
			}
			else
			{
				Vector3 pos = vORG + param.vOffsetEnd;
				Vector2 vP = new Vector2(pos.x, pos.y);
				ec.MoveTo(vP, param.time, new Action<int>(this.actEndEfc), param.typeEnd, param.delay);
			}
		}
	}

	public void EFC_HEADER_FadeIn(bool flg)
	{
		if (this.epEFC_PARAM_HEADER != null)
		{
			this.EFC_FadeIn(flg, this.ecEFC_HEADER, this.vORG_HEADER, this.epEFC_PARAM_HEADER);
		}
	}

	public void EFC_FOOTER_FadeIn(bool flg)
	{
		if (this.epEFC_PARAM_FOOTER != null)
		{
			this.EFC_FadeIn(flg, this.ecEFC_FOOTER, this.vORG_FOOTER, this.epEFC_PARAM_FOOTER);
		}
	}

	public void EFC_RIGHT_FadeIn(bool flg)
	{
		if (this.epEFC_PARAM_RIGHT != null)
		{
			this.EFC_FadeIn(flg, this.ecEFC_RIGHT, this.vORG_RIGHT, this.epEFC_PARAM_RIGHT);
		}
	}

	public void EFC_LEFT_FadeIn(bool flg)
	{
		if (this.epEFC_PARAM_LEFT != null)
		{
			this.EFC_FadeIn(flg, this.ecEFC_LEFT, this.vORG_LEFT, this.epEFC_PARAM_LEFT);
		}
	}

	private void EFC_FadeInCol(bool flg, EfcCont ec)
	{
		if (ec != null)
		{
			this.actStartEfc();
			if (flg)
			{
				this.tempCol = ec.GetColor();
				this.tempCol.a = this.ofCol.a;
				ec.SetColor(this.tempCol);
				this.tempCol.a = this.onCol.a;
				ec.ColorTo(this.tempCol, 0.3f, new Action<int>(this.actEndEfc), iTween.EaseType.linear, 0f);
			}
			else
			{
				this.tempCol = ec.GetColor();
				this.tempCol.a = this.ofCol.a;
				ec.ColorTo(this.tempCol, 0.3f, new Action<int>(this.actEndEfc), iTween.EaseType.linear, 0f);
			}
		}
	}

	public void EFC_BG_FadeIn(bool flg)
	{
		this.EFC_FadeInCol(flg, this.ecEFC_BG);
	}

	public void EFC_CTR_FadeIn(bool flg)
	{
		this.EFC_FadeInCol(flg, this.ecEFC_CTR);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.InitEFC();
		this.f_cmdbk = f;
		this.sizeX_cmdbk = sizeX;
		this.sizeY_cmdbk = sizeY;
		this.aT_cmdbk = aT;
		if (this.useCMDAnim)
		{
			CMD parentDialog = this.GetParentDialog(true);
			if (parentDialog != null && parentDialog.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
			{
				this._CMD_Parent = parentDialog;
				this.isParentActing = true;
				AppCoroutine.Start(this.HideParentWithDelay(), false);
				Vector3 localPosition = base.gameObject.transform.localPosition;
				localPosition.x = 8000f;
				base.gameObject.transform.localPosition = localPosition;
			}
			else
			{
				this.BaseShowWithDelay();
			}
		}
		else
		{
			this.BaseShowWithDelay();
		}
	}

	private void BaseShowWithDelay()
	{
		this.isBusyHideDelay = true;
		base.HideDLG();
		this.hideDelayCT_S = 2;
	}

	private void UpdateBaseShowWithDelay()
	{
		if (this.hideDelayCT_S > 0)
		{
			this.hideDelayCT_S--;
			if (this.hideDelayCT_S == 0)
			{
				if (this.useCMDAnim)
				{
					this.EFC_HEADER_FadeIn(true);
					this.EFC_BG_FadeIn(true);
					this.EFC_FOOTER_FadeIn(true);
					this.EFC_RIGHT_FadeIn(true);
					this.EFC_CTR_FadeIn(true);
					this.EFC_LEFT_FadeIn(true);
					this.isActing = true;
				}
				base.Show(this.f_cmdbk, this.sizeX_cmdbk, this.sizeY_cmdbk, this.aT_cmdbk);
				base.ShowDLG();
				this.isBusyHideDelay = false;
			}
		}
	}

	private IEnumerator HideParentWithDelay()
	{
		this.isBusyHideDelay = true;
		this.hideDelayCT = 1;
		while (this.hideDelayCT > 0)
		{
			this.hideDelayCT--;
			yield return null;
		}
		this._CMD_Parent.EFC_FOOTER_FadeIn(false);
		this._CMD_Parent.EFC_RIGHT_FadeIn(false);
		this._CMD_Parent.EFC_CTR_FadeIn(false);
		this._CMD_Parent.EFC_LEFT_FadeIn(false);
		this.isBusyHideDelay = false;
		yield break;
	}

	public override void ClosePanel(bool animation = true)
	{
		this.animation_bk = animation;
		if (this.useCMDAnim)
		{
			if (animation)
			{
				this.isActing = true;
				CMD parentDialog = this.GetParentDialog(false);
				if (!(parentDialog != null))
				{
					this.EFC_HEADER_FadeIn(false);
				}
				this.EFC_BG_FadeIn(false);
				this.EFC_FOOTER_FadeIn(false);
				this.EFC_RIGHT_FadeIn(false);
				this.EFC_CTR_FadeIn(false);
				this.EFC_LEFT_FadeIn(false);
			}
			if (this._CMD_Parent != null && this.IsExistDialog(this._CMD_Parent))
			{
				this._CMD_Parent.gameObject.SetActive(true);
			}
			base.ClosePanel(animation);
		}
		else
		{
			base.ClosePanel(animation);
		}
	}

	public override void ClosePanelNotEndShow(bool animation = true)
	{
		this.animation_bk = animation;
		if (this.useCMDAnim)
		{
			if (animation)
			{
				this.isActing = true;
				CMD parentDialog = this.GetParentDialog(false);
				if (!(parentDialog != null))
				{
					this.EFC_HEADER_FadeIn(false);
				}
				this.EFC_BG_FadeIn(false);
				this.EFC_FOOTER_FadeIn(false);
				this.EFC_RIGHT_FadeIn(false);
				this.EFC_CTR_FadeIn(false);
				this.EFC_LEFT_FadeIn(false);
			}
			if (this._CMD_Parent != null && this.IsExistDialog(this._CMD_Parent))
			{
				this._CMD_Parent.gameObject.SetActive(true);
			}
			base.ClosePanelNotEndShow(animation);
		}
		else
		{
			base.ClosePanelNotEndShow(animation);
		}
	}

	protected override void OnDestroy()
	{
		if (this.useCMDAnim)
		{
			CMD parentDialog = this.GetParentDialog(false);
			if (parentDialog != null)
			{
				if (parentDialog.IsOpened() && this.animation_bk)
				{
					parentDialog.EFC_FOOTER_FadeIn(true);
					parentDialog.EFC_RIGHT_FadeIn(true);
					parentDialog.EFC_CTR_FadeIn(true);
					parentDialog.EFC_LEFT_FadeIn(true);
					parentDialog.isReActing = true;
				}
			}
			base.OnDestroy();
		}
		else
		{
			base.OnDestroy();
		}
		this.ClearActStart();
	}

	private CMD GetParentDialog(bool open)
	{
		float num = 100000f;
		CMD cmd = null;
		Dictionary<string, CommonDialog> dialogDic = GUIManager.GetDialogDic();
		foreach (string key in dialogDic.Keys)
		{
			GameObject gameObject = dialogDic[key].gameObject;
			if (gameObject.transform.localPosition.z < num && this != dialogDic[key])
			{
				CMD cmd2 = (CMD)dialogDic[key];
				if (cmd2 != null && cmd2.useCMDAnim && cmd2.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN && !this.DontLookParent)
				{
					num = gameObject.transform.localPosition.z;
					if (!cmd2.HideFromChild)
					{
						cmd = cmd2;
					}
					else
					{
						cmd = null;
					}
				}
			}
		}
		if (!(cmd != null) || !cmd.useCMDAnim)
		{
			return null;
		}
		if (open)
		{
			return cmd;
		}
		float z = base.GetOriginalPos().z;
		float z2 = cmd.GetOriginalPos().z;
		if (z2 > z)
		{
			return cmd;
		}
		return null;
	}

	private bool IsExistDialog(CommonDialog cd)
	{
		Dictionary<string, CommonDialog> dialogDic = GUIManager.GetDialogDic();
		foreach (string key in dialogDic.Keys)
		{
			if (dialogDic[key] == cd)
			{
				return true;
			}
		}
		return false;
	}

	public void closeAll()
	{
		Dictionary<string, CommonDialog> dialogDic = GUIManager.GetDialogDic();
		foreach (string key in dialogDic.Keys)
		{
			GameObject gameObject = dialogDic[key].gameObject;
			if (this != dialogDic[key])
			{
				CMD cmd = (CMD)dialogDic[key];
				if (cmd != null && cmd.useCMDAnim && cmd.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
				{
					gameObject.SetActive(true);
				}
			}
		}
		GUIManager.CloseAllCommonDialog(null);
	}

	protected override void MakeAnimation(bool open, float atime, iTween.EaseType type = iTween.EaseType.linear)
	{
		if (!this.useCMDAnim)
		{
			base.MakeAnimation(open, atime, type);
		}
		else if (atime <= 0f)
		{
			base.MakeAnimation(open, atime, type);
		}
		else
		{
			base.SetScaleFlg(false);
		}
	}

	private void UpdateScaleFlg()
	{
		if (this.useCMDAnim && this.isActing && this.actCt == 0)
		{
			this.isActing = false;
			base.SetScaleFlg(true);
		}
	}

	private void UpdateWaitParent()
	{
		this.UpdateBaseShowWithDelay();
		if (this.isBusyHideDelay)
		{
			return;
		}
		if (this.isParentActing)
		{
			if (this._CMD_Parent.GetActCount() == 0)
			{
				Vector3 localPosition = base.gameObject.transform.localPosition;
				localPosition.x = 0f;
				base.gameObject.transform.localPosition = localPosition;
				this.isActing = true;
				this.EFC_BG_FadeIn(true);
				this.EFC_FOOTER_FadeIn(true);
				this.EFC_RIGHT_FadeIn(true);
				this.EFC_CTR_FadeIn(true);
				this.EFC_LEFT_FadeIn(true);
				base.Show(this.f_cmdbk, this.sizeX_cmdbk, this.sizeY_cmdbk, this.aT_cmdbk);
				this.isParentActing = false;
				if (this.actCBSetOpendAction != null)
				{
					this.actCBSetOpendAction(0);
					this.actCBSetOpendAction = null;
				}
			}
		}
		else if (this.GetActCount() == 0)
		{
			if (!this.isParentDeactive && this._CMD_Parent != null && this.IsExistDialog(this._CMD_Parent) && !GUICollider.IsAllColliderDisable())
			{
				this._CMD_Parent.gameObject.SetActive(false);
				this.isParentDeactive = true;
			}
			if (this.actCBSetOpendAction != null)
			{
				this.actCBSetOpendAction(0);
				this.actCBSetOpendAction = null;
			}
		}
	}

	private void UpdateReacting()
	{
		if (this.useCMDAnim && this.isReActing && this.actCt == 0)
		{
			this.isReActing = false;
			DepthController.RefreshWidgetDrawCall(base.transform);
			if (this.actCBSetReOpendAction != null)
			{
				bool flag = this.actCBSetReOpendAction(0);
				if (flag)
				{
					this.actCBSetReOpendAction = null;
				}
			}
		}
	}

	public void SetOpendAction(Action<int> act)
	{
		this.actCBSetOpendAction = act;
	}

	public void SetReOpendAction(Func<int, bool> act)
	{
		this.actCBSetReOpendAction = act;
	}
}
