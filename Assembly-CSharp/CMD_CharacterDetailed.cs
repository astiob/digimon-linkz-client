using CharacterDetailsUI;
using CharacterModelUI;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DigimonModelPlayer))]
public sealed class CMD_CharacterDetailed : CMD
{
	private static CMD_CharacterDetailed instance;

	[Header("チップ")]
	[SerializeField]
	private ChipBaseSelect chipBaseSelect;

	[SerializeField]
	[Header("パーティクルの位置")]
	private Vector3 particlePos = new Vector3(340f, 30f, 0f);

	[SerializeField]
	[Header("覚醒パーティクルの位置")]
	private Vector3 particleAwakeningPos = new Vector3(235f, 30f, 0f);

	[SerializeField]
	[Header("レベルアップの位置")]
	[Header("覚醒アイコンパーティクルの位置")]
	private Vector3 levelUpPos;

	[SerializeField]
	[Header("覚醒の位置")]
	private Vector3 AwakeningPos;

	[Header("右上の背景")]
	[SerializeField]
	private UISprite rightUpBG;

	[SerializeField]
	private MonsterBasicInfoExpGauge monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	[SerializeField]
	private MonsterStatusChangeValueList monsterStatusChangeValueList;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill monsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[SerializeField]
	private UISprite lockSprite;

	[SerializeField]
	private StatusUpAnimation statusAnime;

	[SerializeField]
	private GameObject goArousalAnimPos;

	[SerializeField]
	private GameObject goTORERANCE_EFF_NONE;

	[SerializeField]
	private GameObject goTORERANCE_EFF_FIRE;

	[SerializeField]
	private GameObject goTORERANCE_EFF_WATER;

	[SerializeField]
	private GameObject goTORERANCE_EFF_THUNDER;

	[SerializeField]
	private GameObject goTORERANCE_EFF_NATURE;

	[SerializeField]
	private GameObject goTORERANCE_EFF_LIGHT;

	[SerializeField]
	private GameObject goTORERANCE_EFF_DARK;

	[SerializeField]
	private GameObject goTORERANCE_EFF_STUN;

	[SerializeField]
	private GameObject goTORERANCE_EFF_SKILLLOCK;

	[SerializeField]
	private GameObject goTORERANCE_EFF_SLEEP;

	[SerializeField]
	private GameObject goTORERANCE_EFF_PARALYSIS;

	[SerializeField]
	private GameObject goTORERANCE_EFF_CONFUSION;

	[SerializeField]
	private GameObject goTORERANCE_EFF_POISON;

	[SerializeField]
	private GameObject goTORERANCE_EFF_DEATH;

	[SerializeField]
	[Header("レベルアップアイコンの置き場")]
	private Transform levelUpRoot;

	[SerializeField]
	[Header("覚醒アイコンの置き場")]
	private Transform AwakeningRoot;

	private DataMng.ExperienceInfo experienceInfo;

	private string beforeonsterRare;

	private CharacterCameraView characterCameraView;

	private Transform myTransform;

	[SerializeField]
	private GameObject[] PageObject = new GameObject[2];

	private int page = 1;

	[SerializeField]
	private PartsUpperCutinController cutinController;

	[SerializeField]
	private GameObject goSCR_CHARACTER;

	[SerializeField]
	private CharacterDetailsLeftUI leftUI;

	public GameObject goTargetTex;

	private UITexture ngTargetTex;

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

	private bool enablePageChange = true;

	private CMD_CharacterDetailed.Timer timer = new CMD_CharacterDetailed.Timer();

	private Action<int> movedAct;

	public CMD_CharacterDetailed.LockMode Mode { get; set; }

	public static CMD_CharacterDetailed.ButtonType AddButton { private get; set; }

	public static MonsterData DataChg { get; set; }

	public MonsterData GetShowCharacterMonsterData()
	{
		return CMD_CharacterDetailed.DataChg;
	}

	public static CMD_CharacterDetailed Instance
	{
		get
		{
			return CMD_CharacterDetailed.instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_CharacterDetailed.instance = this;
		this.myTransform = base.transform;
		this.page = 1;
		this.PageChange();
		Vector3 localPosition = this.goSCR_CHARACTER.transform.localPosition;
		localPosition.x = 0f;
		localPosition.y = 0f;
		this.goSCR_CHARACTER.transform.localPosition = localPosition;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("CharaDetailsTitle"));
		this.ShowChgInfo();
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	protected override void Update()
	{
		base.Update();
		if (this.characterCameraView != null)
		{
			this.characterCameraView.Update(Time.deltaTime);
		}
		if (this.timer != null)
		{
			this.timer.Update(Time.deltaTime);
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

	public void ShowByReinforcement(MonsterData oldMonsterData, int oldLevel, int upLuck)
	{
		base.StartCoroutine(this.ShowByReinforcement_(oldMonsterData, oldLevel, upLuck));
	}

	private IEnumerator ShowByReinforcement_(MonsterData oldMonsterData, int oldLevel, int upLuck)
	{
		this.enablePageChange = false;
		this.statusAnime.monsterData = oldMonsterData;
		this.statusAnime.defaultLevel = oldLevel;
		int newLevel = this.experienceInfo.lev;
		this.statusAnime.DisplayDifference(newLevel, upLuck);
		if (newLevel > oldLevel)
		{
			yield return this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.LevelUp, null);
		}
		if (upLuck > 0)
		{
			yield return this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.LuckUp, null);
		}
		if (newLevel > oldLevel)
		{
			this.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
			this.ShowParticle();
		}
		else
		{
			this.enablePageChange = true;
		}
		yield break;
	}

	public void ShowByArousal(MonsterData oldMonsterData, MonsterData newMonsterData)
	{
		this.enablePageChange = false;
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> userUnitResistanceList = newMonsterData.GetUserUnitResistanceList();
		this.statusAnime.monsterData = newMonsterData;
		string NOMAL = "0";
		GameObject target = null;
		userUnitResistanceList.ForEach(delegate(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM n)
		{
			if (n.none != NOMAL)
			{
				(target = this.goTORERANCE_EFF_NONE).SetActive(true);
			}
			if (n.fire != NOMAL)
			{
				(target = this.goTORERANCE_EFF_FIRE).SetActive(true);
			}
			if (n.water != NOMAL)
			{
				(target = this.goTORERANCE_EFF_WATER).SetActive(true);
			}
			if (n.thunder != NOMAL)
			{
				(target = this.goTORERANCE_EFF_THUNDER).SetActive(true);
			}
			if (n.nature != NOMAL)
			{
				(target = this.goTORERANCE_EFF_NATURE).SetActive(true);
			}
			if (n.dark != NOMAL)
			{
				(target = this.goTORERANCE_EFF_DARK).SetActive(true);
			}
			if (n.light != NOMAL)
			{
				(target = this.goTORERANCE_EFF_LIGHT).SetActive(true);
			}
			if (n.stun != NOMAL)
			{
				(target = this.goTORERANCE_EFF_STUN).SetActive(true);
			}
			if (n.skillLock != NOMAL)
			{
				(target = this.goTORERANCE_EFF_SKILLLOCK).SetActive(true);
			}
			if (n.sleep != NOMAL)
			{
				(target = this.goTORERANCE_EFF_SLEEP).SetActive(true);
			}
			if (n.paralysis != NOMAL)
			{
				(target = this.goTORERANCE_EFF_PARALYSIS).SetActive(true);
			}
			if (n.confusion != NOMAL)
			{
				(target = this.goTORERANCE_EFF_CONFUSION).SetActive(true);
			}
			if (n.poison != NOMAL)
			{
				(target = this.goTORERANCE_EFF_POISON).SetActive(true);
			}
			if (n.death != NOMAL)
			{
				(target = this.goTORERANCE_EFF_DEATH).SetActive(true);
			}
		});
		if (target != null)
		{
			RenderFrontThanNGUI component = target.GetComponent<RenderFrontThanNGUI>();
			if (component != null)
			{
				UIPanel[] array = new UIPanel[]
				{
					this.cutinController.gameObject.AddComponent<UIPanel>(),
					this.AwakeningRoot.gameObject.AddComponent<UIPanel>()
				};
				foreach (UIPanel uipanel in array)
				{
					uipanel.sortingOrder = component.GetSortOrder() + 1;
				}
			}
		}
		this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.ResistanceChange, delegate
		{
			this.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
			this.ShowAwakeningParticle();
		});
	}

	private void ShowParticle()
	{
		string path = "Cutscenes/NewFX6";
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(path, typeof(GameObject)));
		gameObject.name = "LevelUpParticle";
		Transform transform = gameObject.transform;
		transform.SetParent(this.myTransform);
		transform.localPosition = this.particlePos;
		this.ShowLevelUpAnimation();
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
	}

	private void ShowAwakeningParticle()
	{
		string path = "Cutscenes/NewFX10";
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(path, typeof(GameObject)));
		gameObject.name = "AwakeningParticle";
		Transform transform = gameObject.transform;
		transform.SetParent(this.myTransform);
		transform.localPosition = this.particleAwakeningPos;
		this.ShowAwakeningAnimation();
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
	}

	private void ShowLevelUpAnimation()
	{
		string path = "UICommon/Parts/LevelUp";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		DepthController depthController = gameObject.AddComponent<DepthController>();
		Transform transform = gameObject.transform;
		int depth = this.rightUpBG.depth;
		depthController.AddWidgetDepth(transform, depth + 10);
		transform.SetParent(this.levelUpRoot);
		transform.localPosition = this.levelUpPos;
		transform.localScale = Vector3.one;
		Animation component = gameObject.GetComponent<Animation>();
		component.Play("LevelUp");
		this.timer.Set(component.clip.length, delegate
		{
			this.enablePageChange = true;
		});
	}

	private void ShowAwakeningAnimation()
	{
		string path = "UICommon/Parts/AwakeningParts";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		DepthController depthController = gameObject.AddComponent<DepthController>();
		Transform transform = gameObject.transform;
		int depth = this.rightUpBG.depth;
		depthController.AddWidgetDepth(transform, depth + 10);
		transform.SetParent(this.AwakeningRoot);
		transform.localPosition = this.AwakeningPos;
		transform.localScale = Vector3.one;
		Animation component = gameObject.GetComponent<Animation>();
		component.Play("Awakening");
		this.timer.Set(component.clip.length, delegate
		{
			this.enablePageChange = true;
		});
	}

	private void ShowChgInfo()
	{
		if (CMD_CharacterDetailed.DataChg != null)
		{
			this.chipBaseSelect.SetSelectedCharChg(CMD_CharacterDetailed.DataChg);
			if (!CMD_CharacterDetailed.DataChg.userMonster.IsEgg())
			{
				this.monsterLeaderSkill.SetSkill(CMD_CharacterDetailed.DataChg);
				this.monsterUniqueSkill.SetSkill(CMD_CharacterDetailed.DataChg);
				this.monsterSuccessionSkill.SetSkill(CMD_CharacterDetailed.DataChg);
				this.leftUI.Initialize(CMD_CharacterDetailed.DataChg.userMonster);
				this.ShowChgInfoUP();
				this.ShowCharacter();
			}
			else
			{
				string eggName = StringMaster.GetString("CharaStatus-06");
				int num = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length;
				for (int i = 0; i < num; i++)
				{
					GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i];
					if (monsterEvolutionRouteM.monsterEvolutionRouteId == CMD_CharacterDetailed.DataChg.userMonster.monsterEvolutionRouteId)
					{
						GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterEvolutionRouteM.eggMonsterId);
						if (monsterGroupMasterByMonsterGroupId != null)
						{
							eggName = monsterGroupMasterByMonsterGroupId.monsterName;
						}
					}
				}
				this.monsterLeaderSkill.ClearSkill();
				this.monsterUniqueSkill.ClearSkill();
				this.monsterSuccessionSkill.ClearSkill();
				this.leftUI.Initialize(CMD_CharacterDetailed.DataChg.userMonster);
				if (CMD_CharacterDetailed.AddButton == CMD_CharacterDetailed.ButtonType.Garden)
				{
					this.leftUI.ShowGardenButton();
				}
				this.ShowEggInfo(eggName);
				this.ShowCharacter();
				this.ShowRareEgg();
			}
		}
		else
		{
			global::Debug.LogError("ここへは来ない想定");
		}
	}

	private void ShowCharacter()
	{
		this.characterCameraView = new CharacterCameraView(CMD_CharacterDetailed.DataChg);
		this.ngTargetTex = this.goTargetTex.GetComponent<UITexture>();
		this.ngTargetTex.mainTexture = this.characterCameraView.renderTex;
	}

	private void ShowChgInfoUP()
	{
		int exp = int.Parse(CMD_CharacterDetailed.DataChg.userMonster.ex);
		this.experienceInfo = DataMng.Instance().GetExperienceInfo(exp);
		this.monsterBasicInfo.SetMonsterData(CMD_CharacterDetailed.DataChg, this.experienceInfo);
		this.monsterStatusList.SetValues(CMD_CharacterDetailed.DataChg, true);
		this.monsterStatusChangeValueList.SetValues(CMD_CharacterDetailed.DataChg);
		this.monsterResistanceList.SetValues(CMD_CharacterDetailed.DataChg);
		this.monsterMedalList.SetValues(CMD_CharacterDetailed.DataChg.userMonster);
	}

	private void ShowEggInfo(string eggName)
	{
		this.monsterBasicInfo.SetEggData(eggName);
		this.monsterStatusList.ClearEggCandidateMedalValues();
		this.monsterStatusChangeValueList.SetEggStatusValues();
		this.monsterResistanceList.ClearValues();
		this.monsterMedalList.SetValues(CMD_CharacterDetailed.DataChg.userMonster);
	}

	private void ShowRareEgg()
	{
		if (CMD_CharacterDetailed.DataChg != null)
		{
			this.goSCR_CHARACTER.transform.localPosition = new Vector3(this.goSCR_CHARACTER.transform.localPosition.x, -200f, this.goSCR_CHARACTER.transform.localPosition.z);
		}
	}

	private void ShowRarityUpAnimation()
	{
		string path = "UICommon/Parts/RareUp";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		DepthController depthController = gameObject.AddComponent<DepthController>();
		Transform transform = gameObject.transform;
		int depth = this.rightUpBG.depth;
		depthController.AddWidgetDepth(transform, depth + 10);
		transform.SetParent(this.goSCR_CHARACTER.transform);
		transform.localPosition = new Vector3(0f, 200f, 0f);
		transform.localScale = Vector3.one;
		Animation component = gameObject.GetComponent<Animation>();
		component.Play();
	}

	private void ShowRareUpStarAnimation()
	{
		string path = "UICommon/Parts/RareUpStar";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		GameObject gameObject2 = this.goArousalAnimPos;
		DepthController depthController = gameObject.AddComponent<DepthController>();
		Transform transform = gameObject.transform;
		int depth = this.rightUpBG.depth;
		depthController.AddWidgetDepth(transform, depth + 10);
		transform.SetParent(gameObject2.transform);
		transform.localPosition = new Vector3(22f, 0f, 0f);
		transform.localScale = Vector3.one;
	}

	private void ShowEggSurroundingsEffect()
	{
		string path = "Cutscenes/NewFX4b";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Transform transform = gameObject.transform;
		transform.SetParent(this.goSCR_CHARACTER.transform);
		transform.localPosition = new Vector3(0f, -300f, 0f);
		transform.localScale = new Vector3(320f, 320f, 320f);
	}

	private void InitOpenScreen()
	{
		this.vOrgSCR_HEADER = this.goSCR_HEADER.transform.localPosition;
		this.vOrgSCR_DETAIL = this.goSCR_DETAIL.transform.localPosition;
		GameObject protectionButton = this.leftUI.GetProtectionButton();
		if (protectionButton.activeSelf)
		{
			this.vOrgSCR_LOCKBTN = protectionButton.transform.localPosition;
			this.vPosSCR_LOCKBTN = this.vOrgSCR_LOCKBTN;
			this.vPosSCR_LOCKBTN.x = -800f;
		}
		this.vPosSCR_HEADER = this.vOrgSCR_HEADER;
		this.vPosSCR_HEADER.y = 480f;
		this.vPosSCR_DETAIL = this.vOrgSCR_DETAIL;
		this.vPosSCR_DETAIL.x = 800f;
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

	private void PageChange()
	{
		if (!this.enablePageChange)
		{
			return;
		}
		if (this.page > this.PageObject.Length)
		{
			this.page = 1;
		}
		int num = 1;
		foreach (GameObject gameObject in this.PageObject)
		{
			if (this.page == num)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
			num++;
		}
		this.page++;
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

	private class Timer
	{
		private float currentTime;

		private float maxTime;

		private Action callback;

		public void Set(float time, Action callback)
		{
			this.currentTime = 0f;
			this.maxTime = time;
			this.callback = callback;
		}

		public void Update(float dt)
		{
			if (this.currentTime >= this.maxTime)
			{
				return;
			}
			if ((this.currentTime += dt) > this.maxTime)
			{
				this.callback();
				this.callback = null;
			}
		}
	}
}
