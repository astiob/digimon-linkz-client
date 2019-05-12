using BattleStateMachineInternal;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class BattleFunctionUtility
{
	public const string EmptyPath = "";

	public static bool IsEmptyPath(string path)
	{
		return string.IsNullOrEmpty(path);
	}

	public static List<CharacterStateControl> GetSufferCharacters(SufferStateProperty.SufferType sufferType, BattleStateData battleStateData)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		foreach (CharacterStateControl characterStateControl in battleStateData.GetTotalCharacters())
		{
			if (!characterStateControl.isDied)
			{
				if (characterStateControl.currentSufferState.FindSufferState(sufferType))
				{
					list.Add(characterStateControl);
				}
			}
		}
		return list;
	}

	public static Vector3[] GetHitIconPositions(BattleUIControlBasic uiControl, List<CharacterStateControl> characters)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < characters.Count; i++)
		{
			Vector3 characterCenterPosition2DFunction = uiControl.GetCharacterCenterPosition2DFunction(characters[i]);
			list.Add(characterCenterPosition2DFunction);
		}
		return list.ToArray();
	}

	public static float ExTimeScale { get; set; }
}
