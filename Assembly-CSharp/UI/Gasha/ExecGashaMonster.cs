using Cutscene;
using Monster;
using Picturebook;
using System;
using System.Collections;
using System.Collections.Generic;
using User;

namespace UI.Gasha
{
	public class ExecGashaMonster : ExecGashaBase
	{
		private Action showedGashaResultAction;

		private void SetGashaResult(GameWebAPI.RespDataGA_ExecGacha gashaResult, int playCount)
		{
			UserHomeInfo.dirtyMyPage = true;
			ClassSingleton<MonsterUserDataMng>.Instance.AddUserMonsterData(gashaResult.userMonsterList);
			ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonsterList());
			for (int i = 0; i < gashaResult.userMonsterList.Length; i++)
			{
				if (Convert.ToBoolean(gashaResult.userMonsterList[i].isNew))
				{
					MonsterUserData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(gashaResult.userMonsterList[i].userMonsterId);
					GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = userMonster.GetMonsterMaster().Group;
					if (!MonsterPicturebookData.ExistPicturebook(group.monsterCollectionId))
					{
						MonsterPicturebookData.AddPictureBook(group.monsterCollectionId);
					}
				}
			}
			base.UpdateUserAssetsInventory(playCount);
			base.UpdateGashaInfo(playCount);
		}

		protected void SetGashaResultWindow(GameWebAPI.RespDataGA_ExecGacha gashaResult, int[] userMonsterIdList, bool isTutorial)
		{
			CMD_MonsterGashaResult.gashaInfo = this.gashaInfo;
			GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster[] userMonsterList = gashaResult.userMonsterList;
			CMD_MonsterGashaResult.RewardsData = gashaResult.rewards;
			CMD_MonsterGashaResult.DataList = new List<MonsterData>();
			for (int i = 0; i < userMonsterList.Length; i++)
			{
				CMD_MonsterGashaResult.DataList.Add(ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterIdList[i]));
			}
			CMD_MonsterGashaResult.IconNewFlagList = new List<bool>();
			for (int j = 0; j < userMonsterList.Length; j++)
			{
				CMD_MonsterGashaResult.IconNewFlagList.Add(Convert.ToBoolean(userMonsterList[j].isNew));
			}
			CMD_MonsterGashaResult.isTutorial = isTutorial;
		}

		protected void SetGashaCutScene(GameWebAPI.RespDataGA_ExecGacha gashaResult, bool isTutorial)
		{
			GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster[] userMonsterList = gashaResult.userMonsterList;
			string[] array = new string[userMonsterList.Length];
			string[] array2 = new string[userMonsterList.Length];
			for (int i = 0; i < userMonsterList.Length; i++)
			{
				MonsterUserData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterList[i].userMonsterId);
				GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = userMonster.GetMonsterMaster().Group;
				array[i] = group.modelId;
				array2[i] = group.growStep;
			}
			CutsceneDataGasha cutsceneData = new CutsceneDataGasha
			{
				path = "Cutscenes/Gasha",
				modelIdList = array,
				growStepList = array2,
				endCallback = new Action(CutSceneMain.FadeReqCutSceneEnd)
			};
			Loading.Invisible();
			if (isTutorial)
			{
				this.showedGashaResultAction = new Action(Singleton<TutorialObserver>.Instance.ResumeScript);
			}
			CutSceneMain.FadeReqCutScene(cutsceneData, new Action(CMD_MonsterGashaResult.CreateDialog), null, new Action<int>(this.OnShowedGashaResultDialog), 0.5f, 0.5f);
		}

		protected override void OnShowedGashaResultDialog(int noop)
		{
			base.OnShowedGashaResultDialog(noop);
			if (this.showedGashaResultAction != null)
			{
				this.showedGashaResultAction();
				this.showedGashaResultAction = null;
			}
			CMD_MonsterGashaResult.instance.FadeInEndAction();
		}

		public override IEnumerator Exec(GameWebAPI.GA_Req_ExecGacha playGashaRequestParam, bool isTutorial)
		{
			GameWebAPI.RequestGA_GashaExec playGashaRequest = new GameWebAPI.RequestGA_GashaExec
			{
				SetSendData = delegate(GameWebAPI.GA_Req_ExecGacha param)
				{
					param.gachaId = playGashaRequestParam.gachaId;
					param.playCount = playGashaRequestParam.playCount;
					param.itemCount = playGashaRequestParam.itemCount;
				},
				OnReceived = delegate(GameWebAPI.RespDataGA_ExecGacha response)
				{
					GameWebAPI.RespDataGA_ExecGacha gashaResult = response;
					int[] userMonsterIdList = new int[gashaResult.userMonsterList.Length];
					for (int i = 0; i < gashaResult.userMonsterList.Length; i++)
					{
						int num = 0;
						if (int.TryParse(gashaResult.userMonsterList[i].userMonsterId, out num))
						{
							userMonsterIdList[i] = num;
						}
					}
					this.SetGashaResult(gashaResult, playGashaRequestParam.playCount);
				}
			};
			GameWebAPI.MonsterSlotInfoListLogic monsterSlotRequest = new GameWebAPI.MonsterSlotInfoListLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataCS_MonsterSlotInfoListLogic param)
				{
					param.userMonsterId = userMonsterIdList;
				},
				OnReceived = delegate(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic response)
				{
					ChipDataMng.GetUserChipSlotData().UpdateMonsterSlotList(response.slotInfo);
					ClassSingleton<MonsterUserDataMng>.Instance.RefreshMonsterSlot(userMonsterIdList);
				}
			};
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			APIRequestTask task = new APIRequestTask(playGashaRequest, true);
			task.Add(new APIRequestTask(monsterSlotRequest, true));
			yield return AppCoroutine.Start(task.Run(delegate
			{
				this.SetGashaResultWindow(gashaResult, userMonsterIdList, isTutorial);
				this.SetGashaCutScene(gashaResult, isTutorial);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
				GUIManager.CloseAllCommonDialog(null);
			}, null), false);
			yield break;
		}
	}
}
