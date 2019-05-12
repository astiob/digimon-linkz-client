using System;
using UI.Common;
using UnityEngine;

public sealed class MonsterEggMedalList : MonoBehaviour
{
	[SerializeField]
	private MonsterEggMedalList.IconGroup hpIcon;

	[SerializeField]
	private MonsterEggMedalList.IconGroup attackIcon;

	[SerializeField]
	private MonsterEggMedalList.IconGroup defenseIcon;

	[SerializeField]
	private MonsterEggMedalList.IconGroup magicAttackIcon;

	[SerializeField]
	private MonsterEggMedalList.IconGroup magicDefenseIcon;

	[SerializeField]
	private MonsterEggMedalList.IconGroup speedIcon;

	public void SetActive(bool isActive)
	{
		this.hpIcon.SetActive(isActive);
		this.attackIcon.SetActive(isActive);
		this.defenseIcon.SetActive(isActive);
		this.magicAttackIcon.SetActive(isActive);
		this.magicDefenseIcon.SetActive(isActive);
		this.speedIcon.SetActive(isActive);
	}

	public void SetValues(MonsterEggStatusInfo statusInfo)
	{
		this.SetCandidateMedal(statusInfo.hpAbilityFlg, this.hpIcon);
		this.SetCandidateMedal(statusInfo.attackAbilityFlg, this.attackIcon);
		this.SetCandidateMedal(statusInfo.defenseAbilityFlg, this.defenseIcon);
		this.SetCandidateMedal(statusInfo.spAttackAbilityFlg, this.magicAttackIcon);
		this.SetCandidateMedal(statusInfo.spDefenseAbilityFlg, this.magicDefenseIcon);
		this.SetCandidateMedal(statusInfo.speedAbilityFlg, this.speedIcon);
	}

	private void SetCandidateMedal(ConstValue.CandidateMedal candidate, MonsterEggMedalList.IconGroup icons)
	{
		string medalType = 1.ToString();
		string medalType2 = 2.ToString();
		switch (candidate)
		{
		case ConstValue.CandidateMedal.NONE:
			break;
		case ConstValue.CandidateMedal.GOLD:
			icons.SetActiveIcon(false, true);
			icons.right.spriteName = MonsterMedalIcon.GetMedalSpriteName(medalType, string.Empty);
			return;
		case ConstValue.CandidateMedal.SILVER:
			icons.SetActiveIcon(false, true);
			icons.right.spriteName = MonsterMedalIcon.GetMedalSpriteName(medalType2, string.Empty);
			return;
		default:
			switch (candidate)
			{
			case ConstValue.CandidateMedal.SILVER_OR_NONE:
				icons.SetActiveIcon(true, false);
				icons.left.spriteName = MonsterMedalIcon.GetMedalSpriteName(medalType2, string.Empty);
				return;
			case ConstValue.CandidateMedal.GOLD_OR_NONE:
				icons.SetActiveIcon(true, false);
				icons.left.spriteName = MonsterMedalIcon.GetMedalSpriteName(medalType, string.Empty);
				return;
			case ConstValue.CandidateMedal.GOLD_OR_SILVER:
				icons.SetActive(true);
				icons.left.spriteName = MonsterMedalIcon.GetMedalSpriteName(medalType, string.Empty);
				icons.right.spriteName = MonsterMedalIcon.GetMedalSpriteName(medalType2, string.Empty);
				return;
			}
			break;
		}
		icons.SetActive(false);
	}

	[Serializable]
	private sealed class IconGroup
	{
		[SerializeField]
		public UISprite left;

		[SerializeField]
		public UISprite right;

		[SerializeField]
		public UILabel label;

		public void SetActive(bool isActive)
		{
			if (this.left.gameObject.activeSelf != isActive)
			{
				this.left.gameObject.SetActive(isActive);
			}
			if (this.right.gameObject.activeSelf != isActive)
			{
				this.right.gameObject.SetActive(isActive);
			}
			if (this.label.gameObject.activeSelf != isActive)
			{
				this.label.gameObject.SetActive(isActive);
			}
		}

		public void SetActiveIcon(bool isActiveLeftIcon, bool isActiveRightIcon)
		{
			if (this.left.gameObject.activeSelf != isActiveLeftIcon)
			{
				this.left.gameObject.SetActive(isActiveLeftIcon);
			}
			if (this.label.gameObject.activeSelf != isActiveLeftIcon)
			{
				this.label.gameObject.SetActive(isActiveLeftIcon);
			}
			if (this.right.gameObject.activeSelf != isActiveRightIcon)
			{
				this.right.gameObject.SetActive(isActiveRightIcon);
			}
		}
	}
}
