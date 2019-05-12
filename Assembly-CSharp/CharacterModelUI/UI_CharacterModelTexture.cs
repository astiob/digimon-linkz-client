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

		public virtual void LoadCharacterModel(MonsterData monsterData, Vector3 characterPosition, float characterEulerAngleY)
		{
			if (!MonsterData.IsEgg(monsterData.monsterMG.growStep))
			{
				string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(monsterData.monsterM.monsterGroupId);
				this.renderTextureCamera.LoadMonsterModel(monsterCharaPathByMonsterGroupId, characterPosition, characterEulerAngleY);
			}
			else
			{
				GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM respDataMA_MonsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM;
				string monsterGroupId = string.Empty;
				for (int i = 0; i < respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length; i++)
				{
					GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM = respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i];
					if (monsterData.userMonster == null || monsterEvolutionRouteM.monsterEvolutionRouteId == monsterData.userMonster.monsterEvolutionRouteId)
					{
						monsterGroupId = monsterEvolutionRouteM.eggMonsterId;
						break;
					}
				}
				string monsterCharaPathByMonsterGroupId2 = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(monsterGroupId);
				this.renderTextureCamera.LoadEgg(monsterCharaPathByMonsterGroupId2, this.renderTextureCamera.transform.localPosition.x, this.renderTextureCamera.transform.localPosition.y, characterPosition.y);
			}
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
