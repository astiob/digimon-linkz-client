using System;
using UnityEngine;

public sealed class Loading : MonoBehaviour
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
		}
	}

	public static void Display(Loading.LoadingType type, bool isMask)
	{
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
		Loading.Instance.SetMaskActive(false);
		if (Loading.Instance.smallLoading.activeSelf || Loading.Instance.largeLoading.activeSelf)
		{
			Loading.Instance.smallLoading.SetActive(false);
			Loading.Instance.largeLoading.SetActive(false);
		}
		else
		{
			global::Debug.Log("<color=blue>Loading 消去済み</color>");
		}
	}

	public static bool IsShow()
	{
		return Loading.Instance.smallLoading.activeSelf || Loading.Instance.largeLoading.activeSelf;
	}

	private void StartLoading(Loading.LoadingType type, bool isMask)
	{
		if (this.loadingType == Loading.LoadingType.SMALL)
		{
			this.SetMaskActive(this.enableMask);
			this.SetLoadingActive(this.smallLoading);
		}
		else if (this.loadingType == Loading.LoadingType.LARGE)
		{
			this.SetMaskActive(this.enableMask);
			this.SetLoadingActive(this.largeLoading);
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

	private void SetLoadingActive(GameObject loading)
	{
		if (!loading.activeSelf)
		{
			loading.SetActive(true);
		}
		else
		{
			global::Debug.Log("<color=blue>Loading 表示済み</color>");
		}
	}

	public enum LoadingType
	{
		SMALL,
		LARGE
	}
}
