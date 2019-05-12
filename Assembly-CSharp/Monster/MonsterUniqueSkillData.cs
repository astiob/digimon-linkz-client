using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterUniqueSkillData
	{
		private static GameWebAPI.RespDataMA_GetSkillM.SkillM normalAttackSkillData;

		private static Dictionary<string, Dictionary<string, MonsterSkillClientMaster>> skillGroupList;

		private static void AddSkillGroupData(MonsterSkillClientMaster skillMaster, ref Dictionary<string, Dictionary<string, MonsterSkillClientMaster>> destSkillGroupList)
		{
			Dictionary<string, MonsterSkillClientMaster> dictionary = null;
			if (!destSkillGroupList.TryGetValue(skillMaster.Simple.skillGroupId, out dictionary))
			{
				dictionary = new Dictionary<string, MonsterSkillClientMaster>();
				dictionary.Add(skillMaster.Simple.skillGroupSubId, skillMaster);
				destSkillGroupList.Add(skillMaster.Simple.skillGroupId, dictionary);
			}
			else if (!dictionary.ContainsKey(skillMaster.Simple.skillGroupSubId))
			{
				dictionary.Add(skillMaster.Simple.skillGroupSubId, skillMaster);
			}
			else
			{
				Debug.Assert(false, "重複する スキルグループサブID(" + skillMaster.Simple.skillGroupSubId + ")のため失敗しました。");
			}
		}

		private static void CreateSkillGroupList(ref Dictionary<string, Dictionary<string, MonsterSkillClientMaster>> destSkillGroupList)
		{
			GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
			for (int i = 0; i < skillM.Length; i++)
			{
				if (skillM[i] != null)
				{
					MonsterSkillClientMaster skillMasterBySkillId = MonsterSkillData.GetSkillMasterBySkillId(skillM[i].skillId);
					if (skillMasterBySkillId != null)
					{
						MonsterUniqueSkillData.AddSkillGroupData(skillMasterBySkillId, ref destSkillGroupList);
					}
				}
			}
		}

		private static void SetNormalAttackData(ref GameWebAPI.RespDataMA_GetSkillM.SkillM simpleMaster)
		{
			GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
			for (int i = 0; i < skillM.Length; i++)
			{
				if (skillM[i] != null && skillM[i].type == "4")
				{
					simpleMaster = skillM[i];
					break;
				}
			}
		}

		public static void Initialize()
		{
			if (null == MasterDataMng.Instance() || MasterDataMng.Instance().RespDataMA_SkillM == null || MasterDataMng.Instance().RespDataMA_SkillM.skillM == null || MasterDataMng.Instance().RespDataMA_SkillDetailM == null || MasterDataMng.Instance().RespDataMA_SkillDetailM.convertSkillDetailM == null)
			{
				return;
			}
			if (MonsterUniqueSkillData.skillGroupList == null)
			{
				MonsterUniqueSkillData.skillGroupList = new Dictionary<string, Dictionary<string, MonsterSkillClientMaster>>();
			}
			else
			{
				MonsterUniqueSkillData.skillGroupList.Clear();
			}
			MonsterUniqueSkillData.normalAttackSkillData = null;
			MonsterUniqueSkillData.CreateSkillGroupList(ref MonsterUniqueSkillData.skillGroupList);
			MonsterUniqueSkillData.SetNormalAttackData(ref MonsterUniqueSkillData.normalAttackSkillData);
		}

		public static Dictionary<string, MonsterSkillClientMaster> GetMonsterSkillBySkillGroupId(int skillGroupId)
		{
			return MonsterUniqueSkillData.GetMonsterSkillBySkillGroupId(string.Format("{0}", skillGroupId));
		}

		public static Dictionary<string, MonsterSkillClientMaster> GetMonsterSkillBySkillGroupId(string skillGroupId)
		{
			Dictionary<string, MonsterSkillClientMaster> dictionary = null;
			MonsterUniqueSkillData.skillGroupList.TryGetValue(skillGroupId, out dictionary);
			Debug.Assert(null != dictionary, "該当情報がありません。スキルグループID(" + skillGroupId + ")");
			return dictionary;
		}

		public static MonsterSkillClientMaster GetMonsterSkillBySkillGroupId(int skillGroupId, int skillGroupSubId)
		{
			return MonsterUniqueSkillData.GetMonsterSkillBySkillGroupId(string.Format("{0}", skillGroupId), string.Format("{0}", skillGroupSubId));
		}

		public static MonsterSkillClientMaster GetMonsterSkillBySkillGroupId(string skillGroupId, string skillGroupSubId)
		{
			MonsterSkillClientMaster monsterSkillClientMaster = null;
			Dictionary<string, MonsterSkillClientMaster> monsterSkillBySkillGroupId = MonsterUniqueSkillData.GetMonsterSkillBySkillGroupId(skillGroupId);
			if (monsterSkillBySkillGroupId != null)
			{
				monsterSkillBySkillGroupId.TryGetValue(skillGroupSubId, out monsterSkillClientMaster);
				Debug.Assert(null != monsterSkillClientMaster, string.Concat(new string[]
				{
					"該当情報がありません。スキルグループID(",
					skillGroupId,
					"), スキルグループサブID(",
					skillGroupSubId,
					")"
				}));
			}
			return monsterSkillClientMaster;
		}

		public static string GetNormalAttackSkillId()
		{
			string result = string.Empty;
			if (MonsterUniqueSkillData.normalAttackSkillData != null)
			{
				result = MonsterUniqueSkillData.normalAttackSkillData.skillId;
			}
			return result;
		}
	}
}
