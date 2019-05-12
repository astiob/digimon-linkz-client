using Master;
using Monster;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExperienceResult : ResultBase
{
	private const int UP_EXP_VALUE = 1;

	private const int UP_EXP_VALUE_NEXT = 6;

	private readonly Vector3 BIG_LABEL_SIZE = new Vector3(1.35f, 1.35f, 1f);

	[SerializeField]
	[Header("カウントアップするときのラベルの色")]
	private Color countUpLabelColor = new Color32(byte.MaxValue, 240, 0, byte.MaxValue);

	[Header("経験値などが入ってる")]
	[SerializeField]
	private GameObject acquisitionGO;

	[Header("デジモン部分")]
	[SerializeField]
	private BattleResultDigimonInfo[] digimonInfos;

	[Header("デジモンに吸収されるパーティクルを消す")]
	[SerializeField]
	private GameObject particleRemover;

	[Header("取得経験値のラベル")]
	[SerializeField]
	private UILabel getExp;

	[Header("取得経験値の文言ラベル")]
	[SerializeField]
	private UILabel getExpText;

	[SerializeField]
	[Header("取得クラスタのラベル")]
	private UILabel getCluster;

	[Header("取得クラスタの文言ラベル")]
	[SerializeField]
	private UILabel getClusterText;

	[Header("取得友情度【リーダー】のラベル")]
	[SerializeField]
	private UILabel getFriendPointForLeader;

	[SerializeField]
	[Header("取得友情度【リーダー】の文言ラベル")]
	private UILabel getFriendPointForLeaderText;

	[Header("取得友情度のラベル")]
	[SerializeField]
	private UILabel getFriendPoint;

	[SerializeField]
	[Header("取得友情度の文言ラベル")]
	private UILabel getFriendPointText;

	private ExperienceResult.SkipCount skipCount;

	private bool isMulti;

	private GameWebAPI.RespDataMN_GetDeckList.DeckList deckData;

	private int restRewardExp;

	private int totalExp;

	private int totalCluster;

	private int totalFriendPointLeader;

	private int totalFriendPoint;

	private int[] friendShipAddPoints;

	private Coroutine[] countUpRewards;

	private Coroutine showDigimonInfos;

	private Coroutine addFriendShip;

	private List<int> seHandles = new List<int>();

	private ExperienceResult.OldMonsterInfo[] oldMonsterInfoList = new ExperienceResult.OldMonsterInfo[2];

	private bool isCantGoNext = true;

	private GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList memoryUserMonsterList;

	private List<GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList> memoryMonsterList;

	public override void Init()
	{
		base.Init();
		this.particleRemover.SetActive(false);
		this.getExp.text = "0";
		this.getCluster.text = "0";
		this.getFriendPointForLeader.text = "0";
		this.getFriendPoint.text = "0";
		NGUITools.SetActiveSelf(this.acquisitionGO, false);
		int num = int.Parse(DataMng.Instance().RespDataMN_DeckList.selectDeckNum) - 1;
		this.deckData = DataMng.Instance().RespDataMN_DeckList.deckList[num];
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		this.isMulti = (null != respData_WorldMultiStartInfo);
		if (!this.isMulti)
		{
			GameWebAPI.RespDataWD_DungeonResult respDataWD_DungeonResult = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult;
			this.totalExp = respDataWD_DungeonResult.totalExp;
			this.totalCluster = respDataWD_DungeonResult.totalMoney;
		}
		else
		{
			GameWebAPI.RespData_WorldMultiResultInfoLogic respData_WorldMultiResultInfoLogic = ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic;
			this.totalExp = respData_WorldMultiResultInfoLogic.totalExp;
			this.totalCluster = respData_WorldMultiResultInfoLogic.totalMoney;
			if (respData_WorldMultiResultInfoLogic.dungeonReward != null)
			{
				if (respData_WorldMultiResultInfoLogic.dungeonReward.luckDrop != null)
				{
					foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LuckDrop luckDrop2 in respData_WorldMultiResultInfoLogic.dungeonReward.luckDrop)
					{
						if (int.Parse(luckDrop2.assetCategoryId) == 5)
						{
							this.totalExp += luckDrop2.assetNum;
						}
						if (int.Parse(luckDrop2.assetCategoryId) == 4)
						{
							this.totalCluster += luckDrop2.assetNum;
						}
					}
				}
				if (respData_WorldMultiResultInfoLogic.dungeonReward.multiReward != null)
				{
					foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward dropReward in respData_WorldMultiResultInfoLogic.dungeonReward.multiReward)
					{
						if (int.Parse(dropReward.assetCategoryId) == 5)
						{
							this.totalExp += dropReward.assetNum;
						}
						if (int.Parse(dropReward.assetCategoryId) == 4)
						{
							this.totalCluster += dropReward.assetNum;
						}
					}
				}
				if (respData_WorldMultiResultInfoLogic.dungeonReward.ownerDropReward != null)
				{
					foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward dropReward2 in respData_WorldMultiResultInfoLogic.dungeonReward.ownerDropReward)
					{
						if (int.Parse(dropReward2.assetCategoryId) == 5)
						{
							this.totalExp += dropReward2.assetNum;
						}
						if (int.Parse(dropReward2.assetCategoryId) == 4)
						{
							this.totalCluster += dropReward2.assetNum;
						}
					}
				}
			}
		}
		this.getExpText.text = StringMaster.GetString("BattleUI-03");
		this.getClusterText.text = StringMaster.GetString("BattleUI-40");
		this.getFriendPointForLeaderText.text = StringMaster.GetString("BattleUI-48");
		this.getFriendPointText.text = StringMaster.GetString("BattleUI-49");
		this.CreateDigimonThumbnail();
	}

	private void CreateDigimonThumbnail()
	{
		if (!this.isMulti)
		{
			int[] aliveInfo = DataMng.Instance().WD_ReqDngResult.aliveInfo;
			for (int i = 0; i < this.deckData.monsterList.Length; i++)
			{
				this.digimonInfos[i].DigimonNo = i;
				GameWebAPI.RespDataMN_GetDeckList.MonsterList monsterList = this.deckData.monsterList[i];
				MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(monsterList.userMonsterId);
				MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(userMonster.GetMonster().monsterId);
				Transform iconLocator = this.digimonInfos[i].GetIconLocator();
				GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, iconLocator.localScale, iconLocator.localPosition, iconLocator.parent, true, false);
				guimonsterIcon.name = "DigimonIcon" + i;
				guimonsterIcon.activeCollider = false;
				int depth = guimonsterIcon.GetComponent<UIWidget>().depth;
				this.digimonInfos[i].SetDepth(depth);
				if (aliveInfo.Length > i && aliveInfo[i] == 0)
				{
					guimonsterIcon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
				}
			}
		}
		else
		{
			int[] aliveInfo2 = DataMng.Instance().WD_ReqDngResult.aliveInfo;
			GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
			for (int j = 0; j < this.digimonInfos.Length; j++)
			{
				this.digimonInfos[j].DigimonNo = j;
				int partyIndex = DataMng.Instance().GetPartyIndex(j);
				int monsterIndex = DataMng.Instance().GetMonsterIndex(j);
				MonsterData monsterData2 = MonsterDataMng.Instance().CreateMonsterDataByMID(respData_WorldMultiStartInfo.party[partyIndex].userMonsters[monsterIndex].monsterId);
				Transform iconLocator2 = this.digimonInfos[j].GetIconLocator();
				GUIMonsterIcon guimonsterIcon2 = GUIMonsterIcon.MakePrefabByMonsterData(monsterData2, iconLocator2.localScale, iconLocator2.localPosition, iconLocator2.parent, true, false);
				guimonsterIcon2.name = "DigimonIcon" + j;
				guimonsterIcon2.activeCollider = false;
				int depth2 = guimonsterIcon2.GetComponent<UIWidget>().depth;
				this.digimonInfos[j].SetDepth(depth2);
				guimonsterIcon2.SetPlayerIcon(partyIndex + 1);
				if (aliveInfo2.Length > j && aliveInfo2[j] == 0)
				{
					guimonsterIcon2.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
				}
			}
		}
		foreach (BattleResultDigimonInfo battleResultDigimonInfo in this.digimonInfos)
		{
			NGUITools.SetActiveSelf(battleResultDigimonInfo.gameObject, false);
		}
	}

	private void ResetLabelSizeAndColor(UILabel label)
	{
		label.transform.localScale = Vector3.one;
		label.color = Color.white;
	}

	private void PlayCountUpSound()
	{
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_102", 0f, true, true, null, -1);
	}

	private void StopCountUpSound()
	{
		if (SoundMng.Instance().IsPlayingSE("SEInternal/Common/se_102"))
		{
			SoundMng.Instance().TryStopSE("SEInternal/Common/se_102", 0.2f, null);
		}
	}

	public override void Show()
	{
		AppCoroutine.Start(this.GetUserMonsterData(new Action(this.SetResultInfo)), false);
	}

	private IEnumerator GetUserMonsterData(Action callback = null)
	{
		this.memoryMonsterList = new List<GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList>();
		if (!this.isMulti)
		{
			foreach (GameWebAPI.RespDataMN_GetDeckList.MonsterList monsterList2 in this.deckData.monsterList)
			{
				this.memoryMonsterList.Add(MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(monsterList2.userMonsterId, false).userMonster);
			}
		}
		else
		{
			GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
			for (int j = 0; j < respData_WorldMultiStartInfo.party.Length; j++)
			{
				if (respData_WorldMultiStartInfo.party[j] != null && respData_WorldMultiStartInfo.party[j].userId.ToInt32() == DataMng.Instance().RespDataCM_Login.playerInfo.UserId)
				{
					for (int k = 0; k < respData_WorldMultiStartInfo.party[j].userMonsters.Length; k++)
					{
						if (respData_WorldMultiStartInfo.party[j].userMonsters[k] != null)
						{
							this.oldMonsterInfoList[k] = new ExperienceResult.OldMonsterInfo();
							string userMonsterId = respData_WorldMultiStartInfo.party[j].userMonsters[k].userMonsterId;
							this.oldMonsterInfoList[k].userMonsterId = userMonsterId;
							MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
							if (monsterDataByUserMonsterID != null)
							{
								this.oldMonsterInfoList[k].friendship = monsterDataByUserMonsterID.userMonster.friendship;
								this.memoryMonsterList.Add(monsterDataByUserMonsterID.userMonster);
							}
						}
					}
				}
			}
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		APIRequestTask task = this.RequestUserMonsterData();
		return task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.friendShipAddPoints = this.GetFiriendShipAddPoints();
			if (callback != null)
			{
				callback();
			}
		}, null, null);
	}

	private void SetResultInfo()
	{
		this.skipCount = ExperienceResult.SkipCount.ResultInfo;
		NGUITools.SetActiveSelf(this.acquisitionGO, true);
		for (int i = 0; i < this.friendShipAddPoints.Length; i++)
		{
			int num = this.friendShipAddPoints[i];
			if (num > 0)
			{
				if (i == 0)
				{
					this.totalFriendPointLeader = num;
				}
				else
				{
					this.totalFriendPoint = num;
				}
			}
		}
		UILabel[] labels = new UILabel[]
		{
			this.getExp,
			this.getCluster,
			this.getFriendPointForLeader,
			this.getFriendPoint
		};
		int[] array = new int[]
		{
			this.totalExp,
			this.totalCluster,
			this.totalFriendPointLeader,
			this.totalFriendPoint
		};
		this.countUpRewards = new Coroutine[labels.Length];
		int endCount = 0;
		this.PlayCountUpSound();
		for (int j = 0; j < labels.Length; j++)
		{
			IEnumerator routine = this.CountUpReward(labels[j], array[j], delegate
			{
				endCount++;
				if (endCount == labels.Length)
				{
					this.StopCountUpSound();
					this.ShowDigimonInfos();
				}
			}, 0);
			this.countUpRewards[j] = AppCoroutine.Start(routine, false);
		}
		this.restRewardExp = this.totalExp;
	}

	private IEnumerator CountUpReward(UILabel label, int addNum, Action callback, int firstNum = 0)
	{
		int i = firstNum;
		int sum = firstNum + addNum;
		Transform labelTrans = label.transform;
		labelTrans.localScale = this.BIG_LABEL_SIZE;
		label.color = this.countUpLabelColor;
		int restExp = sum;
		while (i != sum)
		{
			int addPoint = 1;
			if (i > 100 + firstNum)
			{
				addPoint = Mathf.Min(6, restExp);
			}
			else if (i > 20 + firstNum)
			{
				addPoint = Mathf.Min(1, restExp);
			}
			i = Mathf.Min(sum, i + addPoint);
			restExp -= addPoint;
			label.text = i.ToString();
			yield return null;
		}
		labelTrans.localScale = Vector3.one;
		label.color = Color.white;
		callback();
		yield break;
	}

	private void ShowDigimonInfos()
	{
		this.skipCount = ExperienceResult.SkipCount.DigimonInfos;
		this.showDigimonInfos = AppCoroutine.Start(this.DigimonInfos(), false);
	}

	private IEnumerator DigimonInfos()
	{
		yield return new WaitForSeconds(0.6f);
		this.particleRemover.SetActive(true);
		if (this.digimonInfos != null)
		{
			for (int i = 0; i < this.digimonInfos.Length; i++)
			{
				NGUITools.SetActiveSelf(this.digimonInfos[i].gameObject, true);
				TweenAlpha tw = this.digimonInfos[i].gameObject.AddComponent<TweenAlpha>();
				tw.from = 0f;
				tw.to = 1f;
				if (!this.isMulti)
				{
					string tgtId = this.deckData.monsterList[i].userMonsterId;
					GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList memoryUserMonster = this.memoryMonsterList.Single((GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList x) => x.userMonsterId == tgtId);
					MonsterData digimonData = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(tgtId, false);
					this.digimonInfos[i].CreateDetails(memoryUserMonster.ex.ToInt32(), memoryUserMonster.level.ToInt32(), digimonData.monsterM.maxLevel.ToInt32());
				}
				else
				{
					GameWebAPI.RespData_WorldMultiStartInfo startInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
					int partyIndex = DataMng.Instance().GetPartyIndex(i);
					int monsterIndex = DataMng.Instance().GetMonsterIndex(i);
					GameWebAPI.RespData_WorldMultiStartInfo.Party party = startInfo.party[partyIndex];
					GameWebAPI.Common_MonsterData userMonster = party.userMonsters[monsterIndex];
					MonsterData userMonsterData = userMonster.ToMonsterData();
					string tgtId2 = userMonster.userMonsterId;
					GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList memoryUserMonster2 = this.memoryMonsterList.SingleOrDefault((GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList x) => x.userMonsterId == tgtId2);
					if (memoryUserMonster2 != null)
					{
						this.digimonInfos[i].CreateDetails(memoryUserMonster2.ex.ToInt32(), memoryUserMonster2.level.ToInt32(), userMonsterData.monsterM.maxLevel.ToInt32());
					}
					else
					{
						this.digimonInfos[i].CreateDetails(userMonsterData);
					}
				}
			}
			BattleResultDigimonInfo[] growDigimonInfos = this.GetGrowDigimonInfos(this.digimonInfos);
			IEnumerator countUpExp = this.CountUpExp(growDigimonInfos);
			while (countUpExp.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	private BattleResultDigimonInfo[] GetGrowDigimonInfos(BattleResultDigimonInfo[] digimonInfos)
	{
		List<BattleResultDigimonInfo> list = new List<BattleResultDigimonInfo>();
		if (digimonInfos != null)
		{
			int[] aliveInfo = DataMng.Instance().WD_ReqDngResult.aliveInfo;
			GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
			int i = 0;
			while (i < aliveInfo.Length)
			{
				if (respData_WorldMultiStartInfo == null)
				{
					goto IL_72;
				}
				int partyIndex = DataMng.Instance().GetPartyIndex(i);
				if (int.Parse(respData_WorldMultiStartInfo.party[partyIndex].userId) == DataMng.Instance().RespDataCM_Login.playerInfo.UserId)
				{
					goto IL_72;
				}
				IL_9A:
				i++;
				continue;
				IL_72:
				if (aliveInfo[i] == 1 && digimonInfos.Length > i && !digimonInfos[i].IsFinishExpCountUp())
				{
					list.Add(digimonInfos[i]);
					goto IL_9A;
				}
				goto IL_9A;
			}
		}
		return list.ToArray();
	}

	private IEnumerator CountUpExp(BattleResultDigimonInfo[] growDigimonInfos)
	{
		if (growDigimonInfos != null && 0 < growDigimonInfos.Length)
		{
			this.getExp.transform.localScale = this.BIG_LABEL_SIZE;
			this.getExp.color = this.countUpLabelColor;
			bool soundPlayed = false;
			int countGauge = 0;
			bool finishCountUp = false;
			while (!finishCountUp)
			{
				countGauge++;
				if (countGauge > 20)
				{
					if (!soundPlayed)
					{
						soundPlayed = true;
						this.PlayCountUpSound();
					}
					if (countGauge > 50 || countGauge % 4 == 0)
					{
						finishCountUp = this.UpdateDigimonExp(growDigimonInfos, false);
					}
				}
				yield return null;
			}
			this.ResetLabelSizeAndColor(this.getExp);
			this.StopCountUpSound();
		}
		bool isFinishExpCountUp = false;
		while (!isFinishExpCountUp)
		{
			isFinishExpCountUp = true;
			foreach (BattleResultDigimonInfo digimonInfo in this.digimonInfos)
			{
				if (!digimonInfo.IsFinishExpCountUp())
				{
					isFinishExpCountUp = false;
					break;
				}
			}
			yield return null;
		}
		this.ShowTapNext();
		yield break;
	}

	private bool UpdateDigimonExp(BattleResultDigimonInfo[] growDigimonInfos, bool isSkip = false)
	{
		bool flag = true;
		for (int i = 0; i < growDigimonInfos.Length; i++)
		{
			if (!growDigimonInfos[i].IsFinishExpCountUp())
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			return true;
		}
		int a = (!isSkip) ? 1 : 10;
		int num = Mathf.Min(a, this.restRewardExp);
		bool levelUp = false;
		int num2 = 0;
		for (int j = 0; j < growDigimonInfos.Length; j++)
		{
			growDigimonInfos[j].AddExp(num, delegate
			{
				levelUp = true;
			});
			num2 += ((!growDigimonInfos[j].IsFinishExpCountUp()) ? 0 : 1);
		}
		if (levelUp && !isSkip)
		{
			this.PlayLevelUpSE(growDigimonInfos);
		}
		this.restRewardExp -= num;
		this.getExp.text = this.restRewardExp.ToString();
		return growDigimonInfos.Length <= num2 || this.restRewardExp <= 0;
	}

	private void PlayLevelUpSE(BattleResultDigimonInfo[] growDigimonInfos)
	{
		if (growDigimonInfos.Length <= this.seHandles.Count)
		{
			int num = this.seHandles.First<int>();
			SoundMng.Instance().StopSE_Ex(num);
			this.seHandles.Remove(num);
		}
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
	}

	private void ShowTapNext()
	{
		this.skipCount = ExperienceResult.SkipCount.AddFriendShip;
		this.addFriendShip = AppCoroutine.Start(this.AddFriendShip(), false);
		base.ShowNextTap();
	}

	private void ShowFriendUpEff()
	{
		this.skipCount = ExperienceResult.SkipCount.Finish;
		AppCoroutine.Start(this.FriendshipUpEndPop(), false);
	}

	private int[] GetFiriendShipAddPoints()
	{
		int[] array = new int[this.deckData.monsterList.Length];
		int i = 0;
		while (i < this.deckData.monsterList.Length)
		{
			string tgtId = string.Empty;
			int num = -1;
			if (!this.isMulti)
			{
				tgtId = this.deckData.monsterList[i].userMonsterId;
				GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList = this.memoryMonsterList.Single((GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList x) => x.userMonsterId == tgtId);
				num = userMonsterList.friendship.ToInt32();
				goto IL_163;
			}
			GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
			int partyIndex = DataMng.Instance().GetPartyIndex(i);
			int monsterIndex = DataMng.Instance().GetMonsterIndex(i);
			GameWebAPI.Common_MonsterData common_MonsterData = respData_WorldMultiStartInfo.party[partyIndex].userMonsters[monsterIndex];
			if (int.Parse(common_MonsterData.userId) == DataMng.Instance().RespDataCM_Login.playerInfo.UserId)
			{
				tgtId = respData_WorldMultiStartInfo.party[partyIndex].userMonsters[monsterIndex].userMonsterId;
				for (int j = 0; j < this.oldMonsterInfoList.Length; j++)
				{
					if (this.oldMonsterInfoList != null && tgtId == this.oldMonsterInfoList[j].userMonsterId)
					{
						num = this.oldMonsterInfoList[j].friendship.ToInt32();
						break;
					}
				}
				goto IL_163;
			}
			array[i] = 0;
			IL_1C6:
			i++;
			continue;
			IL_163:
			MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(tgtId, false);
			if (string.IsNullOrEmpty(monsterDataByUserMonsterID.userMonster.friendship))
			{
				monsterDataByUserMonsterID.userMonster.friendship = "0";
			}
			int num2 = int.Parse(monsterDataByUserMonsterID.userMonster.friendship);
			if (num == -1)
			{
				num = num2;
			}
			int num3 = num2 - num;
			array[i] = num3;
			goto IL_1C6;
		}
		return array;
	}

	private IEnumerator AddFriendShip()
	{
		yield return new WaitForSeconds(0.6f);
		for (int i = 0; i < this.digimonInfos.Length; i++)
		{
			int upVal = this.friendShipAddPoints[this.digimonInfos[i].DigimonNo];
			if (upVal > 0)
			{
				this.digimonInfos[i].AddFriend(upVal);
			}
		}
		bool isFinishFriendShipUp = false;
		while (!isFinishFriendShipUp)
		{
			isFinishFriendShipUp = true;
			foreach (BattleResultDigimonInfo digimonInfo in this.digimonInfos)
			{
				if (!digimonInfo.IsFinishFriendShipUp())
				{
					isFinishFriendShipUp = false;
					break;
				}
			}
			yield return null;
		}
		this.ShowFriendUpEff();
		yield break;
	}

	private IEnumerator FriendshipUpEndPop()
	{
		for (int i = 0; i < this.deckData.monsterList.Length; i++)
		{
			string tgtId = string.Empty;
			if (!this.isMulti)
			{
				tgtId = this.deckData.monsterList[i].userMonsterId;
			}
			else
			{
				if (this.oldMonsterInfoList.Length <= i || this.oldMonsterInfoList[i] == null)
				{
					break;
				}
				tgtId = this.oldMonsterInfoList[i].userMonsterId;
			}
			this.memoryUserMonsterList = this.memoryMonsterList.SingleOrDefault((GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList x) => x.userMonsterId == tgtId);
			if (this.memoryUserMonsterList == null)
			{
				break;
			}
			MonsterData afterMonsterData = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(tgtId, false);
			if (string.IsNullOrEmpty(afterMonsterData.userMonster.friendship))
			{
				afterMonsterData.userMonster.friendship = "0";
			}
			int fShipBefore = int.Parse(this.memoryUserMonsterList.friendship);
			int fShipAfter = int.Parse(afterMonsterData.userMonster.friendship);
			int upVal = fShipAfter - fShipBefore;
			if (0 < upVal)
			{
				int surplus = fShipAfter % ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
				if (surplus < upVal)
				{
					IEnumerator popFriendshipUpStatus = this.PopFriendshipUpStatus(afterMonsterData);
					while (popFriendshipUpStatus.MoveNext())
					{
						yield return null;
					}
				}
			}
		}
		this.isCantGoNext = false;
		yield break;
	}

	private IEnumerator PopFriendshipUpStatus(MonsterData afterMonsterData)
	{
		yield return new WaitForSeconds(1f);
		CMD_FriendshipStatusUP cd = GUIMain.ShowCommonDialog(null, "CMD_FriendshipStatusUP", null) as CMD_FriendshipStatusUP;
		cd.SetData(afterMonsterData);
		cd.SetChangeData(this.memoryUserMonsterList.monsterId, this.memoryUserMonsterList.friendship);
		while (cd != null)
		{
			yield return null;
		}
		yield break;
	}

	private APIRequestTask RequestUserMonsterData()
	{
		int num = int.Parse(DataMng.Instance().RespDataMN_DeckList.selectDeckNum) - 1;
		GameWebAPI.RespDataMN_GetDeckList.DeckList deckList = DataMng.Instance().RespDataMN_DeckList.deckList[num];
		int[] aliveInfo = DataMng.Instance().WD_ReqDngResult.aliveInfo;
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		List<int> aliveUserMonsterIds = new List<int>();
		List<MonsterData> list = new List<MonsterData>();
		int i = 0;
		while (i < aliveInfo.Length)
		{
			string text = string.Empty;
			if (respData_WorldMultiStartInfo == null)
			{
				goto IL_D5;
			}
			int partyIndex = DataMng.Instance().GetPartyIndex(i);
			int monsterIndex = DataMng.Instance().GetMonsterIndex(i);
			GameWebAPI.Common_MonsterData common_MonsterData = respData_WorldMultiStartInfo.party[partyIndex].userMonsters[monsterIndex];
			if (int.Parse(common_MonsterData.userId) == DataMng.Instance().RespDataCM_Login.playerInfo.UserId)
			{
				text = common_MonsterData.userMonsterId;
				goto IL_D5;
			}
			IL_136:
			i++;
			continue;
			IL_D5:
			if (aliveInfo[i] == 1 && deckList.monsterList.Length > i)
			{
				string text2 = (respData_WorldMultiStartInfo != null) ? text : deckList.monsterList[i].userMonsterId;
				MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(text2, false);
				list.Add(monsterDataByUserMonsterID);
				aliveUserMonsterIds.Add(int.Parse(text2));
				goto IL_136;
			}
			goto IL_136;
		}
		GameWebAPI.RequestMonsterList requestMonsterList = new GameWebAPI.RequestMonsterList();
		requestMonsterList.SetSendData = delegate(GameWebAPI.ReqDataUS_GetMonsterList param)
		{
			param.userMonsterIds = aliveUserMonsterIds.ToArray();
		};
		requestMonsterList.OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonsterList);
		};
		GameWebAPI.RequestMonsterList request = requestMonsterList;
		return new APIRequestTask(request, true);
	}

	public override void OnTapped()
	{
		switch (this.skipCount)
		{
		case ExperienceResult.SkipCount.ResultInfo:
			this.SkipResultInfo();
			break;
		case ExperienceResult.SkipCount.DigimonInfos:
			this.SkipDigimonInfos();
			break;
		case ExperienceResult.SkipCount.AddFriendShip:
			this.SkipAddFriendShip();
			break;
		case ExperienceResult.SkipCount.Finish:
			if (!this.isCantGoNext)
			{
				base.isEnd = true;
			}
			break;
		}
	}

	private void SkipResultInfo()
	{
		foreach (Coroutine routine in this.countUpRewards)
		{
			AppCoroutine.Stop(routine, false);
		}
		this.ResetLabelSizeAndColor(this.getExp);
		this.ResetLabelSizeAndColor(this.getCluster);
		this.ResetLabelSizeAndColor(this.getFriendPointForLeader);
		this.ResetLabelSizeAndColor(this.getFriendPoint);
		this.getExp.text = this.totalExp.ToString();
		this.getCluster.text = StringFormat.Cluster(this.totalCluster);
		this.getFriendPointForLeader.text = this.totalFriendPointLeader.ToString();
		this.getFriendPoint.text = this.totalFriendPoint.ToString();
		this.StopCountUpSound();
		this.ShowDigimonInfos();
	}

	private void SkipDigimonInfos()
	{
		this.particleRemover.SetActive(true);
		if (this.showDigimonInfos != null)
		{
			AppCoroutine.Stop(this.showDigimonInfos, false);
			this.showDigimonInfos = null;
		}
		this.restRewardExp = this.totalExp;
		this.ResetLabelSizeAndColor(this.getExp);
		this.getCluster.text = StringFormat.Cluster(this.totalCluster);
		if (this.digimonInfos != null)
		{
			for (int i = 0; i < this.digimonInfos.Length; i++)
			{
				NGUITools.SetActiveSelf(this.digimonInfos[i].gameObject, true);
				if (!this.isMulti)
				{
					string tgtId = this.deckData.monsterList[i].userMonsterId;
					GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList = this.memoryMonsterList.Single((GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList x) => x.userMonsterId == tgtId);
					MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(tgtId, false);
					this.digimonInfos[i].CreateDetails(userMonsterList.ex.ToInt32(), userMonsterList.level.ToInt32(), monsterDataByUserMonsterID.monsterM.maxLevel.ToInt32());
				}
				else
				{
					GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
					int partyIndex = DataMng.Instance().GetPartyIndex(i);
					int monsterIndex = DataMng.Instance().GetMonsterIndex(i);
					GameWebAPI.RespData_WorldMultiStartInfo.Party party = respData_WorldMultiStartInfo.party[partyIndex];
					GameWebAPI.Common_MonsterData common_MonsterData = party.userMonsters[monsterIndex];
					MonsterData monsterData = common_MonsterData.ToMonsterData();
					string tgtId = common_MonsterData.userMonsterId;
					GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList2 = this.memoryMonsterList.SingleOrDefault((GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList x) => x.userMonsterId == tgtId);
					if (userMonsterList2 != null)
					{
						this.digimonInfos[i].CreateDetails(userMonsterList2.ex.ToInt32(), userMonsterList2.level.ToInt32(), monsterData.monsterM.maxLevel.ToInt32());
					}
					else
					{
						this.digimonInfos[i].CreateDetails(monsterData);
					}
				}
				GameObject gameObject = this.digimonInfos[i].gameObject;
				base.ResetTweenAlpha(gameObject);
			}
			BattleResultDigimonInfo[] growDigimonInfos = this.GetGrowDigimonInfos(this.digimonInfos);
			if (growDigimonInfos != null)
			{
				while (!this.UpdateDigimonExp(growDigimonInfos, true))
				{
				}
				for (int j = 0; j < growDigimonInfos.Length; j++)
				{
					growDigimonInfos[j].FixExp(this.restRewardExp);
				}
			}
		}
		this.getExp.text = this.restRewardExp.ToString();
		this.StopCountUpSound();
		this.ShowTapNext();
	}

	private void SkipAddFriendShip()
	{
		if (this.addFriendShip != null)
		{
			AppCoroutine.Stop(this.addFriendShip, false);
			this.addFriendShip = null;
		}
		foreach (BattleResultDigimonInfo battleResultDigimonInfo in this.digimonInfos)
		{
			battleResultDigimonInfo.FixFriend();
		}
		this.ShowFriendUpEff();
	}

	private enum SkipCount
	{
		None,
		ResultInfo,
		DigimonInfos,
		AddFriendShip,
		Finish
	}

	private class OldMonsterInfo
	{
		public string userMonsterId;

		public string friendship;
	}
}
