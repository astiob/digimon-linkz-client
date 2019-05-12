using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class FusionController : CutsceneBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private FusionScriptAnimation scriptAnime;

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
		this.scriptAnime.StopAnimation();
		if (this.endCallback != null)
		{
			this.endCallback();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
		GC.Collect();
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

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataFusion cutsceneDataFusion = data as CutsceneDataFusion;
		if (cutsceneDataFusion != null)
		{
			base.transform.localPosition = new Vector3(0f, 10f, 0f);
			this.endCallback = cutsceneDataFusion.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(base.transform, cutsceneDataFusion.baseModelId);
			gameObject.transform.localPosition = new Vector3(-1.5f, 0f, 0f);
			GameObject gameObject2 = CutsceneCommon.LoadMonsterModel(base.transform, cutsceneDataFusion.materialModelId);
			gameObject2.transform.localPosition = new Vector3(1.5f, 0f, 0f);
			CharacterParams component = gameObject.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			component = gameObject2.GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			GameObject gameObject3 = CutsceneCommon.LoadMonsterModel(base.transform, cutsceneDataFusion.eggModelId);
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.SetActive(false);
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject2, this.mainCamera);
			this.scriptAnime.Initialize(this.cutsceneSound, base.transform.position, gameObject, gameObject2, gameObject3, cutsceneDataFusion.upArousal, this.allSkipButton.gameObject);
		}
	}
}
