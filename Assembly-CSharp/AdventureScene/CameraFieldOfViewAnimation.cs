using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CameraFieldOfViewAnimation : BaseCameraAnimation
	{
		private float startFOV;

		private float endFOV;

		public CameraFieldOfViewAnimation(float startFOV, float endFOV, float time)
		{
			this.startFOV = startFOV;
			this.endFOV = endFOV;
			this.animationTime = time;
			this.currentTime = 0f;
		}

		public bool UpdateFOV()
		{
			if (!base.IsFinish())
			{
				float num = this.animationTime / this.currentTime;
				num = Mathf.Clamp01(num);
				float fieldOfView = Mathf.Lerp(this.startFOV, this.endFOV, num);
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.fieldOfView = fieldOfView;
				return false;
			}
			return true;
		}
	}
}
