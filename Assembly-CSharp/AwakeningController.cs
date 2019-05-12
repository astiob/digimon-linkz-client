using System;
using System.Collections;
using UnityEngine;

public class AwakeningController : CutsceneControllerBase
{
	[Header("3Dカメラ")]
	[SerializeField]
	private GameObject Camera;

	[Header("カメラの向き先")]
	[SerializeField]
	private GameObject CameraTarget;

	[Header("回ってるサークル小")]
	[SerializeField]
	private GameObject[] CirclesS = new GameObject[0];

	[Header("回ってるサークル中")]
	[SerializeField]
	private GameObject[] CirclesM = new GameObject[0];

	[SerializeField]
	[Header("回ってるサークル大")]
	private GameObject[] CirclesL = new GameObject[0];

	[Header("サークルの回転スピード")]
	[SerializeField]
	private float[] RotationSpeed = new float[3];

	[SerializeField]
	[Header("モンスターの生成位置")]
	private Vector3 monsPos;

	[SerializeField]
	private GameObject camera2D;

	private bool ChaseFlag;

	private bool RotateSpeedSlow;

	public new int target
	{
		get
		{
			if (base.target.Length < 1)
			{
				base.target = new int[1];
			}
			return base.target[0];
		}
		set
		{
			if (base.target.Length < 1)
			{
				base.target = new int[1];
			}
			base.target[0] = value;
		}
	}

	private void Start()
	{
		if (!this.debugMode)
		{
			this.monsA_instance = (GameObject)UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject("Characters/" + this.target + "/prefab", null, true));
		}
		else
		{
			this.monsA_instance = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/" + this.target + "/prefab"));
		}
		this.monsA_instance.transform.SetParent(base.transform);
		this.monsA_instance.transform.localPosition = new Vector3(this.monsPos.x, this.monsPos.y, this.monsPos.z);
		CharacterParams component = this.monsA_instance.GetComponent<CharacterParams>();
		component.PlayIdleAnimation();
		this.CameraTarget = component.characterCenterTarget.gameObject;
	}

	protected override void UpdateChild()
	{
		if (this.RotateSpeedSlow)
		{
			this.RotationSpeed[0] = 1f;
			this.RotationSpeed[1] = -1f;
			this.RotationSpeed[2] = 0.5f;
		}
		this.RotateSpeedControl();
	}

	public void RotateSpeedControl()
	{
		foreach (GameObject gameObject in this.CirclesS)
		{
			gameObject.transform.Rotate(new Vector3(0f, 0f, this.RotationSpeed[0]));
		}
		foreach (GameObject gameObject2 in this.CirclesM)
		{
			gameObject2.transform.Rotate(new Vector3(0f, 0f, this.RotationSpeed[1]));
		}
		foreach (GameObject gameObject3 in this.CirclesL)
		{
			gameObject3.transform.Rotate(new Vector3(0f, 0f, this.RotationSpeed[2]));
		}
	}

	private void ChaseFlagStarter()
	{
		this.ChaseFlag = true;
	}

	private void ChaseFlagStopper()
	{
		this.ChaseFlag = false;
	}

	private void LateUpdate()
	{
		if (this.ChaseFlag)
		{
			this.CameraChaise();
		}
	}

	public void CameraChaise()
	{
		this.Camera.transform.LookAt(this.CameraTarget.transform.position);
	}

	public void WinMotion()
	{
		this.monsA_instance.GetComponent<CharacterParams>().PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
	}

	public void SoudPlayer1()
	{
		base.PlaySE("se_216", false);
	}

	public void RotateSlower()
	{
		this.RotateSpeedSlow = true;
	}

	protected override IEnumerator NextPageBefore()
	{
		this.camera2D.SendMessage("fadeOut");
		yield break;
	}

	protected override float fadeWaitTime
	{
		get
		{
			return 1f;
		}
	}
}
