using Cutscene;
using Evolution;
using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CMD_VersionUP : CMD_PairSelectBase
{
	[SerializeField]
	private List<VersionUpItem> verUpItemList;

	[SerializeField]
	private VersionUpDetail versionUpDetail;

	private List<HaveSoulData> almHasList_cache;

	private List<EvolutionData.MonsterEvolveData> medList_cache;

	private List<HaveSoulData> almSelectList;

	private CMD_AlMightySelect cmd_AlMightySelect;

	private GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList updatedUserMonster_bk;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	[CompilerGenerated]
	private static Action <>f__mg$cache2;

	public string CurSelectedSoulId { get; set; }

	protected override void ShowSecondTutorial()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			TutorialObserver tutorialObserver2 = tutorialObserver;
			string tutorialName = "second_tutorial_version_up";
			if (CMD_VersionUP.<>f__mg$cache0 == null)
			{
				CMD_VersionUP.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, CMD_VersionUP.<>f__mg$cache0, delegate
			{
				GUICollider.EnableAllCollider("CMD_VersionUP");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_VersionUP");
		}
		base.SetTutorialAnyTime("anytime_second_tutorial_version_up");
	}

	private void OnPushDecide()
	{
		CMD_VersionUpModal cmd_VersionUpModal = GUIMain.ShowCommonDialog(null, "CMD_VersionUpModal", null) as CMD_VersionUpModal;
		int beforeMaxLevel = int.Parse(this.baseDigimon.monsterM.maxLevel);
		int afterMaxLevel = int.Parse(this.medList_cache[0].md_next.monsterM.maxLevel);
		int num = int.Parse(this.medList_cache[0].md_next.monsterM.rare);
		bool isSkillAdd = num == 6;
		cmd_VersionUpModal.SetMonsterIcon(this.medList_cache[0].md_next);
		cmd_VersionUpModal.ShowDetail(beforeMaxLevel, afterMaxLevel, isSkillAdd);
		cmd_VersionUpModal.SetDescription(this.baseDigimon);
		cmd_VersionUpModal.SetActionYesButton(delegate
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			this.DoExec();
		});
	}

	private void DoExec()
	{
		this.useClusterBK = this.CalcCluster();
		GameWebAPI.RequestMN_VersionUP request = new GameWebAPI.RequestMN_VersionUP
		{
			SetSendData = delegate(GameWebAPI.MN_Req_VersionUP param)
			{
				param.baseUserMonsterId = int.Parse(this.baseDigimon.userMonster.userMonsterId);
				param.target = int.Parse(this.medList_cache[0].md_next.monsterM.monsterId);
				param.material = this.GetMaterialList();
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_VersionUP response)
			{
				if (response.userMonster != null)
				{
					this.updatedUserMonster_bk = response.userMonster;
					ClassSingleton<MonsterUserDataMng>.Instance.AddUserMonsterData(response.userMonster);
				}
			}
		};
		AppCoroutine.Start(request.Run(delegate()
		{
			AppCoroutine.Start(base.GetChipSlotInfo(), false);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
	}

	protected override void EndSuccess()
	{
		bool isEquipChip = this.baseDigimon.GetChipEquip().IsAttachedChip();
		if (isEquipChip)
		{
			base.RemoveEquipChip(false, this.baseDigimon.userMonster.userMonsterId);
		}
		string modelId = this.baseDigimon.GetMonsterMaster().Group.modelId;
		this.DeleteUsedSoul();
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(this.baseDigimon.userMonster.userMonsterId);
		ChipDataMng.GetUserChipSlotData().DeleteMonsterSlot(this.baseDigimon.userMonster.userMonsterId);
		GooglePlayGamesTool.Instance.Laboratory();
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonsterList());
		MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(this.updatedUserMonster_bk.userMonsterId);
		CutsceneDataVersionUp cutsceneDataVersionUp = new CutsceneDataVersionUp();
		cutsceneDataVersionUp.path = "Cutscenes/VersionUp";
		cutsceneDataVersionUp.beforeModelId = modelId;
		cutsceneDataVersionUp.afterModelId = userMonster.GetMonsterMaster().Group.modelId;
		CutsceneDataVersionUp cutsceneDataVersionUp2 = cutsceneDataVersionUp;
		if (CMD_VersionUP.<>f__mg$cache1 == null)
		{
			CMD_VersionUP.<>f__mg$cache1 = new Action(CutSceneMain.FadeReqCutSceneEnd);
		}
		cutsceneDataVersionUp2.endCallback = CMD_VersionUP.<>f__mg$cache1;
		CutsceneDataVersionUp cutsceneData = cutsceneDataVersionUp;
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, delegate()
		{
			this.OnStartCutScene(isEquipChip);
		}, delegate()
		{
			this.characterDetailed.StartAnimation();
			if (!isEquipChip)
			{
				RestrictionInput.EndLoad();
			}
		}, 0.5f, 0.5f);
	}

	private void OnStartCutScene(bool isResetEquipChip)
	{
		base.RemoveBaseDigimon();
		base.RemovePartnerDigimon();
		this.ClearTargetStatus();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		base.UpdateClusterNum();
		GUIPlayerStatus.RefreshParams_S(false);
		string userMonsterId = this.GetUserMonsterData().userMonsterId;
		MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
		Action endCutin = null;
		if (isResetEquipChip)
		{
			if (CMD_VersionUP.<>f__mg$cache2 == null)
			{
				CMD_VersionUP.<>f__mg$cache2 = new Action(RestrictionInput.EndLoad);
			}
			endCutin = CMD_VersionUP.<>f__mg$cache2;
		}
		this.characterDetailed = CMD_CharacterDetailed.CreateWindow(monsterDataByUserMonsterID, isResetEquipChip, endCutin);
	}

	protected override string GetTitle()
	{
		return StringMaster.GetString("VersionUpTitle");
	}

	protected override void ClearTargetStatus()
	{
		this.versionUpDetail.ClearStatus();
		this.ClearItemIcon();
	}

	protected override GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetUserMonsterData()
	{
		return this.updatedUserMonster_bk;
	}

	protected override int CalcCluster()
	{
		int result = 0;
		if (this.baseDigimon != null)
		{
			result = EvolutionData.CalcClusterForVersionUp(this.baseDigimon.monsterM.monsterId);
		}
		return result;
	}

	protected override void ShowMATInfo_1()
	{
	}

	protected override bool TargetStatusReady()
	{
		return this.baseDigimon != null;
	}

	protected override void SetTargetStatus()
	{
		this.almHasList_cache = VersionUpMaterialData.GetVersionUpAlMightyMaterial();
		this.medList_cache = ClassSingleton<EvolutionData>.Instance.GetVersionUpList(this.baseDigimon);
		int oldLev = int.Parse(this.baseDigimon.monsterM.maxLevel);
		int newLev = int.Parse(this.medList_cache[0].md_next.monsterM.maxLevel);
		bool isSkillAdd = false;
		int num = int.Parse(this.medList_cache[0].md_next.monsterM.rare);
		if (num == 6)
		{
			isSkillAdd = true;
		}
		this.versionUpDetail.ShowDetail(oldLev, newLev, isSkillAdd);
		this.ShowItemIcon();
	}

	protected override bool CanEnter()
	{
		return 0 < this.GetCountCanVersionUpMonster();
	}

	protected override string GetInfoCannotEnter()
	{
		return StringMaster.GetString("VersionUpCannotVersionUp");
	}

	protected override bool CanSelectMonster(int noop)
	{
		return 0 < this.GetCountCanVersionUpMonster();
	}

	private int GetCountCanVersionUpMonster()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.CAN_VERSION_UP);
		return list.Count;
	}

	protected override void OpenCanNotSelectMonsterPop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("VersionUpTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("VersionUpCannotVersionUp");
	}

	protected override void SetBaseTouchAct_L(GUIMonsterIcon cs)
	{
		cs.SetTouchAct_L(new Action<MonsterData>(base.ActMIconLong));
	}

	protected override void SetBaseSelectType()
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.VERSION_UP;
	}

	protected override void OnBaseSelected()
	{
		this.versionUpDetail.SetMonsterIcon(this.medList_cache[0].md_next, true);
	}

	protected override bool CheckMaterial()
	{
		if (this.baseDigimon != null)
		{
			bool result = this.CheckMaterialNum();
			int num = int.Parse(this.baseDigimon.userMonster.level);
			int num2 = int.Parse(this.baseDigimon.monsterM.maxLevel);
			if (num2 > num)
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	private bool CheckMaterialNum()
	{
		List<EvolutionData.MonsterEvolveItem> itemList = this.medList_cache[0].itemList;
		for (int i = 0; i < itemList.Count; i++)
		{
			VersionUpItem versionUpItem = this.verUpItemList[i];
			if (versionUpItem.AlmightySoulData == null || versionUpItem.AlmightySoulData.haveNum < versionUpItem.NeedNum)
			{
				if (versionUpItem.baseSoulData == null || versionUpItem.baseSoulData.haveNum < versionUpItem.NeedNum)
				{
					return false;
				}
			}
		}
		return true;
	}

	private int[] GetMaterialList()
	{
		List<EvolutionData.MonsterEvolveItem> itemList = this.medList_cache[0].itemList;
		int[] array = new int[itemList.Count];
		for (int i = 0; i < itemList.Count; i++)
		{
			VersionUpItem versionUpItem = this.verUpItemList[i];
			if (versionUpItem.AlmightySoulData != null)
			{
				array[i] = int.Parse(versionUpItem.AlmightySoulData.soulM.soulId);
			}
			else if (versionUpItem.baseSoulData != null)
			{
				array[i] = int.Parse(versionUpItem.baseSoulData.soulM.soulId);
			}
		}
		return array;
	}

	private void DeleteUsedSoul()
	{
		List<EvolutionData.MonsterEvolveItem> itemList = this.medList_cache[0].itemList;
		string materialId = string.Empty;
		for (int i = 0; i < itemList.Count; i++)
		{
			VersionUpItem versionUpItem = this.verUpItemList[i];
			if (versionUpItem.AlmightySoulData != null)
			{
				materialId = versionUpItem.AlmightySoulData.soulM.soulId;
			}
			else if (versionUpItem.baseSoulData != null)
			{
				materialId = versionUpItem.baseSoulData.soulM.soulId;
			}
			GameWebAPI.UserSoulData userEvolutionMaterial = EvolutionMaterialData.GetUserEvolutionMaterial(materialId);
			int num = int.Parse(userEvolutionMaterial.num);
			userEvolutionMaterial.num = (num - versionUpItem.NeedNum).ToString();
		}
	}

	private void ShowItemIcon()
	{
		if (this.baseDigimon != null && this.medList_cache.Count > 0)
		{
			List<EvolutionData.MonsterEvolveItem> itemList = this.medList_cache[0].itemList;
			int i;
			for (i = 0; i < itemList.Count; i++)
			{
				VersionUpItem vupItem = this.verUpItemList[i];
				vupItem.AlmightySoulData = null;
				vupItem.gameObject.SetActive(true);
				vupItem.spIcon.enabled = false;
				vupItem.texIcon.enabled = true;
				vupItem.spNumPlate.gameObject.SetActive(true);
				vupItem.lbNum.gameObject.SetActive(true);
				string soulId = itemList[i].sd_item.soulId;
				vupItem.SetTouchAct_L(delegate
				{
					this.ActCallBackDropItem(vupItem);
				});
				vupItem.baseSoulData = new HaveSoulData();
				vupItem.baseSoulData.soulM = VersionUpMaterialData.GetSoulMasterBySoulId(soulId);
				vupItem.baseSoulData.haveNum = itemList[i].haveNum;
				vupItem.baseSoulData.curUsedNum = 0;
				vupItem.NeedNum = itemList[i].need_num;
				string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(soulId);
				if (vupItem.baseSoulData.haveNum < vupItem.NeedNum)
				{
					HaveSoulData almightySoulData = null;
					bool flag = VersionUpMaterialData.CanChangeToAlmighty(this.almHasList_cache, vupItem.baseSoulData.soulM.soulId, vupItem.NeedNum, ref almightySoulData);
					if (flag)
					{
						vupItem.AlmightySoulData = almightySoulData;
						vupItem.AlmightySoulData.curUsedNum += vupItem.NeedNum;
						evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(vupItem.AlmightySoulData.soulM.soulId);
					}
				}
				Vector3 localScale = vupItem.gameObject.transform.localScale;
				vupItem.gameObject.transform.localScale = Vector2.zero;
				this.LoadObjectASync(evolveItemIconPathByID, vupItem, localScale);
			}
			while (i < this.verUpItemList.Count)
			{
				this.verUpItemList[i].gameObject.SetActive(false);
				i++;
			}
			this.SetAlmightyIcon();
		}
	}

	private void SetAlmightyIcon()
	{
		List<EvolutionData.MonsterEvolveItem> itemList = this.medList_cache[0].itemList;
		for (int i = 0; i < itemList.Count; i++)
		{
			VersionUpItem vupItem = this.verUpItemList[i];
			bool flag;
			if (vupItem.AlmightySoulData != null)
			{
				flag = true;
			}
			else
			{
				HaveSoulData haveSoulData = null;
				flag = VersionUpMaterialData.CanChangeToAlmighty(this.almHasList_cache, vupItem.baseSoulData.soulM.soulId, vupItem.NeedNum, ref haveSoulData);
			}
			if (flag)
			{
				vupItem.lbSelect.gameObject.SetActive(true);
				vupItem.SetTouchAct_S(delegate
				{
					this.SHowAlmightySelect(vupItem);
				});
			}
			else
			{
				vupItem.lbSelect.gameObject.SetActive(false);
				vupItem.SetTouchAct_S(null);
			}
		}
	}

	private void SHowAlmightySelect(VersionUpItem selectedVUpItem)
	{
		this.almSelectList = new List<HaveSoulData>();
		this.almSelectList.Add(selectedVUpItem.baseSoulData);
		if (selectedVUpItem.AlmightySoulData == null)
		{
			this.CurSelectedSoulId = selectedVUpItem.baseSoulData.soulM.soulId;
		}
		else
		{
			this.CurSelectedSoulId = selectedVUpItem.AlmightySoulData.soulM.soulId;
		}
		for (int i = 0; i < this.almHasList_cache.Count; i++)
		{
			bool flag = false;
			if (selectedVUpItem.AlmightySoulData != null && selectedVUpItem.AlmightySoulData.soulM.soulId == this.almHasList_cache[i].soulM.soulId)
			{
				flag = true;
			}
			int num = this.almHasList_cache[i].haveNum - this.almHasList_cache[i].curUsedNum;
			if (num >= selectedVUpItem.NeedNum || flag)
			{
				this.almSelectList.Add(this.almHasList_cache[i]);
			}
		}
		this.cmd_AlMightySelect = (GUIMain.ShowCommonDialog(new Action<int>(this.SoulChangeOperation), "CMD_AlMightySelect", null) as CMD_AlMightySelect);
		this.cmd_AlMightySelect.MakeList(this.almSelectList, selectedVUpItem.NeedNum, this.CurSelectedSoulId);
		this.cmd_AlMightySelect.SelectedVersionUpItem = selectedVUpItem;
	}

	private void ClearItemIcon()
	{
		for (int i = 0; i < this.verUpItemList.Count; i++)
		{
			VersionUpItem versionUpItem = this.verUpItemList[i];
			versionUpItem.gameObject.SetActive(true);
			versionUpItem.spIcon.enabled = true;
			versionUpItem.texIcon.enabled = false;
			versionUpItem.lbSelect.gameObject.SetActive(false);
			versionUpItem.spNumPlate.gameObject.SetActive(false);
			versionUpItem.lbNum.gameObject.SetActive(false);
			versionUpItem.SetTouchAct_S(null);
			versionUpItem.SetTouchAct_L(null);
		}
	}

	private void LoadObjectASync(string path, VersionUpItem vupItem, Vector3 vS)
	{
		AssetDataMng.Instance().LoadObjectASync(path, delegate(UnityEngine.Object obj)
		{
			this.ShowDropItemsCB(obj, vupItem, vS);
		});
	}

	private void ShowDropItemsCB(UnityEngine.Object obj, VersionUpItem vupItem, Vector3 vS)
	{
		if (CMD_PairSelectBase.instance != null)
		{
			Texture2D mainTexture = obj as Texture2D;
			UITexture texIcon = vupItem.texIcon;
			texIcon.mainTexture = mainTexture;
			if (this.medList_cache.Count > 0)
			{
				if (vupItem.AlmightySoulData != null || vupItem.baseSoulData.haveNum >= vupItem.NeedNum)
				{
					texIcon.color = new Color(1f, 1f, 1f, 1f);
				}
				else
				{
					texIcon.color = new Color(0.6f, 0.6f, 0.6f, 1f);
				}
				int haveNum;
				if (vupItem.AlmightySoulData != null)
				{
					haveNum = vupItem.AlmightySoulData.haveNum;
				}
				else
				{
					haveNum = vupItem.baseSoulData.haveNum;
				}
				if (haveNum < vupItem.NeedNum)
				{
					vupItem.lbNum.text = string.Format(StringMaster.GetString("EvolutionNotEnoughFraction"), haveNum, vupItem.NeedNum);
				}
				else
				{
					vupItem.lbNum.text = string.Format(StringMaster.GetString("EvolutionEnoughFraction"), haveNum, vupItem.NeedNum);
				}
				Hashtable hashtable = new Hashtable();
				hashtable.Add("x", vS.x);
				hashtable.Add("y", vS.y);
				hashtable.Add("time", 0.4f);
				hashtable.Add("delay", 0.01f);
				hashtable.Add("easetype", "spring");
				hashtable.Add("oncomplete", "ScaleEnd");
				hashtable.Add("oncompleteparams", 0);
				iTween.ScaleTo(vupItem.gameObject, hashtable);
				ITweenResumer component = vupItem.gameObject.GetComponent<ITweenResumer>();
				if (component == null)
				{
					vupItem.gameObject.AddComponent<ITweenResumer>();
				}
			}
		}
	}

	private void ScaleEnd()
	{
	}

	private void ActCallBackDropItem(VersionUpItem vupItem)
	{
		string soulId = string.Empty;
		if (vupItem.AlmightySoulData != null)
		{
			soulId = vupItem.AlmightySoulData.soulM.soulId;
		}
		else
		{
			soulId = vupItem.baseSoulData.soulM.soulId;
		}
		GameWebAPI.RespDataMA_GetSoulM.SoulM soulMaster = ClassSingleton<EvolutionData>.Instance.GetSoulMaster(soulId);
		CMD_QuestItemPOP.Create(soulMaster);
	}

	private void OnTouchedEvoltionItemListBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_EvolutionItemList", null);
	}

	private void SoulChangeOperation(int idx)
	{
		if (idx == 1 && this.CurSelectedSoulId != this.cmd_AlMightySelect.CurSelectedSoulId)
		{
			this.ExchangeData();
			this.ShowChangeIcon();
			this.SetAlmightyIcon();
			base.BtnCont();
		}
	}

	private void ExchangeData()
	{
		string curSelectedSoulId = this.CurSelectedSoulId;
		string curSelectedSoulId2 = this.cmd_AlMightySelect.CurSelectedSoulId;
		VersionUpItem selectedVersionUpItem = this.cmd_AlMightySelect.SelectedVersionUpItem;
		if (curSelectedSoulId2 == selectedVersionUpItem.baseSoulData.soulM.soulId)
		{
			for (int i = 0; i < this.almHasList_cache.Count; i++)
			{
				if (this.almHasList_cache[i].soulM.soulId == curSelectedSoulId)
				{
					this.almHasList_cache[i].curUsedNum -= selectedVersionUpItem.NeedNum;
				}
			}
			selectedVersionUpItem.AlmightySoulData = null;
		}
		else if (curSelectedSoulId == selectedVersionUpItem.baseSoulData.soulM.soulId)
		{
			int i;
			for (i = 0; i < this.almHasList_cache.Count; i++)
			{
				if (this.almHasList_cache[i].soulM.soulId == curSelectedSoulId2)
				{
					break;
				}
			}
			this.almHasList_cache[i].curUsedNum += selectedVersionUpItem.NeedNum;
			selectedVersionUpItem.AlmightySoulData = this.almHasList_cache[i];
		}
		else
		{
			int i;
			for (i = 0; i < this.almHasList_cache.Count; i++)
			{
				if (this.almHasList_cache[i].soulM.soulId == curSelectedSoulId2)
				{
					break;
				}
			}
			this.almHasList_cache[i].curUsedNum += selectedVersionUpItem.NeedNum;
			selectedVersionUpItem.AlmightySoulData = this.almHasList_cache[i];
			for (i = 0; i < this.almHasList_cache.Count; i++)
			{
				if (this.almHasList_cache[i].soulM.soulId == curSelectedSoulId)
				{
					this.almHasList_cache[i].curUsedNum -= selectedVersionUpItem.NeedNum;
				}
			}
		}
	}

	private void ShowChangeIcon()
	{
		VersionUpItem selectedVersionUpItem = this.cmd_AlMightySelect.SelectedVersionUpItem;
		string id = string.Empty;
		int needNum = selectedVersionUpItem.NeedNum;
		int haveNum;
		if (selectedVersionUpItem.AlmightySoulData != null)
		{
			id = selectedVersionUpItem.AlmightySoulData.soulM.soulId;
			haveNum = selectedVersionUpItem.AlmightySoulData.haveNum;
		}
		else
		{
			id = selectedVersionUpItem.baseSoulData.soulM.soulId;
			haveNum = selectedVersionUpItem.baseSoulData.haveNum;
		}
		if (haveNum < needNum)
		{
			selectedVersionUpItem.lbNum.text = string.Format(StringMaster.GetString("EvolutionNotEnoughFraction"), haveNum, needNum);
		}
		else
		{
			selectedVersionUpItem.lbNum.text = string.Format(StringMaster.GetString("EvolutionEnoughFraction"), haveNum, needNum);
		}
		string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(id);
		Vector3 localScale = selectedVersionUpItem.gameObject.transform.localScale;
		selectedVersionUpItem.gameObject.transform.localScale = Vector2.zero;
		this.LoadObjectASync(evolveItemIconPathByID, selectedVersionUpItem, localScale);
	}
}
