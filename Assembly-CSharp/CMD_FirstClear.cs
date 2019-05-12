using Master;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_FirstClear : CMD
{
	[SerializeField]
	private UILabel firstClearMessage;

	[SerializeField]
	private GameObject rollCircle;

	[SerializeField]
	private GameObject rollInnerCircle;

	[SerializeField]
	private List<RewardIconRoot> rewardIconRootList = new List<RewardIconRoot>();

	[SerializeField]
	private ParticleSystem particle;

	public static bool isRewardBattlePoint;

	private bool isParticleStopped;

	protected override void Awake()
	{
		base.Awake();
		this.firstClearMessage.text = string.Format(StringMaster.GetString("FirstClearBonus"), ConstValue.FIRST_CLEAR_NUM);
		this.moveBehavior = CommonDialog.MOVE_BEHAVIOUR.XY_SCALE_CLOSE;
		GUICollider.DisableAllCollider("CMD_FirstClear");
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		GUICollider.EnableAllCollider("CMD_FirstClear");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.Initialize();
		base.Show(f, sizeX, sizeY, aT);
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_305", 0f, false, true, null, -1);
	}

	protected override void Update()
	{
		base.Update();
		this.rollCircle.transform.Rotate(new Vector3(0f, 0f, 1f));
		this.rollInnerCircle.transform.Rotate(new Vector3(0f, 0f, -1f));
		this.CheckExistFrontDialog();
	}

	private void Initialize()
	{
		this.SetRewardList();
	}

	private void SetRewardList()
	{
		List<GameWebAPI.RespDataWD_DungeonResult.DungeonReward> list = new List<GameWebAPI.RespDataWD_DungeonResult.DungeonReward>();
		if (ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult != null && ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult.dungeonReward != null)
		{
			foreach (GameWebAPI.RespDataWD_DungeonResult.DungeonReward dungeonReward2 in ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult.dungeonReward)
			{
				if (dungeonReward2.everyTimeFlg == 0)
				{
					list.Add(dungeonReward2);
				}
			}
		}
		else if (ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic != null)
		{
			if (ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic.dungeonReward == null || ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic.dungeonReward.clearReward == null)
			{
				global::Debug.LogError("表示する報酬が見つかりませんでした(マルチ)");
				return;
			}
			foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.ClearReward clearReward2 in ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic.dungeonReward.clearReward)
			{
				if (clearReward2.everyTimeFlg == 0)
				{
					list.Add(new GameWebAPI.RespDataWD_DungeonResult.DungeonReward
					{
						assetCategoryId = clearReward2.assetCategoryId.ToString(),
						assetNum = clearReward2.assetNum,
						assetValue = clearReward2.assetValue
					});
				}
			}
		}
		else if (CMD_FirstClear.isRewardBattlePoint)
		{
			CMD_FirstClear.isRewardBattlePoint = false;
			if (ClassSingleton<MultiBattleData>.Instance.BattleEndResponse == null || ClassSingleton<MultiBattleData>.Instance.BattleEndResponse.reward == null || ClassSingleton<MultiBattleData>.Instance.BattleEndResponse.reward.Length == 0)
			{
				global::Debug.LogError("表示する報酬が見つかりませんでした");
				return;
			}
			MultiBattleData.BattleEndResponseData.Reward[] reward = ClassSingleton<MultiBattleData>.Instance.BattleEndResponse.reward;
			foreach (MultiBattleData.BattleEndResponseData.Reward reward2 in reward)
			{
				if (reward2.assetCategoryId == 6 && reward2.assetValue == 2)
				{
					list.Add(new GameWebAPI.RespDataWD_DungeonResult.DungeonReward
					{
						assetCategoryId = reward2.assetCategoryId.ToString(),
						assetNum = reward2.assetNum,
						assetValue = reward2.assetValue
					});
				}
			}
			if (list.Count == 0)
			{
				global::Debug.LogError("表示する報酬が見つかりませんでした");
				return;
			}
			global::Debug.Log("BattlePoint");
		}
		else
		{
			if (ClassSingleton<MultiBattleData>.Instance.BattleEndResponse == null || ClassSingleton<MultiBattleData>.Instance.BattleEndResponse.firstRankUpReward == null || ClassSingleton<MultiBattleData>.Instance.BattleEndResponse.firstRankUpReward.Length == 0)
			{
				global::Debug.LogError("表示する報酬が見つかりませんでした");
				return;
			}
			MultiBattleData.BattleEndResponseData.Reward[] firstRankUpReward = ClassSingleton<MultiBattleData>.Instance.BattleEndResponse.firstRankUpReward;
			foreach (MultiBattleData.BattleEndResponseData.Reward reward3 in firstRankUpReward)
			{
				list.Add(new GameWebAPI.RespDataWD_DungeonResult.DungeonReward
				{
					assetCategoryId = reward3.assetCategoryId.ToString(),
					assetNum = reward3.assetNum,
					assetValue = reward3.assetValue
				});
			}
		}
		this.rewardIconRootList[list.Count - 1].SetRewardList(list);
	}

	private void CheckExistFrontDialog()
	{
		if (GUIManager.CheckOpenDialog("CMD_Alert") || GUIManager.CheckOpenDialog("CMD_AlertDBG"))
		{
			if (this.particle != null && !this.particle.isStopped)
			{
				this.particle.Stop();
				this.isParticleStopped = true;
			}
		}
		else if (this.isParticleStopped && this.particle != null && this.particle.isStopped)
		{
			this.particle.Play();
			this.isParticleStopped = false;
		}
	}

	private void OnTouch()
	{
		bool flag = false;
		GameWebAPI.RespDataWD_DungeonResult respDataWD_DungeonResult = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult;
		if (respDataWD_DungeonResult != null && "3" == respDataWD_DungeonResult.worldDungeonId)
		{
			flag = true;
		}
		else
		{
			GameWebAPI.RespData_WorldMultiResultInfoLogic respData_WorldMultiResultInfoLogic = ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic;
			if (respData_WorldMultiResultInfoLogic != null && "3" == respData_WorldMultiResultInfoLogic.worldDungeonId)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (this.particle != null && !this.particle.isStopped)
			{
				this.particle.Stop();
			}
			Action finishedAction = delegate()
			{
				this.ClosePanel(true);
			};
			LeadReview.ShowReviewConfirm(LeadReview.MessageType.FIRST_CLEAR_AREA1_STAGE3, finishedAction, false);
		}
		else
		{
			this.ClosePanel(true);
		}
	}
}
