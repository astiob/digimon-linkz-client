using Master;
using System;
using System.Collections.Generic;
using UI.Common;
using UnityEngine;

namespace UI.MedalInheritance
{
	[Serializable]
	public sealed class MonsterMedalInheritanceIcon
	{
		[SerializeField]
		private UISprite medalIcon;

		[SerializeField]
		private UILabel inheritanceRate;

		[SerializeField]
		private UIToggleAlphaTween alphaTween;

		private List<string> spriteNames = new List<string>();

		private List<string> rateList = new List<string>();

		private int switchCount;

		private void OnCompleteFadeOut()
		{
			if (0 < this.spriteNames.Count)
			{
				this.switchCount++;
				if (this.spriteNames.Count <= this.switchCount)
				{
					this.switchCount = 0;
				}
				this.medalIcon.spriteName = this.spriteNames[this.switchCount];
				this.inheritanceRate.text = this.rateList[this.switchCount];
			}
		}

		public void Initialize()
		{
			this.alphaTween.SetActionCompleteFadeOut(new Action(this.OnCompleteFadeOut));
		}

		public void ClearMedal()
		{
			this.spriteNames.Clear();
			this.rateList.Clear();
			this.switchCount = 0;
			this.alphaTween.StopFade();
		}

		public void SetActive(bool active)
		{
			this.medalIcon.enabled = active;
			if (!active)
			{
				this.inheritanceRate.text = StringMaster.GetString("SystemNoneHyphen");
			}
			else
			{
				this.alphaTween.SetAlpha(1f);
			}
		}

		public void SetFirstView(string spriteName, string rate)
		{
			this.spriteNames.Clear();
			this.rateList.Clear();
			this.switchCount = 0;
			this.spriteNames.Add(spriteName);
			this.rateList.Add(rate);
			this.medalIcon.spriteName = spriteName;
			this.inheritanceRate.text = rate;
		}

		public void AddMedalInfo(string spriteName, string rate)
		{
			this.spriteNames.Add(spriteName);
			this.rateList.Add(rate);
		}

		public void StartAnimation()
		{
			if (1 < this.spriteNames.Count)
			{
				this.alphaTween.StartFadeOut();
			}
		}
	}
}
