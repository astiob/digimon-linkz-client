using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterSkillData
	{
		private static Dictionary<string, MonsterSkillClientMaster> simpleMasterList;

		private static Dictionary<string, Dictionary<string, MonsterSkillClientMaster>> groupMasterList;

		private static void GetSkillDetailMaster(string skillId, ref List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> destDetailMasterList)
		{
			GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] convertSkillDetailM = MasterDataMng.Instance().RespDataMA_SkillDetailM.convertSkillDetailM;
			for (int i = 0; i < convertSkillDetailM.Length; i++)
			{
				if (skillId == convertSkillDetailM[i].skillId)
				{
					destDetailMasterList.Add(convertSkillDetailM[i]);
				}
			}
		}

		private static void AddSkillSimpleMaster(MonsterSkillClientMaster skillMaster, ref Dictionary<string, MonsterSkillClientMaster> destSimpleMasterList)
		{
			if (!destSimpleMasterList.ContainsKey(skillMaster.Simple.skillId))
			{
				destSimpleMasterList.Add(skillMaster.Simple.skillId, skillMaster);
			}
			else
			{
				Debug.Assert(false, "重複する skillId(" + skillMaster.Simple.skillId + ")のため失敗しました。");
			}
		}

		private static void AddSkillGroupMaster(MonsterSkillClientMaster skillMaster, ref Dictionary<string, Dictionary<string, MonsterSkillClientMaster>> destGroupMasterList)
		{
			Dictionary<string, MonsterSkillClientMaster> dictionary = null;
			if (!destGroupMasterList.TryGetValue(skillMaster.Simple.skillGroupId, out dictionary))
			{
				dictionary = new Dictionary<string, MonsterSkillClientMaster>();
				dictionary.Add(skillMaster.Simple.skillGroupSubId, skillMaster);
				destGroupMasterList.Add(skillMaster.Simple.skillGroupId, dictionary);
			}
			else if (!dictionary.ContainsKey(skillMaster.Simple.skillGroupSubId))
			{
				dictionary.Add(skillMaster.Simple.skillGroupSubId, skillMaster);
			}
			else
			{
				Debug.Assert(false, string.Concat(new string[]
				{
					"重複する skillGroupId(",
					skillMaster.Simple.skillGroupId,
					"), skillGroupSubId(",
					skillMaster.Simple.skillGroupSubId,
					") のため失敗しました。"
				}));
			}
		}

		private static void CreateSkillMasterClient(ref Dictionary<string, MonsterSkillClientMaster> destSimpleMasterList, ref Dictionary<string, Dictionary<string, MonsterSkillClientMaster>> destGroupMasterList)
		{
			GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
			for (int i = 0; i < skillM.Length; i++)
			{
				if (skillM[i] != null)
				{
					List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> list = new List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM>();
					MonsterSkillData.GetSkillDetailMaster(skillM[i].skillId, ref list);
					if (0 < list.Count)
					{
						MonsterSkillClientMaster monsterSkillClientMaster = new MonsterSkillClientMaster(skillM[i], list);
						MonsterSkillData.AddSkillSimpleMaster(monsterSkillClientMaster, ref destSimpleMasterList);
						if ("0" != monsterSkillClientMaster.Simple.skillGroupSubId)
						{
							MonsterSkillData.AddSkillGroupMaster(monsterSkillClientMaster, ref destGroupMasterList);
						}
					}
				}
			}
		}

		public static void Initialize()
		{
			if (null == MasterDataMng.Instance() || MasterDataMng.Instance().RespDataMA_SkillM == null || MasterDataMng.Instance().RespDataMA_SkillM.skillM == null || MasterDataMng.Instance().RespDataMA_SkillDetailM == null || MasterDataMng.Instance().RespDataMA_SkillDetailM.convertSkillDetailM == null)
			{
				return;
			}
			if (MonsterSkillData.groupMasterList == null)
			{
				MonsterSkillData.groupMasterList = new Dictionary<string, Dictionary<string, MonsterSkillClientMaster>>();
			}
			else
			{
				MonsterSkillData.groupMasterList.Clear();
			}
			if (MonsterSkillData.simpleMasterList == null)
			{
				MonsterSkillData.simpleMasterList = new Dictionary<string, MonsterSkillClientMaster>();
			}
			else
			{
				MonsterSkillData.simpleMasterList.Clear();
			}
			MonsterSkillData.CreateSkillMasterClient(ref MonsterSkillData.simpleMasterList, ref MonsterSkillData.groupMasterList);
		}

		public static MonsterSkillClientMaster GetSkillMasterBySkillId(int skillId)
		{
			return MonsterSkillData.GetSkillMasterBySkillId(string.Format("{0}", skillId));
		}

		public static MonsterSkillClientMaster GetSkillMasterBySkillId(string skillId)
		{
			MonsterSkillClientMaster monsterSkillClientMaster = null;
			MonsterSkillData.simpleMasterList.TryGetValue(skillId, out monsterSkillClientMaster);
			Debug.Assert(null != monsterSkillClientMaster, "該当情報がありません。skillId(" + skillId + ")");
			return monsterSkillClientMaster;
		}

		public static MonsterSkillClientMaster GetSkillMasterBySkillGroupId(string skillGroupId, string skillGroupSubId)
		{
			MonsterSkillClientMaster monsterSkillClientMaster = null;
			Dictionary<string, MonsterSkillClientMaster> dictionary = null;
			if (MonsterSkillData.groupMasterList.TryGetValue(skillGroupId, out dictionary))
			{
				dictionary.TryGetValue(skillGroupSubId, out monsterSkillClientMaster);
			}
			Debug.Assert(null != monsterSkillClientMaster, string.Concat(new string[]
			{
				"該当情報がありません。skillGroupId(",
				skillGroupId,
				"), skillGroupSubId(",
				skillGroupSubId,
				")"
			}));
			return monsterSkillClientMaster;
		}

		public static Dictionary<string, MonsterSkillClientMaster> GetSkillMasterBySkillGroupId(string skillGroupId)
		{
			Dictionary<string, MonsterSkillClientMaster> dictionary = null;
			MonsterSkillData.groupMasterList.TryGetValue(skillGroupId, out dictionary);
			Debug.Assert(null != dictionary, "該当情報がありません。skillGroupId(" + skillGroupId + ")");
			return dictionary;
		}
	}
}
