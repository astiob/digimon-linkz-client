using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	public class BattleActionProperties : ScriptableObject
	{
		[SerializeField]
		private BattleStateProperty properties;

		[SerializeField]
		private BattleStateUIProperty uiProperties;

		private static string GetPath(BattleMode battleMode)
		{
			if (battleMode != BattleMode.PvP)
			{
				return "BattleActionProperties";
			}
			return "BattleActionPropertiesPvP";
		}

		public static BattleStateProperty GetProperties(BattleMode battleMode)
		{
			return Resources.Load<BattleActionProperties>(BattleActionProperties.GetPath(battleMode)).properties;
		}

		public static BattleStateUIProperty GetUIProperties(BattleMode battleMode)
		{
			return Resources.Load<BattleActionProperties>(BattleActionProperties.GetPath(battleMode)).uiProperties;
		}
	}
}
