using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("GUI/Collider")]
public class GUICollider : LocalTween, ITouchEvent
{
	private UISprite _UISprite;

	protected Vector3 originalPos;

	protected GameObject locator;

	public Vector3 originalScale = new Vector3(1f, 1f, 1f);

	[SerializeField]
	private bool activeCollider_ = true;

	public bool playOkSE;

	public bool playCancelSE;

	public bool playSelectSE;

	private Vector3 vTmp3 = new Vector3(1f, 1f, 1f);

	private Animation boundAnime;

	public static AnimationClip boundAnimeClip;

	public GameObject fosterParent;

	public GameObject showObject;

	public GUICollider.TouchBehavior touchBehavior = GUICollider.TouchBehavior.ToLarge;

	public float changeSize = 1.1f;

	public float changeSizeSmall = 0.95f;

	public float changeMove = 16f;

	public Color changeColor = new Color(1f, 0.5f, 0.5f, 1f);

	public int pullDownVal = -1;

	private Color firstColor;

	private Vector3 firstPos;

	private IEnumerator routine;

	private bool enableSizeSmall = true;

	private float distance_;

	private int generation_;

	[SerializeField]
	private bool useSubPhase_;

	[SerializeField]
	private bool removePhase_;

	public bool dontAddToDialogEvent;

	private bool isTouchBegan_;

	private bool isTouchMoved_;

	private bool isTouchEnded_;

	private static int disableAllColliderCT;

	private static bool disableAllCollider;

	private bool avoidDisableAllCollider;

	private int toLargeSeq;

	private int toLargeSeq2;

	private int toSmallSeq;

	protected Vector2 beganPostion;

	public GUICollider.CallBackState callBackState = GUICollider.CallBackState.OnTouchEnded;

	public GameObject CallBackClass;

	public string MethodToInvoke;

	public bool SendMoveToParent;

	public bool CancelTouchEndByMove;

	private bool sendMoveFromChild;

	protected BoxCollider boxCollider;

	public event Action<Touch, Vector2> onTouchBegan;

	public event Action<Touch, Vector2> onTouchMoved;

	public event Action<Touch, Vector2, bool> onTouchEnded;

	public void SetOriginalPos(Vector3 v3)
	{
		this.originalPos = v3;
		base.gameObject.transform.localPosition = v3;
	}

	public Vector3 GetOriginalPos()
	{
		return this.originalPos;
	}

	public bool activeCollider
	{
		get
		{
			return this.activeCollider_;
		}
		set
		{
			this.activeCollider_ = value;
		}
	}

	public void PlaySE()
	{
		if (this.playOkSE)
		{
			this.PlayOkSE();
		}
		if (this.playCancelSE)
		{
			this.PlayCancelSE();
		}
		if (this.playSelectSE)
		{
			this.PlaySelectSE();
		}
	}

	public virtual void PlayOkSE()
	{
		SoundMng.Instance().PlaySE("SEInternal/Common/se_105", 0f, false, true, null, -1, 1f);
	}

	public virtual void PlayCancelSE()
	{
		SoundMng.Instance().PlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1, 1f);
	}

	public virtual void PlaySelectSE()
	{
		SoundMng.Instance().PlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1, 1f);
	}

	public string gName
	{
		get
		{
			return base.gameObject.name;
		}
	}

	public GameObject gObject
	{
		get
		{
			return base.gameObject;
		}
	}

	public float distance
	{
		get
		{
			return this.distance_;
		}
		set
		{
			this.distance_ = value;
		}
	}

	public int generation
	{
		get
		{
			return this.generation_;
		}
		set
		{
			this.generation_ = value;
		}
	}

	public bool useSubPhase
	{
		get
		{
			return this.useSubPhase_;
		}
		set
		{
			this.useSubPhase_ = value;
		}
	}

	public bool removePhase
	{
		get
		{
			return this.removePhase_;
		}
	}

	public void ClearAllEventAction()
	{
		this.onTouchBegan = null;
		this.onTouchMoved = null;
		this.onTouchEnded = null;
	}

	public bool isTouchBegan
	{
		get
		{
			return this.isTouchBegan_;
		}
	}

	public bool isTouchMoved
	{
		get
		{
			return this.isTouchMoved_;
		}
	}

	public bool isTouchEnded
	{
		get
		{
			return this.isTouchEnded_;
		}
	}

	public void OnTouchInit()
	{
		this.isTouchBegan_ = false;
		this.isTouchMoved_ = false;
		this.isTouchEnded_ = false;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
		}
		this.isTouching = false;
	}

	private void OnApplicationQuit()
	{
		this.isTouching = false;
	}

	public bool isTouching { get; set; }

	public Rect boundingRect
	{
		get
		{
			if (this.boxCollider != null)
			{
				float x = this.boxCollider.size.x;
				float y = this.boxCollider.size.y;
				float x2 = -(x / 2f) + this.boxCollider.center.x;
				float y2 = -(y / 2f) + this.boxCollider.center.y;
				return new Rect(x2, y2, x, y);
			}
			return new Rect(0f, 0f, 0f, 0f);
		}
	}

	public float width
	{
		get
		{
			if (this.boxCollider != null)
			{
				return this.boxCollider.size.x;
			}
			return 0f;
		}
		set
		{
			if (this._UISprite != null)
			{
				this._UISprite.width = (int)value;
				this._UISprite.MakePixelPerfect();
			}
			if (this.boxCollider != null)
			{
				this.boxCollider.size = new Vector2(value, this.boxCollider.size.y);
			}
		}
	}

	public float height
	{
		get
		{
			if (this.boxCollider != null)
			{
				return this.boxCollider.size.y;
			}
			return 0f;
		}
		set
		{
			if (this._UISprite != null)
			{
				this._UISprite.height = (int)value;
				this._UISprite.MakePixelPerfect();
			}
			if (this.boxCollider != null)
			{
				this.boxCollider.size = new Vector2(this.boxCollider.size.x, value);
			}
		}
	}

	public void ChangeColor()
	{
		if (this._UISprite != null)
		{
			this._UISprite.color = this.changeColor;
		}
		else
		{
			MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.material.color = this.changeColor;
			}
		}
	}

	public static void InitAllCollider()
	{
		GUICollider.disableAllCollider = false;
		GUICollider.disableAllColliderCT = 0;
	}

	public static void DisableAllCollider(string name)
	{
		if (GUICollider.disableAllColliderCT == 0)
		{
			GUICollider.disableAllCollider = true;
		}
		GUICollider.disableAllColliderCT++;
	}

	public static void EnableAllCollider(string name)
	{
		GUICollider.disableAllColliderCT--;
		if (GUICollider.disableAllColliderCT < 0)
		{
			GUICollider.disableAllColliderCT = 0;
			GUICollider.disableAllCollider = false;
		}
		else if (GUICollider.disableAllColliderCT == 0)
		{
			GUICollider.disableAllCollider = false;
		}
	}

	public static bool IsAllColliderDisable()
	{
		return GUICollider.disableAllCollider;
	}

	public static int GetDisableAllColliderCT()
	{
		return GUICollider.disableAllColliderCT;
	}

	public bool AvoidDisableAllCollider
	{
		get
		{
			return this.avoidDisableAllCollider;
		}
		set
		{
			this.avoidDisableAllCollider = value;
		}
	}

	private void TouchBehaviourScaleEnd(int idx)
	{
		if (idx == 0)
		{
			if (this.toLargeSeq2 == 3)
			{
				this.TouchBehaviourScaleEnd2(4);
			}
			else
			{
				this.vTmp3.x = this.changeSize;
				this.vTmp3.y = this.changeSize;
				base.KickTween(LocalTween.TWEEN_TYPE.LOCAL_SCALE, this.vTmp3, 0.16f, new Action<int>(this.TouchBehaviourScaleEnd2), 0);
				this.toLargeSeq = 1;
			}
		}
	}

	private void TouchBehaviourScaleEnd2(int idx)
	{
		if (idx == 4 || (idx == 0 && this.toLargeSeq2 == 3))
		{
			this.vTmp3.x = this.originalScale.x;
			this.vTmp3.y = this.originalScale.y;
			base.KickTween(LocalTween.TWEEN_TYPE.LOCAL_SCALE, this.vTmp3, 0.1f, new Action<int>(this.TouchBehaviourScaleEnd2), 1);
		}
	}

	private void SmallScaleEnd(int idx)
	{
		this.toSmallSeq = 1;
	}

	public virtual void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.disableAllCollider && !this.avoidDisableAllCollider)
		{
			return;
		}
		if (!this.activeCollider)
		{
			return;
		}
		this.isTouching = true;
		if (this.SendMoveToParent && base.transform.parent != null)
		{
			GUICollider component = base.transform.parent.GetComponent<GUICollider>();
			if (component != null)
			{
				component.SendMoveFromChild = true;
				component.OnTouchBegan(touch, pos);
			}
		}
		if (this.sendMoveFromChild)
		{
			this.sendMoveFromChild = false;
			return;
		}
		this.isTouchBegan_ = true;
		this.beganPostion = pos;
		this.firstPos = base.gameObject.transform.localPosition;
		switch (this.touchBehavior)
		{
		case GUICollider.TouchBehavior.ToLarge:
			this.vTmp3.x = this.changeSize * 1.1f;
			this.vTmp3.y = this.changeSize * 1.1f;
			base.KickTween(LocalTween.TWEEN_TYPE.LOCAL_SCALE, this.vTmp3, 0.16f, new Action<int>(this.TouchBehaviourScaleEnd), 0);
			this.toLargeSeq = 0;
			this.toLargeSeq2 = 0;
			break;
		case GUICollider.TouchBehavior.ToSmall:
			this.vTmp3.x = this.changeSizeSmall * this.originalScale.x;
			this.vTmp3.y = this.changeSizeSmall * this.originalScale.y;
			base.KickTween(LocalTween.TWEEN_TYPE.LOCAL_SCALE, this.vTmp3, 0.1f, new Action<int>(this.SmallScaleEnd), 0);
			this.toSmallSeq = 0;
			break;
		case GUICollider.TouchBehavior.MoveUp:
			base.gameObject.transform.position = new Vector3(this.firstPos.x, this.firstPos.y + this.changeMove, this.firstPos.z - 50f);
			break;
		case GUICollider.TouchBehavior.ChangeColor:
			if (this._UISprite != null)
			{
				this._UISprite.color = this.changeColor;
			}
			else
			{
				MeshRenderer component2 = base.gameObject.GetComponent<MeshRenderer>();
				if (component2 != null)
				{
					component2.material.color = this.changeColor;
				}
			}
			break;
		case GUICollider.TouchBehavior.BoundAnime:
			this.boundAnime.wrapMode = WrapMode.Loop;
			this.boundAnime.Play(GUICollider.boundAnimeClip.name);
			break;
		}
		if (this.callBackState == GUICollider.CallBackState.OnTouchBegan)
		{
			this.PlaySE();
		}
		if (this.onTouchBegan != null)
		{
			this.onTouchBegan(touch, pos);
		}
		if (this.callBackState == GUICollider.CallBackState.OnTouchBegan && this.CallBackClass != null && this.MethodToInvoke != null && this.MethodToInvoke != string.Empty)
		{
			this.CallBackClass.SendMessage(this.MethodToInvoke, this, SendMessageOptions.DontRequireReceiver);
		}
	}

	public virtual void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.disableAllCollider && !this.avoidDisableAllCollider)
		{
			return;
		}
		if (!this.activeCollider)
		{
			return;
		}
		this.isTouchMoved_ = true;
		if (this.callBackState == GUICollider.CallBackState.OnTouchMoved)
		{
			this.PlaySE();
		}
		if (this.onTouchMoved != null)
		{
			this.onTouchMoved(touch, pos);
		}
		if (this.callBackState == GUICollider.CallBackState.OnTouchMoved && this.CallBackClass != null && this.MethodToInvoke != null && this.MethodToInvoke != string.Empty)
		{
			this.CallBackClass.SendMessage(this.MethodToInvoke, this, SendMessageOptions.DontRequireReceiver);
		}
		if (this.SendMoveToParent && base.transform.parent != null)
		{
			GUICollider component = base.transform.parent.GetComponent<GUICollider>();
			if (component != null)
			{
				component.OnTouchMoved(touch, pos);
			}
		}
	}

	public virtual void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		try
		{
			if (base.gameObject == null)
			{
				return;
			}
		}
		catch
		{
			return;
		}
		if (GUICollider.disableAllCollider && !this.avoidDisableAllCollider)
		{
			this.isTouching = false;
			return;
		}
		if (!this.activeCollider)
		{
			this.isTouching = false;
			return;
		}
		if (!this.enableSizeSmall)
		{
			return;
		}
		Vector3 localPosition = base.transform.localPosition;
		base.gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, this.firstPos.z);
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		switch (this.touchBehavior)
		{
		case GUICollider.TouchBehavior.ToLarge:
			if (this.toLargeSeq > 0)
			{
				this.TouchBehaviourScaleEnd2(4);
				this.toLargeSeq2 = 2;
			}
			else
			{
				this.toLargeSeq2 = 3;
			}
			break;
		case GUICollider.TouchBehavior.ToSmall:
			if (this.toSmallSeq == 1)
			{
				this.vTmp3.x = this.originalScale.x;
				this.vTmp3.y = this.originalScale.y;
				base.KickTween(LocalTween.TWEEN_TYPE.LOCAL_SCALE, this.vTmp3, 0.1f, null, 0);
			}
			else
			{
				this.enableSizeSmall = false;
				GUICollider.DisableAllCollider("GUICollider:WaitForSmallScaleEnd Start");
				if (!(base.gameObject == null))
				{
					if (!base.gameObject.activeSelf)
					{
						base.gameObject.SetActive(true);
					}
					this.routine = this.WaitForSmallScaleEnd(touch, pos, flag);
					AppCoroutine.Start(this.routine, true);
					return;
				}
			}
			break;
		case GUICollider.TouchBehavior.MoveUp:
			base.gameObject.transform.localPosition = new Vector3(this.firstPos.x, this.firstPos.y, this.firstPos.z);
			break;
		case GUICollider.TouchBehavior.ChangeColor:
			if (this._UISprite != null)
			{
				this._UISprite.color = this.firstColor;
			}
			else if (component != null)
			{
				component.material.color = this.firstColor;
			}
			break;
		case GUICollider.TouchBehavior.BoundAnime:
			this.boundAnime.wrapMode = WrapMode.Once;
			break;
		}
		this.TouchEndMethod(touch, pos, flag);
	}

	private void TouchEndMethod(Touch touch, Vector2 pos, bool flag)
	{
		this.isTouching = false;
		if (this.CancelTouchEndByMove)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude > 40f)
			{
				return;
			}
		}
		if (this.onTouchEnded != null)
		{
			this.onTouchEnded(touch, pos, flag);
		}
		if (flag)
		{
			if (this.callBackState == GUICollider.CallBackState.OnTouchEnded)
			{
				this.PlaySE();
			}
			this.isTouchEnded_ = true;
			if (this.callBackState == GUICollider.CallBackState.OnTouchEnded && this.CallBackClass != null && this.MethodToInvoke != null && this.MethodToInvoke != string.Empty)
			{
				this.CallBackClass.SendMessage(this.MethodToInvoke, this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private IEnumerator WaitForSmallScaleEnd(Touch touch, Vector2 pos, bool flag)
	{
		while (this.toSmallSeq != 1)
		{
			yield return null;
		}
		this.vTmp3.x = this.originalScale.x;
		this.vTmp3.y = this.originalScale.y;
		base.KickTween(LocalTween.TWEEN_TYPE.LOCAL_SCALE, this.vTmp3, 0.1f, null, 0);
		this.TouchEndMethod(touch, pos, flag);
		this.enableSizeSmall = true;
		GUICollider.EnableAllCollider("GUICollider:WaitForSmallScaleEnd End");
		yield break;
	}

	protected bool SendMoveFromChild
	{
		get
		{
			return this.sendMoveFromChild;
		}
		set
		{
			this.sendMoveFromChild = value;
		}
	}

	protected virtual void Awake()
	{
		GUICollider[] components = base.gameObject.GetComponents<GUICollider>();
		if (components.Length > 1)
		{
			global::Debug.LogError(base.gameObject.name + "=multi bind GUICollider!!");
		}
		this.boxCollider = base.gameObject.GetComponent<BoxCollider>();
		this.originalPos = base.gameObject.transform.localPosition;
		GUIManager.AddCollider(this);
		this._UISprite = base.gameObject.GetComponent<UISprite>();
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		if (this._UISprite != null)
		{
			this.firstColor = this._UISprite.color;
		}
		else if (component != null)
		{
			this.firstColor = component.material.color;
		}
		if (this.touchBehavior == GUICollider.TouchBehavior.BoundAnime && null == this.boundAnime)
		{
			this.boundAnime = base.gameObject.GetComponent<Animation>();
			if (null == this.boundAnime)
			{
				this.boundAnime = base.gameObject.AddComponent<Animation>();
			}
			this.boundAnime.playAutomatically = false;
			if (null == GUICollider.boundAnimeClip)
			{
				GUICollider.boundAnimeClip = (Resources.Load("CommonUI/Animation/ButtonTest") as AnimationClip);
			}
			this.boundAnime.AddClip(GUICollider.boundAnimeClip, GUICollider.boundAnimeClip.name);
		}
		this.originalScale.x = base.gameObject.transform.localScale.x;
		this.originalScale.y = base.gameObject.transform.localScale.y;
		this.originalScale.z = base.gameObject.transform.localScale.z;
	}

	protected virtual void OnDestroy()
	{
		if (!this.enableSizeSmall)
		{
			this.enableSizeSmall = true;
			GUICollider.EnableAllCollider("GUICollider:WaitForSmallScaleEnd End");
		}
		GUIManager.DeleteCollider(this);
	}

	protected virtual void Update()
	{
	}

	public virtual void ShowGUI()
	{
		base.transform.localPosition = this.originalPos;
	}

	public virtual void HideGUI()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, 10000f, base.transform.localPosition.z);
	}

	private void OnEnable()
	{
		if (this.routine != null)
		{
			AppCoroutine.Start(this.routine, true);
		}
	}

	public virtual void ResetTouchStatus()
	{
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		switch (this.touchBehavior)
		{
		case GUICollider.TouchBehavior.ToLarge:
			base.gameObject.transform.localScale = new Vector3(this.originalScale.x, this.originalScale.y, 1f);
			break;
		case GUICollider.TouchBehavior.MoveUp:
			base.gameObject.transform.localPosition = new Vector3(this.firstPos.x, this.firstPos.y, this.firstPos.z);
			break;
		case GUICollider.TouchBehavior.ChangeColor:
			if (this._UISprite != null)
			{
				this._UISprite.color = this.firstColor;
			}
			else if (component != null)
			{
				component.material.color = this.firstColor;
			}
			break;
		case GUICollider.TouchBehavior.BoundAnime:
			this.boundAnime.wrapMode = WrapMode.Once;
			break;
		}
		this.OnTouchInit();
	}

	public enum TouchBehavior
	{
		None,
		ToLarge,
		ToSmall,
		MoveUp,
		ChangeColor,
		BoundAnime
	}

	public enum CallBackState
	{
		OnTouchBegan,
		OnTouchMoved,
		OnTouchEnded
	}
}
