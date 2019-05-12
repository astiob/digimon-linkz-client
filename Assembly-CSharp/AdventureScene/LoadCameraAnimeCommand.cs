using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class LoadCameraAnimeCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_load_camera_anm";

		private string animatorFileName;

		public override string GetCommandName()
		{
			return "#adv_load_camera_anm";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.animatorFileName = commandParams[1];
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
			bool result = false;
			string path = "AdventureScene/Animation/Camera/" + this.animatorFileName;
			RuntimeAnimatorController runtimeAnimatorController = AssetDataMng.Instance().LoadObject(path, null, true) as RuntimeAnimatorController;
			if (null != runtimeAnimatorController)
			{
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.SetAnimator(runtimeAnimatorController);
				result = true;
				base.ResumeScriptEngine();
			}
			return result;
		}
	}
}
