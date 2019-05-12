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

	[Header("各進化先のリンク")]
	[SerializeField]
	private GameObject goListParts;

	private GUISelectPanelEvolution csSelectPanelEvolution;

	private GUIMonsterIcon monsterIcon;

	private CMD_Evolution.EvolutionReviewStatus execEvolutionReviewStatus;

	private List<EvolutionData.MonsterEvolveData> monsterEvolveDataList;

	private EvolutionData.MonsterEvolveData evolveDataBK;

	private int execClusterNum;

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
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
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
		this.csSelectPanelEvolution.AllBuild(this.monsterEvolveDataList);
		this.goListParts.SetActive(false);
	}

	public void EvolveDo(EvolutionData.MonsterEvolveData data, int needChip)
	{
		this.evolveDataBK = data;
		this.execClusterNum = needChip;
		this.EndCloseEvolveDo();
	}

	private RequestList CreateRequest()
	{
		RequestList requestList = new RequestList();
		GameWebAPI.RequestMN_MonsterEvolution addRequest = new GameWebAPI.RequestMN_MonsterEvolution
		{
			SetSendData = delegate(GameWebAPI.MN_Req_Evolution param)
			{
				param.userMonsterId = this.evolveDataBK.md.userMonster.userMonsterId;
				param.monsterId = int.Parse(this.evolveDataBK.md_next.monsterM.monsterId);
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_EvolutionExec response)
			{
				ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
				if (response.IsFirstUltimaEvolution())
				{
					this.execEvolutionReviewStatus = CMD_Evolution.EvolutionReviewStatus.FIRST_ULTIMA_EVOLUTION_REVIEW;
				}
				else if (response.IsFirstEvolution())
				{
					this.execEvolutionReviewStatus = CMD_Evolution.EvolutionReviewStatus.FIRST_EVOLUTION_REVIEW;
				}
			}
		};
		requestList.AddRequest(addRequest);
		GameWebAPI.MonsterSlotInfoListLogic addRequest2 = ChipAPIRequest.RequestAPIMonsterSlotInfo(new int[]
		{
			int.Parse(this.evolveDataBK.md.userMonster.userMonsterId)
		});
		requestList.AddRequest(addRequest2);
		return requestList;
	}

	private void EndCloseEvolveDo()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		string beforeModelId = this.evolveDataBK.md.GetMonsterMaster().Group.modelId;
		string beforeGrowStep = this.evolveDataBK.md.GetMonsterMaster().Group.growStep;
		RequestList requestList = this.CreateRequest();
		AppCoroutine.Start(requestList.Run(delegate()
		{
			this.EndEvolveDo(beforeModelId, beforeGrowStep);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void EndEvolveDo(string monsterModelId, string monsterGrowStep)
	{
		if (this.evolveDataBK.mem.effectType != "2")
		{
			GooglePlayGamesTool.Instance.Evolution();
		}
		string evolutionType = ClassSingleton<EvolutionData>.Instance.GetEvolutionEffectType(this.evolveDataBK.md.userMonster.monsterId, this.evolveDataBK.md_next.userMonster.monsterId);
		DataMng.Instance().US_PlayerInfoSubChipNum(this.execClusterNum);
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
		switch (effectType)
		{
		case "1":
			if (MonsterGrowStepData.IsUltimateScope(userMonster.GetMonsterMaster().Group.growStep))
			{
				cutsceneData = new CutsceneDataEvolutionUltimate
				{
					path = "Cutscenes/EvolutionUltimate",
					beforeModelId = monsterModelId,
					afterModelId = userMonster.GetMonsterMaster().Group.modelId,
					endCallback = new Action(CutSceneMain.FadeReqCutSceneEnd)
				};
			}
			else
			{
				cutsceneData = new CutsceneDataEvolution
				{
					path = "Cutscenes/Evolution",
					beforeModelId = monsterModelId,
					beforeGrowStep = monsterGrowStep,
					afterModelId = userMonster.GetMonsterMaster().Group.modelId,
					afterGrowStep = userMonster.GetMonsterMaster().Group.growStep,
					endCallback = new Action(CutSceneMain.FadeReqCutSceneEnd)
				};
			}
			break;
		case "2":
			cutsceneData = new CutsceneDataModeChange
			{
				path = "Cutscenes/ModeChange",
				beforeModelId = monsterModelId,
				afterModelId = userMonster.GetMonsterMaster().Group.modelId,
				endCallback = new Action(CutSceneMain.FadeReqCutSceneEnd)
			};
			break;
		case "3":
		case "4":
			cutsceneData = new CutsceneDataJogress
			{
				path = "Cutscenes/Jogress",
				beforeModelId = monsterModelId,
				afterModelId = userMonster.GetMonsterMaster().Group.modelId,
				partnerModelId = partnerModelId,
				endCallback = new Action(CutSceneMain.FadeReqCutSceneEnd)
			};
			break;
		}
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, new Action(this.StartCutSceneCallBack), null, delegate(int index)
		{
			this.ShowCompletedCutin(evolutionType);
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
		MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(this.evolveDataBK.md.userMonster.userMonsterId, true);
		CMD_CharacterDetailed.DataChg = monsterDataByUserMonsterID;
		base.SetCloseAction(delegate(int inx)
		{
			GUIMain.ShowCommonDialog(new Action<int>(this.RunRefreshPicturebook), "CMD_CharacterDetailed");
		});
		this.ClosePanel(false);
	}

	private void RunRefreshPicturebook(int noop)
	{
	}

	private void ShowCompletedCutin(string evolutionType)
	{
		if (PartsUpperCutinController.Instance != null)
		{
			PartsUpperCutinController.AnimeType playType;
			switch (evolutionType.ToInt32())
			{
			default:
				playType = PartsUpperCutinController.AnimeType.EvolutionComplete;
				break;
			case 2:
				playType = PartsUpperCutinController.AnimeType.ModeChangeComplete;
				break;
			case 3:
				playType = PartsUpperCutinController.AnimeType.Jogress;
				break;
			case 4:
				playType = PartsUpperCutinController.AnimeType.Combine;
				break;
			}
			if (this.execEvolutionReviewStatus == CMD_Evolution.EvolutionReviewStatus.NONE)
			{
				PartsUpperCutinController.Instance.PlayAnimator(playType, null);
				RestrictionInput.EndLoad();
			}
			else
			{
				PartsUpperCutinController.Instance.PlayAnimator(playType, new Action(this.ShowReviewDialog));
			}
		}
		else
		{
			RestrictionInput.EndLoad();
		}
	}

	private void ShowReviewDialog()
	{
		CMD_Evolution.EvolutionReviewStatus evolutionReviewStatus = this.execEvolutionReviewStatus;
		if (evolutionReviewStatus != CMD_Evolution.EvolutionReviewStatus.FIRST_EVOLUTION_REVIEW)
		{
			if (evolutionReviewStatus != CMD_Evolution.EvolutionReviewStatus.FIRST_ULTIMA_EVOLUTION_REVIEW)
			{
				RestrictionInput.EndLoad();
			}
			else
			{
				LeadReview.ShowReviewConfirm(LeadReview.MessageType.FIRST_ULTIMA_EVOLUTION, new Action(RestrictionInput.EndLoad), false);
			}
		}
		else
		{
			LeadReview.ShowReviewConfirm(LeadReview.MessageType.FIRST_EVOLUTION, new Action(RestrictionInput.EndLoad), false);
		}
	}

	private void OnTouchEvolutionDiagramButton()
	{
		MonsterData dataChg = CMD_BaseSelect.DataChg;
		FarmCameraControlForCMD.ClearRefCT();
		FarmCameraControlForCMD.On();
		GUIMain.DestroyAllDialog(null);
		CMD_EvolutionRouteMap.CreateDialog(null, dataChg.GetMonsterMaster());
	}

	public enum EvolveType
	{
		Normal = 1,
		ModeChange,
		Jogress,
		Combine,
		VersionUp
	}

	private enum EvolutionReviewStatus
	{
		NONE,
		FIRST_EVOLUTION_REVIEW,
		FIRST_ULTIMA_EVOLUTION_REVIEW
	}
}
