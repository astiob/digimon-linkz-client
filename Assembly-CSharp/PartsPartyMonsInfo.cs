using Master;
using Monster;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PartsPartyMonsInfo : GUICollider
{
	[Header("チップの処理")]
	[SerializeField]
	protected ChipBaseSelect chipBaseSelect;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterGimmickEffectStatusList monsterGimickEffectStatusList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill monsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill2;

	[SerializeField]
	private UILabel monsterSuccessionSkill2Name;

	[SerializeField]
	private UILabel monsterSuccessionSkill2Info;

	[SerializeField]
	private UILabel monsterSuccessionSkill2AP;

	[SerializeField]
	protected GameObject sortieNG;

	[SerializeField]
	private GameObject goArousal;

	[SerializeField]
	private UISprite spArousal;

	[NonSerialized]
	public GUIListPartsPartyEdit guiListPartsPartyEdit;

	protected StatusPanel statusPanel;

	[SerializeField]
	private UITexture ngTargetTex;

	protected CommonRender3DRT csRender3DRT;

	private RenderTexture renderTex;

	protected Vector3 v3Chara;

	public int statusPage = 1;

	[Header("ステージギミック表記Obj")]
	public GameObject stageGimmickObj;

	[SerializeField]
	private GameObject gimmickSkillActionUp;

	[SerializeField]
	private GameObject gimmickSkillActionDown;

	[SerializeField]
	private GameObject gimmickSkillSucceedUp;

	[SerializeField]
	private GameObject gimmickSkillSucceedDown;

	[SerializeField]
	private GameObject gimmickSkillSucceedUp2;

	[SerializeField]
	private GameObject gimmickSkillSucceedDown2;

	[Header("左クリップのOBJ")]
	[SerializeField]
	private GameObject goL_CLIP;

	[SerializeField]
	[Header("右クリップのOBJ")]
	private GameObject goR_CLIP;

	[SerializeField]
	private UISprite leaderMark;

	[Header("クリッピングオブジェクト")]
	[SerializeField]
	private GameObject clipObject;

	[SerializeField]
	private GameObject spButton;

	private float minScreenX;

	private float maxScreenX;

	private QuestData.WorldDungeonData d_data;

	private List<string> bonusListText = new List<string>();

	private CMD_SPBonusList spBonusPop;

	private GUISelectPanelBSLR selectPanelLR;

	private int RENDER_W = 320;

	private int RENDER_H = 378;

	private int updateCharaCT = 2;

	public MonsterData Data { get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.statusPanel = base.gameObject.GetComponent<StatusPanel>();
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateCharacter();
		this.UpdateActive();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.ReleaseCharacter();
		if (this.spBonusPop != null)
		{
			this.spBonusPop.ClosePanel(true);
		}
	}

	public virtual void OnTapedMonster()
	{
		if (this.selectPanelLR != null && !this.selectPanelLR.IsStopRev())
		{
			return;
		}
		if (this.selectPanelLR != null)
		{
			this.selectPanelLR.SetScrollSpeed(0f);
		}
		global::Debug.Log("================================================== TAP MONS = " + base.gameObject.name);
		CMD_DeckList.SelectMonsterData = this.Data;
		CMD_DeckList cmd_DeckList = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (this.guiListPartsPartyEdit != null)
			{
				this.guiListPartsPartyEdit.OnChanged();
			}
		}, "CMD_DeckList") as CMD_DeckList;
		cmd_DeckList.PPMI_Instance = this;
		cmd_DeckList.SetSortieLimit(this.guiListPartsPartyEdit.partyEdit.GetWorldSortieLimit());
		GUIListPartsPartyEdit guilistPartsPartyEdit = this.guiListPartsPartyEdit;
		cmd_DeckList.ppmiList = guilistPartsPartyEdit.ppmiList;
		this.HideStatusPanel();
	}

	private void UpdateActive()
	{
		if (this.selectPanelLR == null && this.guiListPartsPartyEdit.gameObject.transform.parent != null)
		{
			this.selectPanelLR = this.guiListPartsPartyEdit.gameObject.transform.parent.GetComponent<GUISelectPanelBSLR>();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.statusPanel.InitUI();
		this.CalcScreenClip();
		this.ShowCharacter();
		this.ShowRare();
		this.HideStatusPanel();
	}

	public void SetLeaderMark(bool active)
	{
		this.leaderMark.enabled = active;
	}

	public void ShowRare()
	{
		if (this.Data != null)
		{
			GUIMonsterIcon.DispArousal(this.Data.monsterM.rare, this.goArousal, this.spArousal);
		}
		else
		{
			this.goArousal.SetActive(false);
		}
	}

	public virtual void SetRenderPos()
	{
		if (this.csRender3DRT != null)
		{
			UnityEngine.Object.DestroyImmediate(this.csRender3DRT.gameObject);
			this.csRender3DRT = null;
		}
		else
		{
			this.v3Chara = CMD_PartyEdit.instance.Get3DPos();
		}
	}

	public void ReleaseCharacter()
	{
		if (this.csRender3DRT != null)
		{
			UnityEngine.Object.DestroyImmediate(this.csRender3DRT.gameObject);
			this.csRender3DRT = null;
			this.renderTex = null;
			this.ngTargetTex.mainTexture = null;
		}
	}

	public void ShowCharacter()
	{
		if (this.Data == null)
		{
			this.ngTargetTex.gameObject.SetActive(false);
			this.ShowDet();
			return;
		}
		this.SetRenderPos();
		GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DRT", null);
		this.csRender3DRT = gameObject.GetComponent<CommonRender3DRT>();
		string filePath = MonsterObject.GetFilePath(this.Data.GetMonsterMaster().Group.modelId);
		this.csRender3DRT.LoadChara(filePath, this.v3Chara.x, this.v3Chara.y, 0.1f, 0f, false);
		this.renderTex = this.csRender3DRT.SetRenderTarget(this.RENDER_W, this.RENDER_H, 16);
		this.ngTargetTex.gameObject.SetActive(true);
		this.ngTargetTex.mainTexture = this.renderTex;
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterMG = this.Data.monsterMG;
		float partyCharaPosX = 0f;
		float partyCharaPosY = 0f;
		float partyCharaPosZ = 0f;
		float partyCharaRotY = 0f;
		if (!string.IsNullOrEmpty(monsterMG.partyCharaPosX))
		{
			partyCharaPosX = (float)double.Parse(monsterMG.partyCharaPosX);
		}
		if (!string.IsNullOrEmpty(monsterMG.partyCharaPosY))
		{
			partyCharaPosY = (float)double.Parse(monsterMG.partyCharaPosY);
		}
		if (!string.IsNullOrEmpty(monsterMG.partyCharaPosZ))
		{
			partyCharaPosZ = (float)double.Parse(monsterMG.partyCharaPosZ);
		}
		if (!string.IsNullOrEmpty(monsterMG.partyCharaRotY))
		{
			partyCharaRotY = (float)double.Parse(monsterMG.partyCharaRotY);
		}
		this.csRender3DRT.SetCharacterPositionForParty(partyCharaPosX, partyCharaPosY, partyCharaPosZ, partyCharaRotY);
		this.updateCharaCT = 2;
		this.ShowDet();
	}

	protected void UpdateCharacter()
	{
		if (this.Data != null)
		{
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterMG = this.Data.monsterMG;
			float partyCharaPosX = 0f;
			float partyCharaPosY = 0f;
			float partyCharaPosZ = 0f;
			float partyCharaRotY = 0f;
			if (!string.IsNullOrEmpty(monsterMG.partyCharaPosX))
			{
				partyCharaPosX = (float)double.Parse(monsterMG.partyCharaPosX);
			}
			if (!string.IsNullOrEmpty(monsterMG.partyCharaPosY))
			{
				partyCharaPosY = (float)double.Parse(monsterMG.partyCharaPosY);
			}
			if (!string.IsNullOrEmpty(monsterMG.partyCharaPosZ))
			{
				partyCharaPosZ = (float)double.Parse(monsterMG.partyCharaPosZ);
			}
			if (!string.IsNullOrEmpty(monsterMG.partyCharaRotY))
			{
				partyCharaRotY = (float)double.Parse(monsterMG.partyCharaRotY);
			}
			if (this.csRender3DRT != null)
			{
				if (this.csRender3DRT.party_show_type == CommonRender3DRT.PARTY_SHOW_TYPE.CUSTOM)
				{
					this.csRender3DRT.SetCharacterPositionForParty(partyCharaPosX, partyCharaPosY, partyCharaPosZ, partyCharaRotY);
				}
				else if (this.updateCharaCT > 0)
				{
					this.csRender3DRT.SetCharacterPositionForParty(partyCharaPosX, partyCharaPosY, partyCharaPosZ, partyCharaRotY);
					this.updateCharaCT--;
				}
				else
				{
					Vector3 position = base.transform.position;
					Camera orthoCamera = GUIMain.GetOrthoCamera();
					Vector3 vector = orthoCamera.WorldToScreenPoint(position);
					if (this.minScreenX <= vector.x && vector.x <= this.maxScreenX)
					{
						if (!this.csRender3DRT.gameObject.activeSelf)
						{
							this.csRender3DRT.gameObject.SetActive(true);
							this.csRender3DRT.ResumeAnimation();
						}
					}
					else if (this.csRender3DRT.gameObject.activeSelf)
					{
						this.csRender3DRT.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	private void CalcScreenClip()
	{
		UIRoot uiroot = GUIMain.GetUIRoot();
		Camera orthoCamera = GUIMain.GetOrthoCamera();
		if (this.goL_CLIP != null && this.goR_CLIP != null)
		{
			Vector3 one = Vector3.one;
			one.x = this.goL_CLIP.transform.localPosition.x;
			UITexture component = this.goL_CLIP.GetComponent<UITexture>();
			if (component != null)
			{
				one.x += (float)(component.width / 2);
				one.x -= (float)this.RENDER_W / 2f;
			}
			one.x *= uiroot.gameObject.transform.localScale.x;
			this.minScreenX = orthoCamera.WorldToScreenPoint(one).x;
			one.x = this.goR_CLIP.transform.localPosition.x;
			component = this.goL_CLIP.GetComponent<UITexture>();
			if (component != null)
			{
				one.x -= (float)(component.width / 2);
				one.x += (float)this.RENDER_W / 2f;
			}
			one.x *= uiroot.gameObject.transform.localScale.x;
			this.maxScreenX = orthoCamera.WorldToScreenPoint(one).x;
		}
		else
		{
			this.minScreenX = 0f;
			this.maxScreenX = (float)orthoCamera.pixelWidth;
		}
	}

	protected virtual void SetSelectedCharChg(MonsterData monsterData)
	{
		int[] chipIdList = monsterData.GetChipEquip().GetChipIdList();
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage slotStatus = monsterData.GetChipEquip().GetSlotStatus();
		int maxSlotNum = 0;
		if (slotStatus != null)
		{
			maxSlotNum = slotStatus.maxSlotNum + slotStatus.maxExtraSlotNum;
		}
		this.chipBaseSelect.SetSelectedCharChg(chipIdList, true, maxSlotNum);
	}

	private void ShowDet()
	{
		if (this.Data == null)
		{
			this.chipBaseSelect.ClearChipIcons();
			this.monsterGimickEffectStatusList.ClearValues();
			if (this.sortieNG.activeSelf)
			{
				this.sortieNG.SetActive(false);
			}
			return;
		}
		this.SetSelectedCharChg(this.Data);
		this.monsterLeaderSkill.SetSkill(this.Data);
		this.monsterUniqueSkill.SetSkill(this.Data);
		this.monsterSuccessionSkill.SetSkill(this.Data);
		bool flag = MonsterStatusData.IsVersionUp(this.Data.GetMonsterMaster().Simple.rare);
		if (flag)
		{
			if (this.monsterSuccessionSkill2 != null)
			{
				this.monsterSuccessionSkill2.gameObject.SetActive(true);
				if (this.Data.GetExtraCommonSkill() != null)
				{
					this.monsterSuccessionSkill2.SetSkill(this.Data);
				}
				else
				{
					this.monsterSuccessionSkill2.ClearSkill();
					this.monsterSuccessionSkill2Name.text = StringMaster.GetString("SystemNone");
					this.monsterSuccessionSkill2Info.text = StringMaster.GetString("CharaStatus-02");
					this.monsterSuccessionSkill2AP.text = string.Format(StringMaster.GetString("BattleSkillUI-01"), 0);
				}
			}
		}
		else if (this.monsterSuccessionSkill2 != null)
		{
			this.monsterSuccessionSkill2.gameObject.SetActive(false);
		}
		this.statusPanel.ResetUI();
		this.monsterBasicInfo.SetMonsterData(this.Data);
		this.monsterMedalList.SetValues(this.Data.userMonster);
		GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] gimmickEffectArray = this.SetStageGimmick();
		string value = string.Empty;
		string text = string.Empty;
		string text2 = string.Empty;
		if (CMD_QuestTOP.instance != null)
		{
			if (CMD_QuestTOP.instance.StageDataBk != null)
			{
				text = CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldStageId;
				text2 = CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldDungeonId;
				GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
				foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
				{
					if (text == worldStageM2.worldStageId)
					{
						value = worldStageM2.worldAreaId;
						break;
					}
				}
			}
		}
		else if (CMD_MultiRecruitPartyWait.Instance != null)
		{
			if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				value = CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldAreaId;
				text = CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldStageId;
				text2 = CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldDungeonId;
			}
			else
			{
				value = CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.worldAreaId;
				text = CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.worldStageId;
				text2 = CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.worldDungeonId;
			}
		}
		else if (CMD_QuestTOP.instance == null && CMD_MultiRecruitPartyWait.Instance == null)
		{
			value = CMD_PartyEdit.replayMultiAreaId;
			text = CMD_PartyEdit.replayMultiStageId;
			text2 = CMD_PartyEdit.replayMultiDungeonId;
		}
		this.bonusListText.Clear();
		if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
		{
			this.spButton.SetActive(false);
		}
		else if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			this.spButton.SetActive(false);
		}
		else if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
		{
			List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();
			GameWebAPI.RespDataMA_EventPointBonusM respDataMA_EventPointBonusMaster = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster;
			for (int j = 0; j < respDataMA_EventPointBonusMaster.eventPointBonusM.Length; j++)
			{
				if (respDataMA_EventPointBonusMaster.eventPointBonusM[j].worldDungeonId.Equals(text2) && respDataMA_EventPointBonusMaster.eventPointBonusM[j].effectType != "0")
				{
					list.Add(respDataMA_EventPointBonusMaster.eventPointBonusM[j]);
				}
			}
			List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> extraEffectDataList = DataMng.Instance().StageGimmick.GetExtraEffectDataList(text, text2);
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectM = MasterDataMng.Instance().RespDataMA_ChipEffectMaster.chipEffectM;
			List<string> list2 = new List<string>();
			List<GameWebAPI.RespDataMA_ChipM.Chip> list3 = new List<GameWebAPI.RespDataMA_ChipM.Chip>();
			List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list4 = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
			if (chipEffectM.Length > 0)
			{
				for (int k = 0; k < chipEffectM.Length; k++)
				{
					if (chipEffectM[k].effectTrigger.Equals("11") && chipEffectM[k].effectTriggerValue.Equals(value))
					{
						GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(chipEffectM[k].chipId);
						if (chipMainData != null && !list2.Contains(chipEffectM[k].chipId))
						{
							list4.Add(chipEffectM[k]);
							list3.Add(chipMainData);
							list2.Add(chipEffectM[k].chipId);
						}
					}
				}
			}
			int[] chipIdList = this.Data.GetChipEquip().GetChipIdList();
			List<int> list5 = new List<int>();
			list5.AddRange(chipIdList);
			list5.Sort((int a, int b) => a - b);
			List<GameWebAPI.RespDataMA_ChipM.Chip> list6 = new List<GameWebAPI.RespDataMA_ChipM.Chip>();
			for (int l = 0; l < list5.Count; l++)
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData2 = ChipDataMng.GetChipMainData(list5[l].ToString());
				list6.Add(chipMainData2);
				for (int m = 0; m < list3.Count; m++)
				{
					if (list5[l] == int.Parse(list3[m].chipId))
					{
						GameWebAPI.RespDataMA_ChipM.Chip chipMainData3 = ChipDataMng.GetChipMainData(list3[m].chipId);
						if (chipMainData3 != null)
						{
							this.bonusListText.Add(chipMainData3.name);
						}
					}
				}
			}
			list.Sort((GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus a, GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus b) => int.Parse(a.targetSubType) - int.Parse(b.targetSubType));
			extraEffectDataList.Sort((GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM a, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM b) => int.Parse(a.targetSubType) - int.Parse(b.targetSubType));
			List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list7 = list.FindAll((GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus a) => a.targetSubType.Equals("6"));
			list.RemoveAll((GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus a) => a.targetSubType.Equals("6"));
			List<string> list8 = new List<string>();
			for (int n = 0; n < list7.Count; n++)
			{
				bool flag2 = true;
				for (int num = 0; num < list4.Count; num++)
				{
					if (list4[num].chipId.Equals(list7[n].targetValue))
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					for (int num2 = 0; num2 < list6.Count; num2++)
					{
						GameWebAPI.RespDataMA_ChipM.Chip chipMainData4 = ChipDataMng.GetChipMainData(list7[n].targetValue);
						if (chipMainData4 != null && float.Parse(list7[n].effectValue) >= 0f && chipMainData4.chipId.Equals(list6[num2].chipId) && !list8.Contains(chipMainData4.chipId))
						{
							this.bonusListText.Add(chipMainData4.name);
							list8.Add(chipMainData4.chipId);
						}
					}
				}
			}
			for (int num3 = 0; num3 < list.Count; num3++)
			{
				bool flag3 = ExtraEffectUtil.CheckExtraParams(this.Data, list[num3]);
				if (flag3)
				{
					this.bonusListText.Add(list[num3].detail);
				}
			}
			for (int num4 = 0; num4 < extraEffectDataList.Count; num4++)
			{
				bool flag4 = ExtraEffectUtil.CheckExtraStageParams(this.Data, extraEffectDataList[num4]);
				if (flag4)
				{
					this.bonusListText.Add(extraEffectDataList[num4].detail);
				}
			}
			if (this.bonusListText.Count > 0)
			{
				this.spButton.SetActive(true);
			}
			else
			{
				this.spButton.SetActive(false);
			}
		}
		this.monsterGimickEffectStatusList.SetValues(this.Data, gimmickEffectArray);
		this.monsterResistanceList.SetValues(this.Data);
		this.SetSortieLimitCondition(this.Data);
	}

	protected virtual void SetSortieLimitCondition(MonsterData monsterData)
	{
		bool flag = true;
		List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> worldSortieLimit = this.guiListPartsPartyEdit.partyEdit.GetWorldSortieLimit();
		if (worldSortieLimit != null && 0 < worldSortieLimit.Count)
		{
			string tribe = monsterData.monsterMG.tribe;
			string growStep = monsterData.monsterMG.growStep;
			flag = ClassSingleton<QuestData>.Instance.CheckSortieLimit(worldSortieLimit, tribe, growStep);
		}
		if (!flag)
		{
			if (!this.sortieNG.activeSelf)
			{
				this.sortieNG.SetActive(true);
			}
		}
		else if (this.sortieNG.activeSelf)
		{
			this.sortieNG.SetActive(false);
		}
	}

	public void HideStatusPanel()
	{
		this.statusPanel.ResetUI();
	}

	public void SpEffectButton()
	{
		this.spBonusPop = (GUIMain.ShowCommonDialog(null, "CMD_SPBonusList") as CMD_SPBonusList);
		if (this.spBonusPop != null)
		{
			this.spBonusPop.SetViewData(this.bonusListText);
		}
	}

	public void SpButtonActive(bool active)
	{
		if (this.spButton != null)
		{
			this.spButton.SetActive(active);
		}
	}

	private void StatusPageChangeTap()
	{
		this.StatusPageChange();
	}

	public void StatusPageChange()
	{
		if (this.Data != null)
		{
			this.statusPanel.SetNextPage();
		}
	}

	public void SetStatusPage(int pageNo)
	{
		if (this.Data != null)
		{
			this.statusPanel.SetPage(pageNo);
		}
	}

	public int GetStatusPage()
	{
		return this.statusPanel.GetPageNo();
	}

	private GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] SetStageGimmick()
	{
		GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] result = null;
		string stageID = string.Empty;
		string dungeonID = string.Empty;
		this.StageGimmickAllFalse();
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			this.StageGimmickAllFalse();
			return result;
		}
		if (CMD_MultiRecruitPartyWait.StageDataBk != null)
		{
			stageID = CMD_MultiRecruitPartyWait.StageDataBk.worldStageId;
			dungeonID = CMD_MultiRecruitPartyWait.StageDataBk.worldDungeonId;
			if (DataMng.Instance().StageGimmick.ContainsDungeon(stageID, dungeonID) && DataMng.Instance().StageGimmick.IsMatch(stageID, dungeonID, this.Data))
			{
				result = this.SetStageGimmick(stageID, dungeonID);
			}
			else
			{
				this.StageGimmickAllFalse();
			}
		}
		else if (CMD_QuestTOP.instance != null && CMD_QuestTOP.instance.StageDataBk != null)
		{
			stageID = CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldStageId;
			dungeonID = CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldDungeonId;
			if (DataMng.Instance().StageGimmick.ContainsDungeon(stageID, dungeonID) && DataMng.Instance().StageGimmick.IsMatch(stageID, dungeonID, this.Data))
			{
				result = this.SetStageGimmick(stageID, dungeonID);
			}
			else
			{
				this.StageGimmickAllFalse();
			}
		}
		else if (CMD_QuestTOP.instance == null && CMD_MultiRecruitPartyWait.StageDataBk == null)
		{
			stageID = CMD_PartyEdit.replayMultiStageId;
			dungeonID = CMD_PartyEdit.replayMultiDungeonId;
			if (DataMng.Instance().StageGimmick.ContainsDungeon(stageID, dungeonID) && DataMng.Instance().StageGimmick.IsMatch(stageID, dungeonID, this.Data))
			{
				result = this.SetStageGimmick(stageID, dungeonID);
			}
			else
			{
				this.StageGimmickAllFalse();
			}
		}
		return result;
	}

	private void StageGimmickAllFalse()
	{
		this.stageGimmickObj.SetActive(false);
		this.gimmickSkillActionUp.SetActive(false);
		this.gimmickSkillActionDown.SetActive(false);
		this.gimmickSkillSucceedUp.SetActive(false);
		this.gimmickSkillSucceedDown.SetActive(false);
	}

	private GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] SetStageGimmick(string StageID, string DungeonID)
	{
		GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] array = DataMng.Instance().StageGimmick.GetExtraEffectDataList(StageID, DungeonID).ToArray();
		int num = 0;
		int num2 = 0;
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillPower, 1);
		if (num2 == 1)
		{
			this.gimmickSkillActionUp.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillActionDown.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillHit, 1);
		if (num2 == 1)
		{
			this.gimmickSkillActionUp.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillActionDown.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillPower, 2);
		if (num2 == 1)
		{
			this.gimmickSkillSucceedUp.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillSucceedDown.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillHit, 2);
		if (num2 == 1)
		{
			this.gimmickSkillSucceedUp.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillSucceedDown.SetActive(true);
		}
		if (this.gimmickSkillSucceedUp2 == null || this.gimmickSkillSucceedDown2 == null)
		{
			return array;
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillPower, 3);
		if (num2 == 1)
		{
			this.gimmickSkillSucceedUp2.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillSucceedDown2.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillHit, 3);
		if (num2 == 1)
		{
			this.gimmickSkillSucceedUp2.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillSucceedDown2.SetActive(true);
		}
		return array;
	}

	public void HideClips()
	{
		NGUITools.SetActiveSelf(this.clipObject, false);
	}
}
