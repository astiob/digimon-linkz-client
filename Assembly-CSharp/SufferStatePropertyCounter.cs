using System;
using System.Collections.Generic;

public class SufferStatePropertyCounter
{
	private Dictionary<SufferStateProperty.SufferType, List<CharacterStateControl>> countDictionary = new Dictionary<SufferStateProperty.SufferType, List<CharacterStateControl>>();

	public void AddCountDictionary(SufferStateProperty.SufferType key, CharacterStateControl value)
	{
		if (!this.countDictionary.ContainsKey(key))
		{
			this.countDictionary.Add(key, new List<CharacterStateControl>());
		}
		HaveSufferState currentSufferState = value.currentSufferState;
		if (currentSufferState.FindSufferState(key))
		{
			SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
			if (sufferStateProperty.isMultiHitThrough)
			{
				sufferStateProperty.AddCurrentKeepCount(-1);
			}
			else if (!this.countDictionary[key].Contains(value))
			{
				this.countDictionary[key].Add(value);
			}
		}
	}

	public void UpdateCount(SufferStateProperty.SufferType key)
	{
		List<CharacterStateControl> list = null;
		this.countDictionary.TryGetValue(key, out list);
		if (list == null || list.Count == 0)
		{
			return;
		}
		foreach (CharacterStateControl characterStateControl in list)
		{
			HaveSufferState currentSufferState = characterStateControl.currentSufferState;
			if (currentSufferState.FindSufferState(key))
			{
				SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
				if (!sufferStateProperty.isMultiHitThrough)
				{
					sufferStateProperty.AddCurrentKeepCount(-1);
				}
			}
		}
	}

	public void Clear()
	{
		this.countDictionary.Clear();
	}
}
