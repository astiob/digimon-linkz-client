using Monster;
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
			if (MonsterStatusData.IsVersionUp(monsterData.GetMonsterMaster().Simple.rare))
			{
				if (this.grayNA.activeSelf)
				{
					this.grayNA.SetActive(false);
				}
				if (monsterData.commonSkillM2 == null)
				{
					if (!this.grayReady.activeSelf)
					{
						this.grayReady.SetActive(true);
					}
					if (this.available.activeSelf)
					{
						this.available.SetActive(false);
					}
				}
				else
				{
					if (!this.available.activeSelf)
					{
						this.available.SetActive(true);
					}
					if (this.grayReady.activeSelf)
					{
						this.grayReady.SetActive(false);
					}
				}
			}
			else
			{
				this.ClearData();
			}
		}

		public void ClearData()
		{
			if (!this.grayNA.activeSelf)
			{
				this.grayNA.SetActive(true);
			}
			if (this.grayReady.activeSelf)
			{
				this.grayReady.SetActive(false);
			}
			if (this.available.activeSelf)
			{
				this.available.SetActive(false);
			}
		}
	}
}
