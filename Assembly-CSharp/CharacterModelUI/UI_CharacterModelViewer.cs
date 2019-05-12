using System;
using UnityEngine;

namespace CharacterModelUI
{
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(UIWidget))]
	public sealed class UI_CharacterModelViewer : UI_CharacterModelTexture
	{
		private const float DETAILS_UI_VIEW_POSITION_OFFSET_X = -0.6f;

		private const float DETAILS_UI_SCREEN_POSITION_OFFSET_Y = -80f;

		private BoxCollider touchAreaCollider;

		private ViewerCameraController viewerCameraController;

		private CharacterModelController characterModelController;

		private PinchInOut pinch;

		private void OnDrag(Vector2 deltaMove)
		{
			if (this.touchAreaCollider.enabled && 2 > Input.touchCount)
			{
				this.characterModelController.OnDrag(deltaMove, this.renderTextureCamera);
			}
		}

		private void OnClick()
		{
			if (this.touchAreaCollider.enabled)
			{
				GameObject characterGameObject = this.renderTextureCamera.GetCharacterGameObject();
				if (null != characterGameObject)
				{
					this.characterModelController.OnClick(characterGameObject);
				}
			}
		}

		private void Update()
		{
			if (null != this.renderTextureCamera)
			{
				this.viewerCameraController.UpdateAnimation(Time.deltaTime, this.renderTextureCamera);
				if (this.characterModelController != null)
				{
					this.characterModelController.UpdateAction(Time.deltaTime);
					if (this.touchAreaCollider.enabled)
					{
						GameObject characterGameObject = this.renderTextureCamera.GetCharacterGameObject();
						if (null != characterGameObject)
						{
							this.characterModelController.OnPinth(this.pinch, characterGameObject);
						}
					}
				}
			}
		}

		public override void Initialize(Vector3 renderCameraPosition, int width, int height)
		{
			base.Initialize(renderCameraPosition, width, height);
			this.touchAreaCollider = base.gameObject.GetComponent<BoxCollider>();
			this.viewerCameraController = new ViewerCameraController();
			this.pinch = new PinchInOut();
		}

		public override void LoadCharacterModel(MonsterData monsterData, Vector3 characterPosition, float characterEulerAngleY)
		{
			base.LoadCharacterModel(monsterData, characterPosition, characterEulerAngleY);
			if (!MonsterData.IsEgg(monsterData.monsterMG.growStep))
			{
				this.characterModelController = new MonsterModelController();
			}
			else
			{
				this.characterModelController = new EggModelController();
			}
		}

		public void SetCameraViewOffset(float screenPositionOffsetY, float viewPositionOffsetX)
		{
			this.viewerCameraController.SetCameraProjectionMatrix(this.renderTextureCamera, screenPositionOffsetY, viewPositionOffsetX);
		}

		public void SetCameraViewOffsetDetailsUI()
		{
			this.SetCameraViewOffset(-80f, -0.6f);
		}

		public void MoveToCenterDetailsUI(float animationMaxTime)
		{
			this.viewerCameraController.MoveTo(-0.6f, 0f, animationMaxTime);
		}

		public void MoveToLeftDetailsUI(float animationMaxTime)
		{
			this.viewerCameraController.MoveTo(0f, -0.6f, animationMaxTime);
		}

		public void EnableTouchEvent(bool enable)
		{
			this.touchAreaCollider.enabled = enable;
		}
	}
}
