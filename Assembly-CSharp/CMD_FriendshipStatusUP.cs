using Master;
using Monster;
using System;
using UnityEngine;

public class CMD_FriendshipStatusUP : CMD
{
	[SerializeField]
	private UILabel goMN_DescriptionTXT;

	[SerializeField]
	private GameObject goMN_ICON_NOW;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterStatusChangeValueList monsterStatusChangeValueList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	private void ActMIconLong(MonsterData monsterData)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		GUIMain.ShowCommonDialog(delegate(int noop)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
			icon.Lock = monsterData.userMonster.IsLocked;
		}, "CMD_CharacterDetailed");
	}

	public void SetData(MonsterData monsterData)
	{
		this.goMN_DescriptionTXT.text = string.Format(StringMaster.GetString("CharaTapFriendshipUp"), monsterData.monsterMG.monsterName);
		this.monsterBasicInfo.SetMonsterData(monsterData);
		this.monsterStatusList.SetValues(monsterData, false);
		this.monsterMedalList.SetValues(monsterData.userMonster);
		Transform transform = this.goMN_ICON_NOW.transform;
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, transform.localScale, transform.localPosition, transform.parent, true, false);
		guimonsterIcon.Data = monsterData;
		guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
		UIWidget component = this.goMN_ICON_NOW.GetComponent<UIWidget>();
		UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
		if (component != null && component2 != null)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(guimonsterIcon.transform, add);
		}
		this.goMN_ICON_NOW.SetActive(false);
	}

	public void SetChangeData(string monsterId, string friendship)
	{
		StatusValue values = this.monsterStatusList.GetValues();
		int num = values.friendship / ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
		int num2 = friendship.ToInt32();
		int num3 = num2 / ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
		int bonusStep = num - num3;
		MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
		StatusValue friendshipBonusValue = MonsterFriendshipData.GetFriendshipBonusValue(monsterMasterByMonsterId.Simple, bonusStep);
		StatusValue beforeStatus = new StatusValue
		{
			hp = values.hp - friendshipBonusValue.hp,
			attack = values.attack - friendshipBonusValue.attack,
			defense = values.defense - friendshipBonusValue.defense,
			magicAttack = values.magicAttack - friendshipBonusValue.magicAttack,
			magicDefense = values.magicDefense - friendshipBonusValue.magicDefense,
			speed = values.speed - friendshipBonusValue.speed,
			luck = values.luck,
			friendship = num2
		};
		this.monsterStatusChangeValueList.SetValues(beforeStatus, values);
	}
}
