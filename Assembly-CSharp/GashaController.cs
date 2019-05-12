using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public class GashaController : CutsceneBase
{
	[SerializeField]
	private Camera subCamera;

	[SerializeField]
	private Transform rareSignParticle;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private GashaNextButton gashaNextButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private GashaScriptAnimation scriptAnime;

	private Action endCallback;

	private void EndCutscene()
	{
		this.fade.StartFadeOut(new Action(this.Finish));
		this.allSkipButton.Hide();
		this.gashaNextButton.Hide();
		this.touchScreenButton.Hide();
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
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataGasha cutsceneDataGasha = data as CutsceneDataGasha;
		if (cutsceneDataGasha != null)
		{
			this.endCallback = cutsceneDataGasha.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			this.scriptAnime.Initialize(this.cutsceneSound, this.subCamera, this.rareSignParticle, cutsceneDataGasha.modelIdList, cutsceneDataGasha.growStepList);
		}
	}
}
