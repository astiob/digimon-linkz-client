using System;
using System.Collections;
using UnityEngine;

public class JogressController : CutsceneControllerBase
{
	public int[] target2 = new int[0];

	private GameObject monsC_instance;

	[Header("進化後デジモンの親")]
	[SerializeField]
	private GameObject character3Parent;

	[Header("キャラクターのスタンド")]
	[SerializeField]
	private GameObject[] charaStand;

	[SerializeField]
	[Header("スタンドの回転速度")]
	private float[] standRollSpeed;

	[SerializeField]
	[Header("デジ文字リング")]
	private GameObject ringSet;

	[Header("UIカメラ")]
	[SerializeField]
	private GameObject camera2D;

	[Header("3Dカメラ")]
	[SerializeField]
	private GameObject camera3D_1;

	[SerializeField]
	private GameObject camera3D_2;

	private bool chaseFlag;

	public float rollSpeed = 1f;

	private CharacterParams character3Params;

	private Transform TargetPos;

	private Material m;

	private void Start()
	{
		this.monsA_instance = this.monsterBirth(this.monsA_instance, base.target[0]);
		this.monsA_instance = this.monsterBehavior(this.monsA_instance, this.character1Params, this.character1Parent);
		this.monsB_instance = this.monsterBirth(this.monsB_instance, this.target2[0]);
		this.monsB_instance = this.monsterBehavior(this.monsB_instance, this.character2Params, this.character2Parent);
		this.monsC_instance = this.monsterBirth(this.monsC_instance, base.target[1]);
		this.character3Params = this.monsC_instance.GetComponent<CharacterParams>();
		this.character3Params.PlayIdleAnimation();
		this.monsC_instance.transform.parent = this.character3Parent.transform;
		this.monsC_instance.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.elementsA = this.monsC_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials;
		this.m = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
	}

	private GameObject monsterBirth(GameObject mons, int i)
	{
		if (!this.debugMode)
		{
			mons = (GameObject)UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject("Characters/" + i + "/prefab", null, true));
		}
		else
		{
			mons = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/" + i + "/prefab"));
		}
		return mons;
	}

	private GameObject monsterBehavior(GameObject mon, CharacterParams cp, GameObject cparent)
	{
		cp = mon.GetComponent<CharacterParams>();
		Camera component = this.camera3D_1.GetComponent<Camera>();
		cp.Initialize(component);
		cp.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
		mon.transform.parent = cparent.transform;
		mon.transform.localPosition = new Vector3(0f, 0f, 0f);
		return mon;
	}

	protected override void UpdateChild()
	{
		for (int i = 0; i <= this.charaStand.Length - 1; i++)
		{
			this.charaStand[i].transform.Rotate(new Vector3(0f, 0f, this.standRollSpeed[i]));
		}
	}

	private void LateUpdate()
	{
		this.TargetPos = this.character3Params.characterCenterTarget.gameObject.transform;
		if (this.chaseFlag)
		{
			this.CameraLookAtTarget();
		}
	}

	private void ChaseFlagStarter()
	{
		this.chaseFlag = true;
	}

	private void ChaseFlagStopper()
	{
		this.chaseFlag = false;
	}

	private void CameraLookAtTarget()
	{
		this.camera3D_1.transform.LookAt(this.TargetPos);
		this.camera3D_2.transform.LookAt(this.TargetPos);
	}

	private void GetBaseMaterials()
	{
		this.elementsA = this.monsC_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials;
	}

	public void Character3_LineOn()
	{
		base.OnWireFrameRenderer(this.monsC_instance, this.m);
	}

	public void Character3_LineOff()
	{
		base.OffWireFrameRenderer(this.monsC_instance);
		this.monsC_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsA;
	}

	private void Character3MotionTrigger()
	{
		this.character3Params.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
	}

	private void RingSetPositionSetter()
	{
		this.ringSet.transform.position = this.character3Params.characterCenterTarget.gameObject.transform.position;
	}

	public void SoudPlayer1()
	{
		base.PlaySE("bt_643", false);
	}

	public void SoudPlayer2()
	{
		base.PlaySE("bt_507", false);
	}

	public void SoudPlayer3()
	{
		base.PlaySE("bt_115", false);
	}

	public void SoudPlayer4()
	{
		base.PlaySE("bt_549", false);
	}

	protected override IEnumerator NextPageBefore()
	{
		this.camera2D.SendMessage("fadeOut");
		yield break;
	}

	protected override IEnumerator NextPageAfter()
	{
		this.Character3_LineOff();
		this.chaseFlag = false;
		yield break;
	}
}
