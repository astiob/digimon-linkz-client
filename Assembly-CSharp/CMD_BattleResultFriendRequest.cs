using Master;
using System;
using System.Collections;
using UnityEngine;

public class CMD_BattleResultFriendRequest : CMD
{
	[SerializeField]
	private GameObject battleResultFriendReqInfoSingleWrap;

	[SerializeField]
	private BattleResultFriendReqInfo battleResultFriendReqInfoSingle;

	[SerializeField]
	private GameObject battleResultFriendReqInfoWrap;

	[SerializeField]
	private BattleResultFriendReqInfo[] battleResultFriendReqInfo;

	[SerializeField]
	private GameObject goCloseBtn;

	[SerializeField]
	private UILabel closeButtonLabel;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.Init();
		aT = 0.1f;
		base.Show(f, sizeX, sizeY, aT);
	}

	private void Init()
	{
		this.battleResultFriendReqInfoWrap.gameObject.SetActive(false);
		this.battleResultFriendReqInfoSingleWrap.gameObject.SetActive(false);
		this.goCloseBtn.gameObject.SetActive(false);
		this.closeButtonLabel.text = StringMaster.GetString("SystemButtonClose");
		AppCoroutine.Start(this.ReloadFriend(), false);
	}

	private IEnumerator ReloadFriend()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask task = APIUtil.Instance().RequestFriendData(true);
		yield return base.StartCoroutine(task.Run(null, null, null));
		RestrictionInput.EndLoad();
		this.CreateFriendReqUI();
		yield break;
	}

	private void CreateFriendReqUI()
	{
		this.goCloseBtn.gameObject.SetActive(true);
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		int num = respData_WorldMultiStartInfo.party.Length - 1;
		if (num > 1)
		{
			int num2 = 0;
			for (int i = 0; i < respData_WorldMultiStartInfo.party.Length; i++)
			{
				if (DataMng.Instance().RespDataCM_Login.playerInfo.UserId != int.Parse(respData_WorldMultiStartInfo.party[i].userId))
				{
					this.battleResultFriendReqInfoWrap.gameObject.SetActive(true);
					this.battleResultFriendReqInfo[num2].SetStatusInfo(respData_WorldMultiStartInfo.party[i].userId, respData_WorldMultiStartInfo.party[i].nickname, int.Parse(respData_WorldMultiStartInfo.party[i].userMonsters[0].level), respData_WorldMultiStartInfo.party[i].userMonsters[0].monsterId, respData_WorldMultiStartInfo.party[i].titleId, this.GetFriendType(respData_WorldMultiStartInfo.party[i].userId));
					num2++;
				}
			}
		}
		else
		{
			for (int j = 0; j < respData_WorldMultiStartInfo.party.Length; j++)
			{
				if (DataMng.Instance().RespDataCM_Login.playerInfo.UserId != int.Parse(respData_WorldMultiStartInfo.party[j].userId))
				{
					this.battleResultFriendReqInfoSingleWrap.gameObject.SetActive(true);
					this.battleResultFriendReqInfoSingle.SetStatusInfo(respData_WorldMultiStartInfo.party[j].userId, respData_WorldMultiStartInfo.party[j].nickname, int.Parse(respData_WorldMultiStartInfo.party[j].userMonsters[0].level), respData_WorldMultiStartInfo.party[j].userMonsters[0].monsterId, respData_WorldMultiStartInfo.party[j].titleId, this.GetFriendType(respData_WorldMultiStartInfo.party[j].userId));
				}
			}
		}
	}

	private BattleResultFriendReqInfo.FRIEND_TYPE GetFriendType(string userId)
	{
		GameWebAPI.FriendList[] friendList = DataMng.Instance().RespDataFR_FriendList.friendList;
		foreach (GameWebAPI.FriendList friendList2 in friendList)
		{
			if (friendList2.userData.userId == userId)
			{
				return BattleResultFriendReqInfo.FRIEND_TYPE.FRIEND;
			}
		}
		friendList = DataMng.Instance().RespDataFR_FriendRequestList.friendList;
		foreach (GameWebAPI.FriendList friendList3 in friendList)
		{
			if (friendList3.userData.userId == userId)
			{
				return BattleResultFriendReqInfo.FRIEND_TYPE.APPLYING;
			}
		}
		friendList = DataMng.Instance().RespDataFR_FriendUnapprovedList.friendList;
		foreach (GameWebAPI.FriendList friendList4 in friendList)
		{
			if (friendList4.userData.userId == userId)
			{
				return BattleResultFriendReqInfo.FRIEND_TYPE.STAY;
			}
		}
		return BattleResultFriendReqInfo.FRIEND_TYPE.NONE;
	}

	public void OnPushedCloseButton()
	{
		DataMng.Instance().RespData_WorldMultiStartInfo = null;
		this.ClosePanel(true);
	}
}
