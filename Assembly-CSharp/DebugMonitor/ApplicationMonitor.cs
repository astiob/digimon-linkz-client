using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;
using WebAPIRequest;

namespace DebugMonitor
{
	public sealed class ApplicationMonitor : MonoBehaviour
	{
		private ApplicationMonitorMode[] monitorModeList;

		[SerializeField]
		private float alertUseMemoryPercent = 90f;

		[SerializeField]
		private float updateIntervalTime = 0.5f;

		private GUIStyleState styleState = new GUIStyleState();

		private GUIStyle fontStyle = new GUIStyle();

		private string displayText = string.Empty;

		private Rect displayRect = default(Rect);

		private bool isDisplay;

		private MonitorMode displayMode;

		private string titleText = string.Empty;

		private string counterSuffixText = string.Empty;

		private float peekUseMemoryPercent;

		private bool isLock;

		private int frameCount;

		private float elapsedTime;

		private string log = string.Empty;

		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		private void Start()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void Update()
		{
		}

		private void OnGUI()
		{
			if (!this.isDisplay)
			{
				return;
			}
			if (GUI.Button(this.displayRect, this.titleText + this.displayText + this.counterSuffixText, this.fontStyle))
			{
				this.ChangeMode();
				this.ChangeTextColor(Color.white);
			}
		}

		private void Initialize()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.displayRect = new Rect(new Vector2((float)Screen.width / 2f, 10f), new Vector2((float)Screen.width / 10f, (float)Screen.height / 10f));
			this.ChangeTextColor(Color.white);
			this.fontStyle.fontSize = (int)this.displayRect.size.y;
			this.ChangeMode(this.displayMode);
		}

		private void Monitoring()
		{
			switch (this.displayMode)
			{
			case MonitorMode.USE_MEMORY_PERCENT:
			case MonitorMode.PEEK_USE_MEMORY_PERCENT:
			case MonitorMode.HEAP_MEMORY:
			case MonitorMode.RESERVED_MEMORY:
				this.MonitoringMemory();
				break;
			case MonitorMode.FPS:
				this.MonitoringFPS();
				break;
			}
		}

		private void MonitoringMemory()
		{
			float num = (float)Profiler.usedHeapSizeLong;
			float num2 = (float)Profiler.GetTotalReservedMemoryLong();
			float num3 = num / num2 * 100f;
			if (this.peekUseMemoryPercent < num3)
			{
				this.peekUseMemoryPercent = num3;
			}
			switch (this.displayMode)
			{
			case MonitorMode.USE_MEMORY_PERCENT:
				this.displayText = num3.ToString("0.00");
				this.ChangeTextColor((this.alertUseMemoryPercent >= num3) ? Color.white : Color.red);
				break;
			case MonitorMode.PEEK_USE_MEMORY_PERCENT:
				this.displayText = this.peekUseMemoryPercent.ToString("0.00");
				this.ChangeTextColor((this.alertUseMemoryPercent >= this.peekUseMemoryPercent) ? Color.white : Color.red);
				break;
			case MonitorMode.HEAP_MEMORY:
				this.displayText = (num / 1048576f).ToString("0.00");
				break;
			case MonitorMode.RESERVED_MEMORY:
				this.displayText = (num2 / 1048576f).ToString("0.00");
				break;
			}
		}

		private void MonitoringFPS()
		{
			this.displayText = ((float)this.frameCount / this.elapsedTime).ToString("0.0");
		}

		private void ChangeTextColor(Color TextColor)
		{
			this.styleState.textColor = TextColor;
			this.fontStyle.normal = this.styleState;
		}

		private void ChangeMode()
		{
			this.displayMode++;
			if (this.displayMode >= (MonitorMode)this.monitorModeList.Length)
			{
				this.displayMode = MonitorMode.USE_MEMORY_PERCENT;
			}
			this.ChangeMode(this.displayMode);
		}

		private void ChangeMode(MonitorMode setMode)
		{
			this.displayMode = setMode;
			this.titleText = this.monitorModeList[(int)setMode].title;
			this.displayText = string.Empty;
			this.counterSuffixText = this.monitorModeList[(int)setMode].suffix;
		}

		private IEnumerator ExecuteColosseumBattleEndLogic(GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult battleResult, Action<bool> callback = null)
		{
			GameWebAPI.RespData_ColosseumBattleEndLogic colosseumEnd = null;
			GameWebAPI.ColosseumBattleEndLogic request = new GameWebAPI.ColosseumBattleEndLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_ColosseumBattleEndLogic param)
				{
					param.battleResult = (int)battleResult;
					param.roundCount = 1;
					param.isMockBattle = ((!(ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode == "0")) ? 1 : 0);
					param.skillUseDeckPosition = "0";
				},
				OnReceived = delegate(GameWebAPI.RespData_ColosseumBattleEndLogic resData)
				{
					colosseumEnd = resData;
				}
			};
			RequestBase request2 = request;
			if (ApplicationMonitor.<>f__mg$cache0 == null)
			{
				ApplicationMonitor.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
			}
			yield return request2.Run(ApplicationMonitor.<>f__mg$cache0, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null);
			MultiBattleData.BattleEndResponseData responseData = new MultiBattleData.BattleEndResponseData();
			if (colosseumEnd != null)
			{
				responseData.resultCode = colosseumEnd.resultCode;
				List<MultiBattleData.BattleEndResponseData.Reward> list = new List<MultiBattleData.BattleEndResponseData.Reward>();
				if (colosseumEnd.reward != null)
				{
					for (int i = 0; i < colosseumEnd.reward.Length; i++)
					{
						list.Add(new MultiBattleData.BattleEndResponseData.Reward
						{
							assetCategoryId = colosseumEnd.reward[i].assetCategoryId,
							assetNum = colosseumEnd.reward[i].assetNum,
							assetValue = colosseumEnd.reward[i].assetValue
						});
					}
				}
				List<MultiBattleData.BattleEndResponseData.Reward> list2 = new List<MultiBattleData.BattleEndResponseData.Reward>();
				if (colosseumEnd.firstRankUpReward != null)
				{
					for (int j = 0; j < colosseumEnd.firstRankUpReward.Length; j++)
					{
						list2.Add(new MultiBattleData.BattleEndResponseData.Reward
						{
							assetCategoryId = colosseumEnd.firstRankUpReward[j].assetCategoryId,
							assetNum = colosseumEnd.firstRankUpReward[j].assetNum,
							assetValue = colosseumEnd.firstRankUpReward[j].assetValue
						});
					}
				}
				responseData.reward = list.ToArray();
				responseData.firstRankUpReward = list2.ToArray();
				responseData.score = colosseumEnd.score;
				responseData.colosseumRankId = colosseumEnd.colosseumRankId;
				responseData.isFirstRankUp = colosseumEnd.isFirstRankUp;
				if (colosseumEnd.battleRecord != null)
				{
					responseData.battleRecord = new MultiBattleData.BattleEndResponseData.ColosseumBattleRecord();
					responseData.battleRecord.count = colosseumEnd.battleRecord.count;
					responseData.battleRecord.winPercent = colosseumEnd.battleRecord.winPercent;
				}
				if (callback != null)
				{
					callback(true);
				}
			}
			else
			{
				responseData.reward = new MultiBattleData.BattleEndResponseData.Reward[0];
				if (callback != null)
				{
					callback(false);
				}
			}
			ClassSingleton<MultiBattleData>.Instance.BattleEndResponse = responseData;
			yield break;
		}
	}
}
