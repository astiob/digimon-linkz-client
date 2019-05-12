using System;
using UnityEngine;

public class ScreenEffectBase : MonoBehaviour
{
	private float baseScreenW = 1140f;

	private float baseScreenH = 644f;

	private float realScreenW;

	private float realScreenH;

	private float fixedScreenW;

	private float fixedScreenH;

	protected float aspectRatio;

	public GameObject goEffect;

	public GameObject goCamera;

	private Camera camera;

	private RenderTexture renderTex;

	protected Material material;

	protected virtual void Awake()
	{
		this.realScreenW = (float)Screen.width;
		this.realScreenH = (float)Screen.height;
		global::Debug.Log("=================================================== ScreenEffectBase SCR_W = " + this.realScreenW.ToString());
		global::Debug.Log("=================================================== ScreenEffectBase SCR_H = " + this.realScreenH.ToString());
		float num = this.baseScreenW / this.baseScreenH;
		float num2 = this.realScreenW / this.realScreenH;
		if (num > num2)
		{
			this.fixedScreenW = this.baseScreenW;
			this.fixedScreenH = this.realScreenH * (this.baseScreenW / this.realScreenW);
		}
		else
		{
			this.fixedScreenW = this.realScreenW * (this.baseScreenH / this.realScreenH);
			this.fixedScreenH = this.baseScreenH;
		}
		this.aspectRatio = this.realScreenW / this.realScreenH;
		this.renderTex = new RenderTexture((int)this.fixedScreenW + 4, (int)this.fixedScreenH + 4, 16);
		this.renderTex.antiAliasing = 2;
		if (this.goCamera == null)
		{
			global::Debug.LogError("=================================================== ScreenEffectBase カメラがない");
		}
		this.camera = this.goCamera.GetComponent<Camera>();
		if (this.camera == null)
		{
			global::Debug.LogError("=================================================== ScreenEffectBase カメラがない");
		}
		this.camera.targetTexture = this.renderTex;
		if (this.goEffect == null)
		{
			global::Debug.LogError("=================================================== ScreenEffectBase エフェクト用スクリーンが無い");
		}
		MeshRenderer component = this.goEffect.GetComponent<MeshRenderer>();
		if (component == null)
		{
			global::Debug.LogError("=================================================== ScreenEffectBase MeshRendererがない");
		}
		this.material = component.material;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDestroy()
	{
		this.renderTex = null;
	}

	public RenderTexture GetRenderTexture()
	{
		return this.renderTex;
	}

	public float GetTextureWidth()
	{
		return this.fixedScreenW;
	}

	public float GetTextureHeight()
	{
		return this.fixedScreenH;
	}
}
