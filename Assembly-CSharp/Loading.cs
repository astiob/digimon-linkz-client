using System;
using UnityEngine;

public class Loading : MonoBehaviour
{
	[SerializeField]
	private GameObject smallLoading;

	[SerializeField]
	private GameObject largeLoading;

	[SerializeField]
	private UISprite mask;

	private Loading.LoadingType loadingType;

	private bool enableMask;

	public static Loading Instance;

	private bool isResumeDisplay;

	public static void Initialize(Transform parentTransform)
	{
		if (null == Loading.Instance)
		{
			GameObject original = Resources.Load("UICommon/Load/Loading") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.parent = parentTransform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			Loading.Instance = gameObject.GetComponent<Loading>();
			Loading.Instance.mask.enabled = false;
			GameObject gameObject2 = NativeLoading.Instance.gameObject;
			if (null != gameObject2)
			{
				gameObject2.transform.parent = Loading.Instance.transform;
			}
		}
	}

	public static void Display(Loading.LoadingType type, bool isMask)
	{
		global::Debug.Log("<color=green>loading.start</color>");
		Loading.Instance.loadingType = type;
		Loading.Instance.enableMask = isMask;
		Loading.Instance.StartLoading(type, isMask);
	}

	public static void ResumeDisplay()
	{
		Loading.Instance.StartLoading(Loading.Instance.loadingType, Loading.Instance.enableMask);
	}

	public static void DisableMask()
	{
		Loading.Instance.SetMaskActive(false);
	}

	public static void Invisible()
	{
		global::Debug.Log("<color=green>loading.end</color>");
		Loading.Instance.SetMaskActive(false);
		NativeLoading.Instance.StopAnimation();
	}

	public static bool IsShow()
	{
		return NativeLoading.Instance.IsLoading;
	}

	private void StartLoading(Loading.LoadingType type, bool isMask)
	{
		if (this.loadingType == Loading.LoadingType.SMALL)
		{
			this.SetMaskActive(this.enableMask);
			NativeLoading.Instance.StartAnimation(NativeLoading.LoadingType.Short);
		}
		else if (this.loadingType == Loading.LoadingType.LARGE)
		{
			this.SetMaskActive(this.enableMask);
			NativeLoading.Instance.StartAnimation(NativeLoading.LoadingType.Long);
		}
	}

	private void SetMaskActive(bool isEnable)
	{
		if (isEnable && !this.mask.enabled)
		{
			this.mask.enabled = true;
		}
		else if (!isEnable && this.mask.enabled)
		{
			this.mask.enabled = false;
		}
	}

	private void OnApplicationPause(bool isPause)
	{
		if (isPause)
		{
			if (NativeLoading.Instance.IsLoading)
			{
				this.isResumeDisplay = true;
				NativeLoading.Instance.StopAnimation();
			}
		}
		else if (this.isResumeDisplay)
		{
			this.isResumeDisplay = false;
			if (this.loadingType == Loading.LoadingType.SMALL)
			{
				NativeLoading.Instance.StartAnimation(NativeLoading.LoadingType.Short);
			}
			else if (this.loadingType == Loading.LoadingType.LARGE)
			{
				NativeLoading.Instance.StartAnimation(NativeLoading.LoadingType.Long);
			}
		}
		else
		{
			ServerDateTime.UpdateServerDateTime();
		}
	}

	private void OnApplicationQuit()
	{
		NativeLoading.Instance.StopAnimation();
	}

	public enum LoadingType
	{
		SMALL,
		LARGE
	}
}
