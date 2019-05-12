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
	private int successionSkillSlotId;

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

	[SerializeField]
	private bool shortenName;

	[SerializeField]
	private int shortenNameLength;

	private int shortenNameLengthVal;

	private void Start()
	{
		if (this.isUniqueSkill)
		{
			this.title.text = StringMaster.GetString("CharaStatus-19");
		}
		else
		{
			int num = this.successionSkillSlotId;
			if (num == 1 || num != 2)
			{
				this.title.text = StringMaster.GetString("SkillInheritTitle1");
			}
			else
			{
				this.title.text = StringMaster.GetString("SkillInheritTitle2");
			}
		}
	}

	private void SetSkillUI(string skillName, string skillInfo, string skillCost, int skillAttribute)
	{
		this.calcShortenNameLength(skillName);
		if (this.shortenName && skillName.Length > this.shortenNameLengthVal)
		{
			this.skillName.text = string.Format("{0} ...", skillName.Substring(0, this.shortenNameLengthVal));
		}
		else
		{
			this.skillName.text = skillName;
		}
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
			if (monsterData.GetUniqueSkill() != null)
			{
				this.SetSkillUI(monsterData.GetUniqueSkill().name, monsterData.GetUniqueSkill().description, monsterData.GetUniqueSkill().needPoint, monsterData.GetUniqueSkillDetail().attribute);
			}
		}
		else
		{
			int num = this.successionSkillSlotId;
			if (num == 1 || num != 2)
			{
				if (monsterData.GetCommonSkill() != null)
				{
					this.SetSkillUI(monsterData.GetCommonSkill().name, monsterData.GetCommonSkill().description, monsterData.GetCommonSkill().needPoint, monsterData.GetCommonSkillDetail().attribute);
				}
			}
			else if (monsterData.GetExtraCommonSkill() != null)
			{
				this.SetSkillUI(monsterData.GetExtraCommonSkill().name, monsterData.GetExtraCommonSkill().description, monsterData.GetExtraCommonSkill().needPoint, monsterData.GetExtraCommonSkillDetail().attribute);
			}
			else
			{
				this.ClearSkill();
			}
		}
	}

	public void SetCommonSkill(MonsterData monsterData)
	{
		this.successionSkillSlotId = 1;
		this.title.text = StringMaster.GetString("SkillInheritTitle1");
		if (monsterData != null && monsterData.GetCommonSkill() != null)
		{
			this.SetSkillUI(monsterData.GetCommonSkill().name, monsterData.GetCommonSkill().description, monsterData.GetCommonSkill().needPoint, monsterData.GetCommonSkillDetail().attribute);
		}
		else
		{
			this.ClearSkill();
		}
	}

	public void SetCommonSkill2(MonsterData monsterData)
	{
		this.successionSkillSlotId = 2;
		this.title.text = StringMaster.GetString("SkillInheritTitle2");
		if (monsterData != null && monsterData.GetExtraCommonSkill() != null)
		{
			this.SetSkillUI(monsterData.GetExtraCommonSkill().name, monsterData.GetExtraCommonSkill().description, monsterData.GetExtraCommonSkill().needPoint, monsterData.GetExtraCommonSkillDetail().attribute);
		}
		else
		{
			this.ClearSkill();
		}
	}

	private void calcShortenNameLength(string text)
	{
		if (CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN) == 2.ToString())
		{
			this.shortenNameLengthVal = this.shortenNameLength * 2;
			int num = 0;
			float num2 = 0f;
			for (int i = 0; i < text.Length; i++)
			{
				if (char.IsUpper(text[i]))
				{
					num2 += 1.3f;
				}
				else
				{
					num2 += 1f;
				}
				num++;
				if ((int)Math.Ceiling((double)num2) >= this.shortenNameLengthVal)
				{
					this.shortenNameLengthVal = num;
					break;
				}
			}
		}
		else
		{
			this.shortenNameLengthVal = this.shortenNameLength;
		}
	}
}
