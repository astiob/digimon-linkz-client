using Master;
using System;
using UnityEngine;

public class BattleStartAction : MonoBehaviour
{
	[SerializeField]
	[Header("タイトル")]
	private GameObject titleRoot;

	[SerializeField]
	[Header("味方のリーダースキルのUI")]
	private BattleStartAction.LeaderSkillUI playerLeaderSkillUI;

	[Header("敵のリーダースキルのUI")]
	[SerializeField]
	private BattleStartAction.LeaderSkillUI enemyLeaderSkillUI;

	[SerializeField]
	[Header("VSの時のメッセージ")]
	public UIWidget pvpVSUi;

	public void SetActive(bool value)
	{
		if (!value)
		{
			this.playerLeaderSkillUI.leaderSkillLocalize.text = string.Empty;
			this.playerLeaderSkillUI.leaderSkillNamePlayer.text = string.Empty;
			this.playerLeaderSkillUI.leaderSkillUIPlayer.SetActive(false);
			this.enemyLeaderSkillUI.leaderSkillLocalize.text = string.Empty;
			this.enemyLeaderSkillUI.leaderSkillNamePlayer.text = string.Empty;
			this.enemyLeaderSkillUI.leaderSkillUIPlayer.SetActive(false);
		}
		base.gameObject.SetActive(value);
	}

	public void ApplyBattleStartActionTitle(bool value)
	{
		this.titleRoot.SetActive(value);
	}

	public void ApplyPlayerLeaderSkill(bool isHavingLeaderSkill, string leaderSkillName, bool isChange = false)
	{
		this.ApplyLeaderSkill(this.playerLeaderSkillUI, isHavingLeaderSkill, leaderSkillName, isChange);
	}

	public void ApplyEnemyLeaderSkill(bool isHavingLeaderSkill, string leaderSkillName, bool isChange = false)
	{
		this.ApplyLeaderSkill(this.enemyLeaderSkillUI, isHavingLeaderSkill, leaderSkillName, isChange);
	}

	private void ApplyLeaderSkill(BattleStartAction.LeaderSkillUI leaderSkillUI, bool isHavingLeaderSkill, string leaderSkillName, bool isChange = false)
	{
		if (isChange)
		{
			leaderSkillUI.leaderSkillLocalize.text = StringMaster.GetString("BattleNotice-17");
		}
		else
		{
			leaderSkillUI.leaderSkillLocalize.text = StringMaster.GetString("BattleNotice-05");
		}
		if (string.IsNullOrEmpty(leaderSkillName))
		{
			leaderSkillUI.leaderSkillNamePlayer.text = StringMaster.GetString("BattleNotice-18");
		}
		else
		{
			leaderSkillUI.leaderSkillNamePlayer.text = leaderSkillName;
		}
		leaderSkillUI.leaderSkillLocalize.gameObject.SetActive(isHavingLeaderSkill);
		leaderSkillUI.leaderSkillNamePlayer.gameObject.SetActive(isHavingLeaderSkill);
		leaderSkillUI.leaderSkillUIPlayer.SetActive(isHavingLeaderSkill);
	}

	public void ApplyVSUI(bool value)
	{
		if (value)
		{
			this.pvpVSUi.gameObject.SetActive(value);
			SoundPlayer.PlayBattleVSSE();
		}
		else
		{
			this.pvpVSUi.gameObject.SetActive(value);
		}
	}

	[Serializable]
	private struct LeaderSkillUI
	{
		[SerializeField]
		[Header("味方スキル発動ローカライズ")]
		public UILabel leaderSkillLocalize;

		[SerializeField]
		[Header("リーダースキルのオブジェクト")]
		public GameObject leaderSkillUIPlayer;

		[SerializeField]
		[Header("リーダースキルの名前")]
		public UILabel leaderSkillNamePlayer;
	}
}
