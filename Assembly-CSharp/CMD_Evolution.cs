using Evolution;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

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
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill monsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[SerializeField]
	private UILabel ngTXT_CHIP;

	[SerializeField]
	[Header("キャラクターのステータスPanel")]
	private StatusPanel statusPanel;

	[Header("各進化先のリンク")]
	[SerializeField]
	private GameObject goListParts;

	private GUISelectPanelEvolution csSelectPanelEvolution;

	private int statusPage = 1;

	[SerializeField]
	private List<GameObject> goStatusPanelPage;

	private GUIMonsterIcon monsterIcon;

	private CMD_Evolution.EvolutionReviewStatus execEvolutionReviewStatus;

	private List<EvolutionData.MonsterEvolveData> monsterEvolveDataList;

	private EvolutionData.MonsterEvolveData evolveDataBK;

	private int beforeMonsterGroupId;

	private int afterMonsterGroupId;

	private int execClusterNum;

	protected override void Awake()
	{
		base.Awake();
		CMD_Evolution.instance = this;
		this.monsterMedalList.SetActive(false);
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
		this.statusPanel.SetEnable(CMD_BaseSelect.DataChg != null);
		if (CMD_BaseSelect.DataChg != null)
		{
			this.chipBaseSelect.SetSelectedCharChg(CMD_BaseSelect.DataChg);
			this.monsterBasicInfo.SetMonsterData(CMD_BaseSelect.DataChg);
			this.monsterStatusList.SetValues(CMD_BaseSelect.DataChg, false);
			this.monsterLeaderSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterUniqueSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterSuccessionSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterMedalList.SetValues(CMD_BaseSelect.DataChg.userMonster);
			this.monsterResistanceList.SetValues(CMD_BaseSelect.DataChg);
			this.StatusPageChange(false);
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
				UnityEngine.Object.DestroyImmediate(this.goMN_ICON_CHG_2);
			}
			Transform transform = this.goMN_ICON_CHG.transform;
			this.monsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(CMD_BaseSelect.DataChg, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.monsterIcon.Lock = CMD_BaseSelect.DataChg.userMonster.IsLocked;
			this.goMN_ICON_CHG_2 = this.monsterIcon.gameObject;
			this.goMN_ICON_CHG_2.SetActive(true);
			this.monsterIcon.Data = CMD_BaseSelect.DataChg;
			this.monsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			UIWidget component = this.goMN_ICON_CHG.GetComponent<UIWidget>();
			UIWidget component2 = this.goMN_ICON_CHG_2.GetComponent<UIWidget>();
			GUIManager.AddWidgetDepth(this.goMN_ICON_CHG_2.transform, component.depth - component2.depth);
			this.goMN_ICON_CHG.SetActive(false);
		}
		else
		{
			global::Debug.LogError("ここへは来ない");
		}
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			PartyUtil.SetLock(tappedMonsterData, false);
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

	private void EndCloseEvolveDo()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.beforeMonsterGroupId = int.Parse(this.evolveDataBK.md.monsterM.monsterGroupId);
		GameWebAPI.RequestMN_MonsterEvolution request = new GameWebAPI.RequestMN_MonsterEvolution
		{
			SetSendData = delegate(GameWebAPI.MN_Req_Evolution param)
			{
				param.userMonsterId = int.Parse(this.evolveDataBK.md.userMonster.userMonsterId);
				param.monsterId = int.Parse(this.evolveDataBK.md_next.monsterM.monsterId);
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_EvolutionExec response)
			{
				if (response.userMonsterList != null)
				{
					DataMng.Instance().SetUserMonsterList(response.userMonsterList);
				}
				if (response.userMonster != null)
				{
					DataMng.Instance().SetUserMonster(response.userMonster);
				}
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
		base.StartCoroutine(request.Run(new Action(this.EndEvolveDo), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndEvolveDo()
	{
		if (this.evolveDataBK.mem.effectType != "2")
		{
			GooglePlayGamesTool.Instance.Evolution();
		}
		string evolutionType = this.evolveDataBK.md.GetEvolutionType(this.evolveDataBK.md_next.userMonster.monsterId);
		DataMng.Instance().US_PlayerInfoSubChipNum(this.execClusterNum);
		this.UpdateClusterNum();
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.RefreshMonsterDataList();
		List<int> umidList = ClassSingleton<EvolutionData>.Instance.EvolvePostProcess(this.evolveDataBK);
		MonsterData monsterDataByUserMonsterID = monsterDataMng.GetMonsterDataByUserMonsterID(this.evolveDataBK.md.userMonster.userMonsterId, true);
		monsterDataByUserMonsterID.RefleshNormalChipManagSlot();
		string path = string.Empty;
		bool flag = false;
		int num = int.Parse(monsterDataByUserMonsterID.monsterMG.growStep);
		if (num == 7)
		{
			flag = true;
		}
		string effectType = this.evolveDataBK.mem.effectType;
		switch (effectType)
		{
		case "1":
			if (flag)
			{
				path = "Cutscenes/EvolutionUltimate";
			}
			else
			{
				path = "Cutscenes/Evolution";
			}
			break;
		case "2":
			path = "Cutscenes/ModeChange";
			break;
		case "3":
		case "4":
			path = "Cutscenes/Jogress";
			break;
		}
		this.afterMonsterGroupId = int.Parse(monsterDataByUserMonsterID.monsterM.monsterGroupId);
		List<int> umidList2 = new List<int>
		{
			this.beforeMonsterGroupId,
			this.afterMonsterGroupId
		};
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(path, new Action<int>(this.StartCutSceneCallBack), delegate(int index)
		{
			CutSceneMain.FadeReqCutSceneEnd();
		}, delegate(int index)
		{
			this.ShowCompletedCutin(evolutionType);
		}, umidList2, umidList, 2, 1, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack(int i)
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

	public void StatusPageChangeTap()
	{
		this.StatusPageChange(true);
	}

	public void StatusPageChange(bool pageChange)
	{
		if (pageChange)
		{
			if (this.statusPage < this.goStatusPanelPage.Count)
			{
				this.statusPage++;
			}
			else
			{
				this.statusPage = 1;
			}
		}
		int num = 1;
		foreach (GameObject gameObject in this.goStatusPanelPage)
		{
			if (num == this.statusPage)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
			num++;
		}
	}

	private void RunRefreshPicturebook(int noop)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask picturebookData = MonsterDataMng.Instance().GetPicturebookData(DataMng.Instance().RespDataCM_Login.playerInfo.userId, false);
		AppCoroutine.Start(picturebookData.Run(new Action(RestrictionInput.EndLoad), delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			CMD_BaseSelect.instance.ClosePanel(true);
		}, null), false);
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

	public enum EvolveType
	{
		Normal = 1,
		ModeChange,
		Jogress,
		Combine
	}

	private enum EvolutionReviewStatus
	{
		NONE,
		FIRST_EVOLUTION_REVIEW,
		FIRST_ULTIMA_EVOLUTION_REVIEW
	}
}
