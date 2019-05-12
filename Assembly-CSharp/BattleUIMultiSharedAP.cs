using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIMultiSharedAP : MonoBehaviour
{
	private const int MAX_AP = 16;

	[SerializeField]
	[Header("APアイコン")]
	private SharedApNotes[] apNotes = new SharedApNotes[16];

	[Header("AP値ラベル")]
	[SerializeField]
	private UILabel apLabel;

	[Header("AP Maxエフェクト")]
	[SerializeField]
	private GameObject apMaxEffect;

	private int currentAp;

	private Action callback;

	private BattleUIMultiSharedAP.State state;

	private List<BattleUIMultiSharedAP.AnimationData> animationDataList = new List<BattleUIMultiSharedAP.AnimationData>();

	private float timer;

	private int selectAp;

	private void Awake()
	{
		for (int i = 0; i < this.apNotes.Length; i++)
		{
			this.apNotes[i].icon.gameObject.SetActive(false);
		}
	}

	public void Play(int nextAp, Action callback = null)
	{
		this.Stop();
		this.animationDataList.Clear();
		int num = nextAp - this.currentAp;
		if (num > 0)
		{
			for (int i = 0; i < this.apNotes.Length; i++)
			{
				SharedApNotes apNote = this.apNotes[i];
				if (this.currentAp <= i && i < nextAp)
				{
					BattleUIMultiSharedAP.AnimationData animationData = new BattleUIMultiSharedAP.AnimationData();
					animationData.action = delegate()
					{
						apNote.icon.gameObject.SetActive(true);
						apNote.PlayUpAnimation();
					};
					animationData.check = (() => apNote.isAnimation);
					this.animationDataList.Add(animationData);
				}
			}
		}
		else
		{
			for (int j = this.apNotes.Length - 1; j >= 0; j--)
			{
				SharedApNotes apNote = this.apNotes[j];
				if (nextAp <= j && j < this.currentAp)
				{
					BattleUIMultiSharedAP.AnimationData animationData2 = new BattleUIMultiSharedAP.AnimationData();
					animationData2.action = delegate()
					{
						apNote.icon.gameObject.SetActive(true);
						apNote.PlayDownAnimation();
					};
					animationData2.check = (() => apNote.isAnimation);
					this.animationDataList.Add(animationData2);
				}
			}
		}
		this.currentAp = nextAp;
		this.apLabel.text = string.Concat(new object[]
		{
			"[FFF000]",
			this.currentAp,
			"[-]/",
			16
		});
		if (this.currentAp != 16)
		{
			this.apMaxEffect.SetActive(false);
		}
		this.callback = delegate()
		{
			this.apMaxEffect.SetActive(this.currentAp == 16);
			this.Stop();
			if (callback != null)
			{
				callback();
			}
			this.animationDataList.Clear();
		};
		this.state = BattleUIMultiSharedAP.State.Update;
	}

	private void Stop()
	{
		foreach (SharedApNotes sharedApNotes in this.apNotes)
		{
			sharedApNotes.StopUpAnimation();
			sharedApNotes.StopDownAnimation();
		}
		for (int j = 0; j < this.apNotes.Length; j++)
		{
			bool active = j < this.currentAp;
			this.apNotes[j].icon.gameObject.SetActive(active);
		}
		this.StopSelectAnimation();
		this.state = BattleUIMultiSharedAP.State.None;
		this.timer = 0f;
	}

	public void PlaySelectAnimation(int ap)
	{
		this.selectAp = ap;
	}

	public void StopSelectAnimation()
	{
		this.selectAp = 0;
	}

	private void Update()
	{
		int num = 0;
		for (int i = this.apNotes.Length - 1; i >= 0; i--)
		{
			bool flag = false;
			if (i < this.currentAp && num < this.selectAp)
			{
				num++;
				flag = true;
			}
			if (this.apNotes[i].multiAPActiveEffect.activeSelf != flag)
			{
				this.apNotes[i].multiAPActiveEffect.SetActive(flag);
			}
		}
		if (this.state != BattleUIMultiSharedAP.State.Update)
		{
			return;
		}
		bool flag2 = false;
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = 0.2f;
			flag2 = true;
		}
		foreach (BattleUIMultiSharedAP.AnimationData animationData in this.animationDataList)
		{
			if (flag2 && !animationData.isAction)
			{
				animationData.isAction = true;
				animationData.action();
				break;
			}
		}
		bool flag3 = false;
		foreach (BattleUIMultiSharedAP.AnimationData animationData2 in this.animationDataList)
		{
			if (animationData2.check())
			{
				flag3 = true;
				break;
			}
		}
		if (!flag3 && this.callback != null)
		{
			this.callback();
			this.callback = null;
		}
	}

	private class AnimationData
	{
		public Action action;

		public Func<bool> check;

		public bool isAction;
	}

	private enum State
	{
		None,
		Update
	}
}
