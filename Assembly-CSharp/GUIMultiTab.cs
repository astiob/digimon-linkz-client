using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIMultiTab : UtilForCMD
{
	public float arrowOffset = 32f;

	[SerializeField]
	private Color onColorBase = new Color(0.156862751f, 0.156862751f, 0.156862751f, 1f);

	[SerializeField]
	private Color offColorBase = new Color(0f, 0f, 0.392156869f, 1f);

	[SerializeField]
	private Color onColorLabel = new Color(0.627451f, 0.627451f, 0.627451f, 1f);

	[SerializeField]
	private Color offColorLabel = new Color(1f, 1f, 1f, 1f);

	[SerializeField]
	private Color onColorArrow = new Color(0.627451f, 0.627451f, 0.627451f, 1f);

	[SerializeField]
	private Color offColorArrow = new Color(0f, 0f, 0f, 0f);

	[SerializeField]
	private List<MultiTabData> MultiTabDataList;

	private int curTabIdx = -1;

	public int GetCurentTabIdx()
	{
		return this.curTabIdx;
	}

	protected new virtual void Awake()
	{
		for (int i = 0; i < this.MultiTabDataList.Count; i++)
		{
			this.MultiTabDataList[i].idx = i + 1;
			this.MultiTabDataList[i].name = this.MultiTabDataList[i].goTab.name;
			this.MultiTabDataList[i].collider = this.MultiTabDataList[i].goTab.GetComponent<GUICollider>();
			int mm = i;
			this.MultiTabDataList[i].collider.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				int idx = this.MultiTabDataList[mm].idx;
				if (flag)
				{
					this.OnTabTouchEnded(idx, true);
				}
			};
		}
		base.Awake();
	}

	public void SetOnOffColor(Color _onColor, Color _offColor)
	{
	}

	public virtual void SetFocus(int idx)
	{
		this.OnTabTouchEnded(idx, false);
	}

	public virtual void OnTabTouchEnded(int idx, bool act = true)
	{
		if (idx != this.curTabIdx)
		{
			this.curTabIdx = idx;
			for (int i = 0; i < this.MultiTabDataList.Count; i++)
			{
				MultiTabData multiTabData = this.MultiTabDataList[i];
				if (multiTabData.idx == idx)
				{
					if (act && multiTabData.callbackAction != null)
					{
						multiTabData.callbackAction(multiTabData.idx);
					}
					this.SetTab(multiTabData, true);
				}
				else
				{
					this.SetTab(multiTabData, false);
				}
			}
		}
	}

	private void SetTab(MultiTabData multiTabData, bool isOn)
	{
		Color color = (!isOn) ? this.offColorLabel : this.onColorLabel;
		Color color2 = (!isOn) ? this.offColorArrow : this.onColorArrow;
		Color color3 = (!isOn) ? this.offColorBase : this.onColorBase;
		if (multiTabData.spTab != null)
		{
			multiTabData.spTab.color = color3;
		}
		if (multiTabData.tabLabel != null)
		{
			multiTabData.tabLabel.color = color;
			Vector2 vector = Vector2.zero;
			if (multiTabData.labelText != string.Empty)
			{
				multiTabData.tabLabel.text = multiTabData.labelText;
				vector = multiTabData.tabLabel.printedSize;
				if (multiTabData.spLeftArrow != null)
				{
					multiTabData.spLeftArrow.color = color2;
					Vector3 localPosition = multiTabData.spLeftArrow.transform.localPosition;
					localPosition.x = -(vector.x / 2f + this.arrowOffset);
					multiTabData.spLeftArrow.transform.localPosition = localPosition;
				}
				if (multiTabData.spRightArrow != null)
				{
					multiTabData.spRightArrow.color = color2;
					Vector3 localPosition2 = multiTabData.spRightArrow.transform.localPosition;
					localPosition2.x = vector.x / 2f + this.arrowOffset;
					multiTabData.spRightArrow.transform.localPosition = localPosition2;
				}
			}
		}
		Transform transform = multiTabData.goTab.transform;
		Vector3 localPosition3 = transform.localPosition;
		localPosition3.z = 2f;
		transform.localPosition = localPosition3;
	}

	public virtual void InitMultiTab(List<Action<int>> actions, List<string> tabLabelTexts = null)
	{
		for (int i = 0; i < this.MultiTabDataList.Count; i++)
		{
			MultiTabData multiTabData = this.MultiTabDataList[i];
			multiTabData.callbackAction = actions[i];
			if (tabLabelTexts != null)
			{
				multiTabData.labelText = tabLabelTexts[i];
			}
		}
	}

	public virtual void HideMultiTab(int idx)
	{
		this.MultiTabDataList[idx].goTab.SetActive(false);
	}

	public virtual void HideMultiTabByName(string name)
	{
		for (int i = 0; i < this.MultiTabDataList.Count; i++)
		{
			if (this.MultiTabDataList[i].goTab.name == name)
			{
				this.MultiTabDataList[i].goTab.SetActive(false);
			}
		}
	}

	public virtual void SetActiveAlertIcon(params bool[] isActive)
	{
		for (int i = 0; i < this.MultiTabDataList.Count; i++)
		{
			this.MultiTabDataList[i].goAlertIcon.SetActive(isActive[i]);
		}
	}

	public override void SetParamToCMD()
	{
		CMD cmd = base.FindParentCMD();
		if (cmd != null)
		{
			cmd.MultiTab = this;
			base.DisableCMD_CallBack(this.myTransform);
		}
	}
}
