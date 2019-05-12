using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
	public sealed class ScriptEngine
	{
		private List<ScriptCommandData> allCommandList;

		private int commandAddress;

		private int stepAddress;

		private ScriptInitializer initializer;

		private ScriptCommander commander;

		private bool isWaitCallback;

		private bool isWaitCommandSkip;

		private ScriptEngine.Status engineStatus;

		private Func<string[], bool> actionExternalCommand;

		public ScriptEngine()
		{
			this.initializer = new ScriptInitializer();
			this.commander = new ScriptCommander();
		}

		public void Deserialize(string script)
		{
			List<string> allLineList = new List<string>(script.Split(new char[]
			{
				'\n'
			}));
			this.allCommandList = this.initializer.ConvertAllStrDtsToCommands(allLineList);
			this.stepAddress = 0;
			if (this.allCommandList.Count == 0)
			{
				this.engineStatus = ScriptEngine.Status.EOF;
			}
		}

		public bool IsFinished()
		{
			return !this.isWaitCallback && ScriptEngine.Status.EOF == this.engineStatus;
		}

		private void ActionCommandScript(Action<ScriptEngine.Status, int> actionScriptCommand)
		{
			while (this.engineStatus == ScriptEngine.Status.NONE)
			{
				ScriptCommandData scriptCommandData = this.allCommandList[this.stepAddress];
				string[] commandParams = ScriptUtil.SplitByWhiteSpace(scriptCommandData.strArrange);
				this.AnalyzeCommand(commandParams);
				this.commandAddress = scriptCommandData.lineNum;
				this.stepAddress++;
			}
			if (actionScriptCommand != null)
			{
				actionScriptCommand(this.engineStatus, this.commandAddress);
			}
			if (this.allCommandList.Count <= this.stepAddress)
			{
				this.engineStatus = ScriptEngine.Status.EOF;
			}
			else
			{
				this.engineStatus = ScriptEngine.Status.NONE;
			}
		}

		public void StepCommandScript(Action<ScriptEngine.Status, int> actionScriptCommand)
		{
			if (!this.isWaitCallback)
			{
				this.ActionCommandScript(actionScriptCommand);
			}
		}

		public IEnumerator RestartScript(Action<ScriptEngine.Status, int> actionScriptCommand)
		{
			this.isWaitCallback = true;
			float startTime = Time.realtimeSinceStartup;
			this.isWaitCommandSkip = true;
			while (this.isWaitCommandSkip)
			{
				this.ActionCommandScript(actionScriptCommand);
				if (Time.realtimeSinceStartup - startTime > 0.02f)
				{
					yield return null;
					startTime = Time.realtimeSinceStartup;
				}
			}
			this.isWaitCallback = false;
			yield break;
		}

		public void StartSkip(Action<ScriptEngine.Status, int> actionScriptCommand)
		{
			this.isWaitCallback = true;
			this.isWaitCommandSkip = true;
			while (this.isWaitCommandSkip)
			{
				this.ActionCommandScript(actionScriptCommand);
			}
			this.isWaitCallback = true;
		}

		public void StopSkip()
		{
			this.isWaitCommandSkip = false;
		}

		private void AnalyzeCommand(string[] commandParams)
		{
			string text = commandParams[0];
			switch (text)
			{
			case "#window":
				this.commander.Window(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_WINDOW);
				return;
			case "#window_del":
				this.SetStatus(true, ScriptEngine.Status.WAIT_WINDOW_DELETE);
				return;
			case "#msg":
				this.commander.Msg(commandParams);
				return;
			case "#text":
				this.commander.Text(commandParams);
				return;
			case "#msgend":
				this.commander.MsgEnd(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_MESSAGE_DISPLAY);
				return;
			case "#select":
				this.commander.Select(commandParams);
				return;
			case "#select_display":
				this.commander.SelectDisplay();
				this.SetStatus(true, ScriptEngine.Status.WAIT_SELECT_DISPLAY);
				return;
			case "#chara":
				this.commander.Chara(commandParams);
				this.SetStatus(false, ScriptEngine.Status.WAIT_CHARA);
				return;
			case "#chara_pos":
				this.commander.CharaPos(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_CHARA_POS);
				return;
			case "#chara_face":
				this.commander.CharaFace(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_CHARA_FACE);
				return;
			case "#chara_del":
				this.commander.CharaDelete(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_CHARA_DELETE);
				return;
			case "#window_pic":
				this.commander.WindowPicture(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_WINDOW_PICTURE);
				return;
			case "#wait":
			{
				ScriptEngine.Status status = this.commander.Wait(commandParams);
				if (status != ScriptEngine.Status.NONE)
				{
					this.SetStatus(true, status);
				}
				return;
			}
			case "#mask":
				this.commander.Mask(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_MASK_CONTROL);
				return;
			case "#fade":
				this.commander.Fade(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_FADE);
				return;
			case "#save":
				this.commander.Save(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_SAVE);
				return;
			case "#scene":
				this.commander.Scene(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_SCENE);
				return;
			case "#shake":
				this.commander.Shake(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_SHAKE);
				return;
			case "#shake_stop":
				this.commander.ShakeStop(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_SHAKE_STOP);
				return;
			case "#ui_set":
				this.commander.UI(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_UI_SET);
				return;
			case "#ui_induce":
				this.commander.UI_Induce(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_UI_INDUCE);
				return;
			case "#ui_push":
				this.commander.UI(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_UI_PUSH);
				return;
			case "#ui_pop":
				this.commander.UI_Induce(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_UI_POP);
				return;
			case "#ui_close":
				this.SetStatus(true, ScriptEngine.Status.WAIT_UI_CLOSE);
				return;
			case "#farm_camera":
				this.commander.FarmCamera(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_FARM_CAMERA);
				return;
			case "#farm_facility":
				this.commander.FarmSelect(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_FARM_SELECT);
				return;
			case "#farm_build":
				this.commander.FarmBuild(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_FARM_BUILD);
				return;
			case "#farm_target":
				this.commander.FarmTarget(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_FARM_TARGET);
				return;
			case "#quest":
				this.commander.Quest(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_QUEST_SET);
				return;
			case "#battle_pause":
				this.commander.BattlePause(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_BATTLE_PAUSE);
				return;
			case "#battle_manual":
				this.SetStatus(false, ScriptEngine.Status.SET_BATTLE_MANUAL);
				return;
			case "#battle_end":
				this.SetStatus(true, ScriptEngine.Status.SET_BATTLE_END);
				return;
			case "#movie":
				this.SetStatus(true, ScriptEngine.Status.SET_MOVIE);
				return;
			case "#sound_volume":
				this.commander.SoundVolume(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_SOUND_VOLUME);
				return;
			case "#bgm":
				this.commander.Bgm(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_BGM);
				return;
			case "#se":
				this.commander.Se(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_SE);
				return;
			case "#digimon":
				this.commander.Digimon(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_DIGIMON);
				return;
			case "#digimon_del":
				this.SetStatus(false, ScriptEngine.Status.SET_DIGIMON_DELETE);
				return;
			case "#effect":
				this.commander.Effect(commandParams);
				this.SetStatus(true, ScriptEngine.Status.WAIT_SCREEN_EFFECT);
				return;
			case "#battle_auto":
				this.SetStatus(false, ScriptEngine.Status.SET_BATTLE_AUTO_ON);
				return;
			case "#meat":
				this.commander.Meat(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_MEAT_NUM);
				return;
			case "#stone":
				this.commander.DigiStone(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_DIGI_STONE_NUM);
				return;
			case "#digimon_exp":
				this.commander.DigimonExp(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_DIGIMON_EXP);
				return;
			case "#first_tutorial_complete":
				this.SetStatus(true, ScriptEngine.Status.WAIT_FIRST_TUTORIAL_END);
				return;
			case "#download":
				this.SetStatus(true, ScriptEngine.Status.WAIT_DOWNLOAD_CHECK);
				return;
			case "#end":
				this.SetStatus(false, ScriptEngine.Status.EOF);
				return;
			case "#standard_download":
				this.SetStatus(true, ScriptEngine.Status.START_STANDARD_DOWNLOAD);
				return;
			case "#background_download":
				this.SetStatus(true, ScriptEngine.Status.START_BACKGROUND_DOWNLOAD);
				return;
			case "#stop_download":
				this.SetStatus(true, ScriptEngine.Status.STOP_DOWNLOAD);
				return;
			case "#restart_download":
				this.SetStatus(true, ScriptEngine.Status.RESTART_DOWNLOAD);
				return;
			case "#skip":
				this.SetStatus(false, ScriptEngine.Status.SKIP_START);
				return;
			case "#skip_end":
				this.SetStatus(false, ScriptEngine.Status.SKIP_END);
				return;
			case "#battle_skip_end":
				this.SetStatus(false, ScriptEngine.Status.BATTLE_SKIP_END);
				return;
			case "#non_frame_text":
				this.SetStatus(false, ScriptEngine.Status.SET_NON_FRAME_TEXT);
				return;
			case "#non_frame_text_end":
				this.SetStatus(false, ScriptEngine.Status.SET_NON_FRAME_TEXT_END);
				return;
			case "#set_link_point":
				this.commander.SetLinkPoint(commandParams);
				this.SetStatus(false, ScriptEngine.Status.SET_LINK_POINT);
				return;
			case "#digimon_change":
				this.SetStatus(true, ScriptEngine.Status.WAIT_DIGIMON_CHANGE);
				return;
			}
			if (this.actionExternalCommand != null && !this.actionExternalCommand(commandParams))
			{
				global::Debug.LogError("未知のコマンド : " + commandParams[0]);
			}
		}

		public void SetStatus(bool isWaitStatus, ScriptEngine.Status status)
		{
			this.isWaitCallback = isWaitStatus;
			this.engineStatus = status;
		}

		public void Resume(int waitCommandAddress)
		{
			if (waitCommandAddress == this.commandAddress)
			{
				this.isWaitCallback = false;
				if (this.engineStatus != ScriptEngine.Status.EOF)
				{
					this.engineStatus = ScriptEngine.Status.NONE;
				}
			}
		}

		public void SetExternalCommand(Func<string[], bool> action)
		{
			this.actionExternalCommand = action;
		}

		public ScriptCommandParams.WindowInfo GetWindowInfo()
		{
			return this.commander.commandParams.windowInfo;
		}

		public ScriptCommandParams.TextInfo GetTextInfo()
		{
			return this.commander.commandParams.textInfo;
		}

		public ScriptCommandParams.SelectInfo GetSelectInfo()
		{
			return this.commander.commandParams.selectInfo;
		}

		public ScriptCommandParams.CharaInfo GetCharaInfo()
		{
			return this.commander.commandParams.charaInfo;
		}

		public ScriptCommandParams.WindowPictureInfo GetWindowPictureInfo()
		{
			return this.commander.commandParams.windowPictureInfo;
		}

		public float GetWaitTime()
		{
			return this.commander.commandParams.waitTime;
		}

		public bool GetMaskEnableFlag()
		{
			return this.commander.commandParams.enableMask;
		}

		public ScriptCommandParams.FadeInfo GetFadeInfo()
		{
			return this.commander.commandParams.fadeInfo;
		}

		public string GetSaveStateID()
		{
			return this.commander.commandParams.tutorialStateID;
		}

		public ScriptCommandParams.SceneInfo GetSceneInfo()
		{
			return this.commander.commandParams.sceneInfo;
		}

		public ScriptCommandParams.ShakeInfo GetShakeInfo()
		{
			return this.commander.commandParams.shakeInfo;
		}

		public ScriptCommandParams.UIInfo GetUiInfo()
		{
			return this.commander.commandParams.uiInfo;
		}

		public ScriptCommandParams.FarmCameraMoveInfo GetFarmCameraMoveInfo()
		{
			return this.commander.commandParams.farmCameraMoveInfo;
		}

		public ScriptCommandParams.SelectFacilityInfo GetSelectFacilityInfo()
		{
			return this.commander.commandParams.selectFacilityInfo;
		}

		public ScriptCommandParams.BuildFacilityInfo GetBuildFacilityInfo()
		{
			return this.commander.commandParams.buildFacilityInfo;
		}

		public ScriptCommandParams.TargetFacilityInfo GetTargetFacilityInfo()
		{
			return this.commander.commandParams.targetFacilityInfo;
		}

		public ScriptCommandParams.BattleInfo GetBattleInfo()
		{
			return this.commander.commandParams.battleInfo;
		}

		public ScriptCommandParams.SoundVolumeInfo GetSoundVolumeInfo()
		{
			return this.commander.commandParams.soundVolumeInfo;
		}

		public ScriptCommandParams.BgmInfo GetBgmInfo()
		{
			return this.commander.commandParams.bgmInfo;
		}

		public ScriptCommandParams.SeInfo GetSeInfo()
		{
			return this.commander.commandParams.seInfo;
		}

		public ScriptCommandParams.DigimonInfo GetDigimonInfo()
		{
			return this.commander.commandParams.digimonInfo;
		}

		public ScriptCommandParams.ScreenEffectInfo GetScreenEffectInfo()
		{
			return this.commander.commandParams.screenEffectInfo;
		}

		public int GetMeatNum()
		{
			return this.commander.commandParams.meatNum;
		}

		public int GetDigiStoneNum()
		{
			return this.commander.commandParams.digiStoneNum;
		}

		public int GetLinkPointNum()
		{
			return this.commander.commandParams.linkPointNum;
		}

		public ScriptCommandParams.DigimonExpInfo GetDidimonExpInfo()
		{
			return this.commander.commandParams.digimonExpInfo;
		}

		public int GetWaitOpenDetailUI()
		{
			return this.commander.commandParams.waitOpenDetailUI;
		}

		public void StepCommandScript(Action<ScriptEngine.Status, int> actionScriptCommand, Action updateCommand)
		{
			if (!this.isWaitCallback)
			{
				this.ActionCommandScript(actionScriptCommand);
			}
			else if (updateCommand != null)
			{
				updateCommand();
			}
		}

		public enum Status
		{
			NONE,
			WAIT_WINDOW,
			WAIT_WINDOW_DELETE,
			WAIT_MESSAGE_DISPLAY,
			WAIT_SELECT_DISPLAY,
			WAIT_CHARA,
			SET_CHARA_POS,
			SET_CHARA_FACE,
			WAIT_CHARA_DELETE,
			WAIT_WINDOW_PICTURE,
			WAIT_TIME,
			WAIT_TOUCH,
			WAIT_BATTLE_START,
			WAIT_BATTLE_ACTION_SELECT,
			WAIT_BATTLE_RESULT_START,
			WAIT_BATTLE_RESULT_END,
			WAIT_FIRST_CLEAR,
			WAIT_FARM_HARVEST,
			SET_MASK_CONTROL,
			WAIT_FADE,
			WAIT_SAVE,
			WAIT_SCENE,
			WAIT_SHAKE,
			WAIT_SHAKE_STOP,
			WAIT_UI_SET,
			WAIT_UI_INDUCE,
			WAIT_UI_PUSH,
			WAIT_UI_POP,
			WAIT_UI_CLOSE,
			WAIT_FARM_CAMERA,
			WAIT_FARM_SELECT,
			WAIT_FARM_BUILD,
			WAIT_FARM_BUILD_TAP,
			WAIT_FARM_TARGET,
			WAIT_OPEN_TRAINING_MENU,
			WAIT_OPEN_MEAL_DIGIMON_SELECT,
			WAIT_OPEN_MEAL_GIVE,
			WAIT_OPEN_GASHA_TOP,
			WAIT_OPEN_DIGIMON_DETAIL,
			WAIT_MEAL_LEVEL_UP,
			WAIT_OPEN_UI,
			WAIT_QUEST_SET,
			SET_BATTLE_PAUSE,
			SET_BATTLE_MANUAL,
			SET_BATTLE_END,
			SET_MOVIE,
			SET_SOUND_VOLUME,
			SET_BGM,
			SET_SE,
			WAIT_DIGIMON,
			SET_DIGIMON_DELETE,
			WAIT_SCREEN_EFFECT,
			SET_BATTLE_AUTO_ON,
			SET_MEAT_NUM,
			SET_DIGI_STONE_NUM,
			SET_DIGIMON_EXP,
			WAIT_FIRST_TUTORIAL_END,
			SET_RESTART_POINT,
			WAIT_DOWNLOAD_CHECK,
			WAIT_DOWNLOAD,
			WAIT_BUILD_MEAT_FARM,
			WAIT_DIGIVICE_OPEN,
			START_STANDARD_DOWNLOAD,
			START_BACKGROUND_DOWNLOAD,
			WAIT_BACKGROUND_DOWNLOAD,
			STOP_DOWNLOAD,
			RESTART_DOWNLOAD,
			SKIP_START,
			SKIP_END,
			SET_NON_FRAME_TEXT,
			SET_NON_FRAME_TEXT_END,
			BATTLE_SKIP_END,
			WAIT_DIGI_GARDEN_OPEN,
			WAIT_DIGI_GARDEN_CHANGE_LIST,
			WAIT_DIGI_GARDEN_CHANGE_SET_LIST,
			SET_LINK_POINT,
			WAIT_COLOSSEUM_OPEN,
			EXTERNAL_COMMAND,
			WAIT_FACILITY_SHOP_OPEN,
			WAIT_MISSION_OPEN,
			WAIT_DIGIMON_CHANGE,
			EOF
		}
	}
}
