using System;
using System.Collections;
using UnityEngine;

public class MedalInheritController : CutsceneControllerBase
{
	[Header("最初のデジモンが立ってる土台")]
	[SerializeField]
	private GameObject[] circleStand = new GameObject[4];

	[Header("土台の回転速度")]
	public float[] rollSpeed = new float[4];

	[Header("カメラの注視点")]
	private Transform cameraTarget;

	[Header("カメラ1")]
	[SerializeField]
	private GameObject inharitanceCamera;

	[SerializeField]
	[Header("カメラ2")]
	private GameObject inharitanceCamera2;

	[SerializeField]
	[Header("カメラUI")]
	private GameObject camera2D;

	private MaterialController[] materialcontroller;

	private bool chaseFlag;

	private Vector3 TargetPos;

	private void Start()
	{
		bool flag = false;
		if (base.target == null)
		{
			flag = true;
		}
		else if (base.target.Length <= 0)
		{
			flag = true;
		}
		if (flag)
		{
			int num = (int)UnityEngine.Random.Range(100f, 200f);
			int num2 = (int)UnityEngine.Random.Range(100f, 200f);
			base.target = new int[3];
			base.target[0] = num;
			base.target[1] = num2;
			base.target[2] = num;
		}
		this.materialcontroller = base.GetComponentsInChildren<MaterialController>();
		foreach (MaterialController materialController in this.materialcontroller)
		{
			materialController.isRealtimeUpdate = true;
		}
		this.monsA_instance = base.monsterInstantiater(this.monsA_instance, this.character1Parent, this.character1Params, 0);
		base.monsPosAdjustment(this.monsterLevelClass1, this.monsA_instance);
		this.monsB_instance = base.monsterInstantiater(this.monsB_instance, this.character2Parent, this.character2Params, 1);
		base.monsPosAdjustment(this.monsterLevelClass2, this.monsB_instance);
		this.monsA_instance.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		this.monsB_instance.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		base.monsPosAdjustment(this.monsterLevelClass2, this.monsB_instance);
		this.character1Params = this.monsA_instance.GetComponent<CharacterParams>();
		this.character2Params = this.monsB_instance.GetComponent<CharacterParams>();
		this.character2Params.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
		this.cameraTarget = this.character1Params.characterFaceCenterTarget.gameObject.transform;
	}

	protected override void UpdateChild()
	{
		for (int i = 0; i <= this.circleStand.Length - 1; i++)
		{
			this.circleStand[i].transform.Rotate(new Vector3(0f, 0f, this.rollSpeed[i]));
		}
	}

	private void LateUpdate()
	{
		if (this.chaseFlag)
		{
			this.CameraChaise();
		}
	}

	private void CameraChaise()
	{
		this.inharitanceCamera.transform.LookAt(this.cameraTarget);
	}

	private void ChaseFlagStarter()
	{
		this.chaseFlag = true;
	}

	private void ChaseFlagStopper()
	{
		this.chaseFlag = false;
	}

	private void MotionCallerOfAttack()
	{
		this.monsA_instance.GetComponent<CharacterParams>().PlayAnimation(CharacterAnimationType.attacks, SkillType.Attack, 0, null, null);
	}

	public void WinMotion()
	{
		this.monsB_instance.GetComponent<CharacterParams>().PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
	}

	public void SoudPlayer1()
	{
		base.PlaySE("bt_605", false);
	}

	public void SoudPlayer2()
	{
		base.PlaySE("bt_504", false);
	}

	public void SoudPlayer3()
	{
		base.PlaySE("bt_530", false);
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
