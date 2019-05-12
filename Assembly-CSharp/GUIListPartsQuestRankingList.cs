using Master;
using System;
using UnityEngine;

public class GUIListPartsQuestRankingList : GUIListPartBS
{
	private const string hexColorOutRange = "#707070B4";

	[Header("ベースのスプライト")]
	[SerializeField]
	private UISprite spBase;

	[Header("ベースのSabスプライト")]
	[SerializeField]
	private UISprite spBaseSab;

	[Header("ベースのLineスプライト")]
	[SerializeField]
	private UISprite spBaseLine;

	[Header("ベースのGlowスプライト")]
	[SerializeField]
	private UISprite spBaseGlow;

	[Header("ポイント")]
	[SerializeField]
	private UILabel lbTX_DuelPoint;

	[SerializeField]
	[Header("ランキング順位")]
	private UILabel lbTX_RankingNumber;

	[Header("ランキング順位")]
	[SerializeField]
	private GameObject goIsMine;

	private string[] keyData = new string[2];

	private int valueData;

	private bool isMine;

	private string[] hexColors = new string[]
	{
		"#FF00BCB4",
		"#FF5300B4",
		"#FFA100B4",
		"#FFF725B4",
		"#89FF4AB4",
		"#45FBFFB4",
		"#702BD1B4"
	};

	private string[] hexSabColors = new string[]
	{
		"#FF18C1FF",
		"#FF6500FF",
		"#FFAA0BFF",
		"#FFF300FF",
		"#8BFF00C8",
		"#52FFF1FF",
		"#6D35CAFF"
	};

	private string[] hexLineColors = new string[]
	{
		"#490438FF",
		"#4E1C04FF",
		"#633F00FF",
		"#848C00FF",
		"#2D5418FF",
		"#1D5456FF",
		"#2A154CFF"
	};

	private Color[] selectColor;

	private string[] hexColor;

	public override void SetData()
	{
		if (CMD_PointQuestRanking.instance != null)
		{
			this.keyData = CMD_PointQuestRanking.instance.GetPointRankingKey(base.IDX);
			this.valueData = CMD_PointQuestRanking.instance.GetPointRankingValue(base.IDX);
			this.isMine = CMD_PointQuestRanking.instance.GetIsMine(base.IDX);
		}
		if (CMD_ColosseumRanking.instance != null)
		{
			this.keyData = CMD_ColosseumRanking.instance.GetRankingKey(base.IDX);
			this.valueData = CMD_ColosseumRanking.instance.GetRankingValue(base.IDX);
			this.isMine = CMD_ColosseumRanking.instance.GetIsMine(base.IDX);
		}
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
		base.ShowGUI();
		this.ShowData();
	}

	private void ShowData()
	{
		this.selectColor = new Color[4];
		this.hexColor = new string[]
		{
			this.hexColors[this.hexColors.Length - 1].ToString(),
			this.hexSabColors[this.hexSabColors.Length - 1].ToString(),
			this.hexLineColors[this.hexLineColors.Length - 1].ToString(),
			"#707070B4"
		};
		this.goIsMine.SetActive(false);
		this.spBaseGlow.gameObject.SetActive(false);
		if (this.isMine)
		{
			if (CMD_ColosseumRanking.instance != null && CMD_ColosseumRanking.instance.dispRankingType != CMD_ColosseumRanking.ColosseumRankingType.THIS_TIME)
			{
				this.goIsMine.SetActive(false);
			}
			else
			{
				this.goIsMine.SetActive(true);
			}
			this.spBaseGlow.gameObject.SetActive(true);
		}
		if (base.IDX < this.hexColors.Length - 1)
		{
			this.hexColor[0] = this.hexColors[base.IDX];
			this.hexColor[1] = this.hexSabColors[base.IDX];
			this.hexColor[2] = this.hexLineColors[base.IDX];
		}
		ColorUtility.TryParseHtmlString(this.hexColor[0], out this.selectColor[0]);
		ColorUtility.TryParseHtmlString(this.hexColor[1], out this.selectColor[1]);
		ColorUtility.TryParseHtmlString(this.hexColor[2], out this.selectColor[2]);
		ColorUtility.TryParseHtmlString(this.hexColor[3], out this.selectColor[3]);
		this.spBase.color = this.selectColor[0];
		this.spBaseGlow.color = this.selectColor[0];
		this.spBaseSab.color = this.selectColor[1];
		this.spBaseLine.color = this.selectColor[2];
		this.lbTX_RankingNumber.effectColor = this.selectColor[2];
		if (int.Parse(this.keyData[1]) > 0)
		{
			this.lbTX_DuelPoint.text = this.valueData.ToString();
			this.lbTX_RankingNumber.text = this.keyData[0] + "~\n" + this.keyData[1] + StringMaster.GetString("PointQuestRankingRankLabel");
			this.spBaseSab.gameObject.SetActive(true);
			this.spBaseLine.gameObject.SetActive(true);
		}
		else
		{
			this.spBase.color = this.selectColor[0];
			this.lbTX_RankingNumber.effectColor = this.selectColor[3];
			this.lbTX_DuelPoint.text = string.Empty;
			this.lbTX_RankingNumber.text = StringMaster.GetString("ColosseumRankOutside");
			this.spBase.color = this.selectColor[3];
			this.spBaseSab.gameObject.SetActive(false);
			this.spBaseLine.gameObject.SetActive(false);
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
	}

	private void OnClickedBtnSelect()
	{
		if (CMD_ColosseumRanking.instance != null)
		{
			CMD_ColosseumRanking.instance.DispRankingList(int.Parse(this.keyData[0]), int.Parse(this.keyData[0]) + 99);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
