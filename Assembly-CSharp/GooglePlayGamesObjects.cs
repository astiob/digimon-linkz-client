using Master;
using System;
using UnityEngine;

public class GooglePlayGamesObjects : MonoBehaviour
{
	private readonly string SIGNIN_PLAY_GAMES_SPRITE_NAME = "Common02_ic_play_games";

	private readonly string SIGNOUT_PLAY_GAMES_SPRITE_NAME = "Common02_ic_play_games_g";

	[SerializeField]
	private UISprite googlePlayBtnSprite;

	[SerializeField]
	private GameObject googlePlayMenu;

	[SerializeField]
	private UILabel signOutText;

	[SerializeField]
	private UILabel AchievementsText;

	private bool isOpenMenu;

	private bool isLock;

	private void Awake()
	{
		this.googlePlayMenu.SetActive(false);
		this.googlePlayBtnSprite.gameObject.SetActive(false);
	}

	public void Bootup()
	{
		if (!this.Lock())
		{
			return;
		}
		this.Initialize();
	}

	private void Initialize()
	{
		this.signOutText.text = StringMaster.GetString("GooglePlayGamesObjectsUI-1");
		this.AchievementsText.text = StringMaster.GetString("GooglePlayGamesObjectsUI-2");
		this.googlePlayMenu.SetActive(false);
		this.googlePlayBtnSprite.gameObject.SetActive(true);
		GooglePlayGamesTool.Instance.Initialize(new Action<bool>(this.CallbbackSignIn));
		this.UnLock();
	}

	private void CallbbackSignIn(bool result)
	{
		this.googlePlayBtnSprite.spriteName = ((!result) ? this.SIGNOUT_PLAY_GAMES_SPRITE_NAME : this.SIGNIN_PLAY_GAMES_SPRITE_NAME);
	}

	private void ReactionSignOutConfirm(int index)
	{
		if (index == 0)
		{
			if (!this.Lock())
			{
				return;
			}
			GooglePlayGamesTool.Instance.SignOut();
			PlayerPrefs.SetInt("IsSignOutGoogle", 1);
			this.googlePlayBtnSprite.spriteName = this.SIGNOUT_PLAY_GAMES_SPRITE_NAME;
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("GoogleLogoutTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("GoogleLogoutInfo");
		}
		this.UnLock();
	}

	public void EnableMenu(bool IsEnable)
	{
		if (IsEnable == this.isOpenMenu)
		{
			return;
		}
		this.googlePlayMenu.SetActive(IsEnable);
		this.isOpenMenu = IsEnable;
	}

	private bool Lock()
	{
		if (this.isLock)
		{
			return false;
		}
		this.isLock = true;
		return true;
	}

	private void UnLock()
	{
		this.isLock = false;
	}

	public void OnClickedGooglePlay()
	{
		if (!this.Lock())
		{
			return;
		}
		if (GooglePlayGamesTool.Instance.IsSignIn)
		{
			this.EnableMenu(!this.isOpenMenu);
			this.UnLock();
		}
		else
		{
			GooglePlayGamesTool.Instance.SignIn(new Action<bool>(this.CallbbackSignIn));
			this.UnLock();
		}
	}

	public void OnClickedSignOut()
	{
		this.EnableMenu(false);
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.ReactionSignOutConfirm), "CMD_Confirm", null) as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("GoogleLogoutConfirmTitle");
		cmd_Confirm.Info = StringMaster.GetString("GoogleLogoutConfirmInfo");
	}

	public void OnClickedAchievement()
	{
		this.EnableMenu(false);
		GooglePlayGamesTool.Instance.ShowAchievementsUI();
	}
}
