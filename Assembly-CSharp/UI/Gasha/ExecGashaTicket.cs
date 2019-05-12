using Cutscene;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using User;

namespace UI.Gasha
{
	public sealed class ExecGashaTicket : ExecGashaBase
	{
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

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
			CutsceneDataBase cutsceneData = cutsceneDataTicketGasha;
			if (ExecGashaTicket.<>f__mg$cache0 == null)
			{
				ExecGashaTicket.<>f__mg$cache0 = new Action(CMD_TicketGashaResult.CreateDialog);
			}
			CutSceneMain.FadeReqCutScene(cutsceneData, ExecGashaTicket.<>f__mg$cache0, null, new Action<int>(this.OnShowedGashaResultDialog), 0.5f, 0.5f);
		}

		protected override void OnShowedGashaResultDialog(int noop)
		{
			base.OnShowedGashaResultDialog(noop);
			CMD_TicketGashaResult.instance.StartEffect = true;
		}

		public override IEnumerator Exec(GameWebAPI.GA_Req_ExecGacha playGashaRequestParam, bool isTutorial)
		{
			GameWebAPI.RespDataGA_ExecTicket gashaResult = null;
			GameWebAPI.RequestGA_TicketExec request = new GameWebAPI.RequestGA_TicketExec
			{
				SetSendData = delegate(GameWebAPI.GA_Req_ExecTicket param)
				{
					param.gachaId = playGashaRequestParam.gachaId;
					param.playCount = playGashaRequestParam.playCount;
				},
				OnReceived = delegate(GameWebAPI.RespDataGA_ExecTicket response)
				{
					gashaResult = response;
					UserHomeInfo.dirtyMyPage = true;
					this.UpdateUserAssetsInventory(playGashaRequestParam.playCount);
					this.UpdateGashaInfo(playGashaRequestParam.playCount);
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
