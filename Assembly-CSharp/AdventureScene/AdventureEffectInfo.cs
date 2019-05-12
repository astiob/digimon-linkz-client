using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class AdventureEffectInfo
	{
		public int id;

		public GameObject model;

		public ParticleSystem particle;

		public Animator animator;

		public void Delete()
		{
			if (null != this.model)
			{
				UnityEngine.Object.Destroy(this.model);
				this.model = null;
			}
			this.particle = null;
			this.animator = null;
		}
	}
}
