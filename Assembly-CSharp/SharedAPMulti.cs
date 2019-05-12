using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SharedAPMulti : MonoBehaviour
{
	public const float deltaTime = 0.2f;

	[SerializeField]
	private int _APNum;

	[SerializeField]
	[Header("APアイコン")]
	private SharedApNotes[] apNotes = new SharedApNotes[16];

	[Header("AP Maxエフェクト")]
	[SerializeField]
	private GameObject multiAPMaxEffect;

	[Header("分子数字")]
	[SerializeField]
	private UITextReplacer numLabel;

	[Header("スラッシュ分母数字")]
	[SerializeField]
	private UITextReplacer maxNumLabel;

	private bool isPlayedFirstAnimation;

	private int dirtyAPNum = -1;

	private List<Action> apUpActions = new List<Action>();

	private TextReplacerValue apNumTextReplacerValue = new TextReplacerValue(0);

	public bool isPlayingSharedAp { get; set; }

	public BattleTime time { private get; set; }

	public List<Action> HpUpHudActions { get; set; }

	public List<Action> HpGaugeUpHudActions { get; private set; }

	public bool IsMaxAnimated()
	{
		return this.dirtyAPNum == 16;
	}

	public int APNum
	{
		get
		{
			return this._APNum;
		}
		set
		{
			this._APNum = value;
		}
	}

	public void Initialize()
	{
		this.StopScaleUpAnim();
		this.maxNumLabel.SetValue(0, new TextReplacerValue(16));
		this.HpUpHudActions = new List<Action>();
		this.HpGaugeUpHudActions = new List<Action>();
	}

	public void HideAllDots()
	{
		for (int i = 0; i < this.apNotes.Length; i++)
		{
			NGUITools.SetActiveSelf(this.apNotes[i].icon.gameObject, false);
		}
		this.UpdateNumLabel(0);
	}

	public void PlayFirstFullAnimation()
	{
		if (this.isPlayedFirstAnimation)
		{
			return;
		}
		for (int i = 0; i < 9; i++)
		{
			float waitingTerm = (float)i * 0.2f;
			this.PlayApUpAnim(i, waitingTerm, false);
		}
		if (this.APNum == 16)
		{
			this.PlayScaleUpAnim();
		}
		this.isPlayedFirstAnimation = true;
	}

	public void PlayApUpAnimations()
	{
		foreach (Action action in this.apUpActions)
		{
			action();
		}
		if (this.APNum == 16)
		{
			this.PlayScaleUpAnim();
		}
		else
		{
			this.StopScaleUpAnim();
		}
		this.apUpActions.Clear();
	}

	public void Refresh(bool isAnimMode)
	{
		if (this.dirtyAPNum == this.APNum)
		{
			global::Debug.Log("同じなので処理が無駄になるので何もしない.");
			return;
		}
		bool flag = false;
		int num = 0;
		for (int i = 0; i < this.apNotes.Length; i++)
		{
			if (i < this.APNum)
			{
				if (isAnimMode)
				{
					if (this.dirtyAPNum <= i)
					{
						bool isLast = i == this.APNum - 1;
						int index = i;
						float waitTime = (float)num * 0.2f;
						flag = true;
						this.apUpActions.Add(delegate
						{
							this.PlayApUpAnim(index, waitTime, isLast);
						});
						num++;
					}
					else
					{
						this.MakeActiveAP(i);
					}
				}
				else
				{
					this.MakeActiveAP(i);
				}
			}
			else if (isAnimMode)
			{
				this.PlayReduceAnim(i);
			}
			else
			{
				this.MakeDeactiveAP(i);
			}
		}
		this.StopActiveAnim();
		if (this.APNum != 16)
		{
			this.StopScaleUpAnim();
		}
		this.dirtyAPNum = this.APNum;
		if (!flag)
		{
			this.RefreshNumLabel();
		}
	}

	public void RefreshNumLabel()
	{
		this.UpdateNumLabel(this.APNum);
	}

	public void PlayActiveAnim(int needAp)
	{
		int num = 0;
		foreach (SharedApNotes sharedApNotes in this.apNotes)
		{
			GameObject multiAPActiveEffect = sharedApNotes.multiAPActiveEffect;
			if (this.APNum - needAp <= num && num < this.APNum)
			{
				NGUITools.SetActiveSelf(multiAPActiveEffect, false);
				NGUITools.SetActiveSelf(multiAPActiveEffect, true);
			}
			else
			{
				NGUITools.SetActiveSelf(multiAPActiveEffect, false);
			}
			num++;
		}
	}

	public void StopActiveAnim()
	{
		foreach (SharedApNotes sharedApNotes in this.apNotes)
		{
			NGUITools.SetActiveSelf(sharedApNotes.multiAPActiveEffect, false);
		}
	}

	private void Awake()
	{
		this.HideAllDots();
	}

	private void UpdateNumLabel(int aAPNum)
	{
		this.apNumTextReplacerValue.intValue = aAPNum;
		this.numLabel.SetValue(0, this.apNumTextReplacerValue);
	}

	private void MakeDeactiveAP(int apIndex)
	{
		this.PlayReduceAnim(apIndex);
		NGUITools.SetActiveSelf(this.apNotes[apIndex].icon.gameObject, false);
		this.apNotes[apIndex].icon.transform.localScale = Vector3.zero;
	}

	private void MakeActiveAP(int apIndex)
	{
		this.apNotes[apIndex].tweenerActivePlays.enabled = false;
		NGUITools.SetActiveSelf(this.apNotes[apIndex].icon.gameObject, true);
		this.apNotes[apIndex].icon.transform.localScale = Vector3.one;
	}

	private void PlayReduceAnim(int apIndex)
	{
		this.apNotes[apIndex].tweenerActivePlays.enabled = true;
	}

	private void PlayApUpAnim(int apIndex, float waitingTerm, bool isLast)
	{
		AppCoroutine.Start(this.WaitAndPlayApUpAnim(apIndex, waitingTerm, isLast), false);
	}

	private IEnumerator WaitAndPlayApUpAnim(int apIndex, float waitingTerm, bool isLast)
	{
		IEnumerator wait = this.time.WaitForCertainPeriodTimeAction(waitingTerm, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		GameObject apUpGo = this.apNotes[apIndex].multiAPUpEffect;
		NGUITools.SetActiveSelf(apUpGo, true);
		AnimatorFinishEventTrigger apAnimCallback = apUpGo.GetComponent<AnimatorFinishEventTrigger>();
		apAnimCallback.OnFinishAnimation = delegate(string str)
		{
			NGUITools.SetActiveSelf(apUpGo, false);
		};
		this.MakeActiveAP(apIndex);
		this.UpdateNumLabel(apIndex + 1);
		if (isLast)
		{
			global::Debug.Log("共有APのUIの最後");
			this.RefreshNumLabel();
			foreach (Action hpUpHudAction in this.HpUpHudActions)
			{
				hpUpHudAction();
			}
			this.HpUpHudActions.Clear();
			foreach (Action hpGaugeUpHudAction in this.HpGaugeUpHudActions)
			{
				hpGaugeUpHudAction();
			}
			this.isPlayingSharedAp = false;
			this.HpGaugeUpHudActions.Clear();
		}
		SoundPlayer.PlayBattleRecoverAPSE();
		yield break;
	}

	private void PlayScaleUpAnim()
	{
		NGUITools.SetActiveSelf(this.multiAPMaxEffect, true);
	}

	private void StopScaleUpAnim()
	{
		NGUITools.SetActiveSelf(this.multiAPMaxEffect, false);
	}
}
