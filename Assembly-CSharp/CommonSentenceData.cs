using Master;
using System;
using System.Collections.Generic;

public static class CommonSentenceData
{
	private static Dictionary<string, string> gradeTable = new Dictionary<string, string>();

	private static Dictionary<string, string> tribeTable = new Dictionary<string, string>();

	private static Dictionary<string, string> friendshipTable = new Dictionary<string, string>();

	public static void ClearCache()
	{
		CommonSentenceData.gradeTable.Clear();
		CommonSentenceData.tribeTable.Clear();
		CommonSentenceData.friendshipTable.Clear();
	}

	public static string GetGrade(string key)
	{
		string text = StringMaster.GetString("CharaStatus-03");
		if (!CommonSentenceData.gradeTable.TryGetValue(key, out text))
		{
			GameWebAPI.RespDataMA_GetMonsterGrowStepM respDataMA_MonsterGrowStepM = MasterDataMng.Instance().RespDataMA_MonsterGrowStepM;
			if (respDataMA_MonsterGrowStepM != null && respDataMA_MonsterGrowStepM.monsterGrowStepM != null)
			{
				for (int i = 0; i < respDataMA_MonsterGrowStepM.monsterGrowStepM.Length; i++)
				{
					if (respDataMA_MonsterGrowStepM.monsterGrowStepM[i].monsterGrowStepId == key)
					{
						text = respDataMA_MonsterGrowStepM.monsterGrowStepM[i].monsterGrowStepName;
						CommonSentenceData.gradeTable.Add(key, text);
						break;
					}
				}
			}
		}
		return text;
	}

	public static string GetTribe(string key)
	{
		string text = StringMaster.GetString("CharaStatus-03");
		if (!CommonSentenceData.tribeTable.TryGetValue(key, out text))
		{
			GameWebAPI.RespDataMA_GetMonsterTribeM respDataMA_MonsterTribeM = MasterDataMng.Instance().RespDataMA_MonsterTribeM;
			if (respDataMA_MonsterTribeM != null && respDataMA_MonsterTribeM.monsterTribeM != null)
			{
				for (int i = 0; i < respDataMA_MonsterTribeM.monsterTribeM.Length; i++)
				{
					if (respDataMA_MonsterTribeM.monsterTribeM[i].monsterTribeId == key)
					{
						text = respDataMA_MonsterTribeM.monsterTribeM[i].monsterTribeName;
						CommonSentenceData.tribeTable.Add(key, text);
						break;
					}
				}
			}
		}
		return text;
	}

	private static string GetGrowStepFriendshipMaxNum(string key)
	{
		string text = "0";
		if (!CommonSentenceData.friendshipTable.TryGetValue(key, out text))
		{
			GameWebAPI.RespDataMA_GetMonsterGrowStepM respDataMA_MonsterGrowStepM = MasterDataMng.Instance().RespDataMA_MonsterGrowStepM;
			if (respDataMA_MonsterGrowStepM != null && respDataMA_MonsterGrowStepM.monsterGrowStepM != null)
			{
				for (int i = 0; i < respDataMA_MonsterGrowStepM.monsterGrowStepM.Length; i++)
				{
					if (respDataMA_MonsterGrowStepM.monsterGrowStepM[i].monsterGrowStepId == key)
					{
						text = respDataMA_MonsterGrowStepM.monsterGrowStepM[i].maxFriendship;
						CommonSentenceData.friendshipTable.Add(key, text);
						break;
					}
				}
			}
		}
		return text;
	}

	public static string MaxFriendshipFormat(string nowFriendship, string key)
	{
		string growStepFriendshipMaxNum = CommonSentenceData.GetGrowStepFriendshipMaxNum(key);
		return string.Format(StringMaster.GetString("SystemFraction"), nowFriendship, growStepFriendshipMaxNum);
	}

	public static int MaxFriendshipValue(string key)
	{
		string growStepFriendshipMaxNum = CommonSentenceData.GetGrowStepFriendshipMaxNum(key);
		return growStepFriendshipMaxNum.ToInt32();
	}
}
