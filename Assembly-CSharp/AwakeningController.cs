using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class AwakeningController : CutsceneControllerBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Transform monsterParent;

	[SerializeField]
	private Transform[] circleListS;

	[SerializeField]
	private Transform[] circleListM;

	[SerializeField]
	private Transform[] circleListL;

	[SerializeField]
	private float[] rotateHighSpeed;

	[SerializeField]
	private float[] rotateLowSpeed;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private AwakeningAnimationEvent animeEvent;

	private Action endCallback;

	private Transform cameraTarget;

	private void CircleRotation(Transform[] circleList, float rotSpeed)
	{
		for (int i = 0; i < circleList.Length; i++)
		{
			circleList[i].Rotate(0f, 0f, rotSpeed);
		}
	}

	private void EndCutscene()
	{
		this.fade.StartFadeOut(new Action(this.Finish));
		this.allSkipButton.Hide();
		this.touchScreenButton.Hide();
	}

	private void Finish()
	{
		this.cutsceneSound.StopAllSE();
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
		if (this.animeEvent.IsCircleSlowRotate())
		{
			this.CircleRotation(this.circleListS, this.rotateLowSpeed[0]);
			this.CircleRotation(this.circleListM, this.rotateLowSpeed[1]);
			this.CircleRotation(this.circleListL, this.rotateLowSpeed[2]);
		}
		else
		{
			this.CircleRotation(this.circleListS, this.rotateHighSpeed[0]);
			this.CircleRotation(this.circleListM, this.rotateHighSpeed[1]);
			this.CircleRotation(this.circleListL, this.rotateHighSpeed[2]);
		}
	}

	protected override void OnLateUpdate()
	{
		if (this.animeEvent.IsCameraChase())
		{
			this.mainCamera.transform.LookAt(this.cameraTarget);
		}
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataAwakening cutsceneDataAwakening = data as CutsceneDataAwakening;
		if (cutsceneDataAwakening != null)
		{
			this.endCallback = cutsceneDataAwakening.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(this.monsterParent, cutsceneDataAwakening.modelId);
			gameObject.transform.localPosition = Vector3.zero;
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CharacterParams component = gameObject.GetComponent<CharacterParams>();
			this.cameraTarget = component.characterCenterTarget;
			this.animeEvent.Initialize(this.cutsceneSound, component);
		}
	}
}
