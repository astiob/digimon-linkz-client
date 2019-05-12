using Master;
using System;
using System.Collections;
using UnityEngine;

public class BattleUIInitialize : MonoBehaviour
{
	private const int LoadingGaugeInterval = 10000;

	[Header("UIWidget")]
	[SerializeField]
	public UIWidget widget;

	[SerializeField]
	[Header("ローディングのオブジェクト")]
	public GameObject loadingGaugeRootObject;

	[SerializeField]
	[Header("ローディングのゲージ")]
	private UIGaugeManager loadingGauge;

	[SerializeField]
	[Header("ローディングのパーセンテージ")]
	private UITextReplacer loadingText;

	[Header("ローディングのパーセンテージ")]
	[SerializeField]
	private UILabel loadingLabel;

	[Header("ローディングのメッセージのローカライズ")]
	[SerializeField]
	private UITextReplacer loadingMessageText;

	[SerializeField]
	[Header("ローディングのメッセージのローカライズ")]
	private UILabel loadingMessageLabel;

	private int currentLoadingLevel = 1;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.loadingMessageLabel.text = StringMaster.GetString("SystemLoading");
	}

	public void ApplyLoadingGauge(int current, int max)
	{
		this.loadingGauge.SetMax(max);
		this.loadingGauge.SetValue(current);
		int num = Mathf.RoundToInt(Mathf.InverseLerp(0f, (float)max, (float)current) * 100f);
		string @string = StringMaster.GetString("SystemPercent");
		this.loadingLabel.text = string.Format(@string, num);
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
			this.StartCoroutine(this.TextAnimation());
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

	private IEnumerator TextAnimation()
	{
		float currentTime = 0f;
		int count = 0;
		for (;;)
		{
			currentTime -= Time.deltaTime;
			if (currentTime < 0f)
			{
				currentTime = 0.5f;
				count++;
				if (count > 3)
				{
					count = 0;
				}
				string addString = string.Empty;
				for (int i = 0; i < count; i++)
				{
					addString += ".";
				}
				this.loadingMessageLabel.text = StringMaster.GetString("SystemLoading");
				UILabel uilabel = this.loadingMessageLabel;
				uilabel.text += addString;
			}
			yield return null;
		}
		yield break;
	}
}
