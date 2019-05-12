using Cutscene;
using Cutscene.UI;
using System;
using UnityEngine;

public sealed class ModeChangeController : CutsceneControllerBase
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Camera subCamera;

	[SerializeField]
	private Transform[] circleList;

	[SerializeField]
	private float[] circleRotateSpeedList;

	[SerializeField]
	private Transform[] sphereList;

	[SerializeField]
	private float[] sphereRotateSpeedList;

	[SerializeField]
	private Transform mainCameraParent;

	[SerializeField]
	private float mainCameraRotateSpeed;

	[SerializeField]
	private Transform beforeMonsterParentTransform;

	[SerializeField]
	private Transform afterMonsterParentTransform;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private TouchScreenButton touchScreenButton;

	[SerializeField]
	private ModeChangeAnimationEvent animeEvent;

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
		if (this.animeEvent.IsRotateCamera())
		{
			this.mainCameraParent.Rotate(0f, this.mainCameraRotateSpeed, 0f);
		}
		for (int i = 0; i < this.circleList.Length; i++)
		{
			this.circleList[i].Rotate(0f, 0f, this.circleRotateSpeedList[i]);
		}
		for (int j = 0; j < this.sphereList.Length; j++)
		{
			this.sphereList[j].Rotate(0f, 0f, this.sphereRotateSpeedList[j]);
		}
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataModeChange cutsceneDataModeChange = data as CutsceneDataModeChange;
		if (cutsceneDataModeChange != null)
		{
			this.endCallback = cutsceneDataModeChange.endCallback;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(new Action(this.EndCutscene));
			this.touchScreenButton.Initialize();
			this.touchScreenButton.AddAction(new Action(this.EndCutscene));
			GameObject gameObject = CutsceneCommon.LoadMonsterModel(this.beforeMonsterParentTransform, cutsceneDataModeChange.beforeModelId);
			gameObject.transform.localPosition = Vector3.zero;
			GameObject gameObject2 = CutsceneCommon.LoadMonsterModel(this.afterMonsterParentTransform, cutsceneDataModeChange.afterModelId);
			gameObject2.transform.localPosition = Vector3.zero;
			CutsceneCommon.SetBillBoardCamera(gameObject, this.mainCamera);
			CutsceneCommon.SetBillBoardCamera(gameObject2, this.subCamera);
			this.animeEvent.Initialize(this.cutsceneSound, gameObject, gameObject2);
		}
	}
}
