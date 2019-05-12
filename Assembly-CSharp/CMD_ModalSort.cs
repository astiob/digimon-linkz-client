using Master;
using Monster;
using System;
using System.Collections;
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

	[SerializeField]
	private UILabel ngTX_SORT;

	private MonsterSortType sortType;

	private MonsterSortOrder sortOrder;

	private MonsterDetailedFilterType filterType;

	private Action onChangeSetting;

	private List<MonsterData> targetMonsterList;

	private static bool isSort = true;

	protected override void Awake()
	{
		this.sortType = CMD_BaseSelect.IconSortType;
		this.sortOrder = CMD_BaseSelect.IconSortOrder;
		this.filterType = CMD_BaseSelect.IconFilterType;
		base.Awake();
	}

	public void Initialize()
	{
		this.SetupSortBtn();
		this.SetupSelectBtn();
		this.SetupUtilBtn();
		this.FlipShouKouBtn();
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
			IEnumerator enumerator = this.goBtnSortList[i].transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UILabel component3 = transform.gameObject.GetComponent<UILabel>();
					this.lbBtnSortList.Add(component3);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
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
		this.SetupSortBtnCol(this.sortType);
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
			IEnumerator enumerator = this.goBtnSelectList[i].transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UILabel component3 = transform.gameObject.GetComponent<UILabel>();
					this.lbBtnSelectList.Add(component3);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			switch (i)
			{
			case 0:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((this.filterType & MonsterDetailedFilterType.LEADER_SKILL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-2);
					}
					else
					{
						this.filterType |= MonsterDetailedFilterType.LEADER_SKILL;
					}
					if ((this.filterType & MonsterDetailedFilterType.NO_LEADER_SKILL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-17);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 1:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((this.filterType & MonsterDetailedFilterType.ACTIVE_SUCCESS) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-3);
					}
					else
					{
						this.filterType |= MonsterDetailedFilterType.ACTIVE_SUCCESS;
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 2:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((this.filterType & MonsterDetailedFilterType.PASSIV_SUCCESS) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-5);
					}
					else
					{
						this.filterType |= MonsterDetailedFilterType.PASSIV_SUCCESS;
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 3:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((this.filterType & MonsterDetailedFilterType.MEDAL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-9);
					}
					else
					{
						this.filterType |= MonsterDetailedFilterType.MEDAL;
					}
					if ((this.filterType & MonsterDetailedFilterType.NO_MEDAL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-33);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 4:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((this.filterType & MonsterDetailedFilterType.NO_LEADER_SKILL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-17);
					}
					else
					{
						this.filterType |= MonsterDetailedFilterType.NO_LEADER_SKILL;
					}
					if ((this.filterType & MonsterDetailedFilterType.LEADER_SKILL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-2);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			case 5:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if ((this.filterType & MonsterDetailedFilterType.NO_MEDAL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-33);
					}
					else
					{
						this.filterType |= MonsterDetailedFilterType.NO_MEDAL;
					}
					if ((this.filterType & MonsterDetailedFilterType.MEDAL) > MonsterDetailedFilterType.NONE)
					{
						this.filterType &= (MonsterDetailedFilterType)(-9);
					}
					this.SetSelectNum();
					this.DispSelectBtnList();
				};
				break;
			}
		}
		this.clBtnSelectReset.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.filterType = MonsterDetailedFilterType.NONE;
			this.SetSelectNum();
			this.DispSelectBtnList();
		};
		this.DispSelectBtnList();
	}

	private void SetupUtilBtn()
	{
		this.clBtnChange.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			bool flag2 = false;
			if (CMD_BaseSelect.IconSortType != this.sortType || CMD_BaseSelect.IconSortOrder != this.sortOrder || CMD_BaseSelect.IconFilterType != this.filterType)
			{
				flag2 = true;
			}
			CMD_BaseSelect.IconSortType = this.sortType;
			CMD_BaseSelect.IconSortOrder = this.sortOrder;
			CMD_BaseSelect.IconFilterType = this.filterType;
			if (flag2 && this.onChangeSetting != null)
			{
				this.onChangeSetting();
				CMD_BaseSelect.SaveSetting();
			}
			this.ClosePanel(true);
		};
		this.clBtnSelect.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.FlipSelectBtn(true);
		};
		this.clBtnShou.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.sortOrder = MonsterSortOrder.ASC;
			this.FlipShouKouBtn();
		};
		this.clBtnKou.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			this.sortOrder = MonsterSortOrder.DESC;
			this.FlipShouKouBtn();
		};
	}

	private void SetSelectNum()
	{
		int count = this.targetMonsterList.Count;
		List<MonsterData> list = MonsterFilter.DetailedFilter(this.targetMonsterList, this.filterType);
		int count2 = list.Count;
		this.lbSelectNum.text = string.Format(StringMaster.GetString("SystemFraction"), count2.ToString(), count.ToString());
	}

	private void FlipShouKouBtn()
	{
		if (this.sortOrder == MonsterSortOrder.DESC)
		{
			this.spBtnKou.spriteName = "Common02_Btn_SupportRed";
			this.lbBtnKou.color = Color.white;
			this.spBtnShou.spriteName = "Common02_Btn_SupportWhite";
			this.lbBtnShou.color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
		}
		else if (this.sortOrder == MonsterSortOrder.ASC)
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
		if (this.filterType == MonsterDetailedFilterType.NONE)
		{
			this.spBtnSelectReset.spriteName = "Common02_Btn_BaseG";
			this.lbBtnSelectReset.color = Color.white;
			this.clBtnSelectReset.activeCollider = false;
		}
		else
		{
			if ((this.filterType & MonsterDetailedFilterType.LEADER_SKILL) > MonsterDetailedFilterType.NONE)
			{
				this.spBtnSelectList[0].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[0].color = Color.white;
			}
			if ((this.filterType & MonsterDetailedFilterType.ACTIVE_SUCCESS) > MonsterDetailedFilterType.NONE)
			{
				this.spBtnSelectList[1].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[1].color = Color.white;
			}
			if ((this.filterType & MonsterDetailedFilterType.PASSIV_SUCCESS) > MonsterDetailedFilterType.NONE)
			{
				this.spBtnSelectList[2].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[2].color = Color.white;
			}
			if ((this.filterType & MonsterDetailedFilterType.MEDAL) > MonsterDetailedFilterType.NONE)
			{
				this.spBtnSelectList[3].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[3].color = Color.white;
			}
			if ((this.filterType & MonsterDetailedFilterType.NO_LEADER_SKILL) > MonsterDetailedFilterType.NONE)
			{
				this.spBtnSelectList[4].spriteName = "Common02_Btn_SupportRed";
				this.lbBtnSelectList[4].color = Color.white;
			}
			if ((this.filterType & MonsterDetailedFilterType.NO_MEDAL) > MonsterDetailedFilterType.NONE)
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
			this.sortType = MonsterSortType.DATE;
			break;
		case 1:
			this.sortType = MonsterSortType.AROUSAL;
			break;
		case 2:
			this.sortType = MonsterSortType.LEVEL;
			break;
		case 3:
			this.sortType = MonsterSortType.HP;
			break;
		case 4:
			this.sortType = MonsterSortType.ATK;
			break;
		case 5:
			this.sortType = MonsterSortType.DEF;
			break;
		case 6:
			this.sortType = MonsterSortType.S_ATK;
			break;
		case 7:
			this.sortType = MonsterSortType.S_DEF;
			break;
		case 8:
			this.sortType = MonsterSortType.SPD;
			break;
		case 9:
			this.sortType = MonsterSortType.LUCK;
			break;
		case 10:
			this.sortType = MonsterSortType.GROW_STEP;
			break;
		case 11:
			this.sortType = MonsterSortType.TRIBE;
			break;
		}
		this.SetupSortBtnCol(this.sortType);
		this.SetSortName();
	}

	private void SetupSortBtnCol(MonsterSortType type)
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
		this.ngTX_SORT.text = string.Format(StringMaster.GetString("Sort-01"), CMD_ModalSort.GetSortName(this.sortType));
	}

	public static string GetSortName(MonsterSortType type)
	{
		string result = string.Empty;
		switch (type)
		{
		case MonsterSortType.DATE:
			result = StringMaster.GetString("Sort-13");
			break;
		case MonsterSortType.AROUSAL:
			result = StringMaster.GetString("Sort-10");
			break;
		case MonsterSortType.LEVEL:
			result = StringMaster.GetString("Sort-05");
			break;
		case MonsterSortType.HP:
			result = StringMaster.GetString("Sort-04");
			break;
		case MonsterSortType.ATK:
			result = StringMaster.GetString("Sort-02");
			break;
		case MonsterSortType.DEF:
			result = StringMaster.GetString("Sort-03");
			break;
		case MonsterSortType.S_ATK:
			result = StringMaster.GetString("Sort-06");
			break;
		case MonsterSortType.S_DEF:
			result = StringMaster.GetString("Sort-07");
			break;
		case MonsterSortType.SPD:
			result = StringMaster.GetString("Sort-08");
			break;
		case MonsterSortType.LUCK:
			result = StringMaster.GetString("Sort-09");
			break;
		case MonsterSortType.GROW_STEP:
			result = StringMaster.GetString("Sort-12");
			break;
		case MonsterSortType.TRIBE:
			result = StringMaster.GetString("Sort-11");
			break;
		case MonsterSortType.FRIENDSHIP:
			result = StringMaster.GetString("Sort-14");
			break;
		}
		return result;
	}

	public void SetChangeSettingAction(Action action)
	{
		this.onChangeSetting = action;
	}

	public void SetTargetMonsterList(List<MonsterData> monsterDataList)
	{
		this.targetMonsterList = monsterDataList;
	}
}
