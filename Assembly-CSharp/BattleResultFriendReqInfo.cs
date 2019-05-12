using Master;
using System;
using UnityEngine;

public sealed class BattleResultFriendReqInfo : MonoBehaviour
{
	[SerializeField]
	private UILabel TXT_NAME;

	[SerializeField]
	private UILabel TXT_LV;

	[SerializeField]
	private UISprite buttonSprite;

	[SerializeField]
	private UILabel buttonLabel;

	[SerializeField]
	private BoxCollider buttonCollider;

	[SerializeField]
	private Transform iconAnchor;

	private string userId;

	public void SetStatusInfo(string user_id, string user_name, int leader_lv, string leader_monster_id, BattleResultFriendReqInfo.FRIEND_TYPE friend_type)
	{
		this.userId = user_id;
		this.TXT_NAME.text = user_name;
		this.TXT_LV.text = leader_lv.ToString();
		MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(leader_monster_id);
		GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(monsterData, this.iconAnchor.localScale, this.iconAnchor.localPosition, this.iconAnchor.parent, true, false);
		guimonsterIcon.name = "DigimonIcon";
		UIWidget[] componentsInChildren = guimonsterIcon.GetComponentsInChildren<UIWidget>();
		foreach (UIWidget uiwidget in componentsInChildren)
		{
			uiwidget.depth += 1000;
		}
		this.ButtonSetActive(friend_type);
	}

	private void ButtonSetActive(BattleResultFriendReqInfo.FRIEND_TYPE friend_type)
	{
		this.buttonSprite.spriteName = ((friend_type != BattleResultFriendReqInfo.FRIEND_TYPE.NONE) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON1");
		this.buttonLabel.color = ((friend_type != BattleResultFriendReqInfo.FRIEND_TYPE.NONE) ? Color.gray : Color.white);
		this.buttonCollider.enabled = (friend_type == BattleResultFriendReqInfo.FRIEND_TYPE.NONE);
		switch (friend_type)
		{
		case BattleResultFriendReqInfo.FRIEND_TYPE.NONE:
			this.buttonLabel.text = StringMaster.GetString("BattleResult-07");
			break;
		case BattleResultFriendReqInfo.FRIEND_TYPE.FRIEND:
			this.buttonLabel.text = StringMaster.GetString("FriendTitle");
			break;
		case BattleResultFriendReqInfo.FRIEND_TYPE.APPLYING:
			this.buttonLabel.text = StringMaster.GetString("BattleResult-06");
			break;
		case BattleResultFriendReqInfo.FRIEND_TYPE.STAY:
			this.buttonLabel.text = StringMaster.GetString("BattleResult-05");
			break;
		}
	}

	public void OnClickedFriendRequest()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask task = APIUtil.Instance().RequestFriendApplication(this.userId, true);
		AppCoroutine.Start(task.Run(new Action(this.EndFriendRequest), new Action<Exception>(this.RequestFailed), null), false);
	}

	private void EndFriendRequest()
	{
		RestrictionInput.EndLoad();
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int index)
		{
			this.ButtonSetActive(BattleResultFriendReqInfo.FRIEND_TYPE.APPLYING);
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		if (null != cmd_ModalMessage)
		{
			cmd_ModalMessage.Title = StringMaster.GetString("Profile-13");
			cmd_ModalMessage.Info = StringMaster.GetString("Profile-14");
		}
	}

	private void RequestFailed(Exception e)
	{
		RestrictionInput.EndLoad();
	}

	public enum FRIEND_TYPE
	{
		NONE,
		FRIEND,
		APPLYING,
		STAY
	}
}
