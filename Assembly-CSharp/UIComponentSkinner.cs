using System;
using UnityEngine;
using UnityEngine.UIComponentSkinnerInternal;

public sealed class UIComponentSkinner : MonoBehaviour
{
	[SerializeField]
	private int _currentSkin;

	[SerializeField]
	private bool _isLock;

	[SerializeField]
	private UIComponentSkinnerObject[] _uiCompornentSkinnerObjects = new UIComponentSkinnerObject[]
	{
		new UIComponentSkinnerObject()
	};

	public int currentSkin
	{
		get
		{
			return this._currentSkin;
		}
	}

	public void SetSkins(int currentSkin)
	{
		if (this.isLock)
		{
			return;
		}
		this._currentSkin = Mathf.Clamp(currentSkin, 0, this.Length);
		this.ApplySkins();
	}

	public void ApplySkins()
	{
		this._uiCompornentSkinnerObjects[this._currentSkin].ApplySkins();
	}

	public bool isLock
	{
		get
		{
			return this._isLock;
		}
		set
		{
			this._isLock = value;
		}
	}

	public int Length
	{
		get
		{
			return this._uiCompornentSkinnerObjects.Length;
		}
	}

	public void Reset(int skinLength)
	{
		this._uiCompornentSkinnerObjects = new UIComponentSkinnerObject[skinLength];
		for (int i = 0; i < this._uiCompornentSkinnerObjects.Length; i++)
		{
			this._uiCompornentSkinnerObjects[i] = new UIComponentSkinnerObject();
		}
	}
}
