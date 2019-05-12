using Monster;
using System;
using UnityEngine;

namespace MonsterPicturebook
{
	public sealed class UI_PictureDetailedInfo : MonoBehaviour
	{
		[SerializeField]
		private UILabel monsterName;

		[SerializeField]
		private UILabel specificType;

		[SerializeField]
		private UILabel growStep;

		[SerializeField]
		private UILabel tribe;

		[SerializeField]
		private UILabel description;

		[SerializeField]
		private UILabel[] skillNameList;

		[SerializeField]
		private UILabel[] skillDescriptionList;

		public void SetMonsterData(PicturebookDetailedInfo viewInfo)
		{
			this.monsterName.text = viewInfo.monster.monsterMaster.Group.monsterName;
			this.specificType.text = MonsterSpecificTypeData.GetSpecificTypeName(viewInfo.monster.monsterMaster.Group.monsterStatusId);
			this.growStep.text = MonsterGrowStepData.GetGrowStepName(viewInfo.monster.monsterMaster.Group.growStep);
			this.tribe.text = MonsterTribeData.GetTribeName(viewInfo.monster.monsterMaster.Group.tribe);
			this.description.text = viewInfo.monster.monsterMaster.Group.description;
			for (int i = 0; i < viewInfo.uniqueSkillCount; i++)
			{
				this.skillNameList[i].text = viewInfo.skillGroupSubIdList[i].Simple.name;
				this.skillDescriptionList[i].text = viewInfo.skillGroupSubIdList[i].Simple.description;
			}
		}
	}
}
