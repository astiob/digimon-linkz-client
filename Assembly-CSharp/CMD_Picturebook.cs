using Master;
using Monster;
using MonsterPicturebook;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CMD_Picturebook : CMD
{
	[SerializeField]
	private GameObject thumbnail;

	[SerializeField]
	private UILabel haveMonsterCountLabel;

	[SerializeField]
	private GUISelectPanelViewPartsUD selectPanelPicturebook;

	private List<string> collectionIdList;

	private List<PicturebookMonster> collectionIconMonsterDataList;

	[CompilerGenerated]
	private static Comparison<PicturebookMonster> <>f__mg$cache0;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		string userId = DataMng.Instance().RespDataCM_Login.playerInfo.userId;
		this.SetCommonUI();
		base.HideDLG();
		APIRequestTask task = this.LoadPicturebookData(userId);
		base.StartCoroutine(task.Run(delegate
		{
			this.CreateAllMonsterData();
			this.CreateSelectPanel();
			this.ShowDLG();
			this.<Show>__BaseCallProxy0(f, sizeX, sizeY, aT);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			this.ClosePanel(false);
			RestrictionInput.EndLoad();
		}, null));
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		PicturebookItem.ClearMonsterTex();
		base.ClosePanel(animation);
		this.selectPanelPicturebook.FadeOutAllListParts(null, false);
		this.selectPanelPicturebook.SetHideScrollBarAllWays(true);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	protected override void WindowClosed()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
		base.WindowClosed();
	}

	private void SetCommonUI()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("PicturebookTitle"));
		this.thumbnail.transform.gameObject.SetActive(false);
	}

	private void CreateSelectPanel()
	{
		this.selectPanelPicturebook.gameObject.SetActive(true);
		int num = this.collectionIconMonsterDataList.Count / PicturebookItem.ITEM_COUNT_PER_LINE;
		if (this.collectionIconMonsterDataList.Count % PicturebookItem.ITEM_COUNT_PER_LINE > 0)
		{
			num++;
		}
		PicturebookItem.MakeAllMonsterTex(this, this.collectionIconMonsterDataList.Count);
		this.selectPanelPicturebook.AllBuild(num, true, 1f, 1f, null, this, true);
	}

	private APIRequestTask LoadPicturebookData(string targetUserID)
	{
		GameWebAPI.RequestFA_MN_PicturebookExec request = new GameWebAPI.RequestFA_MN_PicturebookExec
		{
			SetSendData = delegate(GameWebAPI.MN_Req_Picturebook param)
			{
				param.targetUserId = targetUserID;
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_Picturebook response)
			{
				this.haveMonsterCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), response.possessionNum, response.totalNum);
				GameWebAPI.RespDataMN_Picturebook.UserCollectionData[] userCollectionList = response.userCollectionList;
				this.collectionIdList = new List<string>();
				for (int i = 0; i < userCollectionList.Length; i++)
				{
					if (userCollectionList[i].IsHave())
					{
						this.collectionIdList.Add(userCollectionList[i].monsterCollectionId);
					}
				}
			}
		};
		return new APIRequestTask(request, true);
	}

	private void Initialize(Action onInitialized)
	{
		this.CreateAllMonsterData();
		this.CreateSelectPanel();
		if (onInitialized != null)
		{
			onInitialized();
		}
	}

	private void CreateAllMonsterData()
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM[] monsterM = MasterDataMng.Instance().RespDataMA_MonsterMS.monsterM;
		this.collectionIconMonsterDataList = new List<PicturebookMonster>();
		for (int i = 0; i < monsterM.Length; i++)
		{
			if (monsterM[i].GetArousal() == 0)
			{
				GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterM[i].monsterGroupId).Group;
				if ("0" != group.monsterCollectionId && !this.ExistCollectionId(group.monsterCollectionId))
				{
					this.collectionIconMonsterDataList.Add(this.GetCollectionMonsterIconData(group, monsterM[i]));
				}
			}
		}
		List<PicturebookMonster> list = this.collectionIconMonsterDataList;
		if (CMD_Picturebook.<>f__mg$cache0 == null)
		{
			CMD_Picturebook.<>f__mg$cache0 = new Comparison<PicturebookMonster>(CMD_Picturebook.CompareByCollectionId);
		}
		list.Sort(CMD_Picturebook.<>f__mg$cache0);
	}

	private bool ExistCollectionId(string collectionId)
	{
		bool result = false;
		int num = int.Parse(collectionId);
		for (int i = 0; i < this.collectionIconMonsterDataList.Count; i++)
		{
			if (num == this.collectionIconMonsterDataList[i].collectionId)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public PicturebookMonster GetMonsterData(int listId)
	{
		if (this.collectionIconMonsterDataList.Count <= listId)
		{
			return null;
		}
		return this.collectionIconMonsterDataList[listId];
	}

	private static int CompareByCollectionId(PicturebookMonster dataA, PicturebookMonster dataB)
	{
		return dataA.collectionId - dataB.collectionId;
	}

	private PicturebookMonster GetCollectionMonsterIconData(GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMaster, GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMaster)
	{
		PicturebookMonster picturebookMonster = new PicturebookMonster
		{
			collectionId = int.Parse(monsterGroupMaster.monsterCollectionId)
		};
		picturebookMonster.monsterMaster = new MonsterClientMaster(monsterMaster, monsterGroupMaster);
		if (!this.collectionIdList.Contains(monsterGroupMaster.monsterCollectionId))
		{
			picturebookMonster.isUnknown = true;
		}
		return picturebookMonster;
	}

	public void PressMIconShort(PicturebookMonster monsterData)
	{
		CMD_PicturebookDetailedInfo.CreateDialog(base.gameObject, monsterData);
	}
}
