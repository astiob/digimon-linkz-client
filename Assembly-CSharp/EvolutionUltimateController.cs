using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class EvolutionUltimateController : CutsceneControllerBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Camera subCamera;

	[SerializeField]
	private Transform beforeMonsterParent;

	[SerializeField]
	private Transform afterMonsterParent;

	[SerializeField]
	private Transform[] circleList;

	[SerializeField]
	private float[] circleRotateSpeedList;

	[SerializeField]
	private Transform digimojiSpiral;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private EvolutionUltimateAnimationEvent animeEvent;

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
		if (this.animeEvent.IsRotateDigimoji())
		{
			this.digimojiSpiral.Rotate(0f, 0f, -1f);
		}
		for (int i = 0; i < this.circleList.Length; i++)
		{
			this.circleList[i].transform.Rotate(0f, 0f, this.circleRotateSpeedList[i]);
		}
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataEvolutionUltimate cutsceneDataEvolutionUltimate = data as CutsceneDataEvolutionUltimate;
		if (cutsceneDataEvolutionUltimate != null)
		{
			this.endCallback = cutsceneDataEvolutionUltimate.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(this.beforeMonsterParent, cutsceneDataEvolutionUltimate.beforeModelId);
			gameObject.transform.localPosition = Vector3.zero;
			GameObject gameObject2 = CutsceneCommon.LoadMonsterModel(this.afterMonsterParent, cutsceneDataEvolutionUltimate.afterModelId);
			gameObject2.transform.localPosition = Vector3.zero;
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject2, this.subCamera);
			this.animeEvent.Initialize(this.cutsceneSound, gameObject, gameObject2);
		}
	}
}
