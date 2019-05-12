using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TS;
using UnityEngine;

public sealed class TutorialRestart : TutorialCommandBase
{
	private string restartPoint;

	private TutorialRestart.CommandSkipStatus skipStatus;

	private TutorialRestart.TutorialStatus tutorialStatus;

	public TutorialRestart(ScriptEngine scriptEngine, TutorialUI tutorialUI, TutorialControlToGame controlToGame, string restartPoint) : base(scriptEngine, tutorialUI, controlToGame)
	{
		this.restartPoint = restartPoint;
		this.skipStatus = default(TutorialRestart.CommandSkipStatus);
		this.tutorialStatus = new TutorialRestart.TutorialStatus();
		this.actionList.Add(ScriptEngine.Status.WAIT_WINDOW, new Action(this.WaitWindow));
		this.actionList.Add(ScriptEngine.Status.WAIT_WINDOW_DELETE, new Action(this.WaitWindowDelete));
		this.actionList.Add(ScriptEngine.Status.WAIT_MESSAGE_DISPLAY, new Action(this.WaitMessageDisplay));
		this.actionList.Add(ScriptEngine.Status.WAIT_CHARA, new Action(this.WaitChara));
		this.actionList.Add(ScriptEngine.Status.SET_CHARA_POS, new Action(base.SetCharaPosition));
		this.actionList.Add(ScriptEngine.Status.SET_CHARA_FACE, new Action(this.SetCharaFace));
		this.actionList.Add(ScriptEngine.Status.WAIT_CHARA_DELETE, new Action(this.WaitCharaDelete));
		this.actionList.Add(ScriptEngine.Status.SET_MASK_CONTROL, new Action(this.SetMaskControl));
		this.actionList.Add(ScriptEngine.Status.WAIT_FADE, new Action(this.WaitFade));
		this.actionList.Add(ScriptEngine.Status.WAIT_SAVE, new Action(this.WaitSave));
		this.actionList.Add(ScriptEngine.Status.WAIT_SCENE, new Action(this.WaitScene));
		this.actionList.Add(ScriptEngine.Status.WAIT_SHAKE, new Action(this.WaitShake));
		this.actionList.Add(ScriptEngine.Status.WAIT_SHAKE_STOP, new Action(this.WaitShakeStop));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_INDUCE, new Action(this.WaitUIInduce));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_PUSH, new Action(this.WaitUIPush));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_POP, new Action(this.WaitUIPop));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_CLOSE, new Action(this.WaitUIClose));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_CAMERA, new Action(this.WaitFarmCamera));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_BUILD, new Action(this.WaitFarmBuild));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_BUILD_TAP, new Action(this.WaitBuildCompleteTap));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_TRAINING_MENU, new Action(this.WaitOpenTrainingMenu));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_MEAL_DIGIMON_SELECT, new Action(this.WaitOpenMealDigimonSelect));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_MEAL_GIVE, new Action(this.WaitOpenMealGive));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_GASHA_TOP, new Action(this.WaitOpenGashaTop));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_DIGIMON_DETAIL, new Action(this.WaitOpenDigimonDetail));
		this.actionList.Add(ScriptEngine.Status.SET_SOUND_VOLUME, new Action(this.SetSoundVolume));
		this.actionList.Add(ScriptEngine.Status.SET_BGM, new Action(this.SetBgm));
		this.actionList.Add(ScriptEngine.Status.SET_SE, new Action(this.SetSe));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGIMON, new Action(this.WaitDigimon));
		this.actionList.Add(ScriptEngine.Status.SET_DIGIMON_DELETE, new Action(this.SetDigimonDelete));
		this.actionList.Add(ScriptEngine.Status.SET_DIGIMON_EXP, new Action(this.SetDigimonExp));
		this.actionList.Add(ScriptEngine.Status.WAIT_DOWNLOAD_CHECK, new Action(this.WaitDownloadCheck));
		this.actionList.Add(ScriptEngine.Status.WAIT_BUILD_MEAT_FARM, new Action(this.WaitBuildMeatFarm));
		this.actionList.Add(ScriptEngine.Status.SKIP_START, new Action(this.SkipStart));
		this.actionList.Add(ScriptEngine.Status.SKIP_END, new Action(this.SkipEnd));
		this.actionList.Add(ScriptEngine.Status.SET_NON_FRAME_TEXT, new Action(this.SetNonFrameText));
		this.actionList.Add(ScriptEngine.Status.SET_NON_FRAME_TEXT_END, new Action(this.SetNonFrameTextEnd));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGIMON_CHANGE, new Action(this.WaitDigimonChange));
	}

	public IEnumerator SkipCommand()
	{
		return this.scriptEngine.RestartScript(new Action<ScriptEngine.Status, int>(base.ActionScriptCommand));
	}

	public IEnumerator RetryDownload()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		int levelIndex = tutorialObserver.DownloaddAssetBundleLevelIndex;
		string[] assetNameList = tutorialObserver.assetNameList;
		AssetDataMng manager = AssetDataMng.Instance();
		if (levelIndex > 0)
		{
			List<string> downloadList = new List<string>();
			for (int j = 0; j <= levelIndex - 1; j++)
			{
				if (manager.GetDownloadAssetBundleCount(assetNameList[j]) > 0)
				{
					downloadList.Add(assetNameList[j]);
				}
			}
			string[] levels = downloadList.ToArray();
			if (levels.Length > 0)
			{
				Loading.Invisible();
				bool isEndChangeScene = false;
				this.controlToGame.ChangeScene(this.controlToGame.GetBlackSceneID(), delegate
				{
					isEndChangeScene = true;
				}, false, null);
				while (!isEndChangeScene)
				{
					yield return null;
				}
				bool isEndStartFade = false;
				this.tutorialUI.Fade.StartFade(TutorialFade.FadeType.BLACK, false, 0.5f, delegate
				{
					isEndStartFade = true;
				});
				while (!isEndStartFade)
				{
					yield return null;
				}
				bool isEndDownload = false;
				for (int i = 0; i < levels.Length; i++)
				{
					if (0 < manager.GetDownloadAssetBundleCount(levels[i]))
					{
						isEndDownload = false;
						this.controlToGame.StartDownload(levels[i], delegate
						{
							isEndDownload = true;
						});
						while (!isEndDownload)
						{
							yield return null;
						}
					}
				}
				Loading.ResumeDisplay();
			}
		}
		yield break;
	}

	public IEnumerator ReproduceTutorialStatus()
	{
		TutorialCreateRestartScreen createRestartScreen = new TutorialCreateRestartScreen(this.tutorialUI);
		bool isFinishedFade = false;
		TutorialFade.FadeType fadeType = TutorialFade.FadeType.BLACK;
		if (this.tutorialStatus.fadeInfo.enable && this.tutorialStatus.fadeInfo.type == 0)
		{
			fadeType = TutorialFade.FadeType.WHITE;
		}
		this.tutorialUI.Fade.StartFade(fadeType, true, 0.3f, delegate
		{
			isFinishedFade = true;
		});
		while (!isFinishedFade)
		{
			yield return null;
		}
		if (this.tutorialStatus.displayMask)
		{
			this.tutorialUI.MaskBlack.SetEnable(this.scriptEngine.GetMaskEnableFlag(), null, false);
		}
		yield return AppCoroutine.Start(createRestartScreen.CreateRestartScreen(this.tutorialStatus.sceneType, this.tutorialStatus.farmBuildFacilityInfo, this.scriptEngine.GetMeatNum(), this.scriptEngine.GetDigiStoneNum(), this.scriptEngine.GetLinkPointNum(), this.tutorialStatus.expInfo, this.tutorialStatus.openUI, this.tutorialStatus.openConfirmUI, this.tutorialStatus.selectUI, this.controlToGame), false);
		this.CreateScreenParts(createRestartScreen);
		yield return AppCoroutine.Start(createRestartScreen.CheckFinish(), false);
		if (!this.tutorialStatus.fadeInfo.enable)
		{
			isFinishedFade = false;
			this.tutorialUI.Fade.StartFade(fadeType, false, 0.5f, delegate
			{
				isFinishedFade = true;
			});
			while (!isFinishedFade)
			{
				yield return null;
			}
		}
		yield break;
	}

	public void CreateScreenParts(TutorialCreateRestartScreen createRestartScreen)
	{
		ScriptEngine scriptEngine = this.scriptEngine;
		if (this.tutorialStatus.displayTextWindow)
		{
			ScriptCommandParams.WindowInfo windowInfo = scriptEngine.GetWindowInfo();
			createRestartScreen.OpenMessageWindow(windowInfo.xFromCenter, windowInfo.yFromCenter);
			ScriptCommandParams.TextInfo textInfo = scriptEngine.GetTextInfo();
			TutorialMaster.NaviMessage naviMessage = MasterDataMng.Instance().Tutorial.GetNaviMessage(textInfo.displayText);
			this.tutorialUI.MessageWindow.SetDisplayMessage(naviMessage.message);
		}
		if (this.tutorialStatus.displayNonFrameText)
		{
			this.tutorialUI.NonFrameText.Open(NGUIText.Alignment.Center);
		}
		if (this.tutorialStatus.displayChara)
		{
			ScriptCommandParams.CharaInfo charaInfo = scriptEngine.GetCharaInfo();
			createRestartScreen.DisplayChara(charaInfo.type, this.tutorialStatus.charaFaceId, charaInfo.yFromCenter);
		}
		if (this.scriptEngine.GetMaskEnableFlag())
		{
			createRestartScreen.DisplayScreenMask();
		}
		if (this.tutorialStatus.displayTargetPopUI)
		{
			ScriptCommandParams.UIInfo uiInfo = this.scriptEngine.GetUiInfo();
			createRestartScreen.DisplayUIPop(uiInfo.type, uiInfo.arrowPosition, this.controlToGame);
		}
		if (this.tutorialStatus.farmCameraMove)
		{
			ScriptCommandParams.FarmCameraMoveInfo farmCameraMoveInfo = this.scriptEngine.GetFarmCameraMoveInfo();
			createRestartScreen.SetFarmCameraPosition(farmCameraMoveInfo.posGridX, farmCameraMoveInfo.posGridY, this.controlToGame);
		}
		if (this.tutorialStatus.shakeBackGround)
		{
			ScriptCommandParams.ShakeInfo shakeInfo = this.scriptEngine.GetShakeInfo();
			createRestartScreen.ShakeBackGround(shakeInfo.intensity, shakeInfo.decay, this.controlToGame);
		}
		if (0 < this.tutorialStatus.farmBuildCompleteList.Count)
		{
			createRestartScreen.FacilityBuildComplete(this.tutorialStatus.farmBuildCompleteList, this.controlToGame);
		}
		if (this.scriptEngine.GetSelectFacilityInfo().selected)
		{
			createRestartScreen.SetSelectFarmFacility(this.scriptEngine.GetSelectFacilityInfo().id, this.controlToGame);
		}
		if (this.scriptEngine.GetTargetFacilityInfo().popEnable)
		{
			ScriptCommandParams.TargetFacilityInfo targetFacilityInfo = this.scriptEngine.GetTargetFacilityInfo();
			createRestartScreen.SetTargetFarmFacility(targetFacilityInfo.id, targetFacilityInfo.popEnable, this.controlToGame, targetFacilityInfo.adjustY);
		}
		if (this.tutorialStatus.displayDigimon)
		{
			ScriptCommandParams.DigimonInfo digimonInfo = this.scriptEngine.GetDigimonInfo();
			createRestartScreen.DisplayDigimon(digimonInfo.monsterGroupID, digimonInfo.scale, digimonInfo.adjustPosition, this.controlToGame);
		}
		if (this.tutorialStatus.digimonChange)
		{
			createRestartScreen.LocalDigimonEvolution(this.controlToGame);
		}
		if (this.tutorialStatus.displaySkipButton)
		{
			this.tutorialUI.SetSkipButton(true, null);
		}
		if (this.scriptEngine.GetScreenEffectInfo().start)
		{
			createRestartScreen.DisplayScreenEffect(this.scriptEngine.GetScreenEffectInfo().type, this.controlToGame);
		}
		if (this.tutorialStatus.bgmCommandEnable)
		{
			ScriptCommandParams.BgmInfo bgmInfo = this.tutorialStatus.bgmInfo;
			this.controlToGame.SetBgm(bgmInfo.fileName, bgmInfo.play, bgmInfo.fadeTime);
		}
		if (0 < this.tutorialStatus.loopSeInfo.Count)
		{
			for (int i = 0; i < this.tutorialStatus.loopSeInfo.Count; i++)
			{
				ScriptCommandParams.SeInfo seInfo = this.tutorialStatus.loopSeInfo[i];
				this.controlToGame.SetSe(seInfo.fileName, seInfo.play, seInfo.fadeTime, seInfo.loop, seInfo.pitch);
			}
		}
	}

	private void WaitSave()
	{
		if (this.skipStatus.invalidRestart)
		{
			this.skipStatus.invalidRestart = false;
			this.tutorialStatus.sceneType = this.controlToGame.GetBlueSceneID();
			this.tutorialStatus.fadeInfo.enable = false;
			this.tutorialStatus.bgmCommandEnable = true;
			this.tutorialStatus.bgmInfo = new ScriptCommandParams.BgmInfo
			{
				fileName = "BGM/bgm_303/sound",
				play = true,
				fadeTime = 0.5f
			};
		}
		if (this.restartPoint == this.scriptEngine.GetSaveStateID())
		{
			this.scriptEngine.StopSkip();
		}
	}

	private void WaitWindow()
	{
		if (!this.skipStatus.invalidRestart)
		{
			this.tutorialStatus.displayTextWindow = true;
		}
	}

	private void WaitWindowDelete()
	{
		if (!this.skipStatus.invalidRestart)
		{
			this.tutorialStatus.displayTextWindow = false;
		}
	}

	private void WaitMessageDisplay()
	{
		ScriptCommandParams.TextInfo textInfo = this.scriptEngine.GetTextInfo();
		if (textInfo.isWindowText)
		{
			TutorialMaster.NaviMessage naviMessage = MasterDataMng.Instance().Tutorial.GetNaviMessage(textInfo.displayText);
			if ("0" != naviMessage.faceId)
			{
				this.tutorialStatus.charaFaceId = naviMessage.faceId;
			}
		}
	}

	private void WaitChara()
	{
		if (!this.skipStatus.invalidRestart)
		{
			this.tutorialStatus.displayChara = true;
			ScriptCommandParams.CharaInfo charaInfo = this.scriptEngine.GetCharaInfo();
			this.tutorialStatus.charaFaceId = charaInfo.faceId;
		}
	}

	private void WaitCharaDelete()
	{
		if (!this.skipStatus.invalidRestart)
		{
			this.tutorialStatus.displayChara = false;
		}
	}

	private void SetCharaFace()
	{
		if (!this.skipStatus.invalidRestart)
		{
			ScriptCommandParams.CharaInfo charaInfo = this.scriptEngine.GetCharaInfo();
			this.tutorialStatus.charaFaceId = charaInfo.faceId;
		}
	}

	private void SetMaskControl()
	{
		if (!this.skipStatus.invalidRestart)
		{
			this.tutorialStatus.displayMask = this.scriptEngine.GetMaskEnableFlag();
		}
	}

	private void WaitFade()
	{
		if (!this.skipStatus.invalidRestart)
		{
			this.tutorialStatus.fadeInfo = this.scriptEngine.GetFadeInfo();
		}
	}

	private void WaitScene()
	{
		ScriptCommandParams.SceneInfo sceneInfo = this.scriptEngine.GetSceneInfo();
		this.skipStatus.invalidRestart = sceneInfo.isBattle;
		this.tutorialStatus.sceneType = sceneInfo.type;
		this.tutorialStatus.openConfirmUI.Clear();
		this.tutorialStatus.openUI.Clear();
		this.tutorialStatus.selectUI.Clear();
		this.tutorialStatus.farmCameraMove = false;
		this.tutorialStatus.bgmCommandEnable = false;
	}

	private void WaitShake()
	{
		this.tutorialStatus.shakeBackGround = true;
	}

	private void WaitShakeStop()
	{
		this.tutorialStatus.shakeBackGround = false;
	}

	private void WaitUIInduce()
	{
		if (!this.skipStatus.invalidRestart)
		{
			string type = this.scriptEngine.GetUiInfo().type;
			if (type != null)
			{
				if (!(type == "SHORTCUT"))
				{
					if (!(type == "GASHA_START"))
					{
						if (!(type == "MEAL_DIGI"))
						{
							if (type == "MEAL_OK")
							{
								this.tutorialStatus.selectUI.Clear();
							}
						}
						else
						{
							this.tutorialStatus.selectUI.Add("MEAL_DIGI");
						}
					}
					else
					{
						this.tutorialStatus.openConfirmUI.Add("GASHA_START");
					}
				}
				else
				{
					this.tutorialStatus.openConfirmUI.Add("SHORTCUT");
				}
			}
		}
	}

	private void WaitUIPush()
	{
		if (!this.skipStatus.invalidRestart && "YES_BUTTON" == this.scriptEngine.GetUiInfo().type)
		{
			this.tutorialStatus.openConfirmUI.Clear();
		}
	}

	private void WaitUIPop()
	{
		ScriptCommandParams.UIInfo uiInfo = this.scriptEngine.GetUiInfo();
		this.tutorialStatus.displayTargetPopUI = uiInfo.enabled;
	}

	private void WaitUIClose()
	{
		this.tutorialStatus.openConfirmUI.Clear();
		this.tutorialStatus.openUI.Clear();
		this.tutorialStatus.selectUI.Clear();
	}

	private void WaitFarmCamera()
	{
		this.tutorialStatus.farmCameraMove = true;
	}

	private void WaitFarmBuild()
	{
		ScriptCommandParams.BuildFacilityInfo buildFacilityInfo = this.scriptEngine.GetBuildFacilityInfo();
		List<ScriptCommandParams.BuildFacilityInfo> farmBuildFacilityInfo = this.tutorialStatus.farmBuildFacilityInfo;
		for (int i = 0; i < farmBuildFacilityInfo.Count; i++)
		{
			if (farmBuildFacilityInfo[i].id == buildFacilityInfo.id && farmBuildFacilityInfo[i].posGridX == buildFacilityInfo.posGridX && farmBuildFacilityInfo[i].posGridY == buildFacilityInfo.posGridY)
			{
				this.tutorialStatus.farmBuildFacilityInfo.RemoveAt(i);
				break;
			}
		}
		this.tutorialStatus.farmBuildFacilityInfo.Add(buildFacilityInfo);
	}

	private void WaitBuildCompleteTap()
	{
		ScriptCommandParams.BuildFacilityInfo buildFacilityInfo = this.scriptEngine.GetBuildFacilityInfo();
		this.tutorialStatus.farmBuildCompleteList.Add(buildFacilityInfo.id);
	}

	private void WaitOpenTrainingMenu()
	{
		this.tutorialStatus.openUI.Add("CMD_Training_Menu");
	}

	private void WaitOpenMealDigimonSelect()
	{
		this.tutorialStatus.openUI.Add("CMD_BaseSelect");
	}

	private void WaitOpenMealGive()
	{
		this.tutorialStatus.openUI.Add("CMD_MealExecution");
	}

	private void WaitOpenGashaTop()
	{
		this.tutorialStatus.openUI.Add("CMD_GashaTOP");
	}

	private void WaitOpenDigimonDetail()
	{
		this.tutorialStatus.openUI.Add("CMD_CharacterDetailed");
	}

	private void SetSoundVolume()
	{
		if (!this.skipStatus.invalidRestart)
		{
		}
	}

	private void SetBgm()
	{
		if (!this.skipStatus.invalidRestart)
		{
			this.tutorialStatus.bgmCommandEnable = true;
			this.tutorialStatus.bgmInfo = this.scriptEngine.GetBgmInfo();
		}
	}

	private void SetSe()
	{
		if (!this.skipStatus.invalidRestart)
		{
			ScriptCommandParams.SeInfo info = this.scriptEngine.GetSeInfo();
			if (info.play && info.loop)
			{
				if (!this.tutorialStatus.loopSeInfo.Any((ScriptCommandParams.SeInfo x) => x.fileName == info.fileName))
				{
					this.tutorialStatus.loopSeInfo.Add(info);
				}
			}
			else if (this.tutorialStatus.loopSeInfo.Any((ScriptCommandParams.SeInfo x) => x.fileName == info.fileName))
			{
				int index = this.tutorialStatus.loopSeInfo.FindIndex((ScriptCommandParams.SeInfo x) => x.fileName == info.fileName);
				this.tutorialStatus.loopSeInfo.RemoveAt(index);
			}
		}
	}

	private void WaitDigimon()
	{
		this.tutorialStatus.displayDigimon = true;
	}

	private void SetDigimonDelete()
	{
		this.tutorialStatus.displayDigimon = false;
	}

	private void SetDigimonExp()
	{
		ScriptCommandParams.DigimonExpInfo info = this.scriptEngine.GetDidimonExpInfo();
		if (this.tutorialStatus.expInfo.Any((ScriptCommandParams.DigimonExpInfo x) => x.index == info.index))
		{
			ScriptCommandParams.DigimonExpInfo item = this.tutorialStatus.expInfo.Single((ScriptCommandParams.DigimonExpInfo x) => x.index == info.index);
			this.tutorialStatus.expInfo.Remove(item);
		}
		this.tutorialStatus.expInfo.Add(info);
	}

	private void WaitDownloadCheck()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (null != tutorialObserver)
		{
			tutorialObserver.DownloaddAssetBundleLevelIndex++;
		}
	}

	private void WaitBuildMeatFarm()
	{
		List<ScriptCommandParams.BuildFacilityInfo> farmBuildFacilityInfo = this.tutorialStatus.farmBuildFacilityInfo;
		for (int i = 0; i < farmBuildFacilityInfo.Count; i++)
		{
			if (farmBuildFacilityInfo[i].id == 1)
			{
				ScriptCommandParams.BuildFacilityInfo value = farmBuildFacilityInfo[i];
				value.buildTime = 0;
				farmBuildFacilityInfo[i] = value;
			}
		}
	}

	private void SkipStart()
	{
		this.tutorialStatus.displaySkipButton = true;
	}

	private void SkipEnd()
	{
		this.tutorialStatus.displaySkipButton = false;
	}

	private void SetNonFrameText()
	{
		this.tutorialStatus.displayNonFrameText = true;
	}

	private void SetNonFrameTextEnd()
	{
		this.tutorialStatus.displayNonFrameText = false;
	}

	private void WaitDigimonChange()
	{
		this.tutorialStatus.digimonChange = true;
	}

	private struct CommandSkipStatus
	{
		public bool invalidRestart;
	}

	private class TutorialStatus
	{
		public int sceneType = -1;

		public List<string> openUI = new List<string>();

		public List<string> openConfirmUI = new List<string>();

		public List<string> selectUI = new List<string>();

		public List<int> farmBuildCompleteList = new List<int>();

		public List<ScriptCommandParams.BuildFacilityInfo> farmBuildFacilityInfo = new List<ScriptCommandParams.BuildFacilityInfo>();

		public List<ScriptCommandParams.SeInfo> loopSeInfo = new List<ScriptCommandParams.SeInfo>();

		public List<ScriptCommandParams.DigimonExpInfo> expInfo = new List<ScriptCommandParams.DigimonExpInfo>();

		public ScriptCommandParams.FadeInfo fadeInfo;

		public ScriptCommandParams.BgmInfo bgmInfo;

		public string charaFaceId;

		public bool displayMask;

		public bool displayTextWindow;

		public bool displayNonFrameText;

		public bool displayChara;

		public bool displayDigimon;

		public bool displayTargetPopUI;

		public bool displaySkipButton;

		public bool farmCameraMove;

		public bool shakeBackGround;

		public bool bgmCommandEnable;

		public bool digimonChange;
	}
}
