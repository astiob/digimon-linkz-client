using BattleStateMachineInternal;
using Enemy.AI;
using Monster;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
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

	public bool isRecommand;

	public List<SufferStateProperty> hitSufferList = new List<SufferStateProperty>();

	public bool isEscape;

	public int[] skillUseCounts = new int[0];

	private CharacterStateControlChip chip = CharacterStateControlChip.GetNullObject();

	public bool isMultiAreaRandomDamageSkill;

	public Dictionary<EffectStatusBase.EffectTriggerType, List<AffectEffectProperty>> everySkillList = new Dictionary<EffectStatusBase.EffectTriggerType, List<AffectEffectProperty>>();

	public CharacterStateControl(CharacterStatus status, Tolerance tolerance, CharacterDatas datas, SkillStatus[] skillStatuses, LeaderSkillStatus leaderCharacterLeaderSkill, CharacterDatas leaderCharacterData, LeaderSkillStatus myLeaderSkill = null, bool isEnemy = false)
	{
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
		if (status.GetType() == typeof(EnemyStatus))
		{
			this.enemyAi = this.enemyStatus.enemyAiPattern;
		}
	}

	public int extraMaxHp
	{
		get
		{
			return this.m_extraMaxHp + this.chip.chipAddMaxHp + this.chip.stageChipAddMaxHp;
		}
	}

	public int extraAttackPower
	{
		get
		{
			return this.m_extraAttackPower + this.chip.chipAddAttackPower + this.chip.stageChipAddAttackPower;
		}
	}

	public int extraDefencePower
	{
		get
		{
			return this.m_extraDefencePower + this.chip.chipAddDefencePower + this.chip.stageChipAddDefencePower;
		}
	}

	public int extraSpecialAttackPower
	{
		get
		{
			return this.m_extraSpecialAttackPower + this.chip.chipAddSpecialAttackPower + this.chip.stageChipAddSpecialAttackPower;
		}
	}

	public int extraSpecialDefencePower
	{
		get
		{
			return this.m_extraSpecialDefencePower + this.chip.chipAddSpecialDefencePower + this.chip.stageChipAddSpecialDefencePower;
		}
	}

	public int extraSpeed
	{
		get
		{
			return this.m_extraSpeed + this.chip.chipAddSpeed + this.chip.stageChipAddSpeed;
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

	public CharacterParams CharacterParams
	{
		get
		{
			return this._characterParams;
		}
		set
		{
			this._characterParams = value;
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
			int min = 0;
			if (this.chip.gutsData != null && value <= 0)
			{
				this.chip.gutsData.isUse = true;
				min = this.chip.gutsData.hp;
			}
			int currentHp = this._currentHp;
			int num = Mathf.Clamp(value, min, this.extraMaxHp);
			if (this._currentHp == num)
			{
				this._currentHp = num;
				return;
			}
			if (num == 0)
			{
				this.chip.OnChipTrigger(EffectStatusBase.EffectTriggerType.LastDead);
				if (this._currentHp == currentHp)
				{
					this._currentHp = num;
				}
			}
			else
			{
				this._currentHp = num;
			}
			if (this._currentHp == 0)
			{
				this.chip.ClearStagingChipIdList();
				this.chip.OnChipTrigger(EffectStatusBase.EffectTriggerType.Dead);
			}
			else
			{
				this.chip.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpPercentage);
				this.chip.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpFixed);
			}
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
			if (PlayerStatus.Match(this._characterStatus))
			{
				return this.playerStatus.arousal;
			}
			return 0;
		}
	}

	public int maxHp
	{
		get
		{
			return this.defaultMaxHpWithLeaderSkill + this.chip.chipAddMaxHp;
		}
	}

	public int attackPower
	{
		get
		{
			return this._characterStatus.attackPower + this.chip.chipAddAttackPower;
		}
	}

	public int defencePower
	{
		get
		{
			return this._characterStatus.defencePower + this.chip.chipAddDefencePower;
		}
	}

	public int specialAttackPower
	{
		get
		{
			return this._characterStatus.specialAttackPower + this.chip.chipAddSpecialAttackPower;
		}
	}

	public int specialDefencePower
	{
		get
		{
			return this._characterStatus.specialDefencePower + this.chip.chipAddSpecialDefencePower;
		}
	}

	public int speed
	{
		get
		{
			return this._characterStatus.speed + this.chip.chipAddSpeed;
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

	private void InitSkillUseCount()
	{
		this.skillUseCounts = new int[this._skillStatus.Length];
		for (int i = 0; i < this.skillUseCounts.Length; i++)
		{
			SkillStatus skillStatus = this._skillStatus[i];
			this.skillUseCounts[i] = skillStatus.useCountValue;
		}
	}

	public void AddSkillUseCount(int index, int value)
	{
		if (this._skillStatus[index].useCountValue > 0)
		{
			this.skillUseCounts[index] += value;
			if (this.skillUseCounts[index] < 0)
			{
				this.skillUseCounts[index] = 0;
			}
		}
	}

	public void ResetSkillUseCountForWave()
	{
		this.ResetSkillUseCount("1");
	}

	private void ResetSkillUseCount(string useCountType)
	{
		for (int i = 0; i < this._skillStatus.Length; i++)
		{
			if (this._skillStatus[i].useCountType == useCountType)
			{
				this.skillUseCounts[i] = this._skillStatus[i].useCountValue;
			}
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
		this.currentSufferState.Set(savedCSC.currentSufferState);
		this.randomedSpeed = savedCSC.randomedSpeed;
		this.isEscape = savedCSC.isEscape;
		this.skillUseCounts = savedCSC.skillUseCounts;
		this.chip.SetCharacterState(savedCSC);
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

	public int defaultMaxHpWithLeaderSkill
	{
		get
		{
			return this._characterStatus.maxHp + Mathf.FloorToInt((float)this._characterStatus.maxHp * this.leaderSkillResult.hpUpPercent);
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

	public int SkillIdToIndexOf(string SkillId)
	{
		List<string> list = new List<string>(this.skillIds);
		return list.IndexOf(SkillId);
	}

	public bool GetHpRemainingAmoutRange(float minRange, float maxRange)
	{
		float num = (float)this.hp / (float)this.extraMaxHp;
		return num >= minRange && num <= maxRange;
	}

	private void StatusInitialize()
	{
		this._currentHp = this._characterStatus.maxHp;
		this.skillIds = this._characterStatus.skillIds;
		this.skillStatus = new SkillStatus[this.skillIds.Length];
		for (int i = 0; i < this.skillIds.Length; i++)
		{
			this.skillStatus[i] = BattleStateManager.current.hierarchyData.GetSkillStatus(this.skillIds[i]);
		}
		this.InitSkillUseCount();
		if (!PlayerStatus.Match(this._characterStatus))
		{
			this.itemDropResult = this.enemyStatus.itemDropResult;
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
		this.chip = new CharacterStateControlChip(this);
		this.HateReset();
		this.currentSufferState = new HaveSufferState();
		this.isDiedJustBefore = false;
	}

	public void InitializeSpecialCorrectionStatus()
	{
		if (!this.isDied)
		{
			this.OnChipTrigger(EffectStatusBase.EffectTriggerType.Usually);
			this.OnChipTrigger(EffectStatusBase.EffectTriggerType.Area);
		}
		this.InitializeExtraStatus();
		this._currentHp = this.extraMaxHp;
	}

	private void InitializeExtraStatus()
	{
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
		int defaultMaxHpWithLeaderSkill = this.defaultMaxHpWithLeaderSkill;
		int attackPower = this._characterStatus.attackPower;
		int defencePower = this._characterStatus.defencePower;
		int specialAttackPower = this._characterStatus.specialAttackPower;
		int specialDefencePower = this._characterStatus.specialDefencePower;
		int speed = this._characterStatus.speed;
		this.m_extraMaxHp = ExtraEffectStatus.GetExtraEffectValue(invocationList, defaultMaxHpWithLeaderSkill, this, EffectStatusBase.ExtraEffectType.Hp);
		this.m_extraAttackPower = ExtraEffectStatus.GetExtraEffectValue(invocationList, attackPower, this, EffectStatusBase.ExtraEffectType.Atk);
		this.m_extraDefencePower = ExtraEffectStatus.GetExtraEffectValue(invocationList, defencePower, this, EffectStatusBase.ExtraEffectType.Def);
		this.m_extraSpecialAttackPower = ExtraEffectStatus.GetExtraEffectValue(invocationList, specialAttackPower, this, EffectStatusBase.ExtraEffectType.Satk);
		this.m_extraSpecialDefencePower = ExtraEffectStatus.GetExtraEffectValue(invocationList, specialDefencePower, this, EffectStatusBase.ExtraEffectType.Sdef);
		this.m_extraSpeed = ExtraEffectStatus.GetExtraEffectValue(invocationList, speed, this, EffectStatusBase.ExtraEffectType.Speed);
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

	public void Escape()
	{
		this._currentHp = 0;
		this.isDiedJustBefore = true;
		this.isEscape = true;
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
		this._currentHp = this.defaultMaxHpWithLeaderSkill;
		this.isDiedJustBefore = false;
		this.isEscape = false;
		this.currentSufferState.RevivalSufferState();
		this.InitializeSpecialCorrectionStatus();
		this.chip.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpPercentage);
		this.chip.OnChipTrigger(EffectStatusBase.EffectTriggerType.HpFixed);
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

	public bool isUseSkill(int index)
	{
		if (index < 0 || this.skillStatus.Length <= index)
		{
			global::Debug.LogError("Skill Params Out: " + index);
			return false;
		}
		return this.ap >= this.skillStatus[index].GetCorrectedAp(this) && (this.skillStatus[index].useCountValue <= 0 || this.skillUseCounts[index] > 0);
	}

	public bool WaveCountInitialize(float hpRevivalLevel)
	{
		bool result = true;
		int num = Mathf.FloorToInt((float)this.extraMaxHp * hpRevivalLevel);
		if (this.hp == this.extraMaxHp || num == 0)
		{
			result = false;
		}
		this._currentHp = Mathf.Clamp(this._currentHp + num, 1, this.extraMaxHp);
		return result;
	}

	public void GetDifferenceExtraPram(out int upCount, out int downCount)
	{
		upCount = 0;
		downCount = 0;
		int num = this.extraMaxHp - this.maxHp;
		if (num > 0)
		{
			upCount++;
		}
		else if (num < 0)
		{
			downCount++;
		}
		num = this.extraAttackPower - this.attackPower;
		if (num > 0)
		{
			upCount++;
		}
		else if (num < 0)
		{
			downCount++;
		}
		num = this.extraDefencePower - this.defencePower;
		if (num > 0)
		{
			upCount++;
		}
		else if (num < 0)
		{
			downCount++;
		}
		num = this.extraSpecialAttackPower - this.specialAttackPower;
		if (num > 0)
		{
			upCount++;
		}
		else if (num < 0)
		{
			downCount++;
		}
		num = this.extraSpecialDefencePower - this.specialDefencePower;
		if (num > 0)
		{
			upCount++;
		}
		else if (num < 0)
		{
			downCount++;
		}
		num = this.extraSpeed - this.speed;
		if (num > 0)
		{
			upCount++;
		}
		else if (num < 0)
		{
			downCount++;
		}
		if (this.skillStatus.Length > 1)
		{
			AffectEffectProperty affectEffectFirst = this.skillStatus[1].GetAffectEffectFirst();
			if (affectEffectFirst != null)
			{
				if (AffectEffectProperty.IsDamage(affectEffectFirst.type))
				{
					int num2 = this.m_extraDeathblowSkillPower - affectEffectFirst.GetPower(this);
					if (num2 > 0)
					{
						upCount++;
					}
					else if (num2 < 0)
					{
						downCount++;
					}
				}
				float num3 = this.m_extraDeathblowSkillHitRate - affectEffectFirst.hitRate;
				if (num3 > 0f)
				{
					upCount++;
				}
				else if (num3 < 0f)
				{
					downCount++;
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
					int num4 = this.m_extraInheritanceSkillPower - affectEffectFirst2.GetPower(this);
					if (num4 > 0)
					{
						upCount++;
					}
					else if (num4 < 0)
					{
						downCount++;
					}
				}
				float num5 = this.m_extraInheritanceSkillHitRate - affectEffectFirst2.hitRate;
				if (num5 > 0f)
				{
					upCount++;
				}
				else if (num5 < 0f)
				{
					downCount++;
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
					int num6 = this.m_extraInheritanceSkillPower2 - affectEffectFirst3.GetPower(this);
					if (num6 > 0)
					{
						upCount++;
					}
					else if (num6 < 0)
					{
						downCount++;
					}
				}
				float num7 = this.m_extraInheritanceSkillHitRate2 - affectEffectFirst3.hitRate;
				if (num7 > 0f)
				{
					upCount++;
				}
				else if (num7 < 0f)
				{
					downCount++;
				}
			}
		}
	}

	public bool IsPoint()
	{
		BattleStateManager current = BattleStateManager.current;
		if (!current.onServerConnect)
		{
			return false;
		}
		string b = string.Empty;
		if (current.battleMode == BattleMode.Multi)
		{
			b = DataMng.Instance().RespData_WorldMultiStartInfo.worldDungeonId;
		}
		else if (current.battleMode == BattleMode.PvP)
		{
			b = ClassSingleton<MultiBattleData>.Instance.PvPField.worldDungeonId;
		}
		else
		{
			b = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.worldDungeonId;
		}
		List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();
		GameWebAPI.RespDataMA_EventPointBonusM respDataMA_EventPointBonusMaster = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster;
		foreach (GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus eventPointBonus2 in respDataMA_EventPointBonusMaster.eventPointBonusM)
		{
			if (eventPointBonus2.worldDungeonId == b && eventPointBonus2.effectType != "0")
			{
				list.Add(eventPointBonus2);
			}
		}
		using (List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus eventPointBonus = enumerator.Current;
				ExtraEffectUtil.EventPointBonusTargetSubType eventPointBonusTargetSubType = (ExtraEffectUtil.EventPointBonusTargetSubType)int.Parse(eventPointBonus.targetSubType);
				switch (eventPointBonusTargetSubType)
				{
				case ExtraEffectUtil.EventPointBonusTargetSubType.MonsterTribe:
					if (this.characterDatas.tribe.Equals(eventPointBonus.targetValue))
					{
						return true;
					}
					break;
				case ExtraEffectUtil.EventPointBonusTargetSubType.MonsterGroup:
					if (this.characterStatus.groupId.Equals(eventPointBonus.targetValue))
					{
						return true;
					}
					break;
				case ExtraEffectUtil.EventPointBonusTargetSubType.GrowStep:
				{
					string text = MonsterGrowStepData.ToGrowStepString(this.characterDatas.growStep);
					if (text.Equals(eventPointBonus.targetValue))
					{
						return true;
					}
					break;
				}
				case ExtraEffectUtil.EventPointBonusTargetSubType.SkillId:
					if (this.skillStatus[2].skillId.Equals(eventPointBonus.targetValue))
					{
						return true;
					}
					if (this.leaderSkillStatus.leaderSkillId.Equals(eventPointBonus.targetValue))
					{
						return true;
					}
					break;
				case ExtraEffectUtil.EventPointBonusTargetSubType.ChipId:
					if (this.chipIds.Where((int item) => item == eventPointBonus.targetValue.ToInt32()).Any<int>())
					{
						return true;
					}
					break;
				default:
					if (eventPointBonusTargetSubType == ExtraEffectUtil.EventPointBonusTargetSubType.MonsterIntegrationGroup)
					{
						foreach (string value in this.characterStatus.monsterIntegrationIds)
						{
							if (eventPointBonus.targetValue.Equals(value))
							{
								return true;
							}
						}
					}
					break;
				}
			}
		}
		return false;
	}

	public BattleInvariant.Type hittingTheTargetType
	{
		get
		{
			return this.chip.hittingTheTargetType;
		}
	}

	public BattleInvariant.Type criticalTheTargetType
	{
		get
		{
			return this.chip.criticalTheTargetType;
		}
	}

	public string chipSkillId { get; set; }

	public string currentChipId { get; set; }

	public Dictionary<int, int> potencyChipIdList
	{
		get
		{
			return this.chip.potencyChipIdList;
		}
	}

	public Dictionary<int, int> stagingChipIdList
	{
		get
		{
			return this.chip.stagingChipIdList;
		}
	}

	public int chipAddMaxHp
	{
		get
		{
			return this.chip.chipAddMaxHp;
		}
	}

	public int chipAddAttackPower
	{
		get
		{
			return this.chip.chipAddAttackPower;
		}
	}

	public int chipAddDefencePower
	{
		get
		{
			return this.chip.chipAddDefencePower;
		}
	}

	public int chipAddSpecialAttackPower
	{
		get
		{
			return this.chip.chipAddSpecialAttackPower;
		}
	}

	public int chipAddSpecialDefencePower
	{
		get
		{
			return this.chip.chipAddSpecialDefencePower;
		}
	}

	public int chipAddSpeed
	{
		get
		{
			return this.chip.chipAddSpeed;
		}
	}

	public float chipAddCritical
	{
		get
		{
			return this.chip.chipAddCritical;
		}
	}

	public float chipAddHit
	{
		get
		{
			return this.chip.chipAddHit;
		}
	}

	public int stageChipAddAttackPower
	{
		get
		{
			return this.chip.stageChipAddAttackPower;
		}
	}

	public int stageChipAddSpecialAttackPower
	{
		get
		{
			return this.chip.stageChipAddSpecialAttackPower;
		}
	}

	public bool isChipServerAddValue
	{
		get
		{
			return this.chip.isServerAddValue;
		}
	}

	public string GetChipEffectCountToString()
	{
		return this.chip.GetChipEffectCountToString();
	}

	public string GetPotencyChipIdListToString()
	{
		return this.chip.GetPotencyChipIdListToString();
	}

	public void OnChipTrigger(EffectStatusBase.EffectTriggerType triggerType)
	{
		this.chip.OnChipTrigger(triggerType);
	}

	public void RemovePotencyChip(EffectStatusBase.EffectTriggerType triggerType)
	{
		this.chip.RemovePotencyChip(triggerType);
	}

	public void RemovePotencyChip(EffectStatusBase.EffectTriggerType[] triggerType)
	{
		this.chip.RemovePotencyChip(triggerType);
	}

	public void AddChipEffectCount(int chipEffectId, int value)
	{
		this.chip.AddChipEffectCount(chipEffectId, value);
	}

	public void InitChipEffectCountForWave()
	{
		this.chip.InitChipEffectCountForWave();
	}

	public void InitChipEffectCountForTurn()
	{
		this.chip.InitChipEffectCountForTurn();
	}

	public void ClearStagingChipIdList()
	{
		this.chip.stagingChipIdList.Clear();
	}

	public void ClearGutsData()
	{
		this.chip.ClearGutsData();
	}

	public void RemoveDeadStagingChips()
	{
		this.chip.RemoveDeadStagingChips();
	}

	public void ChangeLeaderSkill(LeaderSkillStatus leaderSkillStatus, CharacterDatas characterDatas)
	{
		this._leaderSkillResult = new LeaderSkillResult(this, leaderSkillStatus, characterDatas);
		this.InitializeExtraStatus();
		if (this._currentHp > this.extraMaxHp)
		{
			this._currentHp = this.extraMaxHp;
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

	public static CharacterParams[] ToParams(CharacterStateControl[] status)
	{
		List<CharacterParams> list = new List<CharacterParams>();
		foreach (CharacterStateControl characterStateControl in status)
		{
			list.Add(characterStateControl.CharacterParams);
		}
		return list.ToArray();
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

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
