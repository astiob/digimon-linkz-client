using System;
using UnityEngine;

public sealed class BattleCameraObject : MonoBehaviour
{
	public Camera camera3D;

	public CameraPostEffect postEffect;

	public Light sunLight;

	public LightColorChanger sunLightColorChanger;

	public void Initialize()
	{
		if (this.camera3D == null)
		{
			global::Debug.LogError("camera3D が null です. Inspectorから設定してください.");
		}
		if (this.postEffect == null)
		{
			global::Debug.LogError("postEffect が null です. Inspectorから設定してください.");
		}
		if (this.sunLight == null)
		{
			global::Debug.LogError("sunLight が null です. Inspectorから設定してください.");
		}
		if (this.sunLightColorChanger == null)
		{
			global::Debug.LogError("sunLightColorChanger が null です. Inspectorから設定してください.");
		}
		this.sunLightColorChanger.SetLight(this.sunLight);
	}

	public void RemoveAllCachedObjects()
	{
		if (this.camera3D != null)
		{
			UnityEngine.Object.Destroy(this.camera3D.gameObject);
		}
		if (this.postEffect != null)
		{
			UnityEngine.Object.Destroy(this.postEffect.gameObject);
		}
		if (this.sunLight != null)
		{
			UnityEngine.Object.Destroy(this.sunLight.gameObject);
		}
	}
}
