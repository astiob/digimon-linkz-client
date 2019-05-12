using Cutscene;
using System;
using System.Collections;
using UnityEngine;
using User;

namespace UI.Gasha
{
	public sealed class ExecGashaChip : ExecGashaBase
	{
		private GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] SetGashaResult(GameWebAPI.RespDataGA_ExecChip gashaResult, int playCount)
		{
			UserHomeInfo.dirtyMyPage = true;
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] array = new GameWebAPI.RespDataCS_ChipListLogic.UserChipList[gashaResult.userAssetList.Length];
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				int num2 = 0;
				if (int.TryParse(gashaResult.userAssetList[i].assetValue, out num) && int.TryParse(gashaResult.userAssetList[i].userAssetId, out num2))
				{
					array[i] = new GameWebAPI.RespDataCS_ChipListLogic.UserChipList
					{
						chipId = num,
						userChipId = num2,
						userMonsterId = 0
					};
				}
				global::Debug.Assert(null != array[i], string.Concat(new object[]
				{
					"ガシャ排出された強化チップのIDが不正です:chipId=",
					num,
					", userChipId=",
					num2
				}));
			}
			ChipDataMng.AddUserChipList(array);
			base.UpdateUserAssetsInventory(playCount);
			base.UpdateGashaInfo(playCount);
			return array;
		}

		private void SetGashaResultWindow(GameWebAPI.RespDataGA_ExecChip gashaResult, GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList)
		{
			CMD_ChipGashaResult.gashaInfo = this.gashaInfo;
			CMD_ChipGashaResult.UserAssetList = gashaResult.userAssetList;
			CMD_ChipGashaResult.SetUserChipList(userChipList);
			CMD_ChipGashaResult.RewardsData = gashaResult.rewards;
		}

		private void SetGashaCutScene(GameWebAPI.RespDataGA_ExecChip gashaResult, int playCount)
		{
			string bgmFileName = (playCount != 1) ? "bgm_205" : "bgm_204";
			UIPanel uipanel = GUIMain.GetUIPanel();
			CutsceneDataChipGasha cutsceneDataChipGasha = new CutsceneDataChipGasha
			{
				path = "Cutscenes/AssetBundle/ChipGasha/chip_gacha",
				gashaResult = gashaResult.userAssetList,
				bgmFileName = bgmFileName,
				backgroundSize = uipanel.GetWindowSize()
			};
			cutsceneDataChipGasha.endCallback = delegate(RenderTexture renderTexture)
			{
				UITexture txBG = CMD_ChipGashaResult.instance.txBG;
				txBG.mainTexture = renderTexture;
				txBG.width = renderTexture.width;
				txBG.height = renderTexture.height;
				CutSceneMain.FadeReqCutSceneEnd();
				SoundMng.Instance().PlayGameBGM("bgm_202");
			};
			Loading.Invisible();
			CutSceneMain.FadeReqCutScene(cutsceneDataChipGasha, new Action(CMD_ChipGashaResult.CreateDialog), null, new Action<int>(this.OnShowedGashaResultDialog), 0.5f, 0.5f);
		}

		protected override void OnShowedGashaResultDialog(int noop)
		{
			base.OnShowedGashaResultDialog(noop);
			CMD_ChipGashaResult.instance.StartEffect = true;
		}

		public override IEnumerator Exec(GameWebAPI.GA_Req_ExecGacha playGashaRequestParam, bool isTutorial)
		{
			GameWebAPI.RequestGA_ChipExec request = new GameWebAPI.RequestGA_ChipExec
			{
				SetSendData = delegate(GameWebAPI.GA_Req_ExecChip param)
				{
					param.gachaId = playGashaRequestParam.gachaId;
					param.playCount = playGashaRequestParam.playCount;
				},
				OnReceived = delegate(GameWebAPI.RespDataGA_ExecChip response)
				{
					GameWebAPI.RespDataGA_ExecChip gashaResult = response;
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList = this.SetGashaResult(gashaResult, playGashaRequestParam.playCount);
				}
			};
			yield return AppCoroutine.Start(request.Run(delegate()
			{
				this.SetGashaResultWindow(gashaResult, userChipList);
				this.SetGashaCutScene(gashaResult, playGashaRequestParam.playCount);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
				GUIManager.CloseAllCommonDialog(null);
			}, null), false);
			yield break;
		}
	}
}
