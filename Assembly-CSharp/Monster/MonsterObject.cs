using System;

namespace Monster
{
	public static class MonsterObject
	{
		public static string GetFilePath(string monsterGroupId)
		{
			return "Characters/" + monsterGroupId + "/prefab";
		}
	}
}
