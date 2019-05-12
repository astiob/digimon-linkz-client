using System;

namespace AdventureScene
{
	public sealed class LoadBattleStageCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_load_battle_stage";

		public LoadBattleStageCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_load_battle_stage";
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
				StageParams stageParams = current.hierarchyData.stageParams;
				ClassSingleton<AdventureSceneData>.Instance.stage = stageParams.gameObject;
				LightColorChanger lightColorChanger = ClassSingleton<AdventureSceneData>.Instance.adventureLight.GetComponent<LightColorChanger>();
				if (lightColorChanger == null)
				{
					lightColorChanger = ClassSingleton<AdventureSceneData>.Instance.adventureLight.gameObject.AddComponent<LightColorChanger>();
				}
				BattleCameraObject battleCameraObject = stageParams.gameObject.AddComponent<BattleCameraObject>();
				battleCameraObject.sunLight = ClassSingleton<AdventureSceneData>.Instance.adventureLight;
				battleCameraObject.camera3D = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D;
				battleCameraObject.sunLightColorChanger = lightColorChanger;
				stageParams.SetHierarchyEnviroments(battleCameraObject);
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
