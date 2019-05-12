using System;
using System.Collections;
using System.Collections.Generic;
using UI.Gasha;
using UnityEngine;

public sealed class CMD_ChipGashaResult : CMD
{
	[SerializeField]
	private GashaUserAssetsInventory assetsInventory;

	[SerializeField]
	private GashaStartButtonEvent startButton;

	[Header("アイコン開始位置")]
	[SerializeField]
	private GameObject goICON_START_POS;

	[Header("アイコンオフセット XY")]
	[SerializeField]
	private Vector2 iconOffset;

	[Header("アイコンX方向の数")]
	[SerializeField]
	private int iconNumX;

	[SerializeField]
	[Header("アイコン登場時間(フレーム数)")]
	private int showChipInterval = 16;

	[Header("BLUE エフェクト")]
	[SerializeField]
	private GameObject goEFC_BLUE;

	[Header("GOLD エフェクト")]
	[SerializeField]
	private GameObject goEFC_GOLD;

	[SerializeField]
	[Header("RAINBOW エフェクト")]
	private GameObject goEFC_RAINBOW;

	[Header("BG TEX")]
	[SerializeField]
	public UITexture txBG;

	[SerializeField]
	private float showBonusDelayTime;

	private List<ChipEfc> chipEffectList;

	private int curChipInitNUM;

	private int curChipFrameCT;

	private bool isOnTapped;

	public static CMD_ChipGashaResult instance;

	public static GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo;

	public static GameWebAPI.RespDataGA_ExecChip.UserAssetList[] UserAssetList { get; set; }

	private static List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> UserChipList { get; set; }

	public static GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] RewardsData { get; set; }

	public bool StartEffect { get; set; }

	protected override void Awake()
	{
		CMD_ChipGashaResult.instance = this;
		this.chipEffectList = new List<ChipEfc>();
		base.Awake();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_ChipGashaResult.instance = null;
	}

	private void OnEnable()
	{
		if (base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
		{
			this.assetsInventory.SetGashaPriceType(CMD_ChipGashaResult.gashaInfo.priceType);
			this.startButton.SetGashaInfo(CMD_ChipGashaResult.gashaInfo, false);
			this.startButton.SetPlayButton();
		}
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateAnimationChipIcon();
	}

	private void SetGashaResultDetails()
	{
		GUICollider.DisableAllCollider("=================================== CMD_ChipGashaResult::ShoeDetail");
		this.CreateChipIconEffect();
		this.assetsInventory.SetGashaPriceType(CMD_ChipGashaResult.gashaInfo.priceType);
		this.startButton.SetGashaInfo(CMD_ChipGashaResult.gashaInfo, false);
		this.startButton.SetPlayButton();
	}

	private void CreateChipIconEffect()
	{
		CMD_ChipGashaResult.<CreateChipIconEffect>c__AnonStorey395 <CreateChipIconEffect>c__AnonStorey = new CMD_ChipGashaResult.<CreateChipIconEffect>c__AnonStorey395();
		<CreateChipIconEffect>c__AnonStorey.<>f__this = this;
		<CreateChipIconEffect>c__AnonStorey.i = 0;
		while (<CreateChipIconEffect>c__AnonStorey.i < CMD_ChipGashaResult.UserChipList.Count)
		{
			CMD_ChipGashaResult.<CreateChipIconEffect>c__AnonStorey396 <CreateChipIconEffect>c__AnonStorey2 = new CMD_ChipGashaResult.<CreateChipIconEffect>c__AnonStorey396();
			<CreateChipIconEffect>c__AnonStorey2.<>f__ref$917 = <CreateChipIconEffect>c__AnonStorey;
			<CreateChipIconEffect>c__AnonStorey2.<>f__this = this;
			<CreateChipIconEffect>c__AnonStorey2.effectPosition = this.goICON_START_POS.transform.localPosition;
			float num = (float)(<CreateChipIconEffect>c__AnonStorey.i % this.iconNumX);
			float num2 = (float)(<CreateChipIconEffect>c__AnonStorey.i / this.iconNumX);
			CMD_ChipGashaResult.<CreateChipIconEffect>c__AnonStorey396 <CreateChipIconEffect>c__AnonStorey3 = <CreateChipIconEffect>c__AnonStorey2;
			<CreateChipIconEffect>c__AnonStorey3.effectPosition.x = <CreateChipIconEffect>c__AnonStorey3.effectPosition.x + this.iconOffset.x * num;
			CMD_ChipGashaResult.<CreateChipIconEffect>c__AnonStorey396 <CreateChipIconEffect>c__AnonStorey4 = <CreateChipIconEffect>c__AnonStorey2;
			<CreateChipIconEffect>c__AnonStorey4.effectPosition.y = <CreateChipIconEffect>c__AnonStorey4.effectPosition.y + this.iconOffset.y * num2;
			<CreateChipIconEffect>c__AnonStorey2.effectPosition.z = -5f;
			<CreateChipIconEffect>c__AnonStorey2.chipMaster = ChipDataMng.GetChipMainData(CMD_ChipGashaResult.UserChipList[<CreateChipIconEffect>c__AnonStorey.i].chipId.ToString());
			ChipDataMng.MakePrefabByChipData(<CreateChipIconEffect>c__AnonStorey2.chipMaster, this.goICON_START_POS, <CreateChipIconEffect>c__AnonStorey2.effectPosition, Vector3.one, delegate(ChipIcon icon)
			{
				<CreateChipIconEffect>c__AnonStorey2.<>f__this.OnLoadedChipIcon(CMD_ChipGashaResult.UserAssetList[<CreateChipIconEffect>c__AnonStorey2.<>f__ref$917.i], <CreateChipIconEffect>c__AnonStorey2.chipMaster, icon, <CreateChipIconEffect>c__AnonStorey2.effectPosition);
			}, -1, -1, true);
			<CreateChipIconEffect>c__AnonStorey.i++;
		}
	}

	private void OnLoadedChipIcon(GameWebAPI.RespDataGA_ExecChip.UserAssetList loadChipInfo, GameWebAPI.RespDataMA_ChipM.Chip chipMaster, ChipIcon chipIcon, Vector3 effectPosition)
	{
		GUIListChipParts component = chipIcon.GetComponent<GUIListChipParts>();
		if (null != component)
		{
			component.SetData(new GUIListChipParts.Data());
			component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				if (CMD_ChipGashaResult.UserChipList.Count == this.curChipInitNUM)
				{
					CMD_QuestItemPOP.Create(chipMaster);
				}
				this.isOnTapped = true;
			};
		}
		ChipEfc chipEfc = this.CreateTicketEffect(loadChipInfo.effectType, base.transform, effectPosition);
		Vector3 localScale = chipIcon.transform.localScale;
		chipIcon.transform.parent = chipEfc.goCHIP_THUMB.transform;
		chipIcon.transform.localPosition = chipEfc.goCHIP_THUMB.transform.localPosition;
		chipIcon.transform.localScale = localScale;
		chipEfc.goCHIP_THUMB.GetComponent<UITexture>().enabled = false;
		chipEfc.spNew.enabled = (1 == loadChipInfo.isNew);
		this.chipEffectList.Add(chipEfc);
	}

	private ChipEfc CreateTicketEffect(string effectType, Transform parentTransform, Vector3 effectPosition)
	{
		GameObject original;
		switch (effectType)
		{
		case "2":
			original = this.goEFC_GOLD;
			goto IL_9B;
		case "3":
			original = this.goEFC_RAINBOW;
			goto IL_9B;
		}
		original = this.goEFC_BLUE;
		IL_9B:
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Vector3 localScale = gameObject.transform.localScale;
		gameObject.SetActive(true);
		gameObject.transform.parent = parentTransform;
		gameObject.transform.localPosition = effectPosition;
		gameObject.transform.localScale = localScale;
		ChipEfc component = gameObject.GetComponent<ChipEfc>();
		component.enabled = true;
		return component;
	}

	private IEnumerator GashaRewardSet(float delay)
	{
		yield return new WaitForSeconds(delay);
		CMD_CaptureBonus dialog = GUIMain.ShowCommonDialog(null, "CMD_CaptureBonus", null) as CMD_CaptureBonus;
		dialog.DialogDataSet(CMD_ChipGashaResult.RewardsData);
		dialog.AdjustSize();
		GUICollider.EnableAllCollider("=================================== CMD_ChipGashaResult::ICON");
		yield break;
	}

	private void OnTapped()
	{
		this.isOnTapped = true;
	}

	private void ClearChipIcons()
	{
		for (int i = 0; i < this.chipEffectList.Count; i++)
		{
			UnityEngine.Object.Destroy(this.chipEffectList[i].gameObject);
		}
		this.chipEffectList.Clear();
		this.curChipInitNUM = 0;
		this.curChipFrameCT = 0;
		this.StartEffect = false;
		this.isOnTapped = false;
		this.startButton.gameObject.SetActive(false);
	}

	private void UpdateAnimationChipIcon()
	{
		if (!this.StartEffect)
		{
			return;
		}
		int num = this.curChipFrameCT % this.showChipInterval;
		if ((num == 0 || this.isOnTapped) && CMD_ChipGashaResult.UserChipList.Count > this.curChipInitNUM)
		{
			if (this.curChipInitNUM == 0)
			{
				GUICollider.EnableAllCollider("=================================== CMD_ChipGashaResult::UpdateShowChipIcons");
			}
			this.chipEffectList[this.curChipInitNUM].Play();
			this.curChipInitNUM++;
			SoundMng.Instance().PlaySE("SEInternal/Farm/se_221", 0f, false, true, null, -1, 1f);
			if (CMD_ChipGashaResult.UserChipList.Count == this.curChipInitNUM)
			{
				this.StartEffect = false;
				this.startButton.gameObject.SetActive(true);
				if (CMD_ChipGashaResult.RewardsData != null)
				{
					GUICollider.DisableAllCollider("=================================== CMD_ChipGashaResult::ICON");
					AppCoroutine.Start(this.GashaRewardSet(this.showBonusDelayTime), true);
				}
			}
		}
		this.curChipFrameCT++;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetGashaResultDetails();
		base.Show(f, sizeX, sizeY, aT);
	}

	public static void SetUserChipList(GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList)
	{
		CMD_ChipGashaResult.UserChipList = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(userChipList);
	}

	public static void CreateDialog()
	{
		if (null != CMD_ChipGashaResult.instance)
		{
			CMD_ChipGashaResult.instance.ClearChipIcons();
			CMD_ChipGashaResult.instance.SetGashaResultDetails();
		}
		else
		{
			GUIMain.ShowCommonDialog(null, "CMD_ChipGashaResult", null);
		}
	}
}
