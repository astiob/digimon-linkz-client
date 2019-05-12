using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class EvolutionController : CutsceneControllerBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Transform beforeMonsterParent;

	[SerializeField]
	private Transform afterMonsterParent;

	[SerializeField]
	private Transform[] circleList;

	[SerializeField]
	private float[] circleRotateSpeedList;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private EvolutionAnimationEvent animeEvent;

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
		for (int i = 0; i < this.circleList.Length; i++)
		{
			this.circleList[i].Rotate(0f, 0f, this.circleRotateSpeedList[i]);
		}
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataEvolution cutsceneDataEvolution = data as CutsceneDataEvolution;
		if (cutsceneDataEvolution != null)
		{
			this.endCallback = cutsceneDataEvolution.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(this.beforeMonsterParent, cutsceneDataEvolution.beforeModelId);
			CutsceneCommon.InitializeMonsterPosition(gameObject, cutsceneDataEvolution.beforeGrowStep);
			GameObject gameObject2 = CutsceneCommon.LoadMonsterModel(this.afterMonsterParent, cutsceneDataEvolution.afterModelId);
			CutsceneCommon.InitializeMonsterPosition(gameObject2, cutsceneDataEvolution.afterGrowStep);
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject2, this.mainCamera);
			this.animeEvent.Initialize(this.mainCamera.transform, this.cutsceneSound, gameObject, gameObject2);
		}
	}
}
