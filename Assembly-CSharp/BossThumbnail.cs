using Monster;
using System;

public sealed class BossThumbnail : GUICollider
{
	private MonsterThumbnail monsterIcon;

	private GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy bossInfo;

	public void Initialize()
	{
		this.monsterIcon = base.GetComponent<MonsterThumbnail>();
		this.monsterIcon.Initialize();
	}

	public void SetBossInfo(GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy enemy)
	{
		this.bossInfo = enemy;
		string text = enemy.monsterId.ToString();
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(text).Simple;
		if (simple != null)
		{
			text = simple.monsterGroupId;
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(text).Group;
			if (group != null)
			{
				this.monsterIcon.gameObject.SetActive(true);
				this.monsterIcon.SetImage(simple.iconId, group.growStep);
				UIWidget component = this.monsterIcon.GetComponent<UIWidget>();
				if (null != component)
				{
					this.monsterIcon.SetSize(component.width, component.height);
				}
			}
		}
	}

	private void OnPushed()
	{
		CMD_QuestMonsterPOP cmd_QuestMonsterPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestMonsterPOP", null) as CMD_QuestMonsterPOP;
		cmd_QuestMonsterPOP.SetBossDetails(this.bossInfo.monsterId, this.bossInfo.resistanceId);
	}
}
