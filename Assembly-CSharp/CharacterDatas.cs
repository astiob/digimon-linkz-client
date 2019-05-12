using Monster;
using System;
using UnityEngine;

[Serializable]
public class CharacterDatas
{
	[SerializeField]
	private string _name;

	[SerializeField]
	private GrowStep _growStep;

	[SerializeField]
	private string _tribe;

	public CharacterDatas()
	{
		this.name = string.Empty;
		this.tribe = "0";
		this.growStep = GrowStep.NONE;
	}

	public CharacterDatas(string name, string tribe, GrowStep growStep, string monsterStatusId)
	{
		this.name = name;
		this.tribe = tribe;
		this.growStep = growStep;
		this.monsterStatusId = monsterStatusId;
	}

	public string name { get; private set; }

	public GrowStep growStep { get; private set; }

	public string tribe { get; private set; }

	public string monsterStatusId { get; private set; }
}
