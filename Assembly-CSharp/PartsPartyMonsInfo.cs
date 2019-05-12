using Master;
using Monster;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.PartyEdit;
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

	[Header("右クリップのOBJ")]
	[SerializeField]
	private GameObject goR_CLIP;

	[SerializeField]
	private UISprite leaderMark;

	[Header("クリッピングオブジェクト")]
	[SerializeField]
	private GameObject clipObject;

	[SerializeField]
	private PartyEditQuestBonusButton bonusButton;

	protected QuestBonusPack questBonusPack;

	protected QuestBonusTargetCheck bonusTargetCheck;

	private float minScreenX;

	private float maxScreenX;

	private GUISelectPanelBSLR selectPanelLR;

	private const int RENDER_W = 320;

	private const int RENDER_H = 378;

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
	}

	public virtual void OnTapedMonster()
	{
		if (this.selectPanelLR != null)
		{
			if (!this.selectPanelLR.IsStopRev())
			{
				return;
			}
			this.selectPanelLR.SetScrollSpeed(0f);
		}
		CMD_DeckList.SelectMonsterData = this.Data;
		CMD_DeckList dl = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (this.guiListPartsPartyEdit != null)
			{
				this.guiListPartsPartyEdit.OnChanged();
			}
		}, "CMD_DeckList", new Action<CommonDialog>(this.OnReadyDeckList)) as CMD_DeckList;
		dl.PPMI_Instance = this;
		CMD_BattleNextChoice battleNextChoice = UnityEngine.Object.FindObjectOfType<CMD_BattleNextChoice>();
		if (battleNextChoice != null)
		{
			dl.PartsTitle.SetCloseAct(delegate(int i)
			{
				battleNextChoice.ClosePanel(false);
				CMD_PartyEdit.instance.ClosePanel(false);
				dl.SetCloseAction(delegate(int x)
				{
					CMD_BattleNextChoice.GoToFarm();
				});
				dl.ClosePanel(true);
			});
		}
		dl.SetSortieLimit(this.guiListPartsPartyEdit.partyEdit.GetWorldSortieLimit());
		GUIListPartsPartyEdit guilistPartsPartyEdit = this.guiListPartsPartyEdit;
		dl.ppmiList = guilistPartsPartyEdit.ppmiList;
		this.HideStatusPanel();
	}

	protected void OnReadyDeckList(CommonDialog dialog)
	{
		CMD_DeckList cmd_DeckList = dialog as CMD_DeckList;
		if (null != cmd_DeckList)
		{
			cmd_DeckList.SetQuestBonus(this.questBonusPack, this.bonusTargetCheck);
		}
	}

	private void UpdateActive()
	{
		if (this.selectPanelLR == null && this.guiListPartsPartyEdit.gameObject.transform.parent != null)
		{
			this.selectPanelLR = this.guiListPartsPartyEdit.gameObject.transform.parent.GetComponent<GUISelectPanelBSLR>();
		}
	}

	public void SetQuestBonus(QuestBonusPack questBonus, QuestBonusTargetCheck checker)
	{
		this.questBonusPack = questBonus;
		this.bonusTargetCheck = checker;
		this.bonusButton.Initialize(questBonus, checker);
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
		this.renderTex = this.csRender3DRT.SetRenderTarget(320, 378, 16);
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
				one.x -= 160f;
			}
			one.x *= uiroot.gameObject.transform.localScale.x;
			this.minScreenX = orthoCamera.WorldToScreenPoint(one).x;
			one.x = this.goR_CLIP.transform.localPosition.x;
			component = this.goL_CLIP.GetComponent<UITexture>();
			if (component != null)
			{
				one.x -= (float)(component.width / 2);
				one.x += 160f;
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
		this.bonusButton.SetTargetMonster(this.Data);
		this.bonusButton.SetActive();
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

	public void SpButtonActive(bool active)
	{
		if (null != this.bonusButton)
		{
			this.bonusButton.gameObject.SetActive(active);
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
		array = array.Where((GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM item) => ExtraEffectUtil.CheckExtraStageParams(this.Data, item)).ToArray<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
		int num = 0;
		int num2 = 0;
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillPower, 1, true);
		if (num2 == 1)
		{
			this.gimmickSkillActionUp.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillActionDown.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillHit, 1, true);
		if (num2 == 1)
		{
			this.gimmickSkillActionUp.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillActionDown.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillPower, 2, true);
		if (num2 == 1)
		{
			this.gimmickSkillSucceedUp.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillSucceedDown.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillHit, 2, true);
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
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillPower, 3, true);
		if (num2 == 1)
		{
			this.gimmickSkillSucceedUp2.SetActive(true);
		}
		else if (num2 == -1)
		{
			this.gimmickSkillSucceedDown2.SetActive(true);
		}
		ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, this.Data, array, EffectStatusBase.ExtraEffectType.SkillHit, 3, true);
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
