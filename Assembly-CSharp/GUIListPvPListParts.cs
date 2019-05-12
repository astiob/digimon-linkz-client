using Master;
using MultiBattle.Tools;
using PvP;
using Quest;
using System;
using System.Collections;
using UnityEngine;

public class GUIListPvPListParts : GUIListPartBS
{
	[SerializeField]
	private UILabel lbUserName;

	[SerializeField]
	private UILabel lbComment;

	[SerializeField]
	private GameObject goMasterIcon;

	[SerializeField]
	private GameObject goTitleIcon;

	private GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MemberList data;

	public GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MockBattleStatus mockBattleStatus = GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MockBattleStatus.SUCCESS;

	public GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MemberList Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.SetInitLabel();
	}

	private void SetInitLabel()
	{
		this.lbUserName.text = this.data.userInfo.nickname;
		this.lbComment.text = string.Format(StringMaster.GetString("ColosseumMockInvitation"), this.data.userInfo.nickname);
		TitleDataMng.SetTitleIcon(this.data.userInfo.titleId, this.goTitleIcon.GetComponent<UITexture>());
		if (this.data.userInfo.monsterId != null)
		{
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.userInfo.monsterId);
			if (monsterData != null)
			{
				GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(monsterData, this.goMasterIcon.transform.localScale, this.goMasterIcon.transform.localPosition, this.goMasterIcon.transform.parent, true, false);
				UIWidget component = base.gameObject.GetComponent<UIWidget>();
				if (component != null)
				{
					DepthController depthController = guimonsterIcon.GetDepthController();
					depthController.AddWidgetDepth(guimonsterIcon.transform, component.depth + 2);
				}
			}
		}
	}

	private void OnClickedInfo()
	{
		bool isRecruitListLock;
		if (CMD_ChatTop.instance != null)
		{
			isRecruitListLock = CMD_ChatTop.instance.isRecruitListLock;
		}
		else
		{
			isRecruitListLock = CMD_MultiRecruitTop.instance.isRecruitListLock;
		}
		if (!isRecruitListLock)
		{
			base.StartCoroutine(this.OnClickedInfoExec());
		}
	}

	private IEnumerator OnClickedInfoExec()
	{
		if (this.mockBattleStatus == GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MockBattleStatus.NOT_ENTRY)
		{
			APIRequestTask task = PvPUtility.RequestMockBattleEntry(true);
			yield return base.StartCoroutine(task.Run(null, null, null));
		}
		if (this.mockBattleStatus == GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MockBattleStatus.BATTLE_INTERRUPTION)
		{
			APIRequestTask task2 = PvPUtility.RequestColosseumBattleEnd(GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult.DEFEAT, GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleMode.MOCK_BATTLE);
			yield return base.StartCoroutine(task2.Run(null, null, null));
		}
		global::Debug.Log("対戦相手UserCode: " + this.data.userInfo.userCode);
		ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode = this.data.userInfo.userCode.Replace(" ", string.Empty);
		GameWebAPI.ColosseumMatchingValidateLogic colosseumMatchingValidateLogic = new GameWebAPI.ColosseumMatchingValidateLogic();
		colosseumMatchingValidateLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumMatchingValidateLogic param)
		{
			param.act = 2;
			param.isMockBattle = 1;
			param.targetUserCode = ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode;
		};
		colosseumMatchingValidateLogic.OnReceived = delegate(GameWebAPI.RespData_ColosseumMatchingValidateLogic res)
		{
			GameWebAPI.RespData_ColosseumMatchingValidateLogic response = res;
		};
		GameWebAPI.ColosseumMatchingValidateLogic request = colosseumMatchingValidateLogic;
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			this.EndValidate(response);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
		yield break;
	}

	private void EndValidate(GameWebAPI.RespData_ColosseumMatchingValidateLogic matchingValidate)
	{
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int resultCode = matchingValidate.resultCode;
		if (resultCode != 1)
		{
			if (resultCode != 93)
			{
			}
			AlertManager.ShowModalMessage(delegate(int modal)
			{
				CMD_ChatTop.instance.OnClickMultiRequestUpdate();
			}, StringMaster.GetString("ColosseumWithdraw"), StringMaster.GetString("ColosseumSelect"), AlertManager.ButtonActionType.Close, false);
		}
		else
		{
			ClassSingleton<QuestData>.Instance.SelectDungeon = null;
			CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.PVP;
			GUIMain.ShowCommonDialog(null, "CMD_PartyEdit");
		}
	}
}
