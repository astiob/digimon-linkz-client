using System;
using System.Collections;
using TS;
using TutorialRequestHeader;
using UnityEngine;

public class TutorialCommandAction : TutorialCommandBase
{
	private double waitFinishedTime;

	private double waitCurrentTime;

	private bool isApplicationResume;

	public TutorialCommandAction(ScriptEngine scriptEngine, TutorialUI tutorialUI, TutorialControlToGame controlToGame) : base(scriptEngine, tutorialUI, controlToGame)
	{
		this.actionList.Add(ScriptEngine.Status.WAIT_WINDOW, new Action(this.WaitWindow));
		this.actionList.Add(ScriptEngine.Status.WAIT_WINDOW_DELETE, new Action(this.WaitWindowDelete));
		this.actionList.Add(ScriptEngine.Status.WAIT_MESSAGE_DISPLAY, new Action(this.WaitMessageDisplay));
		this.actionList.Add(ScriptEngine.Status.WAIT_SELECT_DISPLAY, new Action(this.WaitSelectDisplay));
		this.actionList.Add(ScriptEngine.Status.WAIT_CHARA, new Action(this.WaitChara));
		this.actionList.Add(ScriptEngine.Status.SET_CHARA_POS, new Action(base.SetCharaPosition));
		this.actionList.Add(ScriptEngine.Status.SET_CHARA_FACE, new Action(this.SetCharaFace));
		this.actionList.Add(ScriptEngine.Status.WAIT_CHARA_DELETE, new Action(this.WaitCharaDelete));
		this.actionList.Add(ScriptEngine.Status.WAIT_WINDOW_PICTURE, new Action(this.WaitWindowPicture));
		this.actionList.Add(ScriptEngine.Status.WAIT_TIME, new Action(this.WaitTime));
		this.actionList.Add(ScriptEngine.Status.WAIT_TOUCH, new Action(this.WaitTouch));
		this.actionList.Add(ScriptEngine.Status.WAIT_BATTLE_START, new Action(this.WaitBattleStart));
		this.actionList.Add(ScriptEngine.Status.WAIT_BATTLE_ACTION_SELECT, new Action(this.WaitBattleActionSelect));
		this.actionList.Add(ScriptEngine.Status.WAIT_BATTLE_RESULT_START, new Action(this.WaitBattleResultStart));
		this.actionList.Add(ScriptEngine.Status.WAIT_BATTLE_RESULT_END, new Action(this.WaitBattleResultEnd));
		this.actionList.Add(ScriptEngine.Status.WAIT_FIRST_CLEAR, new Action(this.WaitFirstClear));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_HARVEST, new Action(this.WaitFarmHarvest));
		this.actionList.Add(ScriptEngine.Status.SET_MASK_CONTROL, new Action(this.SetMaskControl));
		this.actionList.Add(ScriptEngine.Status.WAIT_FADE, new Action(this.WaitFade));
		this.actionList.Add(ScriptEngine.Status.WAIT_SAVE, new Action(this.WaitSave));
		this.actionList.Add(ScriptEngine.Status.WAIT_SCENE, new Action(this.WaitScene));
		this.actionList.Add(ScriptEngine.Status.WAIT_SHAKE, new Action(this.WaitShake));
		this.actionList.Add(ScriptEngine.Status.WAIT_SHAKE_STOP, new Action(this.WaitShakeStop));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_SET, new Action(this.WaitUISet));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_INDUCE, new Action(this.WaitUIInduce));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_PUSH, new Action(this.WaitUIPush));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_POP, new Action(this.WaitUIPop));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_CLOSE, new Action(this.WaitUIClose));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_CAMERA, new Action(this.WaitFarmCamera));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_SELECT, new Action(this.WaitFarmSelect));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_BUILD, new Action(this.WaitFarmBuild));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_BUILD_TAP, new Action(this.WaitBuildCompleteTap));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_TARGET, new Action(this.WaitFarmTarget));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_TRAINING_MENU, new Action(this.WaitOpenTrainingMenu));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_MEAL_DIGIMON_SELECT, new Action(this.WaitOpenMealDigimonSelect));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_MEAL_GIVE, new Action(this.WaitOpenMealGive));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_GASHA_TOP, new Action(this.WaitOpenGashaTop));
		this.actionList.Add(ScriptEngine.Status.WAIT_OPEN_DIGIMON_DETAIL, new Action(this.WaitOpenDigimonDetail));
		this.actionList.Add(ScriptEngine.Status.WAIT_MEAL_LEVEL_UP, new Action(this.WaitMealLevelUp));
		this.actionList.Add(ScriptEngine.Status.WAIT_QUEST_SET, new Action(this.WaitQuestSet));
		this.actionList.Add(ScriptEngine.Status.SET_BATTLE_PAUSE, new Action(this.SetBattlePause));
		this.actionList.Add(ScriptEngine.Status.SET_BATTLE_MANUAL, new Action(this.SetBattleManual));
		this.actionList.Add(ScriptEngine.Status.SET_BATTLE_END, new Action(this.SetBattleEnd));
		this.actionList.Add(ScriptEngine.Status.SET_MOVIE, new Action(this.SetMovie));
		this.actionList.Add(ScriptEngine.Status.SET_SOUND_VOLUME, new Action(this.SetSoundVolume));
		this.actionList.Add(ScriptEngine.Status.SET_BGM, new Action(this.SetBgm));
		this.actionList.Add(ScriptEngine.Status.SET_SE, new Action(this.SetSe));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGIMON, new Action(this.WaitDigimon));
		this.actionList.Add(ScriptEngine.Status.SET_DIGIMON_DELETE, new Action(this.SetDigimonDelete));
		this.actionList.Add(ScriptEngine.Status.WAIT_SCREEN_EFFECT, new Action(this.WaitScreenEffect));
		this.actionList.Add(ScriptEngine.Status.SET_BATTLE_AUTO_ON, new Action(this.SetBattleAutoOn));
		this.actionList.Add(ScriptEngine.Status.SET_MEAT_NUM, new Action(base.SetMeatNum));
		this.actionList.Add(ScriptEngine.Status.SET_DIGI_STONE_NUM, new Action(base.SetDigiStoneNum));
		this.actionList.Add(ScriptEngine.Status.SET_DIGIMON_EXP, new Action(this.SetDigimonExp));
		this.actionList.Add(ScriptEngine.Status.WAIT_FIRST_TUTORIAL_END, new Action(this.WaitFirstTutorialEnd));
		this.actionList.Add(ScriptEngine.Status.WAIT_DOWNLOAD_CHECK, new Action(this.WaitDownloadCheck));
		this.actionList.Add(ScriptEngine.Status.WAIT_DOWNLOAD, new Action(this.WaitDownload));
		this.actionList.Add(ScriptEngine.Status.WAIT_BUILD_MEAT_FARM, new Action(base.ResumeScript));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGIVICE_OPEN, new Action(this.WaitDigiviceOpen));
		this.actionList.Add(ScriptEngine.Status.START_STANDARD_DOWNLOAD, new Action(this.StartStandardDownload));
		this.actionList.Add(ScriptEngine.Status.START_BACKGROUND_DOWNLOAD, new Action(this.StartBackgroundDownload));
		this.actionList.Add(ScriptEngine.Status.STOP_DOWNLOAD, new Action(this.StopDownload));
		this.actionList.Add(ScriptEngine.Status.RESTART_DOWNLOAD, new Action(this.RestartDownload));
		this.actionList.Add(ScriptEngine.Status.SKIP_START, new Action(this.SkipStart));
		this.actionList.Add(ScriptEngine.Status.SKIP_END, new Action(this.SkipEnd));
		this.actionList.Add(ScriptEngine.Status.SET_NON_FRAME_TEXT, new Action(this.SetNonFrameText));
		this.actionList.Add(ScriptEngine.Status.SET_NON_FRAME_TEXT_END, new Action(this.SetNonFrameTextEnd));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGI_GARDEN_OPEN, new Action(this.WaitDigiGardenOpen));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGI_GARDEN_CHANGE_LIST, new Action(this.WaitDigiGardenChangeList));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGI_GARDEN_CHANGE_SET_LIST, new Action(this.WaitDigiGardenChangeSetList));
		this.actionList.Add(ScriptEngine.Status.SET_LINK_POINT, new Action(base.SetLinkPointNum));
		this.actionList.Add(ScriptEngine.Status.WAIT_COLOSSEUM_OPEN, new Action(this.WaitColosseumOpen));
		this.actionList.Add(ScriptEngine.Status.WAIT_FACILITY_SHOP_OPEN, new Action(this.WaitFacilityShopOpen));
		this.actionList.Add(ScriptEngine.Status.WAIT_MISSION_OPEN, new Action(this.WaitMissionOpen));
		this.actionList.Add(ScriptEngine.Status.WAIT_DIGIMON_CHANGE, new Action(this.WaitDigimonChange));
		this.actionList.Add(ScriptEngine.Status.EOF, new Action(this.Eof));
	}

	private void WaitWindow()
	{
		ScriptCommandParams.WindowInfo windowInfo = this.scriptEngine.GetWindowInfo();
		this.tutorialUI.MessageWindow.DisplayWindow(windowInfo.xFromCenter, windowInfo.yFromCenter, new Action(base.ResumeScript), true);
	}

	private void WaitWindowDelete()
	{
		this.tutorialUI.MessageWindow.DeleteWindow(new Action(base.ResumeScript), true);
	}

	private void WaitMessageDisplay()
	{
		ScriptCommandParams.TextInfo textInfo = this.scriptEngine.GetTextInfo();
		TutorialMaster.NaviMessage naviMessage = MasterDataMng.Instance().Tutorial.GetNaviMessage(textInfo.displayText);
		if (textInfo.isWindowText)
		{
			if ("0" != naviMessage.faceId)
			{
				this.tutorialUI.Thumbnail.SetFace(naviMessage.faceId);
			}
			this.tutorialUI.MessageWindow.SetMessage(naviMessage.message);
			this.tutorialUI.MessageWindow.StartDisplayMessage(new Action(this.OnFinishedDisplayMessage));
		}
		else
		{
			this.tutorialUI.NonFrameText.SetText(naviMessage.message);
			this.tutorialUI.NonFrameText.StartDisplay(textInfo.fadeTime, new Action(this.OnPushedFadeMessage));
		}
	}

	private void OnFinishedDisplayMessage()
	{
		ScriptCommandParams.TextInfo textInfo = this.scriptEngine.GetTextInfo();
		this.tutorialUI.MessageWindow.SetEnableTapIcon(true, new Action(base.ResumeScript));
		if (0f < textInfo.autoFeedTime)
		{
			this.tutorialUI.MessageWindow.StartAutoFeedCountDown(textInfo.autoFeedTime);
		}
	}

	private void OnPushedFadeMessage()
	{
		ScriptCommandParams.TextInfo textInfo = this.scriptEngine.GetTextInfo();
		this.tutorialUI.NonFrameText.StartInvisible(textInfo.fadeTime, new Action(base.ResumeScript));
	}

	private void WaitSelectDisplay()
	{
		ScriptCommandParams.SelectInfo selectInfo = this.scriptEngine.GetSelectInfo();
		for (int i = 0; i < selectInfo.displayText.Count; i++)
		{
			TutorialMaster.NaviMessage naviMessage = MasterDataMng.Instance().Tutorial.GetNaviMessage(selectInfo.displayText[i]);
			this.tutorialUI.SelectItem.SetSelectItem(i, naviMessage.message, new Action(base.ResumeScript));
			this.tutorialUI.SelectItem.StartDisplay();
		}
	}

	private void WaitChara()
	{
		ScriptCommandParams.CharaInfo charaInfo = this.scriptEngine.GetCharaInfo();
		TutorialThumbnail.ThumbnailType thumbnailType = (charaInfo.type != 0) ? TutorialThumbnail.ThumbnailType.MONITOR : TutorialThumbnail.ThumbnailType.BODY;
		this.tutorialUI.Thumbnail.Display(thumbnailType, null, true);
		this.tutorialUI.Thumbnail.SetFace(charaInfo.faceId);
		if (thumbnailType == TutorialThumbnail.ThumbnailType.MONITOR)
		{
			this.tutorialUI.Thumbnail.SetMonitorPosition(charaInfo.yFromCenter);
		}
	}

	private void WaitCharaDelete()
	{
		TutorialThumbnail.ThumbnailType type = (this.scriptEngine.GetCharaInfo().type != 0) ? TutorialThumbnail.ThumbnailType.MONITOR : TutorialThumbnail.ThumbnailType.BODY;
		this.tutorialUI.Thumbnail.Delete(type, new Action(base.ResumeScript), true);
	}

	private void SetCharaFace()
	{
		ScriptCommandParams.CharaInfo charaInfo = this.scriptEngine.GetCharaInfo();
		this.tutorialUI.Thumbnail.SetFace(charaInfo.faceId);
	}

	private void WaitWindowPicture()
	{
		ScriptCommandParams.WindowPictureInfo windowPictureInfo = this.scriptEngine.GetWindowPictureInfo();
		if (windowPictureInfo.thumbnail)
		{
			this.tutorialUI.Thumbnail.Display(TutorialThumbnail.ThumbnailType.BODY, null, true);
		}
		AppCoroutine.Start(this.tutorialUI.ImageWindow.OpenWindow(windowPictureInfo.prefabNames, windowPictureInfo.thumbnail, new Action(base.ResumeScript), null), false);
	}

	private void WaitTime()
	{
		AppCoroutine.Start(this.WaitTimeRoutine(this.scriptEngine.GetWaitTime()), false);
	}

	private IEnumerator WaitTimeRoutine(float time)
	{
		this.isApplicationResume = false;
		float waitStartUnscaledTime = Time.unscaledTime;
		this.waitCurrentTime = (double)Time.unscaledTime;
		this.waitFinishedTime = (double)(Time.unscaledTime + time);
		while (this.waitCurrentTime < this.waitFinishedTime)
		{
			yield return new WaitForEndOfFrame();
			if (this.isApplicationResume)
			{
				this.isApplicationResume = false;
				double num = this.waitFinishedTime - this.waitCurrentTime;
				this.waitCurrentTime = (double)Time.unscaledTime;
				this.waitFinishedTime = num + (double)Time.unscaledTime;
			}
			else if (waitStartUnscaledTime <= Time.unscaledTime)
			{
				this.waitCurrentTime = (double)Time.unscaledTime;
			}
			else
			{
				double num2 = this.waitFinishedTime - this.waitCurrentTime;
				this.waitCurrentTime = (double)Time.unscaledTime;
				this.waitFinishedTime = num2 + (double)Time.unscaledTime;
			}
		}
		base.ResumeScript();
		yield break;
	}

	public void OnApplicationPause()
	{
	}

	public void OnApplicationResume()
	{
		this.isApplicationResume = true;
	}

	private void WaitTouch()
	{
	}

	private void WaitBattleStart()
	{
		AppCoroutine.Start(this.controlToGame.WaitBattleStart(new Action(base.ResumeScript)), false);
	}

	private void WaitBattleActionSelect()
	{
		AppCoroutine.Start(this.controlToGame.WaitBattleActionSelect(new Action(base.ResumeScript)), false);
	}

	private void WaitBattleResultStart()
	{
		this.controlToGame.WaitBattleResultScreen(new Action(base.ResumeScript));
	}

	private void WaitBattleResultEnd()
	{
		AppCoroutine.Start(this.controlToGame.WaitBattleResultEffectEnd(new Action<CMD_BattleResult>(this.OnBattleResultEffectFinished)), false);
	}

	private void OnBattleResultEffectFinished(CMD_BattleResult uiBattleResult)
	{
		if (null != uiBattleResult)
		{
			uiBattleResult.SetCloseAction(null);
		}
		this.tutorialUI.MaskBlack.gameObject.SetActive(true);
		this.tutorialUI.MaskBlack.SetEnable(true, new Action(base.ResumeScript), true);
	}

	private void WaitFirstClear()
	{
		this.controlToGame.WaitFirstClearWindowOpen(new Action(this.OnFirstClearFinished));
	}

	private void OnFirstClearFinished()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(true);
		base.ResumeScript();
	}

	private void WaitFarmHarvest()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(false);
		this.controlToGame.WaitMeatHarvest(new Action(this.OnFinishedMeatHarvest), this.scriptEngine.GetMeatNum());
	}

	private void OnFinishedMeatHarvest()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(true);
		base.ResumeScript();
	}

	private void SetMaskControl()
	{
		AppCoroutine.Start(this.MaskFadeRoutine(), false);
	}

	private IEnumerator MaskFadeRoutine()
	{
		bool isTween = this.tutorialUI.MaskBlack.IsTweenActive;
		while (isTween)
		{
			yield return null;
			isTween = this.tutorialUI.MaskBlack.IsTweenActive;
		}
		this.tutorialUI.MaskBlack.SetEnable(this.scriptEngine.GetMaskEnableFlag(), null, true);
		yield break;
	}

	private void WaitFade()
	{
		ScriptCommandParams.FadeInfo fadeInfo = this.scriptEngine.GetFadeInfo();
		TutorialFade.FadeType type = (fadeInfo.type != 0) ? TutorialFade.FadeType.BLACK : TutorialFade.FadeType.WHITE;
		this.tutorialUI.Fade.StartFade(type, fadeInfo.enable, fadeInfo.time, new Action(base.ResumeScript));
	}

	private void WaitSave()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		TutorialStatusSave request = new TutorialStatusSave
		{
			SetSendData = delegate(TutorialStatusSaveQuery param)
			{
				param.statusId = int.Parse(this.scriptEngine.GetSaveStateID());
			}
		};
		AppCoroutine.Start(request.Run(delegate()
		{
			RestrictionInput.EndLoad();
			base.ResumeScript();
		}, null, null), false);
	}

	private void WaitScene()
	{
		ScriptCommandParams.SceneInfo sceneInfo = this.scriptEngine.GetSceneInfo();
		this.controlToGame.ChangeScene(sceneInfo.type, new Action(base.ResumeScript), false, new Action(this.SkipBattle));
	}

	private void SkipBattle()
	{
		TutorialCommandSkip tutorialCommandSkip = new TutorialCommandSkip(this.scriptEngine, this.tutorialUI, this.controlToGame);
		tutorialCommandSkip.SkipCommand(TutorialCommandSkip.SkipMode.BATTLE_SKIP);
		RestrictionInput.EndLoad();
		this.controlToGame.SetBgm("BGM/bgm_303/sound", true, 0.5f);
		this.tutorialUI.Fade.StartFade(TutorialFade.FadeType.BLACK, false, 0.5f, null);
	}

	private void WaitShake()
	{
		ScriptCommandParams.ShakeInfo shakeInfo = this.scriptEngine.GetShakeInfo();
		this.controlToGame.ShakeBackGround(shakeInfo.intensity, 0f, null);
		base.ResumeScript();
	}

	private void WaitShakeStop()
	{
		ScriptCommandParams.ShakeInfo shakeInfo = this.scriptEngine.GetShakeInfo();
		this.controlToGame.ShakeStopBackGround(shakeInfo.decay, new Action(base.ResumeScript));
	}

	private void WaitUISet()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(false);
		AppCoroutine.Start(this.controlToGame.OpenUI(this.scriptEngine.GetUiInfo().type, this.tutorialUI, new Action(this.OnClosedUI)), false);
	}

	private void OnClosedUI()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(true);
		base.ResumeScript();
	}

	private void WaitUIInduce()
	{
		ScriptCommandParams.UIInfo uiInfo = this.scriptEngine.GetUiInfo();
		this.controlToGame.SetInduceUI(uiInfo.type, uiInfo.enabled, uiInfo.arrowPosition, this.tutorialUI, delegate
		{
			this.controlToGame.ResetEmphasizeUI(this.tutorialUI, new Action(base.ResumeScript));
		});
	}

	private void WaitUIPush()
	{
		this.controlToGame.SetPushUI(this.scriptEngine.GetUiInfo().type, this.tutorialUI, delegate
		{
			this.controlToGame.ResetEmphasizeUI(this.tutorialUI, new Action(base.ResumeScript));
		});
	}

	private void WaitUIPop()
	{
		ScriptCommandParams.UIInfo uiInfo = this.scriptEngine.GetUiInfo();
		if (uiInfo.enabled)
		{
			this.controlToGame.SetPopUI(uiInfo.type, uiInfo.arrowPosition, this.tutorialUI, new Action(base.ResumeScript));
		}
		else
		{
			this.controlToGame.ResetEmphasizeUI(this.tutorialUI, new Action(base.ResumeScript));
		}
	}

	private void WaitUIClose()
	{
		this.controlToGame.CloseAllUI(new Action(base.ResumeScript));
	}

	private void WaitFarmCamera()
	{
		ScriptCommandParams.FarmCameraMoveInfo farmCameraMoveInfo = this.scriptEngine.GetFarmCameraMoveInfo();
		AppCoroutine.Start(this.controlToGame.MoveFarmCamera(farmCameraMoveInfo.posGridX, farmCameraMoveInfo.posGridY, farmCameraMoveInfo.time, new Action(base.ResumeScript)), false);
	}

	private void WaitFarmSelect()
	{
		ScriptCommandParams.SelectFacilityInfo selectFacilityInfo = this.scriptEngine.GetSelectFacilityInfo();
		if (selectFacilityInfo.selected)
		{
			this.controlToGame.SelectFacility(selectFacilityInfo.id);
		}
		else
		{
			this.controlToGame.ReleaseFacility();
		}
		base.ResumeScript();
	}

	private void WaitFarmBuild()
	{
		ScriptCommandParams.BuildFacilityInfo buildFacilityInfo = this.scriptEngine.GetBuildFacilityInfo();
		int id = buildFacilityInfo.id;
		int posGridX = buildFacilityInfo.posGridX;
		int posGridY = buildFacilityInfo.posGridY;
		int time = buildFacilityInfo.buildTime;
		if (!buildFacilityInfo.buildComplete)
		{
			IEnumerator routine = this.controlToGame.BuildFacility(id, posGridX, posGridY, false, delegate(int userFacilityID)
			{
				this.controlToGame.SetFacilityBuildDummyTime(userFacilityID, time);
				this.ResumeScript();
			});
			AppCoroutine.Start(routine, false);
		}
		else
		{
			this.controlToGame.FacilityBuildComplete(id);
			base.ResumeScript();
		}
	}

	private void WaitBuildCompleteTap()
	{
		ScriptCommandParams.BuildFacilityInfo buildFacilityInfo = this.scriptEngine.GetBuildFacilityInfo();
		this.tutorialUI.MaskBlack.gameObject.SetActive(false);
		this.controlToGame.WaitBuildCompleteTap(new Action(this.OnTouchFacilityBuild), new Action(base.ResumeScript), buildFacilityInfo.id);
	}

	private void OnTouchFacilityBuild()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(true);
	}

	private void WaitFarmTarget()
	{
		ScriptCommandParams.TargetFacilityInfo targetFacilityInfo = this.scriptEngine.GetTargetFacilityInfo();
		IEnumerator routine = this.controlToGame.TargetFacility(targetFacilityInfo.id, targetFacilityInfo.popEnable, new Action(base.ResumeScript), targetFacilityInfo.adjustY);
		AppCoroutine.Start(routine, false);
	}

	private void WaitOpenTrainingMenu()
	{
		AppCoroutine.Start(this.controlToGame.WaitOpenTrainingMenuUI(new Action(base.ResumeScript)), false);
	}

	private void WaitOpenMealDigimonSelect()
	{
		AppCoroutine.Start(this.controlToGame.WaitOpenMealSelectDigimonUI(new Action(base.ResumeScript)), false);
	}

	private void WaitOpenMealGive()
	{
		AppCoroutine.Start(this.controlToGame.WaitOpenMealUI(delegate
		{
			this.controlToGame.SetMealUI_NotServerRequest();
			base.ResumeScript();
		}), false);
	}

	private void WaitOpenGashaTop()
	{
		AppCoroutine.Start(this.controlToGame.WaitOpenGashaUI(delegate
		{
			base.ResumeScript();
		}), false);
	}

	private void WaitOpenDigimonDetail()
	{
		int waitOpenDetailUI = this.scriptEngine.GetWaitOpenDetailUI();
		if (waitOpenDetailUI != 0)
		{
			if (waitOpenDetailUI == 1)
			{
				this.controlToGame.WaitOpenedForDigiGardenCharacterDetailWindow(new Action(base.ResumeScript));
			}
		}
		else
		{
			this.controlToGame.RemoveActionRegistFromOutside("GashaConfirmDialog");
		}
	}

	private void WaitMealLevelUp()
	{
		this.controlToGame.WaitLevelUpForMeal(new Action(base.ResumeScript));
	}

	private void WaitQuestSet()
	{
		this.controlToGame.SetTutorialDungeonId();
		base.ResumeScript();
	}

	private void SetBattlePause()
	{
		ScriptCommandParams.BattleInfo battleInfo = this.scriptEngine.GetBattleInfo();
		this.controlToGame.BattlePause(battleInfo.pause);
	}

	private void SetBattleManual()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(false);
		this.controlToGame.StartManualBattle();
	}

	private void SetBattleEnd()
	{
		this.controlToGame.DeleteBattleScreen(new Action(base.ResumeScript));
	}

	private void SetMovie()
	{
		this.controlToGame.PlayMovie(new Action(base.ResumeScript));
	}

	private void SetSoundVolume()
	{
		ScriptCommandParams.SoundVolumeInfo soundVolumeInfo = this.scriptEngine.GetSoundVolumeInfo();
		this.controlToGame.StartSoundVolumeFade(soundVolumeInfo.enable, soundVolumeInfo.time);
	}

	private void SetBgm()
	{
		ScriptCommandParams.BgmInfo bgmInfo = this.scriptEngine.GetBgmInfo();
		this.controlToGame.SetBgm(bgmInfo.fileName, bgmInfo.play, bgmInfo.fadeTime);
	}

	private void SetSe()
	{
		ScriptCommandParams.SeInfo seInfo = this.scriptEngine.GetSeInfo();
		this.controlToGame.SetSe(seInfo.fileName, seInfo.play, seInfo.fadeTime, seInfo.loop, seInfo.pitch);
	}

	private void WaitDigimon()
	{
		ScriptCommandParams.DigimonInfo digimonInfo = this.scriptEngine.GetDigimonInfo();
		AppCoroutine.Start(this.controlToGame.DrawDigimon(digimonInfo.monsterGroupID, digimonInfo.scale, digimonInfo.adjustPosition, new Action(base.ResumeScript)), false);
	}

	private void SetDigimonDelete()
	{
		this.controlToGame.DeleteDigimon();
	}

	private void WaitScreenEffect()
	{
		ScriptCommandParams.ScreenEffectInfo screenEffectInfo = this.scriptEngine.GetScreenEffectInfo();
		if (screenEffectInfo.start)
		{
			this.controlToGame.StartEffect(screenEffectInfo.type, new Action(base.ResumeScript));
		}
		else
		{
			this.controlToGame.StopEffect(screenEffectInfo.type);
			base.ResumeScript();
		}
	}

	private void SetBattleAutoOn()
	{
		this.controlToGame.SetAutoBattleFlag();
	}

	private void SetDigimonExp()
	{
		ScriptCommandParams.DigimonExpInfo didimonExpInfo = this.scriptEngine.GetDidimonExpInfo();
		this.controlToGame.SetDigimonExp(didimonExpInfo.index, didimonExpInfo.level, didimonExpInfo.exp);
	}

	private void WaitFirstTutorialEnd()
	{
		this.tutorialUI.MaskBlack.gameObject.SetActive(false);
		this.controlToGame.SaveFinishFirstTutorial(new Action(base.ResumeScript));
	}

	private void WaitDownloadCheck()
	{
		bool flag = false;
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (null != tutorialObserver)
		{
			string assetBundleLevel = tutorialObserver.GetAssetBundleLevel();
			if (!string.IsNullOrEmpty(assetBundleLevel))
			{
				AssetDataMng assetDataMng = AssetDataMng.Instance();
				if (this.controlToGame.IsBackgroundDownload || (assetDataMng.IsInitializedAssetBundle() && 0 < assetDataMng.GetDownloadAssetBundleCount(assetBundleLevel)))
				{
					this.tutorialUI.Fade.StartFade(TutorialFade.FadeType.BLACK, true, 0.5f, new Action(this.SetDownloadScene));
					flag = true;
				}
			}
		}
		if (!flag)
		{
			base.ResumeScript();
		}
	}

	private void SetDownloadScene()
	{
		this.controlToGame.ChangeScene(this.controlToGame.GetBlackSceneID(), new Action(base.ResumeScript), false, null);
	}

	private void WaitDownload()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (!this.controlToGame.IsBackgroundDownload)
		{
			this.WaitDefaultDownload(tutorialObserver);
		}
		else
		{
			this.WaitBackgroundDownload(tutorialObserver);
		}
	}

	private void WaitDefaultDownload(TutorialObserver tutorialObserver)
	{
		string level = tutorialObserver.GetAssetBundleLevel();
		if (0 < AssetDataMng.Instance().GetDownloadAssetBundleCount(level))
		{
			this.tutorialUI.Fade.StartFade(TutorialFade.FadeType.BLACK, false, 0.5f, delegate
			{
				this.controlToGame.StartDownload(level, new Action(this.ResumeScript));
			});
		}
		else
		{
			base.ResumeScript();
		}
		tutorialObserver.DownloaddAssetBundleLevelIndex++;
	}

	private void WaitBackgroundDownload(TutorialObserver tutorialObserver)
	{
		if (AssetBundleMng.Instance().IsStopDownload())
		{
			AssetBundleMng.Instance().RestartDownload();
		}
		if (AssetDataMng.Instance().IsAssetBundleDownloading())
		{
			this.tutorialUI.Fade.StartFade(TutorialFade.FadeType.BLACK, false, 0.5f, delegate
			{
				this.controlToGame.WaitBackgroundDownload(new Action(base.ResumeScript));
			});
		}
		else
		{
			base.ResumeScript();
		}
		tutorialObserver.DownloaddAssetBundleLevelIndex++;
	}

	private void StartBackgroundDownload()
	{
		if (this.controlToGame.IsBackgroundDownload)
		{
			base.ResumeScript();
			return;
		}
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		string assetBundleLevel = tutorialObserver.GetAssetBundleLevel();
		if (0 < AssetDataMng.Instance().GetDownloadAssetBundleCount(assetBundleLevel))
		{
			this.controlToGame.StartBackgroundDownload(assetBundleLevel, null, 1);
		}
		base.ResumeScript();
	}

	private void WaitDigiviceOpen()
	{
		GUIFace.EventShowBtnDigivice = new Action(base.ResumeScript);
	}

	private void StartStandardDownload()
	{
		if (!this.controlToGame.IsStandardDownload)
		{
			this.controlToGame.StartStandardDownload();
		}
		base.ResumeScript();
	}

	private void StopDownload()
	{
		AssetBundleMng.Instance().StopDownload();
		base.ResumeScript();
	}

	private void RestartDownload()
	{
		AssetBundleMng.Instance().RestartDownload();
		base.ResumeScript();
	}

	private void SkipStart()
	{
		this.tutorialUI.SetSkipButton(true, delegate
		{
			TutorialCommandSkip tutorialCommandSkip = new TutorialCommandSkip(this.scriptEngine, this.tutorialUI, this.controlToGame);
			tutorialCommandSkip.SkipCommand(TutorialCommandSkip.SkipMode.NORMAL_SKIP);
		});
	}

	private void SkipEnd()
	{
		this.tutorialUI.SetSkipButton(false, null);
	}

	private void SetNonFrameText()
	{
		this.tutorialUI.NonFrameText.Open(NGUIText.Alignment.Center);
	}

	private void SetNonFrameTextEnd()
	{
		this.tutorialUI.NonFrameText.Close();
	}

	private void WaitDigiGardenOpen()
	{
		AppCoroutine.Start(this.controlToGame.WaitOpenDigiGarden(new Action(base.ResumeScript)), false);
	}

	private void WaitDigiGardenChangeList()
	{
		AppCoroutine.Start(this.controlToGame.WaitChangeDigiGardenList(new Action(base.ResumeScript)), false);
	}

	private void WaitDigiGardenChangeSetList()
	{
		this.controlToGame.WaitChangeDigiGardenSetList(new Action(base.ResumeScript));
	}

	private void WaitColosseumOpen()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (instance != null)
		{
			instance.StartColosseumOpenAnimation(new Action(base.ResumeScript));
		}
	}

	private void WaitFacilityShopOpen()
	{
		AppCoroutine.Start(this.controlToGame.WaitOpenFacilityShop(new Action(base.ResumeScript)), false);
	}

	private void WaitMissionOpen()
	{
		AppCoroutine.Start(this.controlToGame.WaitOpenMission(new Action(base.ResumeScript)), false);
	}

	private void WaitDigimonChange()
	{
		this.controlToGame.LocalDigimonEvolution();
		base.ResumeScript();
	}

	private void Eof()
	{
	}
}
