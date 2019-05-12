using System;
using UnityEngine;

public class GardenShadow : MonoBehaviour
{
	[SerializeField]
	private float shadowSize = 2.5f;

	[SerializeField]
	private float eggShadowSize = 1.5f;

	private SkinnedMeshRenderer targetRenderer;

	private bool isInitialized;

	private void Update()
	{
		if (this.isInitialized)
		{
			this.UpdatePosition();
		}
	}

	public void Initialize(GameObject TargetObj)
	{
		this.targetRenderer = TargetObj.GetComponentInChildren<SkinnedMeshRenderer>();
		if (this.targetRenderer != null)
		{
			base.gameObject.transform.parent = this.targetRenderer.gameObject.transform;
			base.gameObject.transform.localScale = new Vector3(this.shadowSize, this.shadowSize, this.shadowSize);
			this.UpdatePosition();
			base.gameObject.SetActive(true);
			this.isInitialized = true;
		}
		else
		{
			MeshRenderer componentInChildren = TargetObj.GetComponentInChildren<MeshRenderer>();
			base.gameObject.transform.parent = componentInChildren.gameObject.transform.parent;
			base.gameObject.transform.localScale = new Vector3(this.eggShadowSize, this.eggShadowSize, this.eggShadowSize);
			base.gameObject.transform.localPosition = Vector3.zero;
			base.gameObject.SetActive(true);
		}
	}

	private void UpdatePosition()
	{
		base.gameObject.transform.position = new Vector3(this.targetRenderer.bounds.center.x, 0f, this.targetRenderer.bounds.center.z);
		base.gameObject.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x, 0f, base.gameObject.transform.localPosition.z);
	}
}
