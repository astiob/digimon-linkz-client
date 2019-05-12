using BattleStateMachineInternal;
using System;
using System.Collections;

public class BattleFraudCheck : BattleFunctionBase
{
	public void FraudCheckOverflowMaxHp(CharacterStateControl character)
	{
		if (character.hp >= 0 && character.hp < 10000)
		{
			return;
		}
		if (character.maxHp >= 0 && character.maxHp < 10000)
		{
			return;
		}
		this.FraudCheckOverflowMaxHpOutputLog(character);
	}

	public void FraudCheckOverflowMaxSpeed(CharacterStateControl character)
	{
		if (character.speed >= 0 || character.speed < 500 || character.randomedSpeed > (float)character.speed)
		{
			return;
		}
		this.FraudCheckOverflowMaxSpeedOutputLog(character);
	}

	public void FraudCheckOverflowMaxStatus(CharacterStateControl character)
	{
		bool flag = false;
		if (character.attackPower < 0 || character.attackPower >= 10000)
		{
			flag = true;
		}
		if (character.defencePower < 0 || character.defencePower >= 10000)
		{
			flag = true;
		}
		if (character.specialAttackPower < 0 || character.specialAttackPower >= 10000)
		{
			flag = true;
		}
		if (character.specialDefencePower < 0 || character.specialDefencePower >= 10000)
		{
			flag = true;
		}
		if (character.level < 0 || character.level >= 90)
		{
			flag = true;
		}
		if (character.luck < 0 || character.luck >= 10000)
		{
			flag = true;
		}
		if (flag)
		{
			this.FraudCheckOverflowMaxStatusOutputLog(character);
		}
	}

	public void FraudCheckMaximumAttackerDamage(int damage, CharacterStateControl character)
	{
		if (damage < 40000)
		{
			return;
		}
		this.FraudCheckMaximumAttackerDamageOutputLog(damage, character);
	}

	public void FraudCheckMinimumTargetDamage(int damage, CharacterStateControl character)
	{
		if (damage > 3)
		{
			return;
		}
		this.FraudCheckMinimumTargetDamageOutputLog(damage, character);
	}

	public IEnumerator FraudCheckCallApi(FraudDataLog[] fraudDataLogs)
	{
		if (base.onServerConnect)
		{
			string errorMessage = string.Empty;
			for (int i = 0; i < fraudDataLogs.Length; i++)
			{
				errorMessage += fraudDataLogs[i].ToString();
				if (i < fraudDataLogs.Length - 1)
				{
					errorMessage += "\n\n";
				}
			}
			yield return base.StartCoroutine(GameWebAPI.Instance().SendActivityCheatLog("errorType", errorMessage, null));
			yield return true;
		}
		else
		{
			yield return true;
		}
		yield break;
	}

	private void FraudCheckOverflowMaxHpOutputLog(CharacterStateControl character)
	{
		if (base.onServerConnect)
		{
			FraudDataLog fraudDataLog = new FraudDataLog(FraudDataType.OverflowMaxHp, character);
			string text = "[HPの値が不正です]";
			text = text + "UID: " + DataMng.Instance().RespDataCM_Login.playerInfo.userId + "\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"HP: ",
				character.hp,
				"\n"
			});
			text = text + "MAX HP: " + character.maxHp;
			fraudDataLog.dataLog = text;
			base.battleStateData.fraudDataLogs.Add(fraudDataLog);
		}
		else
		{
			base.battleStateData.fraudDataLogs.Add(new FraudDataLog(FraudDataType.OverflowMaxHp, character));
			Debug.Log(string.Concat(new object[]
			{
				"Overflow Max Hp: ",
				character.myIndex,
				" / ",
				character.name
			}));
		}
	}

	private void FraudCheckOverflowMaxSpeedOutputLog(CharacterStateControl character)
	{
		if (base.onServerConnect)
		{
			FraudDataLog fraudDataLog = new FraudDataLog(FraudDataType.OverflowMaxSpeed, character);
			string text = "[SPDの値が不正です]";
			text = text + "UID: " + DataMng.Instance().RespDataCM_Login.playerInfo.userId + "\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"SPD: ",
				character.speed,
				"\n"
			});
			text = text + "RND SPD: " + character.randomedSpeed;
			fraudDataLog.dataLog = text;
			base.battleStateData.fraudDataLogs.Add(fraudDataLog);
		}
		else
		{
			base.battleStateData.fraudDataLogs.Add(new FraudDataLog(FraudDataType.OverflowMaxSpeed, character));
			Debug.Log(string.Concat(new object[]
			{
				"Overflow Max Speed: ",
				character.myIndex,
				" / ",
				character.name
			}));
		}
	}

	private void FraudCheckOverflowMaxStatusOutputLog(CharacterStateControl character)
	{
		if (base.onServerConnect)
		{
			FraudDataLog fraudDataLog = new FraudDataLog(FraudDataType.OverflowMaxStatus, character);
			string text = "[STATUSの値が不正です]";
			text = text + "UID: " + DataMng.Instance().RespDataCM_Login.playerInfo.userId + "\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"ATK: ",
				character.attackPower,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"DEF: ",
				character.defencePower,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"S-ATK: ",
				character.specialAttackPower,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"S-DEF: ",
				character.specialDefencePower,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"LEVEL: ",
				character.level,
				"\n"
			});
			text = text + "LCK: " + character.luck;
			fraudDataLog.dataLog = text;
			base.battleStateData.fraudDataLogs.Add(fraudDataLog);
		}
		else
		{
			base.battleStateData.fraudDataLogs.Add(new FraudDataLog(FraudDataType.OverflowMaxStatus, character));
			Debug.Log(string.Concat(new object[]
			{
				"Overflow Max Status: ",
				character.myIndex,
				" / ",
				character.name
			}));
		}
	}

	private void FraudCheckMaximumAttackerDamageOutputLog(int damage, CharacterStateControl character)
	{
		if (base.onServerConnect)
		{
			FraudDataLog fraudDataLog = new FraudDataLog(FraudDataType.MaximumAttackerDamage, character, damage);
			string text = "[与ダメージの値が不正です]";
			text = text + "UID: " + DataMng.Instance().RespDataCM_Login.playerInfo.userId + "\n";
			text = text + "DMG: " + damage;
			fraudDataLog.dataLog = text;
			base.battleStateData.fraudDataLogs.Add(fraudDataLog);
		}
		else
		{
			base.battleStateData.fraudDataLogs.Add(new FraudDataLog(FraudDataType.MaximumAttackerDamage, character, damage));
			Debug.Log(string.Concat(new object[]
			{
				"Maximum Attacker Damage: ",
				damage,
				" / ",
				character.myIndex,
				" / ",
				character.name
			}));
		}
	}

	private void FraudCheckMinimumTargetDamageOutputLog(int damage, CharacterStateControl character)
	{
		if (base.onServerConnect)
		{
			FraudDataLog fraudDataLog = new FraudDataLog(FraudDataType.MinimumTargetDamage, character, damage);
			string text = "[被ダメージの値が不正です]";
			text = text + "UID: " + DataMng.Instance().RespDataCM_Login.playerInfo.userId + "\n";
			text = text + "DMG: " + damage;
			fraudDataLog.dataLog = text;
			base.battleStateData.fraudDataLogs.Add(fraudDataLog);
		}
		else
		{
			base.battleStateData.fraudDataLogs.Add(new FraudDataLog(FraudDataType.MinimumTargetDamage, character, damage));
			Debug.Log(string.Concat(new object[]
			{
				"Minimum Target Damage: ",
				damage,
				" / ",
				character.myIndex,
				" / ",
				character.name
			}));
		}
	}
}
