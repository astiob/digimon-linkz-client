using CharacterMiniStatusUI;
using Chip;
using Cutscene;
using Evolution;
using EvolutionRouteMap;
using Master;
using Monster;
using Picturebook;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebAPIRequest;

public sealed class CMD_Evolution : CMD
{
	public static CMD_Evolution instance;

	[SerializeField]
	private ChipBaseSelect chipBaseSelect;

	[SerializeField]
	private GameObject goMN_ICON_CHG;

	private GameObject goMN_ICON_CHG_2;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private UI_MonsterMiniStatus miniStatus;

	[SerializeField]
	private UILabel ngTXT_CHIP;

	[SerializeField]
	private GameObject goListParts;

	private GUISelectPanelEvolution csSelectPanelEvolution;

	private GUIMonsterIcon monsterIcon;

	private List<EvolutionData.MonsterEvolveData> monsterEvolveDataList;

	private CMD_Evolution.EvolutionReviewStatus execEvolutionReviewStatus;

	private EvolutionData.MonsterEvolveData evolveDataBK;

	private int execClusterNum;

	private CMD_CharacterDetailed detailedWindow;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	[CompilerGenerated]
	private static Action <>f__mg$cache2;

	[CompilerGenerated]
	private static Action <>f__mg$cache3;

	[CompilerGenerated]
	private static Action <>f__mg$cache4;

	protected override void Awake()
	{
		base.Awake();
		CMD_Evolution.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("EvolutionTitle"));
		this.ShowChgInfo();
		this.SetSelectedCharChg();
		this.UpdateClusterNum();
		this.SetCommonUI_Evolution();
		this.InitEvolution();
		base.Show(f, sizeX, sizeY, aT);
		base.SetTutorialAnyTime("anytime_second_tutorial_evolution");
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_Evolution.instance = null;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	private void ShowChgInfo()
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			this.chipBaseSelect.SetSelectedCharChg(CMD_BaseSelect.DataChg);
			this.monsterBasicInfo.SetMonsterData(CMD_BaseSelect.DataChg);
			this.miniStatus.SetMonsterData(CMD_BaseSelect.DataChg);
		}
	}

	private void UpdateClusterNum()
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		this.ngTXT_CHIP.text = StringFormat.Cluster(playerInfo.gamemoney);
		GUIPlayerStatus.RefreshParams_S(false);
	}

	private void SetSelectedCharChg()
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			if (this.goMN_ICON_CHG_2 != null)
			{
				UnityEngine.Object.Destroy(this.goMN_ICON_CHG_2);
			}
			Transform transform = this.goMN_ICON_CHG.transform;
			this.monsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(CMD_BaseSelect.DataChg, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.goMN_ICON_CHG_2 = this.monsterIcon.gameObject;
			this.goMN_ICON_CHG_2.SetActive(true);
			this.monsterIcon.SetTouchAct_S(new Action<MonsterData>(this.OnPushedBaseMonsterIcon));
			this.monsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			UIWidget component = this.goMN_ICON_CHG.GetComponent<UIWidget>();
			UIWidget component2 = this.goMN_ICON_CHG_2.GetComponent<UIWidget>();
			GUIManager.AddWidgetDepth(this.goMN_ICON_CHG_2.transform, component.depth - component2.depth);
			this.goMN_ICON_CHG.SetActive(false);
		}
	}

	private void OnPushedBaseMonsterIcon(MonsterData tappedMonsterData)
	{
		this.ClosePanel(true);
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
			icon.Lock = tappedMonsterData.userMonster.IsLocked;
			if (this.monsterIcon != null)
			{
				this.monsterIcon.Lock = CMD_BaseSelect.DataChg.userMonster.IsLocked;
			}
		}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Evolution;
	}

	private void SetCommonUI_Evolution()
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelEvolution", base.gameObject);
		this.csSelectPanelEvolution = gameObject.GetComponent<GUISelectPanelEvolution>();
		if (this.goEFC_RIGHT != null)
		{
			gameObject.transform.SetParent(this.goEFC_RIGHT.transform);
		}
		Vector3 localPosition = this.goListParts.transform.localPosition;
		Vector3 localPosition2 = gameObject.transform.localPosition;
		localPosition2.x = localPosition.x;
		GUICollider component = gameObject.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition2);
		this.csSelectPanelEvolution.selectParts = this.goListParts;
		this.csSelectPanelEvolution.ListWindowViewRect = ConstValue.GetRectWindow3();
	}

	private void InitEvolution()
	{
		this.monsterEvolveDataList = ClassSingleton<EvolutionData>.Instance.GetEvolveListByMonsterData(CMD_BaseSelect.DataChg);
		this.csSelectPanelEvolution.initLocation = true;
		this.csSelectPanelEvolution.AllBuild(this.monsterEvolveDataList, this, new Action<EvolutionData.MonsterEvolveData, int>(this.RequestEvolution));
		this.goListParts.SetActive(false);
	}

	private void RequestEvolution(EvolutionData.MonsterEvolveData evolutionData, int costCluster)
	{
		RequestList requestList = new RequestList();
		GameWebAPI.RequestMN_MonsterEvolution addRequest = new GameWebAPI.RequestMN_MonsterEvolution
		{
			SetSendData = delegate(GameWebAPI.MN_Req_Evolution param)
			{
				param.userMonsterId = evolutionData.md.userMonster.userMonsterId;
				param.monsterId = int.Parse(evolutionData.md_next.monsterM.monsterId);
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_EvolutionExec response)
			{
				ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
				if (response.IsFirstEvolution())
				{
					this.execEvolutionReviewStatus = CMD_Evolution.EvolutionReviewStatus.FIRST_EVOLUTION_REVIEW;
				}
				else if (response.IsFirstUltimaEvolution())
				{
					this.execEvolutionReviewStatus = CMD_Evolution.EvolutionReviewStatus.FIRST_ULTIMA_EVOLUTION_REVIEW;
				}
			}
		};
		requestList.AddRequest(addRequest);
		GameWebAPI.MonsterSlotInfoListLogic addRequest2 = ChipAPIRequest.RequestAPIMonsterSlotInfo(new int[]
		{
			int.Parse(evolutionData.md.userMonster.userMonsterId)
		});
		string beforeModelId = evolutionData.md.GetMonsterMaster().Group.modelId;
		string beforeGrowStep = evolutionData.md.GetMonsterMaster().Group.growStep;
		requestList.AddRequest(addRequest2);
		AppCoroutine.Start(requestList.Run(delegate()
		{
			this.evolveDataBK = evolutionData;
			this.EndEvolveDo(beforeModelId, beforeGrowStep, costCluster);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void EndEvolveDo(string monsterModelId, string monsterGrowStep, int costCluster)
	{
		if (this.evolveDataBK.mem.effectType != "2")
		{
			GooglePlayGamesTool.Instance.Evolution();
		}
		DataMng.Instance().US_PlayerInfoSubChipNum(costCluster);
		this.UpdateClusterNum();
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		ClassSingleton<EvolutionData>.Instance.EvolvePostProcess(this.evolveDataBK.itemList);
		string partnerModelId = string.Empty;
		if ("0" != this.evolveDataBK.mem.effectMonsterId)
		{
			MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(this.evolveDataBK.mem.effectMonsterId);
			if (monsterMasterByMonsterId != null)
			{
				partnerModelId = monsterMasterByMonsterId.Group.modelId;
			}
		}
		MonsterUserData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(this.evolveDataBK.md.GetMonster().userMonsterId);
		if (!MonsterPicturebookData.ExistPicturebook(userMonster.GetMonsterMaster().Group.monsterCollectionId))
		{
			MonsterPicturebookData.AddPictureBook(userMonster.GetMonsterMaster().Group.monsterCollectionId);
		}
		CutsceneDataBase cutsceneData = null;
		string effectType = this.evolveDataBK.mem.effectType;
		if (effectType != null)
		{
			if (!(effectType == "1"))
			{
				if (!(effectType == "2"))
				{
					if (effectType == "3" || effectType == "4")
					{
						CutsceneDataJogress cutsceneDataJogress = new CutsceneDataJogress();
						cutsceneDataJogress.path = "Cutscenes/Jogress";
						cutsceneDataJogress.beforeModelId = monsterModelId;
						cutsceneDataJogress.afterModelId = userMonster.GetMonsterMaster().Group.modelId;
						cutsceneDataJogress.partnerModelId = partnerModelId;
						CutsceneDataJogress cutsceneDataJogress2 = cutsceneDataJogress;
						if (CMD_Evolution.<>f__mg$cache3 == null)
						{
							CMD_Evolution.<>f__mg$cache3 = new Action(CutSceneMain.FadeReqCutSceneEnd);
						}
						cutsceneDataJogress2.endCallback = CMD_Evolution.<>f__mg$cache3;
						cutsceneData = cutsceneDataJogress;
					}
				}
				else
				{
					CutsceneDataModeChange cutsceneDataModeChange = new CutsceneDataModeChange();
					cutsceneDataModeChange.path = "Cutscenes/ModeChange";
					cutsceneDataModeChange.beforeModelId = monsterModelId;
					cutsceneDataModeChange.afterModelId = userMonster.GetMonsterMaster().Group.modelId;
					CutsceneDataModeChange cutsceneDataModeChange2 = cutsceneDataModeChange;
					if (CMD_Evolution.<>f__mg$cache2 == null)
					{
						CMD_Evolution.<>f__mg$cache2 = new Action(CutSceneMain.FadeReqCutSceneEnd);
					}
					cutsceneDataModeChange2.endCallback = CMD_Evolution.<>f__mg$cache2;
					cutsceneData = cutsceneDataModeChange;
				}
			}
			else if (MonsterGrowStepData.IsUltimateScope(userMonster.GetMonsterMaster().Group.growStep))
			{
				CutsceneDataEvolutionUltimate cutsceneDataEvolutionUltimate = new CutsceneDataEvolutionUltimate();
				cutsceneDataEvolutionUltimate.path = "Cutscenes/EvolutionUltimate";
				cutsceneDataEvolutionUltimate.beforeModelId = monsterModelId;
				cutsceneDataEvolutionUltimate.afterModelId = userMonster.GetMonsterMaster().Group.modelId;
				CutsceneDataEvolutionUltimate cutsceneDataEvolutionUltimate2 = cutsceneDataEvolutionUltimate;
				if (CMD_Evolution.<>f__mg$cache0 == null)
				{
					CMD_Evolution.<>f__mg$cache0 = new Action(CutSceneMain.FadeReqCutSceneEnd);
				}
				cutsceneDataEvolutionUltimate2.endCallback = CMD_Evolution.<>f__mg$cache0;
				cutsceneData = cutsceneDataEvolutionUltimate;
			}
			else
			{
				CutsceneDataEvolution cutsceneDataEvolution = new CutsceneDataEvolution();
				cutsceneDataEvolution.path = "Cutscenes/Evolution";
				cutsceneDataEvolution.beforeModelId = monsterModelId;
				cutsceneDataEvolution.beforeGrowStep = monsterGrowStep;
				cutsceneDataEvolution.afterModelId = userMonster.GetMonsterMaster().Group.modelId;
				cutsceneDataEvolution.afterGrowStep = userMonster.GetMonsterMaster().Group.growStep;
				CutsceneDataEvolution cutsceneDataEvolution2 = cutsceneDataEvolution;
				if (CMD_Evolution.<>f__mg$cache1 == null)
				{
					CMD_Evolution.<>f__mg$cache1 = new Action(CutSceneMain.FadeReqCutSceneEnd);
				}
				cutsceneDataEvolution2.endCallback = CMD_Evolution.<>f__mg$cache1;
				cutsceneData = cutsceneDataEvolution;
			}
		}
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, new Action(this.StartCutSceneCallBack), delegate()
		{
			this.detailedWindow.StartAnimation();
			if (this.execEvolutionReviewStatus != CMD_Evolution.EvolutionReviewStatus.FIRST_EVOLUTION_REVIEW && this.execEvolutionReviewStatus != CMD_Evolution.EvolutionReviewStatus.FIRST_ULTIMA_EVOLUTION_REVIEW)
			{
				RestrictionInput.EndLoad();
			}
		}, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack()
	{
		if (CMD_BaseSelect.instance != null)
		{
			CMD_BaseSelect.instance.InitMonsterList(false);
			CMD_BaseSelect.instance.ChipNumUpdate();
			CMD_BaseSelect.instance.SetEmpty();
			CMD_BaseSelect.instance.SetDecideButton(false);
		}
		MonsterData afterMonster = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(this.evolveDataBK.md.userMonster.userMonsterId, true);
		base.SetCloseAction(delegate(int inx)
		{
			CMD_Evolution $this = this;
			MonsterData afterMonster = afterMonster;
			string effectType = this.evolveDataBK.mem.effectType;
			bool reviewFirstEvolution = this.execEvolutionReviewStatus == CMD_Evolution.EvolutionReviewStatus.FIRST_EVOLUTION_REVIEW;
			bool reviewFirstUltimaEvolution = this.execEvolutionReviewStatus == CMD_Evolution.EvolutionReviewStatus.FIRST_ULTIMA_EVOLUTION_REVIEW;
			if (CMD_Evolution.<>f__mg$cache4 == null)
			{
				CMD_Evolution.<>f__mg$cache4 = new Action(RestrictionInput.EndLoad);
			}
			$this.detailedWindow = CMD_CharacterDetailed.CreateWindow(afterMonster, effectType, reviewFirstEvolution, reviewFirstUltimaEvolution, CMD_Evolution.<>f__mg$cache4);
		});
		this.ClosePanel(false);
	}

	private void OnTouchEvolutionDiagramButton()
	{
		MonsterData dataChg = CMD_BaseSelect.DataChg;
		CMD_EvolutionRouteMap.CreateDialog(null, dataChg.GetMonsterMaster());
	}

	private enum EvolutionReviewStatus
	{
		NONE,
		FIRST_EVOLUTION_REVIEW,
		FIRST_ULTIMA_EVOLUTION_REVIEW
	}
}
