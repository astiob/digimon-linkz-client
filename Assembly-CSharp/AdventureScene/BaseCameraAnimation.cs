using System;
using UnityEngine;

namespace AdventureScene
{
	public class BaseCameraAnimation
	{
		protected float animationTime;

		protected float currentTime;

		protected bool IsFinish()
		{
			float num = Time.unscaledDeltaTime;
			if (0.033f < num)
			{
				num = 0.033f;
			}
			this.currentTime += num;
			return this.animationTime <= this.currentTime;
		}
	}
}
