using System;
using UnityEngine;

namespace MonsterIcon
{
	public static class MonsterIconGrayout
	{
		private static Color nonActiveColor = new Color(0.6f, 0.6f, 0.6f, 1f);

		private static Color disableColor = new Color(0.27451f, 0.27451f, 0.27451f, 1f);

		public static void SetGrayout(GameObject icon, GUIMonsterIcon.DIMM_LEVEL type)
		{
			switch (type)
			{
			default:
				GUIManager.SetColorAll(icon.transform, Color.white);
				break;
			case GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE:
				GUIManager.SetColorAll(icon.transform, MonsterIconGrayout.nonActiveColor);
				break;
			case GUIMonsterIcon.DIMM_LEVEL.DISABLE:
				GUIManager.SetColorAll(icon.transform, MonsterIconGrayout.disableColor);
				break;
			}
		}
	}
}
