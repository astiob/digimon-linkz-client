using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_PointQuestRanking : CMD
{
	[SerializeField]
	private GUISelectPanelQuestRanking csSelectPanel;

	[SerializeField]
	private GUIListPartsQuestRanking csParts;

	[SerializeField]
	private GUIListPartsQuestRankingRewardList myRank;

	[SerializeField]
	private GUIListPartsQuestRankingRewardList nextRank;

	[SerializeField]
	private int debugRankingNo;

	private GameWebAPI.RespDataMS_PointQuestRankingList pointQuestRankingList;

	private GameWebAPI.RespDataMS_PointQuestRankingList.RankingData myData;

	private List<string> keysListStartValue;

	private List<string> keysList;

	private List<int> valsList;

	private int listIdx;

	private int limitRank;

	private bool amIOutRange;

	public static CMD_PointQuestRanking instance;

	public static GameWebAPI.RespDataWD_PointQuestInfo PointQuestInfo { private get; set; }

	public GameWebAPI.RespDataMS_PointQuestRankingList GetPointQuestRankingList()
	{
		return this.pointQuestRankingList;
	}

	public int GetListIdx()
	{
		return this.listIdx;
	}

	public bool GetIsMine(int idx)
	{
		bool result = false;
		if (this.pointQuestRankingList.myRankingNo > 0)
		{
			if (idx > 0)
			{
				if (this.pointQuestRankingList.myRankingNo <= int.Parse(this.keysList[idx]) && this.pointQuestRankingList.myRankingNo > int.Parse(this.keysList[idx - 1]))
				{
					result = true;
				}
			}
			else if (this.pointQuestRankingList.myRankingNo <= int.Parse(this.keysList[idx]))
			{
				result = true;
			}
			if (idx == this.keysList.Count - 1 && this.amIOutRange)
			{
				result = true;
			}
		}
		else if (idx == this.keysList.Count - 1)
		{
			result = true;
		}
		return result;
	}

	public string[] GetPointRankingKey(int idx)
	{
		return new string[]
		{
			this.keysListStartValue[idx],
			this.keysList[idx]
		};
	}

	public int GetPointRankingValue(int idx)
	{
		return this.valsList[idx];
	}

	public int GetlimitRank()
	{
		return this.limitRank;
	}

	public int GetNextPoint()
	{
		return this.pointQuestRankingList.pointToNextRank;
	}

	public GameWebAPI.RespDataMS_PointQuestRankingList.RankingData GetData()
	{
		return this.myData;
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_PointQuestRanking.instance = this;
	}

	protected override void WindowClosed()
	{
		base.WindowClosed();
		CMD_PointQuestRanking.instance = null;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.StartCoroutine(this.InitDLG(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitDLG(Action<int> f, float sizeX, float sizeY, float aT)
	{
		bool isOK = false;
		GameWebAPI.PointQuestRankingList pointQuestRankingList = new GameWebAPI.PointQuestRankingList();
		pointQuestRankingList.SetSendData = delegate(GameWebAPI.ReqDataMS_PointQuestRankingList param)
		{
			param.worldEventId = CMD_PointQuestRanking.PointQuestInfo.worldEventId;
		};
		pointQuestRankingList.OnReceived = delegate(GameWebAPI.RespDataMS_PointQuestRankingList response)
		{
			this.pointQuestRankingList = this.ConvertFromPointQuestRankingToPvPRanking(response);
		};
		GameWebAPI.PointQuestRankingList request = pointQuestRankingList;
		yield return base.StartCoroutine(request.RunOneTime(delegate()
		{
			isOK = true;
		}, delegate(Exception exception)
		{
			isOK = false;
		}, null));
		if (isOK)
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("PointQuestRankingTitle"));
			base.ShowDLG();
			this.InitUI();
			base.Show(f, sizeX, sizeY, aT);
		}
		else
		{
			base.SetCloseAction(null);
			base.ClosePanel(false);
		}
		RestrictionInput.EndLoad();
		yield break;
	}

	private void InitUI()
	{
		CMD_PvPRanking.Mode = CMD_PvPRanking.MODE.PointQuest;
		this.myData = this.GetMyData();
		this.csParts.SetData();
		this.csParts.ShowGUI();
		this.myRank.ShowGUI();
		this.nextRank.ShowGUI();
		this.csSelectPanel.AllBuild(this.keysList.Count, true, 1f, 1f, null, null);
		this.csSelectPanel.SetLocationByIDX(this.listIdx, 0f);
		if (!this.csParts.gameObject.activeSelf)
		{
			this.csParts.gameObject.SetActive(true);
		}
	}

	private GameWebAPI.RespDataMS_PointQuestRankingList.RankingData GetMyData()
	{
		this.myData = default(GameWebAPI.RespDataMS_PointQuestRankingList.RankingData);
		this.myData.userId = DataMng.Instance().UserId;
		if (string.IsNullOrEmpty(this.pointQuestRankingList.myRankingNo.ToString()) || this.pointQuestRankingList.myRankingNo <= 0)
		{
			this.myData.rank = "0";
		}
		else
		{
			this.myData.rank = this.pointQuestRankingList.myRankingNo.ToString();
		}
		this.myData.score = CMD_PointQuestRanking.PointQuestInfo.currentPoint.ToString();
		this.myData.nickname = DataMng.Instance().UserName;
		this.myData.iconId = DataMng.Instance().RespDataPRF_Profile.monsterData.monsterId;
		return this.myData;
	}

	private GameWebAPI.RespDataMS_PointQuestRankingList ConvertFromPointQuestRankingToPvPRanking(GameWebAPI.RespDataMS_PointQuestRankingList PointQuestRanking)
	{
		GameWebAPI.RespDataMS_PointQuestRankingList respDataMS_PointQuestRankingList = new GameWebAPI.RespDataMS_PointQuestRankingList();
		respDataMS_PointQuestRankingList.resultCode = PointQuestRanking.resultCode;
		respDataMS_PointQuestRankingList.myRankingNo = PointQuestRanking.myRankingNo;
		respDataMS_PointQuestRankingList.pointToNextRank = PointQuestRanking.pointToNextRank;
		respDataMS_PointQuestRankingList.myRankRewardList = PointQuestRanking.myRankRewardList;
		respDataMS_PointQuestRankingList.nextRankRewardList = PointQuestRanking.nextRankRewardList;
		respDataMS_PointQuestRankingList.pointRankingList = PointQuestRanking.pointRankingList;
		this.testLog(respDataMS_PointQuestRankingList);
		respDataMS_PointQuestRankingList.pointRankingList["-1"] = 0;
		this.keysList = new List<string>(respDataMS_PointQuestRankingList.pointRankingList.Keys);
		this.valsList = new List<int>(respDataMS_PointQuestRankingList.pointRankingList.Values);
		this.keysListStartValue = new List<string>();
		this.keysListStartValue.Add("1");
		for (int i = 1; i < respDataMS_PointQuestRankingList.pointRankingList.Count; i++)
		{
			int num = int.Parse(this.keysList[i - 1]) + 1;
			this.keysListStartValue.Add(num.ToString());
		}
		for (int j = 0; j < respDataMS_PointQuestRankingList.pointRankingList.Count - 1; j++)
		{
		}
		if (respDataMS_PointQuestRankingList.myRankingNo > 0)
		{
			for (int k = 0; k < this.keysList.Count; k++)
			{
				if (respDataMS_PointQuestRankingList.myRankingNo > int.Parse(this.keysList[k]) && int.Parse(this.keysList[k]) > 0)
				{
					this.listIdx = k + 1;
				}
			}
			if (int.Parse(this.keysList[this.keysList.Count - 2]) < respDataMS_PointQuestRankingList.myRankingNo)
			{
				this.listIdx = this.keysList.Count - 1;
				this.amIOutRange = true;
				global::Debug.LogFormat("PointQuestRanking | 圏外 km", new object[0]);
			}
			else
			{
				global::Debug.LogFormat("PointQuestRanking | ランク内:{0}/{1} (0~{1}) km", new object[]
				{
					this.listIdx,
					this.keysList.Count - 1
				});
			}
		}
		else
		{
			this.listIdx = this.keysList.Count - 1;
			this.amIOutRange = true;
			global::Debug.LogFormat("PointQuestRanking | 不参加 km", new object[0]);
		}
		this.limitRank = int.Parse(this.keysList[this.keysList.Count - 2]);
		if (this.listIdx == 0)
		{
			respDataMS_PointQuestRankingList.pointToNextRank = 0;
		}
		return respDataMS_PointQuestRankingList;
	}

	private void testData()
	{
	}

	private void testLog(GameWebAPI.RespDataMS_PointQuestRankingList pqRanking)
	{
	}
}
