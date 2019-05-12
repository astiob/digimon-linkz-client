using System;
using System.Collections;
using UnityEngine;

public sealed class PvPMatchingFinishAnimation
{
	private float winAnimationWaitTime;

	private GameObject matchingProcessEffect;

	private GameObject matchingFinishedEffect;

	private PartsMatchingWaitMonsInfo monsterModel;

	private AlwaysEffectParams[] transferEffectController;

	private Material transferEffectMaterial;

	private Color renderTextureBackgroundColor;

	public PvPMatchingFinishAnimation(GameObject processEffect, GameObject finishedEffect, PartsMatchingWaitMonsInfo monsterInfo, float winAnimationWait, Color renderTextureColor)
	{
		this.matchingProcessEffect = processEffect;
		this.matchingFinishedEffect = finishedEffect;
		this.winAnimationWaitTime = winAnimationWait;
		this.monsterModel = monsterInfo;
		this.renderTextureBackgroundColor = renderTextureColor;
	}

	public IEnumerator StartMatchingEffect()
	{
		this.matchingProcessEffect.SetActive(false);
		this.matchingFinishedEffect.SetActive(true);
		this.monsterModel.SetAnimation(CharacterAnimationType.win);
		this.transferEffectController = this.CreateTransferEffects();
		this.monsterModel.SetRenderTextureBackgroundColor(this.renderTextureBackgroundColor);
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_217", 0f, false, true, null, -1, 1f);
		WaitForSeconds waitTime = new WaitForSeconds(this.winAnimationWaitTime);
		yield return waitTime;
		for (int i = 0; i < this.monsterModel.mdList.Count; i++)
		{
			this.transferEffectController[i].PlayAnimationTrigger(AlwaysEffectState.Out);
		}
		this.monsterModel.HideChara();
		yield break;
	}

	public IEnumerator DeleteTransferEffect()
	{
		while (this.transferEffectController[0].isPlaying)
		{
			yield return null;
		}
		if (null != this.transferEffectMaterial)
		{
			this.transferEffectMaterial.mainTexture = null;
			this.transferEffectMaterial.shader = null;
			this.transferEffectMaterial = null;
		}
		for (int i = 0; i < this.monsterModel.mdList.Count; i++)
		{
			UnityEngine.Object.Destroy(this.transferEffectController[i].gameObject);
			this.transferEffectController[i] = null;
		}
		yield break;
	}

	private AlwaysEffectParams[] CreateTransferEffects()
	{
		int count = this.monsterModel.mdList.Count;
		Shader shader = Shader.Find("Effect/Custom_MobileParticlesAdditive");
		if (shader != null && shader.isSupported)
		{
			this.transferEffectMaterial = new Material(shader);
		}
		GameObject resource = AssetDataMng.Instance().LoadObject("AlwaysEffects/transferPVPCharacterEffect/prefab", null, true) as GameObject;
		AlwaysEffectParams[] array = new AlwaysEffectParams[count];
		for (int i = 0; i < count; i++)
		{
			GameObject chara = this.monsterModel.GetChara(i);
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
}
