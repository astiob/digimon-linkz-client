using BattleStateMachineInternal;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleInputFunctionBase))]
public sealed class BattleStateManager : MonoBehaviour
{
	public BattleAdventureSceneManager battleAdventureSceneManager;

	[Header("シングルUI")]
	[SerializeField]
	private BattleUIComponentsSingle battleUIComponentsSingle;

	[Header("マルチUI")]
	[SerializeField]
	private BattleUIComponentsMulti battleUIComponentsMulti;

	[SerializeField]
	[Header("PvPUI")]
	private BattleUIComponentsPvP battleUIComponentsPvP;

	private static BattleMode _battleMode;

	[SerializeField]
	private BattleScreen _battleScreen;

	[SerializeField]
	private bool _onServerConnect;

	public static bool onAutoServerConnect;

	public static bool onAutoChangeTutorialMode;

	public static bool onAutoChangeSkipActionMode;

	private BattleStateMainController _rootState;

	[SerializeField]
	private BattleStateHierarchyData _hierarchyData;

	[SerializeField]
	private BattleStateData _battleStateData;

	[SerializeField]
	private BattleStateProperty _stateProperty;

	[SerializeField]
	private BattleStateUIProperty _uiProperty;

	public static string BattleSceneName = "Battle";

	public List<Action<bool>> onApplicationPause = new List<Action<bool>>();

	public List<Action> lateUpdateJustOnce = new List<Action>();

	private bool onCalledBattleTrigger;

	private bool onCalledAwake;

	private bool isSetBackKey;

	public BattleInitialize initialize { get; private set; }

	public BattleEvent events { get; private set; }

	public BattleValues values { get; private set; }

	public BattleFraudCheck fraudCheck { get; private set; }

	public BattleSoundPlayer soundPlayer { get; private set; }

	public BattleHelp help { get; private set; }

	public BattleCallAction callAction { get; private set; }

	public BattleServerControl serverControl { get; private set; }

	public BattleRecover recover { get; private set; }

	public BattleTime time { get; private set; }

	public BattleSleep sleep { get; private set; }

	public BattleWaveControl waveControl { get; private set; }

	public BattleDeadOrAlive deadOrAlive { get; private set; }

	public Battle3DAction threeDAction { get; private set; }

	public BattleCameraControl cameraControl { get; private set; }

	public BattleTargetSelect targetSelect { get; private set; }

	public BattleLog log { get; private set; }

	public BattleSystem system { get; private set; }

	public BattleTutorial tutorial { get; private set; }

	public BattleUIControlBasic uiControl { get; private set; }

	public BattleUIControlSingle uiControlSingle
	{
		get
		{
			return this.uiControl as BattleUIControlSingle;
		}
	}

	public BattleUIControlMultiBasic uiControlMultiBasic
	{
		get
		{
			return this.uiControl as BattleUIControlMultiBasic;
		}
	}

	public BattleUIControlMulti uiControlMulti
	{
		get
		{
			return this.uiControl as BattleUIControlMulti;
		}
	}

	public BattleUIControlPvP uiControlPvP
	{
		get
		{
			return this.uiControl as BattleUIControlPvP;
		}
	}

	public BattleInputBasic input { get; private set; }

	public BattleInputSingle inputSingle
	{
		get
		{
			return this.input as BattleInputSingle;
		}
	}

	public BattleInputMulti inputMulti
	{
		get
		{
			return this.input as BattleInputMulti;
		}
	}

	public BattleInputPvP inputPvP
	{
		get
		{
			return this.input as BattleInputPvP;
		}
	}

	public BattleRoundFunction roundFunction { get; private set; }

	public BattleSkillDetails skillDetails { get; private set; }

	public BattleMultiBasicFunction multiBasicFunction { get; private set; }

	public BattlePvPFunction pvpFunction
	{
		get
		{
			return this.multiBasicFunction as BattlePvPFunction;
		}
	}

	public BattleMultiFunction multiFunction
	{
		get
		{
			return this.multiBasicFunction as BattleMultiFunction;
		}
	}

	public IBattleFunctionInput[] GetAllBattleFunctions()
	{
		return new IBattleFunctionInput[]
		{
			this.initialize,
			this.events,
			this.values,
			this.waveControl,
			this.soundPlayer,
			this.uiControl,
			this.callAction,
			this.roundFunction,
			this.skillDetails,
			this.multiBasicFunction,
			this.deadOrAlive,
			this.serverControl,
			this.recover,
			this.time,
			this.sleep,
			this.system,
			this.help,
			this.threeDAction,
			this.targetSelect,
			this.log,
			this.tutorial,
			this.input
		};
	}

	public BattleMode battleMode
	{
		get
		{
			return BattleStateManager._battleMode;
		}
	}

	public BattleScreen battleScreen
	{
		get
		{
			return this._battleScreen;
		}
	}

	public bool onServerConnect
	{
		get
		{
			return this._onServerConnect;
		}
	}

	public bool onEnableTutorial
	{
		get
		{
			return this.battleMode == BattleMode.Tutorial;
		}
	}

	public BattleStateMainController rootState
	{
		get
		{
			return this._rootState;
		}
	}

	public BattleStateHierarchyData hierarchyData
	{
		get
		{
			return this._hierarchyData;
		}
	}

	public BattleStateData battleStateData
	{
		get
		{
			return this._battleStateData;
		}
	}

	public BattleStateProperty stateProperty
	{
		get
		{
			return this._stateProperty;
		}
	}

	public BattleUIComponents battleUiComponents { get; private set; }

	public BattleUIComponentsSingle battleUiComponentsSingle
	{
		get
		{
			return this.battleUiComponents as BattleUIComponentsSingle;
		}
	}

	public BattleUIComponentsMultiBasic battleUiComponentsMultiBasic
	{
		get
		{
			return this.battleUiComponents as BattleUIComponentsMultiBasic;
		}
	}

	public BattleUIComponentsMulti battleUiComponentsMulti
	{
		get
		{
			return this.battleUiComponents as BattleUIComponentsMulti;
		}
	}

	public BattleUIComponentsPvP battleUiComponentsPvP
	{
		get
		{
			return this.battleUiComponents as BattleUIComponentsPvP;
		}
	}

	public BattleStateUIProperty uiProperty
	{
		get
		{
			return this._uiProperty;
		}
	}

	public string publicAttackSkillId
	{
		get
		{
			return BattleStateManager.PublicAttackSkillId;
		}
	}

	public static string PublicAttackSkillId
	{
		get
		{
			if (string.IsNullOrEmpty(GameWebAPI.RespDataMA_GetSkillM.SkillM.ActionSkill))
			{
				return "public_attack";
			}
			return GameWebAPI.RespDataMA_GetSkillM.SkillM.ActionSkill;
		}
	}

	public bool isLastBattle
	{
		get
		{
			return this.battleStateData.currentWaveNumber >= this.hierarchyData.batteWaves.Length - 1;
		}
	}

	public bool IsLastBattleAndAllDeath()
	{
		return this.isLastBattle && this.battleStateData.GetCharactersDeath(true);
	}

	public static BattleStateManager current { get; private set; }

	public bool isShowAlwaysScreen { get; private set; }

	public bool dialogOpenCloseWaitFlag { get; set; }

	public SoundMng soundManager { get; private set; }

	public static void StartSingle(float outSec = 0.5f, float inSec = 0.5f, bool destroyMonsterIcon = true, Action<int> actionEnd = null)
	{
		BattleStateManager._battleMode = BattleMode.Single;
		GUIMain.FadeBlackLoadScene(BattleStateManager.BattleSceneName, outSec, inSec, destroyMonsterIcon, actionEnd);
	}

	public static void StartMulti(float outSec = 0.5f, float inSec = 0.5f, bool destroyMonsterIcon = true, Action<int> actionEnd = null)
	{
		BattleStateManager._battleMode = BattleMode.Multi;
		GUIMain.FadeBlackLoadScene(BattleStateManager.BattleSceneName, outSec, inSec, destroyMonsterIcon, actionEnd);
	}

	public static void StartPvP(float outSec = 0.5f, float inSec = 0.5f, bool destroyMonsterIcon = true, Action<int> actionEnd = null)
	{
		BattleStateManager._battleMode = BattleMode.PvP;
		GUIMain.FadeBlackLoadScene(BattleStateManager.BattleSceneName, outSec, inSec, destroyMonsterIcon, actionEnd);
	}

	private void Awake()
	{
		if (this.onCalledAwake)
		{
			return;
		}
		this.onCalledAwake = true;
		this.battleStateData.isEnableBackKeySystem = false;
		this._battleScreen = BattleScreen.Static;
		BattleStateManager.current = this;
		if (SoundMng.Instance() == null)
		{
			SoundMng soundMng = base.gameObject.AddComponent<SoundMng>();
			soundMng.Initialize();
		}
		this.soundManager = SoundMng.Instance();
		this.hierarchyData.Initialize();
		Input.multiTouchEnabled = false;
		this.battleUiComponents = base.GetComponent<BattleUIComponents>();
		this.input = base.GetComponent<BattleInputBasic>();
		if (this.battleUiComponents == null && this.input == null)
		{
			switch (this.battleMode)
			{
			case BattleMode.Single:
				this.battleUiComponents = this.battleUIComponentsSingle;
				this.input = this.battleUiComponents.gameObject.AddComponent<BattleInputSingle>();
				break;
			case BattleMode.Multi:
				this.battleUiComponents = this.battleUIComponentsMulti;
				this.input = this.battleUiComponents.gameObject.AddComponent<BattleInputMulti>();
				break;
			case BattleMode.PvP:
				this.battleUiComponents = this.battleUIComponentsPvP;
				this.input = this.battleUiComponents.gameObject.AddComponent<BattleInputPvP>();
				break;
			}
			this.battleUIComponentsSingle.enabled = (this.battleMode == BattleMode.Single);
			this.battleUIComponentsMulti.enabled = (this.battleMode == BattleMode.Multi);
			this.battleUIComponentsPvP.enabled = (this.battleMode == BattleMode.PvP);
		}
		this.battleUiComponents.Init();
		this._stateProperty = BattleActionProperties.GetProperties(this.battleMode);
		this._uiProperty = BattleActionProperties.GetUIProperties(this.battleMode);
		if (BattleStateManager.onAutoServerConnect)
		{
			this._onServerConnect = true;
		}
		if (BattleStateManager.onAutoChangeTutorialMode)
		{
			BattleStateManager._battleMode = BattleMode.Tutorial;
		}
		this.initialize = new BattleInitialize();
		this.events = new BattleEvent();
		this.values = new BattleValues();
		this.waveControl = new BattleWaveControl();
		this.fraudCheck = new BattleFraudCheck();
		this.soundPlayer = new BattleSoundPlayer();
		this.help = new BattleHelp();
		this.time = new BattleTime();
		this.sleep = new BattleSleep();
		this.system = new BattleSystem();
		this.threeDAction = new Battle3DAction();
		this.cameraControl = new BattleCameraControl(this);
		this.targetSelect = new BattleTargetSelect();
		this.deadOrAlive = new BattleDeadOrAlive();
		this.callAction = new BattleCallAction();
		this.serverControl = new BattleServerControl();
		this.recover = new BattleRecover();
		this.log = new BattleLog();
		this.roundFunction = new BattleRoundFunction();
		this.skillDetails = new BattleSkillDetails();
		switch (this.battleMode)
		{
		case BattleMode.Multi:
			this.uiControl = new BattleUIControlMulti();
			this.multiBasicFunction = new BattleMultiFunction();
			this._rootState = new MultiBattleState();
			if (!Singleton<TCPMessageSender>.IsInstance())
			{
				base.gameObject.AddComponent<TCPMessageSender>();
			}
			break;
		case BattleMode.PvP:
			this.uiControl = new BattleUIControlPvP();
			this.multiBasicFunction = new BattlePvPFunction();
			this._rootState = new PvPBattleState();
			if (!Singleton<TCPMessageSender>.IsInstance())
			{
				base.gameObject.AddComponent<TCPMessageSender>();
			}
			break;
		case BattleMode.Tutorial:
			this.tutorial = new BattleTutorial();
			this.uiControl = new BattleUIControlSingle();
			this._rootState = new SingleBattleState();
			break;
		default:
			this.uiControl = new BattleUIControlSingle();
			this._rootState = new SingleBattleState();
			break;
		}
		this.uiControl.ApplySetBattleStateRegistration();
		foreach (IBattleFunctionInput battleFunctionInput in this.GetAllBattleFunctions())
		{
			if (battleFunctionInput != null)
			{
				battleFunctionInput.BattleAwakeInitialize();
			}
		}
		if (this.input)
		{
			this.input.BattleAwakeInitialize();
		}
		if (this.onServerConnect)
		{
			this.BattleTrigger();
		}
	}

	public void BattleTrigger(Action awakeAfter, Action triggerAfter = null)
	{
		if (this.onCalledBattleTrigger)
		{
			return;
		}
		this.onCalledBattleTrigger = true;
		this.Awake();
		CharacterStateControl.onUseRevivalMaxAp = this.hierarchyData.onApRevivalMax;
		SkillStatus.onHitRate100Percent = this.hierarchyData.onHitRate100Percent;
		foreach (IBattleFunctionInput battleFunctionInput in this.GetAllBattleFunctions())
		{
			if (battleFunctionInput != null)
			{
				battleFunctionInput.BattleTriggerInitialize();
			}
		}
		if (this.input != null)
		{
			this.input.BattleTriggerInitialize();
		}
		this.SetBattleScreen(BattleScreen.Initialize);
		this.battleStateData.isEnableBackKeySystem = true;
		this.rootState.OnStartState(null, null);
	}

	public void BattleTrigger()
	{
		this.BattleTrigger(null, null);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (this.onApplicationPause == null)
		{
			return;
		}
		for (int i = 0; i < this.onApplicationPause.Count; i++)
		{
			if (this.onApplicationPause[i] != null)
			{
				this.onApplicationPause[i](pauseStatus);
			}
		}
	}

	private void OnDestroy()
	{
		foreach (IBattleFunctionInput battleFunctionInput in this.GetAllBattleFunctions())
		{
			if (battleFunctionInput != null)
			{
				battleFunctionInput.BattleEndBefore();
			}
		}
		if (this.input != null)
		{
			this.input.BattleEndBefore();
		}
	}

	public void SetBattleScreen(BattleScreen state)
	{
		this.isShowAlwaysScreen = this.uiControl.ApplySetBattleState(this._battleScreen, state);
		this._battleScreen = state;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!this.hierarchyData.onEnableBackKey)
			{
				return;
			}
			if (!this.battleStateData.isEnableBackKeySystem)
			{
				return;
			}
			if (this.dialogOpenCloseWaitFlag)
			{
				return;
			}
			this.isSetBackKey = true;
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < this.lateUpdateJustOnce.Count; i++)
		{
			this.lateUpdateJustOnce[i]();
		}
		this.lateUpdateJustOnce.Clear();
		if (this.isSetBackKey)
		{
			this.UpdateBackKeyFunction();
		}
		this.isSetBackKey = false;
	}

	private void UpdateBackKeyFunction()
	{
		if (this.battleStateData.isShowHelp)
		{
			this.callAction.OnHideHelp();
			return;
		}
		if (this.battleStateData.isShowRetireWindow)
		{
			this.callAction.OnCancelRetire();
			return;
		}
		if (this.battleStateData.isShowRevivalWindow)
		{
			this.callAction.OnCancelCharacterRevival();
			return;
		}
		if (this.battleStateData.isShowInitialIntroduction)
		{
			this.callAction.OnCloseInitialInduction();
			return;
		}
		if (this.battleStateData.isImpossibleShowMenu)
		{
			return;
		}
		if (!this.battleStateData.isShowMenuWindow)
		{
			this.callAction.OnShowMenu();
			return;
		}
		this.callAction.OnHideMenu();
	}
}
