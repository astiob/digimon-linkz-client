using System;
using System.Linq;
using UnityEngine;

public sealed class PartyBossIcons : MonoBehaviour
{
	private const int BOSS_TYPE = 2;

	[SerializeField]
	private PartyBossRegistance[] partyBossRegistances;

	public void SetBossIconAndTolerances()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy[] bosses = this.GetBosses();
		if (bosses == null && bosses.Length == 0)
		{
			foreach (PartyBossRegistance partyBossRegistance in this.partyBossRegistances)
			{
				partyBossRegistance.gameObject.SetActive(false);
			}
			return;
		}
		for (int j = 0; j < this.partyBossRegistances.Length; j++)
		{
			if (bosses.Length > j)
			{
				GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy encountEnemy = bosses[j];
				string monsterId = encountEnemy.monsterId.ToString();
				MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(monsterId);
				this.partyBossRegistances[j].SetIcon(monsterData);
				GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM tolerances = monsterData.SerchResistanceById(encountEnemy.resistanceId.ToString());
				this.partyBossRegistances[j].SetTolerances(tolerances);
			}
			else
			{
				this.partyBossRegistances[j].gameObject.SetActive(false);
			}
		}
	}

	private GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy[] GetBosses()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy[] stageEnemies = ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies;
		return stageEnemies.Where((GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy enemy) => enemy.type == 2).ToArray<GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy>();
	}
}
