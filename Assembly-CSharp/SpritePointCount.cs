using System;
using UnityEngine;

public class SpritePointCount : MonoBehaviour
{
	private const int secondsCount = 60;

	[Header("表示番号のスプライト")]
	[SerializeField]
	private UISprite[] numSprite;

	[Header("上桁ゼロ表示しない = true")]
	[SerializeField]
	private bool dontShowUpperZero;

	[SerializeField]
	private AnimationCurve countCurve;

	private int currentNum;

	private bool isStart;

	private int endNum;

	private float time;

	private float intervalTime;

	private int countUpNum;

	private Action finished;

	private bool curveFlag;

	private float curveTime;

	public void SetNum(int num)
	{
		this.isStart = false;
		this.currentNum = num;
		this.ChangeNum(num);
	}

	public void StartCountUp(int num, Action finished = null, int countUpNum = 1, float intervalTime = 0.01f)
	{
		this.isStart = true;
		this.endNum = num;
		this.finished = finished;
		this.countUpNum = countUpNum;
		this.intervalTime = intervalTime;
		this.curveFlag = false;
	}

	public void StartCountUpCurve(int num, Action finished = null, float animaTime = 1f)
	{
		this.isStart = true;
		this.endNum = num;
		this.finished = finished;
		this.curveFlag = true;
		this.curveTime = animaTime;
	}

	private void ChangeNum(int num, UISprite numSprite)
	{
		numSprite.spriteName = string.Format("Common02_FriendshipN_{0}", num);
	}

	private void ChangeNum(int num)
	{
		int num2 = num;
		for (int i = 0; i < this.numSprite.Length; i++)
		{
			int num3 = num2 % 10;
			if (this.dontShowUpperZero && i > 0 && num2 == 0)
			{
				if (this.numSprite[i].gameObject.activeSelf)
				{
					this.numSprite[i].gameObject.SetActive(false);
				}
			}
			else
			{
				if (!this.numSprite[i].gameObject.activeSelf)
				{
					this.numSprite[i].gameObject.SetActive(true);
				}
				this.ChangeNum(num3, this.numSprite[i]);
			}
			num2 /= 10;
		}
	}

	private void Update()
	{
		if (!this.isStart)
		{
			return;
		}
		if (!this.curveFlag)
		{
			if (this.time >= this.intervalTime)
			{
				this.time -= this.intervalTime;
				this.currentNum += this.countUpNum;
				if (this.currentNum > this.endNum)
				{
					this.currentNum = this.endNum;
				}
				this.ChangeNum(this.currentNum);
			}
			if (this.currentNum >= this.endNum)
			{
				this.isStart = false;
				if (this.finished != null)
				{
					this.finished();
				}
				return;
			}
			this.time += Time.deltaTime;
		}
		else if (60f * this.curveTime < (float)this.endNum - (float)this.currentNum)
		{
			if (this.time / this.curveTime >= 1f)
			{
				this.isStart = false;
				if (this.finished != null)
				{
					this.finished();
				}
				this.ChangeNum(this.endNum);
				return;
			}
			int num = (int)(this.countCurve.Evaluate(this.time / this.curveTime) * ((float)this.endNum - (float)this.currentNum));
			this.ChangeNum(this.currentNum + num);
			this.time += Time.deltaTime;
		}
		else
		{
			this.currentNum++;
			if (this.currentNum > this.endNum)
			{
				this.currentNum = this.endNum;
			}
			this.ChangeNum(this.currentNum);
			if (this.currentNum >= this.endNum)
			{
				this.isStart = false;
				if (this.finished != null)
				{
					this.finished();
				}
				return;
			}
		}
	}
}
