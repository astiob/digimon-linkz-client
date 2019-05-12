using System;
using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
	public string targetName = "monsA";

	public float rotationSpeed = 2f;

	public int switchFlg;

	public float angle = 45f;

	private Transform[] points;

	private float interval = 16f;

	private float minDistance = 0.5f;

	private AnimationCurve fovCurve = AnimationCurve.Linear(1f, 30f, 10f, 30f);

	private bool autoChange = true;

	private Transform target;

	private Vector3 followPoint;

	private Vector3 focus = Vector3.zero;

	private Camera mainCamera;

	private Camera highCamera;

	[SerializeField]
	[Header("メインカメラ")]
	private GameObject obj_cam;

	[SerializeField]
	[Header("サブカメラ")]
	private GameObject obj_cam2;

	[SerializeField]
	[Header("カメラポジションルート")]
	private GameObject obj_camposroot;

	private void Start()
	{
		if (this.targetName.IndexOf("mons") != -1)
		{
			this.target = GameObject.Find(this.targetName).GetComponent<CharacterParams>().characterFaceCenterTarget;
		}
		else
		{
			this.target = GameObject.Find(this.targetName).transform;
		}
		this.followPoint = this.target.position;
		this.points = this.obj_camposroot.GetComponentsInChildren<Transform>();
		this.mainCamera = this.obj_cam.GetComponent<Camera>();
		this.highCamera = this.obj_cam2.GetComponent<Camera>();
		this.highCamera.enabled = false;
		if (this.autoChange)
		{
			this.StartAutoChange();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) || this.switchFlg > 0)
		{
			if (this.switchFlg == 1)
			{
				if (this.mainCamera.enabled)
				{
					this.mainCamera.enabled = false;
					this.highCamera.enabled = true;
				}
				else
				{
					this.mainCamera.enabled = true;
					this.highCamera.enabled = false;
				}
			}
			if (this.switchFlg == 2)
			{
				this.mainCamera.enabled = true;
				this.highCamera.enabled = false;
			}
			if (this.switchFlg == 3)
			{
				this.mainCamera.enabled = false;
				this.highCamera.enabled = true;
			}
			this.switchFlg = 0;
		}
	}

	private void LateUpdate()
	{
		if (this.targetName.IndexOf("mons") != -1)
		{
			GameObject gameObject = GameObject.Find(this.targetName);
			if (null != gameObject)
			{
				CharacterParams component = gameObject.GetComponent<CharacterParams>();
				if (null != component)
				{
					this.target = component.characterFaceCenterTarget;
				}
			}
		}
		else
		{
			this.target = GameObject.Find(this.targetName).transform;
		}
		this.followPoint = this.target.position;
		this.followPoint = new Vector3(this.followPoint.x, this.followPoint.y - 0.3f, this.followPoint.z);
		base.transform.LookAt(this.followPoint);
		this.highCamera.transform.RotateAround(this.focus, Vector3.up, -this.angle * Time.deltaTime);
		this.highCamera.transform.LookAt(new Vector3(0f, this.target.position.y, 0f));
	}

	public void ChangePosition(Transform destination, bool forceStable = false)
	{
		if (!base.enabled)
		{
			return;
		}
		base.transform.position = destination.position;
		float time = Vector3.Distance(this.target.position, base.transform.position);
		base.GetComponentInChildren<Camera>().fieldOfView = this.fovCurve.Evaluate(time);
	}

	private Transform ChooseAnotherPoint(Transform current)
	{
		Transform transform;
		float num;
		do
		{
			transform = this.points[UnityEngine.Random.Range(1, this.points.Length)];
			num = Vector3.Distance(transform.position, this.target.position);
		}
		while (!(transform != current) || num <= this.minDistance);
		return transform;
	}

	private IEnumerator AutoChange()
	{
		Transform i = this.points[UnityEngine.Random.Range(1, this.points.Length)];
		Transform current = i;
		for (;;)
		{
			this.ChangePosition(current, false);
			yield return new WaitForSeconds(this.interval);
			current = this.ChooseAnotherPoint(current);
		}
		yield break;
	}

	public void StartAutoChange()
	{
		base.StartCoroutine("AutoChange");
	}

	public void StopAutoChange()
	{
		base.StopCoroutine("AutoChange");
	}
}
