using System;

namespace Monster
{
	public static class MonsterFixedData
	{
		public static MonsterFixedM GetMonsterFixedMaster(string id)
		{
			MonsterFixedM result = null;
			MonsterFixedM[] monsterFixedValueM = MasterDataMng.Instance().ResponseMonsterFixedMaster.monsterFixedValueM;
			if (monsterFixedValueM != null)
			{
				for (int i = 0; i < monsterFixedValueM.Length; i++)
				{
					if (monsterFixedValueM[i].monsterFixedValueId == id)
					{
						result = monsterFixedValueM[i];
						break;
					}
				}
			}
			return result;
		}
	}
}
