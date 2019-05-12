using System;

public static class FarmCameraControlForCMD
{
	private static int refCT;

	public static void ClearRefCT()
	{
		FarmCameraControlForCMD.refCT = 0;
		if (FarmRoot.Instance != null)
		{
			FarmRoot.Instance.Camera.enabled = true;
		}
	}

	public static void Off()
	{
		if (FarmCameraControlForCMD.refCT == 0 && FarmRoot.Instance != null)
		{
			FarmRoot.Instance.Camera.enabled = false;
			FarmRoot.Instance.DigimonManager.SetActiveDigimon(false);
		}
		FarmCameraControlForCMD.refCT++;
	}

	public static void On()
	{
		if (FarmCameraControlForCMD.refCT > 0)
		{
			FarmCameraControlForCMD.refCT--;
		}
		if (FarmCameraControlForCMD.refCT == 0 && FarmRoot.Instance != null)
		{
			FarmRoot.Instance.Camera.enabled = true;
			FarmRoot.Instance.DigimonManager.SetActiveDigimon(true);
		}
	}

	public static void ClearCount()
	{
		FarmCameraControlForCMD.refCT = 0;
	}
}
