using System;
using TextureTimeScrollInternal;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[AddComponentMenu("Digimon Effects/Tools/Texture Time Scroll")]
public class TextureTimeScroll : MonoBehaviour
{
	public static int objectCount;

	[SerializeField]
	private int _materialIndex;

	[SerializeField]
	private TextureTimeScrollModClip[] _modClips = new TextureTimeScrollModClip[]
	{
		new TextureTimeScrollModClip()
	};

	[SerializeField]
	private bool _isRealtimeUpdate;

	private bool isDrawed;

	[HideInInspector]
	public Material instancedMaterial;

	private int modClipCount;

	private bool onAdded;

	public int materialIndex
	{
		get
		{
			return this._materialIndex;
		}
	}

	public Renderer renderer
	{
		get
		{
			return base.GetComponent<Renderer>();
		}
	}

	private void CountPlus()
	{
		if (!this.onAdded)
		{
			TextureTimeScroll.objectCount++;
		}
		this.onAdded = true;
	}

	private void CountMinus()
	{
		if (this.onAdded)
		{
			TextureTimeScroll.objectCount--;
		}
		this.onAdded = false;
	}

	private void OnDisable()
	{
		this.CountMinus();
	}

	private void OnDestroy()
	{
		this.CountMinus();
	}

	private void OnEnable()
	{
		this.CountPlus();
	}

	private void Start()
	{
		Renderer renderer = this.renderer;
		if (renderer.sharedMaterials.Length < 1)
		{
			global::Debug.LogError("Not Find Material This Renderer. (" + base.name + ")");
			base.enabled = false;
			return;
		}
		if (renderer.sharedMaterials.Length > this._materialIndex && renderer.sharedMaterials[this._materialIndex] != null)
		{
			this.instancedMaterial = base.GetComponent<Renderer>().materials[this._materialIndex];
			return;
		}
		global::Debug.LogError(string.Concat(new object[]
		{
			"Unmach Material Index Of This Renderer. [ ",
			this._materialIndex,
			" ] (",
			base.name,
			")"
		}));
		base.enabled = false;
	}

	private void OnWillRenderObject()
	{
		if (this._isRealtimeUpdate)
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
		this.RenderUpdate();
	}

	private void Update()
	{
		this.isDrawed = false;
		if (!this._isRealtimeUpdate)
		{
			return;
		}
		this.RenderUpdate();
	}

	private void RenderUpdate()
	{
		if (this.instancedMaterial == null)
		{
			return;
		}
		this.modClipCount = 0;
		while (this.modClipCount < this._modClips.Length)
		{
			this._modClips[this.modClipCount].SetFModedValue(this.instancedMaterial);
			this.modClipCount++;
		}
		this.isDrawed = true;
	}
}
