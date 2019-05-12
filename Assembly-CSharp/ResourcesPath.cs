using StatusObject;
using System;
using UnityEngine;
using UnityExtension;

public struct ResourcesPath
{
	public const string publicPath = "Public";

	public const string characterPath = "Characters";

	public const string characterThumbnailPath = "CharacterThumbnail";

	public const string playersStatusPath = "PlayerStatus";

	public const string enemiesStatusPath = "EnemyStatus";

	public const string toleranceStatusPath = "ToleranceStatus";

	public const string extraEffectStatusPath = "ExtraEffectStatus";

	public const string invocationEffectPath = "DeathblowEffects";

	public const string passiveEffectPath = "InheritanceTechniqueEffects";

	public const string skillStatusPath = "SkillStatus";

	public const string leaderSkillStatusPath = "LeaderSkillStatus";

	public const string stagePath = "Stages";

	public const string spanwPointPath = "SpawnPoints";

	public const string cameraMotionPath = "CameraMotions";

	public const string hitEffectPath = "HitEffects";

	public const string alwaysEffectPath = "AlwaysEffects";

	public const string skillSePath = "SE";

	public const string internalCommonSePath = "SEInternal/Common";

	public const string internalBattleSePath = "SEInternal/Battle";

	public const string bgmPath = "BGM";

	public const string battleInternalResourcesPath = "BattleInternalResources";

	public const string uiBattlePath = "UIBattle";

	public static string CreatePath(params string[] directory)
	{
		return StringExtension.CreatePath(directory);
	}

	public static GameObject GetCharacterPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"Characters",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("キャラクターデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static PlayerStatus GetPlayerStatus(string id)
	{
		PlayerStatusObject playerStatusObject = Resources.Load<PlayerStatusObject>(ResourcesPath.CreatePath(new string[]
		{
			"PlayerStatus",
			id,
			"status"
		}));
		if (playerStatusObject == null)
		{
			global::Debug.LogWarning("キャラクターデータが存在しません. (" + id + ")");
			return new PlayerStatus();
		}
		return playerStatusObject.playerStatus;
	}

	public static EnemyStatus GetEnemyStatus(string id)
	{
		EnemyStatusObject enemyStatusObject = Resources.Load<EnemyStatusObject>(ResourcesPath.CreatePath(new string[]
		{
			"EnemyStatus",
			id,
			"status"
		}));
		if (enemyStatusObject == null)
		{
			global::Debug.LogWarning("キャラクターデータが存在しません. (" + id + ")");
			return new EnemyStatus();
		}
		return enemyStatusObject.enemyStatus;
	}

	public static Tolerance GetToleranceStatus(string id)
	{
		ToleranceObject toleranceObject = Resources.Load<ToleranceObject>(ResourcesPath.CreatePath(new string[]
		{
			"ToleranceStatus",
			id,
			"status"
		}));
		if (toleranceObject == null)
		{
			global::Debug.LogWarning("耐性データが存在しません. (" + id + ")");
			return Tolerance.GetNutralTolerance();
		}
		return toleranceObject.tolerance;
	}

	public static Sprite GetCharacterThumbnail(string id)
	{
		Sprite sprite = Resources.Load<Sprite>(ResourcesPath.CreatePath(new string[]
		{
			"CharacterThumbnail",
			id,
			"thumb"
		}));
		if (sprite == null)
		{
			Texture2D texture2D = Resources.Load<Texture2D>(ResourcesPath.CreatePath(new string[]
			{
				"CharacterThumbnail",
				id,
				"thumb"
			}));
			if (texture2D != null)
			{
				sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
			}
		}
		if (sprite == null)
		{
			global::Debug.LogWarning("キャラクターサムネイルデータが存在しません. (" + id + ")");
		}
		return sprite;
	}

	public static CharacterDatas GetCharacterData(string id)
	{
		CharacterDatasObject characterDatasObject = Resources.Load<CharacterDatasObject>(ResourcesPath.CreatePath(new string[]
		{
			"Characters",
			id,
			"data"
		}));
		if (characterDatasObject == null)
		{
			global::Debug.LogWarning("キャラクターデータが存在しません. (" + id + ")");
			return new CharacterDatas();
		}
		return characterDatasObject.characterDatas;
	}

	public static GameObject GetInvocationEffectPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"DeathblowEffects",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("発動エフェクトデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static GameObject GetPassiveEffectPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"InheritanceTechniqueEffects",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("受動エフェクトデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static SkillStatus GetSkillStatus(string id)
	{
		SkillStatusObject skillStatusObject = Resources.Load<SkillStatusObject>(ResourcesPath.CreatePath(new string[]
		{
			"SkillStatus",
			id,
			"status"
		}));
		if (skillStatusObject == null)
		{
			global::Debug.LogWarning("スキルステータスデータが存在しません. (" + id + ")");
			return new SkillStatus();
		}
		return skillStatusObject.skillStatus;
	}

	public static LeaderSkillStatus GetLeaderSkillStatus(string id)
	{
		LeaderSkillStatusObject leaderSkillStatusObject = Resources.Load<LeaderSkillStatusObject>(ResourcesPath.CreatePath(new string[]
		{
			"LeaderSkillStatus",
			id,
			"status"
		}));
		if (leaderSkillStatusObject == null)
		{
			global::Debug.LogWarning("リーダースキルステータスデータが存在しません. (" + id + ")");
			return LeaderSkillStatus.GetUnHavingLeaderSkill();
		}
		return leaderSkillStatusObject.leaderSkillStatus;
	}

	public static ExtraEffectStatus GetExtraEffectStatus(string id)
	{
		ExtraEffectStatusObject extraEffectStatusObject = Resources.Load<ExtraEffectStatusObject>(ResourcesPath.CreatePath(new string[]
		{
			"ExtraEffectStatus",
			id,
			"status"
		}));
		if (extraEffectStatusObject == null)
		{
			global::Debug.LogWarning("エリア効果データが存在しません. (" + id + ")");
			return new ExtraEffectStatus();
		}
		return extraEffectStatusObject.extraEffectStatus;
	}

	public static GameObject GetHitEffectPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"HitEffects",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("ヒットエフェクトデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static GameObject GetAlwaysEffectPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"AlwaysEffects",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("常設エフェクトデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static GameObject GetStagePrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"Stages",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("ステージデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static GameObject GetSpawnPointPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"SpawnPoints",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("出現ポイントデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static GameObject GetCameraMotionPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"CameraMotions",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogWarning("カメラモーションデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static GameObject GetBattleInternalResourcesPrefab(string id)
	{
		GameObject gameObject = Resources.Load<GameObject>(ResourcesPath.CreatePath(new string[]
		{
			"BattleInternalResources",
			id,
			"prefab"
		}));
		if (gameObject == null)
		{
			global::Debug.LogError("内部リソースデータが存在しません. (" + id + ")");
		}
		return gameObject;
	}

	public static GameObject GetUIBattlePrefab(string objName)
	{
		string text = "UIBattle/" + objName;
		GameObject gameObject = Resources.Load<GameObject>(text);
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("内部リソースデータが存在しません.", new object[]
			{
				text
			});
		}
		return gameObject;
	}

	public static GameObject GetPvPPrefab(string objName)
	{
		string text = "UIBattle/PvP/" + objName;
		GameObject gameObject = Resources.Load<GameObject>(text);
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("内部リソースデータが存在しません.", new object[]
			{
				text
			});
		}
		return gameObject;
	}

	public struct ReservedID
	{
		public struct CameraMotions
		{
			public const string battleStart = "0002_roundStart";

			public const string pvpBattleStartPlayerInsert = "pvpBattleStart";

			public const string pvpBattleStartEnemyInsert = "pvpBattleStart";

			public const string bossBattleStart = "0001_bossStart";

			public const string roundStart = "0002_command";

			public const string commandSelectPlayer = "0002_command_ally";

			public const string behavioralIncapacitation = "0006_behIncap";

			public const string characterView = "0007_commandCharaView";

			public const string bigBossBattleStart = "BigBoss/0001_Start";

			public const string bigBossRoundStart = "BigBoss/0002_command";

			public const string bigBossEnemyTurn = "BigBoss/0002_enemy";

			public const string bigBossCharacterView = "BigBoss/0007_commandCharaView";

			public const string bigBossWithdrawal = "BigBoss/0008_withdrawal";

			public struct AttackCameraMotion
			{
				public const string skillHitSimple = "skillF";

				public const string skillHitAll = "skillA";
			}

			public struct BigBossAttackCameraMotion
			{
				public const string skillHitSimple = "BigBoss/skillF";

				public const string skillHitAll = "BigBoss/skillA";
			}
		}

		public struct HitEffects
		{
			public const string damageWeak = "EFF_COM_HIT_CRITICAL";

			public const string damageStrong = "EFF_COM_HIT_WEAK";

			public const string damageNone = "EFF_COM_HIT_NORMAL";

			public const string damageInvalid = "EFF_COM_HIT_WEAK";

			public const string damageDrain = "EFF_COM_S_HEAL";

			public const string poison = "EFF_COM_POISONATTACK";

			public const string confusion = "EFF_COM_CONFUSIONATTACK";

			public const string paralysis = "EFF_COM_PARALYSISATTACK";

			public const string sleep = "EFF_COM_SLEEPATTACK";

			public const string stun = "EFF_COM_BUGATTACK";

			public const string skillLock = "EFF_COM_CRYSTALATTACK";

			public const string instantDeath = "EFF_COM_DEATHATTACK";

			public const string attackUp = "EFF_COM_UP";

			public const string attackDown = "EFF_COM_DOWN";

			public const string defenceUp = "EFF_COM_UP";

			public const string defenceDown = "EFF_COM_DOWN";

			public const string specialAttackUp = "EFF_COM_UP";

			public const string specialAttackDown = "EFF_COM_DOWN";

			public const string specialDefenceUp = "EFF_COM_UP";

			public const string specialDefenceDown = "EFF_COM_DOWN";

			public const string speedUp = "EFF_COM_UP";

			public const string speedDown = "EFF_COM_DOWN";

			public const string correctionUpReset = "EFF_COM_DOWN";

			public const string correctionDownReset = "EFF_COM_UP";

			public const string hpRevival = "EFF_COM_M_HEAL";

			public const string counter = "EFF_COM_COUNTER_P";

			public const string reflection = "EFF_COM_COUNTER_M";

			public const string protect = "EFF_COM_GUARD";

			public const string hateUp = "EFF_COM_DOWN";

			public const string hateDown = "EFF_COM_UP";

			public const string powerCharge = "EFF_COM_SPIRIT";

			public const string destruct = "EFF_COM_DEATH";

			public const string hitRateUp = "EFF_COM_UP";

			public const string hitRateDown = "EFF_COM_DOWN";

			public const string sufferStatusClear = "EFF_COM_UP";

			public const string satisfactionRateUp = "EFF_COM_UP";

			public const string satisfactionRateDown = "EFF_COM_DOWN";

			public const string apRevival = "EFF_COM_M_HEAL";

			public const string apUp = "EFF_COM_UP";

			public const string apDown = "EFF_COM_DOWN";

			public const string apConsumptionUp = "EFF_COM_DOWN";

			public const string apConsumptionDown = "EFF_COM_UP";

			public const string gimmickSpecialAttackUp = "EFF_COM_SPECIALATTACK_U";

			public const string gimmickSpecialAttackDown = "EFF_COM_SPECIALATTACK_D";

			public const string death = "EFF_COM_DEATH";

			public const string deathLast = "EFF_COM_BOSSDEATH";

			public const string waveHpRevival = "EFF_COM_L_HEAL";

			public const string roundApRevival = "EFF_COM_APUP";

			public const string turnBarrier = "EFF_COM_GUARD";

			public const string countBarrier = "EFF_COM_GUARD";

			public const string countGuard = "EFF_COM_GUARD";

			public const string recommand = "EFF_COM_M_HEAL";

			public const string damageRateUp = "EFF_COM_UP";

			public const string damageRateDown = "EFF_COM_DOWN";

			public const string turnEvasion = "EFF_COM_UP";

			public const string countEvasion = "EFF_COM_UP";

			public const string referenceTargetHpRate = "EFF_COM_HIT_RATIO";

			public const string apDrain = "EFF_COM_S_APDRAIN";

			public const string escape = "EFF_COM_UP";
		}

		public struct AlwaysEffects
		{
			public const string revivalReserved = "revivalReservationEffect";

			public const string droppingItemNormal = "droppingItemEffectNormal";

			public const string droppingItemRare = "droppingItemEffectRare";

			public const string insertCharacter = "insertCharacterEffect";

			public const string insertBossCharacter = "insertBossCharacterEffect";

			public const string insertPvPCharacter = "insertPVPCharacterEffect";

			public const string insertPvPEnemy = "insertPVPEnemyEffect";

			public const string stageGimmickUp = "EFF_COM_GimmickUP";

			public const string stageGimmickDown = "EFF_COM_GimmickDOWN";
		}

		public struct SkillStatus
		{
			public const string attack = "public_attack";
		}

		public struct InvocationEffects
		{
			public const string attack = "none";

			public const string inheritance = "EFF_COM_SKILL";
		}

		public struct PassiveEffects
		{
			public const string attack = "none";

			public const string deathblow = "none";
		}

		public struct SpawnPoints
		{
			public static string GetSpawnPointId(int enemiesNumber, BattleMode battleMode, int cameraType)
			{
				if (battleMode == BattleMode.PvP)
				{
					if (enemiesNumber == 1)
					{
						return "0001_enemiesOne_small";
					}
					if (enemiesNumber == 2)
					{
						return "0002_enemiesTwo_small";
					}
					if (enemiesNumber == 3)
					{
						return "0003_enemiesThree_small_pvp";
					}
				}
				else if (cameraType == 1)
				{
					if (enemiesNumber == 1)
					{
						return "0001_enemiesOne_small_bigboss";
					}
					if (enemiesNumber == 2)
					{
						return "0002_enemiesTwo_small";
					}
					if (enemiesNumber == 3)
					{
						return "0003_enemiesThree_small";
					}
				}
				else
				{
					if (enemiesNumber == 1)
					{
						return "0001_enemiesOne_small";
					}
					if (enemiesNumber == 2)
					{
						return "0002_enemiesTwo_small";
					}
					if (enemiesNumber == 3)
					{
						return "0003_enemiesThree_small";
					}
				}
				global::Debug.LogError("値が不正です. (" + enemiesNumber + ")");
				return string.Empty;
			}

			public struct Default
			{
				public const string enemies1 = "0001_enemiesOne_small";

				public const string enemies2 = "0002_enemiesTwo_small";

				public const string enemies3 = "0003_enemiesThree_small";
			}

			public struct PVP
			{
				public const string enemies1 = "0001_enemiesOne_small";

				public const string enemies2 = "0002_enemiesTwo_small";

				public const string enemies3 = "0003_enemiesThree_small_pvp";
			}

			public struct BigBoss
			{
				public const string enemies1 = "0001_enemiesOne_small_bigboss";

				public const string enemies2 = "0002_enemiesTwo_small";

				public const string enemies3 = "0003_enemiesThree_small";
			}
		}

		public struct BattleInternalResources
		{
			public const string winCameraMotion = "WinCameraMotion";

			public const string commandSelectCameraMotion = "CommandSelectCameraMotion";

			public const string bigBossCommandSelectCameraMotion = "BigBossCommandSelectCameraMotion";
		}

		public struct Ses
		{
			public struct Battle
			{
				public struct HitEffects
				{
					public const string strongDamage = "bt_104";

					public const string weakDamage = "bt_102";

					public const string normalDamage = "bt_101";

					public const string poison = "bt_528";

					public const string confusion = "bt_529";

					public const string paralysis = "bt_530";

					public const string sleep = "bt_531";

					public const string stun = "bt_535";

					public const string skillLock = "bt_533";

					public const string instantDeath = "bt_535";

					public const string attackUp = "";

					public const string attackDown = "";

					public const string defenceUp = "";

					public const string defenceDown = "";

					public const string specialAttackUp = "";

					public const string specialAttackDown = "";

					public const string specialDefenceUp = "";

					public const string specialDefenceDown = "";

					public const string speedUp = "";

					public const string speedDown = "";

					public const string drain = "bt_540";

					public const string correctionUpReset = "";

					public const string correctionDownReset = "";

					public const string hpRevival = "bt_541";

					public const string counter = "bt_544";

					public const string reflection = "bt_544";

					public const string protect = "bt_105";

					public const string hateUp = "";

					public const string hateDown = "";

					public const string powerCharge = "bt_010";

					public const string destruct = "bt_119";

					public const string hitRateUp = "";

					public const string hitRateDown = "";

					public const string sufferStatusClear = "";

					public const string satisfactionRateUp = "";

					public const string satisfactionRateDown = "";

					public const string attackerDestruct = "bt_544";

					public const string death = "bt_119";

					public const string revival = "bt_540";
				}

				public struct InvocationEffects
				{
					public const string attack = "bt_207";

					public const string inheritance = "bt_010";
				}

				public struct PassiveEffects
				{
					public const string attack = "";

					public const string deathblow = "";
				}
			}
		}
	}

	public struct ReservedFilePath
	{
		public const string prefab = "prefab";

		public const string thumb = "thumb";

		public const string status = "status";

		public const string datas = "data";

		public const string sound = "sound";

		public const string split = "/";

		public struct Public
		{
			public const string characterShadowPath = "Shadow";
		}
	}
}
