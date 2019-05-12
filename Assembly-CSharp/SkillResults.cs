using System;

public sealed class SkillResults
{
	public AffectEffectProperty useAffectEffectProperty;

	public CharacterStateControl attackCharacter;

	public CharacterStateControl targetCharacter;

	public AffectEffect hitIconAffectEffect;

	public int originalAttackPower;

	public int attackPower;

	public bool onCriticalHit;

	public Strength onWeakHit;

	public bool onMissHit;

	public ExtraEffectType extraEffectType;
}
