using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	public class BattleObjectPoolerManager : MonoBehaviour
	{
		private const float checkInterval = 10f;

		private const float maxMemoryPercent = 0.95f;

		public bool CheckEnable { get; set; }

		private void Start()
		{
			base.InvokeRepeating("CheckHeapAndCallRemove", 0f, 10f);
		}

		private void CheckHeapAndCallRemove()
		{
			if (!this.CheckEnable)
			{
				return;
			}
			float num = Profiler.usedHeapSize;
			float num2 = Profiler.GetTotalReservedMemory();
			float num3 = num / num2;
			if (num3 > 0.95f)
			{
				BattleObjectPooler.AllUnloadAssets();
			}
		}
	}
}
