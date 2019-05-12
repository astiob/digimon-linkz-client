using Monster;
using System;
using System.Collections.Generic;

internal sealed class GashaTutorialMode
{
	private const int RARE_GASHA_ID = 1;

	private const int LINK_GASHA_ID = 2;

	private bool fakeExec;

	private static bool tutoExec;

	public bool FakeExec
	{
		get
		{
			return this.fakeExec;
		}
	}

	public static bool TutoExec
	{
		get
		{
			return GashaTutorialMode.tutoExec;
		}
		set
		{
			GashaTutorialMode.tutoExec = value;
		}
	}

	public void UpdateFakeExec(bool isRare)
	{
		int num = (!isRare) ? 1 : 4;
		List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
		if (num <= monsterDataList.Count)
		{
			this.fakeExec = true;
		}
	}

	public GameWebAPI.RespDataGA_ExecGacha GetFakeGashaResult()
	{
		GameWebAPI.RespDataGA_ExecGacha respDataGA_ExecGacha = new GameWebAPI.RespDataGA_ExecGacha();
		MonsterData oldestMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetOldestMonster();
		respDataGA_ExecGacha.userMonsterList = new GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster[]
		{
			new GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster(oldestMonster.userMonster)
			{
				isNew = 1
			}
		};
		return respDataGA_ExecGacha;
	}
}
