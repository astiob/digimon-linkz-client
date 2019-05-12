using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TS;

public class TutorialCommandSkip : TutorialCommandBase
{
	private string charaFaceId;

	private TutorialCommandSkip.ScreenShakeActionType screenShake;

	private TutorialCommandSkip.ScreenEffectSirenActionType screenEffect;

	private TutorialCommandSkip.SkipMode skipMode;

	private List<ScriptCommandParams.SeInfo> seLoopPlayList = new List<ScriptCommandParams.SeInfo>();

	public TutorialCommandSkip(ScriptEngine scriptEngine, TutorialUI tutorialUI, TutorialControlToGame controlToGame) : base(scriptEngine, tutorialUI, controlToGame)
	{
		this.SuspendCommand();
		this.actionList.Add(ScriptEngine.Status.WAIT_WINDOW, new Action(this.SkipOpenWindow));
		this.actionList.Add(ScriptEngine.Status.WAIT_WINDOW_DELETE, new Action(this.SkipCloseWindow));
		this.actionList.Add(ScriptEngine.Status.WAIT_MESSAGE_DISPLAY, new Action(this.SkipDisplayMessage));
		this.actionList.Add(ScriptEngine.Status.WAIT_CHARA, new Action(this.SkipDisplayChara));
		this.actionList.Add(ScriptEngine.Status.SET_CHARA_POS, new Action(base.SetCharaPosition));
		this.actionList.Add(ScriptEngine.Status.SET_CHARA_FACE, new Action(this.SkipChangeCharaFace));
		this.actionList.Add(ScriptEngine.Status.WAIT_CHARA_DELETE, new Action(this.SkipInvisibleChara));
		this.actionList.Add(ScriptEngine.Status.SET_MASK_CONTROL, new Action(this.SkipMaskTween));
		this.actionList.Add(ScriptEngine.Status.WAIT_FADE, new Action(this.SkipFadeTween));
		this.actionList.Add(ScriptEngine.Status.WAIT_SAVE, new Action(this.SkipTutorialStateSave));
		this.actionList.Add(ScriptEngine.Status.WAIT_SHAKE, new Action(this.SkipShakeScreen));
		this.actionList.Add(ScriptEngine.Status.WAIT_SHAKE_STOP, new Action(this.SkipShakeScreenStop));
		this.actionList.Add(ScriptEngine.Status.WAIT_UI_POP, new Action(this.SkipUIPop));
		this.actionList.Add(ScriptEngine.Status.WAIT_FARM_TARGET, new Action(this.SkipFarmTarget));
		this.actionList.Add(ScriptEngine.Status.WAIT_QUEST_SET, new Action(this.SkipQuestSet));
		this.actionList.Add(ScriptEngine.Status.SET_BATTLE_PAUSE, new Action(this.SetBattlePause));
		this.actionList.Add(ScriptEngine.Status.SET_BATTLE_MANUAL, new Action(this.SetBattleManual));
		this.actionList.Add(ScriptEngine.Status.SET_SOUND_VOLUME, new Action(this.SetSoundVolume));
		this.actionList.Add(ScriptEngine.Status.SET_BGM, new Action(this.SetBgm));
		this.actionList.Add(ScriptEngine.Status.SET_SE, new Action(this.SetSe));
		this.actionList.Add(ScriptEngine.Status.WAIT_SCREEN_EFFECT, new Action(this.SkipScreenEffect));
		this.actionList.Add(ScriptEngine.Status.SET_BATTLE_AUTO_ON, new Action(this.SetBattleAutoOn));
		this.actionList.Add(ScriptEngine.Status.SET_MEAT_NUM, new Action(base.SetMeatNum));
		this.actionList.Add(ScriptEngine.Status.SET_DIGI_STONE_NUM, new Action(base.SetDigiStoneNum));
		this.actionList.Add(ScriptEngine.Status.SET_DIGIMON_EXP, new Action(this.SetDigimonExp));
		this.actionList.Add(ScriptEngine.Status.SKIP_END, new Action(this.SkipEnd));
		this.actionList.Add(ScriptEngine.Status.SET_NON_FRAME_TEXT, new Action(this.SetNonFrameText));
		this.actionList.Add(ScriptEngine.Status.SET_NON_FRAME_TEXT_END, new Action(this.SetNonFrameTextEnd));
		this.actionList.Add(ScriptEngine.Status.BATTLE_SKIP_END, new Action(this.BattleSkipEnd));
	}

	private void SuspendCommand()
	{
		if (!this.scriptEngine.GetTextInfo().isWindowText)
		{
			this.tutorialUI.NonFrameText.StartInvisible(0f, null);
		}
		if (this.tutorialUI.MessageWindow.IsOpened)
		{
			this.tutorialUI.MessageWindow.SkipWindowAnimation();
			ScriptCommandParams.TextInfo textInfo = this.scriptEngine.GetTextInfo();
			TutorialMaster.NaviMessage naviMessage = MasterDataMng.Instance().Tutorial.GetNaviMessage(textInfo.displayText);
			this.tutorialUI.MessageWindow.SkipDisplayMessage(naviMessage.message);
		}
		if (this.tutorialUI.Thumbnail.IsOpened)
		{
			TutorialThumbnail.ThumbnailType type = (this.scriptEngine.GetCharaInfo().type != 0) ? TutorialThumbnail.ThumbnailType.MONITOR : TutorialThumbnail.ThumbnailType.BODY;
			this.tutorialUI.Thumbnail.SkipWindowAnimation(type);
		}
		this.controlToGame.SuspendShakeBackGround();
	}

	public void SkipCommand(TutorialCommandSkip.SkipMode mode)
	{
		this.skipMode = mode;
		this.scriptEngine.StartSkip(new Action<ScriptEngine.Status, int>(base.ActionScriptCommand));
		if (!string.IsNullOrEmpty(this.charaFaceId))
		{
			this.tutorialUI.Thumbnail.SetFace(this.charaFaceId);
		}
		if (this.screenShake == TutorialCommandSkip.ScreenShakeActionType.START)
		{
			ScriptCommandParams.ShakeInfo shakeInfo = this.scriptEngine.GetShakeInfo();
			this.controlToGame.ShakeBackGround(shakeInfo.intensity, 0f, null);
		}
		else if (this.screenShake == TutorialCommandSkip.ScreenShakeActionType.STOP)
		{
			this.controlToGame.SuspendShakeBackGround();
		}
		if (this.screenEffect == TutorialCommandSkip.ScreenEffectSirenActionType.START)
		{
			ScriptCommandParams.ScreenEffectInfo screenEffectInfo = this.scriptEngine.GetScreenEffectInfo();
			this.controlToGame.StartEffect(screenEffectInfo.type, null);
		}
		else if (this.screenEffect == TutorialCommandSkip.ScreenEffectSirenActionType.STOP)
		{
			ScriptCommandParams.ScreenEffectInfo screenEffectInfo2 = this.scriptEngine.GetScreenEffectInfo();
			this.controlToGame.StopEffect(screenEffectInfo2.type);
		}
		if (0 < this.seLoopPlayList.Count)
		{
			for (int i = 0; i < this.seLoopPlayList.Count; i++)
			{
				ScriptCommandParams.SeInfo seInfo = this.seLoopPlayList[i];
				this.controlToGame.SetSe(seInfo.fileName, seInfo.play, 0f, true, seInfo.pitch);
			}
		}
		base.ResumeScript();
	}

	private void SkipOpenWindow()
	{
		ScriptCommandParams.WindowInfo windowInfo = this.scriptEngine.GetWindowInfo();
		this.tutorialUI.MessageWindow.DisplayWindow(windowInfo.xFromCenter, windowInfo.yFromCenter, null, false);
	}

	private void SkipCloseWindow()
	{
		this.tutorialUI.MessageWindow.DeleteWindow(null, false);
	}

	private void SkipDisplayMessage()
	{
		ScriptCommandParams.TextInfo textInfo = this.scriptEngine.GetTextInfo();
		TutorialMaster.NaviMessage naviMessage = MasterDataMng.Instance().Tutorial.GetNaviMessage(textInfo.displayText);
		if (textInfo.isWindowText)
		{
			if ("0" != naviMessage.faceId)
			{
				this.charaFaceId = naviMessage.faceId;
			}
			this.tutorialUI.MessageWindow.SkipDisplayMessage(naviMessage.message);
		}
		else
		{
			this.tutorialUI.NonFrameText.SetText(naviMessage.message);
		}
	}

	private void SkipDisplayChara()
	{
		ScriptCommandParams.CharaInfo charaInfo = this.scriptEngine.GetCharaInfo();
		TutorialThumbnail.ThumbnailType thumbnailType = (charaInfo.type != 0) ? TutorialThumbnail.ThumbnailType.MONITOR : TutorialThumbnail.ThumbnailType.BODY;
		this.tutorialUI.Thumbnail.Display(thumbnailType, null, false);
		this.charaFaceId = charaInfo.faceId;
		if (thumbnailType == TutorialThumbnail.ThumbnailType.MONITOR)
		{
			this.tutorialUI.Thumbnail.SetMonitorPosition(charaInfo.yFromCenter);
		}
	}

	private void SkipChangeCharaFace()
	{
		this.charaFaceId = this.scriptEngine.GetCharaInfo().faceId;
	}

	private void SkipInvisibleChara()
	{
		TutorialThumbnail.ThumbnailType type = (this.scriptEngine.GetCharaInfo().type != 0) ? TutorialThumbnail.ThumbnailType.MONITOR : TutorialThumbnail.ThumbnailType.BODY;
		this.tutorialUI.Thumbnail.Delete(type, null, false);
		this.charaFaceId = string.Empty;
	}

	private void SkipMaskTween()
	{
		this.tutorialUI.MaskBlack.SetEnable(this.scriptEngine.GetMaskEnableFlag(), null, false);
	}

	private void SkipFadeTween()
	{
		ScriptCommandParams.FadeInfo fadeInfo = this.scriptEngine.GetFadeInfo();
		TutorialFade.FadeType type = (fadeInfo.type != 0) ? TutorialFade.FadeType.BLACK : TutorialFade.FadeType.WHITE;
		if (!fadeInfo.enable && this.skipMode == TutorialCommandSkip.SkipMode.NORMAL_SKIP)
		{
			this.tutorialUI.Fade.StartFade(type, false, 0f, null);
		}
	}

	private void SkipTutorialStateSave()
	{
	}

	private void SkipShakeScreen()
	{
		this.screenShake = TutorialCommandSkip.ScreenShakeActionType.START;
	}

	private void SkipShakeScreenStop()
	{
		this.screenShake = TutorialCommandSkip.ScreenShakeActionType.STOP;
	}

	private void SkipUIPop()
	{
		if (this.skipMode == TutorialCommandSkip.SkipMode.NORMAL_SKIP)
		{
			ScriptCommandParams.UIInfo uiInfo = this.scriptEngine.GetUiInfo();
			if (uiInfo.enabled)
			{
				this.controlToGame.SetPopUI(uiInfo.type, uiInfo.arrowPosition, this.tutorialUI, null);
			}
			else
			{
				this.controlToGame.ResetEmphasizeUI(this.tutorialUI, null);
			}
		}
	}

	private void SkipFarmTarget()
	{
		ScriptCommandParams.TargetFacilityInfo targetFacilityInfo = this.scriptEngine.GetTargetFacilityInfo();
		IEnumerator routine = this.controlToGame.TargetFacility(targetFacilityInfo.id, targetFacilityInfo.popEnable, null, targetFacilityInfo.adjustY);
		AppCoroutine.Start(routine, false);
	}

	private void SkipQuestSet()
	{
		this.controlToGame.SetTutorialDungeonId();
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
		ScriptCommandParams.SeInfo info = this.scriptEngine.GetSeInfo();
		if (info.loop)
		{
			if (!this.seLoopPlayList.Any((ScriptCommandParams.SeInfo x) => x.fileName == info.fileName))
			{
				this.seLoopPlayList.Add(info);
			}
			else
			{
				this.seLoopPlayList = this.seLoopPlayList.ConvertAll<ScriptCommandParams.SeInfo>(delegate(ScriptCommandParams.SeInfo x)
				{
					if (x.fileName == info.fileName)
					{
						x.play = info.play;
					}
					return x;
				});
			}
		}
	}

	private void SkipScreenEffect()
	{
		if (this.scriptEngine.GetScreenEffectInfo().start)
		{
			this.screenEffect = TutorialCommandSkip.ScreenEffectSirenActionType.START;
		}
		else
		{
			this.screenEffect = TutorialCommandSkip.ScreenEffectSirenActionType.STOP;
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

	private void SkipEnd()
	{
		if (this.skipMode == TutorialCommandSkip.SkipMode.NORMAL_SKIP)
		{
			this.skipMode = TutorialCommandSkip.SkipMode.NONE;
			this.scriptEngine.StopSkip();
		}
	}

	private void BattleSkipEnd()
	{
		if (this.skipMode == TutorialCommandSkip.SkipMode.BATTLE_SKIP)
		{
			this.skipMode = TutorialCommandSkip.SkipMode.NONE;
			this.scriptEngine.StopSkip();
		}
	}

	private void SetNonFrameText()
	{
		this.tutorialUI.NonFrameText.Open(NGUIText.Alignment.Center);
	}

	private void SetNonFrameTextEnd()
	{
		this.tutorialUI.NonFrameText.Close();
	}

	private enum ScreenShakeActionType
	{
		NONE,
		START,
		STOP
	}

	private enum ScreenEffectSirenActionType
	{
		NONE,
		START,
		STOP
	}

	public enum SkipMode
	{
		NONE,
		NORMAL_SKIP,
		BATTLE_SKIP
	}
}
