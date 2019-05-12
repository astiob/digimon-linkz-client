using Cutscene;
using System;
using UnityEngine;

public sealed class TutorialController : CutsceneBase
{
	[SerializeField]
	private TutorialScriptAnimation scriptAnime;

	private Action endCallback;

	private bool isActiveEndAnimation;

	private void Finish()
	{
		if (this.endCallback != null)
		{
			this.endCallback();
		}
		UnityEngine.Object.Destroy(base.gameObject);
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

	public override void SetData(CutsceneDataBase noop)
	{
	}

	public void EndCutscene(Action action)
	{
		if (!this.isActiveEndAnimation)
		{
			this.isActiveEndAnimation = true;
			this.endCallback = action;
			this.fade.StartFadeOut(new Action(this.Finish));
		}
	}
}
