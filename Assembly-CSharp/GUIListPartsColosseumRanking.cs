using Master;
using System;
using System.Collections;
using UnityEngine;

public class GUIListPartsColosseumRanking : GUIListPartBS
{
	[Header("あなたを示すアイコン")]
	[SerializeField]
	private UISprite spYouIcon;

	[SerializeField]
	[Header("キャラサムネの位置")]
	private GameObject goMONSTER_ICON;

	[SerializeField]
	[Header("ユーザーネーム")]
	private UILabel lbTX_UserName;

	[Header("称号アイコン")]
	[SerializeField]
	private GameObject goTITLE_ICON;

	[SerializeField]
	[Header("ポイント")]
	private UILabel lbTX_DuelPoint;

	[Header("ランキング順位")]
	[SerializeField]
	private UILabel lbTX_RankingNumber;

	[SerializeField]
	[Header("ランキングアイコン")]
	private UISprite spRankingIcon;

	[Header("背景スプライト")]
	[SerializeField]
	private UISprite spWindow;

	private MonsterData digimonData;

	private GUIMonsterIcon csMonsIcon;

	private int limitRank;

	public GameWebAPI.RespDataCL_Ranking.RankingData data { get; set; }

	public override void SetData()
	{
		this.data = GUISelectPanelColosseumRanking.partsDataList[base.IDX];
		this.limitRank = CMD_ColosseumRanking.instance.GetlimitRank();
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
		if (this.data.userId.ToString() == DataMng.Instance().RespDataCM_Login.playerInfo.userId)
		{
			this.SetMine(true);
		}
		else
		{
			this.SetMine(false);
		}
		this.lbTX_UserName.text = this.data.nickname;
		TitleDataMng.SetTitleIcon(this.data.titleId, this.goTITLE_ICON.GetComponent<UITexture>());
		this.lbTX_DuelPoint.text = this.data.point.ToString();
		int rank = this.data.rank;
		switch (rank)
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
			if (1 <= rank && rank <= this.limitRank)
			{
				this.lbTX_RankingNumber.text = rank.ToString();
			}
			else
			{
				this.lbTX_RankingNumber.text = StringMaster.GetString("ColosseumRankOutside");
			}
			break;
		}
	}

	private void SetMine(bool isMine)
	{
		this.spYouIcon.gameObject.SetActive(isMine);
		if (isMine)
		{
			this.spWindow.color = new Color32(0, 100, 0, byte.MaxValue);
		}
		else
		{
			this.spWindow.color = new Color32(0, 100, byte.MaxValue, 200);
		}
	}

	private void SetDigimonIcon()
	{
		if (this.data.leaderMonsterId == "0")
		{
			this.data.leaderMonsterId = "81";
		}
		if (this.digimonData == null)
		{
			this.digimonData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.leaderMonsterId);
			this.csMonsIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(this.digimonData, this.goMONSTER_ICON.transform.localScale, this.goMONSTER_ICON.transform.localPosition, this.goMONSTER_ICON.transform.parent, true, true);
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
			this.digimonData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.leaderMonsterId);
			MonsterDataMng.Instance().RefreshPrefabByMonsterData(this.digimonData, this.csMonsIcon, true, true);
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
		if (this.data.userId.ToString() == DataMng.Instance().RespDataCM_Login.playerInfo.userId)
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
			CMD_ProfileFriend.friendData = new GameWebAPI.FriendList
			{
				userData = new GameWebAPI.FriendList.UserData(),
				monsterData = new GameWebAPI.FriendList.MonsterData(),
				userData = 
				{
					userId = this.data.userId.ToString()
				}
			};
			GUIMain.ShowCommonDialog(null, "CMD_ProfileFriend");
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
