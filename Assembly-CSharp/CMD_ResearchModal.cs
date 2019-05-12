using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_ResearchModal : CMD_ModalMessageBtn2
{
	[SerializeField]
	private UILabel beforeLabel;

	[SerializeField]
	private UILabel afterLabel;

	[SerializeField]
	private UISprite beforeRarity;

	[SerializeField]
	private UISprite afterRarity;

	[SerializeField]
	private LaboratoryPartsStatusDetail statusDetail;

	[SerializeField]
	[Header("説明文・警告のメッセージのラベル")]
	private UILabel messageLabel;

	protected override void Awake()
	{
		base.Awake();
		base.SetTitle(StringMaster.GetString("SystemConfirm"));
		this.messageLabel.text = StringMaster.GetString("LaboratoryResearchInfo");
		base.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		base.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
		this.statusDetail.ClearDigitamaStatus();
	}

	public void SetDigitamaStatus(MonsterEggStatusInfo status)
	{
		this.statusDetail.SetDigitamaStatus(status);
		int num = status.rare.ToInt32();
		string arousalSpriteName = MonsterDetailUtil.GetArousalSpriteName(num);
		this.SetArousalValue(this.beforeRarity, this.beforeLabel, arousalSpriteName);
		if (status.isReturn)
		{
			num--;
			this.afterRarity.spriteName = MonsterDetailUtil.GetArousalSpriteName(num);
		}
		else if (!status.isArousal)
		{
			this.SetArousalValue(this.afterRarity, this.afterLabel, arousalSpriteName);
		}
		else
		{
			num++;
			this.afterRarity.spriteName = MonsterDetailUtil.GetArousalSpriteName(num);
		}
	}

	private void SetArousalValue(UISprite arousal, UILabel label, string spriteName)
	{
		if (string.IsNullOrEmpty(spriteName))
		{
			label.gameObject.SetActive(true);
			label.text = StringMaster.GetString("CharaStatus-01");
			arousal.gameObject.SetActive(false);
		}
		else
		{
			arousal.spriteName = spriteName;
		}
	}

	public void SetChipParams(MonsterData baseMonsterData, MonsterData partnerMonsterData)
	{
		bool flag = MonsterDataMng.Instance().HasChip(new List<MonsterData>
		{
			baseMonsterData,
			partnerMonsterData
		});
		if (flag)
		{
			UILabel uilabel = this.messageLabel;
			uilabel.text = uilabel.text + "\n" + StringMaster.GetString("LaboratoryPrecautionChip");
			base.GetComponent<UIWidget>().height += this.messageLabel.fontSize * 2 + this.messageLabel.spacingY;
		}
	}
}
