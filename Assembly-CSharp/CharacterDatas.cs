using System;
using UnityEngine;

[Serializable]
public class CharacterDatas
{
	private static readonly int[] evolutionStepValue = new int[]
	{
		90,
		150,
		210,
		300,
		150,
		300
	};

	[SerializeField]
	private string _name;

	[SerializeField]
	private Species _species;

	[SerializeField]
	private EvolutionStep _evolutionStep;

	public CharacterDatas()
	{
		this._name = string.Empty;
		this._species = Species.Null;
		this._evolutionStep = EvolutionStep.InfancyPhase1;
	}

	public CharacterDatas(string name, Species species, EvolutionStep evolutionStep, string monsterStatusId)
	{
		this._name = name;
		this._species = species;
		this._evolutionStep = evolutionStep;
		this.monsterStatusId = monsterStatusId;
	}

	public string name
	{
		get
		{
			return this._name;
		}
	}

	public Species species
	{
		get
		{
			return this._species;
		}
	}

	public EvolutionStep evolutionStep
	{
		get
		{
			return this._evolutionStep;
		}
	}

	public string monsterStatusId { get; private set; }

	public int GetMaxFriendshipLevel()
	{
		switch (this.evolutionStep)
		{
		case EvolutionStep.GrowthPhase:
			return CharacterDatas.evolutionStepValue[0];
		case EvolutionStep.MaturationPhase:
			return CharacterDatas.evolutionStepValue[1];
		case EvolutionStep.PerfectPhase:
			return CharacterDatas.evolutionStepValue[2];
		case EvolutionStep.UltimatePhase:
			return CharacterDatas.evolutionStepValue[3];
		case EvolutionStep.AmorPhase1:
			return CharacterDatas.evolutionStepValue[4];
		case EvolutionStep.AmorPhase2:
			return CharacterDatas.evolutionStepValue[5];
		default:
			return 0;
		}
	}
}
