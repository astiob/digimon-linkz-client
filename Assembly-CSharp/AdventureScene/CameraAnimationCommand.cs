using System;

namespace AdventureScene
{
	public sealed class CameraAnimationCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_camera_anm";

		private bool isWaitFlag;

		private string animatorStateName;

		private string finishAnimatorStateName;

		public override string GetCommandName()
		{
			return "#adv_camera_anm";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.isWaitFlag = ("wait" == commandParams[1]);
				this.animatorStateName = commandParams[2];
				if (3 < commandParams.Length)
				{
					this.finishAnimatorStateName = commandParams[3];
				}
				else
				{
					this.finishAnimatorStateName = this.animatorStateName;
				}
				base.SetWaitScriptEngine(true);
				result = true;
			}
			catch
			{
				base.OnErrorGetParameter();
			}
			return result;
		}

		public override bool RunScriptCommand()
		{
			ClassSingleton<AdventureSceneData>.Instance.adventureCamera.StartAnimation(this.animatorStateName, this.finishAnimatorStateName);
			base.SetContinueAnalyzeForAnimationWaitTime(this.isWaitFlag);
			return true;
		}

		public override bool UpdateCommand()
		{
			if (this.isWaitFlag && !ClassSingleton<AdventureSceneData>.Instance.adventureCamera.UpdateAnimation())
			{
				base.ResumeScriptEngine();
			}
			return true;
		}
	}
}
