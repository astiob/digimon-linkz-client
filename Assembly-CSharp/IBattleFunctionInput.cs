using System;

public interface IBattleFunctionInput
{
	void BattleAwakeInitialize();

	void BattleTriggerInitialize();

	void BattleEndBefore();
}
