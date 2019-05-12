using DigiChat.Tools;
using Master;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class CMD_ChatSearch : CMD
{
	public static CMD_ChatSearch instance;

	[SerializeField]
	private UILabel modalTitle;

	[SerializeField]
	private UILabel contentMessageLabel;

	[SerializeField]
	private BoxCollider chadIdInputCollider;

	[SerializeField]
	private UILabel chatIdInputLabel;

	[SerializeField]
	private UIInput chatIdInput;

	[SerializeField]
	private UILabel chatIdInputPlaceholder;

	[SerializeField]
	private GameObject decideButton;

	[SerializeField]
	private GameObject decideButtonGray;

	[SerializeField]
	private BoxCollider decideButtonCollider;

	[SerializeField]
	private UILabel decideBtnLabel;

	protected override void Awake()
	{
		base.Awake();
		CMD_ChatSearch.instance = this;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetGrayButton(true);
		this.setInitLabel();
		base.Show(f, sizeX, sizeY, aT);
	}

	private void setInitLabel()
	{
		this.modalTitle.text = StringMaster.GetString("ChatIdSearch-02");
		this.contentMessageLabel.text = StringMaster.GetString("ChatIdSearch-03");
		this.chatIdInputPlaceholder.text = StringMaster.GetString("SystemInput");
	}

	public void CheckInputId()
	{
		bool grayButton = false;
		this.chatIdInput.value = this.chatIdInput.value.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace(" ", string.Empty);
		if (string.IsNullOrEmpty(this.chatIdInputLabel.text) || !Regex.IsMatch(this.chatIdInput.value, "^[0-9]+$"))
		{
			grayButton = true;
		}
		this.chatIdInput.label.text = this.chatIdInput.value;
		this.SetGrayButton(grayButton);
	}

	private void SetGrayButton(bool isGray)
	{
		if (isGray)
		{
			this.decideButton.SetActive(false);
			this.decideButtonGray.SetActive(true);
		}
		else
		{
			this.decideButton.SetActive(true);
			this.decideButtonGray.SetActive(false);
		}
		this.decideBtnLabel.color = ((!isGray) ? Color.white : Color.gray);
		this.decideButtonCollider.enabled = !isGray;
	}

	public void PushedIdSearchDecisionBtn()
	{
		ChatTools.ChatLoadDisplay(true);
		long[] searchResIds = new long[]
		{
			long.Parse(this.chatIdInput.value)
		};
		GameWebAPI.RespData_ChatGroupInfo chatGroup = null;
		GameWebAPI.ChatGroupInfo request = new GameWebAPI.ChatGroupInfo
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatGroupInfo param)
			{
				param.chatGroupId = searchResIds;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatGroupInfo response)
			{
				chatGroup = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterGetChatGroupInfo(chatGroup);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
		this.chatIdInput.value = null;
	}

	private void AfterGetChatGroupInfo(GameWebAPI.RespData_ChatGroupInfo data)
	{
		if (data.groupList != null && 0 < data.groupList.Length)
		{
			CMD_ChatTop.instance.SetChatListUI(data);
			CMD_ChatTop.instance.chatGroupDefaultText.SetActive(false);
			base.ClosePanel(false);
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ChatConfirmTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ChatIdSearch-04");
		}
	}
}
