using Master;
using System;
using UnityEngine;

public sealed class MonsterLeaderSkill : MonoBehaviour
{
	[SerializeField]
	private UILabel title;

	[SerializeField]
	private UILabel skillName;

	[SerializeField]
	private UILabel skillInfo;

	private void Start()
	{
		this.title.text = StringMaster.GetString("CharaStatus-21");
	}

	public void ClearSkill()
	{
		this.skillName.text = StringMaster.GetString("SystemNone");
		this.skillInfo.text = StringMaster.GetString("CharaStatus-01");
	}

	public void SetSkill(MonsterData monsterData)
	{
		if (monsterData.leaderSkillM != null)
		{
			this.skillName.text = monsterData.leaderSkillM.name;
			this.skillInfo.text = monsterData.leaderSkillM.description;
		}
		else
		{
			this.ClearSkill();
		}
	}
}
