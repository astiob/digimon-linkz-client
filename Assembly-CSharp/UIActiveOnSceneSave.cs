using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIActiveOnSceneSave : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _needActiveUiObjects = new GameObject[0];

	[SerializeField]
	private bool[] _previousActiveSelf = new bool[0];

	private bool _isLoaded;

	public GameObject[] needActiveUiObjects
	{
		get
		{
			return this._needActiveUiObjects;
		}
	}

	public void GetCurrentActiveSelf()
	{
		this._previousActiveSelf = new bool[this._needActiveUiObjects.Length];
		for (int i = 0; i < this._previousActiveSelf.Length; i++)
		{
			this._previousActiveSelf[i] = this._needActiveUiObjects[i].activeSelf;
		}
	}

	public void SetCurrentActiveSelf()
	{
		for (int i = 0; i < this._previousActiveSelf.Length; i++)
		{
			this._needActiveUiObjects[i].SetActive(this._previousActiveSelf[i]);
		}
	}

	public void SetAllActive()
	{
		for (int i = 0; i < this._previousActiveSelf.Length; i++)
		{
			this._needActiveUiObjects[i].SetActive(true);
		}
	}

	private void Awake()
	{
		UnityEngine.Object.Destroy(this);
	}

	private void Update()
	{
	}
}
