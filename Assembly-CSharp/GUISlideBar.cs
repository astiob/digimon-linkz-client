using Master;
using System;
using System.Collections;
using UnityEngine;

public class GUISlideBar : MonoBehaviour
{
	private GameObject goLeft;

	private GameObject goRight;

	private GameObject goKnob;

	private GUICollider colKnob;

	private UISprite NGSprbBar;

	private GameStringsFont curValueFont;

	private GameObject goNowValTag;

	private GameObject goNowVal;

	private GameStringsFont nowValueFont;

	private float defBarWidth;

	private float barWidth;

	private float barMoveWidth;

	private float barBlank = 16f;

	private float barLP;

	private float barRP;

	private int maxValue_;

	private int curValue_;

	private int dispAdd;

	private Vector3 vt = new Vector3(0f, 0f, 0f);

	private Vector2 tpos = new Vector2(0f, 0f);

	private Vector2 startPos;

	private Vector2 movePos;

	private Vector3 StartKnob;

	private bool isDragging;

	private Action<int> action;

	private void Awake()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.name == "L_BTN")
				{
					this.goLeft = transform.gameObject;
				}
				if (transform.name == "R_BTN")
				{
					this.goRight = transform.gameObject;
				}
				if (transform.name == "KNOB")
				{
					this.goKnob = transform.gameObject;
					this.colKnob = transform.gameObject.GetComponent<GUICollider>();
					IEnumerator enumerator2 = transform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							Transform transform2 = (Transform)obj2;
							if (transform2.name == "NOW_NUM")
							{
								this.goNowVal = transform2.gameObject;
								this.nowValueFont = transform2.gameObject.GetComponent<GameStringsFont>();
							}
							if (transform2.name == "TAG")
							{
								this.goNowValTag = transform2.gameObject;
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				if (transform.name == "BAR")
				{
					this.NGSprbBar = transform.gameObject.GetComponent<UISprite>();
				}
				if (transform.name == "CUR_NUM")
				{
					this.curValueFont = transform.gameObject.GetComponent<GameStringsFont>();
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		this.colKnob.onTouchBegan += delegate(Touch touch, Vector2 pos)
		{
			this.tpos = pos;
			this.KnobTouchBegan();
		};
		this.colKnob.onTouchMoved += delegate(Touch touch, Vector2 pos)
		{
			this.tpos = pos;
			this.KnobTouchMoved();
		};
		this.colKnob.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.tpos = pos;
			this.KnobTouchEnded();
		};
	}

	public void SetUp(float width, int maxValue, int add, int value)
	{
		if (this.NGSprbBar != null)
		{
			this.defBarWidth = (float)this.NGSprbBar.width;
			this.barWidth = width;
			this.NGSprbBar.width = (int)this.barWidth;
		}
		this.barMoveWidth = this.barWidth - this.barBlank - this.barBlank;
		this.barLP = -(this.barWidth / 2f) + this.barBlank;
		this.barRP = this.barWidth / 2f - this.barBlank;
		float num = (this.barWidth - this.defBarWidth) / 2f;
		this.vt = this.goRight.transform.localPosition;
		this.vt.x = this.vt.x + num;
		this.goRight.transform.localPosition = this.vt;
		this.vt = this.goLeft.transform.localPosition;
		this.vt.x = this.vt.x - num;
		this.goLeft.transform.localPosition = this.vt;
		this.dispAdd = add;
		this.maxValue_ = maxValue;
		this.curValue_ = value;
		this.SetKNOB();
		this.SetNumFontAll();
		if (this.goNowVal != null)
		{
			this.goNowVal.SetActive(false);
		}
		if (this.goNowValTag != null)
		{
			this.goNowValTag.SetActive(false);
		}
	}

	public int GetCurrentValue()
	{
		return this.curValue_;
	}

	public void SetCurrentValue(int value)
	{
		if (value < 0)
		{
			value = 0;
		}
		if (value >= this.maxValue_)
		{
			value = this.maxValue_ - 1;
		}
		this.curValue_ = value;
		this.SetKNOB();
		this.SetNumFontAll();
	}

	public void SetActionFunc(Action<int> act)
	{
		this.action = act;
	}

	public bool IsDragging()
	{
		return this.isDragging;
	}

	public int GetNowValue()
	{
		this.vt = this.goKnob.transform.localPosition;
		this.vt.x = this.vt.x + this.barMoveWidth / 2f;
		float num = this.barMoveWidth / (float)(this.maxValue_ - 1);
		int num2 = 0;
		while (this.vt.x >= num)
		{
			this.vt.x = this.vt.x - num;
			num2++;
		}
		if (this.vt.x > num / 2f)
		{
			num2++;
		}
		return num2;
	}

	private void SetNumFontAll()
	{
		int num = this.curValue_ + this.dispAdd;
		int num2 = this.maxValue_ - 1 + this.dispAdd;
		if (this.curValueFont != null)
		{
			this.curValueFont.text = string.Format(StringMaster.GetString("SystemFraction"), num.ToString(), num2.ToString());
		}
		if (this.nowValueFont != null)
		{
			this.nowValueFont.text = string.Format(StringMaster.GetString("SystemFraction"), num.ToString(), num2.ToString());
		}
	}

	private void SetNumFontMove()
	{
		int num = this.GetNowValue() + this.dispAdd;
		int num2 = this.maxValue_ - 1 + this.dispAdd;
		if (this.nowValueFont != null)
		{
			this.nowValueFont.text = string.Format(StringMaster.GetString("SystemFraction"), num.ToString(), num2.ToString());
		}
	}

	private void SetKNOB()
	{
		this.vt = this.goKnob.transform.localPosition;
		this.vt.x = -(this.barMoveWidth / 2f) + this.barMoveWidth * (float)this.curValue_ / (float)(this.maxValue_ - 1);
		this.goKnob.transform.localPosition = this.vt;
	}

	private void KnobTouchBegan()
	{
		this.isDragging = true;
		this.startPos = this.tpos;
		this.StartKnob = this.goKnob.transform.localPosition;
		if (this.goNowVal != null)
		{
			this.goNowVal.SetActive(true);
		}
		if (this.goNowValTag != null)
		{
			this.goNowValTag.SetActive(true);
		}
	}

	private void KnobTouchMoved()
	{
		this.isDragging = true;
		this.SetNumFontMove();
		this.movePos = this.tpos;
		this.vt = this.StartKnob;
		this.vt.x = this.vt.x + (this.movePos.x - this.startPos.x);
		if (this.vt.x < this.barLP)
		{
			this.vt.x = this.barLP;
		}
		if (this.vt.x > this.barRP)
		{
			this.vt.x = this.barRP;
		}
		this.goKnob.transform.localPosition = this.vt;
	}

	private void KnobTouchEnded()
	{
		this.isDragging = false;
		this.curValue_ = this.GetNowValue();
		if (this.action != null)
		{
			this.action(this.curValue_);
		}
		this.SetKNOB();
		this.SetNumFontAll();
		if (this.goNowVal != null)
		{
			this.goNowVal.SetActive(false);
		}
		if (this.goNowValTag != null)
		{
			this.goNowValTag.SetActive(false);
		}
	}

	private void OnClickedR()
	{
		if (this.curValue_ < this.maxValue_ - 1)
		{
			this.curValue_++;
			this.SetKNOB();
			this.SetNumFontAll();
			if (this.action != null)
			{
				this.action(this.curValue_);
			}
		}
	}

	private void OnClickedL()
	{
		if (this.curValue_ > 0)
		{
			this.curValue_--;
			this.SetKNOB();
			this.SetNumFontAll();
			if (this.action != null)
			{
				this.action(this.curValue_);
			}
		}
	}
}
