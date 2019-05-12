using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class JogressController : CutsceneControllerBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Transform[] circleList;

	[SerializeField]
	private float[] circleRotateSpeedList;

	[SerializeField]
	private Transform beforeMonsterParentTransform;

	[SerializeField]
	private Transform partnerMonsterParentTransform;

	[SerializeField]
	private Transform afterMonsterParentTransform;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private JogressAnimationEvent animeEvent;

	private Action endCallback;

	private CharacterParams afterMonsterCharaParam;

	private void EndCutscene()
	{
		this.fade.StartFadeOut(new Action(this.Finish));
		this.allSkipButton.Hide();
		this.touchScreenButton.Hide();
	}

	private void Finish()
	{
		this.cutsceneSound.StopAllSE();
		this.animeEvent.ResetMonsterMaterial();
		if (this.endCallback != null)
		{
			this.endCallback();
			this.endCallback = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
	}

	protected override void OnStartCutscene()
	{
		if (!this.animeEvent.IsPlaying())
		{
			this.animeEvent.StartAnimation();
		}
	}

	protected override void OnUpdate()
	{
		for (int i = 0; i < this.circleList.Length; i++)
		{
			this.circleList[i].Rotate(0f, 0f, this.circleRotateSpeedList[i]);
		}
	}

	protected override void OnLateUpdate()
	{
		if (this.animeEvent.IsCameraChase() && this.mainCamera.gameObject.activeSelf)
		{
			this.mainCamera.transform.LookAt(this.afterMonsterCharaParam.characterCenterTarget);
		}
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataJogress cutsceneDataJogress = data as CutsceneDataJogress;
		if (cutsceneDataJogress != null)
		{
			this.endCallback = cutsceneDataJogress.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(this.beforeMonsterParentTransform, cutsceneDataJogress.beforeModelId);
			gameObject.transform.localPosition = Vector3.zero;
			GameObject gameObject2 = CutsceneCommon.LoadMonsterModel(this.partnerMonsterParentTransform, cutsceneDataJogress.partnerModelId);
			gameObject2.transform.localPosition = Vector3.zero;
			GameObject gameObject3 = CutsceneCommon.LoadMonsterModel(this.afterMonsterParentTransform, cutsceneDataJogress.afterModelId);
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.SetActive(false);
			this.afterMonsterCharaParam = gameObject3.GetComponent<CharacterParams>();
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject2, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject3, this.mainCamera);
			this.animeEvent.Initialize(this.cutsceneSound, gameObject, gameObject2, gameObject3);
		}
	}
}
