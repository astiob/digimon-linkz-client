using Monster;
using MonsterIcon;
using MonsterIconExtensions;
using System;
using UnityEngine;

namespace UI.MonsterInfoParts
{
	public sealed class MonsterSelectedIcon : MonsterIconTouchEvent
	{
		[SerializeField]
		private int iconSize;

		private MonsterIcon monsterIcon;

		private void InitializeMonsterIcon(MonsterIcon icon)
		{
			UIWidget component = base.GetComponent<UIWidget>();
			int depth = component.depth;
			this.monsterIcon = icon;
			this.monsterIcon.Initialize(base.transform, this.iconSize, depth);
		}

		public void Initialize()
		{
			MonsterIcon icon = MonsterIconFactory.CreateIcon(76);
			this.Initialize(MonsterIconFactory.Copy(icon));
		}

		public void Initialize(MonsterIcon icon)
		{
			this.InitializeMonsterIcon(icon);
			base.InitializeInputEvent();
		}

		public void SetMonsterData(MonsterUserData monster, bool isGimmick)
		{
			this.monsterIcon.SetUserMonsterDetailed(monster, isGimmick);
		}

		public void ClearMonsterData()
		{
			this.monsterIcon.ClaerDetailed();
			this.actionTouch = null;
			this.actionLongPress = null;
		}
	}
}
