using Master;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_VersionUpModal : CMD_ModalMessageBtn2
{
	[SerializeField]
	[Header("バージョンアップ詳細スクリプト")]
	private VersionUpDetail versionUpDitail;

	[Header("説明文・警告のメッセージのラベル")]
	[SerializeField]
	private UILabel messageLabel;

	protected override void Awake()
	{
		base.Awake();
		base.SetTitle(StringMaster.GetString("SystemConfirm"));
		this.messageLabel.text = StringMaster.GetString("VersionUpAlertInfo");
		base.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		base.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	public void ShowIcon(MonsterData md, bool active = true)
	{
		this.versionUpDitail.ShowIcon(md, active);
	}

	public void ShowDetail(int oldLev, int newLev, bool isSkillAdd)
	{
		this.versionUpDitail.ShowDetail(oldLev, newLev, isSkillAdd);
	}

	public void SetChipParams(MonsterData baseMonsterData)
	{
		bool flag = MonsterUserDataMng.AnyChipEquipMonster(new List<MonsterData>
		{
			baseMonsterData
		});
		if (flag)
		{
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + StringMaster.GetString("VerUPPrecautionChip");
			base.GetComponent<UIWidget>().height += this.messageLabel.fontSize * 2 + this.messageLabel.spacingY;
		}
	}
}
