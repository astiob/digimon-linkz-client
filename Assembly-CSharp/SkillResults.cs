using System;

public sealed class SkillResults
{
	public int originalAttackPower;

	public int attackPower;

	public bool onCriticalHit;

	public Strength onWeakHit;

	public bool onMissHit;

	public bool onIgnoreTarget = true;

	public int attackPowerNormal;

	public CharacterStateControl targetCharacter;
}
