using Master;
using System;
using UnityEngine;

namespace Colosseum.Matching
{
	public sealed class CMD_ColosseumMatching : CMDWrapper
	{
		[SerializeField]
		private GameObject cancelButton;

		private Collider cancelButtonCollider;

		private UISprite cancelButtonSprite;

		[SerializeField]
		private Color cancelButtonDisableColor = new Color(0.3921f, 0.3921f, 0.3921f, 1f);

		[SerializeField]
		private UILabel cancelLabel;

		[SerializeField]
		private Color cancelLabelDisableColor = new Color(0.5882f, 0.5882f, 0.5882f, 1f);

		[SerializeField]
		private PVPPartySelect3 monsterSelectUI;

		[SerializeField]
		private ColosseumMatchingAnimation modelAnimation;

		[SerializeField]
		private UITexture loadingBackground;

		private PvPVersusInfo6Icon versusInfoUI;

		private ColosseumMatchingEventListener matchingEventListener;

		private bool isChangeBattleScene;

		private string errorTitleKey;

		private string errorInfoKey;

		private string alertErrorCode;

		private bool isShowDialog;

		private bool isCancelMatching;

		private void OnApplicationPause(bool isPause)
		{
			if (!isPause && !this.isCancelMatching)
			{
				CMD_Alert cmd_Alert = UnityEngine.Object.FindObjectOfType<CMD_Alert>();
				if (null != cmd_Alert)
				{
					cmd_Alert.SetLastCallBack(new Action(this.matchingEventListener.OnResumeApplication));
				}
				else
				{
					this.matchingEventListener.OnResumeApplication();
				}
			}
		}

		private void DestroyExceptionClose()
		{
			if (null != this.versusInfoUI)
			{
				UnityEngine.Object.Destroy(this.versusInfoUI.gameObject);
				this.versusInfoUI = null;
			}
			if (this.monsterSelectUI.gameObject.activeSelf)
			{
				this.monsterSelectUI.gameObject.SetActive(false);
			}
		}

		protected override void OnShowDialog()
		{
			this.cancelButtonCollider = this.cancelButton.GetComponent<BoxCollider>();
			this.cancelButtonSprite = this.cancelButton.GetComponent<UISprite>();
			Screen.sleepTimeout = -1;
		}

		protected override void OnOpenedDialog()
		{
			this.matchingEventListener.InitializeMatching();
		}

		protected override bool OnCloseStartDialog()
		{
			bool result = true;
			this.isCancelMatching = true;
			if (!this.isChangeBattleScene && Singleton<TCPUtil>.Instance.CheckTCPConnection())
			{
				this.matchingEventListener.OnCancelMatching();
				result = false;
			}
			else if (!string.IsNullOrEmpty(this.errorTitleKey) || !string.IsNullOrEmpty(this.errorInfoKey))
			{
				this.DestroyExceptionClose();
				AlertManager.ShowModalMessage(delegate(int noop)
				{
					this.errorTitleKey = string.Empty;
					this.errorInfoKey = string.Empty;
					this.ClosePanel(true);
				}, StringMaster.GetString(this.errorTitleKey), StringMaster.GetString(this.errorInfoKey), AlertManager.ButtonActionType.Close, false);
				result = false;
			}
			else if (!string.IsNullOrEmpty(this.alertErrorCode))
			{
				this.DestroyExceptionClose();
				AlertManager.ShowAlertDialog(delegate(int noop)
				{
					this.alertErrorCode = string.Empty;
					this.ClosePanel(true);
				}, this.alertErrorCode);
				result = false;
			}
			return result;
		}

		protected override void OnClosedDialog()
		{
			Screen.sleepTimeout = -2;
			this.modelAnimation.DeleteObject();
		}

		public static CMD_ColosseumMatching Create(MatchingConfig matchingConfig)
		{
			CMD_ColosseumMatching cmd_ColosseumMatching = null;
			IColosseumMatchingInfo colosseumMatchingInfo;
			if (matchingConfig.IsMockBattle())
			{
				colosseumMatchingInfo = new ColosseumMatchingInfoMockBattle(matchingConfig);
			}
			else
			{
				colosseumMatchingInfo = new ColosseumMatchingInfoMainBattle(matchingConfig);
			}
			string dungeonId = colosseumMatchingInfo.GetDungeonId();
			if (string.IsNullOrEmpty(dungeonId))
			{
				AlertManager.ShowModalMessage(null, "ColosseumCloseTime", "ColosseumGoTop", AlertManager.ButtonActionType.Close, false);
			}
			else
			{
				CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMD_ColosseumMatching");
				cmd_ColosseumMatching = commonDialog.GetComponent<CMD_ColosseumMatching>();
				cmd_ColosseumMatching.matchingEventListener = cmd_ColosseumMatching.GetComponent<ColosseumMatchingEventListener>();
				cmd_ColosseumMatching.matchingEventListener.SetInstance(cmd_ColosseumMatching, matchingConfig, colosseumMatchingInfo, dungeonId, cmd_ColosseumMatching.modelAnimation);
				cmd_ColosseumMatching.modelAnimation.SetInstance(cmd_ColosseumMatching.matchingEventListener);
				cmd_ColosseumMatching.Show();
			}
			return cmd_ColosseumMatching;
		}

		public PvPVersusInfo6Icon CreateVersusInfo()
		{
			this.versusInfoUI = PvPVersusInfo6Icon.CreateInstance(Singleton<GUIMain>.Instance.transform);
			return this.versusInfoUI;
		}

		public PVPPartySelect3 GetMonsterSelectUI()
		{
			return this.monsterSelectUI;
		}

		public void ChangeBattleScene()
		{
			this.isChangeBattleScene = true;
			this.monsterSelectUI.gameObject.SetActive(false);
			this.versusInfoUI.SetBackground(this.loadingBackground);
			this.versusInfoUI.gameObject.SetActive(true);
			this.versusInfoUI.AnimaObjectActiveSet(false);
			base.SetCloseAction(delegate(int x)
			{
				ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
				RestrictionInput.SetDisplayObject(this.versusInfoUI.gameObject);
				this.versusInfoUI = null;
				BattleStateManager.StartPvP(0.5f, 0.5f, true, null);
			});
			if (null != CMD_PvPTop.Instance)
			{
				CMD_PvPTop.Instance.IsToBattle = true;
			}
			this.useCMDAnim = false;
			GUIManager.CloseAllCommonDialog(null);
		}

		public void EnableCancelButton(bool enabled)
		{
			GUIManager.ExtBackKeyReady = enabled;
			this.cancelButtonCollider.enabled = enabled;
			if (enabled)
			{
				this.cancelButtonSprite.spriteName = "Common02_Btn_SupportRed";
				this.cancelButtonSprite.color = Color.white;
				this.cancelLabel.color = Color.white;
			}
			else
			{
				this.cancelButtonSprite.spriteName = "Common02_Btn_SupportWhite";
				this.cancelButtonSprite.color = this.cancelButtonDisableColor;
				this.cancelLabel.color = this.cancelLabelDisableColor;
			}
		}

		public void HideCancelButton()
		{
			this.cancelButton.SetActive(false);
		}

		public void SetErrorData(string titleKey, string infoKey)
		{
			this.errorTitleKey = titleKey;
			this.errorInfoKey = infoKey;
		}

		public void SetAlertData(string errorCode)
		{
			this.alertErrorCode = errorCode;
		}
	}
}
