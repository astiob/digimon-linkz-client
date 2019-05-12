using System;
using System.Collections;
using UnityEngine;

public sealed class GUICameraControll : MonoBehaviour
{
	private InputControll inputControl;

	private FarmCameraDrawArea farmCameraDrawArea;

	private Camera farmCamera;

	[NonSerialized]
	public Vector3 distanceToGround;

	private readonly float OFFSET_VIEW_POSITION_Y = 2f;

	private readonly float DISTANCE_TO_VIEW_POINT = 20f;

	private readonly float ORTHOGRAPHIC_SIZE_MIN = 3f;

	private readonly float ORTHOGRAPHIC_SIZE_MAX = 23.5f;

	private readonly float VIEW_ROTATION_X_MIN = 35f;

	private readonly float VIEW_ROTATION_X_MAX = 35f;

	private Vector3 viewPosition;

	private Vector3 viewRotation;

	private Vector3 targetCameraPosition;

	private float targetOrthographicSize;

	private Vector3 currentCameraPosition;

	private float currentOrthographicSize;

	private Vector3 vtmp;

	private const float SAVE_TIME = 2f;

	private float currentSaveTime;

	private bool saveFlg;

	private bool lastFarmCameraEnabled;

	private TutorialObserver tutorialObserver;

	private void Awake()
	{
		this.farmCamera = base.gameObject.GetComponent<Camera>();
		this.inputControl = base.gameObject.GetComponent<InputControll>();
		this.farmCameraDrawArea = base.GetComponent<FarmCameraDrawArea>();
		this.targetOrthographicSize = this.farmCamera.orthographicSize;
		this.viewRotation = base.transform.localEulerAngles;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y -= this.OFFSET_VIEW_POSITION_Y;
		base.transform.localPosition = localPosition;
		this.SetDistanceToGround();
		this.viewPosition = base.transform.localPosition + this.distanceToGround;
		this.UpdateCameraTargetLocator();
		this.UpdateCameraLocator(false);
		this.SetDistanceToGround();
	}

	private void Start()
	{
		this.LoadSetting();
	}

	private void Update()
	{
		this.UpdateTargetOrthographicSize();
		this.UpdateViewRotation();
		this.UpdateViewPosition();
		this.UpdateCameraTargetLocator();
		this.UpdateCameraLocator(true);
		this.UpdateSave();
	}

	private void UpdateTargetOrthographicSize()
	{
		float num = this.targetOrthographicSize;
		if (this.inputControl != null)
		{
			this.targetOrthographicSize -= this.inputControl.fScl * this.targetOrthographicSize;
		}
		this.targetOrthographicSize = Mathf.Min(this.targetOrthographicSize, this.ORTHOGRAPHIC_SIZE_MAX);
		this.targetOrthographicSize = Mathf.Max(this.targetOrthographicSize, this.ORTHOGRAPHIC_SIZE_MIN);
		if (this.targetOrthographicSize - num != 0f)
		{
			this.RequestSave();
		}
	}

	private void UpdateViewRotation()
	{
		float num = (this.currentOrthographicSize - this.ORTHOGRAPHIC_SIZE_MIN) / (this.ORTHOGRAPHIC_SIZE_MAX - this.ORTHOGRAPHIC_SIZE_MIN);
		this.viewRotation.x = (this.VIEW_ROTATION_X_MAX - this.VIEW_ROTATION_X_MIN) * num + this.VIEW_ROTATION_X_MIN;
	}

	private void UpdateViewPosition()
	{
		Vector3 vector = this.GetCameraMoveValue();
		if (null != this.farmCameraDrawArea)
		{
			vector = this.farmCameraDrawArea.AdjustCameraAdd(this.farmCamera, vector);
		}
		this.viewPosition += Quaternion.Euler(0f, this.viewRotation.y, 0f) * vector;
		if (vector.sqrMagnitude > 0f)
		{
			this.RequestSave();
		}
	}

	private Vector3 GetCameraMoveValue()
	{
		this.vtmp.Set(0f, 0f, 0f);
		if (this.inputControl != null)
		{
			this.vtmp.x = -this.inputControl.fXmove * this.currentOrthographicSize;
			float num = Mathf.Sin(this.viewRotation.x * 0.0174532924f);
			if (num == 0f)
			{
				num = 1f;
			}
			else
			{
				num = 1f / num;
			}
			this.vtmp.z = -this.inputControl.fYmove * this.currentOrthographicSize * num;
		}
		return this.vtmp;
	}

	private void UpdateCameraTargetLocator()
	{
		this.vtmp.x = 0f;
		this.vtmp.y = 0f;
		this.vtmp.z = -this.DISTANCE_TO_VIEW_POINT;
		this.vtmp = Quaternion.Euler(this.viewRotation) * this.vtmp;
		this.targetCameraPosition = this.viewPosition + this.vtmp;
		this.targetCameraPosition.y = this.targetCameraPosition.y + this.OFFSET_VIEW_POSITION_Y;
	}

	private void UpdateCameraLocator(bool Inertia)
	{
		if (!Inertia)
		{
			this.currentCameraPosition = this.targetCameraPosition;
			this.currentOrthographicSize = this.targetOrthographicSize;
		}
		else
		{
			this.currentCameraPosition.x = this.currentCameraPosition.x + (this.targetCameraPosition.x - this.currentCameraPosition.x) / 1.5f;
			this.currentCameraPosition.y = this.currentCameraPosition.y + (this.targetCameraPosition.y - this.currentCameraPosition.y) / 1.5f;
			this.currentCameraPosition.z = this.currentCameraPosition.z + (this.targetCameraPosition.z - this.currentCameraPosition.z) / 1.5f;
			this.currentOrthographicSize += (this.targetOrthographicSize - this.currentOrthographicSize) / 1.5f;
		}
		base.transform.localPosition = this.currentCameraPosition;
		this.farmCamera.orthographicSize = this.currentOrthographicSize;
	}

	public IEnumerator MoveCameraToLookAtPoint(Vector3 position, float duration)
	{
		Vector3 oldViewPosition = this.viewPosition;
		float to = 1.57079637f;
		float from = to * -1f;
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float rate = elapsedTime / duration;
			float t = this.GetSin01(from, to, rate);
			this.viewPosition = Vector3.Lerp(oldViewPosition, position, t);
			yield return null;
		}
		this.viewPosition = position;
		yield break;
	}

	private float GetSin01(float from, float to, float rate)
	{
		float f = Mathf.Lerp(from, to, Mathf.Clamp01(rate));
		float num = Mathf.Sin(f);
		return Mathf.Clamp01((num + 1f) * 0.5f);
	}

	public float CameraOrthographicSizeMax
	{
		get
		{
			return this.ORTHOGRAPHIC_SIZE_MAX;
		}
	}

	private void SetDistanceToGround()
	{
		float f = (90f - this.farmCamera.transform.localEulerAngles.x) * 0.0174532924f;
		float z = Mathf.Abs(this.farmCamera.transform.localPosition.y / Mathf.Cos(f));
		this.distanceToGround = this.farmCamera.transform.localRotation * new Vector3(0f, 0f, z);
	}

	private void RequestSave()
	{
		this.saveFlg = true;
		this.currentSaveTime = 0f;
	}

	private void UpdateSave()
	{
		if (this.IsTutorial())
		{
			return;
		}
		if (this.lastFarmCameraEnabled && !this.farmCamera.enabled)
		{
			this.SaveSetting();
		}
		this.lastFarmCameraEnabled = this.farmCamera.enabled;
		if (this.saveFlg)
		{
			this.currentSaveTime += Time.deltaTime;
			if (this.currentSaveTime >= 2f)
			{
				this.currentSaveTime = 0f;
				this.saveFlg = false;
				this.SaveSetting();
			}
		}
	}

	private bool IsTutorial()
	{
		if (this.tutorialObserver == null)
		{
			this.tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
			return this.tutorialObserver != null && this.tutorialObserver.isTutorial;
		}
		return this.tutorialObserver.isTutorial;
	}

	private void SaveSetting()
	{
		PlayerPrefs.SetFloat("FarmCameraPosX", this.viewPosition.x);
		PlayerPrefs.SetFloat("FarmCameraPosY", this.viewPosition.y);
		PlayerPrefs.SetFloat("FarmCameraPosZ", this.viewPosition.z);
		PlayerPrefs.SetFloat("FarmCameraOrthographicSize", this.targetOrthographicSize);
		PlayerPrefs.Save();
	}

	private void LoadSetting()
	{
		float @float = PlayerPrefs.GetFloat("FarmCameraPosX", this.viewPosition.x);
		float float2 = PlayerPrefs.GetFloat("FarmCameraPosY", this.viewPosition.y);
		float float3 = PlayerPrefs.GetFloat("FarmCameraPosZ", this.viewPosition.z);
		this.viewPosition = new Vector3(@float, float2, float3);
		this.targetOrthographicSize = PlayerPrefs.GetFloat("FarmCameraOrthographicSize", this.targetOrthographicSize);
	}
}
