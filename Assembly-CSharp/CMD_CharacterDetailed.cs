using CharacterDetailsUI;
using CharacterModelUI;
using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DigimonModelPlayer))]
public sealed class CMD_CharacterDetailed : CMD
{
	private static CMD_CharacterDetailed instance;

	[SerializeField]
	private CharacterStatusList statusList;

	[SerializeField]
	private StatusUpAnimation statusAnime;

	[SerializeField]
	private PartsUpperCutinController cutinController;

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

	private bool enablePageChange = true;

	private CMD_CharacterDetailed.Timer timer = new CMD_CharacterDetailed.Timer();

	private Action<int> movedAct;

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
		Transform transform = this.renderTextureObject.transform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = 0f;
		localPosition.y = 0f;
		transform.localPosition = localPosition;
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

	public void TranceEffectActiveSet(bool active)
	{
		this.statusList.TranceEffectActiveSet(active);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_CharacterDetailed.DataChg = null;
		CMD_CharacterDetailed.AddButton = CMD_CharacterDetailed.ButtonType.None;
		this.characterCameraView.Destroy();
	}

	public IEnumerator StartReinforcementEffect(string newExp, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList oldUserMonster, int upLuck)
	{
		this.enablePageChange = false;
		int oldLevel = int.Parse(oldUserMonster.level);
		this.statusAnime.Initialize(oldUserMonster, oldLevel);
		this.statusAnime.userMonster = oldUserMonster;
		this.statusAnime.defaultLevel = oldLevel;
		DataMng.ExperienceInfo expInfo = DataMng.Instance().GetExperienceInfo(int.Parse(newExp));
		this.statusAnime.DisplayDifference(expInfo.lev, upLuck);
		if (expInfo.lev > oldLevel)
		{
			yield return this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.LevelUp, null);
		}
		if (upLuck > 0)
		{
			yield return this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.LuckUp, null);
		}
		if (expInfo.lev > oldLevel)
		{
			this.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
			Animation levelUpAnimtion = this.statusList.ShowLevelUpParticle(base.transform);
			this.timer.Set(levelUpAnimtion.clip.length, delegate
			{
				this.enablePageChange = true;
			});
		}
		else
		{
			this.enablePageChange = true;
		}
		yield break;
	}

	public void ShowByArousal(string uniqueResistanceId, string oldResistanceIds, string newResistanceIds)
	{
		this.enablePageChange = false;
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(uniqueResistanceId);
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceListByJson = MonsterResistanceData.GetUniqueResistanceListByJson(oldResistanceIds);
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM oldResistance = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceListByJson);
		GameObject tranceEffectObject = this.statusList.GetTranceEffectObject(oldResistance, newResistanceIds);
		UIPanel uipanel = this.cutinController.GetComponent<UIPanel>();
		if (null == uipanel)
		{
			uipanel = this.cutinController.gameObject.AddComponent<UIPanel>();
		}
		this.statusList.StartTranceEffect(tranceEffectObject, uipanel);
		this.cutinController.PlayAnimator(PartsUpperCutinController.AnimeType.ResistanceChange, new Action(this.OnFinishTranceCutin));
	}

	private void OnFinishTranceCutin()
	{
		this.characterCameraView.csRender3DRT.SetAnimation(CharacterAnimationType.win);
		Animation animation = this.statusList.ShowTranceParticle(base.transform);
		this.timer.Set(animation.clip.length, delegate
		{
			this.enablePageChange = true;
		});
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
		this.characterCameraView = new CharacterCameraView(CMD_CharacterDetailed.DataChg);
		this.renderTextureObject.mainTexture = this.characterCameraView.renderTex;
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
		if (this.enablePageChange)
		{
			this.statusList.NextPage();
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
