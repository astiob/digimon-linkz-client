using Master;
using Monster;
using System;
using UnityEngine;

public sealed class GUIListPartsDigiGarden : GUIListPartBS
{
	public GameObject goMN_ICON;

	public GameObject goTX_NAME;

	public GameObject goTX_GRADE;

	public GameObject goTX_EXP;

	public GameObject goTX_BTN;

	[SerializeField]
	private GameObject canEvolveParticle;

	private UILabel ngTX_NAME;

	private UILabel ngTX_GRADE;

	private UILabel ngTX_EXP;

	private UILabel ngTX_BTN;

	private MonsterData data;

	private bool isTouchEndFromChild;

	private float restTimeUpdateTime;

	private GameObject goMN_ICON_CHG_2;

	private Vector2 startPostion;

	public MonsterData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngTX_NAME = this.goTX_NAME.GetComponent<UILabel>();
		this.ngTX_GRADE = this.goTX_GRADE.GetComponent<UILabel>();
		this.ngTX_EXP = this.goTX_EXP.GetComponent<UILabel>();
		this.ngTX_BTN = this.goTX_BTN.GetComponent<UILabel>();
		this.canEvolveParticle.SetActive(false);
	}

	public void ChangeSprite(string sprName)
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		if (component != null)
		{
			component.spriteName = sprName;
			component.MakePixelPerfect();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.ShowIcon();
		this.ShowDetail();
	}

	private void ShowIcon()
	{
		if (this.goMN_ICON_CHG_2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.goMN_ICON_CHG_2);
		}
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(this.data, this.goMN_ICON.transform.localScale, this.goMN_ICON.transform.localPosition, this.goMN_ICON.transform.parent, true, false);
		this.goMN_ICON_CHG_2 = guimonsterIcon.gameObject;
		this.goMN_ICON_CHG_2.SetActive(true);
		guimonsterIcon.Data = this.data;
		guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.actMIconLong));
		UIWidget component = this.goMN_ICON.GetComponent<UIWidget>();
		UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
		if (component != null && component2 != null)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(guimonsterIcon.transform, add);
		}
		BoxCollider component4 = this.goMN_ICON.GetComponent<BoxCollider>();
		BoxCollider component5 = guimonsterIcon.GetComponent<BoxCollider>();
		if (component4 != null && component5 != null)
		{
			component5.center = component4.center;
			component5.size = component4.size;
		}
		this.goMN_ICON.SetActive(false);
	}

	private void ShowDetail()
	{
		if (!this.data.userMonster.IsEgg())
		{
			this.ngTX_NAME.text = this.data.monsterMG.monsterName;
			this.ngTX_GRADE.text = MonsterGrowStepData.GetGrowStepName(this.data.monsterMG.growStep);
			if (CMD_DigiGarden.instance != null && CMD_DigiGarden.instance.IsOfflineModeFlag)
			{
				this.ngTX_EXP.text = StringMaster.GetString("Garden-13");
			}
		}
		else
		{
			string text = StringMaster.GetString("CharaStatus-06");
			foreach (GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM2 in MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM)
			{
				if (monsterEvolutionRouteM2.monsterEvolutionRouteId == this.data.userMonster.monsterEvolutionRouteId)
				{
					text = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterEvolutionRouteM2.eggMonsterId).Group.monsterName;
				}
			}
			this.ngTX_NAME.text = text;
			this.ngTX_GRADE.text = StringMaster.GetString("CharaStatus-04");
			this.ngTX_EXP.text = StringMaster.GetString("Garden-08");
			this.canEvolveParticle.SetActive(true);
		}
		if (this.data.userMonster.IsEgg())
		{
			this.ngTX_BTN.text = StringMaster.GetString("Garden-12");
		}
		else
		{
			this.ngTX_BTN.text = StringMaster.GetString("EvolutionTitle");
		}
	}

	private void actMIconLong(MonsterData md)
	{
		CMD_CharacterDetailed.DataChg = md;
		GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null);
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this.startPostion = pos;
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.startPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				CMD_QuestTOP.ChangeSelectA_StageL_S(base.IDX, false);
			}
		}
	}

	private void OnClickedBtnSelect()
	{
		if (this.data.userMonster.IsEgg())
		{
			if (CMD_DigiGarden.instance != null)
			{
				CMD_DigiGarden.instance.BornExec(this.data);
			}
		}
		else if (CMD_DigiGarden.instance != null)
		{
			if (!CMD_DigiGarden.instance.IsOfflineModeFlag)
			{
				CMD_DigiGarden.instance.GrowExec(this.data);
			}
			else
			{
				CMD_DigiGarden.instance.OfflineGrow(this.data);
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateEvolutionRestTime();
	}

	private void UpdateEvolutionRestTime()
	{
		if (!this.canEvolveParticle.activeSelf)
		{
			this.restTimeUpdateTime -= Time.unscaledDeltaTime;
			if (string.IsNullOrEmpty(this.data.userMonster.growEndDate))
			{
				this.canEvolveParticle.SetActive(true);
				this.ngTX_EXP.text = StringMaster.GetString("Garden-10");
			}
			else if (0f >= this.restTimeUpdateTime)
			{
				this.restTimeUpdateTime = 1f;
				DateTime d = DateTime.Parse(this.data.userMonster.growEndDate);
				TimeSpan timeSpan = d - ServerDateTime.Now;
				if (0.0 >= timeSpan.TotalSeconds)
				{
					this.canEvolveParticle.SetActive(true);
					this.ngTX_EXP.text = StringMaster.GetString("Garden-10");
				}
				else
				{
					string arg = string.Empty;
					if (0 < (int)timeSpan.TotalHours)
					{
						arg = string.Format(StringMaster.GetString("SystemTimeHM"), timeSpan.Hours.ToString(), timeSpan.Minutes.ToString());
					}
					else if (0 < timeSpan.Minutes)
					{
						arg = string.Format(StringMaster.GetString("SystemTimeMS"), timeSpan.Minutes.ToString(), timeSpan.Seconds.ToString());
					}
					else
					{
						arg = string.Format(StringMaster.GetString("SystemTimeS"), timeSpan.Seconds.ToString());
					}
					this.ngTX_EXP.text = string.Format(StringMaster.GetString("Garden-09"), arg);
				}
			}
		}
	}
}
