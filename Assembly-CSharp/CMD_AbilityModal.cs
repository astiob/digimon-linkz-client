using Ability;
using Master;
using System;
using UnityEngine;

public sealed class CMD_AbilityModal : CMD_ModalMessageBtn2
{
	[SerializeField]
	private AbilityUpgradeDetail abilityUpgradeDetail;

	[SerializeField]
	private UILabel messageLabel;

	private UIWidget widget;

	private Action onPushYesButton;

	private void AdjustSize(int addDescriptionLine)
	{
		if (null == this.widget)
		{
			this.widget = base.GetComponent<UIWidget>();
		}
		this.widget.height += this.messageLabel.fontSize * addDescriptionLine + this.messageLabel.spacingY;
	}

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
		this.messageLabel.text = StringMaster.GetString("AbilityAlertInfo");
		this.abilityUpgradeDetail.ClearStatus();
	}

	public void SetStatus(MonsterAbilityStatusInfo abilityStatus)
	{
		this.abilityUpgradeDetail.SetStatus(abilityStatus);
	}

	public void SetMonsterIcon(MonsterData monsterData)
	{
		this.abilityUpgradeDetail.ShowIcon(monsterData, true);
	}

	public void SetRemovePartnerEquipChip(MonsterData partnerMonsterData)
	{
		if (partnerMonsterData.GetChipEquip().IsAttachedChip())
		{
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + StringMaster.GetString("MedalInheritPrecautionChip");
			this.AdjustSize(2);
		}
	}

	public void SetAnyNotUpdate(MonsterAbilityStatusInfo mas)
	{
		if (ClassSingleton<AbilityData>.Instance.IsAnyNotUpdate(mas))
		{
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + StringMaster.GetString("MedalInheritPrecautionRateZero");
			this.AdjustSize(1);
		}
	}

	public void SetHasGoldOver(bool hasGoldOver, int maxGoldMedalCount)
	{
		if (hasGoldOver)
		{
			string str = string.Format(StringMaster.GetString("MedalInheritPrecautionGoldOver"), maxGoldMedalCount);
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + str;
			this.AdjustSize(1);
		}
	}

	public void SetActionYesButton(Action action)
	{
		this.onPushYesButton = action;
	}
}
