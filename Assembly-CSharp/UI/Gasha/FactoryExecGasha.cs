using Monster;
using System;

namespace UI.Gasha
{
	public static class FactoryExecGasha
	{
		private static bool IsOnlyCutScene(MasterDataMng.AssetCategory costCategory)
		{
			int num = (costCategory != MasterDataMng.AssetCategory.DIGI_STONE) ? 1 : 4;
			int monsterNum = ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum();
			return monsterNum >= num;
		}

		public static ExecGashaBase CreateExecGasha(GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo, bool isTutorial)
		{
			ExecGashaBase execGashaBase = null;
			MasterDataMng.AssetCategory prizeAssetsCategory = gashaInfo.GetPrizeAssetsCategory();
			if (prizeAssetsCategory != MasterDataMng.AssetCategory.MONSTER)
			{
				if (prizeAssetsCategory != MasterDataMng.AssetCategory.CHIP)
				{
					if (prizeAssetsCategory == MasterDataMng.AssetCategory.DUNGEON_TICKET)
					{
						execGashaBase = new ExecGashaTicket();
					}
				}
				else
				{
					execGashaBase = new ExecGashaChip();
				}
			}
			else if (isTutorial && FactoryExecGasha.IsOnlyCutScene(gashaInfo.priceType.GetCostAssetsCategory()))
			{
				execGashaBase = new ExecGashaMonsterCutSceneOnly();
			}
			else
			{
				execGashaBase = new ExecGashaMonster();
			}
			Debug.Assert(null != execGashaBase, "ガシャ実行機能の作成に失敗.");
			execGashaBase.SetGashaInfo(gashaInfo);
			return execGashaBase;
		}
	}
}
