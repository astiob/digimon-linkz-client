using System;
using System.Collections;
using UnityEngine;

public class EvolutionController : CutsceneControllerBase
{
	[SerializeField]
	[Header("キャラクターのスタンド")]
	private GameObject[] charaStand;

	[SerializeField]
	[Header("スタンドの回転速度")]
	private float[] standRollSpeed;

	[Header("UIカメラ")]
	[SerializeField]
	private GameObject camera2D;

	[Header("3Dカメラ")]
	[SerializeField]
	private GameObject camera3D_1;

	public float rollSpeed = 1f;

	private Transform TargetPos;

	private Material m;

	private void Start()
	{
		this.monsA_instance = base.monsterInstantiater(this.monsA_instance, this.character1Parent, this.character1Params, 0);
		base.monsPosAdjustment(this.monsterLevelClass1, this.monsA_instance);
		this.monsB_instance = base.monsterInstantiater(this.monsB_instance, this.character2Parent, this.character2Params, 1);
		base.monsPosAdjustment(this.monsterLevelClass2, this.monsB_instance);
		this.character2Params = this.monsB_instance.GetComponent<CharacterParams>();
		this.elementsA = base.MaterialKeeper(this.monsA_instance);
		this.elementsB = base.MaterialKeeper(this.monsB_instance);
		this.m = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
	}

	protected override void UpdateChild()
	{
		this.monsA_instance.transform.Rotate(new Vector3(0f, this.rollSpeed, 0f));
		for (int i = 0; i <= this.charaStand.Length - 1; i++)
		{
			this.charaStand[i].transform.Rotate(new Vector3(0f, 0f, this.standRollSpeed[i]));
		}
	}

	public void CharacterA_LineOn()
	{
		base.OnWireFrameRenderer(this.monsA_instance, this.m);
	}

	public void CharacterB_LineOn()
	{
		base.OnWireFrameRenderer(this.monsB_instance, this.m);
	}

	public void CharacterA_LineOff()
	{
		base.OffWireFrameRenderer(this.monsA_instance);
		this.monsA_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsA;
	}

	public void CharacterB_LineOff()
	{
		base.OffWireFrameRenderer(this.monsB_instance);
		this.monsB_instance.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elementsB;
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
