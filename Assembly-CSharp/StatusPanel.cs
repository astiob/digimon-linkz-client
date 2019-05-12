using System;
using UnityEngine;

public sealed class StatusPanel : MonoBehaviour
{
	[Header("左下の表示切替のGameObject")]
	[SerializeField]
	private GameObject switchButton;

	[Header("ボタンのラベル")]
	[SerializeField]
	private UILabel switchButtonLabel;

	[SerializeField]
	public GameObject[] pageList;

	private UISprite switchButtonSprite;

	private GUICollider switchButtonCollider;

	private GUICollider myCollider;

	private int selectPage;

	private void Awake()
	{
		if (null != this.switchButton)
		{
			this.switchButtonSprite = this.switchButton.GetComponent<UISprite>();
			this.switchButtonCollider = this.switchButton.GetComponent<GUICollider>();
		}
		this.myCollider = base.GetComponent<GUICollider>();
	}

	public void SetEnable(bool isEnable)
	{
		if (this.switchButtonLabel != null)
		{
			if (isEnable)
			{
				this.switchButtonLabel.color = Color.white;
			}
			else
			{
				this.switchButtonLabel.color = ConstValue.DEACTIVE_BUTTON_LABEL;
			}
		}
		if (null != this.switchButtonSprite)
		{
			if (isEnable)
			{
				this.switchButtonSprite.spriteName = "Common02_Btn_BaseON";
			}
			else
			{
				this.switchButtonSprite.spriteName = "Common02_Btn_BaseG";
			}
		}
		if (null != this.switchButtonCollider)
		{
			this.switchButtonCollider.activeCollider = isEnable;
		}
		this.myCollider.activeCollider = isEnable;
	}

	public void InitUI()
	{
		this.SetEnable(true);
		this.ResetUI();
	}

	public void ResetUI()
	{
		this.selectPage = -1;
		this.SetHideUI();
	}

	private void SetHideUI()
	{
		for (int i = 0; i < this.pageList.Length; i++)
		{
			if (this.pageList[i].activeSelf)
			{
				this.pageList[i].SetActive(false);
			}
		}
	}

	public void SetPage(int pageNo)
	{
		if (0 <= this.selectPage && this.selectPage < this.pageList.Length)
		{
			this.pageList[this.selectPage].SetActive(false);
		}
		this.selectPage = pageNo;
		if (this.selectPage < 0 || this.pageList.Length <= this.selectPage)
		{
			this.ResetUI();
		}
		else if (!this.pageList[this.selectPage].activeSelf)
		{
			this.pageList[this.selectPage].SetActive(true);
		}
	}

	public void SetNextPage()
	{
		if (0 <= this.selectPage && this.selectPage < this.pageList.Length && this.pageList[this.selectPage].activeSelf)
		{
			this.pageList[this.selectPage].SetActive(false);
		}
		this.selectPage++;
		if (this.pageList.Length <= this.selectPage)
		{
			this.ResetUI();
		}
		else if (!this.pageList[this.selectPage].activeSelf)
		{
			this.pageList[this.selectPage].SetActive(true);
		}
	}

	public int GetPageNo()
	{
		return this.selectPage;
	}
}
