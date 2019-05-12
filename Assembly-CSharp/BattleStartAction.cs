using Master;
using System;
using UnityEngine;

public class BattleStartAction : MonoBehaviour
{
	[Header("味方スキル発動ローカライズ")]
	[SerializeField]
	private UILabel leaderSkillLocalize;

	[SerializeField]
	[Header("敵スキル発動ローカライズ(PvPのみ)")]
	private UILabel leaderSkillEnemyLocalize;

	[SerializeField]
	[Header("リーダースキルのオブジェクト")]
	private GameObject leaderSkillUIPlayer;

	[SerializeField]
	[Header("敵のリーダースキルのオブジェクト(PvPのみ)")]
	private GameObject leaderSkillUIEnemy;

	[Header("リーダースキルの名前")]
	[SerializeField]
	private UILabel leaderSkillNamePlayer;

	[Header("敵のリーダースキルの名前(PvPのみ)")]
	[SerializeField]
	private UILabel leaderSkillNameEnemy;

	public void ApplyBattleStartActionText(bool isHavingLeaderSkill, string leaderSkillName = "")
	{
		this.leaderSkillLocalize.text = StringMaster.GetString("BattleNotice-05");
		this.leaderSkillNamePlayer.text = leaderSkillName;
		this.leaderSkillUIPlayer.SetActive(isHavingLeaderSkill);
	}

	public void ApplyBattleStartActionText(bool isHavingLeaderSkill, string leaderSkillName, bool isHavingEnemyLeaderSkill, string enemyLeaderSkillName)
	{
		this.leaderSkillLocalize.text = StringMaster.GetString("BattleNotice-05");
		this.leaderSkillEnemyLocalize.text = StringMaster.GetString("BattleNotice-05");
		this.leaderSkillNamePlayer.text = leaderSkillName;
		this.leaderSkillUIPlayer.SetActive(isHavingLeaderSkill);
		this.leaderSkillNameEnemy.text = enemyLeaderSkillName;
		this.leaderSkillUIEnemy.SetActive(isHavingEnemyLeaderSkill);
	}
}
