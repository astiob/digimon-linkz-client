using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_ModalSort : CMD
{
	[SerializeField]
	private List<GameObject> goBtnSortList;

	private List<GUICollider> clBtnSortList;

	private List<UISprite> spBtnSortList;

	private List<UILabel> lbBtnSortList;

	[SerializeField]
	private List<GameObject> goBtnSelectList;

	private List<GUICollider> clBtnSelectList;

	private List<UISprite> spBtnSelectList;

	private List<UILabel> lbBtnSelectList;

	[SerializeField]
	private GUICollider clBtnSelectReset;

	[SerializeField]
	private UISprite spBtnSelectReset;

	[SerializeField]
	private UILabel lbBtnSelectReset;

	[SerializeField]
	private GameObject goSORT_ROOT;

	[SerializeField]
	private GameObject goSELECT_ROOT;

	[SerializeField]
	private GUICollider clBtnChange;

	[SerializeField]
	private GUICollider clBtnSelect;

	[SerializeField]
	private UILabel lbBtnSelect;

	[SerializeField]
	private GUICollider clBtnShou;

	[SerializeField]
	private UISprite spBtnShou;

	[SerializeField]
	private UILabel lbBtnShou;

	[SerializeField]
	private GUICollider clBtnKou;

	[SerializeField]
	private UISprite spBtnKou;

	[SerializeField]
	private UILabel lbBtnKou;

	[SerializeField]
	private GUICollider clBtnClose;

	[SerializeField]
	private UILabel lbSelectNum;

	private static bool isSort = true;

	private MonsterDataMng.SORT_TYPE nowSortType_bak;

	private MonsterDataMng.SORT_DIR nowSortDir_bak;

	private MonsterDataMng.SELECTION_TYPE nowSelectionType_bak;

	[SerializeField]
	private UILabel ngTX_SORT;

	public bool IsEvolvePage { get; set; }

	protected override void Awake()
	{
		this.nowSortType_bak = MonsterDataMng.Instance().NowSortType;
		this.nowSortDir_bak = MonsterDataMng.Instance().NowSortDir;
		this.nowSelectionType_bak = MonsterDataMng.Instance().NowSelectionType;
		base.Awake();
		this.SetupSortBtn();
		this.SetupSelectBtn();
		this.SetupUtilBtn();
		this.FlipShouKouBtn(MonsterDataMng.SORT_DIR.NONE);
		this.FlipSelectBtn(false);
		this.SetSortName();
		this.SetSelectNum();
	}

	private void SetupSortBtn()
	{
		this.clBtnSortList = new List<GUICollider>();
		this.spBtnSortList = new List<UISprite>();
		this.lbBtnSortList = new List<UILabel>();
		for (int i = 0; i < this.goBtnSortList.Count; i++)
		{
			GUICollider component = this.goBtnSortList[i].GetComponent<GUICollider>();
			this.clBtnSortList.Add(component);
			UISprite component2 = this.goBtnSortList[i].GetComponent<UISprite>();
			this.spBtnSortList.Add(component2);
			foreach (object obj in this.goBtnSortList[i].transform)
			{
				Transform transform = (Transform)obj;
				UILabel component3 = transform.gameObject.GetComponent<UILabel>();
				this.lbBtnSortList.Add(component3);
			}
			switch (i)
			{
			case 0:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(0);
				};
				break;
			case 1:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(1);
				};
				break;
			case 2:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(2);
				};
				break;
			case 3:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(3);
				};
				break;
			case 4:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(4);
				};
				break;
			case 5:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(5);
				};
				break;
			case 6:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(6);
				};
				break;
			case 7:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(7);
				};
				break;
			case 8:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(8);
				};
				break;
			case 9:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(9);
				};
				break;
			case 10:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(10);
				};
				break;
			case 11:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedBtn(11);
				};
				break;
			}
		}
		this.SetupSortBtnCol(MonsterDataMng.Instance().NowSortType);
	}

	private void SetupSelectBtn()
	{
		this.clBtnSelectList = new List<GUICollider>();
		this.spBtnSelectList = new List<UISprite>();
		this.lbBtnSelectList = new List<UILabel>();
		for (int i = 0; i < this.goBtnSelectList.Count; i++)
		{
			GUICollider component = this.goBtnSelectList[i].GetComponent<GUICollider>();
			this.clBtnSelectList.Add(component);
			UISprite component2 = this.goBtnSelectList[i].GetComponent<UISprite>();
			this.spBtnSelectList.Add(component2);
			foreach (object obj in this.goBtnSelectList[i].transform)
			{
				Transform transform = (Transform)obj;
				UILabel component3 = transform.gameObject.GetComponent<UILabel>();
				this.lbBtnSelectList.Add(component3);
			}
			switch (i)
			{
			case 0:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-2);
					}
					else
					{
						MonsterDataMng.Instance().NowSelectionType |= MonsterDataMng.SELECTION_TYPE.LEADER_SKILL;
					}
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-17);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 1:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.ACTIVE_SUCCESS) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-3);
					}
					else
					{
						MonsterDataMng.Instance().NowSelectionType |= MonsterDataMng.SELECTION_TYPE.ACTIVE_SUCCESS;
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 2:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.PASSIV_SUCCESS) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-5);
					}
					else
					{
						MonsterDataMng.Instance().NowSelectionType |= MonsterDataMng.SELECTION_TYPE.PASSIV_SUCCESS;
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 3:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-9);
					}
					else
					{
						MonsterDataMng.Instance().NowSelectionType |= MonsterDataMng.SELECTION_TYPE.MEDAL;
					}
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-33);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 4:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-17);
					}
					else
					{
						MonsterDataMng.Instance().NowSelectionType |= MonsterDataMng.SELECTION_TYPE.NO_LEADER_SKILL;
					}
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-2);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 5:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-33);
					}
					else
					{
						MonsterDataMng.Instance().NowSelectionType |= MonsterDataMng.SELECTION_TYPE.NO_MEDAL;
					}
					if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
					{
						MonsterDataMng.Instance().NowSelectionType &= (MonsterDataMng.SELECTION_TYPE)(-9);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			}
		}
		this.clBtnSelectReset.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			MonsterDataMng.Instance().NowSelectionType = MonsterDataMng.SELECTION_TYPE.NONE;
			this.SetSelectNum();
			this.DispSelectBtnList();
		};
		this.DispSelectBtnList();
	}

	private void SetupUtilBtn()
	{
		this.clBtnChange.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			List<MonsterData> list = MonsterDataMng.Instance().GetSelectMonsterDataList();
			list = MonsterDataMng.Instance().SortMDList(list, this.IsEvolvePage);
			if (GUISelectPanelMonsterIcon.instance != null)
			{
				GUISelectPanelMonsterIcon.instance.ReAllBuild(list);
			}
			this.ClosePanel(true);
		};
		this.clBtnSelect.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.FlipSelectBtn(true);
		};
		this.clBtnShou.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.FlipShouKouBtn(MonsterDataMng.SORT_DIR.LOWER_2_UPPER);
		};
		this.clBtnKou.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.FlipShouKouBtn(MonsterDataMng.SORT_DIR.UPPER_2_LOWER);
		};
		this.clBtnClose.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			MonsterDataMng.Instance().NowSortType = this.nowSortType_bak;
			MonsterDataMng.Instance().NowSortDir = this.nowSortDir_bak;
			MonsterDataMng.Instance().NowSelectionType = this.nowSelectionType_bak;
		};
	}

	private void SetSelectNum()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetSelectMonsterDataList();
		int count = list.Count;
		list = MonsterDataMng.Instance().SelectionMDList(list);
		int count2 = list.Count;
		this.lbSelectNum.text = string.Format(StringMaster.GetString("SystemFraction"), count2.ToString(), count.ToString());
	}

	private void FlipShouKouBtn(MonsterDataMng.SORT_DIR dir)
	{
		if (dir != MonsterDataMng.SORT_DIR.NONE)
		{
			MonsterDataMng.Instance().NowSortDir = dir;
		}
		if (MonsterDataMng.Instance().NowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			this.spBtnKou.spriteName = "Common02_Btn_SupportRed";
			this.lbBtnKou.color = Color.white;
			this.spBtnShou.spriteName = "Common02_Btn_SupportWhite";
			this.lbBtnShou.color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
		}
		else if (MonsterDataMng.Instance().NowSortDir == MonsterDataMng.SORT_DIR.LOWER_2_UPPER)
		{
			this.spBtnShou.spriteName = "Common02_Btn_SupportRed";
			this.lbBtnShou.color = Color.white;
			this.spBtnKou.spriteName = "Common02_Btn_SupportWhite";
			this.lbBtnKou.color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
		}
	}

	private void FlipSelectBtn(bool flg = false)
	{
		if (flg)
		{
			if (!CMD_ModalSort.isSort)
			{
				CMD_ModalSort.isSort = true;
			}
			else
			{
				CMD_ModalSort.isSort = false;
			}
		}
		if (CMD_ModalSort.isSort)
		{
			this.goSORT_ROOT.SetActive(true);
			this.goSELECT_ROOT.SetActive(false);
			this.lbBtnSelect.text = StringMaster.GetString("SystemRefine");
		}
		else
		{
			this.goSORT_ROOT.SetActive(false);
			this.goSELECT_ROOT.SetActive(true);
			this.lbBtnSelect.text = StringMaster.GetString("ChipSortModal-01");
		}
	}

	private void DispSelectBtnList()
	{
		for (int i = 0; i < this.spBtnSelectList.Count; i++)
		{
			this.spBtnSelectList[i].spriteName = "Common02_Btn_SupportWhite";
			this.lbBtnSelectList[i].color = Color.black;
		}
		if (MonsterDataMng.Instance().NowSelectionType == MonsterDataMng.SELECTION_TYPE.NONE)
		{
			this.spBtnSelectReset.spriteName = "Common02_Btn_BaseG";
			this.lbBtnSelectReset.color = Color.white;
			this.clBtnSelectReset.activeCollider = false;
		}
		else
		{
			if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
			{
				this.spBtnSelectList[0].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[0].color = Color.white;
			}
			if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.ACTIVE_SUCCESS) > MonsterDataMng.SELECTION_TYPE.NONE)
			{
				this.spBtnSelectList[1].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[1].color = Color.white;
			}
			if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.PASSIV_SUCCESS) > MonsterDataMng.SELECTION_TYPE.NONE)
			{
				this.spBtnSelectList[2].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[2].color = Color.white;
			}
			if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
			{
				this.spBtnSelectList[3].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[3].color = Color.white;
			}
			if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
			{
				this.spBtnSelectList[4].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[4].color = Color.white;
			}
			if ((MonsterDataMng.Instance().NowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
			{
				this.spBtnSelectList[5].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[5].color = Color.white;
			}
			this.spBtnSelectReset.spriteName = "Common02_Btn_BaseON";
			this.lbBtnSelectReset.color = Color.white;
			this.clBtnSelectReset.activeCollider = true;
		}
	}

	private void OnClickedBtn(int idx)
	{
		switch (idx)
		{
		case 0:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.DATE;
			break;
		case 1:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.RARE;
			break;
		case 2:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.LEVEL;
			break;
		case 3:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.HP;
			break;
		case 4:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.ATK;
			break;
		case 5:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.DEF;
			break;
		case 6:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.S_ATK;
			break;
		case 7:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.S_DEF;
			break;
		case 8:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.SPD;
			break;
		case 9:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.LUCK;
			break;
		case 10:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.GRADE;
			break;
		case 11:
			MonsterDataMng.Instance().NowSortType = MonsterDataMng.SORT_TYPE.TRIBE;
			break;
		}
		this.SetupSortBtnCol(MonsterDataMng.Instance().NowSortType);
		this.SetSortName();
	}

	private void SetupSortBtnCol(MonsterDataMng.SORT_TYPE type)
	{
		for (int i = 0; i < this.goBtnSortList.Count; i++)
		{
			if (i == (int)type)
			{
				this.spBtnSortList[i].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSortList[i].color = Color.white;
			}
			else
			{
				this.spBtnSortList[i].spriteName = "Common02_Btn_SupportWhite";
				this.lbBtnSortList[i].color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
			}
		}
	}

	private void SetSortName()
	{
		this.ngTX_SORT.text = string.Format(StringMaster.GetString("Sort-01"), MonsterDataMng.Instance().GetSortName());
	}
}
