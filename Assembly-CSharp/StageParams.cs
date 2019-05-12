using System;
using UnityEngine;
using UnityEngine.Rendering;

public class StageParams : MonoBehaviour
{
	[SerializeField]
	private Material skybox;

	[SerializeField]
	private Color skyColor = Color.black;

	[Range(0f, 8f)]
	[SerializeField]
	private float sunLightIntensity = 1f;

	[Range(0f, 8f)]
	[SerializeField]
	private float sunLightBounceLight = 1f;

	[SerializeField]
	private Vector3 sunLightEulerAngle = Vector3.zero;

	[SerializeField]
	private AmbientMode ambientMode;

	[SerializeField]
	private Color ambientLight = Color.white;

	[SerializeField]
	private Color ambientEquatorColor = Color.gray;

	[SerializeField]
	private Color ambientGroundColor = Color.black;

	[SerializeField]
	[Range(0f, 1f)]
	private float ambientIntensity = 1f;

	[SerializeField]
	private bool useCharacterShadow = true;

	[SerializeField]
	private Color[] _sunLightColor = new Color[]
	{
		Color.white
	};

	[SerializeField]
	private float _sunLightColorTransitionSpeed = 1f;

	[SerializeField]
	private Vector3 bigBossPosition = Vector3.zero;

	[SerializeField]
	private Vector3 bigBossRotation = Vector3.zero;

	public void TransformStage(int stageCameraType)
	{
		if (stageCameraType == 1)
		{
			base.transform.localPosition = this.bigBossPosition;
			base.transform.localRotation = Quaternion.Euler(this.bigBossRotation.x, this.bigBossRotation.y, this.bigBossRotation.z);
		}
		else
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}
	}

	public void SetHierarchyEnviroments(BattleCameraObject cameraObject)
	{
		RenderSettings.skybox = this.skybox;
		Light sunLight = cameraObject.sunLight;
		sunLight.intensity = this.sunLightIntensity;
		LightColorChanger sunLightColorChanger = cameraObject.sunLightColorChanger;
		sunLight.bounceIntensity = this.sunLightBounceLight;
		sunLightColorChanger.SetColors(this._sunLightColor);
		sunLightColorChanger.speed = this._sunLightColorTransitionSpeed;
		sunLightColorChanger.ResetColor();
		sunLightColorChanger.transform.rotation = Quaternion.Euler(this.sunLightEulerAngle);
		sunLightColorChanger.isEnable = true;
		Camera camera3D = cameraObject.camera3D;
		if (this.skybox != null)
		{
			camera3D.clearFlags = CameraClearFlags.Skybox;
		}
		else
		{
			camera3D.clearFlags = CameraClearFlags.Color;
			camera3D.backgroundColor = this.skyColor;
		}
		RenderSettings.ambientMode = this.ambientMode;
		AmbientMode ambientMode = RenderSettings.ambientMode;
		if (ambientMode != AmbientMode.Skybox)
		{
			if (ambientMode != AmbientMode.Trilight)
			{
				RenderSettings.ambientLight = this.ambientLight;
			}
			else
			{
				RenderSettings.ambientSkyColor = this.ambientLight;
				RenderSettings.ambientEquatorColor = this.ambientEquatorColor;
				RenderSettings.ambientGroundColor = this.ambientGroundColor;
			}
		}
		else
		{
			RenderSettings.ambientLight = this.ambientLight;
		}
		RenderSettings.ambientIntensity = this.ambientIntensity;
	}

	public bool UseCharacterShadow
	{
		get
		{
			return this.useCharacterShadow;
		}
	}
}
