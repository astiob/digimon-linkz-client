using System;

public sealed class SkillResults
{
	public int originalAttackPower;

	public int attackPower;

	public bool onCriticalHit;

	public Strength onWeakHit;

	public bool onMissHit;

	public CharacterStateControl targetCharacter;
}
