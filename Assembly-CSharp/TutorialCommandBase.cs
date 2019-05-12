using System;
using System.Collections.Generic;
using TS;

public abstract class TutorialCommandBase
{
	protected Dictionary<ScriptEngine.Status, Action> actionList = new Dictionary<ScriptEngine.Status, Action>();

	protected ScriptEngine scriptEngine;

	protected TutorialUI tutorialUI;

	protected TutorialControlToGame controlToGame;

	private int commandAddress;

	public TutorialCommandBase(ScriptEngine scriptEngine, TutorialUI tutorialUI, TutorialControlToGame controlToGame)
	{
		this.scriptEngine = scriptEngine;
		this.tutorialUI = tutorialUI;
		this.controlToGame = controlToGame;
	}

	public void ActionScriptCommand(ScriptEngine.Status engineStatus, int commandAddress)
	{
		this.commandAddress = commandAddress;
		if (this.actionList.ContainsKey(engineStatus))
		{
			this.actionList[engineStatus]();
		}
	}

	protected void ResumeScript()
	{
		this.scriptEngine.Resume(this.commandAddress);
	}

	protected void SetCharaPosition()
	{
		ScriptCommandParams.CharaInfo charaInfo = this.scriptEngine.GetCharaInfo();
		this.tutorialUI.Thumbnail.SetMonitorPosition(charaInfo.yFromCenter);
	}

	protected void SetMeatNum()
	{
		this.controlToGame.SetPlayerMeatCount(this.scriptEngine.GetMeatNum());
	}

	protected void SetDigiStoneNum()
	{
		this.controlToGame.SetPlayerDigiStoneCount(this.scriptEngine.GetDigiStoneNum());
	}

	protected void SetLinkPointNum()
	{
		this.controlToGame.SetPlayerLinkPointCount(this.scriptEngine.GetLinkPointNum());
	}
}
