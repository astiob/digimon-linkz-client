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
			PartyUtil.SetLock(monsterData, false);
		}, "CMD_CharacterDetailed");
	}

	public void SetData(MonsterData monsterData)
	{
		this.goMN_DescriptionTXT.text = string.Format(StringMaster.GetString("CharaTapFriendshipUp"), monsterData.monsterMG.monsterName);
		this.monsterBasicInfo.SetMonsterData(monsterData);
		this.monsterStatusList.SetValues(monsterData, false);
		this.monsterMedalList.SetValues(monsterData.userMonster);
		Transform transform = this.goMN_ICON_NOW.transform;
		GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(monsterData, transform.localScale, transform.localPosition, transform.parent, true, false);
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

	public void SetChangeData(MonsterData beforeMonsterData)
	{
		StatusValue values = this.monsterStatusList.GetValues();
		int num = values.friendship / ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
		int num2 = beforeMonsterData.userMonster.friendship.ToInt32();
		int num3 = num2 / ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
		int bonusStep = num - num3;
		StatusValue beforeStatus = new StatusValue
		{
			hp = values.hp - beforeMonsterData.GetFriendshipBonusHP(bonusStep),
			attack = values.attack - beforeMonsterData.GetFriendshipBonusAttack(bonusStep),
			defense = values.defense - beforeMonsterData.GetFriendshipBonusDefense(bonusStep),
			magicAttack = values.magicAttack - beforeMonsterData.GetFriendshipBonusSpAttack(bonusStep),
			magicDefense = values.magicDefense - beforeMonsterData.GetFriendshipBonusSpDefense(bonusStep),
			speed = values.speed - beforeMonsterData.GetFriendshipBonusSpeed(bonusStep),
			luck = values.luck,
			friendship = num2
		};
		this.monsterStatusChangeValueList.SetValues(beforeStatus, values);
	}
}
