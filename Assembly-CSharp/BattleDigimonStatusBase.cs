using Master;
using Monster;
using System;
using UnityEngine;

public class BattleDigimonStatusBase : MonoBehaviour
{
	[SerializeField]
	protected UILabel monsterName;

	[SerializeField]
	[Header("耐性ローカライズ")]
	protected UILabel toleranceLocalize;

	[SerializeField]
	protected UILabel evolutionStep;

	[SerializeField]
	protected UILabel species;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceNone;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceRed;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceBlue;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceYellow;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceGreen;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceWhite;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceBlack;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _tolerancePoison;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceConfusion;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceParalysis;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceSleep;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceStun;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceSkillLock;

	[SerializeField]
	private BattleDigimonStatusBase.ToleranceIcon _toleranceInstantDeath;

	protected virtual void SetupLocalize()
	{
		this.toleranceLocalize.text = StringMaster.GetString("CharaStatus-22");
	}

	protected void SetupTolerance(CharacterStateControl characterStatus)
	{
		this._toleranceNone.SetTolerance(characterStatus.tolerance.none);
		this._toleranceRed.SetTolerance(characterStatus.tolerance.red);
		this._toleranceBlue.SetTolerance(characterStatus.tolerance.blue);
		this._toleranceYellow.SetTolerance(characterStatus.tolerance.yellow);
		this._toleranceGreen.SetTolerance(characterStatus.tolerance.green);
		this._toleranceWhite.SetTolerance(characterStatus.tolerance.white);
		this._toleranceBlack.SetTolerance(characterStatus.tolerance.black);
		this._tolerancePoison.SetTolerance(characterStatus.tolerance.poison);
		this._toleranceConfusion.SetTolerance(characterStatus.tolerance.confusion);
		this._toleranceParalysis.SetTolerance(characterStatus.tolerance.paralysis);
		this._toleranceSleep.SetTolerance(characterStatus.tolerance.sleep);
		this._toleranceStun.SetTolerance(characterStatus.tolerance.stun);
		this._toleranceSkillLock.SetTolerance(characterStatus.tolerance.skillLock);
		this._toleranceInstantDeath.SetTolerance(characterStatus.tolerance.instantDeath);
	}

	protected void SetupEvolutionStep(CharacterStateControl characterStatus)
	{
		if (MonsterDataMng.Instance() != null)
		{
			string growStep = MonsterGrowStepData.ToGrowStepString(characterStatus.characterDatas.growStep);
			this.evolutionStep.text = MonsterGrowStepData.GetGrowStepName(growStep);
		}
	}

	protected void SetupSpecies(CharacterStateControl characterStatus)
	{
		if (MonsterDataMng.Instance() != null)
		{
			this.species.text = MonsterTribeData.GetTribeName(characterStatus.characterDatas.tribe);
		}
	}

	[Serializable]
	private class ToleranceIcon
	{
		[SerializeField]
		private UISprite icon;

		[SerializeField]
		private UISprite invalid;

		public void SetTolerance(Strength strength)
		{
			switch (strength)
			{
			case Strength.None:
				this.icon.color = Color.gray;
				this.invalid.gameObject.SetActive(false);
				break;
			case Strength.Strong:
				this.icon.color = new Color(0.784313738f, 0f, 0f, 1f);
				this.invalid.gameObject.SetActive(false);
				break;
			case Strength.Weak:
				this.icon.color = new Color(0f, 0.5882353f, 1f, 1f);
				this.invalid.gameObject.SetActive(false);
				break;
			case Strength.Drain:
				this.icon.color = new Color(0f, 1f, 0f, 1f);
				this.invalid.gameObject.SetActive(false);
				break;
			case Strength.Invalid:
				this.icon.color = Color.gray;
				this.invalid.gameObject.SetActive(true);
				break;
			default:
				this.icon.color = Color.gray;
				this.invalid.gameObject.SetActive(false);
				break;
			}
		}
	}
}
