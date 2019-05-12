using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_BattleResult : CMD
{
	private CMD_BattleResult.State state;

	private Action<CMD_BattleResult> actionEffectFinished;

	[Header("ドロップアイテム結果")]
	[SerializeField]
	private DropItemResult dropItemResult;

	[SerializeField]
	[Header("経験値結果")]
	private ExperienceResult experienceResult;

	private Dictionary<CMD_BattleResult.State, ResultBase> results = new Dictionary<CMD_BattleResult.State, ResultBase>();

	public string clearDungeonID { get; private set; }

	protected override void Update()
	{
		base.Update();
		if (this.results.ContainsKey(this.state))
		{
			this.results[this.state].UpdateAndroidBackKey();
			if (this.results[this.state].isEnd)
			{
				CMD_BattleResult.State state = this.state;
				if (state != CMD_BattleResult.State.DropItem)
				{
					if (state == CMD_BattleResult.State.Experience)
					{
						BoxCollider component = base.GetComponent<BoxCollider>();
						component.enabled = false;
						if (this.actionEffectFinished != null)
						{
							this.actionEffectFinished(this);
							this.actionEffectFinished = null;
						}
						else
						{
							SoundMng.Instance().PlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1, 1f);
							this.ClosePanel(true);
						}
						this.ChangeState(CMD_BattleResult.State.None);
					}
				}
				else
				{
					this.ChangeState(CMD_BattleResult.State.Experience);
				}
			}
		}
	}

	private void ChangeState(CMD_BattleResult.State nextState)
	{
		if (this.results.ContainsKey(this.state))
		{
			this.results[this.state].gameObject.SetActive(false);
		}
		this.state = nextState;
		if (this.results.ContainsKey(this.state))
		{
			this.results[this.state].gameObject.SetActive(true);
			this.results[this.state].Show();
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		SoundMng.Instance().PlayGameBGM("bgm_303");
		if (GooglePlayGamesTool.Instance != null)
		{
			GooglePlayGamesTool.Instance.ClearQuest();
		}
		GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
		if (respDataWD_DungeonStart != null)
		{
			this.clearDungeonID = respDataWD_DungeonStart.worldDungeonId;
		}
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		if (respData_WorldMultiStartInfo != null)
		{
			this.clearDungeonID = respData_WorldMultiStartInfo.worldDungeonId;
		}
		this.results.Add(CMD_BattleResult.State.DropItem, this.dropItemResult);
		this.results.Add(CMD_BattleResult.State.Experience, this.experienceResult);
		foreach (ResultBase resultBase in this.results.Values)
		{
			resultBase.gameObject.SetActive(true);
			resultBase.Init();
			resultBase.gameObject.SetActive(false);
		}
		this.ChangeState(CMD_BattleResult.State.DropItem);
		base.Show(f, sizeX, sizeY, aT);
	}

	private void OnTapped()
	{
		if (this.results.ContainsKey(this.state))
		{
			this.results[this.state].OnTapped();
		}
	}

	public void SetActionEffectFinished(Action<CMD_BattleResult> completed)
	{
		this.actionEffectFinished = completed;
	}

	private enum State
	{
		None,
		DropItem,
		Experience
	}
}
