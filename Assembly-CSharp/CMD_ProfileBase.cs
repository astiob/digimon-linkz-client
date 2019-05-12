using CharacterModelUI;
using Master;
using PvP;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebAPIRequest;

[RequireComponent(typeof(DigimonModelPlayer))]
public abstract class CMD_ProfileBase : CMD
{
	[SerializeField]
	protected UITexture ngTargetTex;

	protected GameWebAPI.RespDataPRF_Profile userProfile;

	protected MonsterData leaderMonsterData;

	protected bool isOpenScreen;

	[SerializeField]
	private UILabel fullScreenButtonLabel;

	[SerializeField]
	private GameObject goSCR_HEADER;

	[SerializeField]
	private GameObject goSCR_DETAIL;

	private Vector3 vOrgSCR_HEADER;

	private Vector3 vOrgSCR_DETAIL;

	private Vector3 vPosSCR_HEADER;

	private Vector3 vPosSCR_DETAIL;

	protected CharacterCameraView characterCameraView;

	protected GameWebAPI.ColosseumUserStatus colosseumUserStatus;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		base.Show(closeEvent, sizeX, sizeY, showAnimationTime);
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

	protected abstract void RefreshComponents();

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		UIPanel uipanel = GUIMain.GetUIPanel();
		Vector2 windowSize = uipanel.GetWindowSize();
		this.vOrgSCR_HEADER = this.goSCR_HEADER.transform.localPosition;
		this.vOrgSCR_DETAIL = this.goSCR_DETAIL.transform.localPosition;
		this.vPosSCR_HEADER = this.vOrgSCR_HEADER;
		this.vPosSCR_HEADER.y = windowSize.y;
		this.vPosSCR_DETAIL = this.vOrgSCR_DETAIL;
		this.vPosSCR_DETAIL.x = windowSize.x;
	}

	public override void ClosePanel(bool animation = true)
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
		this.characterCameraView = new CharacterCameraView(this.leaderMonsterData, 1136, 820);
		this.ngTargetTex.mainTexture = this.characterCameraView.renderTex;
	}

	protected void OnClickedScreen()
	{
		CharacterParams characterParams = this.characterCameraView.csRender3DRT.GetCharacterParams();
		DigimonModelPlayer component = base.GetComponent<DigimonModelPlayer>();
		if (!this.isOpenScreen)
		{
			this.MoveTo(this.goSCR_HEADER, this.vPosSCR_HEADER, 0.18f, iTween.EaseType.linear);
			this.MoveTo(this.goSCR_DETAIL, this.vPosSCR_DETAIL, 0.18f, iTween.EaseType.linear);
			this.fullScreenButtonLabel.text = StringMaster.GetString("SystemButtonReturn");
			if (null != component)
			{
				component.MonsterParams = characterParams;
			}
			this.characterCameraView.MoveToCenter(0.18f);
		}
		else
		{
			if (null != characterParams)
			{
				characterParams.transform.localScale = Vector3.one;
			}
			this.MoveTo(this.goSCR_HEADER, this.vOrgSCR_HEADER, 0.18f, iTween.EaseType.linear);
			this.MoveTo(this.goSCR_DETAIL, this.vOrgSCR_DETAIL, 0.18f, iTween.EaseType.linear);
			this.fullScreenButtonLabel.text = StringMaster.GetString("CharaDetailsFullScreen");
			if (null != component)
			{
				component.MonsterParams = null;
			}
			this.characterCameraView.MoveToLeft(0.18f);
		}
		this.isOpenScreen = !this.isOpenScreen;
		this.characterCameraView.enableTouch = this.isOpenScreen;
	}

	protected void MoveTo(GameObject go, Vector3 vP, float time, iTween.EaseType type = iTween.EaseType.linear)
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
			}
		});
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
				PvPUtility.SetPvPTopNoticeCode(resData);
				this.colosseumUserStatus = resData.userStatus;
			}
		};
		RequestBase request2 = request;
		if (CMD_ProfileBase.<>f__mg$cache0 == null)
		{
			CMD_ProfileBase.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
		}
		yield return base.StartCoroutine(request2.Run(CMD_ProfileBase.<>f__mg$cache0, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
		OnEnded();
		yield break;
	}

	protected abstract void SetColosseumUserStatus();
}
