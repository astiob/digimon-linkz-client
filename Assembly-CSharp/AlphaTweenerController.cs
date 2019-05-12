using System;
using System.Collections.Generic;

public class AlphaTweenerController : AlphaTweener
{
	private List<UIWidget> uiList_A;

	private List<UIWidget> uiList_B;

	private void Awake()
	{
		this.uiList_A = new List<UIWidget>();
		this.uiList_B = new List<UIWidget>();
	}

	public void ClearWidgetList()
	{
		this.uiList_A = new List<UIWidget>();
		this.uiList_B = new List<UIWidget>();
	}

	public void AddPairWidget(UIWidget wdgA, UIWidget wdgB)
	{
		this.uiList_A.Add(wdgA);
		this.uiList_B.Add(wdgB);
	}

	private void Update()
	{
		float alphaValue = base.GetAlphaValue();
		for (int i = 0; i < this.uiList_A.Count; i++)
		{
			if (alphaValue > 0f)
			{
				this.uiList_A[i].alpha = alphaValue;
				this.uiList_B[i].alpha = 0f;
			}
			else
			{
				this.uiList_A[i].alpha = 0f;
				this.uiList_B[i].alpha = -alphaValue;
			}
		}
	}
}
