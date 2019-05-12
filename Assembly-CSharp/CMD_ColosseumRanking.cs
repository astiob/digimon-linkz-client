using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_ColosseumRanking : CMD
{
	[SerializeField]
	private GUISelectPanelQuestRanking csSelectPanel;

	[SerializeField]
	private GUIListPartsQuestRanking csParts;

	[SerializeField]
	private UILabelEx lbBtnChangeRanking;

	[SerializeField]
	private GUISelectPanelColosseumRanking csRankingPanel;

	[SerializeField]
	private UILabelEx lbColosseumRankingListEmpty;

	private GameWebAPI.RespDataCL_Ranking colosseumRankingList;

	private List<string> keysListStartValue;

	private List<string> keysList;

	private List<int> valsList;

	private int listIdx;

	private int limitRank;

	private bool amIOutRange;

	public CMD_ColosseumRanking.ColosseumRankingType dispRankingType = CMD_ColosseumRanking.ColosseumRankingType.THIS_TIME;

	public int dispColosseumId;

	public static CMD_ColosseumRanking instance;

	private const int DEFAULT_DISP_RANKING_NUM = 100;

	private const int UPDATE_DISP_RANKING_NUM = 100;

	public GameWebAPI.RespDataCL_Ranking.RankingData myData { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_ColosseumRanking.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.dispColosseumId = DataMng.Instance().RespData_ColosseumInfo.colosseumId;
		base.StartCoroutine(this.InitDLG(f, sizeX, sizeY, aT));
	}

	protected override void WindowClosed()
	{
		base.WindowClosed();
		CMD_ColosseumRanking.instance = null;
	}

	private IEnumerator InitDLG(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GameWebAPI.RequestCL_Ranking request = this.GetRequestRanking(this.dispColosseumId, 1, 100, false);
		yield return base.StartCoroutine(request.RunOneTime(delegate()
		{
			this.PartsTitle.SetTitle(StringMaster.GetString("ColosseumRankTitle"));
			this.ShowDLG();
			this.InitUI();
			this.<Show>__BaseCallProxy0(f, sizeX, sizeY, aT);
		}, delegate(Exception exception)
		{
			this.SetCloseAction(null);
			this.<ClosePanel>__BaseCallProxy1(false);
		}, null));
		RestrictionInput.EndLoad();
		yield break;
	}

	private GameWebAPI.RequestCL_Ranking GetRequestRanking(int colosseumId, int begin, int end, bool isUpdate)
	{
		return new GameWebAPI.RequestCL_Ranking
		{
			SetSendData = delegate(GameWebAPI.CL_Req_Ranking param)
			{
				param.colosseumId = colosseumId;
				param.begin = begin;
				param.end = end;
			},
			OnReceived = delegate(GameWebAPI.RespDataCL_Ranking response)
			{
				if (isUpdate)
				{
					List<GameWebAPI.RespDataCL_Ranking.RankingData> list = new List<GameWebAPI.RespDataCL_Ranking.RankingData>();
					list = this.colosseumRankingList.rankingMember.ToList<GameWebAPI.RespDataCL_Ranking.RankingData>();
					list.AddRange(response.rankingMember.ToList<GameWebAPI.RespDataCL_Ranking.RankingData>());
					if (list.Count == 0)
					{
						this.colosseumRankingList.rankingMember = new GameWebAPI.RespDataCL_Ranking.RankingData[0];
					}
					else
					{
						this.colosseumRankingList.rankingMember = list.ToArray();
					}
				}
				else
				{
					this.colosseumRankingList = response;
					this.DataSetting();
				}
			}
		};
	}

	private GameWebAPI.RespDataCL_Ranking.RankingData GetMyData()
	{
		this.myData = new GameWebAPI.RespDataCL_Ranking.RankingData();
		this.myData.userId = int.Parse(DataMng.Instance().UserId);
		this.myData.titleId = DataMng.Instance().RespDataPRF_Profile.userData.titleId;
		if (string.IsNullOrEmpty(this.colosseumRankingList.myRankingNo.ToString()) || this.colosseumRankingList.myRankingNo <= 0)
		{
			this.myData.rank = 0;
		}
		else
		{
			this.myData.rank = this.colosseumRankingList.myRankingNo;
		}
		this.myData.point = this.colosseumRankingList.myPoint;
		this.myData.nickname = DataMng.Instance().UserName;
		this.myData.leaderMonsterId = DataMng.Instance().RespDataPRF_Profile.monsterData.monsterId;
		return this.myData;
	}

	public int GetNextPoint()
	{
		return this.colosseumRankingList.pointToNextRank;
	}

	private void InitUI()
	{
		if (this.colosseumRankingList.resultCode == 1)
		{
			this.myData = this.GetMyData();
			this.csParts.SetData();
			this.csParts.ShowGUI();
			this.csSelectPanel.DisableList();
			this.csSelectPanel.AllBuild(this.keysList.Count, true, 1f, 1f, null, null, true);
			this.csSelectPanel.SetLocationByIDX(this.listIdx, 0f);
			this.csRankingPanel.DisableList();
			this.csRankingPanel.SetData(this.colosseumRankingList);
			this.csRankingPanel.AllBuild(this.colosseumRankingList.rankingMember.Count<GameWebAPI.RespDataCL_Ranking.RankingData>(), true, 1f, 1f, null, null, true);
			this.csRankingPanel.SetBeforeMaxLocate();
			if (this.colosseumRankingList.rankingMember.Count<GameWebAPI.RespDataCL_Ranking.RankingData>() > 0)
			{
				this.lbColosseumRankingListEmpty.gameObject.SetActive(false);
			}
			else
			{
				this.lbColosseumRankingListEmpty.gameObject.SetActive(true);
			}
		}
		else if (this.colosseumRankingList.resultCode == 2)
		{
			this.myData = this.GetMyData();
			this.csParts.SetData();
			this.csParts.ShowGUI();
			this.csSelectPanel.DisableList();
			this.csSelectPanel.AllBuild(this.keysList.Count, true, 1f, 1f, null, null, true);
			this.csSelectPanel.SetLocationByIDX(this.listIdx, 0f);
			this.csRankingPanel.DisableList();
			this.csRankingPanel.SetData(this.colosseumRankingList);
			this.csRankingPanel.AllBuild(this.colosseumRankingList.rankingMember.Count<GameWebAPI.RespDataCL_Ranking.RankingData>(), true, 1f, 1f, null, null, true);
			this.csRankingPanel.SetBeforeMaxLocate();
			this.lbColosseumRankingListEmpty.gameObject.SetActive(true);
		}
	}

	public bool GetIsMine(int idx)
	{
		bool result = false;
		if (this.colosseumRankingList.myRankingNo > 0)
		{
			if (idx > 0)
			{
				if (this.colosseumRankingList.myRankingNo <= int.Parse(this.keysList[idx]) && this.colosseumRankingList.myRankingNo > int.Parse(this.keysList[idx - 1]))
				{
					result = true;
				}
			}
			else if (this.colosseumRankingList.myRankingNo <= int.Parse(this.keysList[idx]))
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

	public int GetlimitRank()
	{
		return this.limitRank;
	}

	public int GetRankingValue(int idx)
	{
		return this.valsList[idx];
	}

	public string[] GetRankingKey(int idx)
	{
		return new string[]
		{
			this.keysListStartValue[idx],
			this.keysList[idx]
		};
	}

	private void DataSetting()
	{
		GameWebAPI.RespDataCL_Ranking respDataCL_Ranking = this.colosseumRankingList;
		if (this.dispRankingType == CMD_ColosseumRanking.ColosseumRankingType.THIS_TIME)
		{
			respDataCL_Ranking.pointRankingList["-1"] = 0;
		}
		if (respDataCL_Ranking.pointRankingList == null)
		{
			this.keysList = new List<string>();
			this.valsList = new List<int>();
			this.keysListStartValue = new List<string>();
			this.listIdx = this.keysList.Count - 1;
			this.amIOutRange = true;
			this.limitRank = 0;
			respDataCL_Ranking.pointToNextRank = 0;
			return;
		}
		this.keysList = new List<string>(respDataCL_Ranking.pointRankingList.Keys);
		this.valsList = new List<int>(respDataCL_Ranking.pointRankingList.Values);
		this.keysListStartValue = new List<string>();
		this.keysListStartValue.Add("1");
		for (int i = 1; i < respDataCL_Ranking.pointRankingList.Count; i++)
		{
			int num = int.Parse(this.keysList[i - 1]) + 1;
			this.keysListStartValue.Add(num.ToString());
		}
		if (respDataCL_Ranking.myRankingNo > 0)
		{
			for (int j = 0; j < this.keysList.Count; j++)
			{
				if (respDataCL_Ranking.myRankingNo > int.Parse(this.keysList[j]) && int.Parse(this.keysList[j]) > 0)
				{
					this.listIdx = j + 1;
				}
			}
			if (int.Parse(this.keysList[this.keysList.Count - 2]) < respDataCL_Ranking.myRankingNo)
			{
				this.listIdx = this.keysList.Count - 1;
				this.amIOutRange = true;
			}
			else
			{
				this.amIOutRange = false;
			}
		}
		else
		{
			this.listIdx = this.keysList.Count - 1;
			this.amIOutRange = true;
		}
		this.limitRank = int.Parse(this.keysList[this.keysList.Count - 2]);
		if (this.listIdx == 0)
		{
			respDataCL_Ranking.pointToNextRank = 0;
		}
	}

	private void OnTouchedChangeRanking()
	{
		CMD_ColosseumRanking.ColosseumRankingType colosseumRankingType = this.dispRankingType;
		if (colosseumRankingType != CMD_ColosseumRanking.ColosseumRankingType.LAST_TIME)
		{
			if (colosseumRankingType == CMD_ColosseumRanking.ColosseumRankingType.THIS_TIME)
			{
				this.dispRankingType = CMD_ColosseumRanking.ColosseumRankingType.LAST_TIME;
				this.lbBtnChangeRanking.text = StringMaster.GetString("ColosseumRankingOfLastTime");
				GameWebAPI.RespDataMA_ColosseumM.Colosseum colosseum = MasterDataMng.Instance().RespDataMA_ColosseumMaster.colosseumM.Single((GameWebAPI.RespDataMA_ColosseumM.Colosseum x) => x.colosseumId == DataMng.Instance().RespData_ColosseumInfo.colosseumId.ToString());
				this.dispColosseumId = int.Parse(colosseum.prevColosseumId);
			}
		}
		else
		{
			this.dispRankingType = CMD_ColosseumRanking.ColosseumRankingType.THIS_TIME;
			this.lbBtnChangeRanking.text = StringMaster.GetString("ColosseumRankingOfThisTime");
			this.dispColosseumId = DataMng.Instance().RespData_ColosseumInfo.colosseumId;
		}
		this.DispRankingList(1, 100);
	}

	public void DispRankingList(int begin, int end)
	{
		global::Debug.Log(string.Concat(new object[]
		{
			"コロシアムID：",
			this.dispColosseumId,
			" の ",
			begin,
			" 位から",
			end,
			" 位を表示します"
		}));
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		GameWebAPI.RequestCL_Ranking requestRanking = this.GetRequestRanking(this.dispColosseumId, begin, end, false);
		base.StartCoroutine(requestRanking.RunOneTime(delegate()
		{
			this.InitUI();
			RestrictionInput.EndLoad();
		}, delegate(Exception exception)
		{
			RestrictionInput.EndLoad();
			base.SetCloseAction(null);
			this.<ClosePanel>__BaseCallProxy1(false);
		}, null));
	}

	public void UpdateRankingList()
	{
		int num = this.colosseumRankingList.rankingMember.LastOrDefault((GameWebAPI.RespDataCL_Ranking.RankingData x) => x.rank > 1).rank + 1;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		GameWebAPI.RequestCL_Ranking requestRanking = this.GetRequestRanking(this.dispColosseumId, num, num + 100, true);
		base.StartCoroutine(requestRanking.RunOneTime(delegate()
		{
			this.csRankingPanel.SetData(this.colosseumRankingList);
			this.csRankingPanel.initMaxLocation = true;
			this.csRankingPanel.AllBuild(this.colosseumRankingList.rankingMember.Count<GameWebAPI.RespDataCL_Ranking.RankingData>(), false, 1f, 1f, null, null, true);
			this.csRankingPanel.SetSelectLocate();
			this.csRankingPanel.SetBeforeMaxLocate();
			RestrictionInput.EndLoad();
		}, delegate(Exception exception)
		{
			RestrictionInput.EndLoad();
			base.SetCloseAction(null);
			this.<ClosePanel>__BaseCallProxy1(false);
		}, null));
	}

	public enum ColosseumRankingType
	{
		LAST_TIME,
		THIS_TIME
	}
}
