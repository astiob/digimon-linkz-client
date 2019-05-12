using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class LoadStageCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_load_stage";

		private string stageFileName;

		public LoadStageCommand()
		{
			this.stageFileName = string.Empty;
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_load_stage";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.stageFileName = commandParams[1];
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
			bool result = true;
			string stagePrefabPathByAttackEffectId = CommonResourcesDataMng.Instance().GetStagePrefabPathByAttackEffectId(this.stageFileName);
			GameObject original = AssetDataMng.Instance().LoadObject(stagePrefabPathByAttackEffectId, null, true) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			ClassSingleton<AdventureSceneData>.Instance.stage = gameObject;
			gameObject.name = "Stage";
			gameObject.transform.parent = ClassSingleton<AdventureSceneData>.Instance.scriptObjectRoot.transform;
			LightColorChanger sunLightColorChanger = ClassSingleton<AdventureSceneData>.Instance.adventureLight.gameObject.AddComponent<LightColorChanger>();
			BattleCameraObject battleCameraObject = gameObject.AddComponent<BattleCameraObject>();
			battleCameraObject.sunLight = ClassSingleton<AdventureSceneData>.Instance.adventureLight;
			battleCameraObject.camera3D = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D;
			battleCameraObject.sunLightColorChanger = sunLightColorChanger;
			StageParams component = gameObject.GetComponent<StageParams>();
			component.SetHierarchyEnviroments(battleCameraObject);
			base.ResumeScriptEngine();
			return result;
		}
	}
}
