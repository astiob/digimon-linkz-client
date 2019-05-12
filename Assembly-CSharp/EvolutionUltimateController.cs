using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionUltimateController : CutsceneControllerBase
{
	[Header("キャラクターのスタンド")]
	[SerializeField]
	private GameObject[] charaStand;

	[SerializeField]
	[Header("スタンドの回転速度")]
	private float[] standRollSpeed;

	[SerializeField]
	[Header("UIカメラ")]
	private GameObject camera2D;

	[Header("3Dカメラ")]
	[SerializeField]
	private GameObject camera3D_1;

	[Header("黄色いデジタルなマテリアル")]
	[SerializeField]
	private Material afterConversionMaterialB;

	[Header("ライン時のマテリアル")]
	[SerializeField]
	private Material afterConversionMaterialC;

	[SerializeField]
	[Header("デジ文字螺旋")]
	private GameObject spiral;

	[Header("最後のカメラ")]
	[SerializeField]
	private GameObject lastCamera;

	public float rollSpeed = 1f;

	private Transform TargetPos;

	private bool spiralBRotatoFlag;

	private List<Material[]> materialsListA;

	private List<Material[]> materialsListB;

	private void Start()
	{
		this.copyMaterial = UnityEngine.Object.Instantiate<Material>(this.afterConversionMaterialB);
		this.monsA_instance = base.monsterInstantiater(this.monsA_instance, this.character1Parent, this.character1Params, 0);
		this.monsB_instance = base.monsterInstantiater(this.monsB_instance, this.character2Parent, this.character2Params, 1);
		Camera component = this.camera3D_1.GetComponent<Camera>();
		CutsceneControllerBase.SetBillBoardCamera(this.monsA_instance, component);
		CutsceneControllerBase.SetBillBoardCamera(this.monsB_instance, component);
		this.materialAlpha = this.copyMaterial.color;
	}

	protected override void UpdateChild()
	{
		if (this.spiralBRotatoFlag)
		{
			this.spiral.transform.Rotate(new Vector3(0f, 0f, -1f));
		}
		for (int i = 0; i <= this.charaStand.Length - 1; i++)
		{
			this.charaStand[i].transform.Rotate(new Vector3(0f, 0f, this.standRollSpeed[i]));
		}
	}

	private void spriralRotater()
	{
		this.spiralBRotatoFlag = true;
	}

	public void CharacterA_LineOn()
	{
		this.materialsListA = base.OnWireFrameRenderer(this.monsA_instance, this.afterConversionMaterialC);
	}

	public void CharacterB_LineOn()
	{
		this.materialsListB = base.OnWireFrameRenderer(this.monsB_instance, this.afterConversionMaterialC);
	}

	public void CharacterA_LineOff()
	{
		base.OffWireFrameRenderer(this.monsA_instance, this.materialsListA);
	}

	public void CharacterB_LineOff()
	{
		base.OffWireFrameRenderer(this.monsB_instance, null);
	}

	private void MaterialConverterB()
	{
		base.OnMaterialChanger(this.copyMaterial, this.monsB_instance);
	}

	private void MaterialResetB()
	{
		base.ResetMaterialRenderer(this.monsB_instance, this.materialsListB);
	}

	private void AttackAnimation()
	{
		this.monsB_instance.GetComponent<CharacterParams>().PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
	}

	private void FadeOuter()
	{
		base.StartCoroutine(base.FadeoutCorutine(0.0392156877f));
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

	private void cameraPositionAdjustment()
	{
		float num = this.monsB_instance.GetComponent<CharacterParams>().RootToCenterDistance();
		Vector3 vector = this.lastCamera.transform.position;
		num /= 2f;
		vector = vector.normalized;
		this.lastCamera.transform.localPosition = new Vector3(0f, 0f, num + 1.5f);
	}

	public void SoudPlayer1()
	{
		base.PlaySE("bt_504", false);
	}

	public void SoudPlayer2()
	{
		base.PlaySE("bt_115", false);
	}

	public void SoudPlayer3()
	{
		base.PlaySE("ev_127", false);
	}

	public void SoudPlayer4()
	{
		base.PlaySE("bt_519", false);
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
