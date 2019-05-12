using System;

namespace AdventureScene
{
	public sealed class CameraAnimationStopCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_camera_anm_stop";

		public CameraAnimationStopCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_camera_anm_stop";
		}

		public override bool GetParameter(string[] commandParams)
		{
			base.SetWaitScriptEngine(true);
			return true;
		}

		public override bool RunScriptCommand()
		{
			ClassSingleton<AdventureSceneData>.Instance.adventureCamera.StopAnimation();
			base.ResumeScriptEngine();
			return true;
		}
	}
}
