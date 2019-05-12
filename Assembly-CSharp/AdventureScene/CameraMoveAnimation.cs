using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class CameraMoveAnimation : BaseCameraAnimation
	{
		private Vector3 startPosition;

		private Vector3 endPosition;

		private Vector3 currentPosition;

		public CameraMoveAnimation(Vector3 startPosition, Vector3 endPosition, float time)
		{
			this.startPosition = startPosition;
			this.endPosition = endPosition;
			this.currentPosition = startPosition;
			this.animationTime = time;
			this.currentTime = 0f;
		}

		public bool UpdateMove()
		{
			if (!base.IsFinish())
			{
				float num = this.animationTime / this.currentTime;
				num = Mathf.Clamp01(num);
				float newX = Mathf.Lerp(this.startPosition.x, this.endPosition.x, num);
				float newY = Mathf.Lerp(this.startPosition.y, this.endPosition.y, num);
				float newZ = Mathf.Lerp(this.startPosition.z, this.endPosition.z, num);
				this.currentPosition.Set(newX, newY, newZ);
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.transform.localPosition = this.currentPosition;
				return false;
			}
			return true;
		}
	}
}
