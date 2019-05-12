using CharacterDetailsUI;
using CharacterModelUI;
using DeviceSafeArea;
using Master;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DigimonModelPlayer))]
public sealed class CMD_CharacterDetailed : CMD
{
	private ICharacterDetailsUIAnimation uiAnimation;

	private static CMD_CharacterDetailed instance;

	[SerializeField]
	private CharacterStatusList statusList;

	[SerializeField]
	private StatusUpAnimation statusAnime;

	[SerializeField]
	private UITexture renderTextureObject;

	[SerializeField]
	private CharacterDetailsLeftUI leftUI;

	private CharacterCameraView characterCameraView;

	[SerializeField]
	private GameObject goSCR_HEADER;

	[SerializeField]
	private GameObject goSCR_DETAIL;

	private Vector3 vOrgSCR_CHARACTER;

	private Vector3 vOrgSCR_HEADER;

	private Vector3 vOrgSCR_DETAIL;

	private Vector3 vOrgSCR_LOCKBTN;

	private Vector3 vPosSCR_CHARACTER;

	private Vector3 vPosSCR_HEADER;

	private Vector3 vPosSCR_DETAIL;

	private Vector3 vPosSCR_LOCKBTN;

	private bool isInitOpen;

	private bool isOpenScreen;

	private Action<int> movedAct;

	public static CMD_CharacterDetailed CreateWindow(MonsterData monster, bool isResetEquipChip, Action endCutin)
	{
		CMD_CharacterDetailed.DataChg = monster;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		UIPanel uipanel = GUIMain.GetUIPanel();
		int sortingOrder = uipanel.sortingOrder;
		CharacterDetailsVersionUp characterDetailsVersionUp = new CharacterDetailsVersionUp();
		characterDetailsVersionUp.Initialize(sortingOrder, cmd_CharacterDetailed.transform, isResetEquipChip, endCutin);
		cmd_CharacterDetailed.uiAnimation = characterDetailsVersionUp;
		cmd_CharacterDetailed.uiAnimation.OnOpenWindow();
		return cmd_CharacterDetailed;
	}

	public static CMD_CharacterDetailed CreateWindow(MonsterData monster, Action endCutin, bool isSuccessInheritance, bool isResetEquipChip)
	{
		CMD_CharacterDetailed.DataChg = monster;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		UIPanel uipanel = GUIMain.GetUIPanel();
		int sortingOrder = uipanel.sortingOrder;
		CharacterDetailsMedalInheritance characterDetailsMedalInheritance = new CharacterDetailsMedalInheritance();
		characterDetailsMedalInheritance.Initialize(sortingOrder, cmd_CharacterDetailed.transform, isSuccessInheritance, isResetEquipChip, endCutin);
		cmd_CharacterDetailed.uiAnimation = characterDetailsMedalInheritance;
		cmd_CharacterDetailed.uiAnimation.OnOpenWindow();
		return cmd_CharacterDetailed;
	}

	public static CMD_CharacterDetailed CreateWindow(MonsterData monster)
	{
		CMD_CharacterDetailed.DataChg = monster;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		UIPanel uipanel = GUIMain.GetUIPanel();
		int sortingOrder = uipanel.sortingOrder;
		CharacterDetailsGardenEvolution characterDetailsGardenEvolution = new CharacterDetailsGardenEvolution();
		characterDetailsGardenEvolution.Initialize(sortingOrder, cmd_CharacterDetailed.transform);
		cmd_CharacterDetailed.uiAnimation = characterDetailsGardenEvolution;
		cmd_CharacterDetailed.uiAnimation.OnOpenWindow();
		return cmd_CharacterDetailed;
	}

	public static CMD_CharacterDetailed CreateWindow(MonsterData monster, bool isArousal, bool isResetEquipChip, Action endCutin)
	{
		CMD_CharacterDetailed.DataChg = monster;
		CMD_CharacterDetailed.AddButton = CMD_CharacterDetailed.ButtonType.Garden;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		UIPanel uipanel = GUIMain.GetUIPanel();
		int sortingOrder = uipanel.sortingOrder;
		CharacterDetailsLaboratory characterDetailsLaboratory = new CharacterDetailsLaboratory();
		characterDetailsLaboratory.Initialize(sortingOrder, cmd_CharacterDetailed.transform, isArousal, isResetEquipChip, endCutin);
		cmd_CharacterDetailed.uiAnimation = characterDetailsLaboratory;
		cmd_CharacterDetailed.uiAnimation.OnOpenWindow();
		return cmd_CharacterDetailed;
	}

	public static CMD_CharacterDetailed CreateWindow(MonsterData monster, string evolutionType, bool reviewFirstUltima, bool reviewFirstEvolution, Action endCutin)
	{
		CMD_CharacterDetailed.DataChg = monster;
		CMD_CharacterDetailed window = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		UIPanel uipanel = GUIMain.GetUIPanel();
		int sortingOrder = uipanel.sortingOrder;
		CharacterDetailsEvolution characterDetailsEvolution = new CharacterDetailsEvolution();
		characterDetailsEvolution.Initialize(sortingOrder, window.transform, evolutionType, delegate
		{
			window.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
			if (endCutin != null)
			{
				endCutin();
			}
			if (reviewFirstUltima)
			{
				LeadReview.ShowReviewConfirm(LeadReview.MessageType.FIRST_EVOLUTION, null, false);
			}
			else if (reviewFirstEvolution)
			{
				LeadReview.ShowReviewConfirm(LeadReview.MessageType.FIRST_ULTIMA_EVOLUTION, null, false);
			}
		});
		window.uiAnimation = characterDetailsEvolution;
		window.uiAnimation.OnOpenWindow();
		return window;
	}

	public static CMD_CharacterDetailed CreateWindow(MonsterData monster, Action onClose, int statusPage)
	{
		CMD_CharacterDetailed.DataChg = monster;
		CMD_CharacterDetailed window = GUIMain.ShowCommonDialog(delegate(int nop)
		{
			onClose();
		}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		window.statusList.SetPage(statusPage);
		window.statusList.EnablePage(false);
		UIPanel uipanel = GUIMain.GetUIPanel();
		int sortingOrder = uipanel.sortingOrder;
		CharacterDetailsInheritance characterDetailsInheritance = new CharacterDetailsInheritance();
		characterDetailsInheritance.Initialize(sortingOrder, window.transform, delegate
		{
			window.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
			window.statusList.EnablePage(true);
		});
		window.uiAnimation = characterDetailsInheritance;
		window.uiAnimation.OnOpenWindow();
		return window;
	}

	public static CMD_CharacterDetailed CreateWindow(MonsterData monsterData, Action closeAction, string uniqueResistanceId, string oldResistanceIds, string newResistanceIds)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		CMD_CharacterDetailed window = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (closeAction != null)
			{
				closeAction();
			}
		}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		window.statusList.EnablePage(false);
		CharacterDetailsResistanceParameter param = new CharacterDetailsResistanceParameter
		{
			uniqueResistanceId = uniqueResistanceId,
			oldResistanceIds = oldResistanceIds,
			newResistanceIds = newResistanceIds
		};
		CharacterDetailsResistance characterDetailsResistance = new CharacterDetailsResistance();
		characterDetailsResistance.Initialize(window.transform, window.statusList.GetResistance(), param, delegate
		{
			window.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
		}, delegate
		{
			window.statusList.EnablePage(true);
		});
		window.uiAnimation = characterDetailsResistance;
		window.uiAnimation.OnOpenWindow();
		return window;
	}

	public static CMD_CharacterDetailed CreateWindow(MonsterData monsterData, Action closeAction, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList beforeMonsterParam, string resultExp, int upLuck)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		CMD_CharacterDetailed window = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (closeAction != null)
			{
				closeAction();
			}
		}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		window.statusList.EnablePage(false);
		CharacterDetailsReinforcementParam characterDetailsReinforcementParam = new CharacterDetailsReinforcementParam
		{
			beforeMonster = beforeMonsterParam,
			upLuckValue = upLuck
		};
		DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(int.Parse(resultExp));
		characterDetailsReinforcementParam.afterLevel = experienceInfo.lev;
		UIPanel uipanel = GUIMain.GetUIPanel();
		int sortingOrder = uipanel.sortingOrder;
		CharacterDetailsReinforcement characterDetailsReinforcement = new CharacterDetailsReinforcement();
		characterDetailsReinforcement.Initialize(sortingOrder, window.transform, window.statusList.GetReinforcement(), window.statusAnime, characterDetailsReinforcementParam, delegate
		{
			window.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
		}, delegate
		{
			window.statusList.EnablePage(true);
		});
		window.uiAnimation = characterDetailsReinforcement;
		window.uiAnimation.OnOpenWindow();
		return window;
	}

	public void StartAnimation()
	{
		this.uiAnimation.StartAnimation();
	}

	public void OnOpenMenuReceiver()
	{
		if (this.uiAnimation != null)
		{
			this.uiAnimation.OnOpenMenu();
		}
	}

	public void OnCloseMenuReceiver()
	{
		if (this.uiAnimation != null)
		{
			this.uiAnimation.OnCloseMenu();
		}
	}

	public static CMD_CharacterDetailed Instance
	{
		get
		{
			return CMD_CharacterDetailed.instance;
		}
	}

	public static CMD_CharacterDetailed.ButtonType AddButton { private get; set; }

	public static MonsterData DataChg { get; set; }

	public CMD_CharacterDetailed.LockMode Mode { get; set; }

	public MonsterData GetShowCharacterMonsterData()
	{
		return CMD_CharacterDetailed.DataChg;
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_CharacterDetailed.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("CharaDetailsTitle"));
		this.ShowChgInfo();
		this.statusList.EnablePage(true);
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		UIWidget component = this.renderTextureObject.GetComponent<UIWidget>();
		if (null != component)
		{
			component.updateAnchors = UIRect.AnchorUpdate.OnStart;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.characterCameraView != null)
		{
			this.characterCameraView.Update(Time.deltaTime);
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		CMD_CharacterDetailed.AddButton = CMD_CharacterDetailed.ButtonType.None;
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_CharacterDetailed.DataChg = null;
		CMD_CharacterDetailed.AddButton = CMD_CharacterDetailed.ButtonType.None;
		this.characterCameraView.Destroy();
	}

	private void ShowChgInfo()
	{
		if (CMD_CharacterDetailed.DataChg != null)
		{
			if (!CMD_CharacterDetailed.DataChg.userMonster.IsEgg())
			{
				this.statusList.SetMonsterStatus(CMD_CharacterDetailed.DataChg);
				this.leftUI.Initialize(CMD_CharacterDetailed.DataChg.userMonster);
				this.ShowCharacter();
			}
			else
			{
				this.statusList.SetEggStatus(CMD_CharacterDetailed.DataChg);
				this.leftUI.Initialize(CMD_CharacterDetailed.DataChg.userMonster);
				if (CMD_CharacterDetailed.AddButton == CMD_CharacterDetailed.ButtonType.Garden)
				{
					this.leftUI.ShowGardenButton();
				}
				this.ShowCharacter();
				Transform transform = this.renderTextureObject.transform;
				transform.localPosition = new Vector3(transform.localPosition.x, -200f, transform.localPosition.z);
			}
			this.statusList.SetPage(0);
		}
	}

	private void ShowCharacter()
	{
		Vector2 deviceScreenSize = SafeArea.GetDeviceScreenSize();
		this.characterCameraView = new CharacterCameraView(CMD_CharacterDetailed.DataChg, (int)deviceScreenSize.x, (int)deviceScreenSize.y);
		this.renderTextureObject.mainTexture = this.characterCameraView.renderTex;
	}

	private void InitOpenScreen()
	{
		this.vOrgSCR_HEADER = this.goSCR_HEADER.transform.localPosition;
		this.vOrgSCR_DETAIL = this.goSCR_DETAIL.transform.localPosition;
		UIPanel uipanel = GUIMain.GetUIPanel();
		Vector2 windowSize = uipanel.GetWindowSize();
		GameObject protectionButton = this.leftUI.GetProtectionButton();
		if (protectionButton.activeSelf)
		{
			this.vOrgSCR_LOCKBTN = protectionButton.transform.localPosition;
			this.vPosSCR_LOCKBTN = this.vOrgSCR_LOCKBTN;
			this.vPosSCR_LOCKBTN.x = -windowSize.x;
		}
		this.vPosSCR_HEADER = this.vOrgSCR_HEADER;
		this.vPosSCR_HEADER.y = windowSize.y;
		this.vPosSCR_DETAIL = this.vOrgSCR_DETAIL;
		this.vPosSCR_DETAIL.x = windowSize.x;
	}

	public void OnClickedScreen()
	{
		if (!this.isInitOpen)
		{
			this.InitOpenScreen();
			this.isInitOpen = true;
		}
		CharacterParams characterParams = this.characterCameraView.csRender3DRT.GetCharacterParams();
		DigimonModelPlayer component = base.GetComponent<DigimonModelPlayer>();
		GameObject protectionButton = this.leftUI.GetProtectionButton();
		if (!this.isOpenScreen)
		{
			this.MoveTo(this.goSCR_HEADER, this.vPosSCR_HEADER, 0.18f, null, iTween.EaseType.linear);
			this.MoveTo(this.goSCR_DETAIL, this.vPosSCR_DETAIL, 0.18f, null, iTween.EaseType.linear);
			this.MoveTo(protectionButton, this.vPosSCR_LOCKBTN, 0.18f, null, iTween.EaseType.linear);
			this.leftUI.SetModelViewState(CharacterDetailsLeftUI.ModelViewState.FULL_SCREEN);
			if (component != null)
			{
				component.MonsterParams = characterParams;
			}
			this.characterCameraView.MoveToCenter(0.18f);
		}
		else
		{
			if (characterParams != null)
			{
				characterParams.transform.localScale = Vector3.one;
			}
			this.MoveTo(this.goSCR_HEADER, this.vOrgSCR_HEADER, 0.18f, null, iTween.EaseType.linear);
			this.MoveTo(this.goSCR_DETAIL, this.vOrgSCR_DETAIL, 0.18f, null, iTween.EaseType.linear);
			this.MoveTo(protectionButton, this.vOrgSCR_LOCKBTN, 0.18f, null, iTween.EaseType.linear);
			this.leftUI.SetModelViewState(CharacterDetailsLeftUI.ModelViewState.SIMPLE);
			if (component != null)
			{
				component.MonsterParams = null;
			}
			this.characterCameraView.MoveToLeft(0.18f);
		}
		this.isOpenScreen = !this.isOpenScreen;
		this.characterCameraView.enableTouch = this.isOpenScreen;
	}

	private void MoveTo(GameObject go, Vector3 vP, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear)
	{
		this.movedAct = act;
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

	private void MoveEnd(int id)
	{
		if (this.movedAct != null)
		{
			this.movedAct(id);
		}
	}

	public void OnDisplayDrag(Vector2 Delta)
	{
		this.characterCameraView.OnDisplayDrag(Delta);
	}

	public void OnClickReturnButton()
	{
		this.ClosePanel(true);
	}

	public void DisableEvolutionButton()
	{
		this.leftUI.DeleteEvolutionButton();
	}

	public void SetViewNextEvolutionMonster(string monsterId, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
	{
		this.statusList.SetViewNextEvolutionMonster(monsterId, userMonster);
		this.ShowChgInfo();
		GameObject protectionButton = this.leftUI.GetProtectionButton();
		protectionButton.SetActive(false);
	}

	public enum LockMode
	{
		None,
		Laboratory,
		Farewell,
		Reinforcement,
		Succession,
		Evolution,
		Arousal
	}

	public enum ButtonType
	{
		None,
		Return,
		Garden
	}
}
