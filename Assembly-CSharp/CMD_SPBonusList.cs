using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_SPBonusList : CMD
{
	private const float bonusChangeTime = 0.2f;

	[SerializeField]
	private List<UILabel> uiLabelList;

	[SerializeField]
	private GameObject changeButton;

	[SerializeField]
	private GameObject bonusBaseObj;

	private List<string> textDataList = new List<string>();

	private int epbListViewCnt;

	private TweenAlpha bonusPosTween;

	private bool bonusChange;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	public void SetViewData(List<string> textData)
	{
		this.textDataList = textData;
		for (int i = 0; i < this.uiLabelList.Count; i++)
		{
			this.uiLabelList[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this.uiLabelList.Count; j++)
		{
			if (this.textDataList.Count <= j)
			{
				break;
			}
			this.uiLabelList[j].text = this.textDataList[j];
			this.uiLabelList[j].gameObject.SetActive(true);
		}
		if (this.uiLabelList.Count < this.textDataList.Count)
		{
			this.changeButton.SetActive(true);
		}
		else
		{
			this.changeButton.SetActive(false);
		}
	}

	public void OnTapBonusChange()
	{
		if (this.bonusChange)
		{
			return;
		}
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1);
		this.bonusChange = true;
		base.StartCoroutine(this.BonusChangeAnima());
	}

	private IEnumerator BonusChangeAnima()
	{
		this.bonusPosTween = this.bonusBaseObj.AddComponent<TweenAlpha>();
		this.bonusPosTween.from = 1f;
		this.bonusPosTween.to = 0f;
		this.bonusPosTween.duration = 0.2f;
		this.bonusPosTween.PlayForward();
		yield return new WaitForSeconds(0.2f);
		this.epbListViewCnt++;
		int count = this.textDataList.Count / this.uiLabelList.Count;
		if (this.textDataList.Count % this.uiLabelList.Count != 0)
		{
			count++;
		}
		int viewNum = this.epbListViewCnt % count;
		for (int i = 0; i < this.uiLabelList.Count; i++)
		{
			this.uiLabelList[i].gameObject.SetActive(false);
			if (i + viewNum * this.uiLabelList.Count < this.textDataList.Count)
			{
				this.uiLabelList[i].gameObject.SetActive(true);
				this.uiLabelList[i].text = this.textDataList[i + viewNum * this.uiLabelList.Count];
			}
		}
		UnityEngine.Object.Destroy(this.bonusPosTween);
		this.bonusPosTween = null;
		this.bonusPosTween = this.bonusBaseObj.AddComponent<TweenAlpha>();
		this.bonusPosTween.from = 0f;
		this.bonusPosTween.to = 1f;
		this.bonusPosTween.duration = 0.2f;
		this.bonusPosTween.PlayForward();
		yield return new WaitForSeconds(0.2f);
		this.bonusChange = false;
		yield break;
	}

	public void ClosePopup()
	{
		this.ClosePanel(true);
	}
}
