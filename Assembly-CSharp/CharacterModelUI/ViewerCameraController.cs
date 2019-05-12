using System;
using UnityEngine;

namespace CharacterModelUI
{
	public sealed class ViewerCameraController
	{
		private Matrix4x4 defaultProjectionMatrix;

		private Vector3 cameraPositionOffset;

		private bool isAnimation;

		private float animationCurrentTime;

		private float animationMaxTime;

		private Vector3 animationStartPosition;

		private Vector3 animationEndPosition;

		private Matrix4x4 animationTempTranslation;

		public void SetCameraProjectionMatrix(CommonRender3DRT renderTextureCamera, float screenPositionOffsetY, float viewPositionOffsetX)
		{
			this.cameraPositionOffset = Camera.main.ScreenToViewportPoint(new Vector3(0f, screenPositionOffsetY, 0f));
			this.cameraPositionOffset.x = viewPositionOffsetX;
			this.defaultProjectionMatrix = renderTextureCamera.projectionMatrix;
			Matrix4x4 lhs = Matrix4x4.TRS(this.cameraPositionOffset, Quaternion.identity, Vector3.one);
			renderTextureCamera.projectionMatrix = lhs * this.defaultProjectionMatrix;
		}

		public void MoveTo(float startPosX, float endPosX, float animationMaxTime)
		{
			this.animationStartPosition.Set(startPosX, this.cameraPositionOffset.y, 0f);
			this.animationEndPosition.Set(endPosX, this.cameraPositionOffset.y, 0f);
			this.animationCurrentTime = 0f;
			this.animationMaxTime = animationMaxTime;
			this.isAnimation = true;
		}

		public void UpdateAnimation(float deltaTime, CommonRender3DRT reanderTextureCamera)
		{
			if (this.isAnimation)
			{
				this.animationCurrentTime += deltaTime;
				if (this.animationCurrentTime > this.animationMaxTime)
				{
					this.animationCurrentTime = this.animationMaxTime;
					this.isAnimation = false;
				}
				Vector3 pos = Vector3.Lerp(this.animationStartPosition, this.animationEndPosition, this.animationCurrentTime / this.animationMaxTime);
				this.animationTempTranslation.SetTRS(pos, Quaternion.identity, Vector3.one);
				reanderTextureCamera.projectionMatrix = this.animationTempTranslation * this.defaultProjectionMatrix;
			}
		}
	}
}
