using System;
using UnityEngine;

namespace SmartBeat
{
	public class SmartBeatBehaviour : MonoBehaviour
	{
		private const string APP_KEY = "3bbe2b4f-10e9-493d-ab2e-4a120c1c1c5b";

		private void Awake()
		{
			SmartBeat.init("3bbe2b4f-10e9-493d-ab2e-4a120c1c1c5b", this.enabled());
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
