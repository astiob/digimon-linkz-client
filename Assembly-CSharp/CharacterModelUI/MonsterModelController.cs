using System;
using UnityEngine;

namespace CharacterModelUI
{
	public sealed class MonsterModelController : CharacterModelController
	{
		private bool isPlayAnimation;

		private float animationTimeLength;

		private float animationCurrentTime;

		public override void UpdateAction(float deltaTime)
		{
			if (this.isPlayAnimation)
			{
				this.animationCurrentTime += deltaTime;
				if (this.animationTimeLength <= this.animationCurrentTime)
				{
					this.isPlayAnimation = false;
					this.animationCurrentTime = 0f;
				}
			}
		}

		public override void OnClick(GameObject characterGameObject)
		{
			if (!this.isPlayAnimation)
			{
				CharacterParams component = characterGameObject.GetComponent<CharacterParams>();
				if (null != component && null != component.transform.parent)
				{
					this.isPlayAnimation = true;
					switch (UnityEngine.Random.Range(0, 3))
					{
					case 0:
						component.PlayAnimationSmooth(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
						this.animationTimeLength = component.AnimationClipLength;
						break;
					case 1:
						component.PlayAnimationSmooth(CharacterAnimationType.eat, SkillType.Attack, 0, null, null);
						this.animationTimeLength = component.AnimationClipLength;
						break;
					case 2:
						component.PlayAnimationSmooth(CharacterAnimationType.attacks, SkillType.Attack, 0, null, null);
						this.animationTimeLength = component.AnimationClipLength;
						break;
					}
				}
			}
		}

		public override void OnDrag(Vector2 deltaMove, CommonRender3DRT reanderTextureCamera)
		{
			float addRotationEulerY = -(deltaMove.x / 3f);
			reanderTextureCamera.AddCharacterRotationY(addRotationEulerY);
		}

		public override void OnPinth(PinchInOut pinch, GameObject characterGameObject)
		{
			CharacterParams component = characterGameObject.GetComponent<CharacterParams>();
			if (null != component && null != component.transform.parent)
			{
				float value = pinch.Value;
				Vector3 localScale = component.transform.localScale;
				if (value == 0f || (value < 0f && localScale.x <= 0.8f) || (value > 0f && localScale.x >= 1.2f))
				{
					return;
				}
				float num = localScale.x + value / 500f;
				if (num < 0.8f)
				{
					num = 0.8f;
				}
				else if (num > 1.2f)
				{
					num = 1.2f;
				}
				component.transform.localScale = new Vector3(num, num, num);
			}
		}
	}
}
