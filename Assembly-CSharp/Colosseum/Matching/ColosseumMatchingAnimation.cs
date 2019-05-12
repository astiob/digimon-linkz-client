using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colosseum.Matching
{
	[Serializable]
	public sealed class ColosseumMatchingAnimation
	{
		[SerializeField]
		private Vector3 rootPosition;

		private CommonRender3DPartyRT monsterRenderTexture;

		[Header("マッチング中アニメオブジェクト")]
		[SerializeField]
		private GameObject matchingProcessEffect;

		[SerializeField]
		[Header("マッチング完了アニメオブジェクト")]
		private GameObject matchingFinishedEffect;

		[SerializeField]
		[Header("キャラの勝利アニメを見せる時間（秒）")]
		private float winAnimationWait;

		[Header("キャラが消えてから情報が出るまでの時間（秒）")]
		[SerializeField]
		private float transferWait;

		[Header("RenderTarget用テクスチャ")]
		[SerializeField]
		private UITexture renderTargetTexture;

		[Header("転送エフェクトの乗算色(アルファは０固定)")]
		[SerializeField]
		private Color renderTextureColor;

		private int monsterCount;

		private AlwaysEffectParams[] transferEffectController;

		private Material transferEffectMaterial;

		private ColosseumMatchingEventListener eventListener;

		private IEnumerator StartMatchingEffect()
		{
			this.matchingProcessEffect.SetActive(false);
			this.matchingFinishedEffect.SetActive(true);
			this.monsterRenderTexture.SetAnimation(CharacterAnimationType.win);
			this.transferEffectController = this.CreateTransferEffects();
			this.monsterRenderTexture.SetCameraBackgroundColor(this.renderTextureColor);
			SoundMng.Instance().PlaySE("SEInternal/Farm/se_217", 0f, false, true, null, -1, 1f);
			WaitForSeconds waitTime = new WaitForSeconds(this.winAnimationWait);
			yield return waitTime;
			yield break;
		}

		private IEnumerator WaitFinishMatchingEffect()
		{
			for (int i = 0; i < this.monsterCount; i++)
			{
				this.transferEffectController[i].PlayAnimationTrigger(AlwaysEffectState.Out);
			}
			for (int j = 0; j < this.monsterCount; j++)
			{
				GameObject chara = this.monsterRenderTexture.GetChara(j);
				chara.SetActive(false);
			}
			WaitForSeconds waitTime = new WaitForSeconds(this.transferWait);
			yield return waitTime;
			yield break;
		}

		private AlwaysEffectParams[] CreateTransferEffects()
		{
			Shader shader = Shader.Find("Effect/Custom_MobileParticlesAdditive");
			if (shader != null && shader.isSupported)
			{
				this.transferEffectMaterial = new Material(shader);
			}
			GameObject resource = AssetDataMng.Instance().LoadObject("AlwaysEffects/transferPVPCharacterEffect/prefab", null, true) as GameObject;
			AlwaysEffectParams[] array = new AlwaysEffectParams[this.monsterCount];
			for (int i = 0; i < this.monsterCount; i++)
			{
				GameObject chara = this.monsterRenderTexture.GetChara(i);
				GameObject transferEffect = this.GetTransferEffect(resource);
				Transform transform = transferEffect.transform;
				transform.parent = chara.transform.parent;
				transform.position = chara.transform.position;
				transform.localScale = Vector3.one;
				transform.localRotation = Quaternion.identity;
				array[i] = transferEffect.GetComponent<AlwaysEffectParams>();
				array[i].PlayAnimationTrigger(AlwaysEffectState.In);
				this.SetBillboard(chara.transform, array[i]);
			}
			return array;
		}

		private void SetBillboard(Transform charaTransform, AlwaysEffectParams effect)
		{
			BillboardObject[] componentsInChildren = effect.GetComponentsInChildren<BillboardObject>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].transform.forward = -charaTransform.forward;
				}
			}
		}

		private GameObject GetTransferEffect(GameObject resource)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(resource);
			if (null != this.transferEffectMaterial)
			{
				Transform transform = gameObject.transform.FindChild("transferCharacter/UpEffect");
				if (null != transform)
				{
					Renderer component = transform.GetComponent<Renderer>();
					if (null != component && null != component.material)
					{
						if (null == this.transferEffectMaterial.mainTexture)
						{
							this.transferEffectMaterial.mainTexture = component.material.mainTexture;
						}
						component.material = this.transferEffectMaterial;
					}
				}
			}
			return gameObject;
		}

		public void SetInstance(ColosseumMatchingEventListener eventListener)
		{
			this.eventListener = eventListener;
		}

		public int GetShowMonsterCount()
		{
			return this.monsterCount;
		}

		public void ShowMonster(MonsterData[] monsterDataList)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < monsterDataList.Length; i++)
			{
				string filePath = MonsterObject.GetFilePath(monsterDataList[i].GetMonsterMaster().Group.modelId);
				list.Add(filePath);
			}
			this.monsterCount = list.Count;
			GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DPartyRT", null);
			this.monsterRenderTexture = gameObject.GetComponent<CommonRender3DPartyRT>();
			this.monsterRenderTexture.SetCameraBackgroundColor(this.renderTextureColor);
			this.monsterRenderTexture.LoadCharas(list, this.rootPosition.x, this.rootPosition.y);
			this.renderTargetTexture.gameObject.SetActive(true);
			this.renderTargetTexture.mainTexture = this.monsterRenderTexture.SetRenderTarget(1136, 820, 16);
			this.monsterRenderTexture.SetAnimation(CharacterAnimationType.attacks);
		}

		public IEnumerator StartEffect()
		{
			TaskBase task = new NormalTask(this.StartMatchingEffect()).Add(new NormalTask(this.WaitFinishMatchingEffect()));
			return task.Run(new Action(this.eventListener.OnCompletedMatchingAnimation), null, null);
		}

		public IEnumerator DeleteTransferEffect()
		{
			while (this.transferEffectController[0].isPlaying)
			{
				yield return null;
			}
			this.DeleteObject();
			yield break;
		}

		public void DeleteObject()
		{
			if (null != this.transferEffectMaterial)
			{
				this.transferEffectMaterial.mainTexture = null;
				this.transferEffectMaterial.shader = null;
				this.transferEffectMaterial = null;
			}
			if (this.transferEffectController != null)
			{
				for (int i = 0; i < this.transferEffectController.Length; i++)
				{
					if (null != this.transferEffectController[i])
					{
						UnityEngine.Object.Destroy(this.transferEffectController[i].gameObject);
						this.transferEffectController[i] = null;
					}
				}
			}
			if (null != this.monsterRenderTexture)
			{
				UnityEngine.Object.Destroy(this.monsterRenderTexture.gameObject);
				this.monsterRenderTexture = null;
			}
		}
	}
}
