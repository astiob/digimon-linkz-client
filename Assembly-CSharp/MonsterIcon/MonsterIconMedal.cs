using Master;
using System;
using UnityEngine;

namespace MonsterIcon
{
	[Serializable]
	public sealed class MonsterIconMedal
	{
		[SerializeField]
		private UISprite medal;

		[SerializeField]
		private UILabel num;

		public void SetGoldMedal()
		{
			this.medal.spriteName = "Common02_Talent_Gold";
		}

		public void SetSilverMedal()
		{
			this.medal.spriteName = "Common02_Talent_Silver";
		}

		public void SetMedalNum(int medalNum)
		{
			this.num.text = string.Format("{0}{1}", StringMaster.GetString("MissionRewardKakeru"), medalNum);
		}

		public void Show()
		{
			this.medal.enabled = true;
			this.num.enabled = true;
		}

		public void Clear()
		{
			this.medal.enabled = false;
			this.num.enabled = false;
		}
	}
}
