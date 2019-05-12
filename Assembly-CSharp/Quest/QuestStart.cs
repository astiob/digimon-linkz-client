using AdventureScene;
using Master;
using Scene;
using System;
using System.Runtime.CompilerServices;
using UI;
using UnityEngine;

namespace Quest
{
	public static class QuestStart
	{
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Action <>f__mg$cache1;

		[CompilerGenerated]
		private static Action <>f__mg$cache2;

		public static void OpenConfirmReplayEvent(Action<int> onClosedAction)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(onClosedAction, "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("QuestEventReplayConfirm");
		}

		public static void StartEventStage(GameWebAPI.WD_Req_DngStart startInfo)
		{
			QuestEventInfoList.EventInfo eventInfo = ClassSingleton<QuestData>.Instance.GetEventInfo(startInfo.dungeonId);
			AdventureSceneController instance = ClassSingleton<AdventureSceneController>.Instance;
			string scriptFileName = eventInfo.scriptFileName;
			if (QuestStart.<>f__mg$cache0 == null)
			{
				QuestStart.<>f__mg$cache0 = new Action(QuestStart.BeginEventScene);
			}
			Action beginAction = QuestStart.<>f__mg$cache0;
			if (QuestStart.<>f__mg$cache1 == null)
			{
				QuestStart.<>f__mg$cache1 = new Action(QuestStart.EndEventScene);
			}
			instance.Ready(scriptFileName, beginAction, QuestStart.<>f__mg$cache1);
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
				FadeController fadeController = FadeController.Instance();
				float startAlpha = 0f;
				float endAlpha = 1f;
				float time = 0.5f;
				if (QuestStart.<>f__mg$cache2 == null)
				{
					QuestStart.<>f__mg$cache2 = new Action(QuestStart.OnFinishedFadeoutEventStage);
				}
				fadeController.StartBlackFade(startAlpha, endAlpha, time, QuestStart.<>f__mg$cache2, null);
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
