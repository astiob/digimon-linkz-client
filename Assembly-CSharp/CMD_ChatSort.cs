using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_ChatSort : CMD
{
	[SerializeField]
	private List<GameObject> goRefineBtnList;

	private List<GUICollider> clRefineBtnList;

	private List<UISprite> spRefineBtnList;

	private List<UILabel> lbRefineBtnList;

	[SerializeField]
	private List<GameObject> goSortBtnList;

	private List<GUICollider> clSortBtnList;

	private List<UISprite> spSortBtnList;

	private List<UILabel> lbSortBtnList;

	[SerializeField]
	private UILabel ModalTitle;

	[SerializeField]
	private UILabel RefineTitle;

	[SerializeField]
	private UILabel SortTitle;

	[SerializeField]
	private GameObject submitButton;

	[SerializeField]
	private GameObject submitButtonGray;

	[SerializeField]
	private UILabel submitButtonText;

	[SerializeField]
	private BoxCollider submitButtonCollider;

	private bool isInitOpen = true;

	protected override void Awake()
	{
		base.Awake();
		this.OnClickedRefineBtn(0);
		this.SetupRefineBtn();
		this.SetupSortBtn();
		this.SetupLabel();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void SetupLabel()
	{
		this.ModalTitle.text = StringMaster.GetString("SystemSortButton");
		this.RefineTitle.text = StringMaster.GetString("SystemRefine");
		this.SortTitle.text = StringMaster.GetString("SystemSorting");
		this.submitButtonText.text = StringMaster.GetString("SystemButtonDecision");
	}

	private void SetupRefineBtn()
	{
		this.clRefineBtnList = new List<GUICollider>();
		this.spRefineBtnList = new List<UISprite>();
		this.lbRefineBtnList = new List<UILabel>();
		for (int i = 0; i < this.goRefineBtnList.Count; i++)
		{
			GUICollider component = this.goRefineBtnList[i].GetComponent<GUICollider>();
			UISprite component2 = this.goRefineBtnList[i].GetComponent<UISprite>();
			this.clRefineBtnList.Add(component);
			this.spRefineBtnList.Add(component2);
			foreach (object obj in this.goRefineBtnList[i].transform)
			{
				Transform transform = (Transform)obj;
				UILabel component3 = transform.gameObject.GetComponent<UILabel>();
				if (i < 4)
				{
					switch (i + 1)
					{
					case 1:
						component3.text = StringMaster.GetString("ChatCategory-04");
						break;
					case 2:
						component3.text = StringMaster.GetString("ChatCategory-05");
						break;
					case 3:
						component3.text = StringMaster.GetString("ChatCategory-03");
						break;
					case 4:
						component3.text = StringMaster.GetString("ChatCategory-06");
						break;
					}
				}
				else
				{
					int num = i - 3;
					if (num != 1)
					{
						if (num == 2)
						{
							component3.text = StringMaster.GetString("ChatCategory-01");
						}
					}
					else
					{
						component3.text = StringMaster.GetString("ChatCategory-02");
					}
				}
				this.lbRefineBtnList.Add(component3);
			}
			this.SetupRefineBtnColor(i);
			switch (i)
			{
			case 0:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRefineBtn(0);
				};
				break;
			case 1:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRefineBtn(1);
				};
				break;
			case 2:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRefineBtn(2);
				};
				break;
			case 3:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRefineBtn(3);
				};
				break;
			case 4:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRefineBtn(4);
				};
				break;
			case 5:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRefineBtn(5);
				};
				break;
			}
		}
	}

	private void SetupSortBtn()
	{
		this.clSortBtnList = new List<GUICollider>();
		this.spSortBtnList = new List<UISprite>();
		this.lbSortBtnList = new List<UILabel>();
		for (int i = 0; i < this.goSortBtnList.Count; i++)
		{
			GUICollider component = this.goSortBtnList[i].GetComponent<GUICollider>();
			UISprite component2 = this.goSortBtnList[i].GetComponent<UISprite>();
			this.clSortBtnList.Add(component);
			this.spSortBtnList.Add(component2);
			int num;
			foreach (object obj in this.goSortBtnList[i].transform)
			{
				Transform transform = (Transform)obj;
				UILabel component3 = transform.gameObject.GetComponent<UILabel>();
				num = i + 1;
				if (num != 1)
				{
					if (num == 2)
					{
						component3.text = StringMaster.GetString("ChatSortFew");
					}
				}
				else
				{
					component3.text = StringMaster.GetString("ChatSortMany");
				}
				this.lbSortBtnList.Add(component3);
			}
			num = i;
			if (num != 0)
			{
				if (num == 1)
				{
					component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedSortBtn(1);
					};
				}
			}
			else
			{
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedSortBtn(0);
				};
			}
		}
		CMD_ChatTop.SortStatusList selectedSortStatus = CMD_ChatTop.selectedSortStatus;
		if (selectedSortStatus != CMD_ChatTop.SortStatusList.MEMBER_DESC)
		{
			if (selectedSortStatus == CMD_ChatTop.SortStatusList.MEMBER_ASK)
			{
				this.SetupSortBtnColor(1);
			}
		}
		else
		{
			this.SetupSortBtnColor(0);
		}
	}

	private void OnClickedRefineBtn(int idx)
	{
		bool flag = false;
		bool flag2 = false;
		if (this.isInitOpen)
		{
			this.isInitOpen = false;
		}
		else
		{
			CMD_ChatTop.selectedRefineStatusList[idx] = !CMD_ChatTop.selectedRefineStatusList[idx];
			this.SetupRefineBtnColor(idx);
		}
		for (int i = 0; i < CMD_ChatTop.selectedRefineStatusList.Length; i++)
		{
			if (i < 4)
			{
				if (CMD_ChatTop.selectedRefineStatusList[i])
				{
					flag = true;
				}
			}
			else if (CMD_ChatTop.selectedRefineStatusList[i])
			{
				flag2 = true;
			}
		}
		this.SetGraySubmitButton(!flag || !flag2);
	}

	private void OnClickedSortBtn(int idx)
	{
		if (idx != 0)
		{
			if (idx == 1)
			{
				CMD_ChatTop.selectedSortStatus = CMD_ChatTop.SortStatusList.MEMBER_ASK;
			}
		}
		else
		{
			CMD_ChatTop.selectedSortStatus = CMD_ChatTop.SortStatusList.MEMBER_DESC;
		}
		this.SetupSortBtnColor(idx);
	}

	private void SetupRefineBtnColor(int idx)
	{
		if (CMD_ChatTop.selectedRefineStatusList[idx])
		{
			this.spRefineBtnList[idx].spriteName = "Common02_Btn_SupportRed";
			this.lbRefineBtnList[idx].color = Color.white;
		}
		else
		{
			this.spRefineBtnList[idx].spriteName = "Common02_Btn_SupportWhite";
			this.lbRefineBtnList[idx].color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
		}
	}

	private void SetupSortBtnColor(int idx)
	{
		for (int i = 0; i < this.goSortBtnList.Count; i++)
		{
			if (i == idx)
			{
				this.spSortBtnList[i].spriteName = "Common02_Btn_SupportRed";
				this.lbSortBtnList[i].color = Color.white;
			}
			else
			{
				this.spSortBtnList[i].spriteName = "Common02_Btn_SupportWhite";
				this.lbSortBtnList[i].color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
			}
		}
	}

	private void SetGraySubmitButton(bool isGray = true)
	{
		if (isGray)
		{
			this.submitButton.SetActive(false);
			this.submitButtonGray.SetActive(true);
		}
		else
		{
			this.submitButton.SetActive(true);
			this.submitButtonGray.SetActive(false);
		}
		this.submitButtonCollider.enabled = !isGray;
	}

	private void ClickDecideBtn()
	{
		CMD_ChatTop.instance.isGetChatGroupListMax = false;
		CMD_ChatTop.instance.SetChatRecruitGroupList(1);
		CMD_ChatTop.instance.isChatPaging = true;
		this.ClosePanel(true);
	}
}
