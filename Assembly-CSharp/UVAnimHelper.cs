using System;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimHelper : MonoBehaviour
{
	[Header("UV アニメ U方向スピード")]
	[SerializeField]
	private float speedU;

	[Header("UV アニメ V方向スピード")]
	[SerializeField]
	private float speedV;

	[Header("テクスチャ タイリング X")]
	[SerializeField]
	private float tileX = 1f;

	[Header("テクスチャ タイリング Y")]
	[SerializeField]
	private float tileY = 10f;

	private float totalU;

	private float totalV;

	private SkinnedMeshRenderer smr;

	private List<Material> matL;

	protected virtual void Awake()
	{
		this.smr = base.gameObject.GetComponent<SkinnedMeshRenderer>();
		if (this.smr != null)
		{
			this.matL = new List<Material>();
			for (int i = 0; i < this.smr.materials.Length; i++)
			{
				Material material = this.smr.materials[i];
				if (material != null)
				{
					this.matL.Add(material);
				}
			}
		}
	}

	protected virtual void Start()
	{
		if (this.matL != null)
		{
			for (int i = 0; i < this.matL.Count; i++)
			{
				this.matL[i].SetTextureScale("_MainTex", new Vector2(this.tileX, this.tileY));
			}
		}
	}

	protected virtual void Update()
	{
		this.totalU += this.speedU;
		this.totalV += this.speedV;
		int num = (int)this.totalU;
		float value = this.totalU - (float)num;
		int num2 = (int)this.totalV;
		float value2 = this.totalV - (float)num2;
		if (this.matL != null)
		{
			for (int i = 0; i < this.matL.Count; i++)
			{
				this.matL[i].SetFloat("_UVAnimOffsetX", value);
				this.matL[i].SetFloat("_UVAnimOffsetY", value2);
			}
		}
	}

	protected virtual void OnDestroy()
	{
	}
}
