using System;

public static class MultiBattleConstValue
{
	public const string PVP_MOCK_BATTLE_WORLD_DUNGEON_ID = "10001";

	public const string TCP_KEY_BATTLE_RESULT = "820102";

	public const string TCP_KEY_MULTI_BATTLE = "multiBattle";

	public const string TCP_KEY_PVP_BATTLE = "pvpBattle";

	public const string TCP_KEY_PVP_MATCHING = "080106";

	public const string TCP_KEY_FAILED_PLAYER = "800012";

	public const string TCP_KEY_MULTI_BATTLE_RESUME = "820106";

	public const string TCP_KEY_PVP_BATTLE_START = "080108";

	public const string TCP_KEY_PVP_BATTLE_END = "080109";

	public const string TCP_KEY_PVP_ONLINE_CHECK = "080110";

	public const string TCP_KEY_PVP_RECOVER_COMMUNICATE = "080112";

	public const string TCP_KEY_PVP_BATTLE_ACTION_LOG = "080114";

	public enum PVP_BATTLE_START_RESULT_CODE
	{
		NON_PARTNER,
		PROCESS_END,
		PARTNER_PROCESS_END
	}

	public enum PVP_ONLINE_CHECK_RESULT_CODE
	{
		OFFLINE,
		ONLINE
	}

	public enum PVP_RECOVER_COMMUNICATE_RESULT_CODE
	{
		NORMAL = 1,
		ENEMY_WON,
		ENEMY_LOST,
		NO_USER,
		ENEMY_RECOVER,
		TIMEOUT
	}

	public enum PVP_BATTLE_ACTION_LOG_CODE
	{
		SUCCESS = 1,
		FAILED = 0
	}
}
