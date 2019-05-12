﻿using CharacterModelUI;
using Master;
using System;
using System.Collections;
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

	[Header("左下のベースを変更ボタンラベル")]
	[SerializeField]
	private UILabel changeBaseButtonLabel;

	[Header("取外パッチの残り個数")]
	[SerializeField]
	private UILabel ejectCountLabel;

	[Header("拡張パッチの残り個数")]
	[SerializeField]
	private UILabel extraCountLabel;

	[SerializeField]
	[Header("右下の切り替わるメニューのオブジェクト")]
	private ChipSphereStatus[] chipSphereStatus;

	[SerializeField]
	[Header("チップボタン達の親")]
	private Transform sphereRoot;

	[SerializeField]
	[Header("左に出る3Dキャラの表示")]
	private UITexture character3DTexture;

	[Header("ステータスのルート")]
	[SerializeField]
	private GameObject statusRoot;

	[SerializeField]
	[Header("装着アニメーションのゲームオブジェクト")]
	private GameObject partsUpperCutinGO;

	[SerializeField]
	[Header("装着アニメーションのテクスチャ")]
	private UITexture[] partsUpperCutinTextures;

	[Header("拡張/チップ取外アニメーションのゲームオブジェクト")]
	[SerializeField]
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

	private Action fusionSuccessCallback;

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
		this.Init(ChipDataMng.userChipData);
	}

	private void Init(GameWebAPI.RespDataCS_ChipListLogic chipListLogic)
	{
		this.attachAnimationTrigger = this.partsUpperCutinGO.GetComponent<AnimationFinishEventTrigger>();
		this.ejectAnimationTrigger = this.ejectItemCutinGO.GetComponent<AnimationFinishEventTrigger>();
		NGUITools.SetActiveSelf(this.partsUpperCutinGO, false);
		NGUITools.SetActiveSelf(this.ejectItemCutinGO, false);
		this.SetBarrier(false);
		this.SetupLocalize();
		this.ShowCharacter();
		this.SetupItemNumbers();
		this.SetupChipButtons();
		this.SetDegign();
	}

	private void SetBarrier(bool isEnable)
	{
		NGUITools.SetActiveSelf(this.barrierGO, isEnable);
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
		if (CMD_ChipSphere.DataChg.userMonsterSlotInfo == null)
		{
			global::Debug.LogError("DataChg.userMonsterSlotInfo == null");
		}
		else
		{
			this.myMaxChipSlot = 5;
			int num = 0;
			if (CMD_ChipSphere.DataChg.userMonsterSlotInfo.manage == null)
			{
				global::Debug.LogError("DataChg.userMonsterSlotInfo.manage == null");
			}
			else
			{
				GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage manage = CMD_ChipSphere.DataChg.userMonsterSlotInfo.manage;
				global::Debug.LogFormat("無料:{0}/{1}, 課金:{2}/{3}", new object[]
				{
					manage.slotNum,
					manage.maxSlotNum,
					manage.extraSlotNum,
					manage.maxExtraSlotNum
				});
				num = manage.extraSlotNum;
				this.myMaxChipSlot = manage.maxSlotNum + manage.maxExtraSlotNum;
				if (this.myMaxChipSlot != 5 && this.myMaxChipSlot != 10)
				{
					global::Debug.LogErrorFormat("スロット数がありえない. myMaxChipSlot:{0} [{1}, {2}]", new object[]
					{
						this.myMaxChipSlot,
						manage.maxSlotNum,
						manage.maxExtraSlotNum
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
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage manage = CMD_ChipSphere.DataChg.userMonsterSlotInfo.manage;
		int num = manage.maxSlotNum + manage.extraSlotNum;
		if (buttonNo == 1)
		{
			if (myGrowStep >= 5)
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string grade = CommonSentenceData.GetGrade(5.ToString());
				chipName = grade;
				string @string = StringMaster.GetString("ChipInstallingEvolutionInfo");
				chipDetail = string.Format(@string, grade);
			}
		}
		else if (buttonNo == 2)
		{
			if (myGrowStep >= 6 && myGrowStep != 8)
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string grade2 = CommonSentenceData.GetGrade(6.ToString());
				chipName = grade2;
				string string2 = StringMaster.GetString("ChipInstallingEvolutionInfo");
				chipDetail = string.Format(string2, grade2);
			}
		}
		else if (buttonNo == 3)
		{
			if (myGrowStep >= 7 && myGrowStep != 8)
			{
				menuType = CMD_ChipSphere.MenuType.Empty;
			}
			else
			{
				menuType = CMD_ChipSphere.MenuType.NotYet;
				string grade3 = CommonSentenceData.GetGrade(7.ToString());
				chipName = grade3;
				string string3 = StringMaster.GetString("ChipInstallingEvolutionInfo");
				chipDetail = string.Format(string3, grade3);
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
		if (CMD_ChipSphere.DataChg.userMonsterSlotInfo.equip == null)
		{
			global::Debug.Log("装着はありません.");
		}
		else
		{
			foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip2 in CMD_ChipSphere.DataChg.userMonsterSlotInfo.equip)
			{
				int num2 = ChipTools.ConvertToButtonNo(equip2);
				if (buttonNo == num2)
				{
					menuType = CMD_ChipSphere.MenuType.Detail;
					userChipId = equip2.userChipId;
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
		this.currentUserChipList = ChipDataMng.GetUserChipDataByUserChipId(parameter.userChipId);
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
			NGUITools.SetActiveSelf(this.partsUpperCutinGO, false);
			NGUITools.SetActiveSelf(this.ejectItemCutinGO, false);
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
			NGUITools.SetActiveSelf(this.chipSphereStatus[i].gameObject, false);
		}
		switch (parameter.menuType)
		{
		case CMD_ChipSphere.MenuType.Empty:
			NGUITools.SetActiveSelf(this.chipSphereStatus[0].gameObject, true);
			this.chipSphereStatus[0].SetupDetail(parameter);
			break;
		case CMD_ChipSphere.MenuType.Extendable:
			NGUITools.SetActiveSelf(this.chipSphereStatus[0].gameObject, true);
			this.chipSphereStatus[0].SetupDetail(parameter);
			this.consumeItemCount = parameter.itemCount;
			break;
		case CMD_ChipSphere.MenuType.NotYet:
			NGUITools.SetActiveSelf(this.chipSphereStatus[1].gameObject, true);
			this.chipSphereStatus[1].SetupDetail(parameter);
			break;
		case CMD_ChipSphere.MenuType.Detail:
			NGUITools.SetActiveSelf(this.chipSphereStatus[2].gameObject, true);
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
			break;
		case CMD_ChipSphere.MenuType.Extendable:
			this.ShowExtendChipDialog();
			break;
		case CMD_ChipSphere.MenuType.NotYet:
			global::Debug.LogError("ありえない.");
			break;
		case CMD_ChipSphere.MenuType.Detail:
			this.ShowEjectItemDialog();
			break;
		default:
			global::Debug.LogError("ありえない.");
			break;
		}
	}

	private void OnTouchReinforce()
	{
		this.HideLoopAnim();
		Action<int> callback = delegate(int result)
		{
			if (result > 0)
			{
				AppCoroutine.Start(this.Send(this.currentUserChipList), false);
			}
		};
		CMD_ChipReinforcementModal.Create(this.currentUserChipList, callback);
	}

	private IEnumerator Send(GameWebAPI.RespDataCS_ChipListLogic.UserChipList baseChip)
	{
		this.SetBarrier(true);
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		int baseChipId = baseChip.userChipId;
		GameWebAPI.RespDataMA_ChipM.Chip baseMaterChip = ChipDataMng.GetChipMainData(baseChip);
		int needCount = baseMaterChip.needChip.ToInt32();
		int[] needChips = null;
		if (needCount > 0)
		{
			needChips = new int[needCount];
			int i = 0;
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip in ChipDataMng.userChipData.userChipList)
			{
				if (baseChipId != userChip.userChipId && userChip.chipId == baseChip.chipId && userChip.userMonsterId == 0)
				{
					needChips[i] = userChip.userChipId;
					i++;
					if (i >= needChips.Length)
					{
						break;
					}
				}
			}
		}
		Action callback = delegate()
		{
			RestrictionInput.EndLoad();
			int num = this.selectedChipParameter.ConvertButtonIndex();
			ChipSphereIconButton chipSphereIconButton = this.chipSphereIconButtons[num];
			GameWebAPI.RespDataMA_ChipM.Chip chipEnhancedData = ChipDataMng.GetChipEnhancedData(baseMaterChip.chipId);
			chipSphereIconButton.SetupDetail(this.selectedChipParameter.userChipId, chipEnhancedData);
			chipSphereIconButton.SetupOnlyDetailParams(baseChipId, chipEnhancedData);
			this.chipSphereIconButtons[num].OnTouch();
			this.RefreshItemCountColor();
			this.RefreshStatus();
			this.RefreshItemNumbers();
			this.RefreshYellowLines();
			CMD_ChipReinforcementAnimation cmd_ChipReinforcementAnimation = CMD_ChipReinforcementAnimation.Create(base.gameObject, chipEnhancedData, delegate(int i)
			{
				this.SetBarrier(false);
			});
			cmd_ChipReinforcementAnimation.transform.FindChild("ChipLv/Chip").gameObject.AddComponent<AnimationFinishEventTrigger>();
		};
		this.fusionSuccessCallback = callback;
		GameWebAPI.ChipFusionLogic logic = ChipDataMng.RequestAPIChipFusion(baseChip.userChipId, needChips, new Action<int, GameWebAPI.RequestMonsterList>(this.EndChipFusion));
		IEnumerator run = logic.Run(null, null, null);
		while (run.MoveNext())
		{
			object obj = run.Current;
			yield return obj;
		}
		yield break;
	}

	private void EndChipFusion(int resultCode, GameWebAPI.RequestMonsterList subRequest)
	{
		if (resultCode == 1)
		{
			if (subRequest != null)
			{
				base.StartCoroutine(subRequest.Run(delegate()
				{
					this.fusionSuccessCallback();
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
					GUIMain.BarrierOFF();
				}, null));
			}
			else
			{
				this.fusionSuccessCallback();
			}
		}
		else
		{
			this.SetBarrier(false);
			this.DispErrorModal(resultCode);
		}
	}

	private void ShowAttachPage()
	{
		NGUITools.SetActiveSelf(this.partsUpperCutinGO, false);
		NGUITools.SetActiveSelf(this.ejectItemCutinGO, false);
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
			NGUITools.SetActiveSelf(this.partsUpperCutinGO, false);
			int loadCount = 0;
			Action<ChipIcon> callback = delegate(ChipIcon chipIcon)
			{
				loadCount++;
				if (loadCount < this.partsUpperCutinTextures.Length)
				{
					NGUITools.SetActiveSelf(this.partsUpperCutinGO, true);
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
			NGUITools.SetActiveSelf(this.ejectItemCutinGO, false);
			NGUITools.SetActiveSelf(this.ejectItemCutinGO, true);
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
			NGUITools.SetActiveSelf(this.ejectItemCutinGO, false);
			NGUITools.SetActiveSelf(this.ejectItemCutinGO, true);
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
