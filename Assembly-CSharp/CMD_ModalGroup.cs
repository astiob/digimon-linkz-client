using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_ModalGroup : CMD
{
	[Header("メインタイトルラベル")]
	[SerializeField]
	private UILabel titleLabel;

	[Header("サブタイトルラベル")]
	[SerializeField]
	private UILabel subTitleLabel;

	[Header("サブタイトルのGameObject")]
	[SerializeField]
	private GameObject subTitleGO;

	[SerializeField]
	private GameObject goSelectPanel;

	[SerializeField]
	private GameObject goListParts;

	private GUISelectPanelText csSelectPanel;

	private string title = string.Empty;

	private string subTitle = string.Empty;

	private static GameWebAPI.RespDataGA_GetGachaInfo.AppealType[] dts;

	public string Title
	{
		get
		{
			return this.title;
		}
		set
		{
			this.title = value;
			this.titleLabel.text = this.title;
		}
	}

	private void HideSubTitle()
	{
		this.subTitleGO.SetActive(false);
	}

	public string TitleSub
	{
		get
		{
			return this.subTitle;
		}
		set
		{
			this.subTitle = value;
			if (string.IsNullOrEmpty(this.subTitle))
			{
				this.HideSubTitle();
			}
			else
			{
				this.subTitleLabel.text = this.subTitle;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.Title = string.Empty;
		this.TitleSub = string.Empty;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetCommonUI();
		this.InitList();
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

	private void FakeMethod()
	{
	}

	public static GameWebAPI.RespDataGA_GetGachaInfo.AppealType[] Data
	{
		set
		{
			CMD_ModalGroup.dts = value;
		}
	}

	private void SetCommonUI()
	{
		this.csSelectPanel = this.goSelectPanel.GetComponent<GUISelectPanelText>();
		this.csSelectPanel.selectParts = this.goListParts;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -445f;
		listWindowViewRect.xMax = 445f;
		listWindowViewRect.yMin = -238f;
		listWindowViewRect.yMax = 178f;
		this.csSelectPanel.ListWindowViewRect = listWindowViewRect;
	}

	private void InitList()
	{
		List<GUIListPartsTextData> list = new List<GUIListPartsTextData>();
		for (int i = 0; i < CMD_ModalGroup.dts.Length; i++)
		{
			char[] trimChars = new char[]
			{
				'n',
				'_'
			};
			GUIListPartsTextData guilistPartsTextData = new GUIListPartsTextData();
			guilistPartsTextData.text = CMD_ModalGroup.dts[i].monsterGroupId;
			guilistPartsTextData.effList = new List<int>();
			for (int j = 0; j < CMD_ModalGroup.dts[i].status.Length; j++)
			{
				string text = CMD_ModalGroup.dts[i].status[j];
				text = text.Trim(trimChars);
				int num = int.Parse(text);
				if (num > 0)
				{
					guilistPartsTextData.effList.Add(num);
				}
			}
			list.Add(guilistPartsTextData);
		}
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM[] monsterM = MasterDataMng.Instance().RespDataMA_MonsterMG.monsterM;
		for (int j = 0; j < list.Count; j++)
		{
			int i;
			for (i = 0; i < monsterM.Length; i++)
			{
				if (monsterM[i].monsterGroupId == list[j].text)
				{
					list[j].text = monsterM[i].monsterName;
					break;
				}
			}
			if (i == monsterM.Length)
			{
				global::Debug.LogError("=============================================== NotFound Monster Group ID = " + list[j].text + " マスターデータバグ");
			}
		}
		this.goListParts.SetActive(true);
		this.csSelectPanel.initLocation = true;
		this.csSelectPanel.AllBuild(list);
		this.goListParts.SetActive(false);
	}
}
