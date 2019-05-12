using System;
using System.Collections.Generic;
using System.Linq;

public class StageGimmick
{
	private Dictionary<string, Dictionary<string, List<string>>> dataDic = new Dictionary<string, Dictionary<string, List<string>>>();

	public Dictionary<string, Dictionary<string, List<string>>> DataDic
	{
		get
		{
			if (this.dataDic.Count == 0)
			{
				this.CreateDataDic();
			}
			return this.dataDic;
		}
	}

	private void CreateDataDic()
	{
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeon in MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM)
		{
			IEnumerable<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM> enumerable = MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectManageM.worldDungeonExtraEffectManageM.Where((GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM xx) => xx.worldDungeonId == worldDungeon.worldDungeonId);
			foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM worldDungeonExtraEffectManageM in enumerable)
			{
				if (!this.dataDic.ContainsKey(worldDungeon.worldStageId))
				{
					this.dataDic.Add(worldDungeon.worldStageId, new Dictionary<string, List<string>>());
				}
				if (!this.dataDic[worldDungeon.worldStageId].ContainsKey(worldDungeon.worldDungeonId))
				{
					this.dataDic[worldDungeon.worldStageId].Add(worldDungeon.worldDungeonId, new List<string>());
				}
				if (!this.dataDic[worldDungeon.worldStageId][worldDungeon.worldDungeonId].Contains(worldDungeonExtraEffectManageM.worldDungeonExtraEffectId))
				{
					this.dataDic[worldDungeon.worldStageId][worldDungeon.worldDungeonId].Add(worldDungeonExtraEffectManageM.worldDungeonExtraEffectId);
				}
			}
		}
	}

	public bool ContainsStage(string StageID)
	{
		return this.DataDic.ContainsKey(StageID);
	}

	public bool ContainsDungeon(string StageID, string DungeonID)
	{
		return this.ContainsStage(StageID) && this.DataDic[StageID].ContainsKey(DungeonID);
	}

	private List<string> GetExtraEffectIDList(string StageID, string DungeonID)
	{
		if (this.ContainsDungeon(StageID, DungeonID))
		{
			return this.DataDic[StageID][DungeonID];
		}
		return new List<string>();
	}

	private List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> GetExtraEffectDataList(List<string> ExtraEffectIDList)
	{
		IEnumerable<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> source = MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectM.worldDungeonExtraEffectM.SelectMany((GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM xx) => ExtraEffectIDList, (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM xx, string yy) => new
		{
			xx,
			yy
		}).Where(<>__TranspIdent1 => <>__TranspIdent1.xx.worldDungeonExtraEffectId == <>__TranspIdent1.yy).Select(<>__TranspIdent1 => <>__TranspIdent1.xx);
		return source.ToList<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
	}

	public List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> GetExtraEffectDataList(string StageID, string DungeonID)
	{
		return this.GetExtraEffectDataList(this.GetExtraEffectIDList(StageID, DungeonID));
	}

	public bool IsMatch(List<string> ExtraEffectIDList, MonsterData MonsterData)
	{
		return ExtraEffectUtil.IsExtraEffectMonster(MonsterData, this.GetExtraEffectDataList(ExtraEffectIDList).ToArray());
	}

	public bool IsMatch(string StageID, string DungeonID, MonsterData MonsterData)
	{
		return this.IsMatch(this.GetExtraEffectIDList(StageID, DungeonID), MonsterData);
	}

	public void ZeroClear()
	{
		this.dataDic.Clear();
	}
}
