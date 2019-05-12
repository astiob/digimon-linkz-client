using Master;
using System;
using System.Collections;
using UnityEngine;

public class TakeoverInput : MonoBehaviour
{
	[SerializeField]
	private UISprite takeoverButtonSprite;

	[SerializeField]
	private UILabel takeoverButtonLabel;

	[SerializeField]
	private BoxCollider takeoverButtonCollider;

	[SerializeField]
	private UIInput takeoverCodeInput;

	[SerializeField]
	private UIInput userIdInput;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel takeOverLabel;

	[SerializeField]
	private UILabel topLabel;

	[SerializeField]
	private UILabel underLabel;

	private string userCodeInputDefaultString = string.Empty;

	private string takeoverCodeInputDefaultString = string.Empty;

	public void Initialize()
	{
		this.titleLabel.text = StringMaster.GetString("TakeOver-16");
		this.takeOverLabel.text = StringMaster.GetString("TakeOver-22");
		this.topLabel.text = StringMaster.GetString("TakeOver-23");
		this.underLabel.text = StringMaster.GetString("TakeOver-06");
		string @string = StringMaster.GetString("TakeOver-16");
		this.takeoverCodeInput.label.text = @string;
		this.takeoverCodeInput.defaultText = @string;
		this.takeoverCodeInput.value = string.Empty;
		string string2 = StringMaster.GetString("FriendSearch-02");
		this.userIdInput.label.text = string2;
		this.userIdInput.defaultText = string2;
		this.userIdInput.value = string.Empty;
		if (this.takeoverCodeInput != null)
		{
			this.takeoverCodeInputDefaultString = this.takeoverCodeInput.value;
		}
		if (this.userIdInput != null)
		{
			this.userCodeInputDefaultString = this.userIdInput.value;
		}
	}

	private void TakeoverButtonSetActive(bool IsEnable)
	{
		this.takeoverButtonSprite.spriteName = ((!IsEnable) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON");
		this.takeoverButtonLabel.color = ((!IsEnable) ? Color.gray : Color.white);
		this.takeoverButtonCollider.enabled = IsEnable;
	}

	private void AuthenticateTakeover()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.StartCoroutine(this.TryTakeover(delegate(bool Result)
		{
			RestrictionInput.EndLoad();
			if (Result)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int index)
				{
					PlayerPrefsExtentions.DeleteAllGameParams();
					GameCache.ClearCache(null);
					this.ChangeSceneToUILogo();
				}, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("TakeOver-10");
				cmd_ModalMessage.Info = StringMaster.GetString("TakeOver-11");
			}
		}));
	}

	private IEnumerator TryTakeover(Action<bool> TryResult)
	{
		GameWebAPI.Request_CM_TakeoverInput request = new GameWebAPI.Request_CM_TakeoverInput
		{
			SetSendData = delegate(GameWebAPI.CM_Req_TakeoverInput param)
			{
				param.transferUserCode = this.userIdInput.value;
				param.transferCode = this.takeoverCodeInput.value;
			}
		};
		APIRequestTask apirequestTask = new APIRequestTask(request, false);
		apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestPlayerInfo(true));
		return apirequestTask.Run(delegate
		{
			TryResult(true);
		}, delegate(Exception nop)
		{
			TryResult(false);
		}, null);
	}

	private void ChangeSceneToUILogo()
	{
		GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
	}

	public void OnClickedTakeover()
	{
		if (this.takeoverCodeInput.value != this.takeoverCodeInputDefaultString && !string.IsNullOrEmpty(this.takeoverCodeInput.value) && this.userIdInput.value != this.userCodeInputDefaultString && !string.IsNullOrEmpty(this.userIdInput.value))
		{
			this.AuthenticateTakeover();
		}
	}

	public void OnChangeInput()
	{
		if (this.takeoverCodeInput.value != this.takeoverCodeInputDefaultString && !string.IsNullOrEmpty(this.takeoverCodeInput.value) && this.userIdInput.value != this.userCodeInputDefaultString && this.userIdInput.value.Length == 8)
		{
			this.TakeoverButtonSetActive(true);
		}
		else
		{
			this.TakeoverButtonSetActive(false);
		}
	}
}
