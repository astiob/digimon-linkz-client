using Master;
using System;
using System.Collections;
using UnityEngine;

public class GUIListPartsQuestRanking : GUIListPartBS
{
	[Header("あなたを示すアイコン")]
	[SerializeField]
	private UISprite spYouIcon;

	[Header("キャラサムネの位置")]
	[SerializeField]
	private GameObject goMONSTER_ICON;

	[Header("ユーザーネーム")]
	[SerializeField]
	private UILabel lbTX_UserName;

	[Header("称号アイコン")]
	[SerializeField]
	private GameObject goTITLE_ICON;

	[SerializeField]
	[Header("ポイント")]
	private UILabel lbTX_DuelPoint;

	[Header("Nextポイント")]
	[SerializeField]
	private UILabel lbTX_NextPoint;

	[Header("ランキング順位")]
	[SerializeField]
	private UILabel lbTX_RankingNumber;

	[Header("ランキングアイコン")]
	[SerializeField]
	private UISprite spRankingIcon;

	private MonsterData digimonData;

	private GUIMonsterIcon csMonsIcon;

	private GameWebAPI.RespDataMS_PointQuestRankingList.RankingData data;

	private int nextPoint;

	private int limitRank;

	public override void SetData()
	{
		if (CMD_PointQuestRanking.instance != null)
		{
			this.data = CMD_PointQuestRanking.instance.GetData();
			this.data.titleId = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.titleId;
			this.nextPoint = CMD_PointQuestRanking.instance.GetNextPoint();
			this.limitRank = CMD_PointQuestRanking.instance.GetlimitRank();
		}
		if (CMD_ColosseumRanking.instance != null)
		{
			this.data = default(GameWebAPI.RespDataMS_PointQuestRankingList.RankingData);
			this.data.iconId = CMD_ColosseumRanking.instance.myData.leaderMonsterId.ToString();
			this.data.nickname = CMD_ColosseumRanking.instance.myData.nickname;
			this.data.titleId = CMD_ColosseumRanking.instance.myData.titleId;
			this.data.userId = CMD_ColosseumRanking.instance.myData.userId.ToString();
			this.data.score = CMD_ColosseumRanking.instance.myData.point.ToString();
			this.data.rank = CMD_ColosseumRanking.instance.myData.rank.ToString();
			this.nextPoint = CMD_ColosseumRanking.instance.GetNextPoint();
			this.limitRank = CMD_ColosseumRanking.instance.GetlimitRank();
		}
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.ShowData();
		base.ShowGUI();
	}

	private void ShowData()
	{
		this.SetDigimonIcon();
		this.lbTX_UserName.text = this.data.nickname;
		TitleDataMng.SetTitleIcon(this.data.titleId, this.goTITLE_ICON.GetComponent<UITexture>());
		this.lbTX_DuelPoint.text = this.data.score;
		this.lbTX_NextPoint.text = this.nextPoint.ToString();
		int num = int.Parse(this.data.rank);
		switch (num)
		{
		case 1:
			this.spRankingIcon.gameObject.SetActive(true);
			this.spRankingIcon.spriteName = "PvP_Ranking_1";
			this.lbTX_RankingNumber.text = string.Empty;
			break;
		case 2:
			this.spRankingIcon.gameObject.SetActive(true);
			this.spRankingIcon.spriteName = "PvP_Ranking_2";
			this.lbTX_RankingNumber.text = string.Empty;
			break;
		case 3:
			this.spRankingIcon.gameObject.SetActive(true);
			this.spRankingIcon.spriteName = "PvP_Ranking_3";
			this.lbTX_RankingNumber.text = string.Empty;
			break;
		default:
			this.spRankingIcon.gameObject.SetActive(false);
			if (1 <= num && num <= this.limitRank)
			{
				this.lbTX_RankingNumber.text = num.ToString();
			}
			else
			{
				this.lbTX_RankingNumber.text = StringMaster.GetString("ColosseumRankOutside");
			}
			break;
		}
	}

	private void SetBG()
	{
		this.spYouIcon.gameObject.SetActive(false);
	}

	private void SetDigimonIcon()
	{
		if (this.digimonData == null)
		{
			this.digimonData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.iconId);
			this.csMonsIcon = GUIMonsterIcon.MakePrefabByMonsterData(this.digimonData, this.goMONSTER_ICON.transform.localScale, this.goMONSTER_ICON.transform.localPosition, this.goMONSTER_ICON.transform.parent, true, true);
			UIWidget component = this.goMONSTER_ICON.GetComponent<UIWidget>();
			UIWidget component2 = this.csMonsIcon.gameObject.GetComponent<UIWidget>();
			if (component != null && component2 != null)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = this.csMonsIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(this.csMonsIcon.transform, add);
			}
			this.goMONSTER_ICON.SetActive(false);
		}
		else
		{
			this.digimonData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.iconId);
			this.csMonsIcon.RefreshPrefabByMonsterData(this.digimonData);
		}
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
		this.beganPostion = pos;
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
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f)
			{
				this.OnTouchEndedProcess();
			}
		}
		base.OnTouchEnded(touch, pos, flag);
	}

	protected virtual void OnTouchEndedProcess()
	{
		if (this.data.userId == DataMng.Instance().RespDataCM_Login.playerInfo.userId)
		{
			GUIMain.ShowCommonDialog(null, "CMD_Profile");
		}
		else
		{
			AppCoroutine.Start(this.OpenProfileFriend(), false);
		}
	}

	private IEnumerator OpenProfileFriend()
	{
		bool isSuccess = false;
		if (BlockManager.instance().blockList == null)
		{
			APIRequestTask task = BlockManager.instance().RequestBlockList(false);
			yield return AppCoroutine.Start(task.Run(delegate
			{
				isSuccess = true;
			}, delegate(Exception noop)
			{
				isSuccess = false;
			}, null), false);
		}
		else
		{
			isSuccess = true;
		}
		if (isSuccess)
		{
			if (this.data.userId.ToInt32() == DataMng.Instance().RespDataCM_Login.playerInfo.UserId)
			{
				GUIMain.ShowCommonDialog(null, "CMD_Profile");
			}
			else
			{
				CMD_ProfileFriend.friendData = new GameWebAPI.FriendList
				{
					userData = new GameWebAPI.FriendList.UserData(),
					monsterData = new GameWebAPI.FriendList.MonsterData(),
					userData = 
					{
						userId = this.data.userId
					}
				};
				GUIMain.ShowCommonDialog(null, "CMD_ProfileFriend");
			}
		}
		yield break;
	}

	private void OnClickedBtnSelect()
	{
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
