using System;

public sealed class BattleDigimonEnemyStatus : BattleDigimonStatusBase
{
	private void Awake()
	{
		this.SetupLocalize();
	}

	protected override void SetupLocalize()
	{
		base.SetupLocalize();
	}

	public void ApplyEnemyDescription(bool isShow, CharacterStateControl characterStatus = null)
	{
		NGUITools.SetActiveSelf(base.gameObject, isShow);
		if (!isShow)
		{
			return;
		}
		base.SetupTolerance(characterStatus);
		this.monsterName.text = characterStatus.name;
		base.SetupEvolutionStep(characterStatus);
		base.SetupSpecies(characterStatus);
	}
}
