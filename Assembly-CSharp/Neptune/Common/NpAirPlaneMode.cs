using System;

namespace Neptune.Common
{
	public class NpAirPlaneMode : NpSingleton<NpAirPlaneMode>
	{
		private static INpAirPlaneMode mCallbackListener;

		public void EnableDebugLog(bool isDebug)
		{
			NpAirPlaneModeAndroid.EnableDebugLog(isDebug);
		}

		public bool GetAirPlaneMode()
		{
			return NpAirPlaneModeAndroid.GetAirPlaneMode();
		}

		public void SetAirPlaneModeRecieverEnable(bool enable)
		{
			NpAirPlaneModeAndroid.SetAirPlaneModeRecieverEnable(enable);
		}

		public void SetOnAirPlaneModeChangedListener(INpAirPlaneMode Listener)
		{
			NpAirPlaneMode.mCallbackListener = Listener;
		}

		public bool GetAirPlanModeSwitching()
		{
			return NpAirPlaneModeAndroid.GetAirPlanModeSwitching();
		}

		private void OnAirPlaneModeChanged(string state)
		{
			bool state2 = state.ToLower() == "true";
			if (NpAirPlaneMode.mCallbackListener != null)
			{
				NpAirPlaneMode.mCallbackListener.OnAirPlaneModeChanged(state2);
			}
		}
	}
}
