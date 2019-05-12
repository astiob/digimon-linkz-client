using Cutscene;
using Cutscene.EffectParts;
using System;
using UnityEngine;

public abstract class CutsceneBase : MonoBehaviour
{
	private bool isStart;

	[SerializeField]
	protected CutsceneFade fade;

	protected CutsceneSound cutsceneSound;

	private void Update()
	{
		if (this.isStart)
		{
			this.OnUpdate();
			this.fade.UpdateFade();
		}
	}

	private void LateUpdate()
	{
		if (this.isStart)
		{
			this.OnLateUpdate();
		}
	}

	protected abstract void OnStartCutscene();

	protected abstract void OnUpdate();

	protected virtual void OnLateUpdate()
	{
	}

	public virtual void Initialize()
	{
		this.cutsceneSound = new CutsceneSound();
		this.fade.Initialize();
	}

	public abstract void SetData(CutsceneDataBase data);

	public void StartCutscene()
	{
		this.isStart = true;
		this.fade.StartFadeIn(null);
		this.OnStartCutscene();
	}
}
