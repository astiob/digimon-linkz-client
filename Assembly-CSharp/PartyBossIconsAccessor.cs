using System;

public sealed class PartyBossIconsAccessor : ClassSingleton<PartyBossIconsAccessor>
{
	public GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy[] StageEnemies { get; set; }
}
