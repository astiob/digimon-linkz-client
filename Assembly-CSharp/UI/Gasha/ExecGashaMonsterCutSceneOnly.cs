using Monster;
using System;
using System.Collections;

namespace UI.Gasha
{
	public sealed class ExecGashaMonsterCutSceneOnly : ExecGashaMonster
	{
		private GameWebAPI.RespDataGA_ExecGacha GetFakeGashaResult()
		{
			GameWebAPI.RespDataGA_ExecGacha respDataGA_ExecGacha = new GameWebAPI.RespDataGA_ExecGacha();
			MonsterData monsterData;
			if (this.gashaInfo.priceType.GetCostAssetsCategory() == MasterDataMng.AssetCategory.LINK_POINT)
			{
				monsterData = ClassSingleton<MonsterUserDataMng>.Instance.GetOldestMonster();
			}
			else
			{
				monsterData = ClassSingleton<MonsterUserDataMng>.Instance.GetNewestMonster();
			}
			respDataGA_ExecGacha.userMonsterList = new GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster[]
			{
				new GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster(monsterData.userMonster)
				{
					isNew = 1
				}
			};
			return respDataGA_ExecGacha;
		}

		public override IEnumerator Exec(GameWebAPI.GA_Req_ExecGacha playGashaRequestParam, bool isTutorial)
		{
			GameWebAPI.RespDataGA_ExecGacha gashaResult = this.GetFakeGashaResult();
			int[] userMonsterIdList = new int[gashaResult.userMonsterList.Length];
			for (int i = 0; i < gashaResult.userMonsterList.Length; i++)
			{
				int userMonsterId = 0;
				if (int.TryParse(gashaResult.userMonsterList[i].userMonsterId, out userMonsterId))
				{
					userMonsterIdList[i] = userMonsterId;
				}
			}
			base.UpdateUserAssetsInventory(playGashaRequestParam.playCount);
			base.SetGashaResultWindow(gashaResult, userMonsterIdList, isTutorial);
			base.SetGashaCutScene(gashaResult, isTutorial);
			yield return null;
			yield break;
		}
	}
}
