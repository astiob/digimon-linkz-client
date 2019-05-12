using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class FraudDataLog
	{
		[SerializeField]
		private FraudDataType _dataType;

		[SerializeField]
		private int _characterIndex;

		[SerializeField]
		private int _damage;

		[SerializeField]
		private bool _isEnemy;

		[SerializeField]
		private CharacterStatus _characterStatus;

		public string dataLog;

		public FraudDataLog(FraudDataType fraudDataType, CharacterStateControl characterStateControl)
		{
			this._dataType = fraudDataType;
			this._characterIndex = characterStateControl.myIndex;
			this._isEnemy = characterStateControl.isEnemy;
			if (BattleStateManager.current.battleMode == BattleMode.PvP)
			{
				this._characterStatus = characterStateControl.playerStatus.ToCharacterStatus();
			}
			else
			{
				this._characterStatus = ((!characterStateControl.isEnemy) ? characterStateControl.playerStatus.ToCharacterStatus() : characterStateControl.enemyStatus.ToCharacterStatus());
			}
			this._damage = 0;
			this.dataLog = null;
		}

		public FraudDataLog(FraudDataType fraudDataType, CharacterStateControl characterStateControl, int aDamage)
		{
			this._dataType = fraudDataType;
			this._characterIndex = characterStateControl.myIndex;
			this._isEnemy = characterStateControl.isEnemy;
			if (BattleStateManager.current.battleMode == BattleMode.PvP)
			{
				this._characterStatus = characterStateControl.playerStatus.ToCharacterStatus();
			}
			else
			{
				this._characterStatus = ((!characterStateControl.isEnemy) ? characterStateControl.playerStatus.ToCharacterStatus() : characterStateControl.enemyStatus.ToCharacterStatus());
			}
			this._damage = aDamage;
			this.dataLog = null;
		}

		public FraudDataType dataType
		{
			get
			{
				return this._dataType;
			}
		}

		public int characterIndex
		{
			get
			{
				return this._characterIndex;
			}
		}

		public int damage
		{
			get
			{
				return this._damage;
			}
		}

		public bool isEnemy
		{
			get
			{
				return this._isEnemy;
			}
		}

		public CharacterStatus characterStatus
		{
			get
			{
				return this._characterStatus;
			}
		}

		public override string ToString()
		{
			if (this.dataLog != null)
			{
				return this.dataLog;
			}
			if (this._dataType == FraudDataType.MaximumAttackerDamage || this._dataType == FraudDataType.MinimumTargetDamage)
			{
				return string.Format("[FraudDataLog: dataType={0}, characterIndex={1}, damage={2}, isEnemy={3}, characterStatus={4}]", new object[]
				{
					this.dataType,
					this.characterIndex,
					this.damage,
					this.isEnemy,
					this.characterStatus
				});
			}
			return string.Format("[FraudDataLog: dataType={0}, characterIndex={1}, isEnemy={2}, characterStatus={3}]", new object[]
			{
				this.dataType,
				this.characterIndex,
				this.isEnemy,
				this.characterStatus
			});
		}
	}
}
