using System;
using System.Collections;
using UnityEngine;

public class BattleTime : BattleFunctionBase
{
	public override void BattleTriggerInitialize()
	{
		this.SetPlaySpeed(false, false);
	}

	public IEnumerator WaitForCertainPeriodTimeAction(float waitSecond, Action yieldAction = null, Action endAction = null)
	{
		IEnumerator wait = this.WaitForCertainPeriodTimeAction(waitSecond, false, yieldAction, endAction);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	public IEnumerator WaitForCertainPeriodTimeAction(float waitSecond, bool ignoreTimeScale, Action yieldAction = null, Action endAction = null)
	{
		for (float timing = 0f; timing < waitSecond; timing += ((!ignoreTimeScale) ? Time.deltaTime : (Time.unscaledDeltaTime * this.GetSpeed())))
		{
			if (yieldAction != null)
			{
				yieldAction();
			}
			yield return null;
		}
		if (endAction != null)
		{
			endAction();
		}
		yield break;
	}

	private float GetSpeed()
	{
		return (float)(((!base.hierarchyData.on2xSpeedPlay) ? 1 : 2) * ((!base.battleStateData.isShowMenuWindow || base.battleMode == BattleMode.Multi) ? 1 : 0));
	}

	public void SetPlaySpeed(bool on2x, bool onPose = false)
	{
		if (onPose)
		{
			Time.timeScale = 0f;
			return;
		}
		float num = (!base.battleStateData.isSlowMotion) ? 1f : base.stateManager.stateProperty.lastAttackSlowMotionSpeed;
		if (on2x)
		{
			Time.timeScale = ((!base.battleStateData.isInvocationEffectPlaying) ? 1f : base.stateManager.stateProperty.attackActionSpeedTime) * 2f * num;
		}
		else
		{
			Time.timeScale = ((!base.battleStateData.isInvocationEffectPlaying) ? 1f : base.stateManager.stateProperty.attackActionSpeedTime) * num;
		}
	}

	public bool isPause
	{
		get
		{
			return Time.timeScale == 0f;
		}
	}
}
