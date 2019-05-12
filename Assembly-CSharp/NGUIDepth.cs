using System;
using System.Collections.Generic;
using UnityEngine;

public class NGUIDepth : UIWidget
{
	[Tooltip("子オブジェクトのRendererを含める.")]
	public bool includeChildren;

	private Renderer mMainRenderer;

	[SerializeField]
	private List<Renderer> mRenderers = new List<Renderer>();

	private static int _AlphaId;

	private static int[] _ClipRange;

	private static int[] _ClipArgs;

	protected int renderQueue = -1;

	private Vector4 defaultClipRange = new Vector4(0f, 0f, 1000f, 1000f);

	private Vector4 defaultClipArgs = new Vector4(1000f, 1000f, 0f, 1f);

	private Vector4 defaultClipArgs_0 = new Vector4(0f, 0f, 0f, 0f);

	public Renderer MainRenderer
	{
		get
		{
			this.mMainRenderer = (this.mMainRenderer ?? base.GetComponentInChildren<Renderer>());
			return this.mMainRenderer;
		}
	}

	public List<Renderer> Renderers
	{
		get
		{
			this.mRenderers.RemoveAll((Renderer x) => x == null);
			if (this.mRenderers.Count == 0)
			{
				if (this.includeChildren)
				{
					base.GetComponentsInChildren<Renderer>(true, this.mRenderers);
				}
				else
				{
					base.GetComponents<Renderer>(this.mRenderers);
				}
			}
			return this.mRenderers;
		}
	}

	public override Material material
	{
		get
		{
			if (!this.MainRenderer)
			{
				return null;
			}
			if (!Application.isPlaying)
			{
				return this.MainRenderer.sharedMaterial;
			}
			return this.MainRenderer.material;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no material setter");
		}
	}

	public override Shader shader
	{
		get
		{
			return (!(this.material == null)) ? this.material.shader : null;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no shader setter");
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		this.Reset();
	}

	protected virtual void OnDestroy()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		base.RemoveFromPanel();
		this.mMainRenderer = null;
		if (this.mRenderers != null)
		{
			this.mRenderers.Clear();
			this.mRenderers = null;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		verts.Add(new Vector3(10000f, 10000f));
		verts.Add(new Vector3(10000f, 10000f));
		verts.Add(new Vector3(10000f, 10000f));
		verts.Add(new Vector3(10000f, 10000f));
		uvs.Add(new Vector2(0f, 0f));
		uvs.Add(new Vector2(0f, 1f));
		uvs.Add(new Vector2(1f, 1f));
		uvs.Add(new Vector2(1f, 0f));
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
	}

	public void Reset()
	{
		if (base.GetComponentInParent<UIRoot>())
		{
			this.mRenderers = new List<Renderer>();
		}
	}

	public static int AlphaId
	{
		get
		{
			if (NGUIDepth._AlphaId == 0)
			{
				NGUIDepth._AlphaId = Shader.PropertyToID("_Alpha");
			}
			return NGUIDepth._AlphaId;
		}
	}

	private static int[] ClipRange
	{
		get
		{
			if (NGUIDepth._ClipRange == null)
			{
				NGUIDepth._ClipRange = new int[]
				{
					Shader.PropertyToID("_ClipRange0"),
					Shader.PropertyToID("_ClipRange1"),
					Shader.PropertyToID("_ClipRange2"),
					Shader.PropertyToID("_ClipRange3")
				};
			}
			return NGUIDepth._ClipRange;
		}
	}

	private static int[] ClipArgs
	{
		get
		{
			if (NGUIDepth._ClipArgs == null)
			{
				NGUIDepth._ClipArgs = new int[]
				{
					Shader.PropertyToID("_ClipArgs0"),
					Shader.PropertyToID("_ClipArgs1"),
					Shader.PropertyToID("_ClipArgs2"),
					Shader.PropertyToID("_ClipArgs3")
				};
			}
			return NGUIDepth._ClipArgs;
		}
	}

	public override float alpha { get; set; }

	protected virtual void OnWillRenderObject()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (!this.drawCall || !this.panel)
		{
			if (this.alpha != 0f)
			{
				this.alpha = 0f;
				this.UpdateMat(0, this.defaultClipRange, this.defaultClipArgs_0);
				this.UpdateMatAlpha(0f);
			}
			base.enabled = false;
			base.enabled = true;
			if (!this.drawCall)
			{
				return;
			}
		}
		if (this.renderQueue != this.drawCall.renderQueue)
		{
			foreach (Renderer renderer in this.Renderers)
			{
				renderer.sortingOrder = this.drawCall.sortingOrder;
				foreach (Material material in renderer.materials)
				{
					material.renderQueue = this.drawCall.renderQueue;
				}
			}
			UIPanel uipanel = this.panel;
			int j = 0;
			while (j < NGUIDepth.ClipRange.Length && uipanel != null)
			{
				if (uipanel.hasClipping)
				{
					Vector3[] worldCorners = uipanel.worldCorners;
					Vector2 vector = (worldCorners[2] + worldCorners[0]) / 2f;
					Vector2 vector2 = (worldCorners[2] - worldCorners[0]) / 2f;
					Vector4 clipRange = new Vector4(vector.x, vector.y, vector2.x, vector2.y);
					Vector2 clipSoftness = uipanel.clipSoftness;
					clipSoftness.x = ((0f >= clipSoftness.x) ? 1000f : (uipanel.baseClipRegion.z / clipSoftness.x));
					clipSoftness.y = ((0f >= clipSoftness.y) ? 1000f : (uipanel.baseClipRegion.w / clipSoftness.y));
					this.UpdateMat(j, clipRange, clipSoftness);
					j++;
				}
				uipanel = uipanel.parentPanel;
			}
			while (j < NGUIDepth.ClipRange.Length)
			{
				this.UpdateMat(j, this.defaultClipRange, this.defaultClipArgs);
				j++;
			}
			this.alpha = this.finalAlpha;
			this.UpdateMatAlpha(this.finalAlpha);
		}
	}

	protected void UpdateMat(int index, Vector4 clipRange, Vector4 clipArgs)
	{
		if (NGUIDepth.ClipRange.Length <= index)
		{
			return;
		}
		foreach (Renderer renderer in this.Renderers)
		{
			foreach (Material material in renderer.materials)
			{
				material.SetVector(NGUIDepth.ClipRange[index], clipRange);
				material.SetVector(NGUIDepth.ClipArgs[index], clipArgs);
			}
		}
	}

	protected void UpdateMatAlpha(float _alpha)
	{
		foreach (Renderer renderer in this.Renderers)
		{
			foreach (Material material in renderer.materials)
			{
				material.SetFloat(NGUIDepth.AlphaId, _alpha);
			}
		}
	}

	protected override void OnInit()
	{
		base.RemoveFromPanel();
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
		base.Update();
		if (Application.isPlaying)
		{
			this.UpdateMatAlpha(0f);
		}
	}
}
