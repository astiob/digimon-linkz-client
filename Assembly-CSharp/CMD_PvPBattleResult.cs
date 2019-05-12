using MultiBattle.Tools;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_PvPBattleResult : CMD
{
	private const int UP_COUNT_VALUE = 1;

	[Header("Winのロゴ")]
	[SerializeField]
	private GameObject winLogo;

	[Header("Loseのロゴ")]
	[SerializeField]
	private GameObject loseLogo;

	[SerializeField]
	[Header("スキップ用Winのロゴ")]
	private GameObject winLogoForSkip;

	[Header("獲得クラスタ表示ルート")]
	[SerializeField]
	private GameObject acquisitionRoot;

	[Header("DP表示ルート")]
	[SerializeField]
	private GameObject dpRoot;

	[SerializeField]
	[Header("ランク表示用スプライト")]
	private UISprite rankSprite;

	[SerializeField]
	[Header("獲得クラスタ数値ラベル")]
	private UILabel getClusterLabel;

	[Header("変動DP数値ラベル")]
	[SerializeField]
	private UILabel fluctuateDpLabel;

	[Header("現在のDP数値ラベル")]
	[SerializeField]
	private UILabel currentDpLabel;

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

	[Header("ランク用ゲージスライダー")]
	[SerializeField]
	private UISlider gaugeOrangeSlider;

	[SerializeField]
	[Header("ランク用ゲージスライダー")]
	private UISlider gaugeGraySlider;

	[Header("ランク用ゲージ")]
	[SerializeField]
	private UISprite gaugeRedSprite;

	[SerializeField]
	[Header("ランク用ゲージ")]
	private UISprite gaugeBlueSprite;

	[SerializeField]
	[Header("TAP NEXTのオブジェクト")]
	private GameObject tapNext;

	[SerializeField]
	[Header("加算DPのTween")]
	private EfcCont fluctuateDpTween;

	private readonly Vector3 BIG_LABEL_SIZE = new Vector3(1.35f, 1.35f, 1f);

	[SerializeField]
	[Header("カウントアップするときのラベルの色")]
	private Color countUpLabelColor = new Color32(byte.MaxValue, 240, 0, byte.MaxValue);

	private GameWebAPI.RespDataMN_GetDeckList.DeckList deckData;

	private GUIMonsterIcon[] digimonIcons;

	private Coroutine ClusterCoroutine;

	private Coroutine DpCoroutine;

	private Coroutine DpSubCoroutine;

	private Coroutine RankUpCoroutine;

	private Coroutine RankUpSubCoroutine;

	private Action endTask;

	private bool isContinueCoroutine;

	private bool isContinueGauge;

	private int getRewardClusterNum;

	private bool isMockBattle;

	private CMD_PvPBattleResult.SKIP_SUCCESS currentSkipSuccess;

	private MultiBattleData multiBattleData;

	private GameWebAPI.ColosseumUserStatus previousUserStatus;

	private MultiBattleData.BattleEndResponseData pvpResultData;

	private Dictionary<string, GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank> rankDataDict = new Dictionary<string, GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank>();

	protected override void Awake()
	{
		int num = int.Parse(DataMng.Instance().RespDataMN_DeckList.selectDeckNum) - 1;
		this.deckData = DataMng.Instance().RespDataMN_DeckList.deckList[num];
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
		int score = this.previousUserStatus.score;
		GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank colosseumRank2 = this.rankDataDict[this.previousUserStatus.colosseumRankId.ToString()];
		int num = int.Parse(colosseumRank2.maxScore);
		int num2 = int.Parse(colosseumRank2.minScore);
		float num3 = (float)(score - num2);
		float num4 = (float)(num - num2);
		this.currentDpLabel.text = score.ToString();
		this.gaugeOrangeSlider.value = num3 / num4;
		this.gaugeGraySlider.value = 1f - this.gaugeOrangeSlider.value;
		this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(int.Parse(colosseumRank2.colosseumRankId));
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
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] userMonsterList = DataMng.Instance().RespDataUS_MonsterList.userMonsterList;
		for (int i = 0; i < this.deckData.monsterList.Length; i++)
		{
			GameWebAPI.RespDataMN_GetDeckList.MonsterList deckDigimon = this.deckData.monsterList[i];
			GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList2 = userMonsterList.SingleOrDefault((GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList x) => x.userMonsterId == deckDigimon.userMonsterId);
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(userMonsterList2.monsterId);
			this.digimonIcons[i] = MonsterDataMng.Instance().MakePrefabByMonsterData(monsterData, componentsInChildren[i].transform.localScale, componentsInChildren[i].transform.localPosition, base.transform, true, false);
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
			this.DpSubCoroutine = base.StartCoroutine(this.UpdateFluctuateDpCount(isPlusCount, mark, addDpNum));
			yield return this.DpSubCoroutine;
			yield return new WaitForSeconds(1f);
			this.currentSkipSuccess = CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountDp;
			this.DpSubCoroutine = base.StartCoroutine(this.UpdateDpGaugeCount(isPlusCount, mark, addDpNum));
			yield return this.DpSubCoroutine;
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

	private IEnumerator UpdateFluctuateDpMove()
	{
		this.isContinueCoroutine = true;
		UIWidget slider = this.gaugeOrangeSlider.foregroundWidget;
		Vector2 sliderHalfSize = new Vector2((float)slider.width / 2f, (float)slider.height / 2f);
		Vector2 movePos = new Vector2(this.gaugeGraySlider.transform.localPosition.x + sliderHalfSize.x, this.gaugeGraySlider.transform.localPosition.y - sliderHalfSize.y);
		this.fluctuateDpTween.MoveTo(movePos, 0.6f, delegate(int i)
		{
			this.isContinueCoroutine = false;
			TweenAlpha tweenAlpha = this.fluctuateDpLabel.gameObject.AddComponent<TweenAlpha>();
			tweenAlpha.from = 1f;
			tweenAlpha.to = 0f;
			tweenAlpha.duration = 0.3f;
		}, iTween.EaseType.linear, 0f);
		while (this.isContinueCoroutine)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator UpdateDpGaugeCount(bool isPlusCount, string mark, int addDpNum)
	{
		int currentDpNum = this.previousUserStatus.score;
		GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank currentRank = this.rankDataDict[this.previousUserStatus.colosseumRankId.ToString()];
		int nextDpNum = int.Parse(currentRank.maxScore);
		int prevDpNum = int.Parse(currentRank.minScore);
		currentDpNum -= prevDpNum;
		nextDpNum -= prevDpNum;
		int restDpNum = 0;
		if (isPlusCount)
		{
			restDpNum = ((addDpNum > nextDpNum) ? nextDpNum : addDpNum);
			this.gaugeRedSprite.enabled = false;
			this.gaugeBlueSprite.enabled = true;
		}
		else
		{
			restDpNum = ((Mathf.Abs(addDpNum) > currentDpNum) ? (-currentDpNum) : addDpNum);
			this.gaugeRedSprite.enabled = true;
			this.gaugeBlueSprite.enabled = false;
		}
		int restTotalCount = addDpNum;
		bool isContinue = true;
		while (isContinue)
		{
			int add = 0;
			if (isPlusCount)
			{
				add = Mathf.Min(1, restDpNum);
				if (add == 0)
				{
					add = 1;
				}
			}
			else
			{
				add = Math.Max(-1, restDpNum);
				if (add == 0)
				{
					add = -1;
				}
			}
			currentDpNum += add;
			restDpNum -= add;
			restTotalCount -= add;
			bool isRankUp = false;
			bool isRankDown = false;
			if (isPlusCount && currentDpNum > nextDpNum)
			{
				isRankUp = true;
			}
			else if (!isPlusCount && currentDpNum < 0)
			{
				isRankDown = true;
			}
			if (isRankUp && this.rankDataDict.ContainsKey(currentRank.nextRankId))
			{
				this.UpdateDpInfo(isPlusCount, currentDpNum + prevDpNum, (float)currentDpNum, (float)nextDpNum);
				currentRank = this.rankDataDict[currentRank.nextRankId];
				if (this.RankUpCoroutine != null)
				{
					base.StopCoroutine(this.RankUpCoroutine);
					this.RankUpCoroutine = null;
				}
				this.RankUpCoroutine = base.StartCoroutine(this.PlayRankUp(currentRank));
				yield return this.RankUpCoroutine;
				nextDpNum = int.Parse(currentRank.maxScore);
				prevDpNum = int.Parse(currentRank.minScore);
				currentDpNum = 0;
				nextDpNum -= prevDpNum;
				restDpNum = ((restTotalCount >= nextDpNum) ? nextDpNum : restTotalCount);
			}
			else if (isRankDown && this.rankDataDict.ContainsKey(currentRank.prevRankId))
			{
				this.UpdateDpInfo(isPlusCount, currentDpNum + prevDpNum, (float)currentDpNum, (float)nextDpNum);
				currentRank = this.rankDataDict[currentRank.prevRankId];
				if (this.RankUpCoroutine != null)
				{
					base.StopCoroutine(this.RankUpCoroutine);
					this.RankUpCoroutine = null;
				}
				this.RankUpCoroutine = base.StartCoroutine(this.PlayRankDown(currentRank));
				yield return this.RankUpCoroutine;
				nextDpNum = int.Parse(currentRank.maxScore);
				prevDpNum = int.Parse(currentRank.minScore);
				nextDpNum -= prevDpNum;
				currentDpNum = nextDpNum;
				restDpNum = ((Math.Abs(restTotalCount) > currentDpNum) ? (-currentDpNum) : restTotalCount);
			}
			this.UpdateDpInfo(isPlusCount, currentDpNum + prevDpNum, (float)currentDpNum, (float)nextDpNum);
			this.UpdateAddDpInfo((!isPlusCount) ? string.Empty : "+", restTotalCount, 0);
			isContinue = (Math.Abs(restTotalCount) > 0);
			yield return null;
		}
		if (isPlusCount)
		{
			yield return base.StartCoroutine(this.AfterUpGauge(0.3f, delegate
			{
			}));
		}
		else
		{
			yield return base.StartCoroutine(this.AfterDownGauge(0.3f, delegate
			{
			}));
		}
		yield break;
	}

	private void UpdateDpInfo(bool isPlusCount, int currentDpNum, float currentBarNum, float nextBarNum)
	{
		this.currentDpLabel.text = currentDpNum.ToString();
		float num = currentBarNum / nextBarNum;
		if (isPlusCount)
		{
			this.gaugeGraySlider.value = 1f - num;
		}
		else
		{
			this.gaugeOrangeSlider.value = num;
		}
	}

	private IEnumerator PlayRankUp(GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank nextRank)
	{
		this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(int.Parse(nextRank.colosseumRankId));
		this.gaugeGraySlider.value = 0f;
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_218", 0f, false, true, null, -1, 1f);
		this.isContinueGauge = true;
		this.GaugeUpEffect();
		this.PlayRankUpEffect();
		while (this.isContinueGauge)
		{
			yield return null;
		}
		yield break;
	}

	private void GaugeUpEffect()
	{
		NGUITools.SetActiveSelf(this.gaugeUp, true);
		Animator component = this.gaugeUp.GetComponent<Animator>();
		component.enabled = true;
		int fullPathHash = component.GetCurrentAnimatorStateInfo(0).fullPathHash;
		component.Play(fullPathHash, 0, 0f);
		this.gaugeUp.GetComponent<EffectAnimatorEventTime>().SetEvent(0, delegate
		{
			if (this.currentSkipSuccess != CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountDp)
			{
				return;
			}
			float duration = 0.3f;
			this.GaugeUpEffectFadeOut(duration);
			if (this.RankUpSubCoroutine != null)
			{
				base.StopCoroutine(this.RankUpSubCoroutine);
				this.RankUpSubCoroutine = null;
			}
			this.RankUpSubCoroutine = base.StartCoroutine(this.AfterUpGauge(duration, delegate
			{
				this.isContinueGauge = false;
				this.gaugeOrangeSlider.value = 0f;
			}));
		});
	}

	private void GaugeUpEffectFadeOut(float duration)
	{
		TweenAlpha tweenAlpha = this.gaugeUp.GetComponent<TweenAlpha>();
		if (tweenAlpha == null)
		{
			tweenAlpha = this.gaugeUp.AddComponent<TweenAlpha>();
		}
		tweenAlpha.from = 1f;
		tweenAlpha.to = 0f;
		tweenAlpha.duration = duration;
		tweenAlpha.PlayForward();
	}

	private void PlayRankUpEffect()
	{
		NGUITools.SetActiveSelf(this.rankUpEffect, true);
		Animator component = this.rankUpEffect.GetComponent<Animator>();
		component.enabled = true;
		int fullPathHash = component.GetCurrentAnimatorStateInfo(0).fullPathHash;
		component.Play(fullPathHash, 0, 0f);
		this.PlayLevelUpGaugeEffect();
	}

	private IEnumerator PlayRankDown(GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank prevRank)
	{
		this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(int.Parse(prevRank.colosseumRankId));
		this.gaugeOrangeSlider.value = 0f;
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_219", 0f, false, true, null, -1, 1f);
		this.isContinueGauge = true;
		this.GaugeDownEffect();
		this.PlayRankDownEffect();
		while (this.isContinueGauge)
		{
			yield return null;
		}
		yield break;
	}

	private void GaugeDownEffect()
	{
		NGUITools.SetActiveSelf(this.gaugeDown, true);
		Animator component = this.gaugeDown.GetComponent<Animator>();
		component.enabled = true;
		int fullPathHash = component.GetCurrentAnimatorStateInfo(0).fullPathHash;
		component.Play(fullPathHash, 0, 0f);
		this.gaugeDown.GetComponent<EffectAnimatorEventTime>().SetEvent(0, delegate
		{
			if (this.currentSkipSuccess != CMD_PvPBattleResult.SKIP_SUCCESS.PlayCountDp)
			{
				return;
			}
			float duration = 0.3f;
			this.GaugeDownEffectFadeOut(duration);
			if (this.RankUpSubCoroutine != null)
			{
				base.StopCoroutine(this.RankUpSubCoroutine);
				this.RankUpSubCoroutine = null;
			}
			this.RankUpSubCoroutine = base.StartCoroutine(this.AfterDownGauge(duration, delegate
			{
				this.isContinueGauge = false;
				this.gaugeGraySlider.value = 0f;
			}));
		});
	}

	private void GaugeDownEffectFadeOut(float duration)
	{
		TweenAlpha tweenAlpha = this.gaugeDown.GetComponent<TweenAlpha>();
		if (tweenAlpha == null)
		{
			tweenAlpha = this.gaugeDown.AddComponent<TweenAlpha>();
		}
		tweenAlpha.from = 1f;
		tweenAlpha.to = 0f;
		tweenAlpha.duration = duration;
		tweenAlpha.PlayForward();
	}

	private void PlayRankDownEffect()
	{
		NGUITools.SetActiveSelf(this.rankDownEffect, true);
		Animator component = this.rankDownEffect.GetComponent<Animator>();
		component.enabled = true;
		int fullPathHash = component.GetCurrentAnimatorStateInfo(0).fullPathHash;
		component.Play(fullPathHash, 0, 0f);
		this.PlayLevelDownGaugeEffect();
	}

	private IEnumerator AfterUpGauge(float duration, Action finish)
	{
		float addGaugeRate = 1f - this.gaugeGraySlider.value;
		float restGaugeRate = addGaugeRate - this.gaugeOrangeSlider.value;
		bool isContinue = true;
		while (isContinue)
		{
			restGaugeRate -= Time.deltaTime / duration;
			this.gaugeOrangeSlider.value = addGaugeRate - restGaugeRate;
			if (restGaugeRate <= 0f)
			{
				isContinue = false;
				this.gaugeOrangeSlider.value = addGaugeRate;
			}
			yield return null;
		}
		finish();
		yield break;
	}

	private IEnumerator AfterDownGauge(float duration, Action finish)
	{
		float addGaugeRate = 1f - this.gaugeOrangeSlider.value;
		float restGaugeRate = addGaugeRate - this.gaugeGraySlider.value;
		bool isContinue = true;
		while (isContinue)
		{
			restGaugeRate -= Time.deltaTime / duration;
			this.gaugeGraySlider.value = addGaugeRate - restGaugeRate;
			if (restGaugeRate <= 0f)
			{
				isContinue = false;
				this.gaugeGraySlider.value = addGaugeRate;
			}
			yield return null;
		}
		finish();
		yield break;
	}

	private void PlayLevelUpGaugeEffect()
	{
		string path = "Cutscenes/NewFX6";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			Transform transform = gameObject.transform;
			transform.parent = this.gaugeGraySlider.transform;
			transform.localPosition = new Vector3((float)(i + 1) * 100f, -15f, 0f);
		}
	}

	private void PlayLevelDownGaugeEffect()
	{
		string path = "Cutscenes/NewFX9";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			Transform transform = gameObject.transform;
			transform.parent = this.gaugeGraySlider.transform;
			transform.localPosition = new Vector3((float)i * 100f, 150f, 0f);
			transform.localScale = new Vector3(80f, 80f, 80f);
			ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren)
			{
				particleSystem.startColor = new Color(0.482352942f, 0.31764707f, 1f, 1f);
			}
		}
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
			this.ShowBpDialog(0);
		}
		else
		{
			GUIMain.ShowCommonDialog(new Action<int>(this.ShowBpDialog), "CMD_PvPIncentive");
		}
	}

	private void ShowBpDialog(int i)
	{
		if (this.GetRewardBattlePointNum() == 0)
		{
			this.GotoPvPTop(0);
		}
		else
		{
			CMD_FirstClear.isRewardBattlePoint = true;
			GUIMain.ShowCommonDialog(new Action<int>(this.GotoPvPTop), "CMD_PvPIncentive");
		}
	}

	private int GetRewardBattlePointNum()
	{
		int result = 0;
		if (this.multiBattleData.BattleEndResponse == null || this.multiBattleData.BattleEndResponse.reward == null || this.multiBattleData.BattleEndResponse.reward.Length == 0)
		{
			return 0;
		}
		foreach (MultiBattleData.BattleEndResponseData.Reward reward2 in this.multiBattleData.BattleEndResponse.reward)
		{
			if (reward2.assetCategoryId == 6 && reward2.assetValue == 2)
			{
				result = reward2.assetNum;
				break;
			}
		}
		return result;
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
		base.StopCoroutine(this.DpCoroutine);
		base.StopCoroutine(this.DpSubCoroutine);
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
		this.RankUpCoroutine = null;
		this.RankUpSubCoroutine = null;
		NGUITools.SetActiveSelf(this.gaugeUp, false);
		NGUITools.SetActiveSelf(this.gaugeDown, false);
		this.SkipDpCount();
	}

	private void SkipDpCount()
	{
		int num = this.previousUserStatus.score;
		GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank colosseumRank = this.rankDataDict[this.previousUserStatus.colosseumRankId.ToString()];
		int num2 = int.Parse(colosseumRank.maxScore);
		int num3 = int.Parse(colosseumRank.minScore);
		int num4 = this.pvpResultData.score - this.previousUserStatus.score;
		num -= num3;
		num2 -= num3;
		bool flag = num4 > 0;
		int num5;
		if (flag)
		{
			num5 = ((num4 > num2) ? num2 : num4);
		}
		else
		{
			num5 = ((Mathf.Abs(num4) > num) ? (-num) : num4);
		}
		int num6 = num4;
		bool flag2 = true;
		while (flag2)
		{
			int num7;
			if (flag)
			{
				num7 = Mathf.Min(1, num5);
				if (num7 == 0)
				{
					num7 = 1;
				}
			}
			else
			{
				num7 = Math.Max(-1, num5);
				if (num7 == 0)
				{
					num7 = -1;
				}
			}
			num += num7;
			num5 -= num7;
			num6 -= num7;
			bool flag3 = false;
			bool flag4 = false;
			if (flag && num > num2 && this.rankDataDict.ContainsKey(colosseumRank.nextRankId))
			{
				colosseumRank = this.rankDataDict[colosseumRank.nextRankId];
				flag3 = true;
				num2 = int.Parse(colosseumRank.maxScore);
				num3 = int.Parse(colosseumRank.minScore);
				num = 0;
				num2 -= num3;
				num5 = ((num6 >= num2) ? num2 : num6);
			}
			else if (!flag && num < 0 && this.rankDataDict.ContainsKey(colosseumRank.prevRankId))
			{
				colosseumRank = this.rankDataDict[colosseumRank.prevRankId];
				flag4 = true;
				num2 = int.Parse(colosseumRank.maxScore);
				num3 = int.Parse(colosseumRank.minScore);
				num2 -= num3;
				num = num2;
				num5 = ((Math.Abs(num6) > num) ? (-num) : num6);
			}
			if (flag3)
			{
				this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(int.Parse(colosseumRank.colosseumRankId));
				this.GaugeUpEffect();
				this.gaugeUp.GetComponent<EffectAnimatorEventTime>().SetEvent(0, delegate
				{
					float duration = 0.3f;
					this.GaugeUpEffectFadeOut(duration);
				});
				this.PlayRankUpEffect();
				SoundMng.Instance().PlaySE("SEInternal/Farm/se_218", 0f, false, true, null, -1, 1f);
			}
			else if (flag4)
			{
				this.rankSprite.spriteName = MultiTools.GetPvPRankSpriteName(int.Parse(colosseumRank.colosseumRankId));
				this.GaugeDownEffect();
				this.gaugeDown.GetComponent<EffectAnimatorEventTime>().SetEvent(0, delegate
				{
					float duration = 0.3f;
					this.GaugeDownEffectFadeOut(duration);
				});
				this.PlayRankDownEffect();
				SoundMng.Instance().PlaySE("SEInternal/Farm/se_219", 0f, false, true, null, -1, 1f);
			}
			flag2 = (Math.Abs(num6) > 0);
		}
		this.currentDpLabel.text = this.pvpResultData.score.ToString();
		this.gaugeOrangeSlider.value = (float)num / (float)num2;
		this.gaugeGraySlider.value = 1f - this.gaugeOrangeSlider.value;
		this.UpdateAddDpInfo(string.Empty, 0, 0);
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
