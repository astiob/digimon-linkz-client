using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class BattleStateData
	{
		public const int AdditionalConsumptionDigiStoneNumber = 2;

		[NonSerialized]
		public Transform characterRoot;

		[NonSerialized]
		public Transform skillEffectRoot;

		[NonSerialized]
		public Transform stageRoot;

		[NonSerialized]
		public Transform spawnPointRoot;

		[NonSerialized]
		public Transform cameraMotionRoot;

		[NonSerialized]
		public Transform hitEffectRoot;

		[NonSerialized]
		public Transform alwaysEffectRoot;

		[NonSerialized]
		public Transform BattleInternalResourcesRoot;

		private TweenCameraTargetFunction _commandSelectTweenTargetCamera;

		private TweenCameraTargetFunction _bigBossCommandSelectTweenTargetCamera;

		private UnityObjectPooler<SpawnPointParams> _preloadSpawnPoints = new UnityObjectPooler<SpawnPointParams>();

		private Transform[] _playerCharactersSpawnPoint = new Transform[3];

		private Transform[] _enemiesSpawnPoint = new Transform[3];

		private Transform _stageSpawnPoint;

		private int[] _enemiesTargetSelectOrder = new int[]
		{
			0,
			2,
			1
		};

		private GameObject _stageObject;

		[SerializeField]
		private CharacterStateControl[] _playerCharacters = new CharacterStateControl[3];

		private CharacterStateControl[][] _preloadEnemies;

		[SerializeField]
		private CharacterStateControl[] _enemies = new CharacterStateControl[0];

		private UnityObjectPooler<CharacterParams> _preloadEnemiesParams = new UnityObjectPooler<CharacterParams>();

		private UnityObjectPooler<CharacterDatas> _preloadEnemiesCharacterDatas = new UnityObjectPooler<CharacterDatas>();

		private CharacterStateControl _leaderCharacter;

		private CharacterStateControl _leaderEnemyCharacter;

		private List<CharacterStateControl> _sortedCharactersOriginal = new List<CharacterStateControl>();

		private CharacterStateControl autoCounterCharacter;

		private int _maxDropNum;

		private List<ExtraEffectStatus> _extraEffectStatus = new List<ExtraEffectStatus>();

		private UnityObjectPooler<HitEffectParams[]> _hitEffects = new UnityObjectPooler<HitEffectParams[]>();

		private HitEffectParams _drainEffect;

		private HitEffectParams[] _playersDeathEffect = new HitEffectParams[0];

		private HitEffectParams[] _enemiesDeathEffect = new HitEffectParams[0];

		private HitEffectParams[] _enemiesLastDeadEffect = new HitEffectParams[0];

		private HitEffectParams[] _revivalEffect = new HitEffectParams[0];

		private HitEffectParams[] _waveChangeHpRevivalEffect = new HitEffectParams[0];

		private HitEffectParams[] _roundChangeApRevivalEffect = new HitEffectParams[0];

		private AlwaysEffectParams[] _revivalReservedEffect;

		private AlwaysEffectParams[] _droppingItemNormalEffect;

		private AlwaysEffectParams[] _droppingItemRareEffect;

		private AlwaysEffectParams[] _insertCharacterEffect;

		private AlwaysEffectParams[] _insertEnemyEffect;

		private AlwaysEffectParams[] _insertBossCharacterEffect;

		private AlwaysEffectParams[] _stageGimmickUpEffect;

		private AlwaysEffectParams[] _stageGimmickDownEffect;

		private BattleCameraSwitcher _winCameraMotionInternalResources;

		private LayerMask _characterColliderLayerMask = 0;

		private int _lastAttackPlayerCharacterIndex;

		private int _beforeConfirmDigiStoneNumber;

		private int _currentTurnNumber = 1;

		private int _currentRoundNumber = 1;

		private int _totalRoundNumber = 1;

		private int _currentWaveNumber = -1;

		private int _currentActiveCharacter;

		private int _currentSelectCharacterIndex;

		private int _currentSelectRevivalCharacter;

		private int _currentLastGenerateStartTimingSufferState;

		private int _currentDroppedNormalItem;

		private int _currentDroppedRareItem;

		private int _currentGettedNormalChip;

		private int _currentGetteddNormalExp;

		private int _currentGettedItemDropChip;

		private int _currentGettedItemDropExp;

		private bool _onSkillTrigger;

		private bool _useCharacterShadow = true;

		private bool _unFightLoss;

		private bool _isSkipWinnerAction;

		private bool _isPossibleTargetSelect = true;

		private bool _isImpossibleShowMenu;

		private bool _isEnableBackKeySystem = true;

		private bool _isRunnedRevivalFunction;

		private bool _isBattleRetired;

		private bool[] _isRoundStartApRevival = new bool[6];

		private bool[] _isRoundStartHpRevival = new bool[3];

		private bool[] _isRevivalReservedCharacter = new bool[3];

		private int[] _revivaledCharactersIndex = new int[3];

		private float? _pauseBgmVolume;

		private List<ItemDropResult> _itemDropResults = new List<ItemDropResult>();

		[SerializeField]
		private List<FraudDataLog> _fraudDataLogs = new List<FraudDataLog>();

		private int[] _apRevival = new int[3];

		private bool _isShowMenuWindow;

		private bool _isShowRetireWindow;

		private bool _isShowRevivalWindow;

		private bool _isShowContinueWindow;

		private bool _isContinueFlag;

		private bool _isShowCharacterDescription;

		private bool _isShowHelp;

		private bool _isInvocationEffectPlaying;

		private bool _isPossibleShowDescription = true;

		private bool _isSlowMotion;

		private bool _isShowSpecificTrade;

		private bool _isShowInitialIntroduction;

		private bool _isShowShop;

		private bool isChipSkill;

		public bool isConfusionAttack;

		public List<EffectStatusBase.EffectTriggerType> reqestStageEffectTriggerList = new List<EffectStatusBase.EffectTriggerType>();

		public bool isTutorialInduction;

		public int turnUseDigiStoneCount;

		public Dictionary<string, object> sendValues = new Dictionary<string, object>();

		public TweenCameraTargetFunction commandSelectTweenTargetCamera { get; set; }

		public TweenCameraTargetFunction normalCommandSelectTweenTargetCamera
		{
			get
			{
				return this._commandSelectTweenTargetCamera;
			}
			set
			{
				this._commandSelectTweenTargetCamera = value;
			}
		}

		public TweenCameraTargetFunction bigBossCommandSelectTweenTargetCamera
		{
			get
			{
				return this._bigBossCommandSelectTweenTargetCamera;
			}
			set
			{
				this._bigBossCommandSelectTweenTargetCamera = value;
			}
		}

		public UnityObjectPooler<SpawnPointParams> preloadSpawnPoints
		{
			get
			{
				return this._preloadSpawnPoints;
			}
		}

		public Transform[] playerCharactersSpawnPoint
		{
			get
			{
				return this._playerCharactersSpawnPoint;
			}
			private set
			{
				this._playerCharactersSpawnPoint = value;
			}
		}

		public Transform[] enemiesSpawnPoint
		{
			get
			{
				return this._enemiesSpawnPoint;
			}
			private set
			{
				this._enemiesSpawnPoint = value;
			}
		}

		public Transform stageSpawnPoint
		{
			get
			{
				return this._stageSpawnPoint;
			}
			private set
			{
				this._stageSpawnPoint = value;
			}
		}

		public int[] enemiesTargetSelectOrder
		{
			get
			{
				return this._enemiesTargetSelectOrder;
			}
			private set
			{
				this._enemiesTargetSelectOrder = value;
			}
		}

		public GameObject stageObject
		{
			get
			{
				return this._stageObject;
			}
			set
			{
				this._stageObject = value;
			}
		}

		public int leaderCharacterIndex { get; set; }

		public void ApplySpawnPoint(SpawnPointParams spawnPoint)
		{
			if (spawnPoint == null)
			{
				return;
			}
			this.playerCharactersSpawnPoint = spawnPoint.playersSpawnPoint;
			this.enemiesSpawnPoint = spawnPoint.enemiesSpawnPoint;
			this.stageSpawnPoint = spawnPoint.stageSpawnPoint;
			this.enemiesTargetSelectOrder = spawnPoint.enemiesTargetSelectOrder;
			this.normalCommandSelectTweenTargetCamera.SetTargets(spawnPoint.GetHalfSpanwPointPosition());
			if (this.bigBossCommandSelectTweenTargetCamera != null)
			{
				Vector3[] halfSpanwPointPosition = spawnPoint.GetHalfSpanwPointPosition();
				Vector3[] array = new Vector3[halfSpanwPointPosition.Length];
				for (int i = 0; i < halfSpanwPointPosition.Length; i++)
				{
					float num = 5f;
					array[i] = new Vector3(halfSpanwPointPosition[i].x, halfSpanwPointPosition[i].y + num, halfSpanwPointPosition[i].z);
				}
				this.bigBossCommandSelectTweenTargetCamera.SetTargets(array);
			}
		}

		public CharacterStateControl[] playerCharacters
		{
			get
			{
				return this._playerCharacters;
			}
			set
			{
				this._playerCharacters = value;
			}
		}

		public CharacterStateControl[][] preloadEnemies
		{
			get
			{
				return this._preloadEnemies;
			}
			set
			{
				this._preloadEnemies = value;
			}
		}

		public CharacterStateControl[] enemies
		{
			get
			{
				return this._enemies;
			}
			set
			{
				this._enemies = value;
			}
		}

		public UnityObjectPooler<CharacterParams> preloadEnemiesParams
		{
			get
			{
				return this._preloadEnemiesParams;
			}
		}

		public UnityObjectPooler<CharacterDatas> preloadEnemiesCharacterDatas
		{
			get
			{
				return this._preloadEnemiesCharacterDatas;
			}
		}

		public CharacterStateControl leaderCharacter
		{
			get
			{
				return this._leaderCharacter;
			}
			set
			{
				this._leaderCharacter = value;
			}
		}

		public CharacterStateControl leaderEnemyCharacter
		{
			get
			{
				return this._leaderEnemyCharacter;
			}
			set
			{
				this._leaderEnemyCharacter = value;
			}
		}

		public CharacterStateControl currentSelectCharacterState
		{
			get
			{
				if (this._sortedCharactersOriginal == null || this._sortedCharactersOriginal.Count <= this._currentActiveCharacter)
				{
					return null;
				}
				return this._sortedCharactersOriginal[this._currentActiveCharacter];
			}
		}

		public int maxCharacterLength
		{
			get
			{
				return Mathf.Max(this.playerCharacters.Length, this.enemies.Length);
			}
		}

		public int maxPreloadCharacterLength
		{
			get
			{
				return Mathf.Max(this.playerCharacters.Length, this.maxEnemiesLenght);
			}
		}

		public int totalCharacterLength
		{
			get
			{
				return this.playerCharacters.Length + this.enemies.Length;
			}
		}

		public int totalPreloadCharacterLength
		{
			get
			{
				return this.playerCharacters.Length + this.maxEnemiesLenght;
			}
		}

		public int maxEnemiesLenght
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.preloadEnemies.Length; i++)
				{
					num = Mathf.Max(num, this.preloadEnemies[i].Length);
				}
				return num;
			}
		}

		public int maxDropNum
		{
			get
			{
				if (this._maxDropNum == 0)
				{
					for (int i = 0; i < this.preloadEnemies.Length; i++)
					{
						int num = 0;
						foreach (CharacterStateControl characterStateControl in this.preloadEnemies[i])
						{
							num += characterStateControl.itemDropResult.Count;
						}
						if (this._maxDropNum < num)
						{
							this._maxDropNum = num;
						}
					}
					if (this._maxDropNum == 0)
					{
						this._maxDropNum = 1;
					}
				}
				return this._maxDropNum;
			}
		}

		public void SetOrderInSortedCharacter(List<CharacterStateControl> sortedCharacters, int prevIndex = -1)
		{
			if (sortedCharacters == null)
			{
				this._sortedCharactersOriginal = sortedCharacters;
				return;
			}
			this._sortedCharactersOriginal = new List<CharacterStateControl>();
			foreach (CharacterStateControl item in sortedCharacters)
			{
				this._sortedCharactersOriginal.Add(item);
			}
			int num = 0;
			for (int i = 0; i < this._sortedCharactersOriginal.Count; i++)
			{
				if (this._sortedCharactersOriginal[i].isDied || i <= prevIndex)
				{
					this._sortedCharactersOriginal[i].skillOrder = -1;
				}
				else
				{
					this._sortedCharactersOriginal[i].skillOrder = num;
					num++;
				}
			}
		}

		public CharacterStateControl[] GetTotalCharacters()
		{
			CharacterStateControl[] array = new CharacterStateControl[this.totalCharacterLength];
			for (int i = 0; i < this.totalCharacterLength; i++)
			{
				if (i < this.playerCharacters.Length)
				{
					array[i] = this.playerCharacters[i];
				}
				else
				{
					array[i] = this.enemies[i - this.playerCharacters.Length];
				}
			}
			return array;
		}

		public CharacterStateControl[] GetTotalCharactersEnemyFirst()
		{
			CharacterStateControl[] array = new CharacterStateControl[this.totalCharacterLength];
			for (int i = 0; i < this.totalCharacterLength; i++)
			{
				if (i < this.enemies.Length)
				{
					array[i] = this.enemies[i];
				}
				else
				{
					array[i] = this.playerCharacters[i - this.enemies.Length];
				}
			}
			return array;
		}

		public int GetTotalCharacterIndex(CharacterStateControl playerOrEnemy)
		{
			return ((!playerOrEnemy.isEnemy) ? 0 : this.playerCharacters.Length) + playerOrEnemy.myIndex;
		}

		public bool GetCharactersDeath(bool isEnemy = false)
		{
			int num = 0;
			if (!isEnemy)
			{
				for (int i = 0; i < this.playerCharacters.Length; i++)
				{
					if (this.playerCharacters[i].isDied)
					{
						num++;
					}
				}
				if (num == this.playerCharacters.Length)
				{
					return true;
				}
			}
			else
			{
				for (int j = 0; j < this.enemies.Length; j++)
				{
					if (this.enemies[j].isDied)
					{
						num++;
					}
				}
				if (num == this.enemies.Length)
				{
					return true;
				}
			}
			return false;
		}

		public void ChangePlayerLeader(int index)
		{
			BattleStateManager current = BattleStateManager.current;
			current.hierarchyData.leaderCharacter = index;
			this.leaderCharacter = this.playerCharacters[index];
			this.leaderCharacterIndex = index;
			LeaderSkillStatus leaderSkillStatus = this.playerCharacters[index].leaderSkillStatus;
			CharacterDatas characterDatas = this.playerCharacters[index].characterDatas;
			for (int i = 0; i < this.playerCharacters.Length; i++)
			{
				bool isLeader = i == index;
				current.uiControl.ApplyLeaderIcon(i, isLeader);
				this.playerCharacters[i].isLeader = isLeader;
				this.playerCharacters[i].ChangeLeaderSkill(leaderSkillStatus, characterDatas);
			}
		}

		public void ChangeEnemyLeader(int index)
		{
			this.leaderEnemyCharacter = this.enemies[index];
			LeaderSkillStatus leaderSkillStatus = this.enemies[index].leaderSkillStatus;
			CharacterDatas characterDatas = this.enemies[index].characterDatas;
			for (int i = 0; i < this.enemies.Length; i++)
			{
				bool isLeader = i == index;
				this.enemies[i].isLeader = isLeader;
				this.enemies[i].ChangeLeaderSkill(leaderSkillStatus, characterDatas);
			}
		}

		public List<ExtraEffectStatus> extraEffectStatus
		{
			get
			{
				return this._extraEffectStatus;
			}
			set
			{
				this._extraEffectStatus = value;
			}
		}

		public UnityObjectPooler<HitEffectParams[]> hitEffects
		{
			get
			{
				return this._hitEffects;
			}
			set
			{
				this._hitEffects = value;
			}
		}

		public HitEffectParams drainEffect
		{
			get
			{
				return this._drainEffect;
			}
			set
			{
				this._drainEffect = value;
			}
		}

		public HitEffectParams[] playersDeathEffect
		{
			get
			{
				return this._playersDeathEffect;
			}
			set
			{
				this._playersDeathEffect = value;
			}
		}

		public HitEffectParams[] enemiesDeathEffect
		{
			get
			{
				return this._enemiesDeathEffect;
			}
			set
			{
				this._enemiesDeathEffect = value;
			}
		}

		public HitEffectParams[] enemiesLastDeadEffect
		{
			get
			{
				return this._enemiesLastDeadEffect;
			}
			set
			{
				this._enemiesLastDeadEffect = value;
			}
		}

		public HitEffectParams[] revivalEffect
		{
			get
			{
				return this._revivalEffect;
			}
			set
			{
				this._revivalEffect = value;
			}
		}

		public HitEffectParams[] waveChangeHpRevivalEffect
		{
			get
			{
				return this._waveChangeHpRevivalEffect;
			}
			set
			{
				this._waveChangeHpRevivalEffect = value;
			}
		}

		public HitEffectParams[] roundChangeApRevivalEffect
		{
			get
			{
				return this._roundChangeApRevivalEffect;
			}
			set
			{
				this._roundChangeApRevivalEffect = value;
			}
		}

		public HitEffectParams[] GetDamageEffect(Strength strength)
		{
			return this.hitEffects.GetObject(AffectEffect.Damage.ToString(), strength.ToString());
		}

		public AlwaysEffectParams[] revivalReservedEffect
		{
			get
			{
				return this._revivalReservedEffect;
			}
			set
			{
				this._revivalReservedEffect = value;
			}
		}

		public AlwaysEffectParams[] droppingItemNormalEffect
		{
			get
			{
				return this._droppingItemNormalEffect;
			}
			set
			{
				this._droppingItemNormalEffect = value;
			}
		}

		public AlwaysEffectParams[] droppingItemRareEffect
		{
			get
			{
				return this._droppingItemRareEffect;
			}
			set
			{
				this._droppingItemRareEffect = value;
			}
		}

		public AlwaysEffectParams[] insertCharacterEffect
		{
			get
			{
				return this._insertCharacterEffect;
			}
			set
			{
				this._insertCharacterEffect = value;
			}
		}

		public AlwaysEffectParams[] insertEnemyEffect
		{
			get
			{
				return this._insertEnemyEffect;
			}
			set
			{
				this._insertEnemyEffect = value;
			}
		}

		public AlwaysEffectParams[] insertBossCharacterEffect
		{
			get
			{
				return this._insertBossCharacterEffect;
			}
			set
			{
				this._insertBossCharacterEffect = value;
			}
		}

		public AlwaysEffectParams[] stageGimmickUpEffect
		{
			get
			{
				return this._stageGimmickUpEffect;
			}
			set
			{
				this._stageGimmickUpEffect = value;
			}
		}

		public AlwaysEffectParams[] stageGimmickDownEffect
		{
			get
			{
				return this._stageGimmickDownEffect;
			}
			set
			{
				this._stageGimmickDownEffect = value;
			}
		}

		public AlwaysEffectParams[] GetIsActiveRevivalReservedEffect()
		{
			List<AlwaysEffectParams> list = new List<AlwaysEffectParams>();
			for (int i = 0; i < this._isRevivalReservedCharacter.Length; i++)
			{
				if (this._isRevivalReservedCharacter[i])
				{
					list.Add(this._revivalReservedEffect[i]);
				}
			}
			return list.ToArray();
		}

		public BattleCameraSwitcher winCameraMotionInternalResources
		{
			get
			{
				return this._winCameraMotionInternalResources;
			}
			set
			{
				this._winCameraMotionInternalResources = value;
			}
		}

		public void RemoveAllCachedObjects()
		{
			BattleObjectPooler.SetAllDeactive();
			if (this._commandSelectTweenTargetCamera != null)
			{
				UnityEngine.Object.Destroy(this._commandSelectTweenTargetCamera.gameObject);
			}
			if (this._stageObject != null)
			{
				UnityEngine.Object.Destroy(this._stageObject);
			}
			foreach (SpawnPointParams spawnPointParams in this._preloadSpawnPoints.GetAllObject())
			{
				if (spawnPointParams != null)
				{
					UnityEngine.Object.Destroy(spawnPointParams.gameObject);
				}
			}
			foreach (CharacterParams characterParams in this._preloadEnemiesParams.GetAllObject())
			{
				if (!(characterParams == null))
				{
					UnityEngine.Object.Destroy(characterParams.gameObject);
				}
			}
			foreach (CharacterStateControl characterStateControl in this._playerCharacters)
			{
				if (!(characterStateControl.CharacterParams == null))
				{
					UnityEngine.Object.Destroy(characterStateControl.CharacterParams.gameObject);
				}
			}
			foreach (HitEffectParams array in this.hitEffects.GetAllObject())
			{
				foreach (HitEffectParams hitEffectParams in array)
				{
					if (hitEffectParams != null && !BattleObjectPooler.ContainHitEffectParams(hitEffectParams))
					{
						UnityEngine.Object.Destroy(hitEffectParams.gameObject);
					}
				}
			}
			if (this._drainEffect != null)
			{
				UnityEngine.Object.Destroy(this._drainEffect.gameObject);
			}
			foreach (HitEffectParams hitEffectParams2 in this._playersDeathEffect)
			{
				if (hitEffectParams2 != null)
				{
					UnityEngine.Object.Destroy(hitEffectParams2.gameObject);
				}
			}
			foreach (HitEffectParams hitEffectParams3 in this._enemiesDeathEffect)
			{
				if (hitEffectParams3 != null && !BattleObjectPooler.ContainHitEffectParams(hitEffectParams3))
				{
					UnityEngine.Object.Destroy(hitEffectParams3.gameObject);
				}
			}
			foreach (HitEffectParams hitEffectParams4 in this._revivalEffect)
			{
				if (hitEffectParams4 != null && !BattleObjectPooler.ContainHitEffectParams(hitEffectParams4))
				{
					UnityEngine.Object.Destroy(hitEffectParams4.gameObject);
				}
			}
			foreach (AlwaysEffectParams alwaysEffectParams in this._revivalReservedEffect)
			{
				if (alwaysEffectParams != null && !BattleObjectPooler.ContainAlwaysEffectParams(alwaysEffectParams))
				{
					UnityEngine.Object.Destroy(alwaysEffectParams.gameObject);
				}
			}
			foreach (AlwaysEffectParams alwaysEffectParams2 in this._droppingItemNormalEffect)
			{
				if (alwaysEffectParams2 != null && !BattleObjectPooler.ContainAlwaysEffectParams(alwaysEffectParams2))
				{
					UnityEngine.Object.Destroy(alwaysEffectParams2.gameObject);
				}
			}
			foreach (AlwaysEffectParams alwaysEffectParams3 in this._droppingItemRareEffect)
			{
				if (alwaysEffectParams3 != null && !BattleObjectPooler.ContainAlwaysEffectParams(alwaysEffectParams3))
				{
					UnityEngine.Object.Destroy(alwaysEffectParams3.gameObject);
				}
			}
			if (this._winCameraMotionInternalResources != null)
			{
				UnityEngine.Object.Destroy(this._winCameraMotionInternalResources.gameObject);
			}
			BattleObjectPooler.isCheckEnable = true;
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		public LayerMask characterColliderLayerMask
		{
			get
			{
				return this._characterColliderLayerMask;
			}
			set
			{
				this._characterColliderLayerMask = value;
			}
		}

		public int lastAttackPlayerCharacterIndex
		{
			get
			{
				return this._lastAttackPlayerCharacterIndex;
			}
			set
			{
				this._lastAttackPlayerCharacterIndex = value;
			}
		}

		public int currentDigiStoneNumber
		{
			get
			{
				return this._beforeConfirmDigiStoneNumber - this.turnUseDigiStoneCount;
			}
		}

		public int beforeConfirmDigiStoneNumber
		{
			get
			{
				return this._beforeConfirmDigiStoneNumber;
			}
			set
			{
				this._beforeConfirmDigiStoneNumber = Mathf.Clamp(value, 0, int.MaxValue);
			}
		}

		public int currentTurnNumber
		{
			get
			{
				return this._currentTurnNumber;
			}
			set
			{
				this._currentTurnNumber = Mathf.Clamp(value, -1, int.MaxValue);
			}
		}

		public int currentRoundNumber
		{
			get
			{
				return this._currentRoundNumber;
			}
			set
			{
				this._currentRoundNumber = Mathf.Clamp(value, 1, int.MaxValue);
			}
		}

		public int totalRoundNumber
		{
			get
			{
				return this._totalRoundNumber;
			}
			set
			{
				this._totalRoundNumber = Mathf.Clamp(value, 1, int.MaxValue);
			}
		}

		public int currentWaveNumber
		{
			get
			{
				return this._currentWaveNumber;
			}
			set
			{
				this._currentWaveNumber = Mathf.Clamp(value, 0, int.MaxValue);
			}
		}

		public int currentWaveNumberGUI
		{
			get
			{
				return this.currentWaveNumber + 1;
			}
		}

		public int currentActiveCharacter
		{
			get
			{
				return this._currentActiveCharacter;
			}
			set
			{
				this._currentActiveCharacter = ((this._sortedCharactersOriginal != null) ? Mathf.Clamp(value, 0, this._sortedCharactersOriginal.Count) : value);
			}
		}

		public int currentSelectCharacterIndex
		{
			get
			{
				return this._currentSelectCharacterIndex;
			}
			set
			{
				this._currentSelectCharacterIndex = Mathf.Clamp(value, 0, this.maxCharacterLength);
			}
		}

		public int currentSelectRevivalCharacter
		{
			get
			{
				return this._currentSelectRevivalCharacter;
			}
			set
			{
				this._currentSelectRevivalCharacter = Mathf.Clamp(value, 0, this.maxCharacterLength);
			}
		}

		public int currentDroppedNormalItem
		{
			get
			{
				return this._currentDroppedNormalItem;
			}
			set
			{
				this._currentDroppedNormalItem = Mathf.Clamp(value, 0, int.MaxValue);
			}
		}

		public int currentDroppedRareItem
		{
			get
			{
				return this._currentDroppedRareItem;
			}
			set
			{
				this._currentDroppedRareItem = Mathf.Clamp(value, 0, int.MaxValue);
			}
		}

		public int currentGettedChip
		{
			get
			{
				return this.currentGettedNormalChip + this.currentGettedItemDropChip;
			}
		}

		public int currentGettedExp
		{
			get
			{
				return this.currentGettedNormalExp + this.currentGettedItemDropExp;
			}
		}

		public int currentGettedNormalChip
		{
			get
			{
				return this._currentGettedNormalChip;
			}
			set
			{
				this._currentGettedNormalChip = value;
			}
		}

		public int currentGettedNormalExp
		{
			get
			{
				return this._currentGetteddNormalExp;
			}
			set
			{
				this._currentGetteddNormalExp = value;
			}
		}

		public int currentGettedItemDropChip
		{
			get
			{
				return this._currentGettedItemDropChip;
			}
			set
			{
				this._currentGettedItemDropChip = value;
			}
		}

		public int currentGettedItemDropExp
		{
			get
			{
				return this._currentGettedItemDropExp;
			}
			set
			{
				this._currentGettedItemDropExp = value;
			}
		}

		public bool onSkillTrigger
		{
			get
			{
				return this._onSkillTrigger;
			}
			set
			{
				this._onSkillTrigger = value;
			}
		}

		public bool useCharacterShadow
		{
			get
			{
				return this._useCharacterShadow;
			}
			set
			{
				this._useCharacterShadow = value;
			}
		}

		public bool unFightLoss
		{
			get
			{
				return this._unFightLoss;
			}
			set
			{
				this._unFightLoss = value;
			}
		}

		public bool isSkipWinnerAction
		{
			get
			{
				return this._isSkipWinnerAction;
			}
			set
			{
				this._isSkipWinnerAction = value;
			}
		}

		public bool isPossibleTargetSelect
		{
			get
			{
				return this._isPossibleTargetSelect;
			}
			set
			{
				this._isPossibleTargetSelect = value;
			}
		}

		public bool isRunnedRevivalFunction
		{
			get
			{
				return this._isRunnedRevivalFunction;
			}
			set
			{
				this._isRunnedRevivalFunction = value;
			}
		}

		public bool isBattleRetired
		{
			get
			{
				return this._isBattleRetired;
			}
			set
			{
				this._isBattleRetired = value;
			}
		}

		public bool isImpossibleShowMenu
		{
			get
			{
				return this._isImpossibleShowMenu;
			}
			set
			{
				this._isImpossibleShowMenu = value;
			}
		}

		public bool isEnableBackKeySystem
		{
			get
			{
				return this._isEnableBackKeySystem;
			}
			set
			{
				this._isEnableBackKeySystem = value;
			}
		}

		public bool enableRotateCam { get; set; }

		public bool[] isRoundStartApRevival
		{
			get
			{
				return this._isRoundStartApRevival;
			}
			set
			{
				this._isRoundStartApRevival = value;
			}
		}

		public bool[] isRoundStartHpRevival
		{
			get
			{
				return this._isRoundStartHpRevival;
			}
			set
			{
				this._isRoundStartHpRevival = value;
			}
		}

		public bool[] isRevivalReservedCharacter
		{
			get
			{
				return this._isRevivalReservedCharacter;
			}
			set
			{
				this._isRevivalReservedCharacter = value;
			}
		}

		public int[] revivaledCharactersIndex
		{
			get
			{
				return this._revivaledCharactersIndex;
			}
			set
			{
				this._revivaledCharactersIndex = value;
			}
		}

		public int[] apRevival
		{
			get
			{
				return this._apRevival;
			}
			set
			{
				this._apRevival = value;
			}
		}

		public float? pauseBgmVolume
		{
			get
			{
				return this._pauseBgmVolume;
			}
			set
			{
				this._pauseBgmVolume = value;
			}
		}

		public List<ItemDropResult> itemDropResults
		{
			get
			{
				return this._itemDropResults;
			}
			set
			{
				this._itemDropResults = value;
			}
		}

		public List<FraudDataLog> fraudDataLogs
		{
			get
			{
				return this._fraudDataLogs;
			}
			set
			{
				this._fraudDataLogs = value;
			}
		}

		public bool isShowMenuWindow
		{
			get
			{
				return this._isShowMenuWindow;
			}
			set
			{
				this._isShowMenuWindow = value;
			}
		}

		public bool isShowRetireWindow
		{
			get
			{
				return this._isShowRetireWindow;
			}
			set
			{
				this._isShowRetireWindow = value;
			}
		}

		public bool isShowRevivalWindow
		{
			get
			{
				return this._isShowRevivalWindow;
			}
			set
			{
				this._isShowRevivalWindow = value;
			}
		}

		public bool isShowContinueWindow
		{
			get
			{
				return this._isShowContinueWindow;
			}
			set
			{
				this._isShowContinueWindow = value;
			}
		}

		public bool isContinueFlag
		{
			get
			{
				return this._isContinueFlag;
			}
			set
			{
				this._isContinueFlag = value;
			}
		}

		public bool isShowCharacterDescription
		{
			get
			{
				return this._isShowCharacterDescription;
			}
			set
			{
				this._isShowCharacterDescription = value;
			}
		}

		public bool isShowHelp
		{
			get
			{
				return this._isShowHelp;
			}
			set
			{
				this._isShowHelp = value;
			}
		}

		public bool isInvocationEffectPlaying
		{
			get
			{
				return this._isInvocationEffectPlaying;
			}
			set
			{
				this._isInvocationEffectPlaying = value;
			}
		}

		public bool isPossibleShowDescription
		{
			get
			{
				return this._isPossibleShowDescription;
			}
			set
			{
				this._isPossibleShowDescription = value;
			}
		}

		public bool isSlowMotion
		{
			get
			{
				return this._isSlowMotion;
			}
			set
			{
				this._isSlowMotion = value;
			}
		}

		public bool isShowSpecificTrade
		{
			get
			{
				return this._isShowSpecificTrade;
			}
			set
			{
				this._isShowSpecificTrade = value;
			}
		}

		public bool isShowInitialIntroduction
		{
			get
			{
				return this._isShowInitialIntroduction;
			}
			set
			{
				this._isShowInitialIntroduction = value;
			}
		}

		public bool isShowShop
		{
			get
			{
				return this._isShowShop;
			}
			set
			{
				this._isShowShop = value;
			}
		}

		public int currentLastGenerateStartTimingSufferState
		{
			get
			{
				return this._currentLastGenerateStartTimingSufferState;
			}
			set
			{
				this._currentLastGenerateStartTimingSufferState = Mathf.Clamp(value, 0, int.MaxValue);
			}
		}

		public bool isFindRevivalCharacter
		{
			get
			{
				return !BoolExtension.AllMachValue(false, this.isRevivalReservedCharacter);
			}
		}

		public void SetApRevival(CharacterStateControl character, bool isApRevival = true)
		{
			List<CharacterStateControl> list = new List<CharacterStateControl>(this.GetTotalCharacters());
			int num = list.IndexOf(character);
			if (num < 0 || this.apRevival.Length <= num || this.apRevival[num] == 1)
			{
				return;
			}
			this.apRevival[num] = ((!isApRevival) ? 0 : 1);
		}

		public void SetPlayPassiveEffectFunctionValues(CharacterStateControl[] isTargetsStatus, SkillStatus status, AffectEffectProperty currentSuffer)
		{
			this.sendValues["isTargetsStatus"] = isTargetsStatus;
			this.sendValues["status"] = status;
			this.sendValues["currentSuffer"] = currentSuffer;
		}

		public bool IsChipSkill()
		{
			return this.isChipSkill;
		}

		public void SetChipSkillFlag(bool chipSkill)
		{
			this.isChipSkill = chipSkill;
		}

		public CharacterStateControl GetAutoCounterCharacter()
		{
			return this.autoCounterCharacter;
		}

		public void SetAutoCounterCharacter(CharacterStateControl character)
		{
			this.autoCounterCharacter = character;
		}
	}
}
