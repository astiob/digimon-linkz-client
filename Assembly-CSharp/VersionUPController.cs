using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class VersionUPController : CutsceneControllerBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Transform beforeMonsterParentTransform;

	[SerializeField]
	private Transform afterMonsterParentTransform;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private VersionUpAnimationEvent animeEvent;

	private Action endCallback;

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
	}

	protected override void OnLateUpdate()
	{
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataVersionUp cutsceneDataVersionUp = data as CutsceneDataVersionUp;
		if (cutsceneDataVersionUp != null)
		{
			this.endCallback = cutsceneDataVersionUp.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(this.beforeMonsterParentTransform, cutsceneDataVersionUp.beforeModelId);
			gameObject.transform.localPosition = Vector3.zero;
			GameObject gameObject2 = CutsceneCommon.LoadMonsterModel(this.afterMonsterParentTransform, cutsceneDataVersionUp.afterModelId);
			gameObject2.transform.localPosition = Vector3.zero;
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject2, this.mainCamera);
			this.animeEvent.Initialize(this.cutsceneSound, this.mainCamera.transform, gameObject, gameObject2);
		}
	}
}
