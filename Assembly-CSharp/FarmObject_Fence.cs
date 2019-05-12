using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FarmObject_Fence : FarmObject
{
	[SerializeField]
	private List<FarmObject_Fence.Data> dataList = new List<FarmObject_Fence.Data>();

	private MeshFilter meshFilter;

	private MeshRenderer meshRenderer;

	protected override void Awake()
	{
		base.Awake();
		this.meshFilter = base.transform.FindChild("Locator/ModelRoot/farm_item_05").GetComponent<MeshFilter>();
		this.meshRenderer = base.transform.FindChild("Locator/ModelRoot/farm_item_05").GetComponent<MeshRenderer>();
		this.Init();
	}

	public void Init()
	{
		base.transform.localRotation = Quaternion.identity;
		this.ChangeFenceType(FarmObject_Fence.Type.Mesh01);
	}

	public void ChangeFenceType(FarmObject_Fence.Type type)
	{
		this.meshFilter.mesh = this.dataList[(int)type].mesh;
		foreach (Material material in this.meshRenderer.materials)
		{
			material.mainTexture = this.dataList[(int)type].texture;
		}
	}

	public enum Type
	{
		Mesh01,
		Mesh02,
		Mesh03,
		Mesh04
	}

	[Serializable]
	public class Data
	{
		public Mesh mesh;

		public Texture texture;
	}
}
