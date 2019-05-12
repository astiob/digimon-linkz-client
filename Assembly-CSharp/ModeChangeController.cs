using System;
using System.Collections;
using UnityEngine;

public class ModeChangeController : CutsceneControllerBase
{
	[Header("キャラクターのスタンド")]
	[SerializeField]
	private GameObject[] charaStand;

	[Header("スタンドの回転速度")]
	[SerializeField]
	private float[] standRollSpeed;

	[Header("UIカメラ")]
	[SerializeField]
	private GameObject camera2D;

	[Header("スフィア")]
	[SerializeField]
	private GameObject[] breakSphere;

	[SerializeField]
	[Header("スフィアの回転速度")]
	private float[] sphereSpeed;

	[SerializeField]
	[Header("青いデジタルなマテリアル")]
	private Material afterConversionMaterialA;

	[Header("黄色いデジタルなマテリアル")]
	[SerializeField]
	private Material afterConversionMaterialB;

	[SerializeField]
	[Header("カメラの回転速度")]
	private float cameraRollSpeed;

	[Header("カメラの親")]
	[SerializeField]
	private GameObject cameraRollObject;

	private bool cameraRollFlag;

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
		if (this.cameraRollFlag)
		{
			this.cameraRollObject.transform.Rotate(0f, this.cameraRollSpeed, 0f);
		}
		for (int i = 0; i <= this.charaStand.Length - 1; i++)
		{
			this.charaStand[i].transform.Rotate(new Vector3(0f, 0f, this.standRollSpeed[i]));
		}
		for (int j = 0; j <= this.breakSphere.Length - 1; j++)
		{
			this.breakSphere[j].transform.Rotate(new Vector3(0f, 0f, this.sphereSpeed[j]));
		}
	}

	private void OnCameraRoll()
	{
		this.cameraRollFlag = true;
	}

	private void OffCameraRoll()
	{
		this.cameraRollFlag = false;
	}

	private void MonsterCopier()
	{
		SkinnedMeshRenderer componentInChildren = this.monsB_instance.GetComponentInChildren<SkinnedMeshRenderer>();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(componentInChildren.gameObject);
		GameObject gameObject2 = componentInChildren.gameObject.transform.parent.gameObject;
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsB;
	}

	private void FadeOuter()
	{
		base.StartCoroutine(base.FadeoutCorutine(0.0196078438f));
	}

	private void MaterialConverterA()
	{
		base.OnMaterialChanger(this.afterConversionMaterialA, this.monsA_instance);
	}

	private void MaterialConverterB()
	{
		base.OnMaterialChanger(this.copyMaterial, this.monsB_instance);
	}

	private void OffMaterialConvertA()
	{
		this.monsA_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsA;
	}

	private void OffMaterialConvertB()
	{
		this.monsB_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsB;
	}

	private void OnAttackAnimation()
	{
		this.monsB_instance.GetComponent<CharacterParams>().PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
	}

	public void SoudPlayer1()
	{
		base.PlaySE("se_co_memorial_a_rs", false);
	}

	public void SoudPlayer2()
	{
		base.PlaySE("se_evo00_24k", false);
	}

	protected override IEnumerator NextPageBefore()
	{
		this.camera2D.SendMessage("fadeOut");
		global::Debug.Log("test");
		yield break;
	}

	protected override IEnumerator NextPageAfter()
	{
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
