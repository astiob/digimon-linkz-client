using Master;
using System;
using UI.Common;
using UnityEngine;

public sealed class CMD_UpperlimitChip : CMD
{
	[SerializeField]
	private UILabel message;

	[SerializeField]
	private UILabel reinforcementButtonLabel;

	[SerializeField]
	private UILabel listButtonLabel;

	[SerializeField]
	private UILabel closeButtonLabel;

	protected override void Awake()
	{
		base.Awake();
		this.reinforcementButtonLabel.text = StringMaster.GetString("ChipReinforceTitle");
		this.listButtonLabel.text = StringMaster.GetString("ChipListTitle");
		this.closeButtonLabel.text = StringMaster.GetString("SystemButtonClose");
	}

	public void SetType(CMD_UpperlimitChip.MessageType type)
	{
		if (type != CMD_UpperlimitChip.MessageType.GASHA)
		{
			if (type != CMD_UpperlimitChip.MessageType.PRESENTS)
			{
				if (type == CMD_UpperlimitChip.MessageType.QUEST)
				{
					this.message.text = StringMaster.GetString("ChipOverQuest");
				}
			}
			else
			{
				this.message.text = StringMaster.GetString("ChipOverPresent");
			}
		}
		else
		{
			this.message.text = StringMaster.GetString("ChipOverGasha");
		}
	}

	public void SetNoticeMessage(LimitOverNoticeType type)
	{
		if (type != LimitOverNoticeType.GASHA)
		{
			if (type != LimitOverNoticeType.PRESENTS)
			{
				if (type == LimitOverNoticeType.QUEST)
				{
					this.message.text = StringMaster.GetString("ChipOverQuest");
				}
			}
			else
			{
				this.message.text = StringMaster.GetString("ChipOverPresent");
			}
		}
		else
		{
			this.message.text = StringMaster.GetString("ChipOverGasha");
		}
	}

	private void OnPushedReinforcementButton()
	{
		base.SetCloseAction(delegate(int x)
		{
			CMD_ChipReinforcement.Create(null);
		});
		base.ClosePanel(true);
	}

	private void OnPushedListButton()
	{
		base.SetCloseAction(delegate(int x)
		{
			CMD_ChipAdministration.Create(null);
		});
		base.ClosePanel(true);
	}

	public enum MessageType
	{
		GASHA,
		PRESENTS,
		QUEST
	}
}
