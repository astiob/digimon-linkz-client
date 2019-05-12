using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIMonsterIcon : GUIListPartBS
{
	[SerializeField]
	private GameObject goBASE;

	[SerializeField]
	private List<GameObject> goRARES;

	[SerializeField]
	private GameObject goArousal;

	[SerializeField]
	private UISprite spArousal;

	[SerializeField]
	private GameObject goSELECT_BASE;

	[SerializeField]
	private GameObject goNEW;

	[SerializeField]
	private GameObject goLOCK;

	[SerializeField]
	private UISprite spBASE;

	[SerializeField]
	private UITexture txCHAR;

	[SerializeField]
	private UISprite spFRAME;

	private UISprite spSELECT_BASE;

	[SerializeField]
	private UILabel ngTX_SELECT_BASE;

	[SerializeField]
	private UILabel ngTX_DIMM_MESS;

	[SerializeField]
	private UILabel ngTX_SORT_MESS;

	[SerializeField]
	private UILabel ngTX_LEVEL_MESS;

	[SerializeField]
	private UISprite spPLAYER_ICON;

	[SerializeField]
	private GameObject goGimmick;

	private UISprite spLOCK;

	private bool _lock;

	private MonsterData data;

	private Action<MonsterData> actTouchShort;

	private Action<MonsterData> actTouchLong;

	private Color colAct = new Color(1f, 1f, 1f, 1f);

	private Color colNon = new Color(0.6f, 0.6f, 0.6f, 1f);

	private Color colDis = new Color(0.274509817f, 0.274509817f, 0.274509817f, 1f);

	private GUIMonsterIcon.DIMM_LEVEL dimm_level;

	private static Shader shader;

	private int select_num = -1;

	private string dimm_mess = string.Empty;

	private string sort_mess = string.Empty;

	private string level_mess = string.Empty;

	private Color colSMess = new Color(0f, 1f, 0.196078435f, 0f);

	private bool _new;

	private bool _gimmick;

	private bool isTouchEndFromChild;

	private bool awaked;

	private int originalDepth;

	private Vector2 beganPosition;

	private float touchBeganTime;

	private bool isTouching_mi;

	private bool isLongTouched;

	private bool _LongTouch = true;

	private Color dimmMessColorNormal = new Color(0f, 0.784313738f, 1f, 1f);

	private Color dimmMessEffectColorNormal = new Color(0.274509817f, 0.274509817f, 0.274509817f, 0.784313738f);

	private Color dimmMessColorSortieLimit = new Color(1f, 0f, 0f, 1f);

	private Color dimmMessEffectColorSortieLimit = new Color(0.235294119f, 0.117647059f, 0.117647059f, 0.784313738f);

	public GUIMonsterIcon.DIMM_LEVEL DimmLevel
	{
		get
		{
			return this.dimm_level;
		}
		set
		{
			if (this.dimm_level != value)
			{
				this.dimm_level = value;
				switch (this.dimm_level)
				{
				case GUIMonsterIcon.DIMM_LEVEL.ACTIVE:
					GUIManager.SetColorAll(base.transform, this.colAct);
					break;
				case GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE:
					GUIManager.SetColorAll(base.transform, this.colNon);
					break;
				case GUIMonsterIcon.DIMM_LEVEL.DISABLE:
					GUIManager.SetColorAll(base.transform, this.colDis);
					break;
				}
				if (this.goSELECT_BASE.activeSelf)
				{
					this.spSELECT_BASE.color = this.colAct;
				}
			}
		}
	}

	public int SelectNum
	{
		get
		{
			return this.select_num;
		}
		set
		{
			if (this.select_num != value)
			{
				this.select_num = value;
				if (this.select_num <= -1)
				{
					this.goSELECT_BASE.SetActive(false);
				}
				else
				{
					this.goSELECT_BASE.SetActive(true);
					if (this.select_num == 0)
					{
						this.ngTX_SELECT_BASE.text = StringMaster.GetString("SystemBase");
					}
					else
					{
						this.ngTX_SELECT_BASE.text = this.select_num.ToString();
					}
					this.spSELECT_BASE.color = this.colAct;
				}
			}
		}
	}

	public string DimmMess
	{
		get
		{
			return this.dimm_mess;
		}
		set
		{
			if (this.dimm_mess != value)
			{
				this.dimm_mess = value;
				this.ngTX_DIMM_MESS.text = this.dimm_mess;
				this.ngTX_DIMM_MESS.color = this.dimmMessColorNormal;
				this.ngTX_DIMM_MESS.effectColor = this.dimmMessEffectColorNormal;
			}
		}
	}

	public string SortMess
	{
		get
		{
			return this.sort_mess;
		}
		set
		{
			if (this.sort_mess != value)
			{
				this.sort_mess = value;
				this.ngTX_SORT_MESS.text = this.sort_mess;
			}
		}
	}

	public bool ReadTexByASync { get; set; }

	public void SetSortMessageColor(Color color)
	{
		this.ngTX_SORT_MESS.color = color;
	}

	public string LevelMess
	{
		get
		{
			return this.level_mess;
		}
		set
		{
			if (this.level_mess != value)
			{
				this.level_mess = value;
				this.ngTX_LEVEL_MESS.text = this.level_mess;
			}
		}
	}

	public void SetLevelMessageColor(Color color)
	{
		this.ngTX_LEVEL_MESS.color = color;
	}

	public void SortMessAlpha(float a)
	{
		this.ngTX_SORT_MESS.alpha = a;
	}

	public void LevelMessAlpha(float a)
	{
		Color color = this.ngTX_LEVEL_MESS.color;
		color.a = a;
		this.ngTX_LEVEL_MESS.color = color;
	}

	private void InitLSMess()
	{
		this.ngTX_SORT_MESS.text = string.Empty;
		this.ngTX_LEVEL_MESS.text = string.Empty;
		this.ngTX_SORT_MESS.color = this.colSMess;
	}

	public bool New
	{
		get
		{
			return this._new;
		}
		set
		{
			this._new = value;
			if (this.goNEW != null)
			{
				this.goNEW.SetActive(this._new);
			}
		}
	}

	public bool Lock
	{
		get
		{
			return this._lock;
		}
		set
		{
			this._lock = value;
			if (this.goLOCK != null)
			{
				this.goLOCK.SetActive(this._lock);
				if (this._lock)
				{
					this.spLOCK.color = Color.white;
				}
			}
		}
	}

	public bool Gimmick
	{
		get
		{
			return this._gimmick;
		}
		set
		{
			this._gimmick = value;
			if (this.goGimmick != null)
			{
				this.goGimmick.SetActive(this._gimmick);
			}
		}
	}

	public MonsterData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	public void SetTouchAct_L(Action<MonsterData> act)
	{
		this.actTouchLong = act;
	}

	public void SetTouchAct_S(Action<MonsterData> act)
	{
		this.actTouchShort = act;
	}

	public Action<MonsterData> GetTouchAct_S()
	{
		return this.actTouchShort;
	}

	public void SetCollorAll(Color col)
	{
		GUIManager.SetColorAll(base.transform, col);
	}

	public void SetPlayerIcon(int playerNo)
	{
		string spriteName = "MultiBattle_P" + playerNo;
		this.spPLAYER_ICON.gameObject.SetActive(true);
		this.spPLAYER_ICON.spriteName = spriteName;
	}

	protected override void Awake()
	{
		if (!this.awaked)
		{
			this.awaked = true;
			base.Awake();
			this.actTouchShort = null;
			this.actTouchLong = null;
			this.spSELECT_BASE = this.goSELECT_BASE.GetComponent<UISprite>();
			this.InitLSMess();
			this.spLOCK = this.goLOCK.GetComponent<UISprite>();
			UIWidget component = this.goBASE.GetComponent<UIWidget>();
			this.originalDepth = component.depth;
		}
	}

	private void OnDisable()
	{
	}

	public void ResetDepthToOriginal()
	{
		DepthController component = this.goBASE.GetComponent<DepthController>();
		UIWidget component2 = this.goBASE.GetComponent<UIWidget>();
		component.AddWidgetDepth(base.transform, this.originalDepth - component2.depth);
	}

	public DepthController GetDepthController()
	{
		return this.goBASE.GetComponent<DepthController>();
	}

	public void SetParent()
	{
		base.ShowGUI();
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		int rare_i = int.Parse(this.data.monsterM.rare);
		this.SetThumbnailMonster(this.txCHAR, this.data, this.ReadTexByASync);
		MonsterDataMng.Instance().DispArousal(rare_i, this.goArousal, this.spArousal);
		int growStep = int.Parse(this.data.monsterMG.growStep);
		GUIMonsterIcon.SetThumbnailFrame(this.spBASE, this.spFRAME, (GrowStep)growStep);
		if (!this._gimmick && this.goGimmick != null)
		{
			this.goGimmick.SetActive(false);
		}
	}

	public static Shader GetIconShader()
	{
		if (null == GUIMonsterIcon.shader)
		{
			GUIMonsterIcon.shader = Shader.Find("Unlit With Mask/Alpha Blended");
		}
		return GUIMonsterIcon.shader;
	}

	private void SetThumbnailMonster(UITexture iconTexture, MonsterData iconMonsterData, bool isLoadASync)
	{
		if (string.IsNullOrEmpty(iconMonsterData.monsterM.iconId))
		{
			iconTexture.transform.gameObject.SetActive(false);
		}
		else
		{
			string assetBundlePath = string.Empty;
			string resourcePath = string.Empty;
			iconTexture.transform.gameObject.SetActive(true);
			if (!iconMonsterData.userMonster.IsEgg())
			{
				assetBundlePath = MonsterDataMng.Instance().GetMonsterIconPathByMonsterData(iconMonsterData);
				resourcePath = MonsterDataMng.Instance().InternalGetMonsterIconPathByMonsterData(iconMonsterData);
			}
			else
			{
				int num = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length;
				string iconId = string.Empty;
				for (int i = 0; i < num; i++)
				{
					GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i];
					if (monsterEvolutionRouteM.monsterEvolutionRouteId == iconMonsterData.userMonster.monsterEvolutionRouteId)
					{
						iconId = monsterEvolutionRouteM.eggMonsterId;
						break;
					}
				}
				assetBundlePath = MonsterDataMng.Instance().GetMonsterIconPathByIconId(iconId);
				resourcePath = MonsterDataMng.Instance().InternalGetMonsterIconPathByIconId(iconId);
			}
			GUIMonsterIcon.SetTextureMonsterParts(iconTexture, resourcePath, assetBundlePath, isLoadASync);
		}
	}

	public static void SetTextureMonsterParts(UITexture iconTexture, string resourcePath, string assetBundlePath, bool isLoadASync)
	{
		Texture2D texture2D = Resources.Load(resourcePath) as Texture2D;
		Texture2D texture2D2 = Resources.Load(resourcePath + "_alpha") as Texture2D;
		if (texture2D != null && texture2D2 != null)
		{
			if (iconTexture.material == null)
			{
				Shader iconShader = GUIMonsterIcon.GetIconShader();
				iconTexture.material = new Material(iconShader);
			}
			iconTexture.material.SetTexture("_MaskTex", texture2D2);
			iconTexture.material.SetTexture("_MainTex", texture2D);
			iconTexture.mainTexture = null;
		}
		else if (isLoadASync)
		{
			NGUIUtil.ChangeUITexture(iconTexture, null, false);
			MonsterIconCacheBuffer.Instance().LoadAndCacheObj(assetBundlePath, delegate(UnityEngine.Object obj)
			{
				Texture2D tex2 = obj as Texture2D;
				NGUIUtil.ChangeUITexture(iconTexture, tex2, false);
				if (iconTexture.material != null)
				{
					iconTexture.material.SetTexture("_MaskTex", Texture2D.whiteTexture);
				}
			});
		}
		else
		{
			Texture2D tex = MonsterIconCacheBuffer.Instance().LoadAndCacheObj(assetBundlePath, null) as Texture2D;
			NGUIUtil.ChangeUITexture(iconTexture, tex, false);
			if (iconTexture.material != null)
			{
				iconTexture.material.SetTexture("_MaskTex", Texture2D.whiteTexture);
			}
		}
	}

	public static void SetThumbnailFrame(UISprite background, UISprite frame, GrowStep growStep)
	{
		switch (growStep)
		{
		case GrowStep.EGG:
			background.spriteName = "Common02_Thumbnail_bg1";
			frame.spriteName = "Common02_Thumbnail_waku1";
			break;
		case GrowStep.CHILD_1:
			background.spriteName = "Common02_Thumbnail_bg1";
			frame.spriteName = "Common02_Thumbnail_waku1";
			break;
		case GrowStep.CHILD_2:
			background.spriteName = "Common02_Thumbnail_bg1";
			frame.spriteName = "Common02_Thumbnail_waku1";
			break;
		case GrowStep.GROWING:
			background.spriteName = "Common02_Thumbnail_bg2";
			frame.spriteName = "Common02_Thumbnail_waku2";
			break;
		case GrowStep.RIPE:
			background.spriteName = "Common02_Thumbnail_bg3";
			frame.spriteName = "Common02_Thumbnail_waku3";
			break;
		case GrowStep.PERFECT:
			background.spriteName = "Common02_Thumbnail_bg4";
			frame.spriteName = "Common02_Thumbnail_waku4";
			break;
		case GrowStep.ULTIMATE:
			background.spriteName = "Common02_Thumbnail_bg5";
			frame.spriteName = "Common02_Thumbnail_waku5";
			break;
		case GrowStep.ARMOR_1:
			background.spriteName = "Common02_Thumbnail_bg3";
			frame.spriteName = "Common02_Thumbnail_waku3";
			break;
		case GrowStep.ARMOR_2:
			background.spriteName = "Common02_Thumbnail_bg5";
			frame.spriteName = "Common02_Thumbnail_waku5";
			break;
		default:
			background.spriteName = "Common02_Thumbnail_Question";
			frame.spriteName = "Common02_Thumbnail_wakuQ";
			break;
		}
	}

	public void ResetTex()
	{
		NGUIUtil.ChangeUITexture(this.txCHAR, null, false);
	}

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
		this.isTouchEndFromChild = false;
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
			if (magnitude < 40f && !this.isTouchEndFromChild && this.actTouchShort != null)
			{
				this.actTouchShort(this.data);
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
				this.actTouchLong(this.data);
			}
			base.isTouching = false;
			this.isLongTouched = true;
			this.isTouching_mi = false;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.txCHAR != null && this.txCHAR.material != null)
		{
			this.txCHAR.material = null;
		}
	}

	public void ResizeIcon(int x, int y)
	{
		float x2 = (float)x / (float)this.txCHAR.width;
		float y2 = (float)y / (float)this.txCHAR.height;
		base.gameObject.transform.localScale = new Vector3(x2, y2, 1f);
	}

	public void SetCenterText(string text, GUIMonsterIcon.DimmMessColorType colorType)
	{
		this.dimm_mess = text;
		this.ngTX_DIMM_MESS.text = text;
		if (colorType != GUIMonsterIcon.DimmMessColorType.NORMAL)
		{
			if (colorType == GUIMonsterIcon.DimmMessColorType.SORTIE_LIMIT)
			{
				this.ngTX_DIMM_MESS.color = this.dimmMessColorSortieLimit;
				this.ngTX_DIMM_MESS.effectColor = this.dimmMessEffectColorSortieLimit;
			}
		}
		else
		{
			this.ngTX_DIMM_MESS.color = this.dimmMessColorNormal;
			this.ngTX_DIMM_MESS.effectColor = this.dimmMessEffectColorNormal;
		}
	}

	public void SetGrayout(GUIMonsterIcon.DIMM_LEVEL type)
	{
		this.dimm_level = type;
		switch (type)
		{
		case GUIMonsterIcon.DIMM_LEVEL.ACTIVE:
			GUIManager.SetColorAll(base.transform, this.colAct);
			break;
		case GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE:
			GUIManager.SetColorAll(base.transform, this.colNon);
			break;
		case GUIMonsterIcon.DIMM_LEVEL.DISABLE:
			GUIManager.SetColorAll(base.transform, this.colDis);
			break;
		}
		if (this.goSELECT_BASE.activeSelf)
		{
			this.spSELECT_BASE.color = this.colAct;
		}
	}

	public enum DIMM_LEVEL
	{
		INVALID,
		ACTIVE,
		NOTACTIVE,
		DISABLE
	}

	public enum DimmMessColorType
	{
		NORMAL,
		SORTIE_LIMIT
	}
}
