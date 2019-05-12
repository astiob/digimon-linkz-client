using Master;
using System;

namespace Monster
{
	public static class MonsterSpecificTypeData
	{
		public static string GetSpecificTypeName(string specificType)
		{
			GameWebAPI.RespDataMA_MonsterSpecificTypeMaster.SpecificType[] monsterStatusM = MasterDataMng.Instance().ResponseMonsterSpecificTypeMaster.monsterStatusM;
			string result = StringMaster.GetString("CharaStatus-01");
			for (int i = 0; i < monsterStatusM.Length; i++)
			{
				if (specificType == monsterStatusM[i].monsterStatusId)
				{
					result = monsterStatusM[i].name;
					break;
				}
			}
			return result;
		}
	}
}
