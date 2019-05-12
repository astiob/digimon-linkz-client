using System;
using UnityEngine;

namespace CharacterMiniStatusUI
{
	public sealed class UI_ExtraCommonSkillParts : MonoBehaviour
	{
		[SerializeField]
		private GameObject available;

		[SerializeField]
		private GameObject grayReady;

		[SerializeField]
		private GameObject grayNA;

		public void SetMonsterData(MonsterData monsterData)
		{
			if (monsterData.IsVersionUp())
			{
				if (monsterData.commonSkillM2 == null)
				{
					this.grayReady.SetActive(true);
				}
				else
				{
					this.available.SetActive(true);
				}
			}
			else
			{
				this.grayNA.SetActive(true);
			}
		}
	}
}
