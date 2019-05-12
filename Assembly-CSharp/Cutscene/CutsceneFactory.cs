using System;
using UnityEngine;

namespace Cutscene
{
	public static class CutsceneFactory
	{
		private static CutsceneBase GetCutsceneComponent(GameObject cutsceneGameObject, string path)
		{
			CutsceneBase result = null;
			switch (path)
			{
			case "Cutscenes/VersionUp":
				result = cutsceneGameObject.GetComponent<VersionUPController>();
				break;
			case "Cutscenes/EvolutionUltimate":
				result = cutsceneGameObject.GetComponent<EvolutionUltimateController>();
				break;
			case "Cutscenes/Evolution":
				result = cutsceneGameObject.GetComponent<EvolutionController>();
				break;
			case "Cutscenes/ModeChange":
				result = cutsceneGameObject.GetComponent<ModeChangeController>();
				break;
			case "Cutscenes/Jogress":
				result = cutsceneGameObject.GetComponent<JogressController>();
				break;
			case "Cutscenes/Gasha":
				result = cutsceneGameObject.GetComponent<GashaController>();
				break;
			case "Cutscenes/Inheritance":
				result = cutsceneGameObject.GetComponent<InharitanceController>();
				break;
			case "Cutscenes/MedalInherit":
				result = cutsceneGameObject.GetComponent<MedalInheritController>();
				break;
			case "Cutscenes/Fusion":
				result = cutsceneGameObject.GetComponent<FusionController>();
				break;
			case "Cutscenes/Training":
				result = cutsceneGameObject.GetComponent<TrainingController>();
				break;
			case "Cutscenes/Awakening":
				result = cutsceneGameObject.GetComponent<AwakeningController>();
				break;
			case "Cutscenes/AssetBundle/ChipGasha/chip_gacha":
				result = cutsceneGameObject.GetComponent<ChipGashaController>();
				break;
			case "Cutscenes/ticketGacha":
				result = cutsceneGameObject.GetComponent<TicketGashaController>();
				break;
			case "Cutscenes/Tutorial":
				result = cutsceneGameObject.GetComponent<TutorialController>();
				break;
			}
			return result;
		}

		public static CutsceneBase Create(CutsceneDataBase cutsceneData)
		{
			CutsceneBase cutsceneBase = null;
			if (cutsceneData.path != "Cutscenes/dummy_scene")
			{
				GameObject original = AssetDataMng.Instance().LoadObject(cutsceneData.path, null, true) as GameObject;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
				gameObject.transform.localPosition = new Vector3(4000f, 4000f, 0f);
				if (null != gameObject)
				{
					cutsceneBase = CutsceneFactory.GetCutsceneComponent(gameObject, cutsceneData.path);
					if (null != cutsceneBase)
					{
						cutsceneBase.Initialize();
						cutsceneBase.SetData(cutsceneData);
					}
				}
			}
			return cutsceneBase;
		}
	}
}
