using System;
using UnityEngine;

namespace SmartBeat
{
	public class SmartBeatBehaviour : MonoBehaviour
	{
		private const string APP_KEY = "10d19da5-f83a-4f44-9d5a-dede5d742f3b";

		private void Awake()
		{
			SmartBeat.init("10d19da5-f83a-4f44-9d5a-dede5d742f3b", this.enabled());
			this.init();
		}

		private void init()
		{
		}

		private new bool enabled()
		{
			return true;
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			SmartBeat.onPause(pauseStatus);
		}
	}
}
