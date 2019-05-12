using System;
using UnityEngine;

public class GUISelectPanelGashaEdit : GUISelectPanelBSPartsLR
{
	private float partW;

	private float difMaxLocate;

	public int selectNum;

	public float limitOrigin = 4f;

	public float timeCoun;

	protected override void Awake()
	{
		base.Awake();
		base.selectParts.SetActive(false);
		this.timeCoun = this.limitOrigin;
	}

	protected void OnEnable()
	{
		this.timeCoun = this.limitOrigin;
		this.selectNum = 0;
	}

	protected override void Update()
	{
		base.Update();
		this.AutoScroll();
	}

	protected void AutoScroll()
	{
		this.timeCoun -= Time.deltaTime;
		if ((double)this.timeCoun <= 0.0 && this.partsCount > 1)
		{
			this.timeCoun = this.limitOrigin;
			base.OnTouchMoveRight();
		}
	}

	public void ResetAutoScrollTime()
	{
		this.timeCoun = this.limitOrigin;
	}

	public void AllBuild(string[] te)
	{
		base.InitBuild();
		this.partsCount = te.Length;
		if (base.selectCollider != null)
		{
			this.partW = base.selectCollider.width + this.horizontalMargin;
			float num = (float)this.partsCount * this.partW - this.horizontalMargin + this.horizontalBorder * 2f;
			float num2 = num / 2f - this.horizontalBorder - base.selectCollider.width / 2f;
			float y = 0f;
			for (int i = te.Length - 1; i >= 0; i--)
			{
				string text = te[i];
				if (text.StartsWith("n_"))
				{
					text = text.Remove(0, 2);
				}
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsGashaBanner component = gameObject.GetComponent<GUIListPartsGashaBanner>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(num2, y, -5f));
					component.nameId = text;
					component.selectPanelGasha = this;
				}
				num2 -= this.partW;
				gameObject.SetActive(true);
			}
			base.width = num;
			base.InitMinMaxLocation(true);
			base.FreeScrollMode = false;
			if (this.partsCount > 1)
			{
				base.EnableEternalScroll = true;
			}
			else
			{
				base.EnableEternalScroll = false;
			}
		}
	}

	public void ReleaseTex()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIListPartsGashaBanner guilistPartsGashaBanner = (GUIListPartsGashaBanner)this.partObjs[i];
			guilistPartsGashaBanner.ReleaseTex();
		}
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		base.OnTouchMoved(touch, pos);
		this.timeCoun = this.limitOrigin;
	}
}
