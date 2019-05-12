using System;
using UnityEngine;

namespace CharacterModelUI
{
	public sealed class EggModelController : CharacterModelController
	{
		public override void OnClick(GameObject characterGameObject)
		{
		}

		public override void OnDrag(Vector2 deltaMove, CommonRender3DRT reanderTextureCamera)
		{
			float addRotationEulerY = -(deltaMove.x / 3f);
			reanderTextureCamera.AddCharacterRotationY(addRotationEulerY);
		}

		public override void OnPinth(PinchInOut pinch, GameObject characterGameObject)
		{
		}
	}
}
