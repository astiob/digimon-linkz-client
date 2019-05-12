using System;
using UnityEngine;

[ExecuteInEditMode]
public class GUISpriteBase : MonoBehaviour
{
	[SerializeField]
	protected Vector2 scale_ = Vector2.one;

	[SerializeField]
	protected Vector2 shear_ = Vector2.zero;

	protected Vector2 ppfScale_ = Vector2.one;

	[SerializeField]
	protected Camera camera_;

	[SerializeField]
	protected GUISpriteBase.Anchor anchor_ = GUISpriteBase.Anchor.MidCenter;

	[SerializeField]
	protected Vector2 offset_ = Vector2.zero;

	[SerializeField]
	protected Rect boundingRect_ = new Rect(0f, 0f, 0f, 0f);

	protected MeshFilter meshFilter_;

	protected GUISpriteBase.UpdateFlags updateFlags_;

	public Vector2 scale
	{
		get
		{
			return this.scale_;
		}
		set
		{
			if (this.scale_ != value)
			{
				this.scale_ = value;
				this.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	public Vector2 shear
	{
		get
		{
			return this.shear_;
		}
		set
		{
			if (this.shear_ != value)
			{
				this.shear_ = value;
				this.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	public Vector2 ppfScale
	{
		get
		{
			return this.ppfScale_;
		}
		set
		{
			if (this.ppfScale_ != value)
			{
				this.ppfScale_ = value;
				this.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	protected void UpdateBoundRect(float _offsetX, float _offsetY, float _width, float _height)
	{
		float num = Mathf.Sign(_width);
		float num2 = Mathf.Sign(_height);
		this.boundingRect = new Rect(-_offsetX - num * _width * 0.5f, _offsetY - num2 * _height * 0.5f, num * _width, num2 * _height);
	}

	protected Bounds GetMeshBounds(float _offsetX, float _offsetY, float _width, float _height)
	{
		return new Bounds(new Vector3(-_offsetX, _offsetY, 0f), new Vector3(_width, _height, 0.2f));
	}

	public Camera renderCameraForPrefab
	{
		get
		{
			return this.camera_;
		}
	}

	public Camera renderCamera
	{
		get
		{
			if (this.camera_ != null)
			{
				return this.camera_;
			}
			if (Camera.main)
			{
				this.renderCamera = Camera.main;
			}
			return this.camera_;
		}
		set
		{
			if (value != this.camera_)
			{
				this.camera_ = value;
			}
		}
	}

	public GUISpriteBase.Anchor anchor
	{
		get
		{
			return this.anchor_;
		}
		set
		{
			if (this.anchor_ != value)
			{
				this.anchor_ = value;
				this.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	public Vector2 offset
	{
		get
		{
			return this.offset_;
		}
		set
		{
			if (this.offset_ != value)
			{
				this.offset_ = value;
				this.updateFlags |= GUISpriteBase.UpdateFlags.Vertex;
			}
		}
	}

	public Rect boundingRect
	{
		get
		{
			return this.boundingRect_;
		}
		protected set
		{
			this.boundingRect_ = value;
		}
	}

	public MeshFilter meshFilter
	{
		get
		{
			if (this.meshFilter_ == null)
			{
				this.meshFilter_ = base.GetComponent<MeshFilter>();
			}
			return this.meshFilter_;
		}
	}

	public GUISpriteBase.UpdateFlags updateFlags
	{
		get
		{
			return this.updateFlags_;
		}
		set
		{
			this.updateFlags_ = value;
			if (this.updateFlags_ != GUISpriteBase.UpdateFlags.None)
			{
				this.Commit();
			}
		}
	}

	protected void Awake()
	{
		if (this.camera_ == null)
		{
			this.camera_ = Camera.main;
		}
		this.meshFilter_ = base.GetComponent<MeshFilter>();
	}

	protected void OnDestroy()
	{
		if (this.meshFilter)
		{
			UnityEngine.Object.DestroyImmediate(this.meshFilter.sharedMesh, true);
		}
	}

	protected void OnEnable()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = true;
		}
	}

	protected void OnDisable()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = false;
		}
	}

	public virtual void Commit()
	{
	}

	[Flags]
	public enum UpdateFlags
	{
		None = 0,
		Vertex = 1,
		UV = 2,
		Color = 4,
		Text = 8,
		Index = 16
	}

	public enum Anchor
	{
		TopLeft,
		TopCenter,
		TopRight,
		MidLeft,
		MidCenter,
		MidRight,
		BotLeft,
		BotCenter,
		BotRight
	}
}
