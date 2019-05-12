using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelMonsterIcon : GUISelectPanelBSPartsUD
{
	public static GUISelectPanelMonsterIcon instance;

	private float valueA_Min = -1f;

	private float valueA_Max = 1f;

	private float changeTime = 0.4f;

	private float stayTime = 1.1f;

	private float valueA;

	public int PARTS_CT_MN = 4;

	private Action<MonsterData> actL_bak;

	private Action<MonsterData> actS_bak;

	private Vector3 vScl_bak;

	private Func<MonsterData, bool> actionCheckEnablePush;

	public CMD_BaseSelect.BASE_TYPE BaseType { private get; set; }

	protected override void Awake()
	{
		base.Awake();
		GUISelectPanelMonsterIcon.instance = this;
		this.StartUp_VA();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (GUISelectPanelMonsterIcon.instance == this)
		{
			GUISelectPanelMonsterIcon.instance = null;
		}
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateIcons();
	}

	private void StartUp_VA()
	{
		this.valueA = 0f;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", this.valueA);
		hashtable.Add("to", this.valueA_Max);
		hashtable.Add("time", this.changeTime / 2f);
		hashtable.Add("onupdate", "UpdateValueA");
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("oncomplete", "EndValueA_WT");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void UpdateValueA(float vl)
	{
		this.valueA = vl;
	}

	private void EndValueA_WT(int id)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", this.valueA);
		hashtable.Add("to", this.valueA);
		hashtable.Add("time", this.stayTime);
		hashtable.Add("onupdate", "UpdateValueA");
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("oncomplete", "EndValueA_CHG");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void EndValueA_CHG(int id)
	{
		Hashtable hashtable = new Hashtable();
		if (this.valueA > 0.5f)
		{
			hashtable.Add("from", this.valueA_Max);
			hashtable.Add("to", this.valueA_Min);
		}
		else
		{
			hashtable.Add("from", this.valueA_Min);
			hashtable.Add("to", this.valueA_Max);
		}
		hashtable.Add("time", this.changeTime);
		hashtable.Add("onupdate", "UpdateValueA");
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("oncomplete", "EndValueA_WT");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void UpdateIcons()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIMonsterIcon guimonsterIcon = (GUIMonsterIcon)this.partObjs[i];
			if (guimonsterIcon.gameObject.activeSelf)
			{
				if (guimonsterIcon.SortMess != string.Empty && guimonsterIcon.LevelMess != string.Empty)
				{
					if (this.valueA > 0f)
					{
						guimonsterIcon.LevelMessAlpha(this.valueA);
						guimonsterIcon.SortMessAlpha(0f);
					}
					else
					{
						guimonsterIcon.LevelMessAlpha(0f);
						guimonsterIcon.SortMessAlpha(-this.valueA);
					}
				}
				else
				{
					guimonsterIcon.LevelMessAlpha(1f);
					guimonsterIcon.SortMessAlpha(1f);
				}
			}
		}
	}

	public void AllBuild(List<MonsterData> monsterDataList, Vector3 vScl, Action<MonsterData> actL = null, Action<MonsterData> actS = null, bool rebuild = false)
	{
		if (!rebuild)
		{
			this.actL_bak = actL;
			this.actS_bak = actS;
			foreach (MonsterData md in monsterDataList)
			{
				GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(md);
				monsterCS_ByMonsterData.SetTouchAct_S(null);
			}
		}
		MonsterDataMng.Instance().PushBackAllMonsterPrefab();
		base.InitScrollBar();
		base.SetSelectPanelParam();
		this.partsCount = monsterDataList.Count;
		this.partObjs = new List<GUIListPartBS>();
		this.vScl_bak = vScl;
		if (this.partsCount == 0)
		{
			base.height = 0f;
			base.InitMinMaxLocation(-1, 0f);
			return;
		}
		GameObject monsterPrefabByMonsterData = MonsterDataMng.Instance().GetMonsterPrefabByMonsterData(monsterDataList[0]);
		int num = this.partsCount / this.PARTS_CT_MN;
		if (this.partsCount % this.PARTS_CT_MN > 0)
		{
			num++;
		}
		base.selectParts = monsterPrefabByMonsterData;
		GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(this.PARTS_CT_MN, num, vScl.x, vScl.y);
		float num2 = panelBuildData.startY;
		float startX = panelBuildData.startX;
		int num3 = 0;
		UIWidget component = base.gameObject.GetComponent<UIWidget>();
		GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray = null;
		if (CMD_MultiRecruitPartyWait.StageDataBk != null)
		{
			effectArray = ExtraEffectUtil.GetExtraEffectArray(CMD_MultiRecruitPartyWait.StageDataBk.worldDungeonId);
		}
		else if (CMD_QuestTOP.instance != null && CMD_QuestTOP.instance.StageDataBk != null)
		{
			effectArray = ExtraEffectUtil.GetExtraEffectArray(CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldDungeonId);
		}
		foreach (MonsterData monsterData in monsterDataList)
		{
			GameObject monsterPrefabByMonsterData2 = MonsterDataMng.Instance().GetMonsterPrefabByMonsterData(monsterData);
			GUIMonsterIcon monsterCS_ByMonsterData2 = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(monsterData);
			this.partObjs.Add(monsterCS_ByMonsterData2);
			monsterPrefabByMonsterData2.SetActive(true);
			vScl.z = 1f;
			monsterPrefabByMonsterData2.transform.parent = base.transform;
			monsterPrefabByMonsterData2.transform.localScale = vScl;
			float x = startX + panelBuildData.pitchW * (float)(num3 % this.PARTS_CT_MN);
			monsterCS_ByMonsterData2.SetOriginalPos(new Vector3(x, num2, -5f));
			monsterCS_ByMonsterData2.SetParent();
			monsterCS_ByMonsterData2.Gimmick = ExtraEffectUtil.IsExtraEffectMonster(monsterData, effectArray);
			if (component != null)
			{
				DepthController component2 = monsterPrefabByMonsterData2.GetComponent<DepthController>();
				UIWidget component3 = monsterPrefabByMonsterData2.GetComponent<UIWidget>();
				if (component2 != null && component3 != null)
				{
					component2.AddWidgetDepth(monsterPrefabByMonsterData2.transform, component.depth + 10 - component3.depth);
				}
			}
			monsterCS_ByMonsterData2.SetTouchAct_L(actL);
			if (!rebuild || monsterCS_ByMonsterData2.GetTouchAct_S() == null)
			{
				if (this.IsEnablePush(monsterData))
				{
					monsterCS_ByMonsterData2.SetTouchAct_S(actS);
				}
				else
				{
					monsterCS_ByMonsterData2.SetTouchAct_S(null);
				}
			}
			num3++;
			if (num3 % this.PARTS_CT_MN == 0)
			{
				num2 -= panelBuildData.pitchH;
			}
		}
		base.height = panelBuildData.lenH;
		base.InitMinMaxLocation(-1, 0f);
		base.UpdateActive();
	}

	public void ReAllBuild(List<MonsterData> dts)
	{
		this.AllBuild(dts, this.vScl_bak, this.actL_bak, this.actS_bak, true);
	}

	public void SetIconSortieLimitParts(List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList)
	{
		if (limitList == null || limitList.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIMonsterIcon guimonsterIcon = this.partObjs[i] as GUIMonsterIcon;
			if (null != guimonsterIcon)
			{
				MonsterData data = guimonsterIcon.Data;
				if (data != null && !ClassSingleton<QuestData>.Instance.CheckSortieLimit(limitList, data.monsterMG.tribe, data.monsterMG.growStep))
				{
					guimonsterIcon.SetCenterText(StringMaster.GetString("PartySortieLimitNG"), GUIMonsterIcon.DimmMessColorType.SORTIE_LIMIT);
					guimonsterIcon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
					guimonsterIcon.SetTouchAct_S(null);
				}
			}
		}
	}

	public bool IsEnablePush(MonsterData monsterData)
	{
		bool result = true;
		if (this.actionCheckEnablePush != null)
		{
			result = this.actionCheckEnablePush(monsterData);
		}
		return result;
	}

	public void SetCheckEnablePushAction(Func<MonsterData, bool> action)
	{
		this.actionCheckEnablePush = action;
	}
}
