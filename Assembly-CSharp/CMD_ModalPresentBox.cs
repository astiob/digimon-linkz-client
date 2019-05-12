using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebAPIRequest;

public class CMD_ModalPresentBox : CMD
{
	[SerializeField]
	[Header("タイトルラベル")]
	private UILabel lbTitle;

	[SerializeField]
	[Header("残り受取件数")]
	private UILabel lbSubtitle;

	[Header("初期メッセージ")]
	[SerializeField]
	private GameObject goDefaultMessage;

	[SerializeField]
	private UILabel lbDefaultMessage;

	[Header("一括受取ボタン")]
	[SerializeField]
	private GameObject goBtnGetAll;

	[SerializeField]
	private BoxCollider coBtnGetAll;

	[SerializeField]
	private UISprite spBtnGetAll;

	[SerializeField]
	private UILabel lbBtnGetAll;

	[SerializeField]
	[Header("履歴ボタン")]
	private GameObject goBtnHistory;

	[SerializeField]
	private BoxCollider coBtnHistory;

	[SerializeField]
	private UILabel lbBtnHistory;

	[Header("リストパーツ")]
	[SerializeField]
	private GameObject partListParent;

	[SerializeField]
	private GameObject partPresentList;

	[SerializeField]
	private GameObject partHistoryList;

	private GameWebAPI.RespDataPR_PrizeData prizeDataList;

	private GUISelectPresentBoxPanel csPartPresentListParent;

	public CMD_ModalPresentBox.DISPLAY_BOX_TYPE displayBoxType;

	private List<GameWebAPI.RespDataPR_PrizeData.PrizeData> candidateList = new List<GameWebAPI.RespDataPR_PrizeData.PrizeData>();

	private Dictionary<string, int> receivedPresentCountList = new Dictionary<string, int>();

	private Dictionary<string, int> limitPresentCountList = new Dictionary<string, int>();

	private static CMD_ModalPresentBox instance;

	private bool isReceived;

	public int nowPage;

	public static CMD_ModalPresentBox Instance
	{
		get
		{
			return CMD_ModalPresentBox.instance;
		}
	}

	protected override void Awake()
	{
		CMD_ModalPresentBox.instance = this;
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.HideDLG();
		this.SetCommonUI();
		this.InitPresentBox(f, sizeX, sizeY, aT);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.isReceived)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			APIRequestTask apirequestTask = Singleton<UserDataMng>.Instance.RequestPlayerInfo(true);
			apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestUserSoulData(true)).Add(this.RequestUserMonster()).Add(DataMng.Instance().RequestMyPageData(true)).Add(ChipDataMng.RequestAPIChipList(true)).Add(ChipDataMng.RequestAPIMonsterSlotInfoList(true)).Add(TitleDataMng.RequestAPIUsetTitleList(true));
			base.StartCoroutine(apirequestTask.Run(delegate
			{
				GUIPlayerStatus.RefreshParams_S(true);
				ClassSingleton<FacePresentAccessor>.Instance.facePresent.SetBadgeOnly();
				ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
				RestrictionInput.EndLoad();
				this.ClosePanel(animation);
			}, null, null));
		}
		else
		{
			base.ClosePanel(animation);
		}
	}

	private APIRequestTask RequestUserMonster()
	{
		GameWebAPI.RequestMonsterList requestMonsterList = new GameWebAPI.RequestMonsterList();
		requestMonsterList.OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.RefreshUserMonsterData(response.userMonsterList);
		};
		GameWebAPI.RequestMonsterList request = requestMonsterList;
		return new APIRequestTask(request, true);
	}

	private void SetCommonUI()
	{
		this.lbSubtitle.text = string.Format(StringMaster.GetString("Present-01"), 0);
		this.lbBtnGetAll.text = StringMaster.GetString("Present-12");
		this.csPartPresentListParent = this.partListParent.GetComponent<GUISelectPresentBoxPanel>();
	}

	private void InitPresentBox(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.nowPage = 1;
		this.GetPresentBoxData(this.nowPage, true, delegate
		{
			this.displayBoxType = CMD_ModalPresentBox.DISPLAY_BOX_TYPE.PRESENT;
			this.isReceived = false;
			this.ChangeDisplayBox();
			this.ShowDLG();
			this.Show(f, sizeX, sizeY, aT);
		});
	}

	public void GetPresentBoxData(int page, bool isGetHistory, Action callback)
	{
		if (GUIManager.CheckTopDialog("CMD_ModalMessage", null) == null)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		}
		RequestList requestList = new RequestList();
		RequestBase addRequest = new GameWebAPI.RequestPR_PrizeList
		{
			SetSendData = delegate(GameWebAPI.PR_Req_PrizeData param)
			{
				param.page = page;
			},
			OnReceived = delegate(GameWebAPI.RespDataPR_PrizeData response)
			{
				if (page == 1)
				{
					this.prizeDataList = response;
				}
				else
				{
					List<GameWebAPI.RespDataPR_PrizeData.PrizeData> list = new List<GameWebAPI.RespDataPR_PrizeData.PrizeData>();
					list = this.prizeDataList.prizeData.ToList<GameWebAPI.RespDataPR_PrizeData.PrizeData>();
					list.AddRange(response.prizeData.ToList<GameWebAPI.RespDataPR_PrizeData.PrizeData>());
					this.prizeDataList.prizeData = list.ToArray();
				}
			}
		};
		requestList.AddRequest(addRequest);
		if (isGetHistory)
		{
			GameWebAPI.RequestPR_PrizeReceiveHistory requestPR_PrizeReceiveHistory = new GameWebAPI.RequestPR_PrizeReceiveHistory();
			requestPR_PrizeReceiveHistory.OnReceived = delegate(GameWebAPI.RespDataPR_PrizeReceiveHistory response)
			{
				DataMng.Instance().RespDataPR_PrizeReceiveHistory = response;
			};
			addRequest = requestPR_PrizeReceiveHistory;
			requestList.AddRequest(addRequest);
		}
		base.StartCoroutine(requestList.RunOneTime(delegate()
		{
			if (callback != null)
			{
				callback();
			}
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(false);
		}, null));
	}

	public void ChangeDisplayBox()
	{
		CMD_ModalPresentBox.DISPLAY_BOX_TYPE display_BOX_TYPE = this.displayBoxType;
		if (display_BOX_TYPE != CMD_ModalPresentBox.DISPLAY_BOX_TYPE.PRESENT)
		{
			if (display_BOX_TYPE == CMD_ModalPresentBox.DISPLAY_BOX_TYPE.HISTORY)
			{
				this.lbTitle.text = StringMaster.GetString("OtherHistory-02");
				this.lbBtnHistory.text = StringMaster.GetString("SystemButtonReturn");
				this.lbDefaultMessage.text = StringMaster.GetString("Present-07");
				this.goBtnGetAll.SetActive(false);
				this.DispPresentHistory();
			}
		}
		else
		{
			this.lbTitle.text = StringMaster.GetString("PresentTitle");
			this.lbSubtitle.text = string.Format(StringMaster.GetString("Present-01"), this.prizeDataList.prizeTotalCount);
			this.lbBtnHistory.text = StringMaster.GetString("Present-06");
			this.lbDefaultMessage.text = StringMaster.GetString("Present-02");
			this.goBtnGetAll.SetActive(true);
			this.SetGetAllBtn();
			this.DispPresentBox(false);
		}
	}

	private void SetGetAllBtn()
	{
		if (int.Parse(this.prizeDataList.prizeTotalCount) > 0)
		{
			this.coBtnGetAll.enabled = true;
			this.spBtnGetAll.spriteName = "Common02_Btn_Red";
		}
		else
		{
			this.coBtnGetAll.enabled = false;
			this.spBtnGetAll.spriteName = "Common02_Btn_Gray";
		}
	}

	public void DispPresentBox(bool isPaging)
	{
		if (this.prizeDataList.prizeData.Length > 0)
		{
			this.goDefaultMessage.SetActive(false);
			this.csPartPresentListParent.selectParts = this.partPresentList;
			if (!isPaging)
			{
				this.csPartPresentListParent.initLocation = true;
			}
			this.partListParent.SetActive(true);
			this.partPresentList.SetActive(true);
			this.csPartPresentListParent.AllBuild(this.prizeDataList);
			this.partPresentList.SetActive(false);
		}
		else
		{
			this.partListParent.SetActive(false);
			this.partPresentList.SetActive(false);
			this.goDefaultMessage.SetActive(true);
		}
	}

	private void DispPresentHistory()
	{
		if (DataMng.Instance().RespDataPR_PrizeReceiveHistory.prizeReceiveHistory.Length > 0)
		{
			this.goDefaultMessage.SetActive(false);
			this.csPartPresentListParent.selectParts = this.partHistoryList;
			this.partListParent.SetActive(true);
			this.partHistoryList.SetActive(true);
			this.csPartPresentListParent.initLocation = true;
			this.csPartPresentListParent.AllBuildHistory(DataMng.Instance().RespDataPR_PrizeReceiveHistory);
			this.partHistoryList.SetActive(false);
		}
		else
		{
			this.partListParent.SetActive(false);
			this.partHistoryList.SetActive(false);
			this.goDefaultMessage.SetActive(true);
		}
	}

	private void OnClickChangeDisplay()
	{
		CMD_ModalPresentBox.DISPLAY_BOX_TYPE display_BOX_TYPE = this.displayBoxType;
		if (display_BOX_TYPE != CMD_ModalPresentBox.DISPLAY_BOX_TYPE.PRESENT)
		{
			if (display_BOX_TYPE == CMD_ModalPresentBox.DISPLAY_BOX_TYPE.HISTORY)
			{
				this.displayBoxType = CMD_ModalPresentBox.DISPLAY_BOX_TYPE.PRESENT;
			}
		}
		else
		{
			this.displayBoxType = CMD_ModalPresentBox.DISPLAY_BOX_TYPE.HISTORY;
		}
		this.ChangeDisplayBox();
	}

	private void OnClickGetAll()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		string[] receiveIds = new string[this.prizeDataList.prizeData.Length];
		this.candidateList.Clear();
		for (int i = 0; i < this.prizeDataList.prizeData.Length; i++)
		{
			this.candidateList.Add(this.prizeDataList.prizeData[i]);
			receiveIds[i] = this.prizeDataList.prizeData[i].receiveId;
		}
		GameWebAPI.RequestPR_PrizeReceive request = new GameWebAPI.RequestPR_PrizeReceive
		{
			SetSendData = delegate(GameWebAPI.PR_Req_PrizeReceiveIds param)
			{
				param.receiveType = 2;
				param.receiveIds = receiveIds;
			},
			OnReceived = delegate(GameWebAPI.RespDataPR_PrizeReceiveIds response)
			{
				this.DispReceiveResultModal(response);
			}
		};
		base.StartCoroutine(request.RunOneTime(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private IEnumerator AfterReceiveAction()
	{
		this.nowPage = 1;
		yield return base.StartCoroutine(Util.WaitForRealTime(0.5f));
		this.GetPresentBoxData(this.nowPage, true, delegate
		{
			this.ChangeDisplayBox();
		});
		yield break;
	}

	public void DispReceiveResultModal(GameWebAPI.RespDataPR_PrizeReceiveIds data)
	{
		if (data.resultCode != 1)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("Present-13");
			cmd_ModalMessage.Info = StringMaster.GetString(string.Format("PresentRecieveResultCode-{0}", data.resultCode));
			cmd_ModalMessage.AdjustSize();
			return;
		}
		this.SetAfterReceiveParams(data);
		if (data.prizeReceiveIds.Length > 0)
		{
			CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(new Action<int>(this.DispLimitResultModal), "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage2.Title = StringMaster.GetString("Present-03");
			cmd_ModalMessage2.Info = this.GetPresentCountDescription(this.receivedPresentCountList);
			cmd_ModalMessage2.AdjustSize();
			foreach (string receiveId in data.prizeReceiveIds)
			{
				GameWebAPI.RespDataPR_PrizeData.PrizeData prizeData = this.prizeDataList.prizeData.SingleOrDefault((GameWebAPI.RespDataPR_PrizeData.PrizeData x) => x.receiveId == receiveId);
				if (prizeData != null && int.Parse(prizeData.assetCategoryId) == 16)
				{
					Singleton<UserDataMng>.Instance.ClearUserFacilityCondition();
					break;
				}
			}
		}
		else
		{
			this.DispLimitResultModal(0);
		}
		base.StartCoroutine(this.AfterReceiveAction());
	}

	private void DispLimitResultModal(int idx)
	{
		if (this.limitPresentCountList.Count > 0)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("Present-08");
			cmd_ModalMessage.Info = StringMaster.GetString("Present-09") + this.GetPresentCountDescription(this.limitPresentCountList);
			cmd_ModalMessage.AdjustSize();
		}
	}

	private void SetAfterReceiveParams(GameWebAPI.RespDataPR_PrizeReceiveIds data)
	{
		this.isReceived = true;
		this.SetReceivedPresentList(data);
	}

	public void SetCandidateList(GameWebAPI.RespDataPR_PrizeData.PrizeData data)
	{
		this.candidateList.Clear();
		this.candidateList.Add(data);
	}

	private void SetReceivedPresentList(GameWebAPI.RespDataPR_PrizeReceiveIds data)
	{
		this.receivedPresentCountList.Clear();
		this.limitPresentCountList.Clear();
		foreach (GameWebAPI.RespDataPR_PrizeData.PrizeData prizeData in this.candidateList)
		{
			bool flag = data.prizeReceiveIds.Contains(prizeData.receiveId);
			string assetCategoryId = prizeData.assetCategoryId;
			int num = int.Parse(prizeData.assetNum);
			int itemId = int.Parse(prizeData.assetValue);
			if (flag)
			{
				if (this.receivedPresentCountList.ContainsKey(assetCategoryId))
				{
					Dictionary<string, int> dictionary2;
					Dictionary<string, int> dictionary = dictionary2 = this.receivedPresentCountList;
					string key2;
					string key = key2 = assetCategoryId;
					int num2 = dictionary2[key2];
					dictionary[key] = num2 + num;
				}
				else
				{
					this.receivedPresentCountList.Add(assetCategoryId, num);
				}
				if (assetCategoryId.ToInt32() == 6)
				{
					Singleton<UserDataMng>.Instance.UpdateUserItemNum(itemId, num);
				}
			}
			else if (this.limitPresentCountList.ContainsKey(assetCategoryId))
			{
				Dictionary<string, int> dictionary4;
				Dictionary<string, int> dictionary3 = dictionary4 = this.limitPresentCountList;
				string key2;
				string key3 = key2 = assetCategoryId;
				int num2 = dictionary4[key2];
				dictionary3[key3] = num2 + num;
			}
			else
			{
				this.limitPresentCountList.Add(assetCategoryId, num);
			}
		}
	}

	private string GetPresentCountDescription(Dictionary<string, int> assetList)
	{
		string text = string.Empty;
		GameWebAPI.RespDataMA_GetAssetCategoryM respDataMA_AssetCategoryM = MasterDataMng.Instance().RespDataMA_AssetCategoryM;
		foreach (KeyValuePair<string, int> keyValuePair in assetList)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = respDataMA_AssetCategoryM.GetAssetCategory(keyValuePair.Key);
			string arg;
			if (keyValuePair.Key.ToInt32() == 4)
			{
				arg = StringFormat.Cluster(keyValuePair.Value);
			}
			else
			{
				arg = keyValuePair.Value.ToString();
			}
			text += string.Format(StringMaster.GetString("SystemItemCount"), assetCategory.assetTitle, arg);
		}
		return text;
	}

	public enum DISPLAY_BOX_TYPE
	{
		PRESENT,
		HISTORY
	}
}
