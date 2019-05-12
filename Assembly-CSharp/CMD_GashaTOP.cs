using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UI.Gasha;
using UnityEngine;
using User;

public class CMD_GashaTOP : CMD, ITutorialControl
{
	public static CMD_GashaTOP instance;

	[SerializeField]
	private GashaUserAssetsInventory userInventory;

	[SerializeField]
	private GashaStartButtonEvent startButton;

	[SerializeField]
	private GashaMainInfomation mainInfomation;

	[SerializeField]
	private GUISelectPanelGashaMain gashaButtonList;

	[SerializeField]
	private GameObject gashaButtonResource;

	private GashaInfoManager gashaInfoManager;

	private string defaultShowGashaId;

	private int selectGashaButtonIndex;

	private Texture[] gashaButtonTextureList;

	private bool isTutorial;

	protected override void Awake()
	{
		CMD_GashaTOP.instance = this;
		base.Awake();
	}

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.StartCoroutine(this.InitDLG(closeEvent, sizeX, sizeY, showAnimationTime));
	}

	private IEnumerator InitDLG(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		int countryCode;
		if (int.TryParse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN), out countryCode))
		{
			bool requestRetry = this.isTutorial;
			GameWebAPI.RequestGA_GashaInfo request = new GameWebAPI.RequestGA_GashaInfo
			{
				SetSendData = delegate(GameWebAPI.GA_Req_GashaInfo param)
				{
					int type;
					param.isTutorial = type;
					param.countryCode = countryCode;
				},
				OnReceived = delegate(GameWebAPI.RespDataGA_GetGachaInfo response)
				{
					this.gashaInfoManager = new GashaInfoManager(response);
				}
			};
			APIRequestTask task = new APIRequestTask(request, requestRetry);
			yield return base.StartCoroutine(task.Run(delegate
			{
				this.OnSuccessRequestGashaInfo(closeEvent, sizeX, sizeY, showAnimationTime);
			}, new Action<Exception>(this.OnFailedRequestGashaInfo), null));
		}
		else
		{
			global::Debug.LogError("国コードの取得に失敗.");
			if (this.isTutorial)
			{
				base.SetCloseAction(delegate(int nop)
				{
					GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
				});
			}
			else
			{
				base.ClosePanel(false);
			}
		}
		yield break;
	}

	private void OnEnable()
	{
		if (base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
		{
			List<int> list = this.gashaInfoManager.RemoveExcessGasha();
			if (0 < list.Count)
			{
				this.CreateGashaButtonList(this.gashaInfoManager.GetInfoList(), this.gashaButtonTextureList, this.selectGashaButtonIndex, this.isTutorial);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] == this.selectGashaButtonIndex)
					{
						this.selectGashaButtonIndex = 0;
						break;
					}
				}
				this.gashaButtonList.SetCellAnim(this.selectGashaButtonIndex);
				this.ChangeSelection(this.selectGashaButtonIndex);
			}
			else
			{
				this.gashaButtonList.RefreshAbleCount();
				this.SetGashaDetailed(this.selectGashaButtonIndex);
			}
		}
	}

	private void OnSuccessRequestGashaInfo(Action<int> closeEvent, float sizeX, float sizeY, float showTime)
	{
		Singleton<UserDataMng>.Instance.RequestUserStockFacilityDataAPI(delegate(bool isSuccess)
		{
			if (isSuccess)
			{
				if (!this.isTutorial)
				{
					int facilityId = 25;
					if (!Singleton<UserDataMng>.Instance.ExistInBuildFacility(facilityId) && !Singleton<UserDataMng>.Instance.ExistInUserStockFacility(facilityId))
					{
						this.gashaInfoManager.RemoveChipGasha();
					}
					this.gashaInfoManager.SortInfo();
				}
				DownloadGashaTopTex.Instance.DownloadTexture(this.gashaInfoManager.GetInfoList(), delegate(Texture[] textureList)
				{
					this.gashaButtonTextureList = textureList;
					this.ShowDLG();
					SoundMng.Instance().PlayGameBGM("bgm_202");
					RestrictionInput.EndLoad();
					this.mainInfomation.SetTitleParts(this.PartsTitle);
					this.selectGashaButtonIndex = this.GetDefaultShowGashaIndex(this.gashaInfoManager.GetInfoList());
					this.gashaButtonList.Create(this.gashaButtonResource);
					this.CreateGashaButtonList(this.gashaInfoManager.GetInfoList(), this.gashaButtonTextureList, this.selectGashaButtonIndex, this.isTutorial);
					this.ChangeSelection(this.selectGashaButtonIndex);
					this.Show(closeEvent, sizeX, sizeY, showTime);
				});
			}
			else
			{
				RestrictionInput.EndLoad();
				if (this.isTutorial)
				{
					this.SetCloseAction(delegate(int nop)
					{
						GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
					});
				}
				else
				{
					this.ClosePanel(false);
				}
			}
		});
	}

	private void OnFailedRequestGashaInfo(Exception noop)
	{
		RestrictionInput.EndLoad();
		if (this.isTutorial)
		{
			base.SetCloseAction(delegate(int nop)
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			});
		}
		else
		{
			base.ClosePanel(false);
		}
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (UserHomeInfo.dirtyMyPage && base.gameObject.activeSelf && !this.isTutorial)
		{
			base.StartCoroutine(this.RequestMyPageData(animation));
		}
		else
		{
			if (this.isTutorial)
			{
				UserHomeInfo.dirtyMyPage = false;
			}
			this.Finalize(animation);
		}
	}

	private IEnumerator RequestMyPageData(bool animation)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask task = DataMng.Instance().RequestMyPageData(false);
		return task.Run(delegate
		{
			ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge(true);
			this.Finalize(animation);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			this.Finalize(animation);
			RestrictionInput.EndLoad();
		}, null);
	}

	private void Finalize(bool animation)
	{
		if (null != CMD_MonsterGashaResult.instance)
		{
			CMD_MonsterGashaResult.instance.ClosePanel(true);
		}
		SoundMng.Instance().PlayGameBGM("bgm_201");
		this.gashaButtonList.FadeOutAllListParts(null, false);
		GUIPlayerStatus.RefreshParams_S(false);
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		if (this.gashaButtonTextureList != null)
		{
			for (int i = 0; i < this.gashaButtonTextureList.Length; i++)
			{
				this.gashaButtonTextureList[i] = null;
			}
			this.gashaButtonTextureList = null;
		}
		base.OnDestroy();
		CMD_GashaTOP.instance = null;
	}

	private void CreateGashaButtonList(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaInfoList, Texture[] textureList, int selectedButtonIndex, bool isTutorial)
	{
		if (!this.gashaButtonResource.activeSelf)
		{
			this.gashaButtonResource.SetActive(true);
		}
		this.gashaButtonList.AllBuild(gashaInfoList, textureList, new Action<int>(this.ChangeSelection), selectedButtonIndex, isTutorial);
		this.gashaButtonResource.SetActive(false);
	}

	private void ChangeSelection(int selectGashaIndex)
	{
		this.selectGashaButtonIndex = selectGashaIndex;
		GameWebAPI.RespDataGA_GetGachaInfo.Result info = this.gashaInfoManager.GetInfo(selectGashaIndex);
		if (info != null)
		{
			this.mainInfomation.SetGashaInfo(info);
			this.SetGashaDetailed(selectGashaIndex);
			LeadCapture.Instance.SaveCaptureUpdate(this.gashaInfoManager.GetEndTimeList());
		}
	}

	private void SetGashaDetailed(int selectGashaIndex)
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result info = this.gashaInfoManager.GetInfo(selectGashaIndex);
		if (info != null)
		{
			this.startButton.SetGashaInfo(info, this.isTutorial);
			this.startButton.SetPlayButton();
			this.userInventory.SetGashaPriceType(info.priceType);
		}
	}

	public void SetSelectGashaId(string gashaId)
	{
		this.defaultShowGashaId = gashaId;
	}

	private int GetDefaultShowGashaIndex(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaInfoList)
	{
		int result = 0;
		if (!string.IsNullOrEmpty(this.defaultShowGashaId))
		{
			for (int i = 0; i < gashaInfoList.Count; i++)
			{
				if (gashaInfoList[i].gachaId == this.defaultShowGashaId)
				{
					result = i;
					break;
				}
			}
		}
		return result;
	}

	public void SetTutorialParameter(Dictionary<string, Action> passTutorialAction)
	{
		this.isTutorial = true;
		if (passTutorialAction.ContainsKey("GashaConfirmDialog"))
		{
			string key2;
			string key = key2 = "GashaConfirmDialog";
			Action a = passTutorialAction[key2];
			passTutorialAction[key] = (Action)Delegate.Combine(a, new Action(this.startButton.OnPushedOneButton));
		}
		else
		{
			passTutorialAction.Add("GashaConfirmDialog", new Action(this.startButton.OnPushedOneButton));
		}
	}
}
