using System;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneMain : MonoBehaviour
{
	private static string cs_path_bk;

	private static Action<int> cs_startSceneCallBack;

	private static Action<int> cs_endSceneCallBack;

	private static List<int> cs_umidList_bk0;

	private static List<int> cs_umidList_bk1;

	private static int cs_fusionNum_bk;

	private static int cs_rareNum_bk;

	private static GameObject goCutScene;

	private static EvolutionUltimateController csEvoUltimateScene;

	private static EvolutionController csEvoScene;

	private static ModeChangeController csModeChangeScene;

	private static JogressController csJogressScene;

	private static GashaController csGachaScene;

	private static InharitanceController csInharitanceScene;

	private static MedalInheritController csMedalInheritController;

	private static FusionController csFusionScene;

	private static TrainingController csTrainingScene;

	private static AwakeningController csAwakeningScene;

	private static ChipGashaController csChipGashaController;

	private static TicketGashaController csTicketGashaController;

	private static int mons_beforelevel;

	private static GameObject goGUI_CAM;

	public static void SetChipGashaTex(UITexture tex)
	{
		CutSceneMain.csChipGashaController.SetChipGashaTex(tex);
	}

	public static void SetTicketGashaTex(UITexture tex)
	{
		CutSceneMain.csTicketGashaController.SetTicketGashaTex(tex);
	}

	public static int Mons_BeforeLevel
	{
		get
		{
			return CutSceneMain.mons_beforelevel;
		}
		set
		{
			CutSceneMain.mons_beforelevel = value;
		}
	}

	public static GameWebAPI.RespDataGA_ExecChip.UserAssetList[] UserAssetList { get; set; }

	public static GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList[] UserTicketList { get; set; }

	public static void FadeReqCutScene(string path, Action<int> startSceneCallBack, Action<int> endSceneCallBack, Action<int> endFadeInCallBack, List<int> umidList0 = null, List<int> umidList1 = null, int fusionNum = 2, int rareNum = 1, float outSec = 0.5f, float inSec = 0.5f)
	{
		CutSceneMain.cs_path_bk = path;
		CutSceneMain.cs_startSceneCallBack = startSceneCallBack;
		CutSceneMain.cs_endSceneCallBack = endSceneCallBack;
		CutSceneMain.cs_umidList_bk0 = umidList0;
		CutSceneMain.cs_umidList_bk1 = umidList1;
		CutSceneMain.cs_fusionNum_bk = fusionNum;
		CutSceneMain.cs_rareNum_bk = rareNum;
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		GUIFadeControll.SetLoadInfo(new Action<int>(CutSceneMain.ExecCutScene), string.Empty, string.Empty, string.Empty, endFadeInCallBack, false);
		GUIManager.LoadCommonGUI("Effect/FADE_B", GUIMain.GetOrthoCamera().gameObject);
	}

	private static void ExecCutScene(int i)
	{
		GameObject gameObject = GUIMain.GetOrthoCamera().gameObject;
		if (gameObject != null)
		{
			CutSceneMain.goGUI_CAM = gameObject;
			Camera orthoCamera = GUIMain.GetOrthoCamera();
			orthoCamera.depth = 0f;
			orthoCamera.enabled = false;
		}
		if (CutSceneMain.cs_path_bk != "Cutscenes/dummy_scene")
		{
			GameObject original = AssetDataMng.Instance().LoadObject(CutSceneMain.cs_path_bk, null, true) as GameObject;
			CutSceneMain.goCutScene = UnityEngine.Object.Instantiate<GameObject>(original);
			CutSceneMain.goCutScene.transform.localPosition = new Vector3(4000f, 4000f, 0f);
		}
		string monsterGroupId = string.Empty;
		string text = CutSceneMain.cs_path_bk;
		switch (text)
		{
		case "Cutscenes/EvolutionUltimate":
		{
			CutSceneMain.csEvoUltimateScene = CutSceneMain.goCutScene.GetComponent<EvolutionUltimateController>();
			CutSceneMain.csEvoUltimateScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			int[] array = new int[]
			{
				CutSceneMain.cs_umidList_bk0[0],
				CutSceneMain.cs_umidList_bk0[1]
			};
			CutSceneMain.csEvoUltimateScene.target = array;
			monsterGroupId = CutSceneMain.cs_umidList_bk0[0].ToString();
			CutSceneMain.csEvoUltimateScene.monsterLevelClass1 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			monsterGroupId = CutSceneMain.cs_umidList_bk0[1].ToString();
			CutSceneMain.csEvoUltimateScene.monsterLevelClass2 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			break;
		}
		case "Cutscenes/Evolution":
		{
			CutSceneMain.csEvoScene = CutSceneMain.goCutScene.GetComponent<EvolutionController>();
			CutSceneMain.csEvoScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			int[] array = new int[]
			{
				CutSceneMain.cs_umidList_bk0[0],
				CutSceneMain.cs_umidList_bk0[1]
			};
			CutSceneMain.csEvoScene.target = array;
			monsterGroupId = CutSceneMain.cs_umidList_bk0[0].ToString();
			CutSceneMain.csEvoScene.monsterLevelClass1 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			monsterGroupId = CutSceneMain.cs_umidList_bk0[1].ToString();
			CutSceneMain.csEvoScene.monsterLevelClass2 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			break;
		}
		case "Cutscenes/ModeChange":
		{
			CutSceneMain.csModeChangeScene = CutSceneMain.goCutScene.GetComponent<ModeChangeController>();
			CutSceneMain.csModeChangeScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			int[] array = new int[]
			{
				CutSceneMain.cs_umidList_bk0[0],
				CutSceneMain.cs_umidList_bk0[1]
			};
			CutSceneMain.csModeChangeScene.target = array;
			monsterGroupId = CutSceneMain.cs_umidList_bk0[0].ToString();
			CutSceneMain.csModeChangeScene.monsterLevelClass1 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			monsterGroupId = CutSceneMain.cs_umidList_bk0[1].ToString();
			CutSceneMain.csModeChangeScene.monsterLevelClass2 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			break;
		}
		case "Cutscenes/Jogress":
		{
			CutSceneMain.csJogressScene = CutSceneMain.goCutScene.GetComponent<JogressController>();
			CutSceneMain.csJogressScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			int[] array = new int[]
			{
				CutSceneMain.cs_umidList_bk0[0],
				CutSceneMain.cs_umidList_bk0[1]
			};
			CutSceneMain.csJogressScene.target = array;
			monsterGroupId = CutSceneMain.cs_umidList_bk0[0].ToString();
			CutSceneMain.csJogressScene.monsterLevelClass1 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			monsterGroupId = CutSceneMain.cs_umidList_bk0[1].ToString();
			CutSceneMain.csJogressScene.monsterLevelClass2 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			int[] array2 = new int[CutSceneMain.cs_umidList_bk1.Count];
			for (int j = 0; j < CutSceneMain.cs_umidList_bk1.Count; j++)
			{
				array2[j] = CutSceneMain.cs_umidList_bk1[j];
			}
			CutSceneMain.csJogressScene.target2 = array2;
			break;
		}
		case "Cutscenes/Gasha":
		{
			CutSceneMain.csGachaScene = CutSceneMain.goCutScene.GetComponent<GashaController>();
			int[] array = new int[CutSceneMain.cs_umidList_bk0.Count];
			int[] array3 = new int[CutSceneMain.cs_umidList_bk1.Count];
			for (int j = 0; j < CutSceneMain.cs_umidList_bk0.Count; j++)
			{
				array[j] = CutSceneMain.cs_umidList_bk0[j];
			}
			for (int j = 0; j < CutSceneMain.cs_umidList_bk1.Count; j++)
			{
				array3[j] = CutSceneMain.cs_umidList_bk1[j];
			}
			CutSceneMain.csGachaScene.target = array;
			CutSceneMain.csGachaScene.growStep = array3;
			CutSceneMain.csGachaScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		}
		case "Cutscenes/Inheritance":
			CutSceneMain.csInharitanceScene = CutSceneMain.goCutScene.GetComponent<InharitanceController>();
			if (CutSceneMain.cs_fusionNum_bk == 3)
			{
				int[] array = new int[]
				{
					CutSceneMain.cs_umidList_bk0[0],
					CutSceneMain.cs_umidList_bk1[0],
					CutSceneMain.cs_umidList_bk0[0]
				};
				CutSceneMain.csInharitanceScene.target = array;
				monsterGroupId = CutSceneMain.cs_umidList_bk0[0].ToString();
				CutSceneMain.csInharitanceScene.monsterLevelClass1 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
				monsterGroupId = CutSceneMain.cs_umidList_bk1[0].ToString();
				CutSceneMain.csInharitanceScene.monsterLevelClass2 = int.Parse(MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId).growStep);
			}
			CutSceneMain.csInharitanceScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		case "Cutscenes/MedalInherit":
			CutSceneMain.csMedalInheritController = CutSceneMain.goCutScene.GetComponent<MedalInheritController>();
			if (CutSceneMain.cs_fusionNum_bk == 3)
			{
				int[] array = new int[]
				{
					CutSceneMain.cs_umidList_bk0[0],
					CutSceneMain.cs_umidList_bk1[0],
					CutSceneMain.cs_umidList_bk0[0]
				};
				CutSceneMain.csMedalInheritController.target = array;
			}
			CutSceneMain.csMedalInheritController.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		case "Cutscenes/Fusion":
			CutSceneMain.csFusionScene = CutSceneMain.goCutScene.GetComponent<FusionController>();
			if (CutSceneMain.cs_fusionNum_bk == 2)
			{
				int[] array = new int[]
				{
					CutSceneMain.cs_umidList_bk0[0],
					CutSceneMain.cs_umidList_bk1[0]
				};
				CutSceneMain.csFusionScene.target = array;
				CutSceneMain.csFusionScene.eggMonsterGroupId = CutSceneMain.cs_umidList_bk1[1];
				CutSceneMain.csFusionScene.rareUp = CutSceneMain.cs_rareNum_bk;
			}
			CutSceneMain.csFusionScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		case "Cutscenes/Training":
		{
			CutSceneMain.csTrainingScene = CutSceneMain.goCutScene.GetComponent<TrainingController>();
			int[] array = new int[CutSceneMain.cs_umidList_bk0.Count + CutSceneMain.cs_umidList_bk1.Count];
			array[0] = CutSceneMain.cs_umidList_bk0[0];
			for (int j = 0; j < CutSceneMain.cs_umidList_bk1.Count; j++)
			{
				array[j + 1] = CutSceneMain.cs_umidList_bk1[j];
			}
			CutSceneMain.csTrainingScene.target = array;
			CutSceneMain.csTrainingScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		}
		case "Cutscenes/Awakening":
			CutSceneMain.csAwakeningScene = CutSceneMain.goCutScene.GetComponent<AwakeningController>();
			CutSceneMain.csAwakeningScene.target = CutSceneMain.cs_umidList_bk0[0];
			CutSceneMain.csAwakeningScene.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		case "Cutscenes/chip_gacha":
			CutSceneMain.csChipGashaController = CutSceneMain.goCutScene.GetComponent<ChipGashaController>();
			CutSceneMain.csChipGashaController.UserAssetList = CutSceneMain.UserAssetList;
			CutSceneMain.csChipGashaController.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		case "Cutscenes/ticketGacha":
			CutSceneMain.csTicketGashaController = CutSceneMain.goCutScene.GetComponent<TicketGashaController>();
			CutSceneMain.csTicketGashaController.UserTicketList = CutSceneMain.UserTicketList;
			CutSceneMain.csTicketGashaController.EndCallBack = CutSceneMain.cs_endSceneCallBack;
			break;
		}
		if (CutSceneMain.cs_startSceneCallBack != null)
		{
			CutSceneMain.cs_startSceneCallBack(0);
		}
		if (CutSceneMain.cs_path_bk == "Cutscenes/dummy_scene")
		{
			CutSceneMain.cs_endSceneCallBack(0);
		}
	}

	public static void FadeReqCutSceneEnd()
	{
		if (CutSceneMain.goGUI_CAM != null)
		{
			Camera component = CutSceneMain.goGUI_CAM.GetComponent<Camera>();
			component.depth = 8f;
			component.enabled = true;
		}
		GUIFadeControll.ActionRestart();
	}
}
