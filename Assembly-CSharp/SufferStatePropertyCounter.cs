using System;
using System.Collections.Generic;
using System.Linq;

public class SufferStatePropertyCounter
{
	private Dictionary<SufferStateProperty.SufferType, List<SufferStatePropertyCounter.CountData>> countDictionary = new Dictionary<SufferStateProperty.SufferType, List<SufferStatePropertyCounter.CountData>>();

	public void AddCountDictionary(SufferStateProperty.SufferType key, CharacterStateControl value, string[] ids = null)
	{
		if (!this.countDictionary.ContainsKey(key))
		{
			this.countDictionary.Add(key, new List<SufferStatePropertyCounter.CountData>());
		}
		HaveSufferState currentSufferState = value.currentSufferState;
		if (currentSufferState.FindSufferState(key))
		{
			SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
			SufferStateProperty.Data[] array = sufferStateProperty.GetIsMultiHitThroughDatas();
			if (ids != null)
			{
				array = array.Where((SufferStateProperty.Data item) => ids.Contains(item.id)).ToArray<SufferStateProperty.Data>();
			}
			if (array.Length > 0)
			{
				sufferStateProperty.AddCurrentKeepCount(array, -1);
			}
			SufferStateProperty.Data[] array2 = sufferStateProperty.GetNotIsMultiHitThroughDatas();
			if (ids != null)
			{
				array2 = array2.Where((SufferStateProperty.Data item) => ids.Contains(item.id)).ToArray<SufferStateProperty.Data>();
			}
			if (array2.Length > 0)
			{
				foreach (SufferStateProperty.Data notIsMultiHitThroughData in array2)
				{
					if (!this.countDictionary[key].Where((SufferStatePropertyCounter.CountData item) => item.id == notIsMultiHitThroughData.id).Any<SufferStatePropertyCounter.CountData>())
					{
						SufferStatePropertyCounter.CountData countData = new SufferStatePropertyCounter.CountData();
						countData.characterStateControl = value;
						countData.id = notIsMultiHitThroughData.id;
						this.countDictionary[key].Add(countData);
					}
				}
			}
		}
	}

	public void UpdateCount(SufferStateProperty.SufferType key)
	{
		List<SufferStatePropertyCounter.CountData> list = null;
		this.countDictionary.TryGetValue(key, out list);
		if (list == null || list.Count == 0)
		{
			return;
		}
		SufferStatePropertyCounter.CountData countData;
		foreach (SufferStatePropertyCounter.CountData countData2 in list)
		{
			countData = countData2;
			HaveSufferState currentSufferState = countData.characterStateControl.currentSufferState;
			if (currentSufferState.FindSufferState(key))
			{
				SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
				SufferStateProperty.Data[] array = sufferStateProperty.GetNotIsMultiHitThroughDatas();
				if (!string.IsNullOrEmpty(countData.id))
				{
					array = array.Where((SufferStateProperty.Data item) => countData.id == item.id).ToArray<SufferStateProperty.Data>();
				}
				if (array.Length > 0)
				{
					sufferStateProperty.AddCurrentKeepCount(array, -1);
				}
			}
		}
	}

	public void Clear()
	{
		this.countDictionary.Clear();
	}

	private class CountData
	{
		public CharacterStateControl characterStateControl;

		public string id = string.Empty;
	}
}
