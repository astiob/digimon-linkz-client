using System;
using UnityEngine;

namespace MissionData
{
	public static class Label
	{
		public static readonly Color NOT_COMPLETE_COLOR = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color COMPLETE_COLOR = new Color32(byte.MaxValue, 240, 0, byte.MaxValue);
	}
}
