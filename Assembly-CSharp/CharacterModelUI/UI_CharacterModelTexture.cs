using System;
using UnityEngine;

namespace CharacterModelUI
{
	public class UI_CharacterModelTexture : MonoBehaviour
	{
		[SerializeField]
		private UITexture storeUITexture;

		protected CommonRender3DRT renderTextureCamera;

		public virtual void Initialize(Vector3 renderCameraPosition)
		{
			this.Initialize(renderCameraPosition, this.storeUITexture.width, this.storeUITexture.height);
		}

		public virtual void Initialize(Vector3 renderCameraPosition, int width, int height)
		{
			GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DRT", null);
			this.renderTextureCamera = gameObject.GetComponent<CommonRender3DRT>();
			this.renderTextureCamera.transform.localPosition = renderCameraPosition;
			this.renderTextureCamera.SetRenderTarget(width, height, 16);
			this.storeUITexture.mainTexture = this.renderTextureCamera.GetRenderTarget();
			gameObject = this.renderTextureCamera.GetCameraGameObject();
			gameObject.transform.localPosition = Vector3.zero;
		}

		public void DestroyRenderTexture()
		{
			if (null != this.storeUITexture)
			{
				this.storeUITexture.enabled = false;
			}
			if (null != this.renderTextureCamera)
			{
				UnityEngine.Object.Destroy(this.renderTextureCamera.gameObject);
				this.renderTextureCamera = null;
			}
		}

		public virtual void LoadMonsterModel(string monsterGroupId, Vector3 characterPosition, float characterEulerAngleY)
		{
			string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(monsterGroupId);
			this.renderTextureCamera.LoadMonsterModel(monsterCharaPathByMonsterGroupId, characterPosition, characterEulerAngleY);
		}

		public virtual void LoadEggModel(string monsterGroupId, Vector3 characterPosition, float characterEulerAngleY)
		{
			string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(monsterGroupId);
			this.renderTextureCamera.LoadEgg(monsterCharaPathByMonsterGroupId, this.renderTextureCamera.transform.localPosition.x, this.renderTextureCamera.transform.localPosition.y, characterPosition.y);
		}

		public void SetCharacterCameraDistance()
		{
			if (null != this.renderTextureCamera)
			{
				Vector3 characterCameraDistance = this.renderTextureCamera.GetCharacterCameraDistance();
				GameObject characterGameObject = this.renderTextureCamera.GetCharacterGameObject();
				GameObject cameraGameObject = this.renderTextureCamera.GetCameraGameObject();
				if (null != characterGameObject && null != cameraGameObject)
				{
					characterCameraDistance.Set(characterCameraDistance.x, characterCameraDistance.y, characterCameraDistance.z * cameraGameObject.transform.forward.z);
					characterGameObject.transform.localPosition = -characterCameraDistance;
				}
			}
		}

		public void DeleteCharacterModel()
		{
			if (null != this.renderTextureCamera)
			{
				this.renderTextureCamera.DeleteCharacterModel();
			}
		}
	}
}
