using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class LoadBattleCameraCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_load_battle_camera";

		public LoadBattleCameraCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_load_battle_camera";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
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
			BattleStateManager current = BattleStateManager.current;
			bool result;
			if (current != null)
			{
				result = true;
				Camera camera3D = current.hierarchyData.cameraObject.camera3D;
				Camera camera3D2 = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D;
				camera3D2.transform.position = camera3D.transform.position;
				camera3D2.transform.rotation = camera3D.transform.rotation;
				camera3D2.fieldOfView = camera3D.fieldOfView;
				base.ResumeScriptEngine();
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
