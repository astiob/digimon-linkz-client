using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class TrainingController : CutsceneBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TrainingScriptAnimation scriptAnime;

	private Action endCallback;

	private Transform cameraTransform;

	private Transform cameraRotatePosition;

	private Transform lookAtPosition;

	private Vector3 workVector3;

	private void EndCutscene()
	{
		this.fade.StartFadeOut(new Action(this.Finish));
		this.allSkipButton.Hide();
	}

	private void Finish()
	{
		this.cutsceneSound.StopAllSE();
		this.scriptAnime.StopAnimation();
		if (this.endCallback != null)
		{
			this.endCallback();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	protected override void OnStartCutscene()
	{
		if (!this.scriptAnime.IsPlaying())
		{
			this.scriptAnime.StartAnimation();
		}
	}

	protected override void OnUpdate()
	{
	}

	protected override void OnLateUpdate()
	{
		float y = this.cameraTransform.localPosition.y;
		if (this.cameraTransform.localPosition.y <= 1f)
		{
			y = 1f;
		}
		else
		{
			y = this.cameraTransform.localPosition.y - 3f * Time.deltaTime;
		}
		this.workVector3.x = this.cameraTransform.localPosition.x;
		this.workVector3.y = y;
		this.workVector3.z = this.cameraTransform.localPosition.z;
		this.cameraTransform.localPosition = this.workVector3;
		if (this.mainCamera.enabled)
		{
			this.cameraTransform.RotateAround(this.cameraRotatePosition.localPosition, Vector3.up, -45f * Time.deltaTime);
			this.cameraTransform.LookAt(this.lookAtPosition.position);
		}
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataTraining cutsceneDataTraining = data as CutsceneDataTraining;
		if (cutsceneDataTraining != null)
		{
			this.endCallback = cutsceneDataTraining.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(base.transform, cutsceneDataTraining.baseModelId);
			gameObject.transform.localPosition = Vector3.zero;
			CharacterParams component = gameObject.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			this.cameraTransform = this.mainCamera.transform;
			this.cameraRotatePosition = base.transform;
			this.lookAtPosition = component.characterCenterTarget;
			this.workVector3 = Vector3.zero;
			this.scriptAnime.Initialize(this.cutsceneSound, gameObject, cutsceneDataTraining.materialNum, new Action(this.EndCutscene));
		}
	}
}
