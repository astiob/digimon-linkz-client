using System;
using UnityEngine;

public class CommonRender2D : MonoBehaviour
{
	[SerializeField]
	private Camera cam;

	[SerializeField]
	private int texWidth;

	[SerializeField]
	private int texHeight;

	[SerializeField]
	private UITexture uiTex;

	private RenderTexture renderTex;

	private void Start()
	{
		if (this.cam != null && this.uiTex != null)
		{
			this.renderTex = this.SetRenderTarget(this.texWidth, this.texHeight, 16);
			this.cam.targetTexture = this.renderTex;
			this.uiTex.mainTexture = this.renderTex;
		}
	}

	private void OnDestroy()
	{
		if (this.renderTex != null)
		{
			UnityEngine.Object.Destroy(this.renderTex);
			this.renderTex = null;
		}
	}

	private RenderTexture SetRenderTarget(int w, int h, int d = 16)
	{
		return new RenderTexture(w, h, d)
		{
			antiAliasing = 2
		};
	}

	public RenderTexture GetRenderTarget()
	{
		return this.renderTex;
	}
}
