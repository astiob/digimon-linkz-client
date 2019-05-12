using BattleStateMachineInternal;
using Enemy.AI;
using JsonFx.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

[Serializable]
public class CharacterStateControl
{
	public const int AttackSkillIndex = 0;

	public const int DefaultAwakeAp = 3;

	public const int DefaultMaxAp = 5;

	public const int DefaultSkillOrder = -1;

	public static bool onUseRevivalMaxAp;

	[SerializeField]
	private CharacterStatus _characterStatus;

	[SerializeField]
	private Tolerance _tolerance;

	[SerializeField]
	private CharacterDatas _characterDatas;

	[SerializeField]
	private CharacterParams _characterParams;

	[SerializeField]
	private LeaderSkillStatus _leaderSkillStatus;

	[SerializeField]
	private LeaderSkillResult _leaderSkillResult;

	[SerializeField]
	private SkillStatus[] _skillStatus = new SkillStatus[0];

	[SerializeField]
	private SharedStatus _sharedStatus;

	[SerializeField]
	private EnemyAIPattern enemyAi;

	[SerializeField]
	private int _currentHp;

	[SerializeField]
	private int _maxHp;

	[SerializeField]
	private int _currentAp;

	[SerializeField]
	private int _upAp;

	[SerializeField]
	private int _isSelectSkill = -1;

	[SerializeField]
	private float _randomedSpeed;

	[SerializeField]
	private int _hate;

	[SerializeField]
	private int _previousHate;

	[SerializeField]
	private bool _isLeader;

	[SerializeField]
	private int _skillOrder;

	[SerializeField]
	private int _myIndex;

	[SerializeField]
	private bool _isEnemy;

	[SerializeField]
	private bool _isDiedJustBefore;

	[SerializeField]
	private HaveSufferState _currentSufferState = new HaveSufferState();

	[SerializeField]
	private string[] _skillIds = new string[0];

	[SerializeField]
	private CharacterStateControl _targetCharacter;

	[SerializeField]
	private List<ItemDropResult> _itemDropResult = new List<ItemDropResult>();

	private Dictionary<int, int> chipEffectCount = new Dictionary<int, int>();

	private int m_extraMaxHp;

	private int m_extraAttackPower;

	private int m_extraDefencePower;

	private int m_extraSpecialAttackPower;

	private int m_extraSpecialDefencePower;

	private int m_extraSpeed;

	private int m_extraDeathblowSkillPower;

	private int m_extraInheritanceSkillPower;

	private float m_extraDeathblowSkillHitRate;

	private float m_extraInheritanceSkillHitRate;

	private int m_defaultExtraAttackPower;

	private int m_defaultExtraDefencePower;

	private int m_defaultExtraSpecialAttackPower;

	private int m_defaultExtraSpecialDefencePower;

	private int m_defaultExtraSpeed;

	private int m_chipAddMaxHp;

	private int m_chipAddAttackPower;

	private int m_chipAddDefencePower;

	private int m_chipAddSpecialAttackPower;

	private int m_chipAddSpecialDefencePower;

	private int m_chipAddSpeed;

	private float m_chipAddCritical;

	private float m_chipAddHit;

	private int m_stageChipAddMaxHp;

	private int m_stageChipAddAttackPower;

	private int m_stageChipAddDefencePower;

	private int m_stageChipAddSpecialAttackPower;

	private int m_stageChipAddSpecialDefencePower;

	private int m_stageChipAddSpeed;

	private bool isLoad;

	private Dictionary<ChipEffectStatus.EffectTriggerType, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> triggerChips = new Dictionary<ChipEffectStatus.EffectTriggerType, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>>();

	public bool isRecommand;

	public List<SufferStateProperty> hitSufferList = new List<SufferStateProperty>();

	public bool isMultiAreaRandomDamageSkill;

	private static SkillStatus statusTemp;

	private static AIActionClip actionClipTemp;

	private static SufferStateProperty.SufferType sufferTypeTemp;

	public CharacterStateControl()
	{
		this.stagingChipIdList = new Dictionary<int, int>();
		this.autoCounterSkillList = new List<string>();
		this.potencyChipIdList = new Dictionary<int, int>();
		this.isInitSpecialCorrectionStatus = false;
	}

	public CharacterStateControl(CharacterStatus status, Tolerance tolerance, CharacterDatas datas, LeaderSkillStatus leaderCharacterLeaderSkill, string attackSkillId, CharacterDatas leaderCharacterData, LeaderSkillStatus myLeaderSkill = null, bool isEnemy = false)
	{
		this.stagingChipIdList = new Dictionary<int, int>();
		this.autoCounterSkillList = new List<string>();
		this.potencyChipIdList = new Dictionary<int, int>();
		this.isInitSpecialCorrectionStatus = false;
		this._sharedStatus = SharedStatus.GetEmptyStatus();
		this._characterStatus = status;
		this._characterDatas = datas;
		this._tolerance = tolerance;
		this._isEnemy = isEnemy;
		if (myLeaderSkill == null)
		{
			this._leaderSkillStatus = LeaderSkillStatus.GetUnHavingLeaderSkill();
		}
		else
		{
			this._leaderSkillStatus = myLeaderSkill;
		}
		this._leaderSkillResult = new LeaderSkillResult(this, leaderCharacterLeaderSkill, leaderCharacterData);
		this.StatusInitialize(attackSkillId);
		this.enemyAi = ((status.GetType() != typeof(EnemyStatus)) ? null : this.enemyStatus.enemyAiPattern);
	}

	public bool isInitSpecialCorrectionStatus { get; private set; }

	public int extraMaxHp
	{
		get
		{
			return this.m_extraMaxHp + this.m_chipAddMaxHp + this.m_stageChipAddMaxHp;
		}
	}

	public int extraAttackPower
	{
		get
		{
			return this.m_extraAttackPower + this.m_chipAddAttackPower + this.m_stageChipAddAttackPower;
		}
	}

	public int extraDefencePower
	{
		get
		{
			return this.m_extraDefencePower + this.m_chipAddDefencePower + this.m_stageChipAddDefencePower;
		}
	}

	public int extraSpecialAttackPower
	{
		get
		{
			return this.m_extraSpecialAttackPower + this.m_chipAddSpecialAttackPower + this.m_stageChipAddSpecialAttackPower;
		}
	}

	public int extraSpecialDefencePower
	{
		get
		{
			return this.m_extraSpecialDefencePower + this.m_chipAddSpecialDefencePower + this.m_stageChipAddSpecialDefencePower;
		}
	}

	public int extraSpeed
	{
		get
		{
			return this.m_extraSpeed + this.m_chipAddSpeed + this.m_stageChipAddSpeed;
		}
	}

	public int extraDeathblowSkillPower
	{
		get
		{
			return this.m_extraDeathblowSkillPower;
		}
	}

	public int extraInheritanceSkillPower
	{
		get
		{
			return this.m_extraInheritanceSkillPower;
		}
	}

	public float extraDeathblowSkillHitRate
	{
		get
		{
			return this.m_extraDeathblowSkillHitRate;
		}
	}

	public float extraInheritanceSkillHitRate
	{
		get
		{
			return this.m_extraInheritanceSkillHitRate;
		}
	}

	public int defaultExtraMaxHp
	{
		get
		{
			return this._maxHp + this.m_chipAddMaxHp + this.m_stageChipAddMaxHp;
		}
	}

	public int defaultExtraAttackPower
	{
		get
		{
			return this.m_defaultExtraAttackPower + this.m_chipAddAttackPower + this.m_stageChipAddAttackPower;
		}
	}

	public int defaultExtraDefencePower
	{
		get
		{
			return this.m_defaultExtraDefencePower + this.m_chipAddDefencePower + this.m_stageChipAddDefencePower;
		}
	}

	public int defaultExtraSpecialAttackPower
	{
		get
		{
			return this.m_defaultExtraSpecialAttackPower + this.m_chipAddSpecialAttackPower + this.m_stageChipAddSpecialAttackPower;
		}
	}

	public int defaultExtraSpecialDefencePower
	{
		get
		{
			return this.m_defaultExtraSpecialDefencePower + this.m_chipAddSpecialDefencePower + this.m_stageChipAddSpecialDefencePower;
		}
	}

	public int defaultExtraSpeed
	{
		get
		{
			return this.m_defaultExtraSpeed + this.m_chipAddSpeed + this.m_stageChipAddSpeed;
		}
	}

	public int chipAddMaxHp
	{
		get
		{
			return this.m_chipAddMaxHp;
		}
	}

	public int chipAddAttackPower
	{
		get
		{
			return this.m_chipAddAttackPower;
		}
	}

	public int chipAddDefencePower
	{
		get
		{
			return this.m_chipAddDefencePower;
		}
	}

	public int chipAddSpecialAttackPower
	{
		get
		{
			return this.m_chipAddSpecialAttackPower;
		}
	}

	public int chipAddSpecialDefencePower
	{
		get
		{
			return this.m_chipAddSpecialDefencePower;
		}
	}

	public int chipAddSpeed
	{
		get
		{
			return this.m_chipAddSpeed;
		}
	}

	public float chipAddCritical
	{
		get
		{
			return this.m_chipAddCritical / 100f;
		}
	}

	public float chipAddHit
	{
		get
		{
			return this.m_chipAddHit / 100f;
		}
	}

	public bool IsHittingTheTargetChip { get; private set; }

	public Dictionary<int, int> potencyChipIdList { get; private set; }

	public Dictionary<int, int> stagingChipIdList { get; private set; }

	public CharacterParams CharacterParams
	{
		get
		{
			return this._characterParams;
		}
		set
		{
			this._characterParams = value;
			this._characterParams.isEnemy = this._isEnemy;
		}
	}

	public CharacterStatus characterStatus
	{
		get
		{
			return this._characterStatus;
		}
	}

	public PlayerStatus playerStatus
	{
		get
		{
			return (PlayerStatus)this._characterStatus;
		}
	}

	public EnemyStatus enemyStatus
	{
		get
		{
			return (EnemyStatus)this._characterStatus;
		}
	}

	public LeaderSkillResult leaderSkillResult
	{
		get
		{
			return this._leaderSkillResult;
		}
	}

	public LeaderSkillStatus leaderSkillStatus
	{
		get
		{
			return this._leaderSkillStatus;
		}
	}

	public SkillStatus[] skillStatus
	{
		get
		{
			return this._skillStatus;
		}
		set
		{
			this._skillStatus = value;
		}
	}

	public SharedStatus sharedStatus
	{
		get
		{
			return this._sharedStatus;
		}
		set
		{
			this._sharedStatus = value;
		}
	}

	public string name
	{
		get
		{
			return this._characterDatas.name;
		}
	}

	public int hp
	{
		get
		{
			return this._currentHp;
		}
		set
		{
			int currentHp = this._currentHp;
			this._currentHp = Mathf.Clamp(value, 0, this.maxHp);
			if (currentHp == this._currentHp)
			{
				return;
			}
			if (this._currentHp == 0)
			{
				this.JudgeGuts(currentHp, this.maxHp);
			}
			if (this._currentHp == 0)
			{
				this.stagingChipIdList.Clear();
				this.OnChipTrigger(ChipEffectStatus.EffectTriggerType.Dead, false);
				this.RemoveDeadStagingChips();
			}
			else
			{
				this.OnChipTrigger(ChipEffectStatus.EffectTriggerType.HpPercentage, currentHp < this._currentHp);
				this.OnChipTrigger(ChipEffectStatus.EffectTriggerType.HpFixed, false);
			}
		}
	}

	public int maxHp
	{
		get
		{
			return this.extraMaxHp;
		}
	}

	public int ap
	{
		get
		{
			if (!this._sharedStatus.isShared)
			{
				return this._currentAp;
			}
			return this._sharedStatus.sharedAp;
		}
		set
		{
			int num = Mathf.Clamp(value, 0, this.maxAp);
			if (!this._sharedStatus.isShared)
			{
				this._currentAp = num;
			}
			else
			{
				this._sharedStatus.sharedAp = num;
			}
		}
	}

	public int upAp
	{
		get
		{
			return this._upAp;
		}
		set
		{
			this._upAp = value;
		}
	}

	public int arousal
	{
		get
		{
			return (!PlayerStatus.Match(this._characterStatus)) ? 0 : this.playerStatus.arousal;
		}
	}

	public int attackPower
	{
		get
		{
			return ((!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.attackPower : this.playerStatus.friendshipStatus.GetCorrectedAttackPower(this._characterStatus.attackPower)) + this.m_chipAddAttackPower;
		}
	}

	public int defencePower
	{
		get
		{
			return ((!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.defencePower : this.playerStatus.friendshipStatus.GetCorrectedDefencePower(this._characterStatus.defencePower)) + this.m_chipAddDefencePower;
		}
	}

	public int specialAttackPower
	{
		get
		{
			return ((!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.specialAttackPower : this.playerStatus.friendshipStatus.GetCorrectedSpecialAttackPower(this._characterStatus.specialAttackPower)) + this.m_chipAddSpecialAttackPower;
		}
	}

	public int specialDefencePower
	{
		get
		{
			return ((!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.specialDefencePower : this.playerStatus.friendshipStatus.GetCorrectedSpecialDefencePower(this._characterStatus.specialDefencePower)) + this.m_chipAddSpecialDefencePower;
		}
	}

	public int speed
	{
		get
		{
			return this.extraSpeed + this.m_chipAddSpeed;
		}
	}

	public int luck
	{
		get
		{
			return (!PlayerStatus.Match(this._characterStatus)) ? 0 : this.playerStatus.luck;
		}
	}

	public int level
	{
		get
		{
			return this._characterStatus.level;
		}
	}

	public int friendshipLevel
	{
		get
		{
			return (!PlayerStatus.Match(this._characterStatus)) ? 0 : this.playerStatus.friendshipStatus.friendshipLevel;
		}
	}

	public int maxFriendshipLevel
	{
		get
		{
			return this._characterDatas.GetMaxFriendshipLevel();
		}
	}

	public Talent talent
	{
		get
		{
			return (!PlayerStatus.Match(this._characterStatus)) ? new Talent() : this.playerStatus.talent;
		}
	}

	public Species species
	{
		get
		{
			return this._characterDatas.species;
		}
	}

	public EvolutionStep evolutionStep
	{
		get
		{
			return this._characterDatas.evolutionStep;
		}
	}

	public Tolerance tolerance
	{
		get
		{
			return (!PlayerStatus.Match(this._characterStatus)) ? this._tolerance : this.playerStatus.arousalTolerance.GetShiftedTolerance(this._tolerance);
		}
	}

	public HaveSufferState currentSufferState
	{
		get
		{
			return this._currentSufferState;
		}
		private set
		{
			this._currentSufferState = value;
		}
	}

	public string[] skillIds
	{
		get
		{
			return this._skillIds;
		}
		private set
		{
			this._skillIds = value;
		}
	}

	public string prefabId
	{
		get
		{
			return this._characterStatus.prefabId;
		}
	}

	public int[] chipIds
	{
		get
		{
			return this._characterStatus.chipIds;
		}
	}

	public void SetCharacterState(CharacterStateControlStore savedCSC, BattleStateData battleStateData)
	{
		this._currentHp = savedCSC.hp;
		this.ap = savedCSC.ap;
		this.isSelectSkill = savedCSC.isSelectSkill;
		this.hate = savedCSC.hate;
		this.previousHate = savedCSC.previousHate;
		this.isLeader = savedCSC.isLeader;
		this.skillOrder = savedCSC.skillOrder;
		this.myIndex = savedCSC.myIndex;
		this.isEnemy = savedCSC.isEnemy;
		this.SetCurrentSufferState(savedCSC.currentSufferState, battleStateData);
		this.randomedSpeed = savedCSC.randomedSpeed;
		this.SetChipEffectCount(savedCSC.chipEffectCount);
		this.SetPotencyChipIdList(savedCSC.potencyChipIdList);
		foreach (int num in this.potencyChipIdList.Keys)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num.ToString());
			if (chipEffectDataToId != null)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
				{
					chipEffectDataToId
				};
				this.AddChipParam(true, chipEffects, true, false);
			}
		}
		this.isLoad = true;
	}

	public bool isHavingLeaderSkill
	{
		get
		{
			return this.leaderSkillStatus.isHaving;
		}
	}

	public int maxAp
	{
		get
		{
			if (!this._sharedStatus.isShared)
			{
				return 5;
			}
			return 16;
		}
	}

	public int defaultMaxHp
	{
		get
		{
			return this._characterStatus.maxHp;
		}
	}

	public int isSelectSkill
	{
		get
		{
			return this._isSelectSkill;
		}
		set
		{
			this._isSelectSkill = value;
		}
	}

	public float randomedSpeed
	{
		get
		{
			return this._randomedSpeed;
		}
		set
		{
			this._randomedSpeed = value;
		}
	}

	public int hate
	{
		get
		{
			return this._hate;
		}
		set
		{
			this._hate = value;
		}
	}

	public int previousHate
	{
		get
		{
			return this._previousHate;
		}
		set
		{
			this._previousHate = value;
		}
	}

	public bool isLeader
	{
		get
		{
			return this._isLeader;
		}
		set
		{
			this._isLeader = value;
		}
	}

	public int skillOrder
	{
		get
		{
			return this._skillOrder;
		}
		set
		{
			this._skillOrder = value;
		}
	}

	public int myIndex
	{
		get
		{
			return this._myIndex;
		}
		set
		{
			this._myIndex = value;
		}
	}

	public bool isEnemy
	{
		get
		{
			return this._isEnemy;
		}
		set
		{
			this._isEnemy = value;
		}
	}

	public CharacterStateControl targetCharacter
	{
		get
		{
			return this._targetCharacter;
		}
		set
		{
			this._targetCharacter = value;
		}
	}

	public List<ItemDropResult> itemDropResult
	{
		get
		{
			return this._itemDropResult;
		}
		private set
		{
			this._itemDropResult = value;
		}
	}

	public List<string> autoCounterSkillList { get; set; }

	public string chipSkillId { get; set; }

	public string currentChipId { get; set; }

	public SkillStatus currentSkillStatus
	{
		get
		{
			if (this._isSelectSkill < 0 || this._isSelectSkill >= this._skillStatus.Length)
			{
				return null;
			}
			return this._skillStatus[this.isSelectSkill];
		}
	}

	public bool IsSelectedSkill
	{
		get
		{
			return this.isSelectSkill != -1;
		}
	}

	public virtual int GetSkillLength()
	{
		return this.skillIds.Length;
	}

	public int SkillIdToIndexOf(string SkillId)
	{
		List<string> list = new List<string>(this.skillIds);
		return list.IndexOf(SkillId);
	}

	public SkillStatus[] GetRemovedAttackSkillStatus(string attackSkillId)
	{
		List<string> list = new List<string>(this.skillIds);
		int num = list.IndexOf(attackSkillId);
		if (num < 0)
		{
			return this.skillStatus;
		}
		list.RemoveAt(num);
		List<SkillStatus> list2 = new List<SkillStatus>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(this.skillStatus[this.SkillIdToIndexOf(list[i])]);
		}
		return list2.ToArray();
	}

	public bool IsInheritanceSkill(SkillStatus status)
	{
		List<SkillStatus> list = new List<SkillStatus>(this.skillStatus);
		int num = list.IndexOf(status);
		if (num < 0)
		{
			global::Debug.LogError("存在しないスキルです (" + status.ToString() + ")");
			return false;
		}
		string value = this.skillIds[num];
		return this.playerStatus.inheritanceTechniqueId.Equals(value);
	}

	private string CheckNull(SufferStateProperty ssp)
	{
		if (ssp == null || !ssp.isActive)
		{
			return string.Empty;
		}
		string @params = ssp.GetParams();
		string strVal = @params + "#" + ((!ssp.isActive) ? "0" : "1");
		SufferStatePropertyStore value = new SufferStatePropertyStore
		{
			strVal = strVal,
			triggerCharacter = ssp.GetTriggerCharacter()
		};
		return JsonWriter.Serialize(value);
	}

	public HaveSufferStateStore GetCurrentSufferState()
	{
		HaveSufferState currentSufferState = this.currentSufferState;
		HaveSufferStateStore haveSufferStateStore = new HaveSufferStateStore
		{
			onPoison = this.CheckNull(currentSufferState.onPoison),
			onConfusion = this.CheckNull(currentSufferState.onConfusion),
			onParalysis = this.CheckNull(currentSufferState.onParalysis),
			onSleep = this.CheckNull(currentSufferState.onSleep),
			onStun = this.CheckNull(currentSufferState.onStun),
			onSkillLock = this.CheckNull(currentSufferState.onSkillLock),
			onAttackUp = this.CheckNull(currentSufferState.onAttackUp),
			onAttackDown = this.CheckNull(currentSufferState.onAttackDown),
			onDefenceUp = this.CheckNull(currentSufferState.onDefenceUp),
			onDefenceDown = this.CheckNull(currentSufferState.onDefenceDown),
			onSpAttackUp = this.CheckNull(currentSufferState.onSpAttackUp),
			onSpAttackDown = this.CheckNull(currentSufferState.onSpAttackDown),
			onSpDefenceUp = this.CheckNull(currentSufferState.onSpDefenceUp),
			onSpDefenceDown = this.CheckNull(currentSufferState.onSpDefenceDown),
			onSpeedUp = this.CheckNull(currentSufferState.onSpeedUp),
			onSpeedDown = this.CheckNull(currentSufferState.onSpeedDown),
			onCounter = this.CheckNull(currentSufferState.onCounter),
			onReflection = this.CheckNull(currentSufferState.onReflection),
			onProtect = this.CheckNull(currentSufferState.onProtect),
			onPowerCharge = this.CheckNull(currentSufferState.onPowerCharge),
			onHitRateUp = this.CheckNull(currentSufferState.onHitRateUp),
			onHitRateDown = this.CheckNull(currentSufferState.onHitRateDown),
			onSatisfactionRateUp = this.CheckNull(currentSufferState.onSatisfactionRateUp),
			onSatisfactionRateDown = this.CheckNull(currentSufferState.onSatisfactionRateDown),
			onTurnBarrier = this.CheckNull(currentSufferState.onTurnBarrier),
			onCountBarrier = this.CheckNull(currentSufferState.onCountBarrier),
			onDamageRateUp = this.CheckNull(currentSufferState.onDamageRateUp),
			onDamageRateDown = this.CheckNull(currentSufferState.onDamageRateDown)
		};
		int[] array = new int[currentSufferState.sufferOrderList.Count];
		int num = 0;
		foreach (SufferStateProperty.SufferType sufferType in currentSufferState.sufferOrderList)
		{
			array[num] = sufferType.ToInteger();
			num++;
		}
		haveSufferStateStore.sufferOrderList = array;
		return haveSufferStateStore;
	}

	public void SetCurrentSufferState(HaveSufferStateStore haveSufferStateStore, BattleStateData battleStateData)
	{
		HaveSufferState haveSufferState = new HaveSufferState();
		haveSufferState.onPoison.SetParams(haveSufferStateStore.onPoison, battleStateData);
		haveSufferState.onConfusion.SetParams(haveSufferStateStore.onConfusion, battleStateData);
		haveSufferState.onParalysis.SetParams(haveSufferStateStore.onParalysis, battleStateData);
		haveSufferState.onSleep.SetParams(haveSufferStateStore.onSleep, battleStateData);
		haveSufferState.onStun.SetParams(haveSufferStateStore.onStun, battleStateData);
		haveSufferState.onSkillLock.SetParams(haveSufferStateStore.onSkillLock, battleStateData);
		haveSufferState.onAttackUp.SetParams(haveSufferStateStore.onAttackUp, battleStateData);
		haveSufferState.onAttackDown.SetParams(haveSufferStateStore.onAttackDown, battleStateData);
		haveSufferState.onDefenceUp.SetParams(haveSufferStateStore.onDefenceUp, battleStateData);
		haveSufferState.onDefenceDown.SetParams(haveSufferStateStore.onDefenceDown, battleStateData);
		haveSufferState.onSpAttackUp.SetParams(haveSufferStateStore.onSpAttackUp, battleStateData);
		haveSufferState.onSpAttackDown.SetParams(haveSufferStateStore.onSpAttackDown, battleStateData);
		haveSufferState.onSpDefenceUp.SetParams(haveSufferStateStore.onSpDefenceUp, battleStateData);
		haveSufferState.onSpDefenceDown.SetParams(haveSufferStateStore.onSpDefenceDown, battleStateData);
		haveSufferState.onSpeedUp.SetParams(haveSufferStateStore.onSpeedUp, battleStateData);
		haveSufferState.onSpeedDown.SetParams(haveSufferStateStore.onSpeedDown, battleStateData);
		haveSufferState.onCounter.SetParams(haveSufferStateStore.onCounter, battleStateData);
		haveSufferState.onReflection.SetParams(haveSufferStateStore.onReflection, battleStateData);
		haveSufferState.onProtect.SetParams(haveSufferStateStore.onProtect, battleStateData);
		haveSufferState.onPowerCharge.SetParams(haveSufferStateStore.onPowerCharge, battleStateData);
		haveSufferState.onHitRateUp.SetParams(haveSufferStateStore.onHitRateUp, battleStateData);
		haveSufferState.onHitRateDown.SetParams(haveSufferStateStore.onHitRateDown, battleStateData);
		haveSufferState.onSatisfactionRateUp.SetParams(haveSufferStateStore.onSatisfactionRateUp, battleStateData);
		haveSufferState.onSatisfactionRateDown.SetParams(haveSufferStateStore.onSatisfactionRateDown, battleStateData);
		haveSufferState.onTurnBarrier.SetParams(haveSufferStateStore.onTurnBarrier, battleStateData);
		haveSufferState.onCountBarrier.SetParams(haveSufferStateStore.onCountBarrier, battleStateData);
		haveSufferState.onDamageRateUp.SetParams(haveSufferStateStore.onDamageRateUp, battleStateData);
		haveSufferState.onDamageRateDown.SetParams(haveSufferStateStore.onDamageRateDown, battleStateData);
		List<SufferStateProperty.SufferType> list = new List<SufferStateProperty.SufferType>();
		foreach (int item in haveSufferStateStore.sufferOrderList)
		{
			list.Add((SufferStateProperty.SufferType)item);
		}
		haveSufferState.sufferOrderList = list;
		this.currentSufferState = haveSufferState;
	}

	private void SetChipEffectCount(string chiopCount)
	{
		string[] array = chiopCount.Split(",".ToCharArray());
		foreach (string text in array)
		{
			string[] array3 = text.Split(":".ToCharArray());
			if (array3.Length > 1)
			{
				int key = int.Parse(array3[0]);
				int value = int.Parse(array3[1]);
				if (this.chipEffectCount.ContainsKey(key))
				{
					this.chipEffectCount[key] = value;
				}
			}
		}
	}

	public string GetChipEffectCountToString()
	{
		string text = string.Empty;
		int num = 0;
		foreach (int key in this.chipEffectCount.Keys)
		{
			if (num > 0)
			{
				text += ",";
			}
			text = text + key.ToString() + ":" + this.chipEffectCount[key].ToString();
			num++;
		}
		return text;
	}

	private void SetPotencyChipIdList(string chipIdList)
	{
		string[] array = chipIdList.Split(",".ToCharArray());
		foreach (string text in array)
		{
			string[] array3 = text.Split(":".ToCharArray());
			if (array3.Length > 1)
			{
				int key = int.Parse(array3[0]);
				int value = int.Parse(array3[1]);
				if (this.potencyChipIdList.ContainsKey(key))
				{
					this.potencyChipIdList[key] = value;
				}
			}
		}
	}

	public string GetPotencyChipIdListToString()
	{
		string text = string.Empty;
		int num = 0;
		foreach (int key in this.potencyChipIdList.Keys)
		{
			if (num > 0)
			{
				text += ",";
			}
			text = text + key.ToString() + ":" + this.potencyChipIdList[key].ToString();
			num++;
		}
		return text;
	}

	public float hpRemingAmount01
	{
		get
		{
			return (float)this.hp / (float)this.maxHp;
		}
	}

	public bool GetHpRemingAmoutRange(float minRange, float maxRange)
	{
		return this.hpRemingAmount01 >= minRange && this.hpRemingAmount01 <= maxRange;
	}

	private void StatusInitialize(string attackSkillId)
	{
		this.currentSufferState = new HaveSufferState();
		this._maxHp = this._characterStatus.maxHp + Mathf.FloorToInt((float)this.defaultMaxHp * this.leaderSkillResult.hpUpPercent);
		this._currentHp = this._maxHp;
		this.m_extraMaxHp = this._maxHp;
		List<string> list;
		if (PlayerStatus.Match(this._characterStatus))
		{
			list = new List<string>(this.playerStatus.skillIds);
		}
		else
		{
			list = new List<string>(this.enemyStatus.enemyAiPattern.GetAllSkillID());
			this.ItemDrop();
		}
		if (!list.Contains(attackSkillId))
		{
			list.Insert(0, attackSkillId);
		}
		this.skillIds = list.ToArray();
		this.skillStatus = new SkillStatus[this.skillIds.Length];
		this.InitializeChipEffectCount();
		if (this._sharedStatus.isShared)
		{
			this.ap += 5;
		}
		else
		{
			this.ap = this.maxAp;
		}
		this.HateReset();
		this.SpeedRandomize(false);
		this.currentSufferState = new HaveSufferState();
		this.isDiedJustBefore = false;
	}

	public void InitializeSpecialCorrectionStatus()
	{
		if (!this.isInitSpecialCorrectionStatus)
		{
			if (!this.isDied)
			{
				this.InitializePotencyChipIdList();
			}
			this.InitializeExtraStatus();
			this.isInitSpecialCorrectionStatus = true;
			if (!this.isLoad)
			{
				this._currentHp = this.maxHp;
			}
			else
			{
				this.OnChipTrigger(ChipEffectStatus.EffectTriggerType.HpPercentage, false);
				this.OnChipTrigger(ChipEffectStatus.EffectTriggerType.HpFixed, false);
			}
		}
	}

	private void InitializeExtraStatus()
	{
		int baseValue = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.attackPower : this.playerStatus.friendshipStatus.GetCorrectedAttackPower(this._characterStatus.attackPower);
		int baseValue2 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.defencePower : this.playerStatus.friendshipStatus.GetCorrectedDefencePower(this._characterStatus.defencePower);
		int baseValue3 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.specialAttackPower : this.playerStatus.friendshipStatus.GetCorrectedSpecialAttackPower(this._characterStatus.specialAttackPower);
		int baseValue4 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.specialDefencePower : this.playerStatus.friendshipStatus.GetCorrectedSpecialDefencePower(this._characterStatus.specialDefencePower);
		int baseValue5 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.speed : this.playerStatus.friendshipStatus.GetCorrectedSpeed(this._characterStatus.speed);
		this.m_extraMaxHp = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, this._maxHp, this, ExtraEffectStatus.ExtraEffectType.Hp);
		this.m_extraAttackPower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, baseValue, this, ExtraEffectStatus.ExtraEffectType.Atk);
		this.m_extraDefencePower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, baseValue2, this, ExtraEffectStatus.ExtraEffectType.Def);
		this.m_extraSpecialAttackPower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, baseValue3, this, ExtraEffectStatus.ExtraEffectType.Satk);
		this.m_extraSpecialDefencePower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, baseValue4, this, ExtraEffectStatus.ExtraEffectType.Sdef);
		this.m_extraSpeed = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, baseValue5, this, ExtraEffectStatus.ExtraEffectType.Speed);
		this.m_defaultExtraAttackPower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, this._characterStatus.attackPower, this, ExtraEffectStatus.ExtraEffectType.Atk);
		this.m_defaultExtraDefencePower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, this._characterStatus.defencePower, this, ExtraEffectStatus.ExtraEffectType.Def);
		this.m_defaultExtraSpecialAttackPower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, this._characterStatus.specialAttackPower, this, ExtraEffectStatus.ExtraEffectType.Satk);
		this.m_defaultExtraSpecialDefencePower = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, this._characterStatus.specialDefencePower, this, ExtraEffectStatus.ExtraEffectType.Sdef);
		this.m_defaultExtraSpeed = ExtraEffectStatus.GetExtraEffectValue(BattleStateManager.current.battleStateData.extraEffectStatus, this._characterStatus.speed, this, ExtraEffectStatus.ExtraEffectType.Speed);
	}

	public void InitializeSkillExtraStatus()
	{
		if (this.skillStatus.Length > 1)
		{
			AffectEffectProperty affectEffectFirst = this.skillStatus[1].GetAffectEffectFirst();
			if (affectEffectFirst != null)
			{
				if (affectEffectFirst.type == AffectEffect.Damage)
				{
					this.m_extraDeathblowSkillPower = ExtraEffectStatus.GetSkillPowerCorrectionValue(BattleStateManager.current.battleStateData.extraEffectStatus, affectEffectFirst, this);
				}
				this.m_extraDeathblowSkillHitRate = ExtraEffectStatus.GetSkillHitRateCorrectionValue(BattleStateManager.current.battleStateData.extraEffectStatus, affectEffectFirst, this);
			}
		}
		if (this.skillStatus.Length > 2)
		{
			AffectEffectProperty affectEffectFirst2 = this.skillStatus[2].GetAffectEffectFirst();
			if (affectEffectFirst2 != null)
			{
				if (affectEffectFirst2.type == AffectEffect.Damage)
				{
					this.m_extraInheritanceSkillPower = ExtraEffectStatus.GetSkillPowerCorrectionValue(BattleStateManager.current.battleStateData.extraEffectStatus, affectEffectFirst2, this);
				}
				this.m_extraInheritanceSkillHitRate = ExtraEffectStatus.GetSkillHitRateCorrectionValue(BattleStateManager.current.battleStateData.extraEffectStatus, affectEffectFirst2, this);
			}
		}
	}

	private void InitializeChipEffectCount()
	{
		foreach (int num in this._characterStatus.chipIds)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			if (chipEffectData != null)
			{
				foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
				{
					if (chipEffect.effectTurn.ToInt32() > 0)
					{
						this.chipEffectCount[chipEffect.chipEffectId.ToInt32()] = chipEffect.effectTurn.ToInt32();
					}
				}
			}
		}
	}

	private void InitializePotencyChipIdList()
	{
		foreach (int num in this._characterStatus.chipIds)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			if (chipEffectData != null)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipEffectData, ChipEffectStatus.EffectTriggerType.Usually, this.prefabId.ToInt32(), this, 0);
				this.AddChipParam(true, this.GetAddChipParamEffects(invocationList, true).ToArray(), false, false);
				int triggerValue = 0;
				if (BattleStateManager.current != null)
				{
					triggerValue = BattleStateManager.current.hierarchyData.areaId;
				}
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList2 = ChipEffectStatus.GetInvocationList(chipEffectData, ChipEffectStatus.EffectTriggerType.Area, this.prefabId.ToInt32(), this, triggerValue);
				this.AddChipParam(true, this.GetAddChipParamEffects(invocationList2, true).ToArray(), true, true);
			}
		}
	}

	private List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetAddChipParamEffects(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipInvocationEffects, bool isInit = false)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipInvocationEffects)
		{
			int num = chipEffect.monsterGroupId.ToInt32();
			if (num <= 0 || num == this.prefabId.ToInt32())
			{
				if (chipEffect.effectType.ToInt32() != 57 && (!isInit || chipEffect.effectType.ToInt32() != 56))
				{
					int num2 = chipEffect.chipEffectId.ToInt32();
					if (!this.potencyChipIdList.ContainsKey(num2))
					{
						int num3 = 10000;
						if (chipEffect.lot != null && chipEffect.lot.Length > 0)
						{
							num3 = chipEffect.lot.ToInt32();
						}
						if (num3 > 0)
						{
							if (num3 != 10000)
							{
								int num4 = UnityEngine.Random.Range(0, 10000);
								if (num4 > num3)
								{
									goto IL_1CD;
								}
							}
							if (this.chipEffectCount.ContainsKey(num2))
							{
								if (this.chipEffectCount[num2] <= 0)
								{
									goto IL_1CD;
								}
								Dictionary<int, int> dictionary2;
								Dictionary<int, int> dictionary = dictionary2 = this.chipEffectCount;
								int num5;
								int key = num5 = num2;
								num5 = dictionary2[num5];
								dictionary[key] = num5 - 1;
							}
							if (chipEffect.effectType.ToInt32() == 58)
							{
								this.IsHittingTheTargetChip = true;
							}
							list.Add(chipEffect);
							if (chipEffect.effectType.ToInt32() != 60 && chipEffect.effectType.ToInt32() != 56)
							{
								this.potencyChipIdList.Add(num2, chipEffect.targetType.ToInt32());
							}
							if (!this.stagingChipIdList.ContainsKey(num2))
							{
								this.stagingChipIdList.Add(num2, chipEffect.chipId.ToInt32());
							}
						}
					}
				}
			}
			IL_1CD:;
		}
		return list;
	}

	public void InitializeAp()
	{
		if (!CharacterStateControl.onUseRevivalMaxAp)
		{
			this.ap = 3;
		}
		else
		{
			this.ap = 5;
		}
		this.upAp = 0;
	}

	public bool StartMyRoundApUp()
	{
		this.upAp = 0;
		bool flag = this.ap == this.maxAp;
		if (!flag)
		{
			this.ap++;
			this.upAp++;
		}
		return !flag;
	}

	public void ApZero()
	{
		if (!this.sharedStatus.isShared)
		{
			this.ap = 0;
		}
	}

	public void HateReset()
	{
		this.previousHate = this.hate;
		if (this.isLeader)
		{
			this.hate = 30;
		}
		else
		{
			this.hate = 10;
		}
	}

	public void SpeedRandomize(bool onEnableSpeedRandomize)
	{
		this.randomedSpeed = (float)this.speed + (float)this.speed * this.leaderSkillResult.speedUpPercent;
		if (onEnableSpeedRandomize)
		{
			this.randomedSpeed *= UnityEngine.Random.Range(0.9f, 1f);
		}
	}

	public void Kill()
	{
		this.hp -= this.hp;
	}

	public void OnHitDestruct()
	{
		this.hp = 1;
	}

	public void Revival()
	{
		if (this._sharedStatus.isShared)
		{
			this.ap += 5;
		}
		else
		{
			this.ap = this.maxAp;
		}
		this.HateReset();
		this.SpeedRandomize(false);
		this.hp = this.maxHp;
		this.currentSufferState = new HaveSufferState();
		this.isDiedJustBefore = false;
		this.isInitSpecialCorrectionStatus = false;
		this.InitializeSpecialCorrectionStatus();
		this.OnChipTrigger(ChipEffectStatus.EffectTriggerType.HpPercentage, true);
		this.OnChipTrigger(ChipEffectStatus.EffectTriggerType.HpFixed, false);
	}

	public void ItemDrop()
	{
		this.itemDropResult = this.enemyStatus.itemDropResult;
	}

	public void LeaderInitialize()
	{
		if (!this.isLeader)
		{
			return;
		}
		this.HateReset();
	}

	public bool IsHpLess(int border)
	{
		int num = this.hp / this.maxHp * 100;
		return num < border;
	}

	public bool isDied
	{
		get
		{
			return this.hp == 0;
		}
	}

	public bool isDiedJustBefore
	{
		get
		{
			return this._isDiedJustBefore;
		}
		set
		{
			this._isDiedJustBefore = value;
		}
	}

	public bool isAllApShortness()
	{
		for (int i = 0; i < this.skillStatus.Length; i++)
		{
			if (!this.isApShortness(i))
			{
				return false;
			}
		}
		return true;
	}

	public bool isApShortness()
	{
		return this.isApShortness(this.isSelectSkill);
	}

	public bool isApShortness(int index)
	{
		if (this.skillStatus.Length <= index)
		{
			global::Debug.LogError("Skill Params Out: " + index);
			return false;
		}
		return index != -1 && this.ap < this.skillStatus[index].GetCorrectedAp(this);
	}

	public bool[] ApShortnesses()
	{
		bool[] array = new bool[this.skillStatus.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.isApShortness(i);
		}
		return array;
	}

	public bool isSelectingManualSelectTarget()
	{
		return this.targetCharacter != null;
	}

	public void RandomSelectSkill()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.skillStatus.Length; i++)
		{
			if (this.ap >= this.skillStatus[this.isSelectSkill].needAp)
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			this.isSelectSkill = list[UnityEngine.Random.Range(0, list.Count)];
		}
		else
		{
			this.isSelectSkill = 0;
		}
	}

	public bool WaveCountInitialize(float hpRevivalLevel)
	{
		bool result = true;
		int num = Mathf.FloorToInt((float)this.maxHp * hpRevivalLevel);
		if (this.hp == this.maxHp || num == 0)
		{
			result = false;
		}
		this._currentHp = Mathf.Clamp(this._currentHp + num, 1, this.maxHp);
		return result;
	}

	public bool IsMatchLocation(CharacterStateControl c)
	{
		return c.myIndex == this.myIndex && c.isEnemy == this.isEnemy;
	}

	public int GetDifferenceExtraPram()
	{
		int result = 0;
		int num = this.extraMaxHp - this.defaultExtraMaxHp;
		if (num > 0)
		{
			return num;
		}
		if (num < 0)
		{
			result = num;
		}
		num = this.defaultExtraAttackPower - (this.characterStatus.attackPower + this.m_chipAddAttackPower);
		if (num > 0)
		{
			return num;
		}
		if (num < 0)
		{
			result = num;
		}
		num = this.defaultExtraDefencePower - (this.characterStatus.defencePower + this.m_chipAddDefencePower);
		if (num > 0)
		{
			return num;
		}
		if (num < 0)
		{
			result = num;
		}
		num = this.defaultExtraSpecialAttackPower - (this.characterStatus.specialAttackPower + this.m_chipAddSpecialAttackPower);
		if (num > 0)
		{
			return num;
		}
		if (num < 0)
		{
			result = num;
		}
		num = this.defaultExtraSpecialDefencePower - (this.characterStatus.specialDefencePower + this.m_chipAddSpecialDefencePower);
		if (num > 0)
		{
			return num;
		}
		if (num < 0)
		{
			result = num;
		}
		num = this.defaultExtraSpeed - (this.characterStatus.speed + this.m_chipAddSpeed);
		if (num > 0)
		{
			return num;
		}
		if (num < 0)
		{
			result = num;
		}
		if (this.skillStatus.Length > 1)
		{
			AffectEffectProperty affectEffectFirst = this.skillStatus[1].GetAffectEffectFirst();
			if (affectEffectFirst != null)
			{
				if (affectEffectFirst.type == AffectEffect.Damage)
				{
					if (this.m_extraDeathblowSkillPower - affectEffectFirst.power > 0)
					{
						return 1;
					}
					if (this.m_extraDeathblowSkillPower - affectEffectFirst.power < 0)
					{
						result = -1;
					}
				}
				if (this.m_extraDeathblowSkillHitRate - affectEffectFirst.hitRate > 0f)
				{
					return 1;
				}
				if (this.m_extraDeathblowSkillHitRate - affectEffectFirst.hitRate < 0f)
				{
					result = -1;
				}
			}
		}
		if (this.skillStatus.Length > 2)
		{
			AffectEffectProperty affectEffectFirst2 = this.skillStatus[2].GetAffectEffectFirst();
			if (affectEffectFirst2 != null)
			{
				if (affectEffectFirst2.type == AffectEffect.Damage)
				{
					if (this.m_extraInheritanceSkillPower - affectEffectFirst2.power > 0)
					{
						return 1;
					}
					if (this.m_extraInheritanceSkillPower - affectEffectFirst2.power < 0)
					{
						result = -1;
					}
				}
				if (this.m_extraInheritanceSkillHitRate - affectEffectFirst2.hitRate > 0f)
				{
					return 1;
				}
				if (this.m_extraInheritanceSkillHitRate - affectEffectFirst2.hitRate < 0f)
				{
					result = -1;
				}
			}
		}
		return result;
	}

	public void OnChipTrigger(ChipEffectStatus.EffectTriggerType triggerType, bool isRecovery = false)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> chipTriggerList = this.GetChipTriggerList(triggerType);
		if (chipTriggerList.Count == 0)
		{
			return;
		}
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		int triggerValue = 0;
		if (triggerType == ChipEffectStatus.EffectTriggerType.HpPercentage)
		{
			triggerValue = (int)((float)this.hp / (float)this.maxHp * 100f);
		}
		else if (triggerType == ChipEffectStatus.EffectTriggerType.HpFixed)
		{
			triggerValue = this.hp;
		}
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipTriggerList.ToArray(), triggerType, this.prefabId.ToInt32(), this, triggerValue);
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipTriggerList)
		{
			bool flag = false;
			foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect2 in invocationList)
			{
				if (chipEffect.chipEffectId == chipEffect2.chipEffectId)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				int key = chipEffect.chipEffectId.ToInt32();
				if (this.potencyChipIdList.ContainsKey(key))
				{
					list.Add(chipEffect);
					this.potencyChipIdList.Remove(key);
					if (chipEffect.effectType.ToInt32() == 58)
					{
						this.IsHittingTheTargetChip = false;
					}
				}
			}
		}
		bool isInit = true;
		if (BattleStateManager.current != null && BattleStateManager.current.battleStateData != null)
		{
			isInit = BattleStateManager.current.battleStateData.IsChipSkill();
		}
		if (isRecovery)
		{
			isInit = true;
		}
		this.AddChipParam(true, this.GetAddChipParamEffects(invocationList, isInit).ToArray(), true, false);
		if (list.Count > 0)
		{
			this.AddChipParam(false, list.ToArray(), true, false);
		}
	}

	private List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetChipTriggerList(ChipEffectStatus.EffectTriggerType triggerType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		if (!this.triggerChips.ContainsKey(triggerType))
		{
			foreach (int num in this._characterStatus.chipIds)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
				if (chipEffectData != null)
				{
					foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
					{
						int num2 = 0;
						if (chipEffect.monsterGroupId != null && chipEffect.monsterGroupId.Length > 0)
						{
							num2 = chipEffect.monsterGroupId.ToInt32();
						}
						if (num2 <= 0 || num2 == this.prefabId.ToInt32())
						{
							if (chipEffect.effectTrigger.ToInt32() == (int)triggerType)
							{
								list.Add(chipEffect);
							}
						}
					}
				}
			}
			this.triggerChips.Add(triggerType, list);
		}
		else
		{
			list = this.triggerChips[triggerType];
		}
		return list;
	}

	private void RemoveDeadStagingChips()
	{
		if (BattleStateManager.current != null)
		{
			int num = 0;
			if (this.isEnemy)
			{
				foreach (CharacterStateControl characterStateControl in BattleStateManager.current.battleStateData.enemies)
				{
					if (!characterStateControl.isDied)
					{
						num++;
					}
				}
			}
			else
			{
				foreach (CharacterStateControl characterStateControl2 in BattleStateManager.current.battleStateData.playerCharacters)
				{
					if (!characterStateControl2.isDied)
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				if (this.isEnemy)
				{
					foreach (CharacterStateControl characterStateControl3 in BattleStateManager.current.battleStateData.enemies)
					{
						characterStateControl3.stagingChipIdList.Clear();
					}
				}
				else
				{
					foreach (CharacterStateControl characterStateControl4 in BattleStateManager.current.battleStateData.playerCharacters)
					{
						characterStateControl4.stagingChipIdList.Clear();
					}
				}
			}
		}
	}

	private void AddChipParam(bool isAdd, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, bool isAll = true, bool isArea = false)
	{
		int baseValue = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.attackPower : this.playerStatus.friendshipStatus.GetCorrectedAttackPower(this._characterStatus.attackPower);
		int baseValue2 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.defencePower : this.playerStatus.friendshipStatus.GetCorrectedDefencePower(this._characterStatus.defencePower);
		int baseValue3 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.specialAttackPower : this.playerStatus.friendshipStatus.GetCorrectedSpecialAttackPower(this._characterStatus.specialAttackPower);
		int baseValue4 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.specialDefencePower : this.playerStatus.friendshipStatus.GetCorrectedSpecialDefencePower(this._characterStatus.specialDefencePower);
		int baseValue5 = (!PlayerStatus.Match(this._characterStatus)) ? this._characterStatus.speed : this.playerStatus.friendshipStatus.GetCorrectedSpeed(this._characterStatus.speed);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = (!isAdd) ? -1 : 1;
		if (isAll)
		{
			num = ChipEffectStatus.GetChipEffectValue(chipEffects, this.defaultMaxHp, this, ExtraEffectStatus.ExtraEffectType.Hp);
			num2 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue, this, ExtraEffectStatus.ExtraEffectType.Atk);
			num3 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue2, this, ExtraEffectStatus.ExtraEffectType.Def);
			num4 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue3, this, ExtraEffectStatus.ExtraEffectType.Satk);
			num5 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue4, this, ExtraEffectStatus.ExtraEffectType.Sdef);
			num6 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue5, this, ExtraEffectStatus.ExtraEffectType.Speed);
		}
		float chipEffectValueToFloat = ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, 0f, this, ExtraEffectStatus.ExtraEffectType.Critical);
		float chipEffectValueToFloat2 = ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, 0f, this, ExtraEffectStatus.ExtraEffectType.Hit);
		if (isArea)
		{
			this.m_stageChipAddMaxHp += num * num7;
			this.m_stageChipAddAttackPower += num2 * num7;
			this.m_stageChipAddDefencePower += num3 * num7;
			this.m_stageChipAddSpecialAttackPower += num4 * num7;
			this.m_stageChipAddSpecialDefencePower += num5 * num7;
		}
		else
		{
			this.m_chipAddMaxHp += num * num7;
			this.m_chipAddAttackPower += num2 * num7;
			this.m_chipAddDefencePower += num3 * num7;
			this.m_chipAddSpecialAttackPower += num4 * num7;
			this.m_chipAddSpecialDefencePower += num5 * num7;
		}
		this.m_chipAddSpeed += num6 * num7;
		this.m_chipAddCritical += chipEffectValueToFloat * (float)num7;
		this.m_chipAddHit += chipEffectValueToFloat2 * (float)num7;
	}

	private void JudgeGuts(int currentHp, int maxHp)
	{
		int num = (int)((float)currentHp / (float)maxHp * 100f);
		foreach (int num2 in this._characterStatus.chipIds)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num2.ToString());
			if (chipEffectData != null)
			{
				foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
				{
					int num3 = chipEffect.monsterGroupId.ToInt32();
					if (num3 <= 0 || num3 == this.prefabId.ToInt32())
					{
						if (chipEffect.effectType.ToInt32() == 57)
						{
							bool flag = false;
							if (chipEffect.effectTrigger.ToInt32() == 5)
							{
								string[] array2 = chipEffect.effectTriggerValue.Split(new char[]
								{
									','
								});
								if (array2.Length == 2 && array2[0].ToInt32() <= num && array2[1].ToInt32() >= num)
								{
									flag = true;
								}
							}
							else if (chipEffect.effectTrigger.ToInt32() == 7)
							{
								if (chipEffect.effectTriggerValue.ToInt32() == num)
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
							int num4 = chipEffect.chipEffectId.ToInt32();
							if (flag)
							{
								if (this.chipEffectCount.ContainsKey(num2))
								{
									if (this.chipEffectCount[num4] > 0)
									{
										Dictionary<int, int> dictionary2;
										Dictionary<int, int> dictionary = dictionary2 = this.chipEffectCount;
										int num5;
										int key = num5 = num4;
										num5 = dictionary2[num5];
										dictionary[key] = num5 - 1;
										this._currentHp = Mathf.Clamp(chipEffect.effectValue.ToInt32(), 1, maxHp);
										if (!this.stagingChipIdList.ContainsKey(num4))
										{
											this.stagingChipIdList.Add(num4, num2);
										}
									}
								}
								else
								{
									this._currentHp = Mathf.Clamp(chipEffect.effectValue.ToInt32(), 1, maxHp);
									if (!this.stagingChipIdList.ContainsKey(num4))
									{
										this.stagingChipIdList.Add(num4, num2);
									}
								}
								return;
							}
						}
					}
				}
			}
		}
	}

	public void AddChipEffectCount(int chipEffectId, int value)
	{
		if (this.chipEffectCount.ContainsKey(chipEffectId))
		{
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = dictionary2 = this.chipEffectCount;
			int num = dictionary2[chipEffectId];
			dictionary[chipEffectId] = num + value;
		}
	}

	private static int CompareHpBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x.hp > y.hp)
		{
			return -1;
		}
		if (x.hp < y.hp)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareHpRangeBase(CharacterStateControl x, CharacterStateControl y, float minRange, float maxRange)
	{
		if (x == y)
		{
			return 0;
		}
		if (!x.GetHpRemingAmoutRange(minRange, maxRange))
		{
			return 1;
		}
		if (!y.GetHpRemingAmoutRange(minRange, maxRange))
		{
			return -1;
		}
		return CharacterStateControl.CompareHpBase(x, y);
	}

	private static int CompareAttackBase(CharacterStateControl x, CharacterStateControl y, bool checkBath = true)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.attackPower;
		float num2 = (float)y.attackPower;
		if (checkBath)
		{
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackUp))
			{
				num += (float)x.attackPower * x.currentSufferState.onAttackUp.upPercent;
			}
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackDown))
			{
				num -= (float)x.attackPower * x.currentSufferState.onAttackDown.downPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackUp))
			{
				num2 += (float)y.attackPower * y.currentSufferState.onAttackUp.upPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackDown))
			{
				num2 -= (float)y.attackPower * y.currentSufferState.onAttackDown.downPercent;
			}
		}
		num += (float)x.attackPower * x.leaderSkillResult.attackUpPercent;
		num2 += (float)y.attackPower * y.leaderSkillResult.attackUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareDefenceBase(CharacterStateControl x, CharacterStateControl y, bool checkBath = true)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.defencePower;
		float num2 = (float)y.defencePower;
		if (checkBath)
		{
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.DefenceUp))
			{
				num += (float)x.defencePower * x.currentSufferState.onDefenceUp.upPercent;
			}
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.DefenceDown))
			{
				num -= (float)x.defencePower * x.currentSufferState.onDefenceDown.downPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.DefenceUp))
			{
				num2 += (float)y.defencePower * y.currentSufferState.onDefenceUp.upPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.DefenceDown))
			{
				num2 -= (float)y.defencePower * y.currentSufferState.onDefenceDown.downPercent;
			}
		}
		num += (float)x.defencePower * x.leaderSkillResult.defenceUpPercent;
		num2 += (float)y.defencePower * y.leaderSkillResult.defenceUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareSpecialAttackBase(CharacterStateControl x, CharacterStateControl y, bool checkBath = true)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.specialAttackPower;
		float num2 = (float)y.specialAttackPower;
		if (checkBath)
		{
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackUp))
			{
				num += (float)x.specialAttackPower * x.currentSufferState.onSpAttackUp.upPercent;
			}
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackDown))
			{
				num -= (float)x.specialAttackPower * x.currentSufferState.onSpAttackDown.downPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackUp))
			{
				num2 += (float)y.specialAttackPower * y.currentSufferState.onSpAttackUp.upPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackDown))
			{
				num2 -= (float)y.specialAttackPower * y.currentSufferState.onSpAttackDown.downPercent;
			}
		}
		num += (float)x.specialAttackPower * x.leaderSkillResult.specialAttackUpPercent;
		num2 += (float)y.specialAttackPower * y.leaderSkillResult.specialAttackUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareSpecialDefenceBase(CharacterStateControl x, CharacterStateControl y, bool checkBath = true)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.specialDefencePower;
		float num2 = (float)y.specialDefencePower;
		if (checkBath)
		{
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpDefenceUp))
			{
				num += (float)x.specialDefencePower * x.currentSufferState.onSpDefenceUp.upPercent;
			}
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpDefenceDown))
			{
				num -= (float)x.specialDefencePower * x.currentSufferState.onSpDefenceDown.downPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpDefenceUp))
			{
				num2 += (float)y.specialDefencePower * y.currentSufferState.onSpDefenceUp.upPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpDefenceDown))
			{
				num2 -= (float)y.specialDefencePower * y.currentSufferState.onSpDefenceDown.downPercent;
			}
		}
		num += (float)x.specialDefencePower * x.leaderSkillResult.specialDefenceUpPercent;
		num2 += (float)y.specialDefencePower * y.leaderSkillResult.specialDefenceUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareSpeedBase(CharacterStateControl x, CharacterStateControl y, bool checkRandom = false, bool checkBath = true)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.speed;
		float num2 = (float)y.speed;
		if (checkRandom)
		{
			num = x.randomedSpeed;
			num2 = y.randomedSpeed;
		}
		if (checkBath)
		{
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpeedUp))
			{
				num += (float)x.speed * x.currentSufferState.onSpeedUp.upPercent;
			}
			if (x.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpeedDown))
			{
				num -= (float)x.speed * x.currentSufferState.onSpeedDown.downPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpeedUp))
			{
				num2 += (float)y.speed * y.currentSufferState.onSpeedUp.upPercent;
			}
			if (y.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpeedDown))
			{
				num2 -= (float)y.speed * y.currentSufferState.onSpeedDown.downPercent;
			}
		}
		if (!checkRandom)
		{
			num += (float)x.speed * x.leaderSkillResult.speedUpPercent;
			num2 += (float)y.speed * y.leaderSkillResult.speedUpPercent;
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareHateBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int hate = x.hate;
		int hate2 = y.hate;
		if (hate > hate2)
		{
			return -1;
		}
		if (hate < hate2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareLuckBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x.luck > y.luck)
		{
			return -1;
		}
		if (x.luck < y.luck)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareToleranceBase(Strength x, Strength y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x != y)
		{
			switch (x)
			{
			case Strength.None:
				if (y == Strength.Strong || y == Strength.Invalid)
				{
					return 1;
				}
				break;
			case Strength.Strong:
				if (y == Strength.Invalid)
				{
					return 1;
				}
				break;
			case Strength.Weak:
				return 1;
			}
			return -1;
		}
		return 0;
	}

	private static int CompareToleranceAttributeBase(CharacterStateControl x, CharacterStateControl y, global::Attribute attribute)
	{
		if (x == y)
		{
			return 0;
		}
		return CharacterStateControl.CompareToleranceBase(x.tolerance.GetAttributeStrength(attribute), y.tolerance.GetAttributeStrength(attribute));
	}

	private static int CompareToleranceAffectEffectBase(CharacterStateControl x, CharacterStateControl y, AffectEffect affectEffect)
	{
		if (x == y)
		{
			return 0;
		}
		return CharacterStateControl.CompareToleranceBase(x.tolerance.GetAffectEffectStrength(affectEffect), y.tolerance.GetAffectEffectStrength(affectEffect));
	}

	private static int CompareAttributeBase(CharacterStateControl x, CharacterStateControl y, global::Attribute attribute)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		foreach (SkillStatus skillStatus2 in x.skillStatus)
		{
			num += skillStatus2.AttributeMachLevel(attribute);
		}
		foreach (SkillStatus skillStatus4 in y.skillStatus)
		{
			num2 += skillStatus4.AttributeMachLevel(attribute);
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareHpRevivalBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		foreach (SkillStatus skillStatus2 in x.skillStatus)
		{
			num += ((!skillStatus2.ThisSkillIsHpRevival) ? 0 : 1);
		}
		foreach (SkillStatus skillStatus4 in y.skillStatus)
		{
			num2 += ((!skillStatus4.ThisSkillIsHpRevival) ? 0 : 1);
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareApRangeBase(CharacterStateControl x, CharacterStateControl y, float minValue, float maxValue)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		if (minValue <= (float)x.ap && (float)x.ap <= maxValue)
		{
			num = x.ap;
		}
		if (minValue <= (float)y.ap && (float)y.ap <= maxValue)
		{
			num2 = y.ap;
		}
		return Mathf.Clamp(num2 - num, -1, 1);
	}

	private static bool GetApRemingAmoutRange(CharacterStateControl state, float minValue, float maxValue)
	{
		return minValue <= (float)state.ap && (float)state.ap <= maxValue;
	}

	private static int CompareAffectEffectRangeBase(CharacterStateControl x, CharacterStateControl y, int minValue, int maxValue)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		foreach (SkillStatus skillStatus2 in x.skillStatus)
		{
			List<int> serberAffectEffectList = CharacterStateControl.GetSerberAffectEffectList(skillStatus2);
			foreach (int num3 in serberAffectEffectList)
			{
				if (minValue <= num3 && num3 <= maxValue)
				{
					num++;
					break;
				}
			}
		}
		foreach (SkillStatus skillStatus4 in y.skillStatus)
		{
			List<int> serberAffectEffectList2 = CharacterStateControl.GetSerberAffectEffectList(skillStatus4);
			foreach (int num4 in serberAffectEffectList2)
			{
				if (minValue <= num4 && num4 <= maxValue)
				{
					num2++;
					break;
				}
			}
		}
		return Mathf.Clamp(num2 - num, -1, 1);
	}

	private static List<int> GetSerberAffectEffectList(SkillStatus skillStatus)
	{
		List<int> list = new List<int>();
		foreach (AffectEffectProperty affectEffectProperty in skillStatus.affectEffect)
		{
			if (affectEffectProperty.type == AffectEffect.HpRevival)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(14);
				}
				else
				{
					list.Add(35);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.Damage)
			{
				if (affectEffectProperty.techniqueType == TechniqueType.Physics)
				{
					if (affectEffectProperty.useDrain)
					{
						if (affectEffectProperty.powerType == PowerType.Fixable)
						{
							list.Add(49);
						}
						else
						{
							list.Add(36);
						}
					}
					else if (affectEffectProperty.powerType == PowerType.Fixable)
					{
						list.Add(47);
					}
					else
					{
						list.Add(1);
					}
				}
				else if (affectEffectProperty.useDrain)
				{
					if (affectEffectProperty.powerType == PowerType.Fixable)
					{
						list.Add(50);
					}
					else
					{
						list.Add(37);
					}
				}
				else if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(48);
				}
				else
				{
					list.Add(34);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.Poison)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(23);
				}
				else
				{
					list.Add(38);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.ApRevival)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(39);
				}
				else
				{
					list.Add(40);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.ApUp)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(42);
				}
				else
				{
					list.Add(44);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.ApDown)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(41);
				}
				else
				{
					list.Add(43);
				}
			}
			else
			{
				list.Add((int)(affectEffectProperty.type + 1));
			}
		}
		return list;
	}

	private static int CompareSufferTypeRangeBase(CharacterStateControl x, CharacterStateControl y, int minValue, int maxValue)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		foreach (SufferStateProperty.SufferType sufferType in x.currentSufferState.sufferOrderList)
		{
			int num3 = (int)sufferType;
			if (minValue <= num3 && num3 <= maxValue)
			{
				num++;
			}
		}
		foreach (SufferStateProperty.SufferType sufferType2 in y.currentSufferState.sufferOrderList)
		{
			int num4 = (int)sufferType2;
			if (minValue <= num4 && num4 <= maxValue)
			{
				num2++;
			}
		}
		return Mathf.Clamp(num2 - num, -1, 1);
	}

	private static int CompareGenreationStartTimingBase(CharacterStateControl x, CharacterStateControl y, SufferStateProperty.SufferType sufferType)
	{
		if (x == y)
		{
			return 0;
		}
		SufferStateProperty sufferStateProperty = x.currentSufferState.GetSufferStateProperty(sufferType);
		SufferStateProperty sufferStateProperty2 = y.currentSufferState.GetSufferStateProperty(sufferType);
		if (sufferStateProperty.generationStartTiming < sufferStateProperty2.generationStartTiming)
		{
			return -1;
		}
		if (sufferStateProperty.generationStartTiming > sufferStateProperty2.generationStartTiming)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareSpeed(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControl.CompareSpeedBase(x, y, true, true);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}

	private static int CompareSpeedEnemyPriority(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControl.CompareSpeedBase(x, y, true, true);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		if (x.isEnemy == y.isEnemy)
		{
			return RandomExtension.IntPlusMinus();
		}
		if (x.isEnemy)
		{
			return -1;
		}
		return 1;
	}

	private static int CompareSpeedLuck(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControl.CompareSpeedBase(x, y, true, true);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		num = CharacterStateControl.CompareLuckBase(x, y);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}

	private static int CompareHpSpeed(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControl.CompareHpBase(x, y);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		num = CharacterStateControl.CompareSpeedBase(x, y, false, true);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}

	private static int CompareHate(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControl.CompareHateBase(x, y);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}

	private static int CompareTargetSelect(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		if (CharacterStateControl.actionClipTemp != null)
		{
			switch (CharacterStateControl.actionClipTemp.targetSelectRerefence)
			{
			case TargetSelectReference.Hp:
				num = CharacterStateControl.CompareHpRangeBase(x, y, CharacterStateControl.actionClipTemp.minValue, CharacterStateControl.actionClipTemp.maxValue);
				break;
			case TargetSelectReference.Hate:
				num = CharacterStateControl.CompareHateBase(x, y);
				break;
			case TargetSelectReference.Attack:
				num = CharacterStateControl.CompareAttackBase(x, y, false);
				break;
			case TargetSelectReference.Defence:
				num = CharacterStateControl.CompareDefenceBase(x, y, false);
				break;
			case TargetSelectReference.SpecialAttack:
				num = CharacterStateControl.CompareSpecialAttackBase(x, y, false);
				break;
			case TargetSelectReference.SpecialDefence:
				num = CharacterStateControl.CompareSpecialDefenceBase(x, y, false);
				break;
			case TargetSelectReference.Speed:
				num = CharacterStateControl.CompareSpeedBase(x, y, false, false);
				break;
			case TargetSelectReference.Luck:
				num = CharacterStateControl.CompareLuckBase(x, y);
				break;
			case TargetSelectReference.ToleranceNone:
				num = CharacterStateControl.CompareToleranceAttributeBase(x, y, global::Attribute.None);
				break;
			case TargetSelectReference.ToleranceRed:
				num = CharacterStateControl.CompareToleranceAttributeBase(x, y, global::Attribute.Red);
				break;
			case TargetSelectReference.ToleranceBlue:
				num = CharacterStateControl.CompareToleranceAttributeBase(x, y, global::Attribute.Blue);
				break;
			case TargetSelectReference.ToleranceYellow:
				num = CharacterStateControl.CompareToleranceAttributeBase(x, y, global::Attribute.Yellow);
				break;
			case TargetSelectReference.ToleranceGreen:
				num = CharacterStateControl.CompareToleranceAttributeBase(x, y, global::Attribute.Green);
				break;
			case TargetSelectReference.ToleranceWhite:
				num = CharacterStateControl.CompareToleranceAttributeBase(x, y, global::Attribute.White);
				break;
			case TargetSelectReference.ToleranceBlack:
				num = CharacterStateControl.CompareToleranceAttributeBase(x, y, global::Attribute.Black);
				break;
			case TargetSelectReference.TolerancePoison:
				num = CharacterStateControl.CompareToleranceAffectEffectBase(x, y, AffectEffect.Poison);
				break;
			case TargetSelectReference.ToleranceConfusion:
				num = CharacterStateControl.CompareToleranceAffectEffectBase(x, y, AffectEffect.Confusion);
				break;
			case TargetSelectReference.ToleranceParalysis:
				num = CharacterStateControl.CompareToleranceAffectEffectBase(x, y, AffectEffect.Paralysis);
				break;
			case TargetSelectReference.ToleranceSleep:
				num = CharacterStateControl.CompareToleranceAffectEffectBase(x, y, AffectEffect.Sleep);
				break;
			case TargetSelectReference.ToleranceStun:
				num = CharacterStateControl.CompareToleranceAffectEffectBase(x, y, AffectEffect.Stun);
				break;
			case TargetSelectReference.ToleranceSkillLock:
				num = CharacterStateControl.CompareToleranceAffectEffectBase(x, y, AffectEffect.SkillLock);
				break;
			case TargetSelectReference.ToleranceInstantDeath:
				num = CharacterStateControl.CompareToleranceAffectEffectBase(x, y, AffectEffect.InstantDeath);
				break;
			case TargetSelectReference.AttributeNone:
				num = CharacterStateControl.CompareAttributeBase(x, y, global::Attribute.None);
				break;
			case TargetSelectReference.AttributeRed:
				num = CharacterStateControl.CompareAttributeBase(x, y, global::Attribute.Red);
				break;
			case TargetSelectReference.AttributeBlue:
				num = CharacterStateControl.CompareAttributeBase(x, y, global::Attribute.Blue);
				break;
			case TargetSelectReference.AttributeYellow:
				num = CharacterStateControl.CompareAttributeBase(x, y, global::Attribute.Yellow);
				break;
			case TargetSelectReference.AttributeGreen:
				num = CharacterStateControl.CompareAttributeBase(x, y, global::Attribute.Green);
				break;
			case TargetSelectReference.AttributeWhite:
				num = CharacterStateControl.CompareAttributeBase(x, y, global::Attribute.White);
				break;
			case TargetSelectReference.AttributeBlack:
				num = CharacterStateControl.CompareAttributeBase(x, y, global::Attribute.Black);
				break;
			case TargetSelectReference.Ap:
				num = CharacterStateControl.CompareApRangeBase(x, y, CharacterStateControl.actionClipTemp.minValue, CharacterStateControl.actionClipTemp.maxValue);
				break;
			case TargetSelectReference.AffectEffect:
				num = CharacterStateControl.CompareAffectEffectRangeBase(x, y, (int)CharacterStateControl.actionClipTemp.minValue, (int)CharacterStateControl.actionClipTemp.maxValue);
				break;
			case TargetSelectReference.SufferType:
				num = CharacterStateControl.CompareSufferTypeRangeBase(x, y, (int)CharacterStateControl.actionClipTemp.minValue, (int)CharacterStateControl.actionClipTemp.maxValue);
				break;
			}
			if (CharacterStateControl.actionClipTemp.selectingOrder == SelectingOrder.LowAndHavent)
			{
				num *= -1;
			}
		}
		return num;
	}

	private static bool CheckTargetSelect(CharacterStateControl x)
	{
		if (CharacterStateControl.actionClipTemp == null)
		{
			return false;
		}
		switch (CharacterStateControl.actionClipTemp.targetSelectRerefence)
		{
		case TargetSelectReference.Hp:
			return x.GetHpRemingAmoutRange(CharacterStateControl.actionClipTemp.minValue, CharacterStateControl.actionClipTemp.maxValue);
		case TargetSelectReference.Hate:
			return true;
		case TargetSelectReference.Attack:
			return true;
		case TargetSelectReference.Defence:
			return true;
		case TargetSelectReference.SpecialAttack:
			return true;
		case TargetSelectReference.SpecialDefence:
			return true;
		case TargetSelectReference.Speed:
			return true;
		case TargetSelectReference.Luck:
			return true;
		case TargetSelectReference.ToleranceNone:
			return true;
		case TargetSelectReference.ToleranceRed:
			return true;
		case TargetSelectReference.ToleranceBlue:
			return true;
		case TargetSelectReference.ToleranceYellow:
			return true;
		case TargetSelectReference.ToleranceGreen:
			return true;
		case TargetSelectReference.ToleranceWhite:
			return true;
		case TargetSelectReference.ToleranceBlack:
			return true;
		case TargetSelectReference.TolerancePoison:
			return true;
		case TargetSelectReference.ToleranceConfusion:
			return true;
		case TargetSelectReference.ToleranceParalysis:
			return true;
		case TargetSelectReference.ToleranceSleep:
			return true;
		case TargetSelectReference.ToleranceStun:
			return true;
		case TargetSelectReference.ToleranceSkillLock:
			return true;
		case TargetSelectReference.ToleranceInstantDeath:
			return true;
		case TargetSelectReference.AttributeNone:
			return true;
		case TargetSelectReference.AttributeRed:
			return true;
		case TargetSelectReference.AttributeBlue:
			return true;
		case TargetSelectReference.AttributeYellow:
			return true;
		case TargetSelectReference.AttributeGreen:
			return true;
		case TargetSelectReference.AttributeWhite:
			return true;
		case TargetSelectReference.AttributeBlack:
			return true;
		case TargetSelectReference.Ap:
			return CharacterStateControl.GetApRemingAmoutRange(x, CharacterStateControl.actionClipTemp.minValue, CharacterStateControl.actionClipTemp.maxValue);
		case TargetSelectReference.AffectEffect:
			return true;
		case TargetSelectReference.SufferType:
			return true;
		default:
			return false;
		}
	}

	private static int CompareBaseTargetSelect(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControl.CompareToleranceBase(CharacterStateControl.statusTemp.GetSkillStrength(x.tolerance), CharacterStateControl.statusTemp.GetSkillStrength(y.tolerance));
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		num = -CharacterStateControl.CompareHpBase(x, y);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return CharacterStateControl.CompareHate(x, y);
	}

	private static int CompareGenereationStartTiming(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControl.CompareGenreationStartTimingBase(x, y, CharacterStateControl.sufferTypeTemp);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}

	public bool Contains(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl a in characters)
		{
			if (a == this)
			{
				return true;
			}
		}
		return false;
	}

	public static CharacterParams[] ToParams(CharacterStateControl[] status)
	{
		List<CharacterParams> list = new List<CharacterParams>();
		foreach (CharacterStateControl characterStateControl in status)
		{
			list.Add(characterStateControl.CharacterParams);
		}
		return list.ToArray();
	}

	public static CharacterStateControl[] GetAliveCharacters(CharacterStateControl[] characterStatus)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		for (int i = 0; i < characterStatus.Length; i++)
		{
			if (!characterStatus[i].isDied)
			{
				list.Add(characterStatus[i]);
			}
		}
		return list.ToArray();
	}

	public static CharacterStateControl[] SortedSpeed(CharacterStateControl[] characterStatus)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControl.CompareSpeed));
		return array;
	}

	public static CharacterStateControl[] SortedSpeedEnemyPriority(CharacterStateControl[] characterStatus)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControl.CompareSpeedEnemyPriority));
		return array;
	}

	public static CharacterStateControl[] SortedSpeedLuck(CharacterStateControl[] characterStatus)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControl.CompareSpeedLuck));
		return array;
	}

	public static CharacterStateControl[] SortedHpSpeed(CharacterStateControl[] characterStatus)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControl.CompareHpSpeed));
		return array;
	}

	public static CharacterStateControl[] SortedHate(CharacterStateControl[] characterStatus)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControl.CompareHate));
		return array;
	}

	public static CharacterStateControl[] SortedTargetSelect(CharacterStateControl[] characterStatus, SkillStatus skillState, AIActionClip actionClip = null)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		CharacterStateControl.statusTemp = skillState;
		CharacterStateControl.actionClipTemp = actionClip;
		foreach (CharacterStateControl characterStateControl in characterStatus)
		{
			if (CharacterStateControl.CheckTargetSelect(characterStateControl))
			{
				list.Add(characterStateControl);
			}
		}
		if (list.Count > 0)
		{
			list.Sort(new Comparison<CharacterStateControl>(CharacterStateControl.CompareTargetSelect));
		}
		else
		{
			list = new List<CharacterStateControl>(characterStatus);
			list.Sort(new Comparison<CharacterStateControl>(CharacterStateControl.CompareBaseTargetSelect));
		}
		return list.ToArray();
	}

	public static CharacterStateControl[] SortedSufferStateGenerateStartTiming(CharacterStateControl[] characterStatus, SufferStateProperty.SufferType sufferType)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		CharacterStateControl.sufferTypeTemp = sufferType;
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControl.CompareGenereationStartTiming));
		return array;
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(CharacterStateControl a, CharacterStateControl b)
	{
		if (object.ReferenceEquals(a, b))
		{
			return true;
		}
		if (a == null)
		{
			return b == null;
		}
		return b != null && (a.isEnemy && b.isEnemy && a.myIndex == b.myIndex);
	}

	public static bool operator !=(CharacterStateControl a, CharacterStateControl b)
	{
		return !(a == b);
	}
}
