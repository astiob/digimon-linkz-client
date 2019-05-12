using System;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Tools/Material Controller")]
[RequireComponent(typeof(Renderer))]
public class MaterialController : MonoBehaviour
{
	private const float maxTint = 10f;

	public Color color = Color.white;

	public Vector2 uvScroll = Vector2.zero;

	[Range(0f, 10f)]
	public float tint;

	[Range(0f, 1f)]
	public float vertexColorLevel;

	public Vector2 uvOffset = Vector2.zero;

	public bool useTilingOverride;

	public Vector2 uvTiling = Vector2.one;

	[NonSerialized]
	public bool isRealtimeUpdate;

	private Material mat;

	private bool isDrawed;

	private Renderer renderer;

	private void Awake()
	{
		if (base.GetComponent<TextureTimeScroll>())
		{
			this.mat = base.GetComponent<TextureTimeScroll>().instancedMaterial;
		}
	}

	private void OnEnable()
	{
		if (this.renderer == null)
		{
			this.renderer = base.GetComponent<Renderer>();
			if (this.mat != null)
			{
				return;
			}
			this.mat = this.renderer.material;
		}
	}

	private void LateUpdate()
	{
		this.isDrawed = false;
		if (this.color.a == 0f)
		{
			this.renderer.enabled = false;
		}
		else
		{
			this.renderer.enabled = true;
		}
		if (this.isRealtimeUpdate)
		{
			this.UpdateMaterial();
		}
	}

	private void OnWillRenderObject()
	{
		if (this.isRealtimeUpdate)
		{
			return;
		}
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
		this.UpdateMaterial();
		this.isDrawed = true;
	}

	private void UpdateMaterial()
	{
		if (this.mat == null)
		{
			return;
		}
		this.mat.SetColor("_Color", this.color);
		this.mat.SetFloat("_UAnimSpeed", this.uvScroll.x);
		this.mat.SetFloat("_VAnimSpeed", this.uvScroll.y);
		this.mat.SetFloat("_TintLevel", Mathf.Clamp(this.tint, 0f, 10f));
		this.mat.SetFloat("_VertexColorLevel", Mathf.Clamp(this.vertexColorLevel, 0f, 1f));
		this.mat.SetTextureOffset("_MainTex", this.uvOffset);
		if (this.useTilingOverride)
		{
			this.mat.SetTextureScale("_MainTex", this.uvTiling);
		}
	}

	public Material GetMaterial()
	{
		return this.mat;
	}

	public void SetProperty(Color _color, Vector2 _uvScroll, float _tint, float _vertexColorLevel, Vector2 _uvOffset, Vector2 _uvScale)
	{
		this.color = _color;
		this.tint = _tint;
		this.vertexColorLevel = _vertexColorLevel;
		this.uvOffset = _uvOffset;
		if (this.useTilingOverride)
		{
			this.uvTiling = _uvScale;
		}
	}

	public void GetMaterialProperty()
	{
		this.renderer = base.GetComponent<Renderer>();
		if (this.renderer != null)
		{
			Material sharedMaterial = this.renderer.sharedMaterial;
			if (sharedMaterial.HasProperty("_MainTex"))
			{
				this.uvOffset = sharedMaterial.GetTextureOffset("_MainTex");
				this.uvTiling = sharedMaterial.GetTextureScale("_MainTex");
			}
			if (sharedMaterial.HasProperty("_Color"))
			{
				this.color = sharedMaterial.GetColor("_Color");
			}
			float x = 0f;
			float y = 0f;
			bool flag = false;
			if (sharedMaterial.HasProperty("_UAnimSpeed"))
			{
				x = sharedMaterial.GetFloat("_UAnimSpeed");
				flag = true;
			}
			if (sharedMaterial.HasProperty("_VAnimSpeed"))
			{
				y = sharedMaterial.GetFloat("_VAnimSpeed");
				flag = true;
			}
			if (flag)
			{
				this.uvScroll = new Vector2(x, y);
			}
			if (sharedMaterial.HasProperty("_TintLevel"))
			{
				this.tint = sharedMaterial.GetFloat("_TintLevel");
			}
			if (sharedMaterial.HasProperty("_VertexColorLevel"))
			{
				this.vertexColorLevel = sharedMaterial.GetFloat("_VertexColorLevel");
			}
		}
	}

	public bool ThisPropertyIsUAnimSpeed(string propertyName)
	{
		return propertyName.Equals("UAnimSpeed") || propertyName.Equals("_UAnimSpeed");
	}

	public bool ThisPropertyIsVAnimSpeed(string propertyName)
	{
		return propertyName.Equals("VAnimSpeed") || propertyName.Equals("_VAnimSpeed");
	}
}
