using CharacterModelUI;
using Master;
using Monster;
using System;
using UnityEngine;

public sealed class CMD_ChipSphere : CMD
{
	public const string chipEmptyTexturePath = "ChipThumbnail/Chip_Empty";

	public const string chipNotOpenTexturePath = "ChipThumbnail/Chip_NotOpen";

	public const string chipChipTexturePathFormat = "ChipThumbnail/{0}";

	public const string extendItemSpriteName = "extra_slot_01";

	public const string ejectItemSpriteName = "eject_slot_01";

	public const int adultButtonNo = 1;

	public const int perfectButtonNo = 2;

	public const int ultimateButtonNo = 3;

	public const int arousal2ButtonNo = 4;

	public const int arousal4ButtonNo = 5;

	[Header("演出中に守るバリア")]
	[SerializeField]
	private GameObject barrierGO;

	[SerializeField]
	[Header("左下のベースを変更ボタンラベル")]
	private UILabel changeBaseButtonLabel;

	[SerializeField]
	[Header("取外パッチの残り個数")]
	private UILabel ejectCountLabel;

	[Header("拡張パッチの残り個数")]
	[SerializeField]
	private UILabel extraCountLabel;

	[Header("右下の切り替わるメニューのオブジェクト")]
	[SerializeField]
	private ChipSphereStatus[] chipSphereStatus;

	[Header("チップボタン達の親")]
	[SerializeField]
	private Transform sphereRoot;

	[SerializeField]
	[Header("左に出る3Dキャラの表示")]
	private UITexture character3DTexture;

	[SerializeField]
	[Header("ステータスのルート")]
	private GameObject statusRoot;

	[Header("装着アニメーションのゲームオブジェクト")]
	[SerializeField]
	private GameObject partsUpperCutinGO;

	[Header("装着アニメーションのテクスチャ")]
	[SerializeField]
	private UITexture[] partsUpperCutinTextures;

	[SerializeField]
	[Header("拡張/チップ取外アニメーションのゲームオブジェクト")]
	private GameObject ejectItemCutinGO;

	[Header("拡張/チップ取外アニメーションのテクスチャ")]
	[SerializeField]
	private UITexture[] ejectItemCutinTextures;

	private CharacterCameraView characterCameraView;

	private ChipSphereIconButton[] chipSphereIconButtons;

	private bool isLoopOff;

	private int oldButtonIndex;

	private int ejectPatchCount;

	private int extraPatchCount;

	private int myMaxChipSlot = 5;

	private int consumeItemCount;

	private bool onceAnimFlag;

	private bool isTouchGuard;

	private ChipSphereIconButton.Parameter cachedParameter;

	private ChipSphereIconButton.Parameter selectedChipParameter;

	private GameWebAPI.RespDataCS_ChipListLogic.UserChipList currentUserChipList;

	private Vector3[] chipButtonsPositions = new Vector3[]
	{
		Vector3.zero,
		new Vector3(-132f, -80f, 0f),
		new Vector3(-132f, 80f, 0f),
		new Vector3(132f, -80f, 0f),
		new Vector3(132f, 80f, 0f),
		new Vector3(264f, 0f, 0f),
		new Vector3(264f, 160f, 0f),
		new Vector3(0f, 160f, 0f),
		new Vector3(-264f, 160f, 0f),
		new Vector3(-264f, 0f, 0f)
	};

	private CMD_ChangePOP cmdChangePop;

	private AnimationFinishEventTrigger attachAnimationTrigger;

	private AnimationFinishEventTrigger ejectAnimationTrigger;

	[Header("ライン達")]
	[SerializeField]
	private ChipSphereLines chipSphereLines;

	public static MonsterData DataChg { get; set; }

	private CMD_ChipSphere.MenuType myMenuType { get; set; }

	protected override void Awake()
	{
		base.Awake();
		Vector3 localPosition = this.character3DTexture.transform.localPosition;
		localPosition.x = 0f;
		localPosition.y = 0f;
		this.character3DTexture.transform.localPosition = localPosition;
	}

	protected override void Update()
	{
		base.Update();
		if (this.characterCameraView != null)
		{
			this.characterCameraView.Update(Time.deltaTime);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_ChipSphere.DataChg = null;
		if (this.characterCameraView != null)
		{
			this.characterCameraView.Destroy();
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		base.SetTutorialAnyTime("anytime_second_tutorial_chip_equipment");
		this.Init(ChipDataMng.userChipData);
	}

	private void Init(GameWebAPI.RespDataCS_ChipListLogic chipListLogic)
	{
		this.attachAnimationTrigger = this.partsUpperCutinGO.GetComponent<AnimationFinishEventTrigger>();
		this.ejectAnimationTrigger = this.ejectItemCutinGO.GetComponent<AnimationFinishEventTrigger>();
		this.partsUpperCutinGO.SetActive(false);
		this.ejectItemCutinGO.SetActive(false);
		this.SetBarrier(false);
		this.SetupLocalize();
		this.ShowCharacter();
		this.SetupItemNumbers();
		this.SetupChipButtons();
		this.SetDegign();
	}

	private void SetBarrier(bool isEnable)
	{
		this.barrierGO.SetActive(isEnable);
	}

	private void SetDegign()
	{
		bool degign = this.myMaxChipSlot == 10;
		this.chipSphereLines.SetDegign(degign);
	}

	public void OnDisplayDrag(Vector2 Delta)
	{
		this.characterCameraView.OnDisplayDrag(Delta);
	}

	private void OnTouchBaseSelect()
	{
		base.ClosePanel(true);
	}

	private void ShowCharacter()
	{
		this.characterCameraView = new CharacterCameraView(CMD_ChipSphere.DataChg);
		this.character3DTexture.mainTexture = this.characterCameraView.renderTex;
	}

	private void SetupItemNumbers()
	{
		this.ejectPatchCount = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(7);
		this.extraPatchCount = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(6);
		this.RefreshItemNumbers();
	}

	private void RefreshItemNumbers()
	{
		this.ejectCountLabel.text = this.ejectPatchCount.ToString();
		this.extraCountLabel.text = this.extraPatchCount.ToString();
	}

	private void SetupLocalize()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("ChipSphereTitle"));
		this.changeBaseButtonLabel.text = StringMaster.GetString("BaseChange");
	}

	private void SetupChipButtons()
	{
		GameObject original = Resources.Load<GameObject>("UICommon/Parts/Parts_Sphere_CHIP");
		this.chipSphereIconButtons = new ChipSphereIconButton[this.chipButtonsPositions.Length];
		DepthController component = this.sphereRoot.GetComponent<DepthController>();
		int depth = this.sphereRoot.GetComponent<UIWidget>().depth;
		int myGrowStep = CMD_ChipSphere.DataChg.monsterMG.growStep.ToInt32();
		int arousal = CMD_ChipSphere.DataChg.monsterM.GetArousal();
		this.myMaxChipSlot = 5;
		int num = 0;
		if (CMD_ChipSphere.DataChg.GetChipEquip().GetSlotStatus() == null)
		{
			global::Debug.LogError("DataChg.GetSlotStatus() == null");
		}
		else
		{
			GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage slotStatus = CMD_ChipSphere.DataChg.GetChipEquip().GetSlotStatus();
			global::Debug.LogFormat("無料:{0}/{1}, 課金:{2}/{3}", new object[]
			{
				slotStatus.slotNum,
				slotStatus.maxSlotNum,
				slotStatus.extraSlotNum,
				slotStatus.maxExtraSlotNum
			});
			num = slotStatus.extraSlotNum;
			this.myMaxChipSlot = slotStatus.maxSlotNum + slotStatus.maxExtraSlotNum;
			if (this.myMaxChipSlot != 5 && this.myMaxChipSlot != 10)
			{
				global::Debug.LogErrorFormat("スロット数がありえない. myMaxChipSlot:{0} [{1}, {2}]", new object[]
				{
					this.myMaxChipSlot,
					slotStatus.maxSlotNum,
					slotStatus.maxExtraSlotNum
				});
				this.myMaxChipSlot = 10;
			}
		}
		for (int i = 0; i < this.myMaxChipSlot; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate<GameObject>(original).transform;
			transform.name = string.Format("Parts_Sphere_CHIP_{0}", i + 1);
			ChipSphereIconButton component2 = transform.GetComponent<ChipSphereIconButton>();
			component.AddWidgetDepth(transform, depth);
			component2.cmdChipSphere = this;
			transform.SetParent(this.sphereRoot);
			transform.localScale = Vector3.one;
			transform.localPosition = this.chipButtonsPositions[i];
			int num2 = i + 1;
			ChipSphereIconButton.Parameter parameter = this.CreateParameter(myGrowStep, arousal, num2);
			component2.SetupChip(parameter);
			component2.RefreshItemCountColor(this.extraPatchCount);
			component2.SetUnChoose();
			this.chipSphereIconButtons[i] = component2;
			if (num2 >= 6)
			{
				if (num >= 0)
				{
					this.chipSphereIconButtons[num2 - 1].SetChipColor(true);
				}
				else
				{
					this.chipSphereIconButtons[num2 - 1].SetChipColor(false);
				}
				num--;
			}
		}
		this.RefreshYellowLines();
		this.RefreshStatus();
		this.chipSphereIconButtons[0].OnTouch();
	}

	private void RefreshItemCountColor()
	{
		for (int i = 0; i < this.myMaxChipSlot; i++)
		{
			this.chipSphereIconButtons[i].RefreshItemCountColor(this.extraPatchCount);
		}
	}

	private ChipSphereIconButton.Parameter CreateParameter(int myGrowStep, int myArousal, int buttonNo)
	{
		int type = 0;
		CMD_ChipSphere.MenuType menuType = CMD_ChipSphere.MenuType.Empty;
		string chipName = string.Empty;
		string chipDetail = string.Empty;
		int userChipId = 0;
		int itemCount = 0;
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage slotStatus = CMD_ChipSphere.DataChg.GetChipEquip().GetSlotStatus();
		int num = slotStatus.maxSlotNum + slotStatus.extraSlotNum;
		if (buttonNo == 1)
		{
			if (MonsterGrowStepData.IsRipeScope(myGrowStep) || MonsterGrowStepData.IsPerfectScope(myGrowStep) || MonsterGrowStepData.IsUltimateScope(myGrowStep))
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string growStepName = MonsterGrowStepData.GetGrowStepName(5.ToString());
				chipName = growStepName;
				string @string = StringMaster.GetString("ChipInstallingEvolutionInfo");
				chipDetail = string.Format(@string, growStepName);
			}
		}
		else if (buttonNo == 2)
		{
			if (MonsterGrowStepData.IsPerfectScope(myGrowStep) || MonsterGrowStepData.IsUltimateScope(myGrowStep))
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string growStepName2 = MonsterGrowStepData.GetGrowStepName(6.ToString());
				chipName = growStepName2;
				string string2 = StringMaster.GetString("ChipInstallingEvolutionInfo");
				chipDetail = string.Format(string2, growStepName2);
			}
		}
		else if (buttonNo == 3)
		{
			if (MonsterGrowStepData.IsUltimateScope(myGrowStep))
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string growStepName3 = MonsterGrowStepData.GetGrowStepName(7.ToString());
				chipName = growStepName3;
				string string3 = StringMaster.GetString("ChipInstallingEvolutionInfo");
				chipDetail = string.Format(string3, growStepName3);
				this.chipSphereLines.OpenMiddleToLeftUp(ChipSphereLines.LineType.None);
			}
		}
		else if (buttonNo == 4)
		{
			if (myArousal >= 2)
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string string4 = StringMaster.GetString("CharaStatus-23");
				chipName = string.Format("{0}{1}", string4, 2);
				chipDetail = string4;
				this.chipSphereLines.OpenMiddleToRightDown(ChipSphereLines.LineType.None);
			}
		}
		else if (buttonNo == 5)
		{
			if (myArousal >= 4)
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string string5 = StringMaster.GetString("CharaStatus-23");
				chipName = string.Format("{0}{1}", string5, 4);
				chipDetail = string5;
				this.chipSphereLines.OpenMiddleToRightUp(ChipSphereLines.LineType.None);
			}
		}
		else if (buttonNo >= 6)
		{
			type = 1;
			itemCount = ConstValue.CHIP_EXTEND_SLOT_NEEDS[buttonNo - 6];
			if (buttonNo <= num)
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.Extendable;
			}
		}
		if (CMD_ChipSphere.DataChg.GetSlotEquip() == null)
		{
			global::Debug.Log("装着はありません.");
		}
		else
		{
			foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip in CMD_ChipSphere.DataChg.GetSlotEquip())
			{
				int num2 = ChipTools.ConvertToButtonNo(equip);
				if (buttonNo == num2)
				{
					menuType = CMD_ChipSphere.MenuType.Detail;
					userChipId = equip.userChipId;
					break;
				}
			}
		}
		return new ChipSphereIconButton.Parameter
		{
			type = type,
			buttonNo = buttonNo,
			menuType = menuType,
			itemCount = itemCount,
			chipName = chipName,
			chipDetail = chipDetail,
			userChipId = userChipId
		};
	}

	private void RefreshLines(Action<ChipSphereLines.LineType> method, int leftButtonNo, int rightButtonNo)
	{
		ChipSphereLines.LineType obj = ChipSphereLines.LineType.None;
		if (this.myMaxChipSlot == 5 && (leftButtonNo > 5 || rightButtonNo > 5))
		{
			return;
		}
		if (this.chipSphereIconButtons[leftButtonNo - 1].isPuttedChip && this.chipSphereIconButtons[rightButtonNo - 1].isPuttedChip)
		{
			obj = ChipSphereLines.LineType.Yellow;
		}
		else if (this.chipSphereIconButtons[leftButtonNo - 1].isOpened && this.chipSphereIconButtons[rightButtonNo - 1].isOpened)
		{
			obj = ChipSphereLines.LineType.Blue;
		}
		method(obj);
	}

	private void RefreshYellowLines()
	{
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenMiddleToRightUp), 1, 5);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenMiddleToLeftUp), 1, 3);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenMiddleToRightDown), 1, 4);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenMiddleToLeftDown), 1, 2);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenMiddleToLeft), 1, 10);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenMiddleToRight), 1, 6);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenRightDownToUp), 6, 7);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenUpMiddleToRight), 7, 8);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenUpMiddleToLeft), 8, 9);
		this.RefreshLines(new Action<ChipSphereLines.LineType>(this.chipSphereLines.OpenLeftDownToUp), 9, 10);
	}

	public void OnTouchChipIcon(ChipSphereIconButton.Parameter parameter)
	{
		if (this.selectedChipParameter == parameter)
		{
			if (this.myMenuType == CMD_ChipSphere.MenuType.Empty)
			{
				this.HideLoopAnim();
				this.ShowAttachPage();
			}
			return;
		}
		if (this.isTouchGuard)
		{
			return;
		}
		this.HideLoopAnim();
		this.selectedChipParameter = parameter;
		if (!parameter.IsEmpty())
		{
			this.currentUserChipList = ChipDataMng.GetUserChip(parameter.userChipId);
		}
		else
		{
			this.currentUserChipList = null;
		}
		this.chipSphereIconButtons[this.oldButtonIndex].SetUnChoose();
		this.chipSphereIconButtons[parameter.buttonNo - 1].SetChoose();
		this.oldButtonIndex = parameter.buttonNo - 1;
		this.myMenuType = parameter.menuType;
		if (!this.onceAnimFlag)
		{
			this.onceAnimFlag = true;
			this.ChangeStatusRoot(parameter);
		}
		else
		{
			this.cachedParameter = parameter;
			this.isTouchGuard = true;
			this.MoveAnim();
		}
	}

	private void HideLoopAnim()
	{
		if (this.isLoopOff)
		{
			this.partsUpperCutinGO.SetActive(false);
			this.ejectItemCutinGO.SetActive(false);
			this.isLoopOff = false;
		}
	}

	private void MoveAnim()
	{
		iTween.Stop(this.statusRoot, "move");
		foreach (ChipSphereStatus chipSphereStatus in this.chipSphereStatus)
		{
			chipSphereStatus.SetButtonOff();
		}
		this.statusRoot.transform.localPosition = new Vector3(252f, -238.4f, 0f);
		iTween.MoveBy(this.statusRoot, iTween.Hash(new object[]
		{
			"y",
			-0.6,
			"easeType",
			"easeInOutExpo",
			"time",
			0.3,
			"oncomplete",
			"OnCompleteAnim",
			"oncompletetarget",
			base.gameObject
		}));
	}

	private void OnCompleteAnim()
	{
		this.isTouchGuard = false;
		this.ChangeStatusRoot(this.cachedParameter);
		iTween.MoveBy(this.statusRoot, iTween.Hash(new object[]
		{
			"y",
			0.6,
			"easeType",
			"easeInOutExpo",
			"time",
			0.3
		}));
	}

	private void RefreshStatus()
	{
		foreach (ChipSphereStatus chipSphereStatus in this.chipSphereStatus)
		{
			chipSphereStatus.Refresh(this.extraPatchCount, this.ejectPatchCount);
		}
	}

	private void ChangeStatusRoot(ChipSphereIconButton.Parameter parameter)
	{
		for (int i = 0; i < this.chipSphereStatus.Length; i++)
		{
			this.chipSphereStatus[i].gameObject.SetActive(false);
		}
		switch (parameter.menuType)
		{
		case CMD_ChipSphere.MenuType.Empty:
			this.chipSphereStatus[0].gameObject.SetActive(true);
			this.chipSphereStatus[0].SetupDetail(parameter);
			break;
		case CMD_ChipSphere.MenuType.Extendable:
			this.chipSphereStatus[0].gameObject.SetActive(true);
			this.chipSphereStatus[0].SetupDetail(parameter);
			this.consumeItemCount = parameter.itemCount;
			break;
		case CMD_ChipSphere.MenuType.NotYet:
			this.chipSphereStatus[1].gameObject.SetActive(true);
			this.chipSphereStatus[1].SetupDetail(parameter);
			break;
		case CMD_ChipSphere.MenuType.Detail:
			this.chipSphereStatus[2].gameObject.SetActive(true);
			this.chipSphereStatus[2].SetupDetail(parameter);
			break;
		default:
			global::Debug.LogError("ありえない.");
			break;
		}
	}

	private void OnTouchDecide()
	{
		this.HideLoopAnim();
		switch (this.myMenuType)
		{
		case CMD_ChipSphere.MenuType.Empty:
			this.ShowAttachPage();
			return;
		case CMD_ChipSphere.MenuType.Extendable:
			this.ShowExtendChipDialog();
			return;
		case CMD_ChipSphere.MenuType.Detail:
			this.ShowEjectItemDialog();
			return;
		}
		global::Debug.LogError("ありえない.");
	}

	private void OnTouchReinforce()
	{
		this.HideLoopAnim();
		Action<int> callback = delegate(int result)
		{
			if (result > 0)
			{
				this.Send(this.currentUserChipList);
			}
		};
		CMD_ChipReinforcementModal.Create(this.currentUserChipList, callback);
	}

	private void Send(GameWebAPI.RespDataCS_ChipListLogic.UserChipList baseChip)
	{
		this.SetBarrier(true);
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		int baseChipId = baseChip.userChipId;
		GameWebAPI.RespDataMA_ChipM.Chip baseMaterChip = ChipDataMng.GetChipMainData(baseChip);
		int num = baseMaterChip.needChip.ToInt32();
		int[] array = null;
		if (num > 0)
		{
			array = new int[num];
			int num2 = 0;
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in ChipDataMng.userChipData.userChipList)
			{
				if (baseChipId != userChipList2.userChipId && userChipList2.chipId == baseChip.chipId && userChipList2.userMonsterId == 0)
				{
					array[num2] = userChipList2.userChipId;
					num2++;
					if (num2 >= array.Length)
					{
						break;
					}
				}
			}
		}
		Action callback = delegate()
		{
			int num3 = this.selectedChipParameter.ConvertButtonIndex();
			ChipSphereIconButton chipSphereIconButton = this.chipSphereIconButtons[num3];
			GameWebAPI.RespDataMA_ChipM.Chip chipEnhancedData = ChipDataMng.GetChipEnhancedData(baseMaterChip.chipId);
			chipSphereIconButton.SetupDetail(this.selectedChipParameter.userChipId, chipEnhancedData);
			chipSphereIconButton.SetupOnlyDetailParams(baseChipId, chipEnhancedData);
			this.chipSphereIconButtons[num3].OnTouch();
			this.RefreshItemCountColor();
			this.RefreshStatus();
			this.RefreshItemNumbers();
			this.RefreshYellowLines();
			CMD_ChipReinforcementAnimation cmd_ChipReinforcementAnimation = CMD_ChipReinforcementAnimation.Create(this.gameObject, chipEnhancedData, delegate(int i)
			{
				this.SetBarrier(false);
			});
			cmd_ChipReinforcementAnimation.transform.FindChild("ChipLv/Chip").gameObject.AddComponent<AnimationFinishEventTrigger>();
		};
		int resultCode = 0;
		APIRequestTask task = ChipDataMng.RequestAPIChipFusion(baseChip.userChipId, array, delegate(int res)
		{
			resultCode = res;
		});
		AppCoroutine.Start(task.Run(delegate
		{
			if (resultCode == 1)
			{
				callback();
			}
			else
			{
				this.SetBarrier(false);
				this.DispErrorModal(resultCode);
			}
			RestrictionInput.EndLoad();
		}, null, null), false);
	}

	private void ShowAttachPage()
	{
		this.partsUpperCutinGO.SetActive(false);
		this.ejectItemCutinGO.SetActive(false);
		GameWebAPI.ReqDataCS_ChipEquipLogic equip = new GameWebAPI.ReqDataCS_ChipEquipLogic
		{
			dispNum = this.selectedChipParameter.ConvertDispNum(),
			type = this.selectedChipParameter.type,
			userMonsterId = CMD_ChipSphere.DataChg.userMonster.userMonsterId.ToInt32()
		};
		CMD_ChipInstalling.Create(equip, delegate(int userChipId, GameWebAPI.RespDataMA_ChipM.Chip attachedChip)
		{
			this.SetBarrier(true);
			global::Debug.Log(attachedChip.name);
			int index = this.selectedChipParameter.buttonNo - 1;
			this.chipSphereIconButtons[index].SetupOnlyDetailParams(userChipId, attachedChip);
			this.RefreshItemCountColor();
			this.RefreshStatus();
			this.RefreshItemNumbers();
			this.RefreshYellowLines();
			this.partsUpperCutinGO.transform.localPosition = this.chipButtonsPositions[index];
			this.partsUpperCutinGO.SetActive(false);
			int loadCount = 0;
			Action<ChipIcon> callback = delegate(ChipIcon chipIcon)
			{
				loadCount++;
				if (loadCount < this.partsUpperCutinTextures.Length)
				{
					this.partsUpperCutinGO.SetActive(true);
					this.attachAnimationTrigger.OnFinishAnimation = delegate(string str)
					{
						SoundMng.Instance().TryPlaySE("SEInternal/Common/se_110", 0f, false, true, null, -1);
						this.chipSphereIconButtons[index].SetupDetail(userChipId, attachedChip);
						this.SetBarrier(false);
						this.isLoopOff = true;
					};
					this.chipSphereIconButtons[index].OnTouch();
				}
			};
			foreach (UITexture baseIcon in this.partsUpperCutinTextures)
			{
				ChipTools.CreateChipIcon(attachedChip, baseIcon, callback);
			}
		});
	}

	private void ShowExtendChipDialog()
	{
		this.cmdChangePop = CMD_ChangePOP.CreateExtendChipPOP(this.extraPatchCount, this.consumeItemCount, new Action(this.OnPushedExtendConfirmYesButton));
		this.cmdChangePop.SetSpriteIcon("extra_slot_01");
	}

	private void OnPushedExtendConfirmYesButton()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int dispNum = this.selectedChipParameter.ConvertDispNum();
		GameWebAPI.ChipUnlockExtraSlotLogic request = ChipDataMng.RequestAPIChipUnlockExtraSlot(CMD_ChipSphere.DataChg, dispNum, this.consumeItemCount, new Action<int>(this.EndExtend));
		base.StartCoroutine(request.Run(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndExtend(int resultCode)
	{
		if (resultCode == 1)
		{
			this.SetBarrier(true);
			this.extraPatchCount -= this.consumeItemCount;
			int num = this.selectedChipParameter.ConvertButtonIndex();
			this.chipSphereIconButtons[num].SetupEmpty();
			int num2 = Mathf.Clamp(num + 1, 0, this.chipSphereIconButtons.Length - 1);
			this.chipSphereIconButtons[num2].SetChipColor(true);
			this.RefreshItemCountColor();
			this.RefreshStatus();
			this.RefreshItemNumbers();
			this.RefreshYellowLines();
			this.cmdChangePop.ClosePanel(true);
			string texname = string.Format("ChipThumbnail/{0}", "extra_slot_01");
			foreach (UITexture uiTex in this.ejectItemCutinTextures)
			{
				NGUIUtil.ChangeUITextureFromFileASync(uiTex, texname, false, null);
			}
			this.ejectItemCutinGO.transform.localPosition = this.chipButtonsPositions[num];
			this.ejectItemCutinGO.SetActive(false);
			this.ejectItemCutinGO.SetActive(true);
			this.ejectAnimationTrigger.OnFinishAnimation = delegate(string str)
			{
				this.SetBarrier(false);
			};
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_109", 0f, false, true, null, -1);
			this.chipSphereIconButtons[num].OnTouch();
		}
		else
		{
			RestrictionInput.EndLoad();
			ChipTools.CheckResultCode(resultCode);
			this.DispErrorModal(resultCode);
		}
	}

	private void DispErrorModal(int resultCode)
	{
		string @string = StringMaster.GetString("SystemDataMismatchTitle");
		string message = string.Format(StringMaster.GetString("ChipDataMismatchMesage"), resultCode);
		AlertManager.ShowModalMessage(delegate(int modal)
		{
			this.cmdChangePop.ClosePanel(true);
		}, @string, message, AlertManager.ButtonActionType.Close, false);
	}

	private void ShowEjectItemDialog()
	{
		this.cmdChangePop = CMD_ChangePOP.CreateEjectChipPOP(this.ejectPatchCount, 1, new Action(this.OnPushedEjectConfirmYesButton));
		this.cmdChangePop.SetSpriteIcon("eject_slot_01");
	}

	private void OnPushedEjectConfirmYesButton()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int num = this.selectedChipParameter.ConvertDispNum();
		global::Debug.LogFormat("チップ取外でYes押下: dispNum{0}", new object[]
		{
			num
		});
		GameWebAPI.ReqDataCS_ChipEquipLogic equip = new GameWebAPI.ReqDataCS_ChipEquipLogic
		{
			dispNum = this.selectedChipParameter.ConvertDispNum(),
			type = this.selectedChipParameter.type,
			userChipId = this.selectedChipParameter.userChipId,
			userMonsterId = CMD_ChipSphere.DataChg.userMonster.userMonsterId.ToInt32()
		};
		GameWebAPI.ChipEquipLogic request = ChipDataMng.RequestAPIChipEquip(equip, new Action<int, GameWebAPI.RequestMonsterList>(this.EndEject));
		Action<Exception> onFailed = delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		};
		base.StartCoroutine(request.Run(null, onFailed, null));
	}

	private void EndEject(int resultCode, GameWebAPI.RequestMonsterList subRequest)
	{
		if (resultCode == 1)
		{
			this.SetBarrier(true);
			this.ejectPatchCount--;
			int num = this.selectedChipParameter.ConvertButtonIndex();
			this.chipSphereIconButtons[num].SetupEmpty();
			this.RefreshItemCountColor();
			this.RefreshStatus();
			this.RefreshItemNumbers();
			this.RefreshYellowLines();
			this.cmdChangePop.ClosePanel(true);
			string texname = string.Format("ChipThumbnail/{0}", "eject_slot_01");
			foreach (UITexture uiTex in this.ejectItemCutinTextures)
			{
				NGUIUtil.ChangeUITextureFromFileASync(uiTex, texname, false, null);
			}
			this.ejectItemCutinGO.transform.localPosition = this.chipButtonsPositions[num];
			this.ejectItemCutinGO.SetActive(false);
			this.ejectItemCutinGO.SetActive(true);
			this.ejectAnimationTrigger.OnFinishAnimation = delegate(string str)
			{
				this.SetBarrier(false);
				this.isLoopOff = true;
			};
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_108", 0f, false, true, null, -1);
			this.chipSphereIconButtons[num].OnTouch();
			base.StartCoroutine(subRequest.Run(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
		else
		{
			ChipTools.CheckResultCode(resultCode);
			this.DispErrorModal(resultCode);
		}
	}

	public enum MenuType
	{
		Empty,
		Extendable,
		NotYet,
		Detail
	}
}
