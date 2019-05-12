using System;

public class QuestTOPAccessor : ClassSingleton<QuestTOPAccessor>
{
	public bool nextAreaFlg;

	public bool nextAreaEvent;

	public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM nextDungeon;

	public GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM nextStage;

	public CMD_QuestTOP questTOP { get; set; }
}
