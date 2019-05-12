using Master;
using Monster;
using MultiBattle.Tools;
using System;
using TMPro;
using UnityEngine;

public sealed class PvPVersusInfo6Icon : MonoBehaviour
{
	[Header("対人戦の名称を表示するスプライト")]
	[SerializeField]
	private TextMeshPro title;

	[Header("ユーザー情報を表示するUIのルート")]
	[SerializeField]
	private EffectAnimatorEventTime userDataUIRoot;

	[SerializeField]
	[Header("自分の情報を表示するUI群")]
	private PvPVersusInfo6Icon.UserDataUI myDataUI;

	[SerializeField]
	[Header("相手の情報を表示するUI群")]
	private PvPVersusInfo6Icon.UserDataUI opponentDataUI;

	[SerializeField]
	[Header("背景のDepth値")]
	private int backgroundDepth;

	[SerializeField]
	private GameObject animaBaseObject;

	private float pausedTime;

	public static PvPVersusInfo6Icon CreateInstance(Transform parentTransform)
	{
		GameObject original = Resources.Load("UIPrefab/PvP/PvPVersusInfo6Icon") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Transform transform = gameObject.transform;
		transform.parent = parentTransform;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		PvPVersusInfo6Icon component = gameObject.GetComponent<PvPVersusInfo6Icon>();
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
				this.title.text = StringMaster.GetString("PVP_SimulatedGame");
			}
		}
		else
		{
			this.title.text = StringMaster.GetString("PVP_NationwideBattle");
		}
		CountrySetting.ConvertTMProText(ref this.title);
	}

	public void SetUserInfo(MultiBattleData.PvPUserData[] userDataList)
	{
		this.SetUserInfo(userDataList[0], this.myDataUI);
		this.SetUserInfo(userDataList[1], this.opponentDataUI);
	}

	public void SetUserInfo(MultiBattleData.PvPUserData myData, MultiBattleData.PvPUserData opponentData)
	{
		this.SetUserInfo(myData, this.myDataUI);
		this.SetUserInfo(opponentData, this.opponentDataUI);
	}

	private void SetUserInfo(MultiBattleData.PvPUserData userData, PvPVersusInfo6Icon.UserDataUI ui)
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
			if (ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userData.monsterData[i].userMonsterId) != null)
			{
				guimonsterIcon.Data = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userData.monsterData[i].userMonsterId);
			}
			else
			{
				guimonsterIcon.Data = userData.monsterData[i].ToMonsterData();
			}
			guimonsterIcon.SetTouchAct_L(null);
			guimonsterIcon.SetTouchAct_S(null);
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

	public void AnimaObjectActiveSet(bool active)
	{
		this.animaBaseObject.SetActive(active);
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

		public GUIMonsterIcon[] icons;
	}

	private enum AnimationEvent
	{
		VS_SE_PLAY,
		ANIMATION_END
	}
}
