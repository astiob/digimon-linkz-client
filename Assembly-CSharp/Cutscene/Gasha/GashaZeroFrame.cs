using Monster;
using System;
using UnityEngine;

namespace Cutscene.Gasha
{
	public sealed class GashaZeroFrame : GashaAnimationBase
	{
		public Camera mainCamera;

		public Camera subCamera;

		public CameraSwitcher cameraSwitcher;

		public ParticleSystem rareSignParticleSpark;

		public GameObject rareSignRainbowBox;

		public Color bgColorRareNone;

		public Color bgColorRareLow;

		public Color bgColorRareMiddle;

		public Transform rareSignParticle;

		public void StartAnimation(string growStep)
		{
			this.rareSignParticleSpark.Clear();
			if (MonsterGrowStepData.IsGrowStepHigh(growStep))
			{
				this.rareSignRainbowBox.SetActive(true);
			}
			else
			{
				this.rareSignRainbowBox.SetActive(false);
			}
			this.subCamera.transform.localPosition = new Vector3(0f, 1f, -2f);
			if (MonsterGrowStepData.IsRipeScope(growStep))
			{
				this.mainCamera.backgroundColor = this.bgColorRareLow;
				this.subCamera.backgroundColor = this.bgColorRareLow;
			}
			else if (MonsterGrowStepData.IsGrowStepHigh(growStep))
			{
				this.mainCamera.backgroundColor = this.bgColorRareMiddle;
				this.subCamera.backgroundColor = this.bgColorRareMiddle;
			}
			else
			{
				this.mainCamera.backgroundColor = this.bgColorRareNone;
				this.subCamera.backgroundColor = this.bgColorRareNone;
			}
			this.cameraSwitcher.SetLookAtObject(this.rareSignParticle);
			this.cameraSwitcher.EnableSubCamera();
			base.EndCallback();
		}
	}
}
