using BattleStateMachineInternal;
using Enemy.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStateControl
{
	public const int AttackSkillIndex = 0;

	public const int DeathblowSkillIndex = 1;

	public const int InheritanceSkillIndex1 = 2;

	public const int InheritanceSkillIndex2 = 3;

	public const int DefaultAwakeAp = 3;

	public const int DefaultMaxAp = 5;

	private const int ArousalVersionUpValue = 5;

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

	private int m_extraInheritanceSkillPower2;

	private float m_extraDeathblowSkillHitRate;

	private float m_extraInheritanceSkillHitRate;

	private float m_extraInheritanceSkillHitRate2;

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

	private Dictionary<EffectStatusBase.EffectTriggerType, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> triggerChips = new Dictionary<EffectStatusBase.EffectTriggerType, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>>();

	public bool isRecommand;

	public List<SufferStateProperty> hitSufferList = new List<SufferStateProperty>();

	public bool isMultiAreaRandomDamageSkill;

	public CharacterStateControl()
	{
		this.stagingChipIdList = new Dictionary<int, int>();
		this.autoCounterSkillList = new List<string>();
		this.potencyChipIdList = new Dictionary<int, int>();
		this.isInitSpecialCorrectionStatus = false;
	}

	public CharacterStateControl(CharacterStatus status, Tolerance tolerance, CharacterDatas datas, SkillStatus[] skillStatuses, LeaderSkillStatus leaderCharacterLeaderSkill, CharacterDatas leaderCharacterData, LeaderSkillStatus myLeaderSkill = null, bool isEnemy = false)
	{
		this.stagingChipIdList = new Dictionary<int, int>();
		this.autoCounterSkillList = new List<string>();
		this.potencyChipIdList = new Dictionary<int, int>();
		this.isInitSpecialCorrectionStatus = false;
		this._sharedStatus = SharedStatus.GetEmptyStatus();
		this._characterStatus = status;
		this._characterDatas = datas;
		this._tolerance = tolerance;
		this._skillStatus = skillStatuses;
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
		this.StatusInitialize();
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

	public int extraInheritanceSkillPower2
	{
		get
		{
			return this.m_extraInheritanceSkillPower2;
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

	public float extraInheritanceSkillHitRate2
	{
		get
		{
			return this.m_extraInheritanceSkillHitRate2;
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

	public int stageChipAddMaxHp
	{
		get
		{
			return this.m_stageChipAddMaxHp;
		}
	}

	public int stageChipAddAttackPower
	{
		get
		{
			return this.m_stageChipAddAttackPower;
		}
	}

	public int stageChipAddDefencePower
	{
		get
		{
			return this.m_stageChipAddDefencePower;
		}
	}

	public int stageChipAddSpecialAttackPower
	{
		get
		{
			return this.m_stageChipAddSpecialAttackPower;
		}
	}

	public int stageChipAddSpecialDefencePower
	{
		get
		{
			return this.m_stageChipAddSpecialDefencePower;
		}
	}

	public int stageChipAddSpeed
	{
		get
		{
			return this.m_stageChipAddSpeed;
		}
	}

	public InvariantType hittingTheTargetChipType { get; private set; }

	public InvariantType criticalTheTargetChipType { get; private set; }

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

	public CharacterDatas characterDatas
	{
		get
		{
			return this._characterDatas;
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
				this.OnChipTrigger(EffectStatusBase.EffectTriggerType.Dead, false);
				this.RemoveDeadStagingChips();
			}
			else
			{
				this.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpPercentage, currentHp < this._currentHp);
				this.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpFixed, false);
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
			return this._characterStatus.speed;
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

	public Talent talent
	{
		get
		{
			return (!PlayerStatus.Match(this._characterStatus)) ? new Talent() : this.playerStatus.talent;
		}
	}

	public Tolerance tolerance
	{
		get
		{
			if (this.leaderSkillResult.addTolerances != null)
			{
				return this._tolerance.CreateAddTolerance(this._leaderSkillResult.addTolerances);
			}
			return this._tolerance;
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

	public string groupId
	{
		get
		{
			return this._characterStatus.groupId;
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
		this.currentSufferState.SetCurrentSufferState(savedCSC.currentSufferState, battleStateData);
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

	public bool isVersionUp
	{
		get
		{
			return this.arousal >= 5;
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

	public bool GetHpRemingAmoutRange(float minRange, float maxRange)
	{
		float num = (float)this.hp / (float)this.maxHp;
		return num >= minRange && num <= maxRange;
	}

	private void StatusInitialize()
	{
		this.currentSufferState = new HaveSufferState();
		this._maxHp = this._characterStatus.maxHp + Mathf.FloorToInt((float)this.defaultMaxHp * this.leaderSkillResult.hpUpPercent);
		this._currentHp = this._maxHp;
		this.m_extraMaxHp = this._maxHp;
		this.skillIds = this._characterStatus.skillIds;
		this.skillStatus = new SkillStatus[this.skillIds.Length];
		for (int i = 0; i < this.skillIds.Length; i++)
		{
			this.skillStatus[i] = BattleStateManager.current.hierarchyData.GetSkillStatus(this.skillIds[i]);
		}
		if (!PlayerStatus.Match(this._characterStatus))
		{
			this.ItemDrop();
		}
		if (this._sharedStatus.isShared)
		{
			this.ap += 5;
		}
		else
		{
			this.ap = this.maxAp;
		}
		this.InitializeSkillExtraStatus();
		this.InitializeChipEffectCount();
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
				this.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpPercentage, false);
				this.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpFixed, false);
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
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
		this.m_extraMaxHp = ExtraEffectStatus.GetExtraEffectValue(invocationList, this._maxHp, this, EffectStatusBase.ExtraEffectType.Hp);
		this.m_extraAttackPower = ExtraEffectStatus.GetExtraEffectValue(invocationList, baseValue, this, EffectStatusBase.ExtraEffectType.Atk);
		this.m_extraDefencePower = ExtraEffectStatus.GetExtraEffectValue(invocationList, baseValue2, this, EffectStatusBase.ExtraEffectType.Def);
		this.m_extraSpecialAttackPower = ExtraEffectStatus.GetExtraEffectValue(invocationList, baseValue3, this, EffectStatusBase.ExtraEffectType.Satk);
		this.m_extraSpecialDefencePower = ExtraEffectStatus.GetExtraEffectValue(invocationList, baseValue4, this, EffectStatusBase.ExtraEffectType.Sdef);
		this.m_extraSpeed = ExtraEffectStatus.GetExtraEffectValue(invocationList, baseValue5, this, EffectStatusBase.ExtraEffectType.Speed);
		this.m_defaultExtraAttackPower = ExtraEffectStatus.GetExtraEffectValue(invocationList, this._characterStatus.attackPower, this, EffectStatusBase.ExtraEffectType.Atk);
		this.m_defaultExtraDefencePower = ExtraEffectStatus.GetExtraEffectValue(invocationList, this._characterStatus.defencePower, this, EffectStatusBase.ExtraEffectType.Def);
		this.m_defaultExtraSpecialAttackPower = ExtraEffectStatus.GetExtraEffectValue(invocationList, this._characterStatus.specialAttackPower, this, EffectStatusBase.ExtraEffectType.Satk);
		this.m_defaultExtraSpecialDefencePower = ExtraEffectStatus.GetExtraEffectValue(invocationList, this._characterStatus.specialDefencePower, this, EffectStatusBase.ExtraEffectType.Sdef);
		this.m_defaultExtraSpeed = ExtraEffectStatus.GetExtraEffectValue(invocationList, this._characterStatus.speed, this, EffectStatusBase.ExtraEffectType.Speed);
	}

	public void InitializeSkillExtraStatus()
	{
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
		if (this.skillStatus.Length > 1)
		{
			AffectEffectProperty affectEffectFirst = this.skillStatus[1].GetAffectEffectFirst();
			if (affectEffectFirst != null)
			{
				if (AffectEffectProperty.IsDamage(affectEffectFirst.type))
				{
					this.m_extraDeathblowSkillPower = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, affectEffectFirst, this);
				}
				this.m_extraDeathblowSkillHitRate = ExtraEffectStatus.GetSkillHitRateCorrectionValue(invocationList, affectEffectFirst, this);
			}
		}
		if (this.skillStatus.Length > 2)
		{
			AffectEffectProperty affectEffectFirst2 = this.skillStatus[2].GetAffectEffectFirst();
			if (affectEffectFirst2 != null)
			{
				if (AffectEffectProperty.IsDamage(affectEffectFirst2.type))
				{
					this.m_extraInheritanceSkillPower = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, affectEffectFirst2, this);
				}
				this.m_extraInheritanceSkillHitRate = ExtraEffectStatus.GetSkillHitRateCorrectionValue(invocationList, affectEffectFirst2, this);
			}
		}
		if (this.skillStatus.Length > 3)
		{
			AffectEffectProperty affectEffectFirst3 = this.skillStatus[3].GetAffectEffectFirst();
			if (affectEffectFirst3 != null)
			{
				if (AffectEffectProperty.IsDamage(affectEffectFirst3.type))
				{
					this.m_extraInheritanceSkillPower2 = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, affectEffectFirst3, this);
				}
				this.m_extraInheritanceSkillHitRate2 = ExtraEffectStatus.GetSkillHitRateCorrectionValue(invocationList, affectEffectFirst3, this);
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
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipEffectData, EffectStatusBase.EffectTriggerType.Usually, this.groupId.ToInt32(), this, 0);
				this.AddChipParam(true, this.GetAddChipParamEffects(invocationList, true).ToArray(), false, false);
				int triggerValue = 0;
				if (BattleStateManager.current != null)
				{
					triggerValue = BattleStateManager.current.hierarchyData.areaId;
				}
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList2 = ChipEffectStatus.GetInvocationList(chipEffectData, EffectStatusBase.EffectTriggerType.Area, this.groupId.ToInt32(), this, triggerValue);
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
			if (num <= 0 || num == this.groupId.ToInt32())
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
									goto IL_252;
								}
							}
							if (this.chipEffectCount.ContainsKey(num2))
							{
								if (this.chipEffectCount[num2] <= 0)
								{
									goto IL_252;
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
								if (this.hittingTheTargetChipType != InvariantType.Up && chipEffect.effectValue != "0")
								{
									this.hittingTheTargetChipType = InvariantType.Down;
								}
								else
								{
									this.hittingTheTargetChipType = InvariantType.Up;
								}
							}
							if (chipEffect.effectType.ToInt32() == 59)
							{
								if (this.criticalTheTargetChipType != InvariantType.Up && chipEffect.effectValue != "0")
								{
									this.criticalTheTargetChipType = InvariantType.Down;
								}
								else
								{
									this.criticalTheTargetChipType = InvariantType.Up;
								}
							}
							list.Add(chipEffect);
							if (chipEffect.effectType.ToInt32() != 60 && chipEffect.effectType.ToInt32() != 61 && chipEffect.effectType.ToInt32() != 56)
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
			IL_252:;
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
		this.randomedSpeed = (float)this.extraSpeed + (float)this.extraSpeed * this.leaderSkillResult.speedUpPercent;
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
		this.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpPercentage, true);
		this.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpFixed, false);
	}

	public void ItemDrop()
	{
		if (!PlayerStatus.Match(this.enemyStatus))
		{
			this.itemDropResult = this.enemyStatus.itemDropResult;
		}
	}

	public void LeaderInitialize()
	{
		if (!this.isLeader)
		{
			return;
		}
		this.HateReset();
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

	public bool isApShortness(int index)
	{
		if (this.skillStatus.Length <= index)
		{
			global::Debug.LogError("Skill Params Out: " + index);
			return false;
		}
		return index != -1 && this.ap < this.skillStatus[index].GetCorrectedAp(this);
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
				if (AffectEffectProperty.IsDamage(affectEffectFirst.type))
				{
					if (this.m_extraDeathblowSkillPower - affectEffectFirst.GetPower(this) > 0)
					{
						return 1;
					}
					if (this.m_extraDeathblowSkillPower - affectEffectFirst.GetPower(this) < 0)
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
				if (AffectEffectProperty.IsDamage(affectEffectFirst2.type))
				{
					if (this.m_extraInheritanceSkillPower - affectEffectFirst2.GetPower(this) > 0)
					{
						return 1;
					}
					if (this.m_extraInheritanceSkillPower - affectEffectFirst2.GetPower(this) < 0)
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
		if (this.skillStatus.Length > 3)
		{
			AffectEffectProperty affectEffectFirst3 = this.skillStatus[3].GetAffectEffectFirst();
			if (affectEffectFirst3 != null)
			{
				if (AffectEffectProperty.IsDamage(affectEffectFirst3.type))
				{
					if (this.m_extraInheritanceSkillPower2 - affectEffectFirst3.GetPower(this) > 0)
					{
						return 1;
					}
					if (this.m_extraInheritanceSkillPower2 - affectEffectFirst3.GetPower(this) < 0)
					{
						result = -1;
					}
				}
				if (this.m_extraInheritanceSkillHitRate2 - affectEffectFirst3.hitRate > 0f)
				{
					return 1;
				}
				if (this.m_extraInheritanceSkillHitRate2 - affectEffectFirst3.hitRate < 0f)
				{
					result = -1;
				}
			}
		}
		return result;
	}

	public void OnChipTrigger(EffectStatusBase.EffectTriggerType triggerType, bool isRecovery = false)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> chipTriggerList = this.GetChipTriggerList(triggerType);
		if (chipTriggerList.Count == 0)
		{
			return;
		}
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		int triggerValue = 0;
		if (triggerType == EffectStatusBase.EffectTriggerType.HpPercentage)
		{
			triggerValue = (int)((float)this.hp / (float)this.maxHp * 100f);
		}
		else if (triggerType == EffectStatusBase.EffectTriggerType.HpFixed)
		{
			triggerValue = this.hp;
		}
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipTriggerList.ToArray(), triggerType, this.groupId.ToInt32(), this, triggerValue);
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
						this.hittingTheTargetChipType = InvariantType.Non;
					}
					if (chipEffect.effectType.ToInt32() == 59)
					{
						this.criticalTheTargetChipType = InvariantType.Non;
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

	private List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetChipTriggerList(EffectStatusBase.EffectTriggerType triggerType)
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
						if (num2 <= 0 || num2 == this.groupId.ToInt32())
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
			num = ChipEffectStatus.GetChipEffectValue(chipEffects, this.defaultMaxHp, this, EffectStatusBase.ExtraEffectType.Hp);
			num2 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue, this, EffectStatusBase.ExtraEffectType.Atk);
			num3 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue2, this, EffectStatusBase.ExtraEffectType.Def);
			num4 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue3, this, EffectStatusBase.ExtraEffectType.Satk);
			num5 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue4, this, EffectStatusBase.ExtraEffectType.Sdef);
			num6 = ChipEffectStatus.GetChipEffectValue(chipEffects, baseValue5, this, EffectStatusBase.ExtraEffectType.Speed);
		}
		float chipEffectValueToFloat = ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, 0f, this, EffectStatusBase.ExtraEffectType.Critical);
		float chipEffectValueToFloat2 = ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, 0f, this, EffectStatusBase.ExtraEffectType.Hit);
		if (isArea)
		{
			this.m_stageChipAddMaxHp += num * num7;
			this.m_stageChipAddAttackPower += num2 * num7;
			this.m_stageChipAddDefencePower += num3 * num7;
			this.m_stageChipAddSpecialAttackPower += num4 * num7;
			this.m_stageChipAddSpecialDefencePower += num5 * num7;
			this.m_stageChipAddSpeed += num6 * num7;
		}
		else
		{
			this.m_chipAddMaxHp += num * num7;
			this.m_chipAddAttackPower += num2 * num7;
			this.m_chipAddDefencePower += num3 * num7;
			this.m_chipAddSpecialAttackPower += num4 * num7;
			this.m_chipAddSpecialDefencePower += num5 * num7;
			this.m_chipAddSpeed += num6 * num7;
		}
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
					if (num3 <= 0 || num3 == this.groupId.ToInt32())
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
										int value;
										if (chipEffect.effectSubType.ToInt32() == 1)
										{
											value = chipEffect.effectValue.ToInt32();
										}
										else if (chipEffect.effectSubType.ToInt32() == 2)
										{
											value = chipEffect.effectValue.ToInt32();
										}
										else if (chipEffect.effectSubType.ToInt32() == 3)
										{
											value = (int)((float)this._maxHp * chipEffect.effectValue.ToFloat());
										}
										else
										{
											value = chipEffect.effectValue.ToInt32();
										}
										this._currentHp = Mathf.Clamp(value, 1, maxHp);
										if (!this.stagingChipIdList.ContainsKey(num4))
										{
											this.stagingChipIdList.Add(num4, num2);
										}
									}
								}
								else
								{
									int value2;
									if (chipEffect.effectSubType.ToInt32() == 1)
									{
										value2 = chipEffect.effectValue.ToInt32();
									}
									else if (chipEffect.effectSubType.ToInt32() == 2)
									{
										value2 = chipEffect.effectValue.ToInt32();
									}
									else if (chipEffect.effectSubType.ToInt32() == 3)
									{
										value2 = (int)((float)this._maxHp * chipEffect.effectValue.ToFloat());
									}
									else
									{
										value2 = chipEffect.effectValue.ToInt32();
									}
									this._currentHp = Mathf.Clamp(value2, 1, maxHp);
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

	public void InitChipEffectCountForWave()
	{
		this.InitChipEffectCount("1");
	}

	public void InitChipEffectCountForTurn()
	{
		this.InitChipEffectCount("2");
	}

	private void InitChipEffectCount(string value)
	{
		foreach (int num in this.chipIds)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
			{
				if (chipEffect.effectTurnType == value && this.chipEffectCount.ContainsKey(chipEffect.chipEffectId.ToInt32()))
				{
					this.chipEffectCount[chipEffect.chipEffectId.ToInt32()] = chipEffect.effectTurn.ToInt32();
				}
			}
		}
	}

	public void ChangeLeaderSkill(LeaderSkillStatus leaderSkillStatus, CharacterDatas characterDatas)
	{
		this._leaderSkillResult = new LeaderSkillResult(this, leaderSkillStatus, characterDatas);
		this._maxHp = this.defaultMaxHp + Mathf.FloorToInt((float)this.defaultMaxHp * this.leaderSkillResult.hpUpPercent);
		this.m_extraMaxHp = this._maxHp;
		if (this._currentHp > this._maxHp)
		{
			this._currentHp = this._maxHp;
		}
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
