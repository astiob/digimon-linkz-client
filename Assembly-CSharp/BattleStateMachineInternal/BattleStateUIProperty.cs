using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class BattleStateUIProperty
	{
		[Header("HUDアイコンインスタンスの大きさ.")]
		public Vector3 hudObjectLocalScale = new Vector3(1f, 1f, 1f);

		[Header("ヒットアイコンインスタンスの大きさ.")]
		public Vector3 hitIconLocalScale = new Vector3(1f, 1f, 1f);

		[Header("毒アイコンインスタンスの大きさ.")]
		public float onPoisonScalingSizeHitIcon = 0.7f;

		[Header("選択アイコンインスタンスの大きさ.")]
		public Vector3 manualSelectTargetObjectLocalScale = new Vector3(1f, 1f, 1f);

		[Header("ヒットアイコン生成数.")]
		public int hitIconLengthTime = 9;

		[Header("ドロップアイテムの出現UI移動完了までの時間.")]
		public float droppingItemMoveDuration = 0.25f;

		[Header("ドロップアイテムの出現UI演出完了までの時間.")]
		public float droppingItemUIActionDuration = 0.255f;

		[Header("ドロップアイテムの出現UIイーズタイプ.")]
		public iTween.EaseType droppingItemMoveEaseType = iTween.EaseType.easeInOutSine;

		[Header("初期誘導UI表示までの時間.")]
		public float showInitialIntroductionDialogWaitSecond = 1f;

		[Header("初期誘導UI表示後画面切り替えまでの時間.")]
		public float afterHideInitialIntroductionDialogWaitSecond = 0.5f;

		[Header("WIN演出のTAP NEXTが表示されるまでの時間 (秒)")]
		public float winActionShowNextButtonWaitSecond = 1.5f;

		public BattleStateUIProperty Clone()
		{
			return new BattleStateUIProperty
			{
				hudObjectLocalScale = this.hudObjectLocalScale,
				hitIconLocalScale = this.hitIconLocalScale,
				onPoisonScalingSizeHitIcon = this.onPoisonScalingSizeHitIcon,
				manualSelectTargetObjectLocalScale = this.manualSelectTargetObjectLocalScale,
				hitIconLengthTime = this.hitIconLengthTime,
				droppingItemMoveDuration = this.droppingItemMoveDuration,
				droppingItemUIActionDuration = this.droppingItemUIActionDuration,
				droppingItemMoveEaseType = this.droppingItemMoveEaseType,
				showInitialIntroductionDialogWaitSecond = this.showInitialIntroductionDialogWaitSecond,
				afterHideInitialIntroductionDialogWaitSecond = this.afterHideInitialIntroductionDialogWaitSecond,
				winActionShowNextButtonWaitSecond = this.winActionShowNextButtonWaitSecond
			};
		}
	}
}
