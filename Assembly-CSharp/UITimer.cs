using System;
using System.Collections;
using UnityEngine;

public sealed class UITimer : MonoBehaviour
{
	[SerializeField]
	private UITextReplacer label;

	[SerializeField]
	private int maxTime;

	private Action callback;

	private IEnumerator coroutine;

	private void OnEnable()
	{
		this.StartTimer();
	}

	private void OnDisable()
	{
		this.Stop();
	}

	public void Set(int maxTime, Action timerEndCallback)
	{
		this.maxTime = maxTime;
		this.callback = timerEndCallback;
	}

	public void Stop()
	{
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
	}

	public void Restart()
	{
		if (this.coroutine != null && base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.coroutine);
		}
	}

	private void StartTimer()
	{
		this.coroutine = this.RunCountDown();
		base.StartCoroutine(this.coroutine);
	}

	private IEnumerator RunCountDown()
	{
		for (int i = 0; i < this.maxTime; i++)
		{
			this.label.SetValue(0, new TextReplacerValue(this.maxTime - i));
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
		}
		this.label.SetValue(0, new TextReplacerValue(0));
		if (this.callback != null)
		{
			this.callback();
		}
		yield break;
	}
}
