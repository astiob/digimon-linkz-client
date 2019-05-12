using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionUPController : CutsceneControllerBase
{
	[SerializeField]
	[Header("キャラクターのスタンド")]
	private GameObject[] charaStand;

	[Header("スタンドの回転速度")]
	[SerializeField]
	private float[] standRollSpeed;

	[Header("UIカメラ")]
	[SerializeField]
	private GameObject camera2D;

	[SerializeField]
	[Header("3Dカメラ")]
	private GameObject camera3D_1;

	public float rollSpeed = 1f;

	private bool isBaseAdjusted;

	private Transform TargetPos;

	private Material wireMaterial;

	private List<Material[]> materialsListA;

	private List<Material[]> materialsListB;

	private void Start()
	{
		this.monsA_instance = base.monsterInstantiater(this.monsA_instance, this.character1Parent, this.character1Params, 0);
		base.monsPosAdjustment(this.monsterLevelClass1, this.monsA_instance);
		this.monsB_instance = base.monsterInstantiater(this.monsB_instance, this.character2Parent, this.character2Params, 1);
		base.monsPosAdjustment(this.monsterLevelClass2, this.monsB_instance);
		this.character2Params = this.monsB_instance.GetComponent<CharacterParams>();
		Camera component = this.camera3D_1.GetComponent<Camera>();
		CutsceneControllerBase.SetBillBoardCamera(this.monsA_instance, component);
		CutsceneControllerBase.SetBillBoardCamera(this.monsB_instance, component);
		this.wireMaterial = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_222", 0f, false, true, null, -1, 1f);
	}

	protected override void UpdateChild()
	{
		this.monsA_instance.transform.Rotate(new Vector3(0f, this.rollSpeed, 0f));
		for (int i = 0; i <= this.charaStand.Length - 1; i++)
		{
			this.charaStand[i].transform.Rotate(new Vector3(0f, 0f, this.standRollSpeed[i]));
		}
	}

	private void LateUpdate()
	{
		if (!this.isBaseAdjusted)
		{
			Vector3 localPosition = this.monsA_instance.transform.parent.transform.localPosition;
			localPosition.y = 0f;
			this.monsA_instance.transform.parent.transform.localPosition = localPosition;
			Vector3 eulerAngles = this.monsB_instance.transform.parent.transform.localRotation.eulerAngles;
			this.monsB_instance.transform.parent.transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y - 47f, eulerAngles.z);
		}
	}

	public void CharacterA_LineOn()
	{
		this.materialsListA = base.OnWireFrameRenderer(this.monsA_instance, this.wireMaterial);
		this.materialsListB = base.OnWireFrameRenderer(this.monsB_instance, this.wireMaterial);
	}

	public void CharacterB_LineOn()
	{
	}

	public void CharacterA_LineOff()
	{
		base.OffWireFrameRenderer(this.monsA_instance, this.materialsListA);
	}

	public void CharacterB_LineOff()
	{
		base.OffWireFrameRenderer(this.monsB_instance, this.materialsListB);
	}

	private void AttackAnimation()
	{
		this.monsB_instance.GetComponent<CharacterParams>().PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
	}

	private void monsterBpositionAdjustment()
	{
		this.monsB_instance.transform.localPosition = Vector3.zero;
	}

	private void ChaseFlagStarter()
	{
		this.TargetPos = this.character2Params.characterFaceCenterTarget.gameObject.transform;
		this.camera3D_1.transform.LookAt(this.TargetPos);
	}

	public void SoudPlayer1()
	{
	}

	public void SoudPlayer2()
	{
	}

	public void SoudPlayer3()
	{
	}

	public void SoudPlayer4()
	{
	}

	protected override IEnumerator NextPageBefore()
	{
		this.camera2D.SendMessage("fadeOut");
		yield break;
	}

	protected override IEnumerator NextPageAfter()
	{
		this.CharacterA_LineOff();
		this.CharacterB_LineOff();
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
