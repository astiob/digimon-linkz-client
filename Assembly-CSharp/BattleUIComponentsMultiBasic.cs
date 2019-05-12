using System;
using UnityEngine;

public abstract class BattleUIComponentsMultiBasic : BattleUIComponents
{
	[SerializeField]
	private AttackTime _attackTime;

	public AttackTime attackTime
	{
		get
		{
			return this._attackTime;
		}
	}
}
