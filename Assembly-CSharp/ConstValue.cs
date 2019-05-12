using Secure;
using System;
using UnityEngine;

public class ConstValue
{
	public const string TAB_ARROW = "Common02_Btn_Arrow";

	public const string RESISTANCE_WEAK_VALUE = "-1";

	public const string RESISTANCE_NONE_VALUE = "0";

	public const string RESISTANCE_STRONG_VALUE = "1";

	public const string RESISTANCE_DRAIN_VALUE = "2";

	public const string RESISTANCE_INVALID_VALUE = "99";

	public const int AROUSAL_MIN_VALUE = 2;

	public const int AROUSAL_MAX_VALUE = 5;

	public const int AROUSAL_VERSIONUP_MIN_VALUE = 6;

	public const int AROUSAL_VERSIONUP_MAX_VALUE = 6;

	public const string ACTIVE_BLUE_BUTTON = "Common02_Btn_Blue";

	public const string ACTIVE_RED_BUTTON = "Common02_Btn_Red";

	public const string ACTIVE_GREEN_BUTTON = "Common02_Btn_Green";

	public const string DEACTIVE_BUTTON = "Common02_Btn_Gray";

	public const string ON_BUTTON = "Common02_Btn_BaseON";

	public const string OFF_BUTTON = "Common02_Btn_BaseG";

	public const string SELECT_RED_BUTTON = "Common02_Btn_SupportRed";

	public const string SELECT_WHITE_BUTTON = "Common02_Btn_SupportWhite";

	public const string MISSION_NOT_COMPLETE_BUTTON = "Common02_Btn_BaseG";

	public const string MISSION_CAN_RECEIVE_BUTTON = "Common02_Btn_BaseON1";

	public const string MISSION_LINK_BUTTON = "Common02_Btn_BaseON";

	public const string STATUS_CHG_ACTIVE_BUTTON = "Common02_Btn_BaseON";

	public const string STATUS_CHG_GRAY_BUTTON = "Common02_Btn_BaseG";

	public const string PROFILE_GREEN_BUTTON = "Common02_Btn_BaseON1";

	public const string PROFILE_RED_BUTTON = "Common02_Btn_BaseON2";

	public const string PROFILE_GRAY_BUTTON = "Common02_Btn_BaseG";

	public const string GOLD_TALENT_ICON = "Common02_Talent_Gold";

	public const string SILVER_TALENT_ICON = "Common02_Talent_Silver";

	public const string COMMON_OTHER_SCENE_BGM = "bgm_102";

	public const string FORM_MAIN_BGM = "bgm_201";

	public const string FORM_GASHA_BGM = "bgm_202";

	public const string COLOSSEUM_BGM = "bgm_203";

	public const string BATTLE_QUEST_TOP_BGM = "bgm_301";

	public const string BATTLE_RESULT_BGM = "bgm_303";

	public const string BATTLE_ALL_DIE_BGM = "bgm_304";

	public const string BGM_PATH_FORMAT = "BGM/{0}/sound";

	public const float LIST_TAP_ENABLE_MAGNITUDE_MAX = 40f;

	public const float RENDER3D_CHARA_SCALE_MIN = 0.8f;

	public const float RENDER3D_CHARA_SCALE_MAX = 1.2f;

	private static int[] skill_ditailM_efcs = new int[]
	{
		1,
		34
	};

	public static int NO_MEAN_NUM = -1;

	public static Color TAB_YELLOW = new Color(0.980392158f, 0.945098042f, 0f, 1f);

	public static Color DIGIMON_YELLOW = new Color32(byte.MaxValue, 200, 0, byte.MaxValue);

	public static Color DIGIMON_BLUE = new Color32(0, 200, byte.MaxValue, byte.MaxValue);

	public static Color DIGIMON_GREEN = new Color32(0, byte.MaxValue, 50, byte.MaxValue);

	public static Color PLUS_COLOR = new Color32(0, 150, 0, byte.MaxValue);

	public static Color MINUS_COLOR = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	public static Color DEFAULT_COLOR = new Color32(58, 58, 58, byte.MaxValue);

	public static Color DEACTIVE_BUTTON_LABEL = new Color32(150, 150, 150, byte.MaxValue);

	public static string APP_SITE_DOMAIN = Secure.ConstValue.MULTI_LANGUAGE_APP_AND_SITE_DOMAIN;

	public static string APP_WEB_DOMAIN = Secure.ConstValue.MULTI_LANGUAGE_APP_AND_SITE_DOMAIN;

	public static string APP_ASSET_DOMAIN = Secure.ConstValue.MULTI_LANGUAGE_APP_AND_ASSET_DOMAIN;

	public static string WEB_INFO_ADR = "/information/InformationDetail/?informationId=";

	public static int WEB_INFO_ADR_GASHA_OFFSET = 10000;

	public static int MAX_CHILD_MONSTER
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MAX_CHILD_MONSTER;
		}
	}

	public static int USE_STONE_NUM_TO_RECOVER
	{
		get
		{
			global::Debug.Log("USE_STONE_NUM_TO_RECOVER は廃止されます");
			return 5;
		}
	}

	public static int RECOVER_STAMINA_DIGISTONE_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.RECOVER_STAMINA_DIGISTONE_NUM;
		}
	}

	public static int MAX_GOLD_MEDAL_COUNT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MAX_GOLD_MEDAL_COUNT;
		}
	}

	public static int MAX_CLUSTER_COUNT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MAX_CLUSTER_COUNT;
		}
	}

	public static int MAX_DIGISTONE_COUNT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MAX_DIGISTONE_COUNT;
		}
	}

	public static int MAX_LINK_POINT_COUNT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MAX_LINK_POINT_COUNT;
		}
	}

	public static int RARE_STAR
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.RARE_STAR;
		}
	}

	public static int MAX_RARE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MAX_RARE;
		}
	}

	public static int MAX_BLOCK_COUNT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MAX_BLOCK_COUNT;
		}
	}

	public static int ENABLE_MONSTER_SPACE_TOEXEC_GASHA_1
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ENABLE_MONSTER_SPACE_TOEXEC_GASHA_1;
		}
	}

	public static int ENABLE_MONSTER_SPACE_TOEXEC_GASHA_10
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ENABLE_MONSTER_SPACE_TOEXEC_GASHA_10;
		}
	}

	public static int ENABLE_MONSTER_SPACE_TOEXEC_DUNGEON
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ENABLE_MONSTER_SPACE_TOEXEC_DUNGEON;
		}
	}

	public static int ENABLE_CHIP_SPACE_TOEXEC_GASHA_1
	{
		get
		{
			return 1;
		}
	}

	public static int ENABLE_CHIP_SPACE_TOEXEC_GASHA_10
	{
		get
		{
			return 1;
		}
	}

	public static int ENABLE_CHIP_SPACE_TOEXEC_DUNGEON
	{
		get
		{
			return 1;
		}
	}

	public static int RARE_GASHA_TYPE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.RARE_GASHA_TYPE;
		}
	}

	public static int LINK_GASHA_TYPE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.LINK_GASHA_TYPE;
		}
	}

	public static int EVOLVE_ITEM_MONS
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EVOLVE_ITEM_MONS;
		}
	}

	public static int EVOLVE_ITEM_SOUL
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EVOLVE_ITEM_SOUL;
		}
	}

	public static int EXP_OF_ONE_MEAT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EXP_OF_ONE_MEAT;
		}
	}

	public static int MEAT_SHORTCUT_DEGISTONE_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MEAT_SHORTCUT_DEGISTONE_NUM;
		}
	}

	public static int SUCCESSION_COEFFICIENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.SUCCESSION_COEFFICIENT;
		}
	}

	public static int SUCCESSION_BASE_COEFFICIENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.SUCCESSION_BASE_COEFFICIENT;
		}
	}

	public static int SUCCESSION_PARTNER_COEFFICIENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.SUCCESSION_PARTNER_COEFFICIENT;
		}
	}

	public static int LABORATORY_BASE_PLUS_COEFFICIENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.LABORATORY_BASE_PLUS_COEFFICIENT;
		}
	}

	public static int LABORATORY_PARTNER_PLUS_COEFFICIENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.LABORATORY_PARTNER_PLUS_COEFFICIENT;
		}
	}

	public static int LABORATORY_BASE_COEFFICIENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.LABORATORY_BASE_COEFFICIENT;
		}
	}

	public static int LABORATORY_PARTNER_COEFFICIENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.LABORATORY_PARTNER_COEFFICIENT;
		}
	}

	public static float REINFORCEMENT_COEFFICIENT
	{
		get
		{
			return (float)MasterDataMng.Instance().RespDataMA_CodeM.codeM.REINFORCEMENT_COEFFICIENT;
		}
	}

	public static int EVOLVE_COEFFICIENT_FOR_5
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EVOLVE_COEFFICIENT_FOR_5;
		}
	}

	public static int EVOLVE_COEFFICIENT_FOR_6
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EVOLVE_COEFFICIENT_FOR_6;
		}
	}

	public static int EVOLVE_COEFFICIENT_FOR_7
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EVOLVE_COEFFICIENT_FOR_7;
		}
	}

	public static int[] EVOLVE_COEFFICIENT_RARE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EVOLVE_COEFFICIENT_RARE;
		}
	}

	public static int MODE_CHANGE_COEFFICIENT_FOR_5
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MODE_CHANGE_COEFFICIENT_FOR_5;
		}
	}

	public static int MODE_CHANGE_COEFFICIENT_FOR_6
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MODE_CHANGE_COEFFICIENT_FOR_6;
		}
	}

	public static int MODE_CHANGE_COEFFICIENT_FOR_7
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MODE_CHANGE_COEFFICIENT_FOR_7;
		}
	}

	public static int[] MODE_CHANGE_COEFFICIENT_RARE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MODE_CHANGE_COEFFICIENT_RARE;
		}
	}

	public static string QUEST_AREA_ID_DEFAULT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.QUEST_AREA_ID_DEFAULT;
		}
	}

	public static string QUEST_AREA_ID_EVENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.QUEST_AREA_ID_EVENT;
		}
	}

	public static string QUEST_AREA_ID_ADVENT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.QUEST_AREA_ID_ADVENT;
		}
	}

	public static int FIRST_CLEAR_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.FIRST_CLEAR_NUM;
		}
	}

	public static int TCP_PONG_END_COUNT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.TCP_PONG_END_COUNT;
		}
	}

	public static int TCP_PONG_INTERVAL_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.TCP_PONG_INTERVAL_TIME;
		}
	}

	public static string BUILDING_TYPE_FACILITY
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.BUILDING_TYPE_FACILITY;
		}
	}

	public static string BUILDING_TYPE_DECORATION
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.BUILDING_TYPE_DECORATION;
		}
	}

	public static int GROW_STEP_HIGH
	{
		get
		{
			return 6;
		}
	}

	public static int BATTLEFAIL_TIPS_SHOW_DNG_NUM
	{
		get
		{
			return 7;
		}
	}

	public static string CONTACT_SITE_URL
	{
		get
		{
			if (MasterDataMng.Instance().RespDataMA_CodeM != null)
			{
				return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ANDROID_CONTACT_SITE_URL;
			}
			return string.Empty;
		}
	}

	public static string OFFICIAL_SITE_URL
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.OFFICIAL_SITE_URL;
		}
	}

	public static int RIZE_CONDITION_FRENDSHIPSTATUS
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.RIZE_CONDITION_FRENDSHIPSTATUS;
		}
	}

	public static string GASHA_PRICE_TYPE_DIGISTONE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_PRICE_TYPE_DIGISTONE;
		}
	}

	public static string GASHA_PRICE_TYPE_LINKPOINT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_PRICE_TYPE_LINKPOINT;
		}
	}

	public static string GASHA_PRICE_TYPE_DIGISTONE_CHIP
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_PRICE_TYPE_DIGISTONE_CHIP;
		}
	}

	public static string GASHA_PRICE_TYPE_LINKPOINT_CHIP
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_PRICE_TYPE_LINKPOINT_CHIP;
		}
	}

	public static string GASHA_PRICE_TYPE_DIGISTONE_TICKET
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_PRICE_TYPE_DIGISTONE_TICKET;
		}
	}

	public static string GASHA_PRICE_TYPE_LINKPOINT_TICKET
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_PRICE_TYPE_LINKPOINT_TICKET;
		}
	}

	public static int GASHA_INHARITANCE_PRIZE_LEVEL
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_INHARITANCE_PRIZE_LEVEL;
		}
	}

	public static int GASHA_LEADERSKILL_PRIZE_LEVEL
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GASHA_LEADERSKILL_PRIZE_LEVEL;
		}
	}

	public static string TUTORIAL_GASHA_MAIN_BANNER_PATH
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.TUTORIAL_GASHA_MAIN_BANNER_PATH;
		}
	}

	public static string TUTORIAL_GASHA_SUB_BANNER1_PATH
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.TUTORIAL_GASHA_SUB_BANNER1_PATH;
		}
	}

	public static string TUTORIAL_GASHA_SUB_BANNER2_PATH
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.TUTORIAL_GASHA_SUB_BANNER2_PATH;
		}
	}

	public static string TUTORIAL_GASHA_SUB_BANNER3_PATH
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.TUTORIAL_GASHA_SUB_BANNER3_PATH;
		}
	}

	public static int IS_CHAT_OPEN
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.IS_CHAT_OPEN;
		}
	}

	public static int PVP_MAX_RANK
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_MAX_RANK;
		}
	}

	public static int PVP_MAX_ATTACK_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_MAX_ATTACK_TIME;
		}
	}

	public static int PVP_HURRYUP_ATTACK_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_HURRYUP_ATTACK_TIME;
		}
	}

	public static int PVP_MAX_ROUND_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_MAX_ROUND_NUM;
		}
	}

	public static int PVP_BATTLE_TIMEOUT_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_BATTLE_TIMEOUT_TIME;
		}
	}

	public static int PVP_BATTLE_ENEMY_RECOVER_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_BATTLE_ENEMY_RECOVER_TIME;
		}
	}

	public static int MULTI_MAX_ATTACK_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MULTI_MAX_ATTACK_TIME;
		}
	}

	public static int MULTI_HURRYUP_ATTACK_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MULTI_HURRYUP_ATTACK_TIME;
		}
	}

	public static int MULTI_BATTLE_TIMEOUT_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.MULTI_BATTLE_TIMEOUT_TIME;
		}
	}

	public static int[] CHIP_EXTEND_SLOT_NEEDS
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.CHIP_EXTEND_SLOT_NEEDS;
		}
	}

	public static int EVENT_QUEST_OPEN_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EVENT_QUEST_OPEN_TIME;
		}
	}

	public static bool REVIEW_STOP_FLAG
	{
		get
		{
			return "1" == MasterDataMng.Instance().RespDataMA_CodeM.codeM.REVIEW_STOP_FLAG;
		}
	}

	public static int BASE_CHIP_SELL_PRICE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.BASE_CHIP_SELL_PRICE;
		}
	}

	public static int BUY_HQMEAT_DIGISTONE_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.BUY_HQMEAT_DIGISTONE_NUM;
		}
	}

	public static float ABILITY_UPGRADE_MULRATE_GROWING
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_UPGRADE_MULRATE_GROWING;
		}
	}

	public static float ABILITY_UPGRADE_MULRATE_RIPE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_UPGRADE_MULRATE_RIPE;
		}
	}

	public static float ABILITY_UPGRADE_MULRATE_PERFECT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_UPGRADE_MULRATE_PERFECT;
		}
	}

	public static float ABILITY_UPGRADE_MULRATE_ULTIMATE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_UPGRADE_MULRATE_ULTIMATE;
		}
	}

	public static int ABILITY_INHERITRATE_GROWING
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_INHERITRATE_GROWING;
		}
	}

	public static int ABILITY_INHERITRATE_RIPE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_INHERITRATE_RIPE;
		}
	}

	public static int ABILITY_INHERITRATE_PERFECT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_INHERITRATE_PERFECT;
		}
	}

	public static int ABILITY_INHERITRATE_ULTIMATE
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.ABILITY_INHERITRATE_ULTIMATE;
		}
	}

	public static string EXT_ADR_AGREE
	{
		get
		{
			if (MasterDataMng.Instance().RespDataMA_CodeM == null || string.IsNullOrEmpty(MasterDataMng.Instance().RespDataMA_CodeM.codeM.EXT_ADR_AGREE))
			{
				return "https://legal.bandainamcoent.co.jp/terms/";
			}
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EXT_ADR_AGREE;
		}
	}

	public static string EXT_ADR_PRIVACY_POLICY
	{
		get
		{
			if (MasterDataMng.Instance().RespDataMA_CodeM == null || string.IsNullOrEmpty(MasterDataMng.Instance().RespDataMA_CodeM.codeM.EXT_ADR_PRIVACY_POLICY))
			{
				return "https://legal.bandainamcoent.co.jp/privacy/";
			}
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.EXT_ADR_PRIVACY_POLICY;
		}
	}

	public static string GDPR_OPT_OUT_SITE_URL
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.GDPR_OPT_OUT_SITE_URL;
		}
	}

	public static int PLAYLIMIT_USE_COUNT
	{
		get
		{
			return 1;
		}
	}

	public static string STORE_SITE_URL
	{
		get
		{
			return "https://play.google.com/store/apps/details?id=com.bandainamcoent.digimon_linkz_ww";
		}
	}

	public static int CACHE_TEXTURE_COUNT
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.CACHE_TEXTURE_COUNT;
		}
	}

	public static int[] SkillDetailM_effectType
	{
		get
		{
			return global::ConstValue.skill_ditailM_efcs;
		}
	}

	public static Rect GetRectWindow()
	{
		Rect result = default(Rect);
		float num = 280f;
		result.xMin = -num;
		result.xMax = num;
		result.yMin = -240f - GUIMain.VerticalSpaceSize;
		result.yMax = 160f + GUIMain.VerticalSpaceSize;
		return result;
	}

	public static Rect GetRectWindow2()
	{
		Rect result = default(Rect);
		float num = 240f;
		result.xMin = -num;
		result.xMax = num;
		result.yMin = -297f - GUIMain.VerticalSpaceSize;
		result.yMax = 158f + GUIMain.VerticalSpaceSize;
		return result;
	}

	public static Rect GetRectWindow3()
	{
		return new Rect
		{
			xMin = -280f,
			xMax = 280f,
			yMin = -312f - GUIMain.VerticalSpaceSize,
			yMax = 244f + GUIMain.VerticalSpaceSize
		};
	}

	public enum Medal
	{
		None,
		Gold,
		Silver
	}

	public enum CandidateMedal
	{
		NONE,
		GOLD,
		SILVER,
		SILVER_OR_NONE = 100,
		GOLD_OR_NONE,
		GOLD_OR_SILVER
	}

	public enum ResistanceType
	{
		NONE,
		NOTHINGNESS,
		FIRE,
		WATER,
		THUNDER,
		NATURE,
		LIGHT,
		DARK,
		STUN,
		SKILL_LOCK,
		SLEEP,
		PARALYSIS,
		CONFUSION,
		POISON,
		DEATH
	}
}
