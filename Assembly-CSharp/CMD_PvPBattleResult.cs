using Colosseum;
using Master;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_PvPBattleResult : CMD
{
	[Header("Winのロゴ")]
	[SerializeField]
	private GameObject winLogo;

	[Header("Loseのロゴ")]
	[SerializeField]
	private GameObject loseLogo;

	[Header("スキップ用Winのロゴ")]
	[SerializeField]
	private GameObject winLogoForSkip;

	[Header("獲得クラスタ表示ルート")]
	[SerializeField]
	private GameObject acquisitionRoot;

	[Header("DP表示ルート")]
	[SerializeField]
	private GameObject dpRoot;

	[Header("ランク表示用スプライト")]
	[SerializeField]
	private UISprite rankSprite;

	[Header("獲得クラスタ数値ラベル")]
	[SerializeField]
	private UILabel getClusterLabel;

	[Header("変動DP数値ラベル")]
	[SerializeField]
	private UILabel fluctuateDpLabel;

	[Header("現在のDP数値ラベル")]
	[SerializeField]
	private UILabel currentDpLabel;

	[Header("通算勝利数のラベル")]
	[SerializeField]
	private UILabel totalWinNum;

	[Header("次のランクアップまでの勝利数ラベル")]
	[SerializeField]
	private UILabel nextRankupWinNum;

	[Header("ランクアップ用オブジェクト")]
	[SerializeField]
	private GameObject gaugeUp;

	[Header("ランクダウン用オブジェクト")]
	[SerializeField]
	private GameObject gaugeDown;

	[Header("ランクアップ用エフェクト")]
	[SerializeField]
	private GameObject rankUpEffect;

	[Header("ランクダウン用エフェクト")]
	[SerializeField]
	private GameObject rankDownEffect;

	[Header("TAP NEXTのオブジェクト")]
	[SerializeField]
	private GameObject tapNext;

	[Header("加算DPのTween")]
	[SerializeField]
	private EfcCont fluctuateDpTween;

	private readonly Vector3 BIG_LABEL_SIZE = new Vector3(1.35f, 1.35f, 1f);

	[Header("カウントアップするときのラベルの色")]
	[SerializeField]
	private Color countUpLabelColor = new Color32(byte.MaxValue, 240, 0, byte.MaxValue);

	private GUIMonsterIcon[] digimonIcons;

	private Coroutine ClusterCoroutine;

	private Coroutine DpCoroutine;

	private Coroutine DpSubCoroutine;

	private Coroutine DpTotalCoroutine;

	private Coroutine RankUpCoroutine;

	private Coroutine RankUpSubCoroutine;

	private Action endTask;

	private bool isContinueCoroutine;

	private bool isContinueGauge;

	private int getRewardClusterNum;

	private bool isMockBattle;

	private int nowTotalWin;

	private const int UP_COUNT_VALUE = 1;

	private CMD_PvPBattleResult.SKIP_SUCCESS currentSkipSuccess;

	private MultiBattleData multiBattleData;

	private GameWebAPI.ColosseumUserStatus previousUserStatus;

	private MultiBattleData.BattleEndResponseData pvpResultData;

	private Dictionary<string, GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank> rankDataDict = new Dictionary<string, GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank>();

	protected override void Awake()
	{
		base.Awake();
		this.InitInfo();
		this.HideItems();
	}

	private void InitInfo()
	{
		this.multiBattleData = ClassSingleton<MultiBattleData>.Instance;
		this.isMockBattle = !(this.multiBattleData.MockBattleUserCode == "0");
		this.previousUserStatus = this.multiBattleData.PvPUserDatas.Where((MultiBattleData.PvPUserData item) => item.userStatus.userId == this.multiBattleData.MyPlayerUserId).Select((MultiBattleData.PvPUserData item) => item.userStatus).First<GameWebAPI.ColosseumUserStatus>();
		this.pvpResultData = this.multiBattleData.BattleEndResponse;
		foreach (GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank colosseumRank in MasterDataMng.Instance().RespDataMA_ColosseumRankMaster.colosseumRankM)
		{
			this.rankDataDict.Add(colosseumRank.colosseumRankId, colosseumRank);
		}
		this.getClusterLabel.text = "0";
		this.fluctuateDpLabel.text = "0";
		GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank colosseumRank2 = this.rankDataDict[this.previousUserStatus.colosseumRankId.ToString()];
		this.currentDpLabel.text = this.previousUserStatus.score.ToString();
		bool flag = this.multiBattleData.BattleResult == 1;
		if (flag && !this.isMockBattle)
		{
			this.nowTotalWin = this.previousUserStatus.winTotal + 1;
		}
		else
		{
			this.nowTotalWin = this.previousUserStatus.winTotal;
		}
		this.totalWinNum.text = string.Format(StringMaster.GetString("MyColosseumTotalWinNum"), this.nowTotalWin.ToString());
		if (this.isMockBattle)
		{
			this.nextRankupWinNum.text = string.Empty;
		}
		else if (this.pvpResultData.battleRecord != null)
		{
			this.nextRankupWinNum.text = string.Format(StringMaster.GetString("ColosseumRankAGroup"), this.pvpResultData.battleRecord.count, this.pvpResultData.battleRecord.winPercent);
		}
		else
		{
			this.nextRankupWinNum.text = string.Format(StringMaster.GetString("NextRankupNum"), int.Parse(colosseumRank2.maxScore) + 1 - this.nowTotalWin);
		}
		this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(int.Parse(colosseumRank2.colosseumRankId));
		this.UpdateItemNum();
	}

	private void UpdateItemNum()
	{
		if (this.multiBattleData.BattleEndResponse != null && this.multiBattleData.BattleEndResponse.reward != null)
		{
			foreach (MultiBattleData.BattleEndResponseData.Reward reward2 in this.multiBattleData.BattleEndResponse.reward)
			{
				if (reward2.assetCategoryId == 6)
				{
					Singleton<UserDataMng>.Instance.UpdateUserItemNum(reward2.assetValue, reward2.assetNum);
				}
			}
		}
	}

	private void HideItems()
	{
		NGUITools.SetActiveSelf(this.winLogo, false);
		NGUITools.SetActiveSelf(this.winLogoForSkip, false);
		NGUITools.SetActiveSelf(this.loseLogo, false);
		NGUITools.SetActiveSelf(this.gaugeUp, false);
		NGUITools.SetActiveSelf(this.gaugeDown, false);
		NGUITools.SetActiveSelf(this.tapNext, false);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.CreateDigimonThumbnail();
		bool flag = this.multiBattleData.BattleResult == 1;
		if (flag)
		{
			SoundMng.Instance().PlayGameBGM("bgm_303");
		}
		else
		{
			SoundMng.Instance().PlayGameBGM("bgm_304");
		}
		this.RunWinAnimation();
		if (GooglePlayGamesTool.Instance != null)
		{
			GooglePlayGamesTool.Instance.ClearQuest();
		}
		aT = 0f;
		base.Show(f, sizeX, sizeY, aT);
	}

	private void CreateDigimonThumbnail()
	{
		GUIMonsterIcon[] componentsInChildren = base.GetComponentsInChildren<GUIMonsterIcon>();
		this.digimonIcons = new GUIMonsterIcon[componentsInChildren.Length];
		for (int i = 0; i < ColosseumData.LastUseMonsterList.Length; i++)
		{
			MonsterData monsterData = ColosseumData.LastUseMonsterList[i];
			this.digimonIcons[i] = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, componentsInChildren[i].transform.localScale, componentsInChildren[i].transform.localPosition, base.transform, true, false);
			this.digimonIcons[i].name = "DigimonIcon" + i;
			this.digimonIcons[i].activeCollider = false;
		}
		foreach (GUIMonsterIcon guimonsterIcon in componentsInChildren)
		{
			NGUITools.SetActiveSelf(guimonsterIcon.gameObject, false);
		}
	}

	private void RunWinAnimation()
	{
		bool isWin = this.multiBattleData.BattleResult == 1;
		base.StartCoroutine(this.WaitTime(0.6f, delegate
		{
			if (isWin)
			{
				Animator component = this.winLogo.GetComponent<Animator>();
				component.enabled = true;
				NGUITools.SetActiveSelf(this.winLogo, true);
				this.winLogo.GetComponent<EffectAnimatorEventTime>().SetEvent(0, new Action(this.FinishWinLoseAnimation));
			}
			else
			{
				Animator component2 = this.loseLogo.GetComponent<Animator>();
				component2.enabled = true;
				NGUITools.SetActiveSelf(this.loseLogo, true);
				this.loseLogo.GetComponent<EffectAnimatorEventTime>().SetEvent(0, new Action(this.FinishWinLoseAnimation));
			}
		}));
	}

	private void FinishWinLoseAnimation()
	{
		this.CountCluster();
	}

	private void CountCluster()
	{
		this.getRewardClusterNum = this.GetRewardClusterNum();
		this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountCluster;
		this.ClusterCoroutine = base.StartCoroutine(this.UpdateClusterCount(this.getRewardClusterNum));
	}

	private int GetRewardClusterNum()
	{
		int result = 0;
		foreach (MultiBattleData.BattleEndResponseData.Reward reward2 in this.multiBattleData.BattleEndResponse.reward)
		{
			if (reward2.assetCategoryId == 4)
			{
				result = reward2.assetNum;
				break;
			}
		}
		return result;
	}

	private IEnumerator UpdateClusterCount(int addClusterNum)
	{
		if (addClusterNum > 0)
		{
			this.getClusterLabel.color = this.countUpLabelColor;
			this.getClusterLabel.transform.localScale = this.BIG_LABEL_SIZE;
			int restClusterNum = addClusterNum;
			float addNum = 1f;
			float frameAddNum = (float)addClusterNum * 0.001f;
			this.isContinueCoroutine = true;
			while (this.isContinueCoroutine)
			{
				addNum += frameAddNum;
				int add = Mathf.Min((int)addNum, restClusterNum);
				if (add == 0)
				{
					add = 1;
				}
				restClusterNum -= add;
				this.getClusterLabel.text = (addClusterNum - restClusterNum).ToString();
				this.isContinueCoroutine = (restClusterNum > 0);
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			this.getClusterLabel.color = Color.white;
			this.getClusterLabel.transform.localScale = Vector3.one;
		}
		this.CountDp();
		yield break;
	}

	private void CountDp()
	{
		this.DpCoroutine = base.StartCoroutine(this.UpdateDpCount());
	}

	private IEnumerator UpdateDpCount()
	{
		if (!this.isMockBattle)
		{
			int addDpNum = this.pvpResultData.score - this.previousUserStatus.score;
			bool isPlusCount = addDpNum > 0;
			string mark = (!isPlusCount) ? string.Empty : "+";
			this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountAddDp;
			if (addDpNum != 0)
			{
				this.DpSubCoroutine = base.StartCoroutine(this.UpdateFluctuateDpCount(isPlusCount, mark, addDpNum));
				yield return this.DpSubCoroutine;
			}
			this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountDp;
			if (addDpNum != 0)
			{
				this.DpTotalCoroutine = base.StartCoroutine(this.UpdateTotalDpCount(addDpNum));
				yield return this.DpTotalCoroutine;
			}
			if (this.pvpResultData.colosseumRankId > this.previousUserStatus.colosseumRankId)
			{
				this.RankUpCoroutine = base.StartCoroutine(this.PlayRankUp());
				yield return this.RankUpCoroutine;
			}
			else if (this.pvpResultData.colosseumRankId < this.previousUserStatus.colosseumRankId)
			{
				this.RankUpCoroutine = base.StartCoroutine(this.PlayRankDown());
				yield return this.RankUpCoroutine;
			}
		}
		this.DispTapNextButton();
		yield break;
	}

	private IEnumerator UpdateFluctuateDpCount(bool isPlusCount, string mark, int addDpNum)
	{
		this.fluctuateDpLabel.color = this.countUpLabelColor;
		this.fluctuateDpLabel.transform.localScale = this.BIG_LABEL_SIZE;
		int restDpNum = addDpNum;
		float addNum = 1f;
		float frameAddNum = (float)addDpNum * 0.001f;
		this.isContinueCoroutine = true;
		while (this.isContinueCoroutine)
		{
			addNum += frameAddNum;
			int add = 0;
			if (isPlusCount)
			{
				add = Mathf.Min((int)addNum, restDpNum);
				if (add == 0)
				{
					add = 1;
				}
			}
			else
			{
				add = Math.Max((int)(-(int)addNum), restDpNum);
				if (add == 0)
				{
					add = -1;
				}
			}
			restDpNum -= add;
			this.UpdateAddDpInfo(mark, addDpNum, restDpNum);
			this.isContinueCoroutine = (Math.Abs(restDpNum) > 0);
			yield return null;
		}
		this.fluctuateDpLabel.color = Color.white;
		this.fluctuateDpLabel.transform.localScale = Vector3.one;
		yield break;
	}

	private void UpdateAddDpInfo(string mark, int addDpNum, int restDpNum)
	{
		if (addDpNum - restDpNum == 0)
		{
			mark = string.Empty;
		}
		this.fluctuateDpLabel.text = mark + (addDpNum - restDpNum).ToString();
	}

	private IEnumerator UpdateTotalDpCount(int addDpNum)
	{
		this.currentDpLabel.color = this.countUpLabelColor;
		this.currentDpLabel.transform.localScale = this.BIG_LABEL_SIZE;
		int restDpNum = addDpNum;
		float addNum = 1f;
		float frameAddNum = (float)addDpNum * 0.001f;
		this.isContinueCoroutine = true;
		while (this.isContinueCoroutine)
		{
			addNum += frameAddNum;
			int add = 0;
			add = Mathf.Min((int)addNum, restDpNum);
			if (add == 0)
			{
				add = 1;
			}
			restDpNum -= add;
			this.UpdateDpInfo(addDpNum);
			this.isContinueCoroutine = (Math.Abs(restDpNum) > 0);
			yield return null;
		}
		this.currentDpLabel.color = Color.white;
		this.currentDpLabel.transform.localScale = Vector3.one;
		yield break;
	}

	private void UpdateDpInfo(int currentDpNum)
	{
		this.currentDpLabel.text = (this.previousUserStatus.score + currentDpNum).ToString();
	}

	private IEnumerator PlayRankUp()
	{
		this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(this.pvpResultData.colosseumRankId);
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_218", 0f, false, true, null, -1, 1f);
		this.PlayRankUpEffect();
		yield return new WaitForSeconds(1f);
		yield break;
	}

	private void PlayRankUpEffect()
	{
		NGUITools.SetActiveSelf(this.rankUpEffect, true);
		Animator component = this.rankUpEffect.GetComponent<Animator>();
		component.enabled = true;
		int fullPathHash = component.GetCurrentAnimatorStateInfo(0).fullPathHash;
		component.Play(fullPathHash, 0, 0f);
	}

	private IEnumerator PlayRankDown()
	{
		this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(this.pvpResultData.colosseumRankId);
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_219", 0f, false, true, null, -1, 1f);
		this.PlayRankDownEffect();
		yield return new WaitForSeconds(1f);
		yield break;
	}

	private void PlayRankDownEffect()
	{
		NGUITools.SetActiveSelf(this.rankDownEffect, true);
		Animator component = this.rankDownEffect.GetComponent<Animator>();
		component.enabled = true;
		int fullPathHash = component.GetCurrentAnimatorStateInfo(0).fullPathHash;
		component.Play(fullPathHash, 0, 0f);
	}

	private void DispTapNextButton()
	{
		this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.End;
		NGUITools.SetActiveSelf(this.tapNext, true);
		base.StartCoroutine(this.WaitTime(0.6f, delegate
		{
			this.endTask = new Action(this.ShowRewardDialog);
		}));
	}

	private void ShowRewardDialog()
	{
		NGUITools.SetActiveSelf(this.rankUpEffect, false);
		NGUITools.SetActiveSelf(this.rankDownEffect, false);
		ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult = null;
		if (this.multiBattleData.BattleEndResponse == null || this.multiBattleData.BattleEndResponse.firstRankUpReward == null || this.multiBattleData.BattleEndResponse.firstRankUpReward.Length == 0)
		{
			this.ShowRewardDialog(0);
		}
		else
		{
			CMD_FirstClear cmd_FirstClear = GUIMain.ShowCommonDialog(new Action<int>(this.ShowRewardDialog), "CMD_PvPIncentive", null) as CMD_FirstClear;
			cmd_FirstClear.SetRankUpRewardTitle();
		}
	}

	private void ShowRewardDialog(int i)
	{
		if (this.multiBattleData.BattleEndResponse == null || this.multiBattleData.BattleEndResponse.reward == null || this.multiBattleData.BattleEndResponse.reward.Length == 0)
		{
			this.GotoPvPTop(0);
		}
		else
		{
			CMD_FirstClear.isNormalReward = true;
			CMD_FirstClear cmd_FirstClear = GUIMain.ShowCommonDialog(new Action<int>(this.GotoPvPTop), "CMD_PvPIncentive", null) as CMD_FirstClear;
			if (this.multiBattleData.BattleResult == 1)
			{
				cmd_FirstClear.SetWinRewardTitle();
			}
			else
			{
				cmd_FirstClear.SetTitleHide();
			}
		}
	}

	private void GotoPvPTop(int i)
	{
		SoundMng.Instance().PlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1, 1f);
		this.ClosePanel(true);
	}

	private IEnumerator WaitTime(float time, Action finish)
	{
		yield return new WaitForSeconds(time);
		finish();
		yield break;
	}

	private void OnTapped()
	{
		switch (this.currentSkipSuccess)
		{
		case CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountCluster:
			this.SkipPlayCountCluster();
			break;
		case CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountAddDp:
			this.SkipPlayCountAddDp();
			break;
		case CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountDp:
			this.SkipPlayCountDp();
			break;
		case CMD_PvPBattleResult.SKIP_SUCCESS.End:
			if (this.endTask != null)
			{
				this.endTask();
			}
			break;
		}
	}

	private void SkipPlayCountCluster()
	{
		this.isContinueCoroutine = false;
		base.StopCoroutine(this.ClusterCoroutine);
		this.getClusterLabel.text = this.getRewardClusterNum.ToString();
		this.getClusterLabel.color = Color.white;
		this.getClusterLabel.transform.localScale = Vector3.one;
		this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountAddDp;
		this.CountDp();
	}

	private void SkipPlayCountAddDp()
	{
		this.isContinueCoroutine = false;
		this.SkipAddDpCount();
	}

	private void SkipAddDpCount()
	{
		int num = this.pvpResultData.score - this.previousUserStatus.score;
		this.UpdateAddDpInfo((num <= 0) ? string.Empty : "+", num, 0);
		this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountDp;
	}

	private void SkipPlayCountDp()
	{
		if (this.DpCoroutine != null)
		{
			base.StopCoroutine(this.DpCoroutine);
		}
		if (this.DpSubCoroutine != null)
		{
			base.StopCoroutine(this.DpSubCoroutine);
		}
		if (this.DpTotalCoroutine != null)
		{
			base.StopCoroutine(this.DpTotalCoroutine);
		}
		if (this.RankUpCoroutine != null)
		{
			base.StopCoroutine(this.RankUpCoroutine);
		}
		if (this.RankUpSubCoroutine != null)
		{
			base.StopCoroutine(this.RankUpSubCoroutine);
		}
		this.DpCoroutine = null;
		this.DpSubCoroutine = null;
		this.DpTotalCoroutine = null;
		this.RankUpCoroutine = null;
		this.RankUpSubCoroutine = null;
		this.SkipDpCount();
	}

	private void SkipDpCount()
	{
		if (this.pvpResultData.colosseumRankId > this.previousUserStatus.colosseumRankId)
		{
			base.StartCoroutine(this.PlayRankUp());
		}
		else if (this.pvpResultData.colosseumRankId < this.previousUserStatus.colosseumRankId)
		{
			base.StartCoroutine(this.PlayRankDown());
		}
		this.currentDpLabel.text = this.pvpResultData.score.ToString();
		this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.End;
		this.DispTapNextButton();
	}

	private enum SKIP_SUCCESS
	{
		Start,
		PlayCountCluster,
		PlayCountAddDp,
		PlayCountDp,
		End
	}
}
