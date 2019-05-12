using System;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Tools/Tiled Texture")]
public class TiledTexture : MonoBehaviour
{
	[SerializeField]
	private uint tiledSheetsX = 1u;

	[SerializeField]
	private uint tiledSheetsY = 1u;

	public float interval = 0.5f;

	[SerializeField]
	private string propetyName = "MainTex";

	private Material mat;

	private Vector2 tiledScale;

	private Vector2[] tiledOffset;

	private int currentTile;

	private float nextTime;

	private bool isDrawed;

	private void OnEnable()
	{
		if (base.GetComponent<MaterialController>())
		{
			MaterialController component = base.GetComponent<MaterialController>();
			this.mat = component.GetMaterial();
		}
		else
		{
			if (base.GetComponent<MeshRenderer>())
			{
				MeshRenderer component2 = base.GetComponent<MeshRenderer>();
				this.mat = component2.material;
			}
			if (base.GetComponent<SkinnedMeshRenderer>())
			{
				SkinnedMeshRenderer component3 = base.GetComponent<SkinnedMeshRenderer>();
				this.mat = component3.material;
			}
		}
		this.tiledScale = new Vector2(1f / this.tiledSheetsX, 1f / this.tiledSheetsY);
		this.tiledOffset = new Vector2[this.tiledSheetsX * this.tiledSheetsY];
		int num = 0;
		while ((long)num < (long)((ulong)this.tiledSheetsY))
		{
			int num2 = 0;
			while ((long)num2 < (long)((ulong)this.tiledSheetsX))
			{
				this.tiledOffset[(int)(checked((IntPtr)(unchecked((long)num * (long)((ulong)this.tiledSheetsX) + (long)num2))))] = new Vector2(this.tiledScale.x * (float)num2, (1f - this.tiledScale.y * (float)num - this.tiledScale.y) % 1f);
				num2++;
			}
			num++;
		}
		this.nextTime = Time.realtimeSinceStartup + Mathf.Clamp(this.interval, 0f, float.MaxValue);
		if (this.mat != null && this.mat.HasProperty("_" + this.propetyName))
		{
			this.mat.SetTextureScale("_" + this.propetyName, this.tiledScale);
		}
		this.SetImage(0);
	}

	private void SetImage(int index)
	{
		if (this.mat != null && this.mat.HasProperty("_" + this.propetyName))
		{
			this.mat.SetTextureOffset("_" + this.propetyName, this.tiledOffset[index]);
		}
	}

	private void Update()
	{
		this.isDrawed = false;
	}

	private void OnWillRenderObject()
	{
		bool flag = false;
		if (FollowTargetCamera.IsVisible())
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		if (this.isDrawed)
		{
			return;
		}
		this.SetImage(this.currentTile);
		this.isDrawed = true;
	}

	private void FixedUpdate()
	{
		if (!base.enabled)
		{
			return;
		}
		if (Time.time > this.nextTime)
		{
			this.currentTile = (this.currentTile + 1) % this.tiledOffset.Length;
			this.nextTime = Time.time + Mathf.Clamp(this.interval, 0f, float.MaxValue);
		}
	}
}
