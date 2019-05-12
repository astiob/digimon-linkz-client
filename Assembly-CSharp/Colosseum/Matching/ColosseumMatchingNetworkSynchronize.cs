using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebAPIRequest;

namespace Colosseum.Matching
{
	public sealed class ColosseumMatchingNetworkSynchronize
	{
		private float intervalTime;

		private float trialTime;

		private Action actionFailedSync;

		private ColosseumMatchingNetworkSynchronize.SendType sendType;

		private List<int> toAddress;

		private Dictionary<string, object> sendMessage;

		private string messageType;

		private RequestBase sendRequest;

		private Func<RequestBase, Coroutine> sendRequestRoutine;

		private bool stopSync;

		public Coroutine SynchronizeRoutine { get; set; }

		private IEnumerator SendRequestHTTP()
		{
			float endTime = Time.realtimeSinceStartup + this.trialTime;
			WaitForSeconds waitTime = new WaitForSeconds(this.intervalTime);
			while (Time.realtimeSinceStartup < endTime && !this.stopSync)
			{
				yield return this.sendRequestRoutine(this.sendRequest);
				yield return waitTime;
			}
			if (!this.stopSync)
			{
				this.actionFailedSync();
			}
			yield break;
		}

		private IEnumerator SendRequestSocket()
		{
			float endTime = Time.realtimeSinceStartup + this.trialTime;
			WaitForSeconds waitTime = new WaitForSeconds(this.intervalTime);
			while (Time.realtimeSinceStartup < endTime && !this.stopSync)
			{
				Singleton<TCPUtil>.Instance.SendTCPRequest(this.sendMessage, "activityList");
				yield return waitTime;
			}
			if (!this.stopSync)
			{
				this.actionFailedSync();
			}
			yield break;
		}

		private IEnumerator SendMessage()
		{
			float endTime = Time.realtimeSinceStartup + this.trialTime;
			WaitForSeconds waitTime = new WaitForSeconds(this.intervalTime);
			while (Time.realtimeSinceStartup < endTime && !this.stopSync)
			{
				Singleton<TCPUtil>.Instance.SendTCPRequest(this.sendMessage, this.toAddress, this.messageType);
				yield return waitTime;
			}
			if (!this.stopSync)
			{
				this.actionFailedSync();
			}
			yield break;
		}

		private IEnumerator WaitReceive()
		{
			float endTime = Time.realtimeSinceStartup + this.trialTime;
			WaitForSeconds waitTime = new WaitForSeconds(1f);
			while (Time.realtimeSinceStartup < endTime && !this.stopSync)
			{
				yield return waitTime;
			}
			if (!this.stopSync)
			{
				this.actionFailedSync();
			}
			yield break;
		}

		public void SetIntervalAndTrialTime(float intervalTime, float trialTime)
		{
			this.intervalTime = intervalTime;
			this.trialTime = trialTime;
		}

		public void SetFailedAction(Action action)
		{
			this.actionFailedSync = action;
		}

		public void SetRequestHTTP(RequestBase request, Func<RequestBase, Coroutine> requestRoutine)
		{
			this.sendRequest = request;
			this.sendRequestRoutine = requestRoutine;
			this.sendType = ColosseumMatchingNetworkSynchronize.SendType.HTTP_REQUEST;
		}

		public void SetRequestSocket(Dictionary<string, object> message)
		{
			this.sendMessage = new Dictionary<string, object>(message);
			this.sendType = ColosseumMatchingNetworkSynchronize.SendType.SOCKET_REQUEST;
		}

		public void SetMessage(List<int> address, Dictionary<string, object> message, string type)
		{
			this.toAddress = new List<int>(address);
			this.sendMessage = new Dictionary<string, object>(message);
			this.messageType = type;
			this.sendType = ColosseumMatchingNetworkSynchronize.SendType.SOCKET_MESSASE;
		}

		public void SetWaitReceive(float waitTime)
		{
			global::Debug.Assert(1f <= waitTime, "Error SetWaitReceive : waitTime に１秒以上を設定してください.");
			this.trialTime = waitTime;
			this.sendType = ColosseumMatchingNetworkSynchronize.SendType.WAIT_RECEIVE;
		}

		public IEnumerator Synchronize()
		{
			switch (this.sendType)
			{
			case ColosseumMatchingNetworkSynchronize.SendType.HTTP_REQUEST:
				return this.SendRequestHTTP();
			case ColosseumMatchingNetworkSynchronize.SendType.SOCKET_REQUEST:
				return this.SendRequestSocket();
			case ColosseumMatchingNetworkSynchronize.SendType.SOCKET_MESSASE:
				return this.SendMessage();
			case ColosseumMatchingNetworkSynchronize.SendType.WAIT_RECEIVE:
				return this.WaitReceive();
			default:
				return null;
			}
		}

		public void StopSync()
		{
			this.SynchronizeRoutine = null;
			this.stopSync = true;
		}

		private enum SendType
		{
			HTTP_REQUEST,
			SOCKET_REQUEST,
			SOCKET_MESSASE,
			WAIT_RECEIVE
		}
	}
}
