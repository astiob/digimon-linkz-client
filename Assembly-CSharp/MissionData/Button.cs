using Master;
using System;
using UnityEngine;

namespace MissionData
{
	public static class Button
	{
		public static string[] spriteNames = new string[]
		{
			"Common02_Btn_BaseG",
			"Common02_Btn_BaseON1",
			string.Empty,
			"Common02_Btn_BaseON",
			string.Empty
		};

		public static Color[] colors = new Color[]
		{
			new Color32(100, 100, 100, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color32(byte.MaxValue, 240, 0, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		};

		public static string[] texts = new string[]
		{
			StringMaster.GetString("Mission-02"),
			StringMaster.GetString("Mission-03"),
			StringMaster.GetString("Mission-04"),
			string.Empty
		};

		public static string[] categoryTexts = new string[]
		{
			StringMaster.GetString("QuestNormal"),
			StringMaster.GetString("Mission-01"),
			StringMaster.GetString("ReinforcementTitle"),
			string.Empty
		};
	}
}
