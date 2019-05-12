using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(MaterialController))]
public class CameraPostEffect : MonoBehaviour
{
	public static CameraPostEffect current;

	[Range(0f, 1f)]
	[SerializeField]
	private float _effectLevel;

	private MaterialController _materialController;

	private Camera _camera;

	private bool _isInitialized;

	public float effectLevel
	{
		get
		{
			return this._effectLevel;
		}
		set
		{
			this._effectLevel = value;
		}
	}

	private void Awake()
	{
		CameraPostEffect.current = this;
	}

	private void OnEnable()
	{
		if (this._isInitialized)
		{
			return;
		}
		this._materialController = base.GetComponentInChildren<MaterialController>();
		this._camera = base.GetComponent<Camera>();
		if (this._materialController != null && this._camera != null)
		{
			this._isInitialized = true;
			this._materialController.isRealtimeUpdate = true;
		}
	}

	private void LateUpdate()
	{
		this.OnEnable();
		if (!this._isInitialized)
		{
			return;
		}
		this._effectLevel = Mathf.Clamp01(this._effectLevel);
		if (this._effectLevel <= 0f)
		{
			this._camera.enabled = false;
			this._materialController.enabled = false;
		}
		else
		{
			this._camera.enabled = true;
			this._materialController.enabled = true;
			this._materialController.color.a = this._effectLevel;
		}
	}

	public static GameObject GetPostEffectPrefab()
	{
		return Resources.Load<GameObject>("PostEffect");
	}

	public void Off()
	{
		this._effectLevel = 0f;
		this.LateUpdate();
	}
}
