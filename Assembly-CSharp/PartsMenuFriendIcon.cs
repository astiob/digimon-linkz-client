using System;
using UnityEngine;

public class PartsMenuFriendIcon : MonoBehaviour
{
	private void Start()
	{
		ClassSingleton<PartsMenuFriendIconAccessor>.Instance.partsMenuFriendIcon = this;
	}

	public void FrinedListCheck()
	{
		GameWebAPI.RespDataMP_MyPage respDataMP_MyPage = DataMng.Instance().RespDataMP_MyPage;
		if (respDataMP_MyPage != null)
		{
			int newFriend = respDataMP_MyPage.userNewsCountList.newFriend;
			int friendApplication = respDataMP_MyPage.userNewsCountList.friendApplication;
			base.gameObject.SetActive(newFriend > 0 || friendApplication > 0);
		}
	}
}
