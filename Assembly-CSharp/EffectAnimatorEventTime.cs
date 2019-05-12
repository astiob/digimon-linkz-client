using System;
using System.Collections;
using UnityEngine;

public sealed class EffectAnimatorEventTime : MonoBehaviour
{
	[SerializeField]
	private float[] eventTimes;

	public void SetEvent(int eventIndex, Action effectEvent)
	{
		if (this.eventTimes != null && eventIndex < this.eventTimes.Length && effectEvent != null)
		{
			base.StartCoroutine(this.DelayEffectEvent(this.eventTimes[eventIndex], effectEvent));
		}
	}

	private IEnumerator DelayEffectEvent(float delayTime, Action effectEvent)
	{
		yield return new WaitForSeconds(delayTime);
		effectEvent();
		yield break;
	}
}
