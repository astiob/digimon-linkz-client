using DigiChat.Tools;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_ChatFixedPhrase : CMD
{
	[SerializeField]
	private UILabel menuWindowTitle;

	[SerializeField]
	private UILabel submitButtonLabel;

	[SerializeField]
	private GameObject submitButton;

	[SerializeField]
	private GameObject submitButtonGray;

	[SerializeField]
	private BoxCollider submitButtonCollider;

	[SerializeField]
	private List<GameObject> goFixedList;

	private string sendMessageParam { get; set; }

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		this.SetInitLabel();
		this.SetGrayButton(true);
		this.SetFixedList();
	}

	private void SetFixedList()
	{
		for (int i = 0; i < this.goFixedList.Count; i++)
		{
			UILabel componentInChildren = this.goFixedList[i].GetComponentInChildren<UILabel>();
			switch (i + 1)
			{
			case 1:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-01");
				break;
			case 2:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-02");
				break;
			case 3:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-03");
				break;
			case 4:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-04");
				break;
			case 5:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-05");
				break;
			case 6:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-06");
				break;
			case 7:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-07");
				break;
			case 8:
				componentInChildren.text = StringMaster.GetString("ChatPhrase-08");
				break;
			}
			CMD_ChatFixedPhrase.IntClone cp = new CMD_ChatFixedPhrase.IntClone();
			cp.eachCnt = i;
			GUICollider component = this.goFixedList[i].GetComponent<GUICollider>();
			component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnTouchEndedAction(cp.eachCnt);
			};
		}
	}

	private void SetInitLabel()
	{
		this.menuWindowTitle.text = StringMaster.GetString("ChatLog-10");
		this.submitButtonLabel.text = StringMaster.GetString("ChatLog-09");
	}

	public void PushedFixedPhraseSubmitBtn()
	{
		base.StartCoroutine(Singleton<TCPUtil>.Instance.SendChatMessage(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, this.sendMessageParam, ChatTools.SetMessageType(), null));
		CMD_ChatWindow.instance.SetSendLock();
		base.ClosePanel(true);
	}

	private void SetGrayButton(bool isGray)
	{
		if (isGray)
		{
			this.submitButton.SetActive(false);
			this.submitButtonGray.SetActive(true);
		}
		else
		{
			this.submitButton.SetActive(true);
			this.submitButtonGray.SetActive(false);
		}
		this.submitButtonLabel.color = ((!isGray) ? Color.white : Color.gray);
		this.submitButtonCollider.enabled = !isGray;
	}

	public void OnTouchEndedAction(int idx)
	{
		for (int i = 0; i < this.goFixedList.Count; i++)
		{
			UILabel componentInChildren = this.goFixedList[i].GetComponentInChildren<UILabel>();
			UISprite componentInChildren2 = this.goFixedList[i].GetComponentInChildren<UISprite>();
			if (i == idx)
			{
				componentInChildren2.spriteName = "Common02_Btn_SupportRed";
				componentInChildren.color = Color.white;
				this.sendMessageParam = componentInChildren.text;
			}
			else
			{
				componentInChildren2.spriteName = "Common02_Btn_SupportWhite";
				componentInChildren.color = ChatConstValue.CHAT_DEF_TXT_COLOR;
			}
		}
		this.SetGrayButton(false);
	}

	private class IntClone
	{
		public int eachCnt;
	}
}
