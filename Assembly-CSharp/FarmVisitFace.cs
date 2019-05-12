using System;
using UnityEngine;

public class FarmVisitFace : MonoBehaviour
{
	[SerializeField]
	private UILabel friendUserNameLabel;

	[SerializeField]
	public UITexture friendUserTitleIcon;

	public static FarmVisitFace Create()
	{
		string path = "UICommon/Farm/FarmVisit";
		UnityEngine.Object original = AssetDataMng.Instance().LoadObject(path, null, true);
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.transform.SetParent(Singleton<GUIMain>.Instance.transform);
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<FarmVisitFace>();
	}

	public string friendUserName
	{
		set
		{
			this.friendUserNameLabel.text = value;
		}
	}

	public Action onFriendProfile { get; set; }

	public Action onFriendList { get; set; }

	public Action onBackFarm { get; set; }

	private void OnFriendProfile()
	{
		if (this.onFriendProfile != null)
		{
			this.onFriendProfile();
		}
	}

	private void OnFriendList()
	{
		if (this.onFriendList != null)
		{
			this.onFriendList();
		}
	}

	private void OnBackFarm()
	{
		if (this.onBackFarm != null)
		{
			this.onBackFarm();
		}
	}

	public void Destroy()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}
}
