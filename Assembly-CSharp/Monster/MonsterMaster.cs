using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterMaster
	{
		private static Dictionary<string, MonsterClientMaster> simpleMasterList;

		private static Dictionary<string, Dictionary<string, MonsterClientMaster>> groupMasterList;

		private static GameWebAPI.RespDataMA_GetMonsterMG.MonsterM GetMonsterGroupMaster(string monsterGroupId)
		{
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM result = null;
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM[] monsterM = MasterDataMng.Instance().RespDataMA_MonsterMG.monsterM;
			for (int i = 0; i < monsterM.Length; i++)
			{
				if (monsterM[i] != null && monsterM[i].monsterGroupId == monsterGroupId)
				{
					result = monsterM[i];
					break;
				}
			}
			return result;
		}

		private static void AddMonsterSimpleMaster(MonsterClientMaster monsterMaster, ref Dictionary<string, MonsterClientMaster> destSimpleMasterList)
		{
			if (!destSimpleMasterList.ContainsKey(monsterMaster.Simple.monsterId))
			{
				destSimpleMasterList.Add(monsterMaster.Simple.monsterId, monsterMaster);
			}
			else
			{
				Debug.Assert(false, "重複する モンスターID(" + monsterMaster.Simple.monsterId + ")のため失敗しました。");
			}
		}

		private static void AddMonsterGroupMaster(MonsterClientMaster monsterMaster, ref Dictionary<string, Dictionary<string, MonsterClientMaster>> destGroupMasterList)
		{
			Dictionary<string, MonsterClientMaster> dictionary = null;
			if (!destGroupMasterList.TryGetValue(monsterMaster.Simple.monsterGroupId, out dictionary))
			{
				dictionary = new Dictionary<string, MonsterClientMaster>();
				dictionary.Add(monsterMaster.Simple.rare, monsterMaster);
				destGroupMasterList.Add(monsterMaster.Simple.monsterGroupId, dictionary);
			}
			else if (!dictionary.ContainsKey(monsterMaster.Simple.rare))
			{
				dictionary.Add(monsterMaster.Simple.rare, monsterMaster);
			}
			else
			{
				Debug.Assert(false, string.Concat(new string[]
				{
					"重複する モンスターID(",
					monsterMaster.Simple.monsterId,
					"), 覚醒値(",
					monsterMaster.Simple.rare,
					")のため失敗しました。"
				}));
			}
		}

		private static void CreateMonsterMasterClient(ref Dictionary<string, MonsterClientMaster> destSimpleMasterList, ref Dictionary<string, Dictionary<string, MonsterClientMaster>> destGroupMasterList)
		{
			GameWebAPI.RespDataMA_GetMonsterMS.MonsterM[] monsterM = MasterDataMng.Instance().RespDataMA_MonsterMS.monsterM;
			for (int i = 0; i < monsterM.Length; i++)
			{
				if (monsterM[i] != null)
				{
					GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMaster = MonsterMaster.GetMonsterGroupMaster(monsterM[i].monsterGroupId);
					if (monsterGroupMaster != null)
					{
						MonsterClientMaster monsterMaster = new MonsterClientMaster(monsterM[i], monsterGroupMaster);
						MonsterMaster.AddMonsterSimpleMaster(monsterMaster, ref destSimpleMasterList);
						MonsterMaster.AddMonsterGroupMaster(monsterMaster, ref destGroupMasterList);
					}
				}
			}
		}

		public static void Initialize()
		{
			if (null == MasterDataMng.Instance() || MasterDataMng.Instance().RespDataMA_MonsterMS == null || MasterDataMng.Instance().RespDataMA_MonsterMS.monsterM == null || MasterDataMng.Instance().RespDataMA_MonsterMG == null || MasterDataMng.Instance().RespDataMA_MonsterMG.monsterM == null)
			{
				return;
			}
			if (MonsterMaster.simpleMasterList == null)
			{
				MonsterMaster.simpleMasterList = new Dictionary<string, MonsterClientMaster>();
			}
			else
			{
				MonsterMaster.simpleMasterList.Clear();
			}
			if (MonsterMaster.groupMasterList == null)
			{
				MonsterMaster.groupMasterList = new Dictionary<string, Dictionary<string, MonsterClientMaster>>();
			}
			else
			{
				MonsterMaster.groupMasterList.Clear();
			}
			MonsterMaster.CreateMonsterMasterClient(ref MonsterMaster.simpleMasterList, ref MonsterMaster.groupMasterList);
		}

		public static MonsterClientMaster GetMonsterMasterByMonsterId(int monsterId)
		{
			return MonsterMaster.GetMonsterMasterByMonsterId(string.Format("{0}", monsterId));
		}

		public static MonsterClientMaster GetMonsterMasterByMonsterId(string monsterId)
		{
			MonsterClientMaster monsterClientMaster = null;
			MonsterMaster.simpleMasterList.TryGetValue(monsterId, out monsterClientMaster);
			Debug.Assert(null != monsterClientMaster, "該当情報がありません。モンスターID(" + monsterId + ")");
			return monsterClientMaster;
		}

		public static Dictionary<string, MonsterClientMaster> GetMonsterMasterListByMonsterGroupId(int monsterGroupId)
		{
			return MonsterMaster.GetMonsterMasterListByMonsterGroupId(string.Format("{0}", monsterGroupId));
		}

		public static Dictionary<string, MonsterClientMaster> GetMonsterMasterListByMonsterGroupId(string monsterGroupId)
		{
			Dictionary<string, MonsterClientMaster> dictionary = null;
			MonsterMaster.groupMasterList.TryGetValue(monsterGroupId, out dictionary);
			Debug.Assert(null != dictionary, "該当情報がありません。モンスターグループID(" + monsterGroupId + ")");
			return dictionary;
		}

		public static MonsterClientMaster GetMonsterMasterByMonsterGroupId(int monsterGroupId)
		{
			return MonsterMaster.GetMonsterMasterByMonsterGroupId(string.Format("{0}", monsterGroupId), "1");
		}

		public static MonsterClientMaster GetMonsterMasterByMonsterGroupId(string monsterGroupId)
		{
			return MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterGroupId, "1");
		}

		public static MonsterClientMaster GetMonsterMasterByMonsterGroupId(int monsterGroupId, int arousal)
		{
			return MonsterMaster.GetMonsterMasterByMonsterGroupId(string.Format("{0}", monsterGroupId), string.Format("{0}", arousal));
		}

		public static MonsterClientMaster GetMonsterMasterByMonsterGroupId(string monsterGroupId, string arousal)
		{
			MonsterClientMaster monsterClientMaster = null;
			Dictionary<string, MonsterClientMaster> monsterMasterListByMonsterGroupId = MonsterMaster.GetMonsterMasterListByMonsterGroupId(monsterGroupId);
			if (monsterMasterListByMonsterGroupId != null)
			{
				monsterMasterListByMonsterGroupId.TryGetValue(arousal, out monsterClientMaster);
				Debug.Assert(null != monsterClientMaster, string.Concat(new string[]
				{
					"該当情報がありません。モンスターグループID(",
					monsterGroupId,
					"), 覚醒値(",
					arousal,
					")"
				}));
			}
			return monsterClientMaster;
		}

		public static Dictionary<string, Dictionary<string, MonsterClientMaster>> GetGroupMasterList()
		{
			return MonsterMaster.groupMasterList;
		}
	}
}
