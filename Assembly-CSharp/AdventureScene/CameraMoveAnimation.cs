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
				float new_x = Mathf.Lerp(this.startPosition.x, this.endPosition.x, num);
				float new_y = Mathf.Lerp(this.startPosition.y, this.endPosition.y, num);
				float new_z = Mathf.Lerp(this.startPosition.z, this.endPosition.z, num);
				this.currentPosition.Set(new_x, new_y, new_z);
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.transform.localPosition = this.currentPosition;
				return false;
			}
			return true;
		}
	}
}
