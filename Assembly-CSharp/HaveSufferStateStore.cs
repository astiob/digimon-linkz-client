using System;

public class HaveSufferStateStore
{
	public string onPoison;

	public string onConfusion;

	public string onParalysis;

	public string onSleep;

	public string onStun;

	public string onSkillLock;

	public string onAttackUp;

	public string onAttackDown;

	public string onDefenceUp;

	public string onDefenceDown;

	public string onSpAttackUp;

	public string onSpAttackDown;

	public string onSpDefenceUp;

	public string onSpDefenceDown;

	public string onSpeedUp;

	public string onSpeedDown;

	public string onCounter;

	public string onReflection;

	public string onProtect;

	public string onPowerCharge;

	public string onHitRateUp;

	public string onHitRateDown;

	public string onSatisfactionRateUp;

	public string onSatisfactionRateDown;

	public string onTurnBarrier;

	public string onCountBarrier;

	public string onDamageRateUp;

	public string onDamageRateDown;

	public int[] sufferOrderList;

	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"[HaveSufferStateStore] ",
			this.onPoison,
			"\n",
			this.onConfusion,
			"\n",
			this.onParalysis,
			"\n",
			this.onSleep,
			"\n",
			this.onStun,
			"\n",
			this.onSkillLock,
			"\n",
			this.onAttackUp,
			"\n",
			this.onAttackDown,
			"\n",
			this.onDefenceUp,
			"\n",
			this.onDefenceDown,
			"\n",
			this.onSpAttackUp,
			"\n",
			this.onSpAttackDown,
			"\n",
			this.onSpDefenceUp,
			"\n",
			this.onSpDefenceDown,
			"\n",
			this.onSpeedUp,
			"\n",
			this.onSpeedDown,
			"\n",
			this.onCounter,
			"\n",
			this.onReflection,
			"\n",
			this.onProtect,
			"\n",
			this.onPowerCharge,
			"\n",
			this.onHitRateUp,
			"\n",
			this.onHitRateDown,
			"\n",
			this.onSatisfactionRateUp,
			"\n",
			this.onSatisfactionRateDown,
			"\n",
			this.onTurnBarrier,
			"\n",
			this.onCountBarrier,
			"\n",
			this.onDamageRateUp,
			"\n",
			this.onDamageRateDown
		});
	}
}
