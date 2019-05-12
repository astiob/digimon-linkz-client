using Master;
using System;
using System.Collections;
using UnityEngine;

public class GUIListPartsPvPRanking : GUIListPartBS
{
	[SerializeField]
	[Header("ベースのスプライト")]
	private UISprite spBase;

	[Header("わたしのカラー")]
	[SerializeField]
	private Color myColor;

	[SerializeField]
	[Header("ほかのカラー")]
	private Color youColor;

	[SerializeField]
	[Header("あなたを示すアイコン")]
	private UISprite spYouIcon;

	[Header("キャラサムネの位置")]
	[SerializeField]
	private GameObject goMONSTER_ICON;

	[Header("ユーザーネーム")]
	[SerializeField]
	private UILabel lbTX_UserName;

	[SerializeField]
	[Header("ポイント")]
	private UILabel lbTX_DuelPoint;

	[Header("戦歴")]
	[SerializeField]
	private UILabel lbTX_Record;

	[Header("ランキング順位")]
	[SerializeField]
	private UILabel lbTX_RankingNumber;

	[Header("ランキングアイコン")]
	[SerializeField]
	private UISprite spRankingIcon;

	private MonsterData digimonData;

	private GUIMonsterIcon csMonsIcon;

	private GameWebAPI.RespDataCL_Ranking.RankingDataList data;

	public GameWebAPI.RespDataCL_Ranking.RankingDataList Data
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

	public override void SetData()
	{
		this.data = CMD_PvPRanking.instance.GetData(base.IDX);
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	public override void InactiveParts()
	{
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
		this.SetBG();
		this.SetDigimonIcon();
		this.lbTX_UserName.text = this.data.nickname;
		if (CMD_PvPRanking.Mode == CMD_PvPRanking.MODE.PvP)
		{
			if (CMD_PvPTop.Instance != null && CMD_PvPTop.Instance.IsAggregate)
			{
				this.lbTX_DuelPoint.text = StringMaster.GetString("PvpAggregate");
				this.lbTX_Record.text = StringMaster.GetString("PvpAggregate");
			}
			else
			{
				this.lbTX_DuelPoint.text = this.data.score;
				float num = float.Parse(this.data.winRate);
				this.lbTX_Record.text = string.Format(StringMaster.GetString("ColosseumRankRate"), this.data.totalBattleCount, this.data.winCount, num.ToString("0.0"));
			}
		}
		else
		{
			this.lbTX_DuelPoint.text = this.data.score;
			if (this.lbTX_Record != null)
			{
				this.lbTX_Record.text = string.Empty;
			}
		}
		int num2 = int.Parse(this.data.rank);
		switch (num2)
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
			if (1 <= num2 && num2 <= 100)
			{
				this.lbTX_RankingNumber.text = this.data.rank;
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
		if (this.data.isMine)
		{
			this.spBase.color = this.myColor;
			global::Debug.Log(base.name);
			this.spYouIcon.gameObject.SetActive(true);
		}
		else
		{
			this.spBase.color = this.youColor;
			this.spYouIcon.gameObject.SetActive(false);
		}
	}

	private void SetDigimonIcon()
	{
		if (this.digimonData == null)
		{
			this.digimonData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.iconId);
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
			this.digimonData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.iconId);
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
		if (this.data.isMine)
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
					userId = this.data.userId
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
