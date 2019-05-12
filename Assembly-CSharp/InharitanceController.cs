using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class InharitanceController : CutsceneControllerBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Transform[] circleList;

	[SerializeField]
	private float[] circleRotateSpeedList;

	[SerializeField]
	private Transform baseMonsterParentTransform;

	[SerializeField]
	private Transform materialMonsterParentTransform;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private InheritanceAnimationEvent animeEvent;

	private Action endCallback;

	private Transform cameraTarget;

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
		for (int i = 0; i < this.circleList.Length; i++)
		{
			this.circleList[i].Rotate(0f, 0f, this.circleRotateSpeedList[i]);
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
		CutsceneDataInheritance cutsceneDataInheritance = data as CutsceneDataInheritance;
		if (cutsceneDataInheritance != null)
		{
			this.endCallback = cutsceneDataInheritance.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			MaterialController[] componentsInChildren = base.GetComponentsInChildren<MaterialController>();
			foreach (MaterialController materialController in componentsInChildren)
			{
				materialController.isRealtimeUpdate = true;
			}
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(this.baseMonsterParentTransform, cutsceneDataInheritance.baseModelId);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			GameObject gameObject2 = CutsceneCommon.LoadMonsterModel(this.materialMonsterParentTransform, cutsceneDataInheritance.materialModelId);
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			CharacterParams component = gameObject.GetComponent<CharacterParams>();
			this.cameraTarget = component.characterFaceCenterTarget;
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject2, this.mainCamera);
			CharacterParams component2 = gameObject2.GetComponent<CharacterParams>();
			this.animeEvent.Initialize(this.cutsceneSound, component, component2);
		}
	}
}
