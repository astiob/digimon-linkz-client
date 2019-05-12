using System;
using UnityEngine;

[ExecuteInEditMode]
public class ShadowParams : MonoBehaviour
{
	private const string _layerName = "IgnoreShadow";

	private static GameObject cachedShadowParamPrefab;

	[SerializeField]
	private Projector _shadowProjector;

	private Transform cachedTransform;

	private static GameObject GetShadowParamPrefab()
	{
		if (ShadowParams.cachedShadowParamPrefab == null)
		{
			ShadowParams.cachedShadowParamPrefab = Resources.Load<GameObject>("Public/Characters/Shadow/prefab");
		}
		return ShadowParams.cachedShadowParamPrefab;
	}

	public static ShadowParams SetShadowObject(CharacterParams character)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ShadowParams.GetShadowParamPrefab());
		gameObject.name = "Shadow";
		gameObject.transform.SetParent(character.transform);
		gameObject.transform.position = new Vector3(character.characterCenterTarget.position.x, character.characterCenterTarget.position.y + character.RootToCenterDistance(), character.characterCenterTarget.position.z);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		ShadowParams component = gameObject.GetComponent<ShadowParams>();
		foreach (Renderer renderer in character.transform.GetComponentsInChildren<Renderer>())
		{
			renderer.gameObject.layer = component.shadowLayerMask.value;
		}
		component.Initialize();
		return component;
	}

	public LayerMask shadowLayerMask
	{
		get
		{
			return LayerMask.NameToLayer("IgnoreShadow");
		}
	}

	public void Initialize()
	{
		this.cachedTransform.localScale = Vector3.one;
		this.cachedTransform.localRotation = Quaternion.identity;
		this._shadowProjector.nearClipPlane = 0f;
		this._shadowProjector.farClipPlane = 30f;
	}

	private void Awake()
	{
		this.cachedTransform = base.transform;
		this.Initialize();
	}

	public void UpdateShadowPosition(CharacterParams character)
	{
		this.cachedTransform.position = new Vector3(character.characterCenterTarget.position.x, character.characterCenterTarget.position.y + character.RootToCenterDistance(), character.characterCenterTarget.position.z);
		this.cachedTransform.localRotation = Quaternion.identity;
	}

	public bool shadowEnable
	{
		get
		{
			return this._shadowProjector.enabled;
		}
		set
		{
			this._shadowProjector.enabled = value;
		}
	}
}
