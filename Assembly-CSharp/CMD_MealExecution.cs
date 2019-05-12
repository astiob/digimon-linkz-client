using Master;
using Monster;
using System;
using System.Collections;
using UnityEngine;

public class CMD_MealExecution : CMD
{
	private const string SP_NAME_MEAL_PLUS = "Common02_Meal_UP";

	private const string SP_NAME_MEAL_MINUS = "Common02_Meal_Down";

	private const string SP_NAME_MEAL_PLUS_GRAY = "Common02_Meal_UP_G";

	private const string SP_NAME_MEAL_MINUS_GRAY = "Common02_Meal_Down_G";

	[SerializeField]
	private UISprite rightUpBG;

	[SerializeField]
	private Transform LevelUpRoot;

	[SerializeField]
	private UILabel tipsLabel;

	[SerializeField]
	private UILabel consumptionScheduleNum;

	private int execMeatNum;

	[SerializeField]
	private UILabel consumptionLevelUpLabel;

	private int consumptionLevelUpCount;

	[SerializeField]
	private UISprite meatNumDownButtonSprite;

	[SerializeField]
	private GUICollider meatNumDownButtonCollider;

	[SerializeField]
	private UISprite meatNumUpButtonSprite;

	[SerializeField]
	private GUICollider meatNumUpButtonCollider;

	[SerializeField]
	private MonsterBasicInfoExpGauge monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	[SerializeField]
	private UILabel labelBtnExec;

	[SerializeField]
	private UILabel ngTX_MEAT;

	[SerializeField]
	private GUICollider clBtnExec;

	[SerializeField]
	private UISprite spriteBtnExec;

	[SerializeField]
	private StatusUpAnimation statusAnime;

	[SerializeField]
	private UILabel btnExecText;

	[SerializeField]
	[Header("高級肉関連")]
	private UILabel lbHQMeatNum;

	[SerializeField]
	private UILabel lbBtnHQMeatExec;

	[SerializeField]
	private UISprite spBtnHQMeat;

	[SerializeField]
	private BoxCollider coBtnHQMeat;

	[SerializeField]
	private Animator hqMeatAnim;

	[SerializeField]
	private GameObject goTargetTex;

	private bool isExecHQMeat;

	private UITexture ngTargetTex;

	private CommonRender3DRT csRender3DRT;

	private RenderTexture renderTex;

	private CharacterParams charaParams;

	private bool isLockClose;

	private Action actionLevelUp;

	private bool dontExec;

	[SerializeField]
	private PartsUpperCutinController cutinController;

	[SerializeField]
	private UILabel baseChangeBtnText;

	private static DataMng.ExperienceInfo last_exp_info_bk;

	private static int execMeatNum_bk;

	private static MonsterData monsterdata_bk;

	private bool isEating;

	private bool isMeatExCTOver;

	[SerializeField]
	private IncreaseJumpingMeat meat;

	private int updateExCT;

	private bool IsLockClose
	{
		get
		{
			return this.isLockClose;
		}
		set
		{
			this.isLockClose = value;
			if (value)
			{
				GUICollider.DisableAllCollider(base.gameObject.name);
			}
			else
			{
				GUICollider.EnableAllCollider(base.gameObject.name);
			}
		}
	}

	public bool DontExec
	{
		get
		{
			return this.dontExec;
		}
		set
		{
			this.dontExec = value;
		}
	}

	public static MonsterData DataChg { get; set; }

	public static Action UpdateParamCallback { private get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.tipsLabel.text = StringMaster.GetString("MealCost");
		this.btnExecText.text = StringMaster.GetString("MealButtonText");
		this.baseChangeBtnText.text = StringMaster.GetString("BaseChange");
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_MealExecution.DataChg = null;
		if (this.csRender3DRT != null)
		{
			UnityEngine.Object.DestroyImmediate(this.csRender3DRT.gameObject);
		}
		GUIPlayerStatus.RefreshParams_S(false);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		base.PartsTitle.SetTitle(StringMaster.GetString("MealTitle"));
		this.ShowChgInfo();
		this.CheckMeat();
		this.InitBtnHQMeat();
		base.SetTutorialAnyTime("anytime_second_tutorial_meal");
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateExecute();
	}

	private void EndExecSuccess()
	{
		if (this.execMeatNum > 0)
		{
			GooglePlayGamesTool.Instance.Meal(this.execMeatNum);
		}
		base.StartCoroutine(this.PlayGiftMeatAnimation(delegate
		{
			DataMng.Instance().US_PlayerInfoSubMeatNum(CMD_MealExecution.execMeatNum_bk);
			MonsterUserData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(CMD_MealExecution.monsterdata_bk.userMonster.userMonsterId);
			GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList monster = userMonster.GetMonster();
			monster.ex = CMD_MealExecution.last_exp_info_bk.exp.ToString();
			monster.level = CMD_MealExecution.last_exp_info_bk.lev.ToString();
			CMD_MealExecution.monsterdata_bk = null;
			this.execMeatNum = 0;
			this.consumptionScheduleNum.text = "0";
			this.consumptionLevelUpCount = 0;
			this.consumptionLevelUpLabel.text = string.Empty;
			this.SetStatus();
			this.IsLockClose = false;
			this.meatNumDownButtonSprite.spriteName = "Common02_Meal_Down_G";
			this.meatNumDownButtonCollider.activeCollider = false;
			if (!this.meatNumUpButtonCollider.activeCollider)
			{
				this.meatNumUpButtonSprite.spriteName = "Common02_Meal_UP";
				this.meatNumUpButtonCollider.activeCollider = true;
			}
			DataMng.ExperienceInfo expInfo = this.GetExpInfo();
			if (expInfo.lev >= int.Parse(CMD_MealExecution.DataChg.monsterM.maxLevel))
			{
				this.meatNumUpButtonSprite.spriteName = "Common02_Meal_UP_G";
				this.meatNumUpButtonCollider.activeCollider = false;
			}
			this.InitBtnHQMeat();
		}));
		RestrictionInput.EndLoad();
	}

	private void EndExecFailed(Exception noop)
	{
		this.clBtnExec.activeCollider = true;
		this.spriteBtnExec.spriteName = "Common02_Btn_Red";
		this.labelBtnExec.color = Color.white;
		this.IsLockClose = false;
		RestrictionInput.EndLoad();
	}

	private void BaseClose()
	{
		this.CloseAndFarmCamOn(true);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.dontExec)
		{
			this.CloseAndFarmCamOn(animation);
		}
		else
		{
			if (this.IsLockClose)
			{
				return;
			}
			if (CMD_BaseSelect.instance != null)
			{
				CMD_BaseSelect.instance.ChipNumUpdate();
				if (CMD_Training_Menu.instance != null)
				{
					CMD_Training_Menu.instance.ShowDatas();
				}
			}
			if (CMD_BaseSelect.instance != null)
			{
				CMD_MealExecution.UpdateParamCallback();
			}
			this.CloseAndFarmCamOn(animation);
		}
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	private void ShowChgInfo()
	{
		if (CMD_MealExecution.DataChg != null)
		{
			this.ShowCharacter();
			this.SetStatus();
			this.meatNumDownButtonSprite.spriteName = "Common02_Meal_Down_G";
			this.meatNumDownButtonCollider.activeCollider = false;
		}
	}

	private void ShowCharacter()
	{
		this.ngTargetTex = this.goTargetTex.GetComponent<UITexture>();
		GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DRT", null);
		this.csRender3DRT = gameObject.GetComponent<CommonRender3DRT>();
		string filePath = MonsterObject.GetFilePath(CMD_MealExecution.DataChg.GetMonsterMaster().Group.modelId);
		this.csRender3DRT.LoadChara(filePath, 0f, 4000f, -0.65f, 1.1f, true);
		this.renderTex = this.csRender3DRT.SetRenderTarget(1136, 820, 16);
		this.ngTargetTex.mainTexture = this.renderTex;
		this.charaParams = this.csRender3DRT.transform.GetComponentInChildren<CharacterParams>();
	}

	private void SetStatus()
	{
		DataMng.ExperienceInfo expInfo = this.GetExpInfo();
		this.statusAnime.Initialize(CMD_MealExecution.DataChg.userMonster, expInfo.lev);
		this.monsterBasicInfo.SetMonsterData(CMD_MealExecution.DataChg, expInfo);
		this.monsterStatusList.SetValues(CMD_MealExecution.DataChg, false);
		this.monsterResistanceList.SetValues(CMD_MealExecution.DataChg);
		this.monsterMedalList.SetValues(CMD_MealExecution.DataChg.userMonster);
		int nowMeatNum = this.GetNowMeatNum();
		this.ngTX_MEAT.text = nowMeatNum.ToString();
		int userItemNumByItemId = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(50001);
		this.lbHQMeatNum.text = userItemNumByItemId.ToString();
	}

	private void OnExecute(bool IsCountUp, int VariationValue = 1)
	{
		if (this.IsLockClose)
		{
			return;
		}
		int nowMeatNum = this.GetNowMeatNum();
		if (IsCountUp)
		{
			if (nowMeatNum == 0 && VariationValue == 1 && this.updateExCT != 0)
			{
				this.isMeatExCTOver = true;
				this.meatNumUpButtonCollider.isTouching = false;
				return;
			}
			if (nowMeatNum - VariationValue < 0 && this.updateExCT != 0 && !this.isMeatExCTOver)
			{
				VariationValue = nowMeatNum;
				this.isMeatExCTOver = true;
				this.meatNumUpButtonCollider.isTouching = false;
			}
		}
		else
		{
			if (nowMeatNum == 0 && this.updateExCT != 0)
			{
				this.meatNumUpButtonCollider.isTouching = false;
				this.isMeatExCTOver = false;
				return;
			}
			if (nowMeatNum < 0 && nowMeatNum + VariationValue > 0 && this.updateExCT != 0)
			{
				VariationValue = nowMeatNum * -1;
				this.meatNumUpButtonCollider.isTouching = false;
				this.isMeatExCTOver = false;
			}
		}
		if (this.updateExCT > 1)
		{
			SoundMng.Instance().PlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1, 1f);
		}
		DataMng.ExperienceInfo expInfo = this.GetExpInfo();
		int i = VariationValue;
		while (i > 0)
		{
			int lev = expInfo.lev;
			i--;
			if (IsCountUp)
			{
				if (!this.meatNumDownButtonCollider.activeCollider)
				{
					this.meatNumDownButtonSprite.spriteName = "Common02_Meal_Down";
					this.meatNumDownButtonCollider.activeCollider = true;
				}
				this.execMeatNum++;
			}
			else
			{
				if (!this.meatNumUpButtonCollider.activeCollider)
				{
					this.meatNumUpButtonSprite.spriteName = "Common02_Meal_UP";
					this.meatNumUpButtonCollider.activeCollider = true;
				}
				this.execMeatNum--;
			}
			if (this.execMeatNum == 0)
			{
				this.clBtnExec.activeCollider = false;
				this.spriteBtnExec.spriteName = "Common02_Btn_Gray";
				this.labelBtnExec.color = Color.gray;
				this.meatNumDownButtonSprite.spriteName = "Common02_Meal_Down_G";
				this.meatNumDownButtonCollider.activeCollider = false;
				i = 0;
			}
			else if (!this.clBtnExec.activeCollider)
			{
				this.clBtnExec.activeCollider = true;
				this.spriteBtnExec.spriteName = "Common02_Btn_Red";
				this.labelBtnExec.color = Color.white;
			}
			expInfo = this.GetExpInfo();
			if (lev < expInfo.lev)
			{
				this.consumptionLevelUpCount += expInfo.lev - lev;
				this.statusAnime.Displaylevel += expInfo.lev - lev;
				this.consumptionLevelUpLabel.text = string.Format(StringMaster.GetString("MealLvUp"), this.consumptionLevelUpCount);
				if (expInfo.lev >= int.Parse(CMD_MealExecution.DataChg.monsterM.maxLevel))
				{
					this.meatNumUpButtonSprite.spriteName = "Common02_Meal_UP_G";
					this.meatNumUpButtonCollider.activeCollider = false;
					i = 0;
				}
			}
			else if (lev > expInfo.lev)
			{
				this.consumptionLevelUpCount -= lev - expInfo.lev;
				this.statusAnime.Displaylevel -= lev - expInfo.lev;
				if (this.consumptionLevelUpCount > 0)
				{
					this.consumptionLevelUpLabel.text = string.Format(StringMaster.GetString("MealLvUp"), this.consumptionLevelUpCount);
				}
				else
				{
					this.consumptionLevelUpLabel.text = string.Empty;
				}
			}
		}
		this.monsterBasicInfo.UpdateExpGauge(CMD_MealExecution.DataChg, expInfo);
		this.consumptionScheduleNum.text = this.execMeatNum.ToString();
		if (this.CheckMeat() || this.execMeatNum == 0)
		{
			this.consumptionScheduleNum.color = Color.white;
		}
		else
		{
			this.consumptionScheduleNum.color = Color.red;
		}
	}

	private int GetNowMeatNum()
	{
		return int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum) - this.execMeatNum;
	}

	private bool CheckMeat()
	{
		bool flag = true;
		int nowMeatNum = this.GetNowMeatNum();
		if (nowMeatNum >= 0)
		{
			DataMng.ExperienceInfo expInfo = this.GetExpInfo();
			int lev = expInfo.lev;
			if (lev > int.Parse(CMD_MealExecution.DataChg.monsterM.maxLevel))
			{
				flag = false;
			}
		}
		else
		{
			flag = false;
		}
		if (this.execMeatNum == 0)
		{
			flag = false;
		}
		this.clBtnExec.activeCollider = flag;
		this.spriteBtnExec.spriteName = ((!flag) ? "Common02_Btn_Gray" : "Common02_Btn_Red");
		this.labelBtnExec.color = ((!flag) ? Color.gray : Color.white);
		return flag;
	}

	private void InitBtnHQMeat()
	{
		int userItemNumByItemId = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(50001);
		if (int.Parse(CMD_MealExecution.DataChg.userMonster.level) < int.Parse(CMD_MealExecution.DataChg.monsterM.maxLevel) && !this.dontExec)
		{
			if (userItemNumByItemId > 0)
			{
				this.spBtnHQMeat.spriteName = "Common02_Btn_Red";
				this.lbBtnHQMeatExec.text = StringMaster.GetString("UseHQMeal");
			}
			else
			{
				this.spBtnHQMeat.spriteName = "Common02_Btn_Green";
				this.lbBtnHQMeatExec.text = StringMaster.GetString("BuyHQMeal");
			}
			this.coBtnHQMeat.enabled = true;
			this.lbBtnHQMeatExec.color = Color.white;
		}
		else
		{
			this.DisableExecBtns();
		}
	}

	private IEnumerator PlayCharaAnimation(CharacterAnimationType AnimeType, bool IsForce = false)
	{
		if (this.isEating && !IsForce)
		{
			yield break;
		}
		this.isEating = true;
		this.charaParams.PlayAnimation(AnimeType, SkillType.Attack, 0, null, null);
		float animeClipLength = this.charaParams.AnimationClipLength;
		float playTime = 0f;
		while (playTime < animeClipLength)
		{
			playTime += Time.deltaTime;
			yield return null;
		}
		this.isEating = false;
		yield break;
	}

	private void UpdateExecute()
	{
		DataMng.ExperienceInfo expInfo = this.GetExpInfo();
		if ((expInfo.lev >= int.Parse(CMD_MealExecution.DataChg.monsterM.maxLevel) || this.execMeatNum == 0) && this.updateExCT != 0)
		{
			this.meatNumUpButtonCollider.isTouching = false;
			this.meatNumDownButtonCollider.isTouching = false;
			this.updateExCT = 0;
			return;
		}
		if (this.meatNumUpButtonCollider.isTouching || this.meatNumDownButtonCollider.isTouching)
		{
			if (++this.updateExCT > 20)
			{
				if (this.updateExCT > 52)
				{
					if (this.updateExCT > 82)
					{
						this.OnExecute(this.meatNumUpButtonCollider.isTouching, 10);
					}
					else
					{
						this.OnExecute(this.meatNumUpButtonCollider.isTouching, 1);
					}
				}
				else if (this.updateExCT % 4 == 0)
				{
					this.OnExecute(this.meatNumUpButtonCollider.isTouching, 1);
				}
			}
		}
		else
		{
			this.updateExCT = 0;
		}
	}

	private DataMng.ExperienceInfo GetExpInfo()
	{
		float num = 1f;
		GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
		if (respDataCP_Campaign != null && !DataMng.Instance().CampaignForceHide)
		{
			GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp, false);
			if (campaign != null)
			{
				num = campaign.rate.ToFloat();
			}
		}
		float num2 = (float)DataMng.Instance().GetExpFromMeat(this.execMeatNum) * num;
		int exp = int.Parse(CMD_MealExecution.DataChg.userMonster.ex) + (int)num2;
		return DataMng.Instance().GetExperienceInfo(exp);
	}

	private void StopSE_ExpGaugeUp()
	{
		if (SoundMng.Instance().IsPlayingSE("SEInternal/Common/se_102"))
		{
			SoundMng.Instance().TryStopSE("SEInternal/Common/se_102", 0.2f, null);
		}
	}

	public void PlayGiftMeat()
	{
		this.DisableExecBtns();
		if (this.dontExec)
		{
			base.StartCoroutine(this.PlayGiftMeatAnimation(delegate
			{
				CMD_MealExecution.last_exp_info_bk = this.GetExpInfo();
				CMD_MealExecution.execMeatNum_bk = this.execMeatNum;
				CMD_MealExecution.monsterdata_bk = CMD_MealExecution.DataChg;
				if (this.actionLevelUp != null)
				{
					this.actionLevelUp();
					this.actionLevelUp = null;
				}
			}));
			this.IsLockClose = false;
		}
		else
		{
			this.ReportServer();
			this.IsLockClose = true;
		}
	}

	private void PlayGiftHQMeat()
	{
		int userItemNumByItemId = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(50001);
		if (userItemNumByItemId > 0)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnClickUseHQMeatExec), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("UseHQMealTitle");
			cmd_Confirm.Info = StringMaster.GetString("UseHQMealMessage");
		}
		else
		{
			CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE;
			cmd_ChangePOP_STONE.Title = StringMaster.GetString("BuyHQMealTitle");
			cmd_ChangePOP_STONE.Info = string.Format(StringMaster.GetString("BuyHQMealMessage"), ConstValue.BUY_HQMEAT_DIGISTONE_NUM);
			cmd_ChangePOP_STONE.OnPushedYesAction = new Action(this.OnClickBuyHQMeatExec);
			cmd_ChangePOP_STONE.SetDigistone(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point, ConstValue.BUY_HQMEAT_DIGISTONE_NUM);
		}
	}

	private void OnClickUseHQMeatExec(int idx)
	{
		if (idx == 0)
		{
			this.HQMeatExec(GameWebAPI.MN_Req_HQMeal.FusionType.FREE);
		}
	}

	private void OnClickBuyHQMeatExec()
	{
		CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
		if (cmd_ChangePOP_STONE != null)
		{
			cmd_ChangePOP_STONE.ClosePanel(true);
		}
		if (DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point < ConstValue.BUY_HQMEAT_DIGISTONE_NUM)
		{
			GUIMain.ShowCommonDialog(null, "CMD_Shop");
		}
		else
		{
			this.HQMeatExec(GameWebAPI.MN_Req_HQMeal.FusionType.STONE);
		}
	}

	private void HQMeatExec(GameWebAPI.MN_Req_HQMeal.FusionType fusionType)
	{
		this.DisableExecBtns();
		this.isLockClose = false;
		this.IsLockClose = true;
		GameWebAPI.RequestMN_MonsterHQMeal request = new GameWebAPI.RequestMN_MonsterHQMeal
		{
			SetSendData = delegate(GameWebAPI.MN_Req_HQMeal param)
			{
				param.baseMonster = CMD_MealExecution.DataChg.userMonster.userMonsterId;
				param.fusionType = (int)fusionType;
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_HQMealExec resData)
			{
				if (resData.userMonster != null)
				{
					if (fusionType == GameWebAPI.MN_Req_HQMeal.FusionType.FREE)
					{
						Singleton<UserDataMng>.Instance.UpdateUserItemNum(50001, -1);
					}
					else
					{
						DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point -= ConstValue.BUY_HQMEAT_DIGISTONE_NUM;
					}
					if (int.Parse(CMD_MealExecution.DataChg.userMonster.level) < int.Parse(resData.userMonster.level))
					{
						this.consumptionLevelUpLabel.text = string.Format(StringMaster.GetString("MealLvUp"), int.Parse(resData.userMonster.level) - int.Parse(CMD_MealExecution.DataChg.userMonster.level));
					}
					ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(resData.userMonster);
					CMD_MealExecution.last_exp_info_bk = new DataMng.ExperienceInfo
					{
						exp = int.Parse(resData.userMonster.ex),
						lev = int.Parse(resData.userMonster.level)
					};
					CMD_MealExecution.monsterdata_bk = CMD_MealExecution.DataChg;
					this.clBtnExec.activeCollider = false;
					this.spriteBtnExec.spriteName = "Common02_Btn_Gray";
					this.labelBtnExec.color = Color.gray;
					CMD_MealExecution.execMeatNum_bk = 0;
					this.isExecHQMeat = true;
					this.statusAnime.Displaylevel = int.Parse(CMD_MealExecution.DataChg.monsterM.maxLevel);
					NGUIUtil.ChangeUITextureFromFile(this.meat.txMeat, "UITexture/Common02_Meal_Meat2", false);
				}
			}
		};
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		base.StartCoroutine(request.Run(new Action(this.EndExecSuccess), new Action<Exception>(this.EndExecFailed), null));
	}

	private void DisableExecBtns()
	{
		this.clBtnExec.activeCollider = false;
		this.spriteBtnExec.spriteName = "Common02_Btn_Gray";
		this.labelBtnExec.color = Color.gray;
		this.spBtnHQMeat.spriteName = "Common02_Btn_Gray";
		this.lbBtnHQMeatExec.text = StringMaster.GetString("UseHQMeal");
		this.lbBtnHQMeatExec.color = Color.gray;
		this.coBtnHQMeat.enabled = false;
	}

	public IEnumerator PlayGiftMeatAnimation(Action OnPlayed = null)
	{
		if (this.meat != null)
		{
			this.meat.Act();
		}
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_208", 0f, false, true, null, -1, 1f);
		if (!this.isEating)
		{
			yield return base.StartCoroutine(this.PlayCharaAnimation(CharacterAnimationType.eat, false));
		}
		if (!string.IsNullOrEmpty(this.consumptionLevelUpLabel.text))
		{
			if (this.isExecHQMeat)
			{
				this.hqMeatAnim.enabled = true;
				this.hqMeatAnim.Play("Aura_niku");
				yield return null;
			}
			yield return this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.LevelUp, delegate
			{
				this.ShowLevelUpAnimation();
				SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
			});
			yield return base.StartCoroutine(this.PlayCharaAnimation(CharacterAnimationType.win, false));
			if (OnPlayed != null)
			{
				OnPlayed();
			}
		}
		else if (OnPlayed != null)
		{
			OnPlayed();
		}
		yield break;
	}

	private void ReportServer()
	{
		CMD_MealExecution.last_exp_info_bk = this.GetExpInfo();
		CMD_MealExecution.execMeatNum_bk = this.execMeatNum;
		CMD_MealExecution.monsterdata_bk = CMD_MealExecution.DataChg;
		if (CMD_MealExecution.execMeatNum_bk <= 0)
		{
			return;
		}
		if (this.dontExec)
		{
			this.IsLockClose = false;
			return;
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		DataMng.Instance().CheckCampaign(new Action<int>(this.MealExec), new GameWebAPI.RespDataCP_Campaign.CampaignType[]
		{
			GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp
		});
	}

	private void MealExec(int result)
	{
		if (result == -1)
		{
			return;
		}
		if (result > 0)
		{
			RestrictionInput.EndLoad();
			this.IsLockClose = false;
			DataMng.Instance().CampaignErrorCloseAllCommonDialog(result == 1, delegate
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				DataMng.Instance().ReloadCampaign(delegate
				{
					RestrictionInput.EndLoad();
				});
			});
		}
		else
		{
			GameWebAPI.RequestMN_MonsterMeal requestMN_MonsterMeal = new GameWebAPI.RequestMN_MonsterMeal();
			requestMN_MonsterMeal.SetSendData = delegate(GameWebAPI.MN_Req_Meal param)
			{
				param.mealMonster = CMD_MealExecution.DataChg.userMonster.userMonsterId;
				param.meatNum = this.execMeatNum;
			};
			requestMN_MonsterMeal.OnReceived = delegate(GameWebAPI.RespDataMN_MealExec response)
			{
				ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
			};
			GameWebAPI.RequestMN_MonsterMeal request = requestMN_MonsterMeal;
			base.StartCoroutine(request.Run(new Action(this.EndExecSuccess), new Action<Exception>(this.EndExecFailed), null));
		}
	}

	private void ShowLevelUpAnimation()
	{
		string path = "UICommon/Parts/LevelUp";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		DepthController depthController = gameObject.AddComponent<DepthController>();
		Transform transform = gameObject.transform;
		int depth = this.rightUpBG.depth;
		depthController.AddWidgetDepth(transform, depth + 10);
		transform.parent = this.LevelUpRoot;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		Animation component = gameObject.GetComponent<Animation>();
		component.Play("LevelUp");
		this.ShowParticle();
	}

	private void ShowParticle()
	{
		string path = "Cutscenes/NewFX6";
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(path, typeof(GameObject)));
		Transform transform = gameObject.transform;
		transform.parent = base.transform;
		transform.localPosition = this.LevelUpRoot.localPosition;
	}

	private void OnClickedScreen()
	{
		this.ClosePanel(true);
	}

	public void OnDisplayDrag(Vector2 Delta)
	{
	}

	public void OnDisplayClick()
	{
	}

	public void OnClickMeatCountUp()
	{
		this.OnExecute(true, 1);
	}

	public void OnClickMeatCountDown()
	{
		if (this.execMeatNum < 0)
		{
			this.execMeatNum = 0;
		}
		if (this.execMeatNum == 0)
		{
			return;
		}
		this.OnExecute(false, 1);
	}

	public void SetActionLevelUp(Action action)
	{
		this.actionLevelUp = action;
	}

	public void SetMeatNum(int num)
	{
		this.ngTX_MEAT.text = num.ToString();
	}

	private void CampaignReloadServerRequest()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		APIRequestTask task = DataMng.Instance().RequestCampaignAll(true);
		IEnumerator routine = task.Run(new Action(this.RequestCompleted), null, null);
		base.StartCoroutine(routine);
	}

	private void RequestCompleted()
	{
		RestrictionInput.EndLoad();
		if (!this.IsLockClose)
		{
			this.CloseAndFarmCamOn(false);
		}
	}
}
