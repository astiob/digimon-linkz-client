using Master;
using System;
using UnityEngine;

public sealed class MonsterLearnSkill : MonoBehaviour
{
	private string NONE_ATTRIBUTE_SPRITE_NAME = "Battle_Attribute_1";

	private string PREFIX_ATTRIBUTE_SPRITE_NAME = "Battle_Attribute_";

	[SerializeField]
	private bool isUniqueSkill;

	[SerializeField]
	private UILabel title;

	[SerializeField]
	private UILabel skillName;

	[SerializeField]
	private UILabel skillInfo;

	[SerializeField]
	private UILabel skillCost;

	[SerializeField]
	private UISprite skillAttribute;

	private void Start()
	{
		if (this.isUniqueSkill)
		{
			this.title.text = StringMaster.GetString("CharaStatus-19");
		}
		else
		{
			this.title.text = StringMaster.GetString("CharaStatus-20");
		}
	}

	private void SetSkillUI(string skillName, string skillInfo, string skillCost, int skillAttribute)
	{
		this.skillName.text = skillName;
		this.skillInfo.text = skillInfo;
		this.skillCost.text = string.Format(StringMaster.GetString("BattleSkillUI-01"), skillCost);
		this.skillAttribute.enabled = true;
		this.SetAttributeIcon(this.skillAttribute, skillAttribute);
	}

	private void SetAttributeIcon(UISprite icon, int attribute)
	{
		if (0 < attribute)
		{
			icon.spriteName = this.PREFIX_ATTRIBUTE_SPRITE_NAME + attribute.ToString();
		}
		else
		{
			icon.spriteName = this.NONE_ATTRIBUTE_SPRITE_NAME;
		}
	}

	public void ClearSkill()
	{
		this.skillName.text = StringMaster.GetString("SystemNone");
		this.skillInfo.text = StringMaster.GetString("CharaStatus-02");
		this.skillCost.text = string.Empty;
		this.skillAttribute.enabled = false;
	}

	public void SetSkill(MonsterData monsterData)
	{
		if (this.isUniqueSkill)
		{
			if (monsterData.actionSkillM != null)
			{
				this.SetSkillUI(monsterData.actionSkillM.name, monsterData.actionSkillM.description, monsterData.actionSkillM.needPoint, monsterData.actionSkillDetailM.attribute);
			}
		}
		else if (monsterData.commonSkillM != null)
		{
			this.SetSkillUI(monsterData.commonSkillM.name, monsterData.commonSkillM.description, monsterData.commonSkillM.needPoint, monsterData.commonSkillDetailM.attribute);
		}
	}
}
