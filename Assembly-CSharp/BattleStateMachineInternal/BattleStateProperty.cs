using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class BattleStateProperty
	{
		[Header("複数ヒット時の間隔時間 (秒)")]
		public float multiHitIntervalWaitSecond = 1f;

		[Header("ダメージモーション後の間隔時間 (秒)")]
		public float skillAfterWaitSecond = 1f;

		[Header("「行動不能」演出時間 (秒)")]
		public float freezeActionWaitSecond = 3f;

		[Header("毒ヒットエフェクトの表示待ち時間 (秒)")]
		public float poisonHitEffectWaitSecond = 2f;

		[Header("自爆者の自爆演出時間 (秒)")]
		public float destructCharacterDeathActionWaitSecond = 1f;

		[Header("攻撃者のドレイン演出時間 (秒)")]
		public float attackerCharacterDrainActionWaitSecond = 1f;

		[Header("ターゲットのカウンター/リフレクション演出時間 (秒)")]
		public float targetCounterReflectionActionWaitSecond = 1f;

		[Header("開始時演出時間 (秒)")]
		public float introActionWaitSecond = 3f;

		[Header("復活の演出時間 (秒)")]
		public float revivalActionWaitSecond = 1f;

		[Header("アイテムドロップ時の演出時間 (秒)")]
		public float droppingItemActionWaitSecond = 1f;

		[Header("アイテムドロップ時のアイテム消去演出時間 (秒)")]
		public float droppingItemHiddenActionWaitSecond = 1f;

		[Header("ラウンド切替時の演出時間 (秒)")]
		public float RoundStartActionWaitSecond = 1f;

		[Header("回復時のスケーリング速度")]
		public float revivalActionSpeed = 0.01f;

		[Header("BGMフェードアウト時間 (秒)")]
		public float bgmFadeoutSecond = 0.25f;

		[Header("BGMクロスフェード時間 (秒)")]
		public float bgmCrossfadeSecond = 0.3f;

		[Header("攻撃順番")]
		public int[] playersTargetSelectOrder = new int[]
		{
			0,
			1,
			2
		};

		[Header("アイテムドロップ演出開始までの待ち時間 (秒)")]
		public float droppingItemActionStartWaitSecond = 0.5f;

		[Header("モンスター出現間隔 (秒)")]
		public float insertCharacterWaitSecond = 0.5f;

		[Header("バトルスタートプレイヤー出現演出時間 (秒)")]
		public float battleStartPlayerInsertWaitSecond = 3f;

		[Header("バトルスタート敵出現演出時間 (秒)")]
		public float battleStartEnemyInsertWaitSecond = 3f;

		[Header("バトルスタート演出時間 (秒)")]
		public float battleStartActionWaitSecond = 3f;

		[Header("ビッグボススタート演出時間 (秒)")]
		public float battleStartBigBossActionWaitSecond = 6f;

		[Header("ビッグボス退場演出時間 (秒)")]
		public float battleEndBigBossActionWaitSecond = 3.5f;

		[Header("ボスの開始時演出時間 (秒)")]
		public float bossIntroActionWaitSecond = 5f;

		[Header("ボスモンスター出現開始時時間 (秒)")]
		public float bossIntroInsertActionWaitSecond = 3f;

		[Header("NEXTWAVE演出時間 (秒)")]
		public float nextWaveActionWaitSecond = 3f;

		[Header("敵ターン開始演出時間 (秒)")]
		public float enemyTurnStartActionWaitSecond = 1.5f;

		[Header("メニュー表示BGMボリューム (%)")]
		[Range(1f, 0f)]
		public float pauseBgmVolumeLevel = 0.5f;

		[Header("通常攻撃演出速度 (倍速)")]
		public float attackActionSpeedTime = 1.5f;

		[Header("WIN演出のWinモーション再生開始までの時間 (秒)")]
		public float winActionStartMotionWaitSecond = 1.5f;

		[Header("スローモーションの速度倍率")]
		[Range(1f, 0f)]
		public float lastAttackSlowMotionSpeed = 0.1f;

		[Header("スローモーション開始までの時間 (秒)")]
		public float lastAttackSlowMotionStartWaitSecond = 0.25f;

		[Header("スローモーション終了までの時間 (秒)")]
		public float lastAttackSlowMotionWaitSecond = 1.5f;

		public BattleStateProperty Clone()
		{
			return new BattleStateProperty
			{
				multiHitIntervalWaitSecond = this.multiHitIntervalWaitSecond,
				skillAfterWaitSecond = this.skillAfterWaitSecond,
				freezeActionWaitSecond = this.freezeActionWaitSecond,
				poisonHitEffectWaitSecond = this.poisonHitEffectWaitSecond,
				destructCharacterDeathActionWaitSecond = this.destructCharacterDeathActionWaitSecond,
				attackerCharacterDrainActionWaitSecond = this.attackerCharacterDrainActionWaitSecond,
				targetCounterReflectionActionWaitSecond = this.targetCounterReflectionActionWaitSecond,
				introActionWaitSecond = this.introActionWaitSecond,
				revivalActionWaitSecond = this.revivalActionWaitSecond,
				droppingItemActionWaitSecond = this.droppingItemActionWaitSecond,
				droppingItemHiddenActionWaitSecond = this.droppingItemHiddenActionWaitSecond,
				RoundStartActionWaitSecond = this.RoundStartActionWaitSecond,
				revivalActionSpeed = this.revivalActionSpeed,
				bgmFadeoutSecond = this.bgmFadeoutSecond,
				bgmCrossfadeSecond = this.bgmCrossfadeSecond,
				playersTargetSelectOrder = (this.playersTargetSelectOrder.Clone() as int[]),
				droppingItemActionStartWaitSecond = this.droppingItemActionStartWaitSecond,
				insertCharacterWaitSecond = this.insertCharacterWaitSecond,
				battleStartPlayerInsertWaitSecond = this.battleStartPlayerInsertWaitSecond,
				battleStartEnemyInsertWaitSecond = this.battleStartEnemyInsertWaitSecond,
				battleStartActionWaitSecond = this.battleStartActionWaitSecond,
				bossIntroActionWaitSecond = this.bossIntroActionWaitSecond,
				bossIntroInsertActionWaitSecond = this.bossIntroInsertActionWaitSecond,
				nextWaveActionWaitSecond = this.nextWaveActionWaitSecond,
				enemyTurnStartActionWaitSecond = this.enemyTurnStartActionWaitSecond,
				pauseBgmVolumeLevel = this.pauseBgmVolumeLevel,
				attackActionSpeedTime = this.attackActionSpeedTime,
				winActionStartMotionWaitSecond = this.winActionStartMotionWaitSecond,
				lastAttackSlowMotionSpeed = this.lastAttackSlowMotionSpeed,
				lastAttackSlowMotionStartWaitSecond = this.lastAttackSlowMotionStartWaitSecond,
				lastAttackSlowMotionWaitSecond = this.lastAttackSlowMotionWaitSecond
			};
		}
	}
}
