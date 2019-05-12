using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class AdventureDigimonInfo
	{
		public int id;

		public GameObject model;

		public Animator animator;

		public GameObject[] skillEffectList = new GameObject[2];

		public string[] skillEffectSeList = new string[2];

		public GameObject[] skillCameraAnimation = new GameObject[2];

		public void Delete()
		{
			if (null != this.model)
			{
				UnityEngine.Object.Destroy(this.model);
				this.model = null;
			}
			this.animator = null;
			for (int i = 0; i < this.skillEffectList.Length; i++)
			{
				if (null != this.skillEffectList[i])
				{
					UnityEngine.Object.Destroy(this.skillEffectList[i]);
					this.skillEffectList[i] = null;
				}
			}
			for (int j = 0; j < this.skillCameraAnimation.Length; j++)
			{
				if (null != this.skillCameraAnimation[j])
				{
					UnityEngine.Object.Destroy(this.skillCameraAnimation[j]);
					this.skillCameraAnimation[j] = null;
				}
			}
		}
	}
}
