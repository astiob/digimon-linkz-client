using System;
using UnityEngine;

public class BattleSystem : BattleFunctionBase
{
	public static bool useMemoryCheck = true;

	private bool previousFullMemory;

	public bool IfFullMemoryCallGC()
	{
		if (!BattleSystem.useMemoryCheck)
		{
			return false;
		}
		float num = Profiler.usedHeapSize;
		float num2 = Profiler.GetTotalReservedMemory();
		float num3 = num / num2;
		if (num3 > 0.95f)
		{
			if (this.previousFullMemory)
			{
				global::Debug.LogWarning("メモリが足りないため, ロードを続行できません.");
			}
			this.previousFullMemory = true;
			Resources.UnloadUnusedAssets();
			GC.Collect();
			return true;
		}
		this.previousFullMemory = false;
		return false;
	}
}
