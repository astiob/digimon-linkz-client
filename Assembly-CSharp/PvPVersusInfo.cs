using Master;
using MultiBattle.Tools;
using System;
using UnityEngine;

public sealed class PvPVersusInfo : MonoBehaviour
{
	[SerializeField]
	[Header("対人戦の名称を表示するスプライト")]
	private UISprite title;

	[Header("ユーザー情報を表示するUIのルート")]
	[SerializeField]
	private EffectAnimatorEventTime userDataUIRoot;

	[Header("自分の情報を表示するUI群")]
	[SerializeField]
	private PvPVersusInfo.UserDataUI myDataUI;

	[SerializeField]
	[Header("相手の情報を表示するUI群")]
	private PvPVersusInfo.UserDataUI opponentDataUI;

	[SerializeField]
	[Header("背景のDepth値")]
	private int backgroundDepth;

	private float pausedTime;

	public static PvPVersusInfo CreateInstance(Transform parentTransform)
	{
		GameObject original = Resources.Load("UIPrefab/PvP/PvPVersusInfo") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Transform transform = gameObject.transform;
		transform.parent = parentTransform;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		PvPVersusInfo component = gameObject.GetComponent<PvPVersusInfo>();
		component.userDataUIRoot.SetEvent(0, delegate
		{
			SoundMng.Instance().PlaySE("SEInternal/Farm/se_220", 0f, false, true, null, -1, 1f);
		});
		return component;
	}

	public void SetTitle(int mode)
	{
		if (mode != 0)
		{
			if (mode == 1)
			{
				this.title.spriteName = "TXT_MockBattle";
			}
		}
		else
		{
			this.title.spriteName = "TXT_NationalBattle";
		}
		this.title.MakePixelPerfect();
	}

	public void SetUserInfo(MultiBattleData.PvPUserData[] userDataList)
	{
		this.SetUserInfo(userDataList[0], this.myDataUI);
		this.SetUserInfo(userDataList[1], this.opponentDataUI);
		this.SetSkillInfo(userDataList[0].monsterData[0].leaderSkillId, this.myDataUI, userDataList[1].monsterData[0].leaderSkillId, this.opponentDataUI);
	}

	private void SetUserInfo(MultiBattleData.PvPUserData userData, PvPVersusInfo.UserDataUI ui)
	{
		ui.name.text = userData.userStatus.nickname;
		TitleDataMng.SetTitleIcon(userData.userStatus.titleId, ui.title);
		ui.rank.spriteName = MultiTools.GetPvPRankSpriteName(userData.userStatus.colosseumRankId);
		ui.koTitle.text = StringMaster.GetString("ColosseumScoreTitle");
		ui.koValue.text = string.Format(StringMaster.GetString("ColosseumScore"), userData.userStatus.winTotal, userData.userStatus.loseTotal);
		ui.dpTitle.text = StringMaster.GetString("ColosseumDuelPoint");
		ui.dpValue.text = userData.userStatus.score.ToString();
		for (int i = 0; i < userData.monsterData.Length; i++)
		{
			GUIMonsterIcon guimonsterIcon = ui.icons[i];
			guimonsterIcon.Data = userData.monsterData[i].ToMonsterData();
			guimonsterIcon.SetTouchAct_L(null);
			guimonsterIcon.SetTouchAct_S(null);
		}
	}

	private void SetSkillInfo(string myLeaderSkillId, PvPVersusInfo.UserDataUI myDataUI, string opponentLeaderSkillId, PvPVersusInfo.UserDataUI opponentDataUI)
	{
		myDataUI.SkillName.text = string.Empty;
		opponentDataUI.SkillName.text = string.Empty;
		foreach (GameWebAPI.RespDataMA_GetSkillM.SkillM skillM2 in MasterDataMng.Instance().RespDataMA_SkillM.skillM)
		{
			if (skillM2.skillId == myLeaderSkillId)
			{
				myDataUI.SkillName.text = skillM2.name;
				myDataUI.SkillDescript.text = skillM2.description;
				if (!string.IsNullOrEmpty(opponentDataUI.SkillName.text))
				{
					break;
				}
			}
			if (skillM2.skillId == opponentLeaderSkillId)
			{
				opponentDataUI.SkillName.text = skillM2.name;
				opponentDataUI.SkillDescript.text = skillM2.description;
				if (!string.IsNullOrEmpty(myDataUI.SkillName.text))
				{
					break;
				}
			}
		}
		if (string.IsNullOrEmpty(myDataUI.SkillName.text))
		{
			myDataUI.SkillName.text = StringMaster.GetString("SystemNone");
			myDataUI.SkillDescript.text = StringMaster.GetString("CharaStatus-01");
		}
		if (string.IsNullOrEmpty(opponentDataUI.SkillName.text))
		{
			opponentDataUI.SkillName.text = StringMaster.GetString("SystemNone");
			opponentDataUI.SkillDescript.text = StringMaster.GetString("CharaStatus-01");
		}
	}

	public void SetActionAnimationEnd(Action action)
	{
		this.userDataUIRoot.SetEvent(1, action);
	}

	public void SetBackground(UITexture background)
	{
		Transform transform = background.transform;
		transform.parent = base.transform;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
		transform.localPosition = Vector3.zero;
		background.depth = this.backgroundDepth;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			this.pausedTime = Time.realtimeSinceStartup;
		}
		else
		{
			int num = (int)(Time.realtimeSinceStartup - this.pausedTime);
			if (num >= ConstValue.MULTI_BATTLE_TIMEOUT_TIME)
			{
				global::Debug.LogErrorFormat("{0}秒経ったので負け.", new object[]
				{
					num
				});
				BattlePvPFunction.isAlreadyLoseBeforeBattle = true;
			}
		}
	}

	[Serializable]
	public class UserDataUI
	{
		public UILabel name;

		public UITexture title;

		public UISprite rank;

		public UILabel koTitle;

		public UILabel koValue;

		public UILabel dpTitle;

		public UILabel dpValue;

		public UILabel SkillName;

		public UILabel SkillDescript;

		public GUIMonsterIcon[] icons;
	}

	private enum AnimationEvent
	{
		VS_SE_PLAY,
		ANIMATION_END
	}
}
