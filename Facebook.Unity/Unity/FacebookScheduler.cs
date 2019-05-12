using System;
using System.Collections;
using UnityEngine;

namespace Facebook.Unity
{
	internal class FacebookScheduler : MonoBehaviour, IFacebookScheduler
	{
		public void Schedule(Action action, long delay)
		{
			base.StartCoroutine(this.DelayEvent(action, delay));
		}

		public IEnumerator DelayEvent(Action action, long delay)
		{
			yield return new WaitForSeconds((float)delay);
			action();
			yield break;
		}
	}
}
