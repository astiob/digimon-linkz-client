using AdventureScene;
using Master;
using Scene;
using System;
using UI;
using UnityEngine;

namespace Quest
{
	public static class QuestStart
	{
		public static void OpenConfirmReplayEvent(Action<int> onClosedAction)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(onClosedAction, "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("QuestEventReplayConfirm");
		}

		public static void StartEventStage(GameWebAPI.WD_Req_DngStart startInfo)
		{
			QuestEventInfoList.EventInfo eventInfo = ClassSingleton<QuestData>.Instance.GetEventInfo(startInfo.dungeonId);
			ClassSingleton<AdventureSceneController>.Instance.Ready(eventInfo.scriptFileName, new Action(QuestStart.BeginEventScene), new Action(QuestStart.EndEventScene));
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			GameWebAPI.RequestWD_WorldStart requestWD_WorldStart = new GameWebAPI.RequestWD_WorldStart();
			requestWD_WorldStart.SetSendData = delegate(GameWebAPI.WD_Req_DngStart param)
			{
				param.dungeonId = startInfo.dungeonId;
				param.deckNum = startInfo.deckNum;
				param.userDungeonTicketId = startInfo.userDungeonTicketId;
			};
			requestWD_WorldStart.OnReceived = delegate(GameWebAPI.RespDataWD_DungeonStart response)
			{
				ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart = response;
			};
			GameWebAPI.RequestWD_WorldStart request = requestWD_WorldStart;
			AppCoroutine.Start(request.RunOneTime(delegate()
			{
				if (null != DataMng.Instance() && DataMng.Instance().WD_ReqDngResult != null)
				{
					DataMng.Instance().WD_ReqDngResult.startId = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.startId;
					DataMng.Instance().WD_ReqDngResult.dungeonId = startInfo.dungeonId;
					DataMng.Instance().WD_ReqDngResult.clear = 0;
				}
				DataMng.Instance().GetResultUtilData().last_dng_req = startInfo;
				FadeController.Instance().StartBlackFade(0f, 1f, 0.5f, new Action(QuestStart.OnFinishedFadeoutEventStage), null);
				SoundMng.Instance().StopBGM(0.5f, null);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
				CMD_QuestTOP.instance.ClosePanel(true);
			}, null), false);
		}

		private static void OnFinishedFadeoutEventStage()
		{
			Loading.DisableMask();
			TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToSoloBattle, true);
			if (null != CMD_QuestTOP.instance)
			{
				CMD_QuestTOP.instance.isGoingBattle = true;
				CMD_QuestTOP.instance.ClosePanel(true);
			}
			else
			{
				CMD_BattleNextChoice cmd_BattleNextChoice = UnityEngine.Object.FindObjectOfType<CMD_BattleNextChoice>();
				if (null != cmd_BattleNextChoice)
				{
					cmd_BattleNextChoice.ClosePanel(true);
				}
			}
			GUIMain.ReqScreen("UIIdle", string.Empty);
			SceneController.DeleteCurrentScene();
			ClassSingleton<AdventureSceneController>.Instance.Start();
		}

		private static void BeginEventScene()
		{
			TipsLoading.Instance.StopTipsLoad(true);
			RestrictionInput.EndLoad();
			FadeController.Instance().DeleteFade();
		}

		private static void EndEventScene()
		{
			DataMng.Instance().WD_ReqDngResult.clear = 1;
			DataMng.Instance().WD_ReqDngResult.aliveInfo = new int[]
			{
				1,
				1,
				1
			};
			SceneController.ChangeBattleResultScene(0f);
		}
	}
}
