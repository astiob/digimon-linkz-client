using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using UnityEngine;

public sealed class AttackTime : MonoBehaviour
{
	[SerializeField]
	private GameObject _hurryUpObject;

	[SerializeField]
	[Header("MAX時間(MultiBattleDataから自動セット)")]
	private int maxAttackTime = 30;

	[SerializeField]
	[Header("急かす時間(MultiBattleDataから自動セット)")]
	private int hurryUpAttackTime = 10;

	[SerializeField]
	[Header("攻撃のカウントダウンの急かす色")]
	private Color hurryUpColor = new Color32(byte.MaxValue, 240, 0, byte.MaxValue);

	private int nowTime = 10;

	[Header("'あと'ラベル")]
	[SerializeField]
	private UILabel afterLabel;

	[SerializeField]
	[Header("時間ラベル")]
	private UILabel timeLabel;

	private IEnumerator cor;

	public Action callBackAction { private get; set; }

	private GameObject hurryUpObject
	{
		get
		{
			return this._hurryUpObject;
		}
	}

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.afterLabel.text = StringMaster.GetString("BattleNotice-08");
	}

	private void OnEnable()
	{
		this.maxAttackTime = ClassSingleton<MultiBattleData>.Instance.MaxAttackTime;
		this.hurryUpAttackTime = ClassSingleton<MultiBattleData>.Instance.HurryUpAttackTime;
		this.StartTimer();
	}

	private void OnDisable()
	{
		this.StopTimer();
	}

	public void StopTimer()
	{
		if (this.cor != null)
		{
			base.StopCoroutine(this.cor);
		}
	}

	public void RestartTimer()
	{
		if (this.cor != null && base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.cor);
		}
	}

	public Func<bool> checkEnemyRecoverDialog { private get; set; }

	private void StartTimer()
	{
		this.nowTime = this.maxAttackTime;
		this.timeLabel.text = this.nowTime.ToString();
		this.timeLabel.color = Color.white;
		if (this.hurryUpObject != null)
		{
			NGUITools.SetActiveSelf(this.hurryUpObject, false);
		}
		this.cor = this.RunCountDown();
		base.StartCoroutine(this.cor);
		if (this.checkEnemyRecoverDialog != null && this.checkEnemyRecoverDialog())
		{
			this.StopTimer();
		}
	}

	private IEnumerator RunCountDown()
	{
		for (;;)
		{
			this.timeLabel.text = this.nowTime.ToString();
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.nowTime--;
			if (this.nowTime < 0)
			{
				break;
			}
			if (this.nowTime <= this.hurryUpAttackTime)
			{
				NGUITools.SetActiveSelf(this.hurryUpObject, true);
				this.timeLabel.color = this.hurryUpColor;
				if (this.nowTime != 0)
				{
					SoundPlayer.PlayBattleCountDownSE();
				}
			}
		}
		if (this.callBackAction == null)
		{
			global::Debug.LogErrorFormat("{0}ゲームオブジェクトの{1}の{2}変数にコールバック登録お願いします.", new object[]
			{
				base.name,
				base.GetType(),
				"icallBackAction"
			});
		}
		else
		{
			this.callBackAction();
		}
		yield break;
		yield break;
	}
}
