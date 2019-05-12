using System;
using UnityEngine;

public class BattleSimulatorManager : MonoBehaviour
{
	[SerializeField]
	private BattleSimulator singleBattleSimulator;

	[SerializeField]
	private PvPBattleSimulator multiBattleSimulator;

	[SerializeField]
	private MultiBattleSimulator pvpBattleSimulator;

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
