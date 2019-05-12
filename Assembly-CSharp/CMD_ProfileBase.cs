using CharacterModelUI;
using Master;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DigimonModelPlayer))]
public abstract class CMD_ProfileBase : CMD
{
	[SerializeField]
	protected UITexture ngTargetTex;

	protected GameWebAPI.RespDataPRF_Profile userProfile;

	protected MonsterData leaderMonsterData;

	protected bool isOpenScreen;

	public GameObject goOPEN_SCREEN;

	private UILabel ngTX_OPEN_SCREEN;

	public GameObject goSCR_CHARACTER;

	public GameObject goSCR_HEADER;

	public GameObject goSCR_DETAIL;

	private Vector3 vOrgSCR_CHARACTER;

	private Vector3 vOrgSCR_HEADER;

	private Vector3 vOrgSCR_DETAIL;

	private Vector3 vPosSCR_CHARACTER;

	private Vector3 vPosSCR_HEADER;

	private Vector3 vPosSCR_DETAIL;

	protected CharacterCameraView characterCameraView;

	protected GameWebAPI.ColosseumUserStatus colosseumUserStatus;

	private Action<int> movedAct;

	protected override void Awake()
	{
		base.Awake();
		Vector3 localPosition = this.goSCR_CHARACTER.transform.localPosition;
		localPosition.x = 0f;
		localPosition.y = 0f;
		this.goSCR_CHARACTER.transform.localPosition = localPosition;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		base.PartsTitle.SetTitle(StringMaster.GetString("ProfileTitle"));
		this.RefreshComponents();
		this.leaderMonsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.userProfile.monsterData.monsterId);
		if (this.leaderMonsterData != null)
		{
			this.ShowCharacter();
		}
		base.StartCoroutine(this.GetColosseumUserStatus(this.userProfile.userData.userId, delegate
		{
			this.SetColosseumUserStatus();
		}));
	}

	protected abstract void dataReload();

	protected abstract void RefreshComponents();

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		this.InitOpenScreen();
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void Update()
	{
		base.Update();
		if (this.characterCameraView != null)
		{
			this.characterCameraView.Update(Time.deltaTime);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.leaderMonsterData = null;
		this.characterCameraView.Destroy();
	}

	protected virtual void OnCloseGetProfileComplete()
	{
	}

	protected void OnCloseGetProfileErr(int i)
	{
		AlertManager.ShowAlertDialog(null, "C-US01");
	}

	private void ShowCharacter()
	{
		this.characterCameraView = new CharacterCameraView(this.leaderMonsterData);
		this.ngTargetTex.mainTexture = this.characterCameraView.renderTex;
	}

	private void InitOpenScreen()
	{
		this.ngTX_OPEN_SCREEN = this.goOPEN_SCREEN.GetComponent<UILabel>();
		this.vOrgSCR_CHARACTER = this.goSCR_CHARACTER.transform.localPosition;
		this.vOrgSCR_HEADER = this.goSCR_HEADER.transform.localPosition;
		this.vOrgSCR_DETAIL = this.goSCR_DETAIL.transform.localPosition;
		this.vPosSCR_CHARACTER = this.vOrgSCR_CHARACTER;
		this.vPosSCR_CHARACTER.x = 0f;
		this.vPosSCR_HEADER = this.vOrgSCR_HEADER;
		this.vPosSCR_HEADER.y = 480f;
		this.vPosSCR_DETAIL = this.vOrgSCR_DETAIL;
		this.vPosSCR_DETAIL.x = 1000f;
	}

	protected void OnClickedScreen()
	{
		CharacterParams characterParams = this.characterCameraView.csRender3DRT.GetCharacterParams();
		DigimonModelPlayer component = base.GetComponent<DigimonModelPlayer>();
		if (!this.isOpenScreen)
		{
			this.MoveTo(this.goSCR_HEADER, this.vPosSCR_HEADER, 0.18f, null, iTween.EaseType.linear);
			this.MoveTo(this.goSCR_DETAIL, this.vPosSCR_DETAIL, 0.18f, null, iTween.EaseType.linear);
			this.ngTX_OPEN_SCREEN.text = StringMaster.GetString("SystemButtonReturn");
			if (null != component)
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
			this.ngTX_OPEN_SCREEN.text = StringMaster.GetString("CharaDetailsFullScreen");
			if (null != component)
			{
				component.MonsterParams = null;
			}
			this.characterCameraView.MoveToLeft(0.18f);
		}
		this.isOpenScreen = !this.isOpenScreen;
		this.characterCameraView.enableTouch = this.isOpenScreen;
	}

	protected void MoveTo(GameObject go, Vector3 vP, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear)
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

	private IEnumerator GetColosseumUserStatus(string UserID, Action OnEnded)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		GameWebAPI.ColosseumUserStatusLogic request = new GameWebAPI.ColosseumUserStatusLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
			{
				param.target = UserID;
			},
			OnReceived = delegate(GameWebAPI.RespData_ColosseumUserStatusLogic resData)
			{
				this.colosseumUserStatus = resData.userStatus;
			}
		};
		yield return base.StartCoroutine(request.Run(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
		OnEnded();
		yield break;
	}

	protected abstract void SetColosseumUserStatus();
}
