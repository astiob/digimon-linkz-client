using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUIComponents : MonoBehaviour
{
	[SerializeField]
	[Header("UIRoot")]
	private UIRoot _uiRoot;

	[Header("UIカメラ")]
	[SerializeField]
	private Camera _uiCamera;

	[Header("ロードUI")]
	[SerializeField]
	public BattleUIInitialize initializeUi;

	[Header("AlwaysのPrefab")]
	[SerializeField]
	public BattleAlways battleAlwaysUi;

	[SerializeField]
	[Header("ボスウェーブ開始時のテロップ")]
	private UIWidget _bossStartUi;

	[Header("バトル開始時のテロップ")]
	[SerializeField]
	public BattleStartAction battleStartAction;

	[Header("乱入ウェーブ開始時のテロップ")]
	[SerializeField]
	private UIWidget _extraStartUi;

	[Header("ラウンド開始時のテロップ")]
	[SerializeField]
	public RoundStart roundStart;

	[Header("ラウンド開始時のテロップ(制限ラウンド)")]
	[SerializeField]
	public RoundLimitStart roundLimitStart;

	[SerializeField]
	[Header("ラウンド開始時のテロップ(スピードクリア設定)")]
	public RoundChallengeStart roundChallengeStart;

	[Header("敵ターン開始時のテロップ")]
	[SerializeField]
	private UIWidget _enemyTurnUi;

	[Header("タイムオーバー時のテロップ")]
	[SerializeField]
	private UIWidget _timeOverUi;

	[Header("状態異常による警告テロップ")]
	[SerializeField]
	public IsWarning isWarning;

	[Header("PlayerWinnerのPrefab")]
	[SerializeField]
	public PlayerWinner playerWinnerUi;

	[SerializeField]
	[Header("ウェーブ開始時のテロップ")]
	private UIWidget _nextWaveUi;

	[SerializeField]
	[Header("プレイヤーの敗北UI")]
	private UIWidget _playerFailUi;

	[SerializeField]
	[Header("コンテニューダイアログ")]
	private UIWidget _continueUi;

	[SerializeField]
	[Header("フェードUI")]
	private BattleFadeout _fadeoutUi;

	[SerializeField]
	[Header("SkillSelect")]
	private BattleSkillSelect _skillSelectUi;

	[Header("復活ダイアログ")]
	[SerializeField]
	public CharacterRevivalDialog characterRevivalDialog;

	[NonSerialized]
	public MenuDialog menuDialog;

	[SerializeField]
	[Header("コンテニューダイアログ")]
	public DialogContinue dialogContinue;

	[Header("オートボタン")]
	[SerializeField]
	public BattleAutoMenu autoPlayButton;

	[Header("スキップボタン")]
	public BattleUISingleX2PlayButton x2PlayButton;

	[SerializeField]
	[Header("メニューのボタン")]
	public UIButton menuButton;

	[Header("スキル発動時のテロップ")]
	[SerializeField]
	public TurnAction turnAction;

	[Header("ItemInfoFieldのPrefab")]
	[SerializeField]
	public ItemInfoField itemInfoField;

	[NonSerialized]
	public GameObject helpDialog;

	[SerializeField]
	[Header("初期誘導UI")]
	public BattleUIInitialInduction initialInduction;

	[Header("ヒットアイコンUI")]
	[SerializeField]
	private GameObject _hitIconObject;

	[SerializeField]
	[Header("HUD")]
	private GameObject _hudObject;

	[Header("ビッグボス用のHUD")]
	public GameObject bigBossHudObject;

	[SerializeField]
	[Header("ターゲットアイコン（カーソル）")]
	private GameObject _manualSelectTargetObject;

	[SerializeField]
	[Header("ターゲットアイコン（アロー）")]
	private GameObject _toleranceIconObject;

	[Header("ドロップアイテム")]
	[SerializeField]
	private GameObject _droppingItemObject;

	[Header("特効効果テロップ")]
	public BattleExtraEffectUI battleExtraEffectUI;

	[Header("Winのオブジェクト(PvP限定)")]
	[SerializeField]
	public GameObject winGameObject;

	[SerializeField]
	[Header("ステージ効果による上昇減少アイコン")]
	private GameObject _battleGimmickStatusObject;

	[SerializeField]
	[Header("チップ効果発動時の演出用")]
	private ChipBarLnvocation _chipBarLnvocation;

	[Header("チップ発動アイコンオブジェクト")]
	[SerializeField]
	private GameObject _chipThumbnailAdvent;

	[NonSerialized]
	public UIPanel rootPanel;

	[NonSerialized]
	public UICamera rootCamera;

	[NonSerialized]
	public List<BattleUIHUD> hudObjectInstanced = new List<BattleUIHUD>();

	[NonSerialized]
	public BattleUIHUD bigBossHudObjectInstanced;

	[NonSerialized]
	public List<Collider> hudColliderInstanced = new List<Collider>();

	[NonSerialized]
	public List<DepthController> hudObjectDepthController = new List<DepthController>();

	[NonSerialized]
	public List<DroppingItem> droppingItemObjectInstanced = new List<DroppingItem>();

	[NonSerialized]
	public List<ManualSelectTarget> manualSelectTargetObjectInstanced = new List<ManualSelectTarget>();

	[NonSerialized]
	public List<ToleranceIcon> toleranceIconObjectInstanced = new List<ToleranceIcon>();

	[NonSerialized]
	public DepthController arrowDepthController;

	[NonSerialized]
	public List<Transform> otherCeratedUITransforms = new List<Transform>();

	[NonSerialized]
	public List<List<HitIcon>> hitIconObjectInstanced;

	[NonSerialized]
	public List<GameObject> battleGimmickStatusInstanced = new List<GameObject>();

	public UIRoot uiRoot
	{
		get
		{
			return this._uiRoot;
		}
	}

	public Camera uiCamera
	{
		get
		{
			return this._uiCamera;
		}
	}

	public UIWidget bossStartUi
	{
		get
		{
			return this._bossStartUi;
		}
	}

	public UIWidget extraStartUi
	{
		get
		{
			return this._extraStartUi;
		}
	}

	public UIWidget enemyTurnUi
	{
		get
		{
			return this._enemyTurnUi;
		}
	}

	public UIWidget timeOverUi
	{
		get
		{
			return this._timeOverUi;
		}
	}

	public UIWidget nextWaveUi
	{
		get
		{
			return this._nextWaveUi;
		}
	}

	public UIWidget playerFailUi
	{
		get
		{
			return this._playerFailUi;
		}
	}

	public UIWidget continueUi
	{
		get
		{
			return this._continueUi;
		}
	}

	public BattleFadeout fadeoutUi
	{
		get
		{
			return this._fadeoutUi;
		}
	}

	public BattleSkillSelect skillSelectUi
	{
		get
		{
			return this._skillSelectUi;
		}
	}

	public BattleDigimonStatus characterStatusDescription { get; protected set; }

	public BattleDigimonEnemyStatus enemyStatusDescription { get; private set; }

	public BattleDigimonStatus pvpEnemyStatusDescription { get; set; }

	public virtual GameObject enemyStatusDescriptionGO
	{
		get
		{
			return this.enemyStatusDescription.gameObject;
		}
	}

	public DialogRetire dialogRetire { get; private set; }

	public GameObject hitIconObject
	{
		get
		{
			return this._hitIconObject;
		}
	}

	public GameObject hudObject
	{
		get
		{
			return this._hudObject;
		}
	}

	public GameObject manualSelectTargetObject
	{
		get
		{
			return this._manualSelectTargetObject;
		}
	}

	public GameObject toleranceIconObject
	{
		get
		{
			return this._toleranceIconObject;
		}
	}

	public GameObject droppingItemObject
	{
		get
		{
			return this._droppingItemObject;
		}
	}

	public GameObject battleGimmickStatusObject
	{
		get
		{
			return this._battleGimmickStatusObject;
		}
	}

	public ChipBarLnvocation chipBarLnvocation
	{
		get
		{
			return this._chipBarLnvocation;
		}
	}

	public GameObject chipThumbnailAdvent
	{
		get
		{
			return this._chipThumbnailAdvent;
		}
	}

	public void GetUIRoot()
	{
		if (GUIMain.GetUIRoot() != null)
		{
			UIRoot uiroot = GUIMain.GetUIRoot();
			UIRoot uiRoot = this._uiRoot;
			this._uiRoot = uiroot;
			UIPanel component = BattleStateManager.current.battleUiComponents.transform.GetComponent<UIPanel>();
			if (component == null)
			{
				component = this.initializeUi.transform.parent.GetComponent<UIPanel>();
			}
			component.transform.SetParent(uiroot.transform);
			component.SetAnchor(uiroot.transform);
			this.rootPanel = component;
			this._uiCamera = GUIMain.GetUICamera().cachedCamera;
			this.rootCamera = GUIMain.GetUICamera();
			UIAnchorOverride.SetUIRoot(this.uiRoot);
			if (uiRoot != null)
			{
				UnityEngine.Object.Destroy(uiRoot.gameObject);
			}
		}
	}

	public void Init()
	{
		this.CreateRetireObject();
		this.CreateStatusObjects();
	}

	private void CreateRetireObject()
	{
		GameObject uibattlePrefab = ResourcesPath.GetUIBattlePrefab("DialogRetire");
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(uibattlePrefab);
		Transform transform = gameObject.transform;
		this.SetupDialogs(transform);
		transform.localPosition = new Vector3(2f, 0f, 0f);
		transform.localScale = Vector3.one;
		this.dialogRetire = gameObject.GetComponent<DialogRetire>();
		UIWidget component = gameObject.transform.FindChild("BG").GetComponent<UIWidget>();
		component.SetAnchor(this.battleAlwaysUi.gameObject);
		component.leftAnchor.absolute = -1;
		component.rightAnchor.absolute = 1;
		component.bottomAnchor.absolute = -1;
		component.topAnchor.absolute = 1;
		component.ResetAnchors();
		component.UpdateAnchors();
		gameObject.SetActive(false);
	}

	protected virtual void SetupDialogs(Transform dialogRetireTrans)
	{
		this.menuDialog = this.battleAlwaysUi.SetupMenu();
		this.helpDialog = this.battleAlwaysUi.helpDialogGO;
		dialogRetireTrans.SetParent(this.battleAlwaysUi.menuPanelTransform);
	}

	protected virtual void CreateStatusObjects()
	{
		this.characterStatusDescription = this.skillSelectUi.CreateStatusAlly();
		this.characterStatusDescription.gameObject.SetActive(false);
		this.enemyStatusDescription = this.skillSelectUi.CreateStatusEnemy();
		this.enemyStatusDescription.gameObject.SetActive(false);
	}
}
