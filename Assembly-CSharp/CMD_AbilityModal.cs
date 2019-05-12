using Ability;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_AbilityModal : CMD_ModalMessageBtn2
{
	[SerializeField]
	private AbilityUpgradeDetail abilityUpgradeDetail;

	[Header("説明文・警告のメッセージのラベル")]
	[SerializeField]
	private UILabel messageLabel;

	protected override void Awake()
	{
		base.Awake();
		base.SetTitle(StringMaster.GetString("SystemConfirm"));
		this.messageLabel.text = StringMaster.GetString("AbilityAlertInfo");
		base.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		base.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
		this.abilityUpgradeDetail.ClearStatus();
	}

	public void SetStatus(MonsterAbilityStatusInfo abilityStatus)
	{
		this.abilityUpgradeDetail.SetStatus(abilityStatus);
	}

	public void ShowIcon(MonsterData md, bool active = true)
	{
		this.abilityUpgradeDetail.ShowIcon(md, active);
	}

	public void SetChipParams(MonsterData partnerMonsterData)
	{
		bool flag = MonsterDataMng.Instance().HasChip(new List<MonsterData>
		{
			partnerMonsterData
		});
		if (flag)
		{
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + StringMaster.GetString("MedalInheritPrecautionChip");
			base.GetComponent<UIWidget>().height += this.messageLabel.fontSize * 2 + this.messageLabel.spacingY;
		}
	}

	public void SetAnyNotUpdate(MonsterAbilityStatusInfo mas)
	{
		bool flag = ClassSingleton<AbilityData>.Instance.IsAnyNotUpdate(mas);
		if (flag)
		{
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + StringMaster.GetString("MedalInheritPrecautionRateZero");
			base.GetComponent<UIWidget>().height += this.messageLabel.fontSize * 1 + this.messageLabel.spacingY;
		}
	}

	public void SetHasGoldOver(bool hasGoldOver)
	{
		if (hasGoldOver)
		{
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + string.Format(StringMaster.GetString("MedalInheritPrecautionGoldOver"), ConstValue.MAX_GOLD_MEDAL_COUNT);
			base.GetComponent<UIWidget>().height += this.messageLabel.fontSize * 1 + this.messageLabel.spacingY;
		}
	}
}
