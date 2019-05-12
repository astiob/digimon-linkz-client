using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_PicturebookDetail : CMD
{
	[SerializeField]
	private GameObject uiNop;

	[SerializeField]
	private UILabel displayModeButtonLabel;

	[SerializeField]
	private PicturebookDetailStatus oneSkillStatus;

	[SerializeField]
	private PicturebookDetailStatus twoSkillStatus;

	[SerializeField]
	private UITexture modelUiTex;

	[SerializeField]
	private GameObject headerObj;

	private static Action onClosedPanel;

	private static MonsterData displayMonsterData;

	private CharacterParams monsterParam;

	private CMD_PicturebookDetail.DISPLAY_MODE displayMode;

	private PinchInOut pinch = new PinchInOut();

	private CommonRender3DRT render3DRT;

	private RenderTexture renderTex;

	private Vector3 detailScreenModelPos = Vector3.zero;

	private Vector3 detailScreenHeaderPos = Vector3.zero;

	private Vector3 detailScreenUiPos = Vector3.zero;

	private Vector3 fullScreenModelPos = Vector3.zero;

	private Vector3 fullScreenHeaderPos = Vector3.zero;

	private Vector3 fullScreenUiPos = Vector3.zero;

	private bool isAnimation;

	public static MonsterData DisplayMonsterData
	{
		set
		{
			CMD_PicturebookDetail.displayMonsterData = value;
		}
	}

	protected override void Update()
	{
		base.Update();
		this.ManageModelScale();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_PicturebookDetail.displayMonsterData = null;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.StartCoroutine(this.Initialize());
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		if (this.monsterParam != null)
		{
			this.monsterParam.gameObject.SetActive(true);
			this.monsterParam.PlayIdleAnimation();
		}
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.modelUiTex != null)
		{
			this.modelUiTex.gameObject.SetActive(false);
		}
		if (CMD_PicturebookDetail.onClosedPanel != null)
		{
			CMD_PicturebookDetail.onClosedPanel();
		}
		this.CloseAndFarmCamOn(animation);
		if (this.monsterParam != null && this.monsterParam.transform.parent != null)
		{
			UnityEngine.Object.Destroy(this.monsterParam.transform.parent.gameObject);
		}
	}

	private IEnumerator Initialize()
	{
		this.fullScreenModelPos = (this.detailScreenModelPos = this.modelUiTex.transform.localPosition);
		this.fullScreenModelPos.x = 0f;
		this.fullScreenHeaderPos = (this.detailScreenHeaderPos = this.headerObj.transform.localPosition);
		this.fullScreenHeaderPos.y = 480f;
		this.fullScreenUiPos = (this.detailScreenUiPos = this.uiNop.transform.localPosition);
		this.fullScreenUiPos.x = 800f;
		this.SetupUI();
		this.Show3dModel();
		this.displayMode = CMD_PicturebookDetail.DISPLAY_MODE.DetailMode;
		yield break;
	}

	private void SetupUI()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("PicturebookTitle"));
		string monsterName = CMD_PicturebookDetail.displayMonsterData.monsterMG.monsterName;
		string growStepName = MonsterGrowStepData.GetGrowStepName(CMD_PicturebookDetail.displayMonsterData.monsterMG.growStep);
		string tribeName = MonsterTribeData.GetTribeName(CMD_PicturebookDetail.displayMonsterData.monsterMG.tribe);
		string description = CMD_PicturebookDetail.displayMonsterData.monsterMG.description;
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
		foreach (GameWebAPI.RespDataMA_GetSkillM.SkillM skillM2 in skillM)
		{
			if (skillM2.skillGroupId == CMD_PicturebookDetail.displayMonsterData.monsterM.skillGroupId && !list.Contains(skillM2.name))
			{
				list.Add(skillM2.name);
				list2.Add(skillM2.description);
			}
		}
		string monsterStatusId = CMD_PicturebookDetail.displayMonsterData.monsterMG.monsterStatusId;
		string specificTypeName = MonsterSpecificTypeData.GetSpecificTypeName(monsterStatusId);
		switch (list.Count)
		{
		default:
			list.Add(string.Empty);
			list2.Add(string.Empty);
			this.oneSkillStatus.Initialize(monsterName, growStepName, tribeName, specificTypeName, description, list, list2);
			break;
		case 1:
			this.oneSkillStatus.Initialize(monsterName, growStepName, tribeName, specificTypeName, description, list, list2);
			break;
		case 2:
			this.twoSkillStatus.Initialize(monsterName, growStepName, tribeName, specificTypeName, description, list, list2);
			break;
		}
	}

	private void Show3dModel()
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DRT", null);
		this.render3DRT = gameObject.GetComponent<CommonRender3DRT>();
		string modelId = CMD_PicturebookDetail.displayMonsterData.GetMonsterMaster().Group.modelId;
		string filePath = MonsterObject.GetFilePath(modelId);
		this.render3DRT.LoadChara(filePath, 0f, 10000f, -0.65f, 1.1f, true);
		this.renderTex = this.render3DRT.SetRenderTarget(1136, 820, 16);
		this.modelUiTex.mainTexture = this.renderTex;
		this.monsterParam = this.render3DRT.transform.GetComponentInChildren<CharacterParams>();
	}

	public static void SetOnClosedPanel(Action OnClosedPanel)
	{
		CMD_PicturebookDetail.onClosedPanel = OnClosedPanel;
	}

	private void RotateModel(Vector3 Delta)
	{
		this.monsterParam.transform.Rotate(Delta);
	}

	private IEnumerator RandomAnimateModel()
	{
		if (this.monsterParam == null || this.isAnimation)
		{
			yield break;
		}
		this.isAnimation = true;
		int rand = UnityEngine.Random.Range(0, 3);
		float animeClipLength = 0f;
		float playTime = 0f;
		switch (rand)
		{
		case 0:
			this.monsterParam.PlayAnimationSmooth(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
			animeClipLength = this.monsterParam.AnimationClipLength;
			break;
		case 1:
			this.monsterParam.PlayAnimationSmooth(CharacterAnimationType.eat, SkillType.Attack, 0, null, null);
			animeClipLength = this.monsterParam.AnimationClipLength;
			break;
		case 2:
			this.monsterParam.PlayAnimationSmooth(CharacterAnimationType.attacks, SkillType.Attack, 0, null, null);
			animeClipLength = this.monsterParam.AnimationClipLength;
			break;
		}
		while (playTime < animeClipLength)
		{
			playTime += Time.deltaTime;
			yield return null;
		}
		this.isAnimation = false;
		yield break;
	}

	private void ChangeDisplayMode()
	{
		CMD_PicturebookDetail.DISPLAY_MODE display_MODE = this.displayMode;
		if (display_MODE != CMD_PicturebookDetail.DISPLAY_MODE.DetailMode)
		{
			if (display_MODE != CMD_PicturebookDetail.DISPLAY_MODE.FullScreenMode)
			{
				this.ChangeDisplayModeToFullScreen();
			}
			else
			{
				this.ChangeDisplayModeToDetail();
			}
		}
		else
		{
			this.ChangeDisplayModeToFullScreen();
		}
	}

	private void ChangeDisplayModeToDetail()
	{
		this.PlayCancelSE();
		this.MoveTo(this.modelUiTex.gameObject, this.detailScreenModelPos, 0.18f, iTween.EaseType.linear);
		this.MoveTo(this.headerObj, this.detailScreenHeaderPos, 0.18f, iTween.EaseType.linear);
		this.MoveTo(this.uiNop, this.detailScreenUiPos, 0.18f, iTween.EaseType.linear);
		this.displayModeButtonLabel.text = StringMaster.GetString("CharaDetailsFullScreen");
		this.displayMode = CMD_PicturebookDetail.DISPLAY_MODE.DetailMode;
	}

	private void ChangeDisplayModeToFullScreen()
	{
		this.PlaySelectSE();
		this.MoveTo(this.modelUiTex.gameObject, this.fullScreenModelPos, 0.18f, iTween.EaseType.linear);
		this.MoveTo(this.headerObj, this.fullScreenHeaderPos, 0.18f, iTween.EaseType.linear);
		this.MoveTo(this.uiNop, this.fullScreenUiPos, 0.18f, iTween.EaseType.linear);
		this.displayModeButtonLabel.text = StringMaster.GetString("SystemButtonReturn");
		this.displayMode = CMD_PicturebookDetail.DISPLAY_MODE.FullScreenMode;
	}

	private void MoveTo(GameObject go, Vector3 vP, float time, iTween.EaseType type = iTween.EaseType.linear)
	{
		iTween.MoveTo(go, new Hashtable
		{
			{
				"isLocal",
				true
			},
			{
				"x",
				vP.x
			},
			{
				"y",
				vP.y
			},
			{
				"time",
				time
			},
			{
				"easetype",
				type
			},
			{
				"oncomplete",
				"MoveEnd"
			},
			{
				"oncompleteparams",
				0
			}
		});
	}

	private void ManageModelScale()
	{
		if (this.monsterParam == null || this.monsterParam.transform.parent == null)
		{
			return;
		}
		float value = this.pinch.Value;
		Vector3 localScale = this.monsterParam.transform.localScale;
		if (value == 0f || (value < 0f && localScale.x <= 0.8f) || (value > 0f && localScale.x >= 1.2f))
		{
			return;
		}
		float num = localScale.x + value / 500f;
		if (num < 0.8f)
		{
			num = 0.8f;
		}
		if (num > 1.2f)
		{
			num = 1.2f;
		}
		this.monsterParam.transform.localScale = new Vector3(num, num, num);
	}

	public void OnDisplayDrag(Vector2 Delta)
	{
		this.RotateModel(new Vector3(0f, Delta.x / -3f));
	}

	public void OnDisplayClick()
	{
		if (!this.isAnimation)
		{
			base.StartCoroutine(this.RandomAnimateModel());
		}
	}

	public void OnClickDisplayModeButton()
	{
		this.ChangeDisplayMode();
	}

	public enum DISPLAY_MODE
	{
		DetailMode,
		FullScreenMode
	}
}
