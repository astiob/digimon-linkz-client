using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_PvPRanking : CMD
{
	[SerializeField]
	[Header("ランキングが無いことを示すラベル")]
	private UILabel lbTX_None;

	[SerializeField]
	private GUISelectPanelPvPRanking csSelectPanel;

	[SerializeField]
	[Header("PvPのランキング時")]
	private GUIListPartsPvPRanking pvpParts;

	[Header("ポイントクエスト時")]
	[SerializeField]
	private GUIListPartsPvPRanking pqParts;

	private GameWebAPI.RespDataCL_Ranking RankingData;

	private List<GameWebAPI.RespDataCL_Ranking.RankingDataList> RankingDataL;

	public static CMD_PvPRanking instance;

	public static CMD_PvPRanking.MODE Mode { get; set; }

	public static GameWebAPI.RespDataWD_PointQuestInfo PointQuestInfo { private get; set; }

	private GUIListPartsPvPRanking csParts
	{
		get
		{
			if (CMD_PvPRanking.Mode == CMD_PvPRanking.MODE.PvP)
			{
				return this.pvpParts;
			}
			return this.pqParts;
		}
	}

	public GameWebAPI.RespDataCL_Ranking.RankingDataList GetData(int idx)
	{
		return this.RankingDataL[idx];
	}

	public void RemoveDataAt(int idx)
	{
		this.RankingDataL.RemoveAt(idx);
	}

	public void AddDataAt(int idx)
	{
		this.RankingDataL.Add(this.RankingDataL[idx]);
	}

	public void InsertDataAt(int idx)
	{
		this.RankingDataL.Insert(idx, this.RankingDataL[4]);
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_PvPRanking.instance = this;
	}

	protected override void WindowClosed()
	{
		base.WindowClosed();
		CMD_PvPRanking.instance = null;
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
		if (CMD_PvPRanking.Mode == CMD_PvPRanking.MODE.PvP)
		{
			NGUITools.SetActiveSelf(this.pqParts.gameObject, false);
			GameWebAPI.RequestCL_Ranking request = new GameWebAPI.RequestCL_Ranking
			{
				OnReceived = delegate(GameWebAPI.RespDataCL_Ranking response)
				{
					this.RankingData = response;
				}
			};
			yield return base.StartCoroutine(request.RunOneTime(delegate()
			{
				isOK = true;
			}, delegate(Exception exception)
			{
				isOK = false;
			}, null));
		}
		else if (CMD_PvPRanking.Mode == CMD_PvPRanking.MODE.PointQuest)
		{
			NGUITools.SetActiveSelf(this.pvpParts.gameObject, false);
			GameWebAPI.PointQuestRankingList pointQuestRankingList = new GameWebAPI.PointQuestRankingList();
			pointQuestRankingList.SetSendData = delegate(GameWebAPI.ReqDataMS_PointQuestRankingList param)
			{
				param.worldEventId = CMD_PvPRanking.PointQuestInfo.worldEventId;
			};
			pointQuestRankingList.OnReceived = delegate(GameWebAPI.RespDataMS_PointQuestRankingList response)
			{
				this.RankingData = this.ConvertFromPointQuestRankingToPvPRanking(response);
			};
			GameWebAPI.PointQuestRankingList request2 = pointQuestRankingList;
			yield return base.StartCoroutine(request2.RunOneTime(delegate()
			{
				isOK = true;
			}, delegate(Exception exception)
			{
				isOK = false;
			}, null));
		}
		if (isOK)
		{
			base.ShowDLG();
			base.PartsTitle.SetTitle(StringMaster.GetString("ColosseumRankTitle"));
			this.SetCommonUI();
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

	private void SetCommonUI()
	{
	}

	private void InitUI()
	{
		this.RankingDataL = new List<GameWebAPI.RespDataCL_Ranking.RankingDataList>();
		GameWebAPI.RespDataCL_Ranking.RankingDataList myData = this.GetMyData(true);
		if (this.RankingData.rankingDataList != null)
		{
			if (this.RankingData.rankingDataList.Length > 0)
			{
			}
			GameWebAPI.RespDataCL_Ranking.RankingDataList rankingDataList = null;
			for (int i = 0; i < this.RankingData.rankingDataList.Length; i++)
			{
				if (this.RankingData.rankingDataList[i].userId == DataMng.Instance().UserId)
				{
					this.RankingData.rankingDataList[i].isMine = true;
					rankingDataList = this.RankingData.rankingDataList[i];
				}
				else
				{
					this.RankingData.rankingDataList[i].isMine = false;
				}
				this.RankingDataL.Add(this.RankingData.rankingDataList[i]);
			}
			this.RankingDataL.Sort(new Comparison<GameWebAPI.RespDataCL_Ranking.RankingDataList>(this.CompareRanking));
			this.csSelectPanel.AllBuild(this.RankingDataL.Count, true, 1f, 1f, null, null);
			if (!this.csParts.gameObject.activeSelf)
			{
				this.csParts.gameObject.SetActive(true);
			}
			if (rankingDataList != null)
			{
				this.csParts.Data = rankingDataList;
			}
			else
			{
				this.csParts.Data = myData;
			}
		}
		else
		{
			this.csParts.Data = myData;
		}
	}

	private int CompareRanking(GameWebAPI.RespDataCL_Ranking.RankingDataList x, GameWebAPI.RespDataCL_Ranking.RankingDataList y)
	{
		int num = int.Parse(x.rank);
		int num2 = int.Parse(y.rank);
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		if (this.RankingDataL.Count <= 0)
		{
			this.lbTX_None.gameObject.SetActive(true);
			this.lbTX_None.text = StringMaster.GetString("ColosseumRankNotFound");
		}
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
		this.lbTX_None.gameObject.SetActive(false);
		this.lbTX_None.text = string.Empty;
	}

	private GameWebAPI.RespDataCL_Ranking.RankingDataList GetMyData(bool isMine = true)
	{
		if (CMD_PvPRanking.Mode == CMD_PvPRanking.MODE.PvP && CMD_PvPTop.Instance != null)
		{
			GameWebAPI.ColosseumUserStatus colosseumUserStatus = CMD_PvPTop.Instance.ColosseumUserStatus;
			GameWebAPI.RespDataCL_Ranking.RankingDataList rankingDataList = new GameWebAPI.RespDataCL_Ranking.RankingDataList();
			int num = colosseumUserStatus.winWeek + colosseumUserStatus.loseWeek;
			float num2 = 0f;
			if (num > 0)
			{
				num2 = (float)colosseumUserStatus.winWeek / (float)num * 100f;
			}
			rankingDataList.userColosseumScoreRankingId = colosseumUserStatus.colosseumRankId.ToString();
			rankingDataList.userId = colosseumUserStatus.userId;
			rankingDataList.colosseumId = this.RankingData.colosseumId;
			if (string.IsNullOrEmpty(this.RankingData.myRankingNo))
			{
				rankingDataList.rank = "0";
			}
			else
			{
				rankingDataList.rank = this.RankingData.myRankingNo;
			}
			rankingDataList.score = colosseumUserStatus.score.ToString();
			rankingDataList.winRate = num2.ToString();
			rankingDataList.winCount = colosseumUserStatus.winWeek.ToString();
			rankingDataList.totalBattleCount = num.ToString();
			rankingDataList.nickname = colosseumUserStatus.nickname;
			rankingDataList.iconId = DataMng.Instance().RespDataPRF_Profile.monsterData.monsterId;
			rankingDataList.createTime = string.Empty;
			rankingDataList.createUserId = string.Empty;
			rankingDataList.createActivityId = string.Empty;
			rankingDataList.updateTime = string.Empty;
			rankingDataList.updateUserId = string.Empty;
			rankingDataList.updateActivityId = string.Empty;
			rankingDataList.deleteFlg = "0";
			rankingDataList.isMine = isMine;
			return rankingDataList;
		}
		return null;
	}

	private GameWebAPI.RespDataCL_Ranking ConvertFromPointQuestRankingToPvPRanking(GameWebAPI.RespDataMS_PointQuestRankingList PointQuestRanking)
	{
		return new GameWebAPI.RespDataCL_Ranking
		{
			resultCode = PointQuestRanking.resultCode,
			colosseumId = "0",
			myRankingNo = PointQuestRanking.myRankingNo.ToString()
		};
	}

	public enum MODE
	{
		PvP,
		PointQuest
	}
}
