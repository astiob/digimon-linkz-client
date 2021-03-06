﻿using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_PointResult : CMD
{
	private CMD_PointResult.Proccess currentProccess;

	[SerializeField]
	private GameObject pointRoot;

	[SerializeField]
	private GameObject itemGetRoot;

	[SerializeField]
	private GameObject itemGetListRoot;

	[SerializeField]
	private GameObject itemGetMoveRoot;

	[SerializeField]
	private GUISelectPanelItemList csSelectPanelItemList;

	[SerializeField]
	private GameObject nextItemRoot;

	[SerializeField]
	private GameObject tapScreenRoot;

	[SerializeField]
	private SpritePointCount bonusTotalPoint;

	[SerializeField]
	private SpritePointCount totalPoint;

	[SerializeField]
	private PointResultRewardInfo[] getItemRewardInfo;

	[SerializeField]
	private PointResultRewardInfo[] nextItemRewardInfo;

	[SerializeField]
	private UILabel nextItemPointText;

	[SerializeField]
	private PointResultBonus basePoint;

	[SerializeField]
	private GameObject createBonusPart;

	[SerializeField]
	private Transform bonusPartPos;

	[SerializeField]
	private GameObject changeTapObj;

	private bool isDispItemGet;

	private GameWebAPI.RespData_AreaEventResult eventResult;

	private const int BONUS_UI_MAX = 5;

	private int dataViewConter;

	private int dataPageNum;

	private Animation basePointAnima;

	private TweenAlpha bonusPosTween;

	private const float BONUS_CHANGE_TIME = 0.2f;

	private IEnumerator bonusCoroutine;

	private bool countUpSe;

	private const float BONUS_CURVE_TIME = 1f;

	private const float BONUS_VIEW_TIME = 0.5f;

	private const float BONUS_END_WAIT_TIME = 1.5f;

	private List<PointResultBonus> pointBonusList = new List<PointResultBonus>();

	private bool bonusChange = true;

	private bool isItemListStarted;

	private int itemListFrame;

	private Action task;

	private Action finished;

	private float waitTime;

	private float currentWaitTime;

	private float halfPitch;

	private int addPitch;

	private bool isItemGetDisplayed;

	protected override void Awake()
	{
		this.itemGetListRoot.SetActive(false);
		this.itemGetRoot.SetActive(false);
		this.bonusChange = true;
		this.InitUI();
		base.Awake();
	}

	private void InitUI()
	{
		this.pointRoot.SetActive(false);
		this.itemGetRoot.SetActive(false);
		this.nextItemRoot.SetActive(false);
		this.tapScreenRoot.SetActive(false);
		this.changeTapObj.SetActive(false);
	}

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float ShowAnimationTime)
	{
		this.ConnectEventPointResult(delegate
		{
			RestrictionInput.EndLoad();
			this.isDispItemGet = (this.eventResult.reward != null && this.eventResult.reward.Length > 0);
			this.Wait(0.5f, delegate
			{
				this.DispPoint();
			});
			this.<Show>__BaseCallProxy0(closeEvent, sizeX, sizeY, 0f);
		});
	}

	public GameWebAPI.RespData_AreaEventResult.Reward GetDataByIDX(int idx)
	{
		return this.eventResult.reward[idx];
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateAndroidBackKey();
		this.PointAnimationCheck();
		this.UpdateDispItemGet();
		if (this.task != null)
		{
			this.task();
		}
	}

	private void PointAnimationCheck()
	{
		if (this.currentProccess != CMD_PointResult.Proccess.Point || !this.pointRoot.activeSelf)
		{
			return;
		}
		if (-392f <= this.basePoint.gameObject.transform.localPosition.x && null == this.basePointAnima)
		{
			this.basePointAnima = this.pointRoot.gameObject.GetComponent<Animation>();
			this.basePointAnima[this.basePointAnima.clip.name].speed = 0f;
			this.ViewBonusPoint();
		}
	}

	private void ConnectEventPointResult(Action finishedAction)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		string startId = (DataMng.Instance().RespData_WorldMultiStartInfo != null) ? DataMng.Instance().RespData_WorldMultiStartInfo.startId : DataMng.Instance().WD_ReqDngResult.startId;
		GameWebAPI.AreaEventResultLogic request = new GameWebAPI.AreaEventResultLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_AreaEventResult param)
			{
				param.startId = startId;
			},
			OnReceived = delegate(GameWebAPI.RespData_AreaEventResult response)
			{
				this.eventResult = response;
			}
		};
		AppCoroutine.Start(request.Run(finishedAction, null, null), false);
	}

	private void DispPoint()
	{
		this.pointRoot.SetActive(true);
		this.currentProccess = CMD_PointResult.Proccess.Point;
		this.SetPoint();
	}

	private void SetPoint()
	{
		UIWidget component = base.gameObject.GetComponent<UIWidget>();
		this.bonusPartPos.gameObject.GetComponent<UIWidget>().depth = component.depth + 10;
		int num = this.eventResult.point.bonusPoint.Length - 1;
		if (num < 5)
		{
			this.dataPageNum = 1;
		}
		else
		{
			this.dataPageNum = Mathf.CeilToInt((float)num / 5f);
		}
		for (int i = 0; i < this.eventResult.point.bonusPoint.Length - 1; i++)
		{
			if (i < 5)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.createBonusPart);
				gameObject.transform.parent = this.bonusPartPos;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = new Vector3(0f, -30f * (float)i, 0f);
				gameObject.gameObject.SetActive(false);
				this.pointBonusList.Add(gameObject.GetComponent<PointResultBonus>());
			}
		}
		this.basePoint.LabelDataSet(this.eventResult.point.bonusPoint[0].eventPointBonusMessage, string.Format("{0}", this.eventResult.point.bonusPoint[0].point));
		for (int j = 1; j <= 5; j++)
		{
			if (j >= this.eventResult.point.bonusPoint.Length)
			{
				break;
			}
			this.pointBonusList[j - 1].LabelDataSet(this.eventResult.point.bonusPoint[j].eventPointBonusMessage, string.Format("{0}", this.eventResult.point.bonusPoint[j].point));
		}
		this.bonusTotalPoint.SetNum(0);
		this.totalPoint.SetNum(this.eventResult.point.eventPoint - this.eventResult.point.totalPoint);
	}

	private void ReSetPoint()
	{
		this.dataViewConter++;
		int num = this.eventResult.point.bonusPoint.Length - 1;
		for (int i = 0; i < 5; i++)
		{
			int num2 = this.dataViewConter * 5 + i;
			if (num2 >= num)
			{
				break;
			}
			string eventPointBonusMessage = this.eventResult.point.bonusPoint[num2 + 1].eventPointBonusMessage;
			string pointText = this.eventResult.point.bonusPoint[num2 + 1].point.ToString();
			this.pointBonusList[i].LabelDataSet(eventPointBonusMessage, pointText);
		}
	}

	private void ViewBonusPoint()
	{
		this.bonusCoroutine = this.ViewBonusPointAnima();
		base.StartCoroutine(this.bonusCoroutine);
	}

	private IEnumerator ViewBonusPointAnima()
	{
		for (int i = 0; i < this.pointBonusList.Count; i++)
		{
			yield return new WaitForSeconds(0.5f);
			this.pointBonusList[i].gameObject.SetActive(true);
			this.pointBonusList[i].BonusLabelFadeIn(null);
		}
		if (this.dataViewConter < this.dataPageNum - 1)
		{
			yield return new WaitForSeconds(0.5f);
			for (int j = 0; j < this.pointBonusList.Count; j++)
			{
				this.pointBonusList[j].gameObject.SetActive(false);
			}
			this.ReSetPoint();
			this.bonusCoroutine = this.ViewBonusPointReStartAnima();
			base.StartCoroutine(this.bonusCoroutine);
		}
		else if (1f > this.basePointAnima[this.basePointAnima.clip.name].speed)
		{
			this.basePointAnima[this.basePointAnima.clip.name].speed = 1f;
			base.StartCoroutine(this.WaitStartCountUp());
		}
		yield break;
	}

	private IEnumerator ViewBonusPointReStartAnima()
	{
		int bonusPointLength = this.eventResult.point.bonusPoint.Length - 1;
		for (int i = 0; i < 5; i++)
		{
			int totalIndex = 5 * this.dataViewConter + i;
			if (totalIndex >= bonusPointLength)
			{
				break;
			}
			yield return new WaitForSeconds(0.5f);
			this.pointBonusList[i].gameObject.SetActive(true);
			this.pointBonusList[i].BonusLabelFadeIn(null);
		}
		if (this.dataViewConter < this.dataPageNum - 1)
		{
			yield return new WaitForSeconds(0.5f);
			for (int j = 0; j < this.pointBonusList.Count; j++)
			{
				this.pointBonusList[j].gameObject.SetActive(false);
			}
			this.ReSetPoint();
			this.bonusCoroutine = this.ViewBonusPointReStartAnima();
			base.StartCoroutine(this.bonusCoroutine);
		}
		else if (1f > this.basePointAnima[this.basePointAnima.clip.name].speed)
		{
			this.basePointAnima[this.basePointAnima.clip.name].speed = 1f;
			base.StartCoroutine(this.WaitStartCountUp());
		}
		yield break;
	}

	private string GetBonusDetailMessage(string id)
	{
		string result = string.Empty;
		GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus eventPointBonus = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster.eventPointBonusM.FirstOrDefault((GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus item) => item.eventPointBonusId == id);
		if (eventPointBonus != null)
		{
			result = eventPointBonus.detail;
		}
		return result;
	}

	private void StartCountUp()
	{
		this.countUpSe = true;
		this.bonusTotalPoint.StartCountUpCurve(this.eventResult.point.totalPoint, null, 1f);
		this.totalPoint.StartCountUpCurve(this.eventResult.point.eventPoint, new Action(this.OnFinishedDispPoint), 1f);
		this.pointRoot.GetComponent<EffectAnimatorEventTime>().SetEvent(0, new Action(this.PlayCountUpSound));
	}

	private void DispItemGet()
	{
		if (this.isItemGetDisplayed)
		{
			return;
		}
		this.isItemGetDisplayed = true;
		if (5 <= this.eventResult.reward.Length)
		{
			this.itemGetListRoot.SetActive(true);
			this.itemGetRoot.SetActive(false);
			this.csSelectPanelItemList.AllBuild(1, true, 1f, 1f, null, this, true);
			this.csSelectPanelItemList.UpdateBarrier();
			this.isItemListStarted = true;
			this.addPitch = 3;
			GUISelectPanelViewPartsUD.PanelBuildData panelBuildData = this.csSelectPanelItemList.GetPanelBuildData();
			this.halfPitch = panelBuildData.pitchH / 2f;
			Vector3 localPosition = this.itemGetMoveRoot.transform.localPosition;
			localPosition.y -= this.halfPitch * (float)this.addPitch;
			this.itemGetMoveRoot.transform.localPosition = localPosition;
		}
		else
		{
			this.itemGetListRoot.SetActive(false);
			this.itemGetRoot.SetActive(true);
			this.itemGetRoot.GetComponent<EffectAnimatorEventTime>().SetEvent(0, new Action(this.OnFinishedDispItemGet));
			this.currentProccess = CMD_PointResult.Proccess.ItemGet;
			this.SetGetItemInfo();
		}
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_305", 0f, false, true, null, -1);
	}

	private void UpdateDispItemGet()
	{
		if (this.isItemListStarted)
		{
			this.itemListFrame++;
			if (this.itemListFrame % this.csSelectPanelItemList.ItemListShowInterval == 0 && this.csSelectPanelItemList.partObjs.Count < this.eventResult.reward.Length)
			{
				if (this.csSelectPanelItemList.partObjs.Count + 1 == this.eventResult.reward.Length)
				{
					this.OnFinishedDispItemGet();
				}
				bool flag = this.csSelectPanelItemList.AddPartAndAdjustForXY(true, 0f, -5f);
				if (flag)
				{
					this.csSelectPanelItemList.UpdateBarrier();
					if (0 < this.addPitch)
					{
						Vector3 localPosition = this.itemGetMoveRoot.transform.localPosition;
						localPosition.y += this.halfPitch;
						this.itemGetMoveRoot.transform.localPosition = localPosition;
						this.addPitch--;
					}
				}
			}
		}
	}

	private void SetGetItemInfo()
	{
		int num = this.eventResult.reward.Length;
		this.getItemRewardInfo[num - 1].gameObject.SetActive(true);
		for (int i = 0; i < num; i++)
		{
			this.getItemRewardInfo[num - 1].SetDetail(i, this.eventResult.reward[i].assetCategoryId, this.eventResult.reward[i].assetValue);
		}
	}

	private void DispNextItem()
	{
		this.currentProccess = CMD_PointResult.Proccess.NextItem;
		List<GameWebAPI.RespData_AreaEventResult.Reward> nextRewardList = (this.eventResult.nextReward != null) ? new List<GameWebAPI.RespData_AreaEventResult.Reward>(this.eventResult.nextReward) : null;
		if (nextRewardList == null || nextRewardList.Count<GameWebAPI.RespData_AreaEventResult.Reward>() == 0)
		{
			this.OnFinishedDispNextItem();
		}
		else
		{
			this.nextItemRoot.SetActive(true);
			this.nextItemRoot.GetComponent<EffectAnimatorEventTime>().SetEvent(0, delegate
			{
				this.SetNextItemInfo(nextRewardList);
				this.OnFinishedDispNextItem();
			});
			int num = (nextRewardList.Count <= 0) ? 0 : (int.Parse(nextRewardList[0].point) - this.eventResult.point.eventPoint);
			this.nextItemPointText.text = string.Format(StringMaster.GetString("BattleResult-14"), num);
		}
	}

	private string GetWorldEventId(string worldDungeonId)
	{
		string result = string.Empty;
		GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus eventPointBonus = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster.eventPointBonusM.FirstOrDefault((GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus item) => item.worldDungeonId == worldDungeonId);
		if (eventPointBonus != null)
		{
			result = eventPointBonus.worldEventId;
		}
		return result;
	}

	private List<GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward> GetNextReward(int currentPoint, string worldEventId)
	{
		List<GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward> list = new List<GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward>(MasterDataMng.Instance().RespDataMA_EventPointAchieveRewardMaster.eventPointAchieveRewardM);
		List<GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward> list2 = new List<GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward>();
		list.Sort((GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward a, GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward b) => int.Parse(a.point) - int.Parse(b.point));
		int num = -1;
		for (int i = 0; i < list.Count; i++)
		{
			if (!(list[i].worldEventId != worldEventId))
			{
				int num2 = int.Parse(list[i].point);
				if (0 < num)
				{
					if (num2 == num)
					{
						list2.Add(list[i]);
					}
				}
				else if (num2 > currentPoint)
				{
					num = num2;
					list2.Add(list[i]);
				}
			}
		}
		return list2;
	}

	private void SetNextItemInfo(List<GameWebAPI.RespData_AreaEventResult.Reward> nextRewardList)
	{
		this.nextItemRewardInfo[3].gameObject.SetActive(false);
		if (nextRewardList.Count == 0)
		{
			return;
		}
		int count = nextRewardList.Count;
		this.nextItemRewardInfo[count - 1].gameObject.SetActive(true);
		for (int i = 0; i < count; i++)
		{
			this.nextItemRewardInfo[count - 1].SetDetail(i, nextRewardList[i].assetCategoryId, nextRewardList[i].assetValue);
		}
	}

	private void DispTapScreen()
	{
		this.bonusChange = false;
		this.tapScreenRoot.SetActive(true);
		if (this.eventResult.point.bonusPoint.Length - 1 > 5)
		{
			this.changeTapObj.SetActive(true);
			TweenAlpha component = this.changeTapObj.gameObject.GetComponent<TweenAlpha>();
			if (null != component)
			{
				component.PlayForward();
			}
		}
		this.currentProccess = CMD_PointResult.Proccess.TapScreen;
	}

	private void OnFinishedDispPoint()
	{
		this.bonusCoroutine = null;
		this.StopCountUpSound();
		this.countUpSe = false;
		if (this.isDispItemGet)
		{
			this.DispItemGet();
		}
		else
		{
			this.DispNextItem();
		}
	}

	private void OnFinishedDispItemGet()
	{
		this.DispNextItem();
	}

	private void OnFinishedDispNextItem()
	{
		this.DispTapScreen();
	}

	private void OnTapped()
	{
		switch (this.currentProccess)
		{
		case CMD_PointResult.Proccess.Point:
			this.SkipPoint();
			break;
		case CMD_PointResult.Proccess.TapScreen:
			this.EndTask();
			break;
		}
	}

	private void SkipPoint()
	{
		Animation component = this.pointRoot.GetComponent<Animation>();
		if (!component.isPlaying && this.pointRoot.activeSelf && this.bonusCoroutine == null)
		{
			this.bonusTotalPoint.SetNum(this.eventResult.point.totalPoint);
			this.totalPoint.SetNum(this.eventResult.point.eventPoint);
			this.OnFinishedDispPoint();
		}
		else if (this.bonusCoroutine != null)
		{
			this.StopBonusCoroutine();
		}
	}

	private void StopBonusCoroutine()
	{
		base.StopCoroutine(this.bonusCoroutine);
		this.bonusCoroutine = null;
		this.dataViewConter = this.dataPageNum - 1;
		for (int i = 0; i < this.pointBonusList.Count; i++)
		{
			this.pointBonusList[i].gameObject.SetActive(false);
			this.pointBonusList[i].SetLabelAlpha(1f);
		}
		int num = 0;
		int num2 = 5 * (this.dataPageNum - 1) + 1;
		for (int j = num2; j <= 5 * this.dataPageNum; j++)
		{
			if (j >= this.eventResult.point.bonusPoint.Length)
			{
				break;
			}
			this.pointBonusList[num].LabelDataSet(this.eventResult.point.bonusPoint[j].eventPointBonusMessage, string.Format("{0}", this.eventResult.point.bonusPoint[j].point));
			this.pointBonusList[num].gameObject.SetActive(true);
			this.pointBonusList[num].SetLabelAlpha(1f);
			num++;
		}
		if (null == this.basePointAnima)
		{
			this.basePointAnima = this.pointRoot.gameObject.GetComponent<Animation>();
		}
		if (null != this.basePointAnima && 1f > this.basePointAnima[this.basePointAnima.clip.name].speed)
		{
			this.basePointAnima[this.basePointAnima.clip.name].speed = 1f;
			base.StartCoroutine(this.WaitStartCountUp());
		}
	}

	private IEnumerator WaitStartCountUp()
	{
		yield return new WaitForSeconds(1.5f);
		this.StartCountUp();
		yield break;
	}

	private void EndTask()
	{
		if (this.bonusChange)
		{
			return;
		}
		this.StopCountUpSound();
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1);
		base.ClosePanel(true);
	}

	public void BonusListChange()
	{
		if (this.bonusChange || this.eventResult.point.bonusPoint.Length - 1 <= 5)
		{
			return;
		}
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1);
		this.bonusChange = true;
		this.dataViewConter++;
		if (this.dataViewConter > this.dataPageNum - 1)
		{
			this.dataViewConter = 0;
		}
		this.bonusCoroutine = this.BonusChangeFadeOut();
		base.StartCoroutine(this.bonusCoroutine);
	}

	private IEnumerator BonusChangeFadeOut()
	{
		int viewNum = this.dataViewConter % this.dataPageNum;
		int counter = 0;
		this.bonusPosTween = this.bonusPartPos.gameObject.AddComponent<TweenAlpha>();
		this.bonusPosTween.from = 1f;
		this.bonusPosTween.to = 0f;
		this.bonusPosTween.duration = 0.2f;
		this.bonusPosTween.PlayForward();
		yield return new WaitForSeconds(0.2f);
		for (int i = 0; i < this.pointBonusList.Count; i++)
		{
			this.pointBonusList[i].gameObject.SetActive(false);
		}
		int offset = 5 * viewNum + 1;
		for (int j = offset; j <= 5 * (viewNum + 1); j++)
		{
			if (j >= this.eventResult.point.bonusPoint.Length)
			{
				break;
			}
			this.pointBonusList[counter].LabelDataSet(this.eventResult.point.bonusPoint[j].eventPointBonusMessage, string.Format("{0}", this.eventResult.point.bonusPoint[j].point));
			this.pointBonusList[counter].gameObject.SetActive(true);
			counter++;
		}
		UnityEngine.Object.Destroy(this.bonusPosTween);
		this.bonusPosTween = null;
		this.bonusPosTween = this.bonusPartPos.gameObject.AddComponent<TweenAlpha>();
		this.bonusPosTween.from = 0f;
		this.bonusPosTween.to = 1f;
		this.bonusPosTween.duration = 0.2f;
		this.bonusPosTween.PlayForward();
		yield return new WaitForSeconds(0.2f);
		this.bonusChange = false;
		yield break;
	}

	private IEnumerator BonusChangeEnd()
	{
		yield return new WaitForEndOfFrame();
		this.bonusChange = false;
		yield break;
	}

	private void UpdateAndroidBackKey()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnTapped();
		}
	}

	private void Wait(float time, Action f)
	{
		this.waitTime = time;
		this.finished = f;
		this.task = new Action(this.WaitTask);
	}

	private void WaitTask()
	{
		if (this.waitTime <= this.currentWaitTime)
		{
			this.currentWaitTime = 0f;
			this.finished();
			this.task = null;
			return;
		}
		this.currentWaitTime += Time.deltaTime;
	}

	private void PlayCountUpSound()
	{
		if (this.countUpSe)
		{
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_102", 0f, true, true, null, -1);
		}
	}

	private void StopCountUpSound()
	{
		if (SoundMng.Instance().IsPlayingSE("SEInternal/Common/se_102"))
		{
			SoundMng.Instance().TryStopSE("SEInternal/Common/se_102", 0.2f, null);
		}
	}

	private enum Proccess
	{
		Point,
		ItemGet,
		NextItem,
		TapScreen
	}
}
