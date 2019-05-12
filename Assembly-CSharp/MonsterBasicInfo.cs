using Master;
using Monster;
using System;
using UnityEngine;

public class MonsterBasicInfo : MonoBehaviour
{
	[SerializeField]
	protected UILabel monsterName;

	[SerializeField]
	protected MonsterBasicInfo.ArousalUI arousalUI;

	[SerializeField]
	protected UILabel specificTypeName;

	[SerializeField]
	protected UILabel growName;

	[SerializeField]
	protected UILabel tribeName;

	[SerializeField]
	protected MonsterBasicInfo.LevelUI levelUI;

	private void Start()
	{
		this.SetLabelText();
	}

	private void SetNoneArousal()
	{
		this.arousalUI.count.gameObject.SetActive(false);
		this.arousalUI.nothing.text = StringMaster.GetString("CharaStatus-01");
	}

	private void SetArousal(int arousalCount)
	{
		this.arousalUI.title.text = MonsterArousalData.GetTitle(arousalCount.ToString());
		if (2 <= arousalCount)
		{
			this.arousalUI.nothing.text = string.Empty;
			this.arousalUI.count.gameObject.SetActive(true);
			this.arousalUI.count.spriteName = MonsterArousalData.GetSpriteName(arousalCount.ToString());
		}
		else
		{
			this.SetNoneArousal();
		}
	}

	private void SetLevel(MonsterData monsterData)
	{
		if (!this.levelUI.disable)
		{
			if (this.levelUI.existLevelMax)
			{
				this.levelUI.level.text = string.Format(StringMaster.GetString("SystemFraction"), monsterData.userMonster.level, monsterData.monsterM.maxLevel);
			}
			else
			{
				this.levelUI.level.text = monsterData.userMonster.level;
			}
		}
	}

	protected virtual void SetLabelText()
	{
		if (!this.levelUI.disable)
		{
			this.levelUI.title.text = StringMaster.GetString("CharaStatus-11");
		}
	}

	public void SetMonsterData(MonsterData monsterData)
	{
		if (null != this.monsterName)
		{
			this.monsterName.text = monsterData.monsterMG.monsterName;
		}
		this.growName.text = MonsterGrowStepData.GetGrowStepName(monsterData.monsterMG.growStep);
		this.tribeName.text = MonsterTribeData.GetTribeName(monsterData.monsterMG.tribe);
		if (this.arousalUI.exist)
		{
			this.SetArousal(monsterData.monsterM.rare.ToInt32());
		}
		this.SetLevel(monsterData);
		this.specificTypeName.text = MonsterSpecificTypeData.GetSpecificTypeName(monsterData.monsterMG.monsterStatusId);
	}

	public void SetEggData(string eggName, string rare)
	{
		this.monsterName.text = eggName;
		this.growName.text = StringMaster.GetString("CharaStatus-04");
		this.tribeName.text = StringMaster.GetString("CharaStatus-01");
		if (this.arousalUI.exist)
		{
			this.SetArousal(rare.ToInt32());
		}
		if (!this.levelUI.disable)
		{
			this.levelUI.level.text = StringMaster.GetString("CharaStatus-01");
		}
		this.specificTypeName.text = StringMaster.GetString("CharaStatus-01");
	}

	public virtual void ClearMonsterData()
	{
		if (null != this.monsterName)
		{
			this.monsterName.text = string.Empty;
		}
		this.growName.text = string.Empty;
		this.tribeName.text = string.Empty;
		if (this.arousalUI.exist)
		{
			this.SetNoneArousal();
		}
		if (!this.levelUI.disable)
		{
			this.levelUI.level.text = string.Empty;
		}
		this.specificTypeName.text = string.Empty;
	}

	[Serializable]
	protected sealed class ArousalUI
	{
		public bool exist;

		public UILabel title;

		public UISprite count;

		public UILabel nothing;
	}

	[Serializable]
	protected sealed class LevelUI
	{
		public bool disable;

		public bool existLevelMax;

		public UILabel title;

		public UILabel level;
	}
}
