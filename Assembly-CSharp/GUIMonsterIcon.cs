using Master;
using Monster;
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

	private static GameObject goMONSTER_ICON_BAK_ROOT;

	private static GameObject goMONSTER_ICON_M;

	private readonly Color LEVEL_NORMAL_COLOR = new Color(1f, 1f, 1f, 1f);

	private readonly Color MAX_LEVEL_COLOR = new Color(1f, 0.9411765f, 0f, 1f);

	private readonly Color NONE_LEVEL_COLOR = new Color(1f, 1f, 1f, 1f);

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
			if (this.goNEW != null && this.goNEW.activeSelf != this._new)
			{
				this.goNEW.SetActive(this._new);
			}
		}
	}

	public bool Lock
	{
		set
		{
			if (this.goLOCK != null && this.goLOCK.activeSelf != value)
			{
				this.goLOCK.SetActive(value);
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
		this.SetThumbnailMonster(this.txCHAR, this.data, this.ReadTexByASync);
		GUIMonsterIcon.DispArousal(this.data.monsterM.rare, this.goArousal, this.spArousal);
		int growStep = int.Parse(this.data.monsterMG.growStep);
		GUIMonsterIcon.SetThumbnailFrame(this.spBASE, this.spFRAME, growStep);
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
				assetBundlePath = GUIMonsterIcon.GetMonsterIconPathByIconId(iconMonsterData.monsterM.iconId);
				resourcePath = GUIMonsterIcon.InternalGetMonsterIconPathByIconId(iconMonsterData.monsterM.iconId);
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
				assetBundlePath = GUIMonsterIcon.GetMonsterIconPathByIconId(iconId);
				resourcePath = GUIMonsterIcon.InternalGetMonsterIconPathByIconId(iconId);
			}
			GUIMonsterIcon.SetTextureMonsterParts(iconTexture, resourcePath, assetBundlePath, isLoadASync);
		}
	}

	public static void SetTextureMonsterParts(UITexture iconTexture, string resourcePath, string assetBundlePath, bool isLoadASync)
	{
		bool flag = false;
		string localizedPath = AssetDataMng.GetLocalizedPath(resourcePath);
		Texture2D texture2D = Resources.Load(localizedPath) as Texture2D;
		Texture2D texture2D2 = Resources.Load(localizedPath + "_alpha") as Texture2D;
		Texture2D texture2D3 = (!(texture2D != null)) ? (Resources.Load(resourcePath) as Texture2D) : texture2D;
		Texture2D texture2D4 = (!(texture2D2 != null)) ? (Resources.Load(resourcePath + "_alpha") as Texture2D) : texture2D2;
		if (texture2D == null && texture2D2 == null)
		{
			flag = AssetDataMng.Instance().IsIncludedInAssetBundle(AssetDataMng.GetLocalizedPath(assetBundlePath));
		}
		if (null != texture2D3 && null != texture2D4 && !flag)
		{
			if (null == iconTexture.material)
			{
				Shader iconShader = GUIMonsterIcon.GetIconShader();
				iconTexture.material = new Material(iconShader);
			}
			iconTexture.material.SetTexture("_MaskTex", texture2D4);
			iconTexture.material.SetTexture("_MainTex", texture2D3);
			iconTexture.mainTexture = null;
		}
		else if (isLoadASync)
		{
			NGUIUtil.ChangeUITexture(iconTexture, null, false);
			MonsterIconCacheBuffer.Instance().LoadAndCacheObj(assetBundlePath, delegate(UnityEngine.Object obj)
			{
				Texture2D tex = obj as Texture2D;
				NGUIUtil.ChangeUITexture(iconTexture, tex, false);
				if (null != iconTexture.material)
				{
					iconTexture.material.SetTexture("_MaskTex", Texture2D.whiteTexture);
				}
			});
		}
		else
		{
			Texture2D texture2D5 = MonsterIconCacheBuffer.Instance().LoadAndCacheObj(assetBundlePath, null) as Texture2D;
			if (null != texture2D5)
			{
				NGUIUtil.ChangeUITexture(iconTexture, texture2D5, false);
				if (null != iconTexture.material)
				{
					iconTexture.material.SetTexture("_MaskTex", Texture2D.whiteTexture);
				}
			}
		}
	}

	public static void SetThumbnailFrame(UISprite background, UISprite frame, int growStep)
	{
		if (MonsterGrowStepData.IsEggScope(growStep) || MonsterGrowStepData.IsChild1Scope(growStep) || MonsterGrowStepData.IsChild2Scope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg1";
			frame.spriteName = "Common02_Thumbnail_waku1";
		}
		else if (MonsterGrowStepData.IsGrowingScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg2";
			frame.spriteName = "Common02_Thumbnail_waku2";
		}
		else if (MonsterGrowStepData.IsRipeScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg3";
			frame.spriteName = "Common02_Thumbnail_waku3";
		}
		else if (MonsterGrowStepData.IsPerfectScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg4";
			frame.spriteName = "Common02_Thumbnail_waku4";
		}
		else if (MonsterGrowStepData.IsUltimateScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg5";
			frame.spriteName = "Common02_Thumbnail_waku5";
		}
		else
		{
			background.spriteName = "Common02_Thumbnail_Question";
			frame.spriteName = "Common02_Thumbnail_wakuQ";
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
		if (this.dimm_level != type)
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
			if (null != this.spLOCK)
			{
				this.spLOCK.color = Color.white;
			}
			if (this.goSELECT_BASE.activeSelf)
			{
				this.spSELECT_BASE.color = this.colAct;
			}
		}
	}

	public static void InitMonsterGO(Transform parent)
	{
		if (GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT != null)
		{
			UnityEngine.Object.Destroy(GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT);
		}
		GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT = new GameObject();
		GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT.name = "MONSTER_ICON_BAK_ROOT";
		GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT.transform.parent = parent;
		GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT.transform.localScale = new Vector3(1f, 1f, 1f);
		GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT.transform.localPosition = new Vector3(2000f, 2000f, 0f);
		GUIMonsterIcon.goMONSTER_ICON_M = GUIManager.LoadCommonGUI("ListParts/ListPartsThumbnail", GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT);
		GUIMonsterIcon.goMONSTER_ICON_M.transform.localScale = new Vector3(1f, 1f, 1f);
		GUIMonsterIcon.goMONSTER_ICON_M.SetActive(false);
	}

	public void PushBack(float offsetX, float offsetY)
	{
		Transform transform = base.transform;
		transform.parent = GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT.transform;
		transform.localScale = Vector3.one;
		transform.localPosition = new Vector3(offsetX, offsetY, -10f);
		this.ResetDepthToOriginal();
		base.gameObject.SetActive(false);
	}

	public void RefreshPrefabByMonsterData(MonsterData monsterData)
	{
		this.ReadTexByASync = true;
		this.data = monsterData;
		this.ShowGUI();
		this.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
		this.SelectNum = -1;
		this.DimmMess = string.Empty;
		this.SortMess = string.Empty;
		if (monsterData != null && monsterData.userMonster != null)
		{
			this.Lock = monsterData.userMonster.IsLocked;
		}
	}

	public void SetMessageLevel()
	{
		MonsterData monsterData = this.Data;
		if (monsterData.userMonster.IsEgg())
		{
			this.LevelMess = string.Format(StringMaster.GetString("CharaIconLv"), StringMaster.GetString("CharaStatus-01"));
			this.SetLevelMessageColor(this.NONE_LEVEL_COLOR);
		}
		else
		{
			int num = int.Parse(monsterData.userMonster.level);
			int num2 = int.Parse(monsterData.monsterM.maxLevel);
			if (num >= num2)
			{
				this.LevelMess = string.Format(StringMaster.GetString("CharaIconLv"), StringMaster.GetString("CharaStatus-18"));
				this.SetLevelMessageColor(this.MAX_LEVEL_COLOR);
			}
			else
			{
				this.LevelMess = string.Format(StringMaster.GetString("CharaIconLv"), monsterData.userMonster.level);
				this.SetLevelMessageColor(this.LEVEL_NORMAL_COLOR);
			}
		}
	}

	public void SetMonsterSortMessage(string value)
	{
		MonsterData monsterData = this.Data;
		if (monsterData.userMonster.IsEgg())
		{
			this.SortMess = StringMaster.GetString("CharaStatus-01");
		}
		else
		{
			this.SortMess = value;
		}
	}

	public static GUIMonsterIcon MakePrefabByMonsterData(MonsterData monsterData, Vector3 vScl, Vector3 vPos, Transform parent = null, bool initIconState = true, bool readTexByASync = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GUIMonsterIcon.goMONSTER_ICON_M);
		gameObject.SetActive(true);
		Transform transform = gameObject.transform;
		if (parent == null)
		{
			transform.parent = GUIMonsterIcon.goMONSTER_ICON_BAK_ROOT.transform;
		}
		else
		{
			transform.parent = parent;
		}
		transform.localScale = vScl;
		transform.localPosition = vPos;
		GUIMonsterIcon component = gameObject.GetComponent<GUIMonsterIcon>();
		component.ReadTexByASync = readTexByASync;
		component.data = monsterData;
		component.ShowGUI();
		if (initIconState)
		{
			component.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			component.SelectNum = -1;
			component.DimmMess = string.Empty;
			component.SortMess = string.Empty;
			if (monsterData != null && monsterData.userMonster != null)
			{
				component.Lock = monsterData.userMonster.IsLocked;
			}
		}
		return component;
	}

	public static GUIMonsterIcon MakeQuestionPrefab(Vector3 vScl, Vector3 vPos, int depth, Transform parent)
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("ListParts/ListPartsThumbnail", null);
		gameObject.SetActive(true);
		GUIMonsterIcon component = gameObject.GetComponent<GUIMonsterIcon>();
		component.transform.parent = parent;
		component.transform.localScale = vScl;
		component.transform.localPosition = vPos;
		DepthController.SetWidgetDepth_Static(component.transform, depth);
		component.SetQuestionIcon();
		return component;
	}

	public void SetQuestionIcon()
	{
		base.ShowGUI();
		this.txCHAR.gameObject.SetActive(false);
		this.goArousal.SetActive(false);
		this.spBASE.spriteName = "Common02_Thumbnail_Question";
		this.spFRAME.spriteName = "Common02_Thumbnail_wakuQ";
		if (this.goGimmick != null)
		{
			this.goGimmick.SetActive(false);
		}
	}

	public void SetMonsterIcon(string iconId, string arousal, string growStep)
	{
		base.ShowGUI();
		this.txCHAR.transform.gameObject.SetActive(true);
		string monsterIconPathByIconId = GUIMonsterIcon.GetMonsterIconPathByIconId(iconId);
		string resourcePath = GUIMonsterIcon.InternalGetMonsterIconPathByIconId(iconId);
		GUIMonsterIcon.SetTextureMonsterParts(this.txCHAR, resourcePath, monsterIconPathByIconId, this.ReadTexByASync);
		GUIMonsterIcon.DispArousal(arousal, this.goArousal, this.spArousal);
		int growStep2 = int.Parse(growStep);
		GUIMonsterIcon.SetThumbnailFrame(this.spBASE, this.spFRAME, growStep2);
		if (!this._gimmick && this.goGimmick != null)
		{
			this.goGimmick.SetActive(false);
		}
	}

	public void ClearMonsterData()
	{
		this.data = null;
	}

	public void SetEvolutionMonsterIcon(bool canEvoluve, bool onlyGrayOut)
	{
		this.SetSortMessageColor(ConstValue.DIGIMON_GREEN);
		if (canEvoluve)
		{
			if (!onlyGrayOut)
			{
				this.SortMess = StringMaster.GetString("CharaIcon-01");
				this.SetSortMessageColor(ConstValue.DIGIMON_YELLOW);
			}
		}
		else
		{
			this.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
			if (!onlyGrayOut)
			{
				this.SortMess = StringMaster.GetString("CharaIcon-02");
				this.SetSortMessageColor(ConstValue.DIGIMON_BLUE);
			}
		}
	}

	public void SetVersionUpMonsterIcon(bool canVersionUp, bool onlyGrayOut)
	{
		this.SetSortMessageColor(ConstValue.DIGIMON_GREEN);
		if (canVersionUp)
		{
			if (!onlyGrayOut)
			{
				this.SortMess = StringMaster.GetString("CharaIcon-05");
				this.SetSortMessageColor(ConstValue.DIGIMON_YELLOW);
			}
		}
		else
		{
			this.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
			if (!onlyGrayOut)
			{
				this.SortMess = StringMaster.GetString("CharaIcon-06");
				this.SetSortMessageColor(ConstValue.DIGIMON_BLUE);
			}
		}
	}

	public static bool GetIconGrayOutType(MonsterSortType type)
	{
		bool result = true;
		if (type == MonsterSortType.DATE || type == MonsterSortType.LEVEL)
		{
			result = false;
		}
		return result;
	}

	public static string GetMonsterIconPathByIconId(string iconId)
	{
		return "CharacterThumbnail/" + iconId + "/thumb";
	}

	public static string InternalGetMonsterIconPathByIconId(string iconId)
	{
		return "CharacterThumbnailInternal/" + iconId + "/thumb";
	}

	public static void DispArousal(string arousal, GameObject goArousal, UISprite spArousal)
	{
		if (MonsterStatusData.IsArousal(arousal))
		{
			if (!goArousal.activeSelf)
			{
				goArousal.SetActive(true);
			}
			spArousal.spriteName = MonsterDetailUtil.GetArousalSpriteName(int.Parse(arousal));
		}
		else if (goArousal.activeSelf)
		{
			goArousal.SetActive(false);
		}
	}

	public void SetLock()
	{
		if (this.Data != null && this.Data.userMonster != null)
		{
			this.Lock = this.Data.userMonster.IsLocked;
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
