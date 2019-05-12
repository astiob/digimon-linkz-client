using System;
using UnityEngine;

public class GUIListPartBS : GUICollider
{
	public bool isUpdate;

	public bool panelEnable = true;

	public GUICollider parent;

	private int _idx;

	protected bool useOnDisable = true;

	private Vector2 oldPos;

	[SerializeField]
	private EfcParam efcParam;

	private EfcCont ecc;

	private int actCT;

	private Color onCol = new Color(1f, 1f, 1f, 1f);

	private Color ofCol = new Color(1f, 1f, 1f, 0f);

	private Color ofEndCol = new Color(1f, 1f, 1f, 0f);

	private float DOUBLE_LIST_RIGHT_TRETHOLD = 200f;

	private float delayAdd_reserve;

	private Action<int> actEndCallBack;

	private bool efcIn = true;

	private Action actCBFadeInEnd;

	public bool UseOnDisable
	{
		set
		{
			this.useOnDisable = value;
		}
	}

	public int IDX
	{
		get
		{
			return this._idx;
		}
		set
		{
			this._idx = value;
		}
	}

	public CMD GetInstanceCMD()
	{
		GUISelectPanelViewPartsUD guiselectPanelViewPartsUD = (GUISelectPanelViewPartsUD)this.parent;
		if (guiselectPanelViewPartsUD != null)
		{
			CMD instanceCMD = guiselectPanelViewPartsUD.InstanceCMD;
			if (instanceCMD == null)
			{
				global::Debug.LogError("========================================== ベース class GUISelectPanelViewPartsUD.AllBuild() で、instCMD : this を指定してください！");
			}
			return instanceCMD;
		}
		global::Debug.LogError("========================================== ベース class GUISelectPanelViewPartsUD を使っていないのであれば GetInstanceCMD() は使わないでください！");
		return null;
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateEfc();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.actCT == 1)
		{
			GUICollider.EnableAllCollider("ListPartsBS : " + base.gameObject.name);
			this.actCT = 0;
			this.SetCT2Parent(-1);
			if (this.actEndCallBack != null)
			{
				this.actEndCallBack(0);
				this.actEndCallBack = null;
			}
		}
	}

	public override void ShowGUI()
	{
		if (!this.isUpdate && this.parent == null)
		{
			this.parent = base.gameObject.transform.parent.gameObject.GetComponent<GUICollider>();
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable() && !base.AvoidDisableAllCollider)
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		if (this.panelEnable)
		{
			if (this.parent != null)
			{
				this.parent.OnTouchBegan(touch, pos);
			}
			base.OnTouchBegan(touch, pos);
		}
		this.oldPos = pos;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable() && !base.AvoidDisableAllCollider)
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		if (this.panelEnable)
		{
			if (this.parent != null)
			{
				this.parent.OnTouchMoved(touch, pos);
			}
			base.OnTouchMoved(touch, pos);
		}
	}

	protected bool TouchDistance(Vector2 pos)
	{
		float magnitude = (this.oldPos - pos).magnitude;
		return magnitude <= 40f;
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable() && !base.AvoidDisableAllCollider)
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		flag = (this.TouchDistance(pos) && flag);
		if (this.panelEnable)
		{
			if (this.parent != null)
			{
				this.parent.OnTouchEnded(touch, pos, flag);
			}
			base.OnTouchEnded(touch, pos, flag);
		}
	}

	public virtual void UpdateShowCard()
	{
	}

	public void InitEfc()
	{
		if (this.efcParam != null)
		{
			if (this.ecc == null)
			{
				this.ecc = base.gameObject.AddComponent<EfcCont>();
			}
			float delayTime = this.GetDelayTime(true);
			if (delayTime >= -10f)
			{
				this.delayAdd_reserve = delayTime;
				Vector3 vector = base.gameObject.transform.localPosition;
				Vector3 vOffset = this.efcParam.vOffset;
				if (this.efcParam.isDoubleList && vector.x > this.DOUBLE_LIST_RIGHT_TRETHOLD)
				{
					vOffset.x = -vOffset.x;
				}
				vector += vOffset;
				base.gameObject.transform.localPosition = vector;
				if (this.efcParam.vLP_StartScale != Vector3.one)
				{
					this.ecc.SetScale(this.efcParam.vLP_StartScale);
				}
				this.efcIn = false;
				this.SetCT2Parent(1);
			}
		}
	}

	public bool IsFadeDo()
	{
		return !this.efcIn;
	}

	public int GetActCT()
	{
		return this.actCT;
	}

	private void SetCT2Parent(int add)
	{
		if (base.transform.parent != null)
		{
			GUISelectPanelBSUD component = base.transform.parent.GetComponent<GUISelectPanelBSUD>();
			if (component != null)
			{
				int num = component.StartFadeEfcCT;
				num += add;
				component.StartFadeEfcCT = num;
			}
			else
			{
				GUISelectPanelViewUD component2 = base.transform.parent.GetComponent<GUISelectPanelViewUD>();
				if (component2 != null)
				{
					int num2 = component2.StartFadeEfcCT;
					num2 += add;
					component2.StartFadeEfcCT = num2;
				}
			}
		}
	}

	public void FadeInEfc(Action<int> act = null)
	{
		this.actEndCallBack = act;
		this.actCT = 1;
		GUICollider.DisableAllCollider("ListPartsBS : " + base.gameObject.name);
		if (this.efcParam != null && this.ecc != null)
		{
			float delayTime;
			if (!this.efcIn)
			{
				delayTime = this.delayAdd_reserve;
			}
			else
			{
				this.SetCT2Parent(1);
				delayTime = this.GetDelayTime(true);
			}
			if (delayTime < -10f)
			{
				this.actEndList(0);
				return;
			}
			Vector2 vector = new Vector2(base.GetOriginalPos().x, base.GetOriginalPos().y);
			this.ecc.MoveTo(vector, this.efcParam.time, new Action<int>(this.actEndList), this.efcParam.type, this.efcParam.delay + delayTime);
			if (this.efcParam.vLP_StartScale != Vector3.one)
			{
				this.ecc.SetScale(this.efcParam.vLP_StartScale);
				vector = new Vector2(1f, 1f);
				this.ecc.ScaleTo(vector, this.efcParam.time, null, this.efcParam.scaleType, this.efcParam.delay + delayTime);
			}
			this.ecc.SetColor(this.ofCol);
			this.ecc.ColorTo(this.onCol, this.efcParam.time, null, iTween.EaseType.linear, 0f);
		}
		else
		{
			this.actEndList(0);
		}
	}

	public void FadeOutEfc(Action<int> act = null, bool immediate = false)
	{
		this.actEndCallBack = act;
		this.actCT = 1;
		GUICollider.DisableAllCollider("ListPartsBS : " + base.gameObject.name);
		if (this.efcParam != null && this.ecc != null)
		{
			this.SetCT2Parent(1);
			float delayTime = this.GetDelayTime(false);
			if (delayTime < -10f)
			{
				this.actEndList(0);
				return;
			}
			Vector3 a = base.GetOriginalPos();
			Vector3 vOffsetEnd = this.efcParam.vOffsetEnd;
			if (this.efcParam.isDoubleList && a.x > this.DOUBLE_LIST_RIGHT_TRETHOLD)
			{
				vOffsetEnd.x = -vOffsetEnd.x;
			}
			a += vOffsetEnd;
			Vector2 vector = new Vector2(a.x, a.y);
			if (!immediate)
			{
				this.ecc.MoveTo(vector, this.efcParam.time, new Action<int>(this.actEndList), this.efcParam.typeEnd, this.efcParam.delay + delayTime);
				if (this.efcParam.vLP_EndScale != Vector3.one)
				{
					vector = new Vector2(this.efcParam.vLP_EndScale.x, this.efcParam.vLP_EndScale.y);
					this.ecc.ScaleTo(vector, this.efcParam.time, null, this.efcParam.scaleTypeEnd, this.efcParam.delay + delayTime);
				}
				this.ofEndCol.a = this.efcParam.endAlpha;
				this.ecc.ColorTo(this.ofEndCol, this.efcParam.time, null, iTween.EaseType.linear, 0f);
			}
			else
			{
				this.ecc.SetPosX(vector.x);
				this.ecc.SetPosY(vector.y);
				if (this.efcParam.vLP_EndScale != Vector3.one)
				{
					this.ecc.SetScale(this.efcParam.vLP_EndScale);
				}
				this.ofEndCol.a = this.efcParam.endAlpha;
				this.ecc.SetColor(this.ofEndCol);
				this.actEndList(0);
			}
		}
		else
		{
			this.actEndList(0);
		}
	}

	private void actEndList(int i)
	{
		this.actCT = 0;
		this.SetCT2Parent(-1);
		GUICollider.EnableAllCollider("ListPartsBS : " + base.gameObject.name);
		if (this.actEndCallBack != null)
		{
			this.actEndCallBack(0);
			this.actEndCallBack = null;
		}
		this.FadeInEnd(0);
	}

	private void UpdateEfc()
	{
		if (this.efcParam != null && this.ecc != null)
		{
			if (!this.efcIn)
			{
				this.FadeInEfc(new Action<int>(this.FadeInEnd));
				this.efcIn = true;
			}
		}
	}

	private float GetDelayTime(bool isFadeIn = true)
	{
		Camera orthoCamera = GUIMain.GetOrthoCamera();
		Vector3 vector = orthoCamera.WorldToScreenPoint(base.gameObject.transform.position);
		GUISelectPanelBSUD guiselectPanelBSUD = null;
		if (base.transform.parent != null)
		{
			guiselectPanelBSUD = base.transform.parent.gameObject.GetComponent<GUISelectPanelBSUD>();
		}
		if (guiselectPanelBSUD == null)
		{
			float num = (float)orthoCamera.pixelHeight - vector.y;
			if (num < -(base.height / 2f))
			{
				return -800f;
			}
			if (num > (float)orthoCamera.pixelHeight + base.height / 2f)
			{
				return -800f;
			}
			num += base.height / 2f;
			return num / this.efcParam.lengPerSec;
		}
		else
		{
			float num2 = guiselectPanelBSUD.ListWindowViewRect.yMin - base.height / 2f - 40f;
			float num3 = guiselectPanelBSUD.ListWindowViewRect.yMax + base.height / 2f + 40f;
			float num4 = guiselectPanelBSUD.gameObject.transform.localPosition.y + base.gameObject.transform.localPosition.y;
			if (num4 > num3)
			{
				return -800f;
			}
			if (num4 < num2)
			{
				return -800f;
			}
			bool flag;
			if (isFadeIn)
			{
				flag = this.efcParam.isTopDelayStart;
			}
			else
			{
				flag = this.efcParam.isTopDelayEnd;
			}
			float num5;
			if (flag)
			{
				num5 = num3 - num4;
			}
			else
			{
				num5 = num4 - num2;
			}
			return num5 / this.efcParam.lengPerSec;
		}
	}

	public void SetFadeInEndCallBack(Action act)
	{
		this.actCBFadeInEnd = act;
	}

	public virtual void FadeInEnd(int i)
	{
		if (this.actCBFadeInEnd != null)
		{
			this.actCBFadeInEnd();
			this.actCBFadeInEnd = null;
		}
	}

	public virtual void SetData()
	{
	}

	public virtual void InitParts()
	{
	}

	public virtual void RefreshParts()
	{
	}

	public virtual void InactiveParts()
	{
	}
}
