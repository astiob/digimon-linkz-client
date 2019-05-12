using System;
using System.Collections.Generic;

namespace Colosseum.Matching
{
	public sealed class ColosseumMatchingNetworkSocket
	{
		private const string SOCKET_API_ACTIVITY_ID_MATCHING_FINISH = "080121";

		private Dictionary<string, object> messageBuffer;

		private Dictionary<string, object> opponentOnlineStatusCheckMessage;

		private ColosseumMatchingEventListener eventListener;

		public ColosseumMatchingNetworkSocket(ColosseumMatchingEventListener listener)
		{
			this.eventListener = listener;
			this.messageBuffer = new Dictionary<string, object>();
			this.opponentOnlineStatusCheckMessage = new Dictionary<string, object>
			{
				{
					"080110",
					new PvPOnlineCheck()
				}
			};
		}

		private void OnRecievedMatchingState(int state)
		{
			switch (state)
			{
			case 0:
			case 4:
				this.eventListener.OnBecameOwner();
				return;
			case 1:
				this.eventListener.OnDecidedOpponent();
				return;
			case 3:
			case 7:
				this.eventListener.OnStopMatching();
				return;
			case 5:
				this.eventListener.OnDeleteMathing();
				return;
			}
			this.OnErrorMatching(state);
		}

		private void OnErrorMatching(int errorCode)
		{
			int num = errorCode;
			if (num != 2)
			{
				if (num != 6)
				{
					if (num != 93)
					{
						this.eventListener.OnExceptionMatchingResponse(errorCode.ToString());
					}
					else
					{
						this.eventListener.OnErrorMatchingResponse("ColosseumWithdraw", "ColosseumSelect");
					}
				}
				else
				{
					this.eventListener.OnErrorMatchingResponse("ColosseumCloseTime", "ColosseumGoTop");
				}
			}
			else
			{
				this.eventListener.OnErrorMatchingResponse("ColosseumNotEntry", "ColosseumGoTop");
			}
		}

		private void OnReceivedResumeMatchingResult(int resultCode, int colosseumStatus)
		{
			if (colosseumStatus == 1)
			{
				this.OnNormalResumeMatching(colosseumStatus);
			}
			else
			{
				switch (resultCode)
				{
				case 1:
					this.OnNormalResumeMatching(colosseumStatus);
					break;
				case 4:
					this.eventListener.OnCancelMatching();
					break;
				case 5:
					this.eventListener.OnResumeOpponent();
					break;
				case 6:
					this.eventListener.OnDeleteMathing();
					break;
				}
			}
		}

		private void OnNormalResumeMatching(int colosseumStatus)
		{
			switch (colosseumStatus)
			{
			case 2:
			case 4:
			case 5:
				this.eventListener.OnResumeSelectMonsterSuccess();
				break;
			case 3:
				this.eventListener.OnResumeMatchingSuccess();
				break;
			case 6:
				this.eventListener.OnDeleteMathing();
				break;
			default:
				this.eventListener.OnCancelMatching();
				break;
			}
		}

		private void OnRecievedBattleStart(int resultCode)
		{
			switch (resultCode)
			{
			case 1:
			case 3:
				this.eventListener.OnReceivedMyStartBattleResponse();
				break;
			case 2:
				this.eventListener.OnReceivedOpponentStartBattleResponse();
				break;
			default:
				this.eventListener.OnExceptionStartBattleResponse(resultCode);
				break;
			}
		}

		private void OnRecievedBattlePvPEnemyData(List<object> monsterIndexList)
		{
			int[] array = new int[monsterIndexList.Count];
			for (int i = 0; i < monsterIndexList.Count; i++)
			{
				array[i] = Convert.ToInt32(monsterIndexList[i]);
			}
			this.eventListener.OnReceivedSelectMonster(array);
		}

		private void OnRecievedOnlineCheck(int resultCode)
		{
			if (resultCode == 0)
			{
				this.eventListener.OnOpponentLeave();
			}
			else
			{
				this.eventListener.OnOpponentOnline();
			}
		}

		public void GetResponseData(Dictionary<string, object> response)
		{
			foreach (KeyValuePair<string, object> keyValuePair in response)
			{
				Dictionary<object, object> dictionary = keyValuePair.Value as Dictionary<object, object>;
				string key = keyValuePair.Key;
				switch (key)
				{
				case "800013":
					this.eventListener.OnReceivedUpdateOnlineStatusResponse();
					break;
				case "080106":
					if (dictionary.ContainsKey("errorCode"))
					{
						this.eventListener.OnExceptionMatchingResponse(dictionary["errorCode"].ToString());
					}
					else
					{
						this.OnRecievedMatchingState(Convert.ToInt32(dictionary["resultCode"]));
					}
					break;
				case "080108":
					this.OnRecievedBattleStart(Convert.ToInt32(dictionary["resultCode"]));
					break;
				case "enemyData":
				{
					Dictionary<object, object> dictionary2 = (Dictionary<object, object>)dictionary["PvPEnemyData"];
					this.OnRecievedBattlePvPEnemyData((List<object>)dictionary2["indexId"]);
					break;
				}
				case "enemyDataReceive":
				{
					Dictionary<object, object> dictionary3 = (Dictionary<object, object>)dictionary["PvPEnemyDataReceive"];
					this.eventListener.OnAckReceivedSelectMonster(Convert.ToInt32(dictionary3["randomSeed"]));
					break;
				}
				case "080110":
					this.OnRecievedOnlineCheck(Convert.ToInt32(dictionary["resultCode"]));
					break;
				case "080121":
					this.eventListener.OnReadyBattleCompleted();
					break;
				case "080112":
					this.OnReceivedResumeMatchingResult(Convert.ToInt32(dictionary["resultCode"]), Convert.ToInt32(dictionary["colosseumStatus"]));
					break;
				}
			}
		}

		public void UpdateOnlineStatus()
		{
			PvPConnectionNoticeCheck value = new PvPConnectionNoticeCheck();
			this.messageBuffer.Clear();
			this.messageBuffer.Add("800013", value);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, "activityList");
		}

		public void StartMatching(IColosseumMatchingInfo matchingInfo)
		{
			PvPMatching startMatchingRequest = matchingInfo.GetStartMatchingRequest();
			this.messageBuffer.Clear();
			this.messageBuffer.Add("080106", startMatchingRequest);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, "activityList");
		}

		public void StopMatching(IColosseumMatchingInfo matchingInfo)
		{
			PvPMatching stopMatchingRequest = matchingInfo.GetStopMatchingRequest();
			this.messageBuffer.Clear();
			this.messageBuffer.Add("080106", stopMatchingRequest);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, "activityList");
		}

		public void ResumeMatching(IColosseumMatchingInfo matchingInfo)
		{
			PvPBattleRecover resumeMatchingRequest = matchingInfo.GetResumeMatchingRequest();
			this.messageBuffer.Clear();
			this.messageBuffer.Add("080112", resumeMatchingRequest);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, "activityList");
		}

		public void StartBattle(IColosseumMatchingInfo matchingInfo)
		{
			PvPBattleStart startBattleRequest = matchingInfo.GetStartBattleRequest();
			this.messageBuffer.Clear();
			this.messageBuffer.Add("080108", startBattleRequest);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, "activityList");
		}

		public ColosseumMatchingNetworkSynchronize CreateSelectMonsterMessage(List<int> toAddress, int[] selectMonsterIndexList)
		{
			PvPEnemyData value = new PvPEnemyData
			{
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.PvPEnemyData, DataMng.Instance().UserId, TCPMessageType.None),
				playerUserId = DataMng.Instance().UserId,
				indexId = selectMonsterIndexList
			};
			this.messageBuffer.Clear();
			this.messageBuffer.Add(TCPMessageType.PvPEnemyData.ToString(), value);
			ColosseumMatchingNetworkSynchronize colosseumMatchingNetworkSynchronize = new ColosseumMatchingNetworkSynchronize();
			colosseumMatchingNetworkSynchronize.SetIntervalAndTrialTime(5f, 15f);
			colosseumMatchingNetworkSynchronize.SetFailedAction(new Action(this.eventListener.OnFailedSelectMonsterSend));
			colosseumMatchingNetworkSynchronize.SetMessage(toAddress, this.messageBuffer, "enemyData");
			return colosseumMatchingNetworkSynchronize;
		}

		public void SendSelectMonsterIndexList(List<int> toAddress, int[] selectMonsterIndexList)
		{
			PvPEnemyData value = new PvPEnemyData
			{
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.PvPEnemyData, DataMng.Instance().UserId, TCPMessageType.None),
				playerUserId = DataMng.Instance().UserId,
				indexId = selectMonsterIndexList
			};
			this.messageBuffer.Clear();
			this.messageBuffer.Add(TCPMessageType.PvPEnemyData.ToString(), value);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, toAddress, "enemyData");
		}

		public void SendAckReceivedSelectMonsterIndex(List<int> toAddress, int battleRandomSeed)
		{
			PvPEnemyDataReceive value = new PvPEnemyDataReceive
			{
				randomSeed = battleRandomSeed
			};
			this.messageBuffer.Clear();
			this.messageBuffer.Add(TCPMessageType.PvPEnemyDataReceive.ToString(), value);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, toAddress, "enemyDataReceive");
		}

		public void SendMatchingFinish(int[] mySelectMonster, int[] opponentSelectMonster)
		{
			SocketRequestMatchingFinish value = new SocketRequestMatchingFinish
			{
				myDeck = mySelectMonster,
				battleUserDeck = opponentSelectMonster
			};
			this.messageBuffer.Clear();
			this.messageBuffer.Add("080121", value);
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.messageBuffer, "activityList");
		}

		public void CheckOpponentOnlineStatus()
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(this.opponentOnlineStatusCheckMessage, "activityList");
		}
	}
}
