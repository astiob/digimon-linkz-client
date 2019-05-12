using Master;
using System;
using UnityEngine;

public sealed class CMD_VersionUpModal : CMD_ModalMessageBtn2
{
	[Header("バージョンアップ詳細スクリプト")]
	[SerializeField]
	private VersionUpDetail versionUpDitail;

	[Header("説明文・警告のメッセージのラベル")]
	[SerializeField]
	private UILabel messageLabel;

	private Action onPushYesButton;

	private void OnPushYesButton()
	{
		if (this.onPushYesButton != null)
		{
			this.onPushYesButton();
		}
		this.ClosePanel(true);
	}

	protected override void Awake()
	{
		base.Awake();
		base.SetTitle(StringMaster.GetString("SystemConfirm"));
		base.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		base.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	public void SetMonsterIcon(MonsterData monsterData)
	{
		this.versionUpDitail.SetMonsterIcon(monsterData, true);
	}

	public void ShowDetail(int beforeMaxLevel, int afterMaxLevel, bool isSkillAdd)
	{
		this.versionUpDitail.ShowDetail(beforeMaxLevel, afterMaxLevel, isSkillAdd);
	}

	public void SetActionYesButton(Action action)
	{
		this.onPushYesButton = action;
	}

	public void SetDescription(MonsterData monsterData)
	{
		if (monsterData.GetChipEquip().IsAttachedChip())
		{
			this.messageLabel.text = StringMaster.GetString("VersionUpAlertInfo") + "\n" + StringMaster.GetString("VerUPPrecautionChip");
			base.GetComponent<UIWidget>().height += this.messageLabel.fontSize * 2 + this.messageLabel.spacingY;
		}
		else
		{
			this.messageLabel.text = StringMaster.GetString("VersionUpAlertInfo");
		}
	}
}
