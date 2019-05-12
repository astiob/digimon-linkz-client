using System;
using UnityEngine;

namespace CharacterModelUI
{
	public abstract class CharacterModelController
	{
		public virtual void UpdateAction(float deltaTime)
		{
		}

		public abstract void OnClick(GameObject characterGameObject);

		public abstract void OnDrag(Vector2 deltaMove, CommonRender3DRT reanderTextureCamera);

		public abstract void OnPinth(PinchInOut pinch, GameObject characterGameObject);
	}
}
