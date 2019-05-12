using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class GUISprite : GUISpriteBase
{
	public string textureGUID = string.Empty;

	public bool trimTexture = true;

	public Rect trimUV = new Rect(0f, 0f, 1f, 1f);

	[SerializeField]
	protected bool useTextureOffset_ = true;

	[SerializeField]
	protected bool divideUD_;

	[SerializeField]
	protected bool divideLR_;

	[SerializeField]
	protected Color color_ = Color.white;

	[SerializeField]
	protected bool customSize_;

	[SerializeField]
	protected float width_ = 1f;

	[SerializeField]
	protected float height_ = 1f;

	public bool useTextureOffset
	{
		get
		{
			return this.useTextureOffset_;
		}
		set
		{
			if (this.useTextureOffset_ != value)
			{
				this.useTextureOffset_ = value;
				base.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	public bool divideUD
	{
		get
		{
			return this.divideUD_;
		}
		set
		{
			if (this.divideUD_ != value)
			{
				this.divideUD_ = value;
				base.updateFlags |= GUISpriteBase.UpdateFlags.UV;
			}
		}
	}

	public bool divideLR
	{
		get
		{
			return this.divideLR_;
		}
		set
		{
			if (this.divideLR_ != value)
			{
				this.divideLR_ = value;
				base.updateFlags |= GUISpriteBase.UpdateFlags.UV;
			}
		}
	}

	public Color color
	{
		get
		{
			return this.color_;
		}
		set
		{
			if (this.color_ != value)
			{
				this.color_ = value;
				base.updateFlags |= GUISpriteBase.UpdateFlags.Color;
			}
		}
	}

	public bool customSize
	{
		get
		{
			return this.customSize_;
		}
		set
		{
			if (this.customSize_ != value)
			{
				this.customSize_ = value;
				if (!this.customSize_)
				{
					Texture mainTexture = base.gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture;
					float num = this.trimUV.width * (float)mainTexture.width;
					float num2 = this.trimUV.height * (float)mainTexture.height;
					if (num != this.width_ || num2 != this.height_)
					{
						this.width_ = num;
						this.height_ = num2;
						base.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
					}
				}
			}
		}
	}

	public float width
	{
		get
		{
			return this.width_;
		}
		set
		{
			if (this.width_ != value)
			{
				this.width_ = value;
				base.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	public float height
	{
		get
		{
			return this.height_;
		}
		set
		{
			if (this.height_ != value)
			{
				this.height_ = value;
				base.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	public bool isHFlipped
	{
		get
		{
			return this.scale_.x < 0f;
		}
	}

	public bool isVFlipped
	{
		get
		{
			return this.scale_.y < 0f;
		}
	}

	public void CalculateVertex(out float _x, out float _y, float _widthScaled, float _heightScaled, float _col, float _row, float _offsetX, float _offsetY)
	{
		_x = _widthScaled * (_col - 0.5f);
		_y = _heightScaled * (0.5f - _row);
		_x -= _offsetX;
		_y += _offsetY;
		float num = _x;
		_x += _y * this.shear_.x;
		_y += num * this.shear_.y;
	}

	public virtual void UpdateMesh(Mesh _mesh)
	{
		if ((base.updateFlags & GUISpriteBase.UpdateFlags.Vertex) != GUISpriteBase.UpdateFlags.None)
		{
			Vector2 vector = new Vector2(this.scale_.x * this.ppfScale_.x, this.scale_.y * this.ppfScale_.y);
			float num = this.width_ * vector.x * 0.5f;
			float num2 = this.height_ * vector.y * 0.5f;
			Vector3[] array = new Vector3[4];
			Vector3[] array2 = new Vector3[4];
			float num5;
			float num6;
			if (this.useTextureOffset_)
			{
				float num3 = 0f;
				float num4 = 0f;
				Rect rect = new Rect(0f, 0f, 1f, 1f);
				if (base.gameObject.GetComponent<Renderer>().sharedMaterial != null)
				{
					Texture mainTexture = base.gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture;
					num3 = (float)mainTexture.width * vector.x;
					num4 = (float)mainTexture.height * vector.y;
					rect = new Rect(this.trimUV.x * num3, (1f - this.trimUV.height - this.trimUV.y) * num4, this.trimUV.width * num3, this.trimUV.height * num4);
				}
				switch (this.anchor_)
				{
				case GUISpriteBase.Anchor.TopLeft:
					num5 = -num - rect.x;
					num6 = -num2 - rect.y;
					break;
				case GUISpriteBase.Anchor.TopCenter:
					num5 = (num3 - rect.width) * 0.5f - rect.x;
					num6 = -num2 - rect.y;
					break;
				case GUISpriteBase.Anchor.TopRight:
					num5 = num + num3 - rect.xMax;
					num6 = -num2 - rect.y;
					break;
				case GUISpriteBase.Anchor.MidLeft:
					num5 = -num - rect.x;
					num6 = (num4 - rect.height) * 0.5f - rect.y;
					break;
				case GUISpriteBase.Anchor.MidCenter:
					num5 = (num3 - rect.width) * 0.5f - rect.x;
					num6 = (num4 - rect.height) * 0.5f - rect.y;
					break;
				case GUISpriteBase.Anchor.MidRight:
					num5 = num + num3 - rect.xMax;
					num6 = (num4 - rect.height) * 0.5f - rect.y;
					break;
				case GUISpriteBase.Anchor.BotLeft:
					num5 = -num - rect.x;
					num6 = num2 + num4 - rect.yMax;
					break;
				case GUISpriteBase.Anchor.BotCenter:
					num5 = (num3 - rect.width) * 0.5f - rect.x;
					num6 = num2 + num4 - rect.yMax;
					break;
				case GUISpriteBase.Anchor.BotRight:
					num5 = num + num3 - rect.xMax;
					num6 = num2 + num4 - rect.yMax;
					break;
				default:
					num5 = (num3 - rect.width) * 0.5f - rect.x;
					num6 = (num4 - rect.height) * 0.5f - rect.y;
					break;
				}
			}
			else
			{
				switch (this.anchor_)
				{
				case GUISpriteBase.Anchor.TopLeft:
					num5 = -num;
					num6 = -num2;
					break;
				case GUISpriteBase.Anchor.TopCenter:
					num5 = 0f;
					num6 = -num2;
					break;
				case GUISpriteBase.Anchor.TopRight:
					num5 = num;
					num6 = -num2;
					break;
				case GUISpriteBase.Anchor.MidLeft:
					num5 = -num;
					num6 = 0f;
					break;
				case GUISpriteBase.Anchor.MidCenter:
					num5 = 0f;
					num6 = 0f;
					break;
				case GUISpriteBase.Anchor.MidRight:
					num5 = num;
					num6 = 0f;
					break;
				case GUISpriteBase.Anchor.BotLeft:
					num5 = -num;
					num6 = num2;
					break;
				case GUISpriteBase.Anchor.BotCenter:
					num5 = 0f;
					num6 = num2;
					break;
				case GUISpriteBase.Anchor.BotRight:
					num5 = num;
					num6 = num2;
					break;
				default:
					num5 = 0f;
					num6 = 0f;
					break;
				}
			}
			num5 -= this.offset_.x;
			num6 += this.offset_.y;
			float num7 = 9999f;
			float num8 = 9999f;
			float num9 = -9999f;
			float num10 = -9999f;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					int num11 = i * 2 + j;
					float num12;
					float num13;
					this.CalculateVertex(out num12, out num13, this.width_ * vector.x, this.height_ * vector.y, (float)j, (float)i, num5, num6);
					array[num11] = new Vector3(num12, num13, 0f);
					array2[num11] = new Vector3(0f, 0f, -1f);
					if (num12 < num7)
					{
						num7 = num12;
					}
					else if (num12 > num9)
					{
						num9 = num12;
					}
					if (num13 < num8)
					{
						num8 = num13;
					}
					else if (num13 > num10)
					{
						num10 = num13;
					}
				}
			}
			float width = num9 - num7;
			float height = num10 - num8;
			_mesh.vertices = array;
			_mesh.normals = array2;
			_mesh.bounds = base.GetMeshBounds(num5, num6, width, height);
			base.UpdateBoundRect(num5, num6, width, height);
		}
		if ((base.updateFlags & GUISpriteBase.UpdateFlags.UV) != GUISpriteBase.UpdateFlags.None)
		{
			Vector2[] array3 = new Vector2[4];
			float x = this.trimUV.x;
			float y = this.trimUV.y;
			float xMax = this.trimUV.xMax;
			float yMax = this.trimUV.yMax;
			array3[0] = new Vector2(x, yMax);
			array3[1] = new Vector2(xMax, yMax);
			array3[2] = new Vector2(x, y);
			array3[3] = new Vector2(xMax, y);
			_mesh.uv = array3;
		}
		if ((base.updateFlags & GUISpriteBase.UpdateFlags.Color) != GUISpriteBase.UpdateFlags.None)
		{
			Color[] array4 = new Color[4];
			for (int k = 0; k < 4; k++)
			{
				array4[k] = this.color_;
			}
			_mesh.colors = array4;
		}
		if ((base.updateFlags & GUISpriteBase.UpdateFlags.Index) != GUISpriteBase.UpdateFlags.None)
		{
			_mesh.triangles = new int[]
			{
				0,
				1,
				2,
				2,
				1,
				3
			};
		}
		base.updateFlags = GUISpriteBase.UpdateFlags.None;
	}

	public void ForceUpdateMesh(Mesh _mesh)
	{
		if (_mesh == null)
		{
			return;
		}
		_mesh.Clear();
		base.updateFlags = (GUISpriteBase.UpdateFlags.Vertex | GUISpriteBase.UpdateFlags.UV | GUISpriteBase.UpdateFlags.Color | GUISpriteBase.UpdateFlags.Index);
		this.UpdateMesh(_mesh);
	}

	public override void Commit()
	{
		if (base.meshFilter && this.meshFilter_.sharedMesh != null)
		{
			this.UpdateMesh(this.meshFilter_.sharedMesh);
		}
	}

	protected new void OnEnable()
	{
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	protected new void Awake()
	{
		base.Awake();
		if (base.gameObject.GetComponent<Renderer>().sharedMaterial && base.gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture != null && base.meshFilter)
		{
			this.meshFilter_.mesh = new Mesh();
			this.meshFilter_.sharedMesh.hideFlags = HideFlags.DontSave;
			this.ForceUpdateMesh(this.meshFilter_.sharedMesh);
		}
	}

	private void Update()
	{
		if (this.meshFilter_ != null && this.meshFilter_.sharedMesh != null)
		{
			this.ForceUpdateMesh(this.meshFilter_.sharedMesh);
		}
	}

	public void Clear()
	{
		if (base.gameObject.GetComponent<Renderer>() != null)
		{
			base.gameObject.GetComponent<Renderer>().sharedMaterial = null;
		}
		if (base.meshFilter)
		{
			UnityEngine.Object.DestroyImmediate(this.meshFilter_.sharedMesh, true);
			this.meshFilter_.sharedMesh = null;
		}
	}

	public void HFlip()
	{
		this.scale_.x = -this.scale_.x;
		base.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
	}

	public void VFlip()
	{
		this.scale_.y = -this.scale_.y;
		base.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
	}
}
