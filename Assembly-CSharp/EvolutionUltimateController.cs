using System;
using System.Collections;
using UnityEngine;

public class EvolutionUltimateController : CutsceneControllerBase
{
	[SerializeField]
	[Header("キャラクターのスタンド")]
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

	[SerializeField]
	[Header("黄色いデジタルなマテリアル")]
	private Material afterConversionMaterialB;

	[SerializeField]
	[Header("ライン時のマテリアル")]
	private Material afterConversionMaterialC;

	[Header("デジ文字螺旋")]
	[SerializeField]
	private GameObject spiral;

	[Header("最後のカメラ")]
	[SerializeField]
	private GameObject lastCamera;

	public float rollSpeed = 1f;

	private Transform TargetPos;

	private bool spiralBRotatoFlag;

	private void Start()
	{
		this.copyMaterial = UnityEngine.Object.Instantiate<Material>(this.afterConversionMaterialB);
		this.monsA_instance = base.monsterInstantiater(this.monsA_instance, this.character1Parent, this.character1Params, 0);
		this.monsB_instance = base.monsterInstantiater(this.monsB_instance, this.character2Parent, this.character2Params, 1);
		this.elementsA = this.monsA_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials;
		this.elementsB = this.monsB_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials;
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
		base.OnWireFrameRenderer(this.monsA_instance, this.afterConversionMaterialC);
	}

	public void CharacterB_LineOn()
	{
		base.OnWireFrameRenderer(this.monsB_instance, this.afterConversionMaterialC);
	}

	public void CharacterA_LineOff()
	{
		base.OffWireFrameRenderer(this.monsA_instance);
		this.monsA_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsA;
	}

	public void CharacterB_LineOff()
	{
		base.OffWireFrameRenderer(this.monsB_instance);
	}

	private void MaterialConverterB()
	{
		base.OnMaterialChanger(this.copyMaterial, this.monsB_instance);
	}

	private void MaterialResetB()
	{
		this.monsB_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsB;
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
