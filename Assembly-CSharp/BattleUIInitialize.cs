using Master;
using System;
using System.Collections;
using UnityEngine;

public class BattleUIInitialize : MonoBehaviour
{
	private const int LoadingGaugeInterval = 10000;

	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[Header("ローディングのオブジェクト")]
	[SerializeField]
	public GameObject loadingGaugeRootObject;

	[SerializeField]
	[Header("ローディングのゲージ")]
	private UIGaugeManager loadingGauge;

	[Header("ローディングのパーセンテージ")]
	[SerializeField]
	private UITextReplacer loadingText;

	[Header("ローディングのメッセージのローカライズ")]
	[SerializeField]
	private UITextReplacer loadingMessageText;

	private int currentLoadingLevel = 1;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		TextReplacerValue replacerValue = new TextReplacerValue(StringMaster.GetString("SystemLoading"));
		this.loadingMessageText.SetValue(0, replacerValue);
	}

	public void ApplyLoadingGauge(int current, int max)
	{
		this.loadingGauge.SetMax(max);
		this.loadingGauge.SetValue(current);
		this.loadingText.SetValue(0, new TextReplacerValue(Mathf.RoundToInt(Mathf.InverseLerp(0f, (float)max, (float)current) * 100f)));
	}

	public UIPanel panel
	{
		get
		{
			return this.widget.panel;
		}
	}

	public Action GetLoadingInvoke(int maxLoading)
	{
		return delegate()
		{
			this.ApplyLoadingGauge(this.currentLoadingLevel, maxLoading);
			this.currentLoadingLevel += 10000;
			IEnumerator routine = this.LoadingGaugeAnimation(this.currentLoadingLevel - 10000, this.currentLoadingLevel, maxLoading * 10000);
			this.StartCoroutine(routine);
		};
	}

	private IEnumerator LoadingGaugeAnimation(int current, int diff, int max)
	{
		int val = current;
		int pow = diff - current;
		while (val < diff)
		{
			this.ApplyLoadingGauge(val, max);
			pow = Mathf.RoundToInt((float)pow * 0.1f);
			if (pow < 1)
			{
				break;
			}
			val += pow;
			yield return null;
		}
		this.ApplyLoadingGauge(diff, max);
		yield break;
	}
}
