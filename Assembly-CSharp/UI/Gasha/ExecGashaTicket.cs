using Cutscene;
using System;
using System.Collections;
using UnityEngine;
using User;

namespace UI.Gasha
{
	public sealed class ExecGashaTicket : ExecGashaBase
	{
		private void SetGashaResultWindow(GameWebAPI.RespDataGA_ExecTicket gashaResult)
		{
			CMD_TicketGashaResult.gashaInfo = this.gashaInfo;
			CMD_TicketGashaResult.UserDungeonTicketList = gashaResult.userDungeonTicketList;
			CMD_TicketGashaResult.RewardsData = gashaResult.rewards;
		}

		private void SetGashaCutScene(GameWebAPI.RespDataGA_ExecTicket gashaResult, int playCount)
		{
			string bgmFileName = (playCount != 1) ? "bgm_205" : "bgm_204";
			UIPanel uipanel = GUIMain.GetUIPanel();
			CutsceneDataTicketGasha cutsceneDataTicketGasha = new CutsceneDataTicketGasha
			{
				path = "Cutscenes/ticketGacha",
				gashaResult = gashaResult.userDungeonTicketList,
				bgmFileName = bgmFileName,
				backgroundSize = uipanel.GetWindowSize()
			};
			cutsceneDataTicketGasha.endCallback = delegate(RenderTexture renderTexture)
			{
				UITexture txBG = CMD_TicketGashaResult.instance.txBG;
				txBG.mainTexture = renderTexture;
				txBG.width = renderTexture.width;
				txBG.height = renderTexture.height;
				CutSceneMain.FadeReqCutSceneEnd();
				SoundMng.Instance().PlayGameBGM("bgm_202");
			};
			Loading.Invisible();
			CutSceneMain.FadeReqCutScene(cutsceneDataTicketGasha, new Action(CMD_TicketGashaResult.CreateDialog), null, new Action<int>(this.OnShowedGashaResultDialog), 0.5f, 0.5f);
		}

		protected override void OnShowedGashaResultDialog(int noop)
		{
			base.OnShowedGashaResultDialog(noop);
			CMD_TicketGashaResult.instance.StartEffect = true;
		}

		public override IEnumerator Exec(GameWebAPI.GA_Req_ExecGacha playGashaRequestParam, bool isTutorial)
		{
			GameWebAPI.RequestGA_TicketExec request = new GameWebAPI.RequestGA_TicketExec
			{
				SetSendData = delegate(GameWebAPI.GA_Req_ExecTicket param)
				{
					param.gachaId = playGashaRequestParam.gachaId;
					param.playCount = playGashaRequestParam.playCount;
				},
				OnReceived = delegate(GameWebAPI.RespDataGA_ExecTicket response)
				{
					GameWebAPI.RespDataGA_ExecTicket gashaResult = response;
					UserHomeInfo.dirtyMyPage = true;
					base.UpdateUserAssetsInventory(playGashaRequestParam.playCount);
					base.UpdateGashaInfo(playGashaRequestParam.playCount);
				}
			};
			yield return AppCoroutine.Start(request.RunOneTime(delegate()
			{
				this.SetGashaResultWindow(gashaResult);
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
