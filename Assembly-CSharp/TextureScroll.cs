using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[AddComponentMenu("Digimon Effects/Tools/Texture Scroll")]
public class TextureScroll : MonoBehaviour
{
	[SerializeField]
	private int _materialIndex;

	[SerializeField]
	private Vector2 _scroll;

	private Material _material;

	private Vector2 _scrollOffset;

	private bool isDrawed;

	private void OnEnable()
	{
		if (this._material == null)
		{
			this._material = base.GetComponent<Renderer>().materials[this._materialIndex];
		}
		this._scrollOffset = Vector2.zero;
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
		if (this._material == null)
		{
			return;
		}
		if (this.isDrawed)
		{
			return;
		}
		if (!this._material.HasProperty("_MainTex"))
		{
			return;
		}
		this._scrollOffset += this._scroll * Time.deltaTime;
		this._scrollOffset.Set(this._scrollOffset.x % 1f, this._scrollOffset.y % 1f);
		this._material.SetTextureOffset("_MainTex", this._scrollOffset);
		this.isDrawed = true;
	}
}
