using System;

public abstract class BattleUIControlMultiBasic : BattleUIControlBasic
{
	public virtual MultiConnetionMessage connetionMessage
	{
		get
		{
			return (base.ui.battleAlwaysUi as MultiBattleAlways).connectionMessage;
		}
	}

	public void ShowPrepareMessage()
	{
		this.connetionMessage.ShowPrepareMessage();
	}

	public void HidePrepareMessage()
	{
		this.connetionMessage.HideLoading();
	}

	public void ShowLoading(bool isEnemy)
	{
		this.connetionMessage.ShowLoading(isEnemy);
	}

	public void HideLoading()
	{
		this.connetionMessage.HideLoading();
	}

	protected enum guardCilliderSkin
	{
		None,
		Prepare,
		Commnad
	}
}
