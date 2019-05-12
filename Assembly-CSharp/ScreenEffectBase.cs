using System;
using UnityEngine;

public abstract class ScreenEffectBase : MonoBehaviour
{
	[SerializeField]
	private Camera effectCamera;

	[SerializeField]
	private MeshRenderer meshRenderer;

	[SerializeField]
	private GUISprite boardMesh;

	private RenderTexture effectRenderTexture;

	private float effectWidth;

	private float effectHeight;

	protected Material material;

	protected virtual void OnDestroy()
	{
		this.effectRenderTexture = null;
	}

	protected abstract void OnInitialize(float aspectRatio);

	public void Initialize(Vector2 size)
	{
		global::Debug.Assert(null != this.boardMesh, "ScreenEffectBase.boardMesh == null");
		global::Debug.Assert(null != this.meshRenderer, "ScreenEffectBase.meshRenderer == null");
		global::Debug.Assert(null != this.meshRenderer.material, "ScreenEffectBase.meshRenderer.material == null");
		global::Debug.Assert(null != this.effectCamera, "ScreenEffectBase.effectCamera == null");
		this.effectWidth = size.x;
		this.effectHeight = size.y;
		float aspectRatio = this.effectWidth / this.effectHeight;
		this.boardMesh.SetBoardSize(this.effectWidth, this.effectHeight);
		this.effectRenderTexture = new RenderTexture((int)this.effectWidth + 4, (int)this.effectHeight + 4, 16);
		this.effectRenderTexture.antiAliasing = 2;
		this.effectCamera.targetTexture = this.effectRenderTexture;
		this.material = this.meshRenderer.material;
		this.OnInitialize(aspectRatio);
	}

	public RenderTexture GetRenderTexture()
	{
		return this.effectRenderTexture;
	}

	public float GetTextureWidth()
	{
		return this.effectWidth;
	}

	public float GetTextureHeight()
	{
		return this.effectHeight;
	}
}
