using System;
using System.Linq;

public class SoundPlayer
{
	private const string seInternalCommonPath = "SEInternal/Common/";

	private const string seInternalBattlePath = "SEInternal/Battle/";

	private static readonly string[] seCommonPathList = new string[]
	{
		"se_101",
		"se_102",
		"se_103",
		"se_104",
		"se_105",
		"se_106",
		"se_107"
	};

	private static readonly string[] seBattlePathList = new string[]
	{
		"warning/sound",
		"bnm002_2mix_rs/sound",
		"bnm003_2mix_rs/sound",
		"se_307/sound",
		"se_308/sound",
		"se_306/sound",
		"se_309/sound",
		"se_310/sound"
	};

	private static void LoadCommonSEAudio()
	{
		SoundMng.Instance().PreLoadAudio(SoundPlayer.seCommonPathList.Select((string soundName) => "SEInternal/Common/" + soundName).ToArray<string>());
	}

	private static void LoadBattleSEAudio()
	{
		SoundMng.Instance().PreLoadAudio(SoundPlayer.seBattlePathList.Select((string soundName) => "SEInternal/Battle/" + soundName).ToArray<string>());
	}

	public static void PlayMenuOpen()
	{
		SoundPlayer.PlayCommonSE(2);
	}

	public static void StopMenuOpen()
	{
		SoundPlayer.StopCommonSE(2);
	}

	public static void PlayMenuClose()
	{
		SoundPlayer.PlayCommonSE(3);
	}

	public static void StopMenuClose()
	{
		SoundPlayer.StopCommonSE(3);
	}

	public static void PlayButtonEnter()
	{
		SoundPlayer.PlayCommonSE(4);
	}

	public static void StopButtonEnter()
	{
		SoundPlayer.StopCommonSE(4);
	}

	public static void PlayButtonCancel()
	{
		SoundPlayer.PlayCommonSE(5);
	}

	public static void StopButtonCancel()
	{
		SoundPlayer.StopCommonSE(5);
	}

	public static void PlayButtonSelect()
	{
		SoundPlayer.PlayCommonSE(6);
	}

	public static void StopButtonSelect()
	{
		SoundPlayer.StopCommonSE(6);
	}

	private static void PlayCommonSE(int num)
	{
		SoundPlayer.LoadCommonSEAudio();
		SoundMng.Instance().PlaySE("SEInternal/Common/" + SoundPlayer.seCommonPathList[num], 0f, false, true, null, -1, 1f);
	}

	private static void StopCommonSE(int num)
	{
		if (SoundMng.Instance().IsPlayingSE("SEInternal/Common/" + SoundPlayer.seCommonPathList[num]))
		{
			SoundMng.Instance().StopSE("SEInternal/Common/" + SoundPlayer.seCommonPathList[num], 0f, null);
		}
	}

	public static void PlayBattleWarning()
	{
		SoundPlayer.PlayBattleSE(0);
	}

	public static void StopBattleWarning()
	{
		SoundPlayer.StopBattleSE(0);
	}

	public static void PlayBattleWinBGM()
	{
		SoundPlayer.PlayBattleSE(1);
	}

	public static void StopBattleWinBGM()
	{
		SoundPlayer.StopBattleSE(1);
	}

	public static void PlayBattleFailBGM()
	{
		SoundPlayer.PlayBattleSE(2);
	}

	public static void StopBattleFailBGM()
	{
		SoundPlayer.StopBattleSE(2);
	}

	public static void PlayBattleCountDownSE()
	{
		SoundPlayer.PlayBattleSE(3);
	}

	public static void StopBattleCountDownSE()
	{
		SoundPlayer.StopBattleSE(3);
	}

	public static void PlayBattleMyTurnSE()
	{
		SoundPlayer.PlayBattleSE(4);
	}

	public static void StopBattleMyTurnSE()
	{
		SoundPlayer.StopBattleSE(4);
	}

	public static void PlayBattleRecoverAPSE()
	{
		SoundPlayer.PlayBattleSE(5);
	}

	public static void StopBattleRecoverAPSE()
	{
		SoundPlayer.StopBattleSE(5);
	}

	public static void PlayBattlePopupOtherEmotionSE()
	{
		SoundPlayer.PlayBattleSE(6);
	}

	public static void StopBattlePopupOtherEmotionSE()
	{
		SoundPlayer.StopBattleSE(6);
	}

	public static void PlayBattleVSSE()
	{
		SoundPlayer.PlayBattleSE(7);
	}

	public static void StopBattleVSSE()
	{
		SoundPlayer.StopBattleSE(7);
	}

	private static void PlayBattleSE(int num)
	{
		SoundPlayer.LoadBattleSEAudio();
		SoundMng.Instance().PlaySE("SEInternal/Battle/" + SoundPlayer.seBattlePathList[num], 0f, false, true, null, -1, 1f);
	}

	private static void StopBattleSE(int num)
	{
		if (SoundMng.Instance().IsPlayingSE("SEInternal/Battle/" + SoundPlayer.seBattlePathList[num]))
		{
			SoundMng.Instance().StopSE("SEInternal/Battle/" + SoundPlayer.seBattlePathList[num], 0f, null);
		}
	}
}
