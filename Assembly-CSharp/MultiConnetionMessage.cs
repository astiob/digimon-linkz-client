using Master;
using System;
using UnityEngine;

public sealed class MultiConnetionMessage : MonoBehaviour
{
	[SerializeField]
	[Header("開始準備中ローカライズ")]
	private UILabel prepareMessageLocalize;

	[SerializeField]
	[Header("コマンド入力中ローカライズ")]
	private UILabel cmdSelectMessageeLocalize;

	[SerializeField]
	[Header("開始準備中オブジェクト")]
	private GameObject prepareStartGO;

	[SerializeField]
	[Header("コマンド入力中オブジェクト")]
	private GameObject commandWaitingGO;

	private string myWaitingCommand = string.Empty;

	private string enemyWaitingCommand = string.Empty;

	private void Awake()
	{
		this.SetLocalize();
	}

	private void SetLocalize()
	{
		this.prepareMessageLocalize.text = StringMaster.GetString("BattleUI-38");
		this.myWaitingCommand = StringMaster.GetString("BattleUI-43");
		this.enemyWaitingCommand = StringMaster.GetString("BattleUI-39");
	}

	public void ShowPrepareMessage()
	{
		NGUITools.SetActiveSelf(this.prepareStartGO, true);
		NGUITools.SetActiveSelf(this.commandWaitingGO, false);
	}

	public void ShowLoading(bool isEnemy)
	{
		NGUITools.SetActiveSelf(this.prepareStartGO, false);
		if (isEnemy)
		{
			this.cmdSelectMessageeLocalize.text = this.enemyWaitingCommand;
		}
		else
		{
			this.cmdSelectMessageeLocalize.text = this.myWaitingCommand;
		}
		NGUITools.SetActiveSelf(this.commandWaitingGO, true);
	}

	public void HideLoading()
	{
		NGUITools.SetActiveSelf(this.prepareStartGO, false);
		NGUITools.SetActiveSelf(this.commandWaitingGO, false);
	}
}
