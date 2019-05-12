using Master;
using Monster;
using MonsterPicturebook;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Picturebook : CMD
{
	[SerializeField]
	private GameObject thumbnail;

	[SerializeField]
	private UILabel haveMonsterCountLabel;

	private GUISelectPanelPicturebookIcon selectPanelPicturebookIcon;

	private List<string> collectionIdList;

	private List<PicturebookMonster> collectionIconMonsterDataList;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int userId = DataMng.Instance().RespDataCM_Login.playerInfo.UserId;
		this.SetCommonUI();
		base.HideDLG();
		APIRequestTask apirequestTask = this.LoadPicturebookData(userId);
		apirequestTask.Add(new NormalTask(delegate()
		{
			this.CreateAllMonsterData();
			return this.InitializeMonsterList();
		}));
		base.StartCoroutine(apirequestTask.Run(delegate
		{
			this.ShowDLG();
			this.Show(f, sizeX, sizeY, aT);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			this.ClosePanel(false);
			RestrictionInput.EndLoad();
		}, null));
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	protected override void WindowClosed()
	{
		if (MonsterDataMng.Instance() != null)
		{
			MonsterDataMng.Instance().PushBackAllMonsterPrefab();
		}
		base.WindowClosed();
	}

	private void SetCommonUI()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("PicturebookTitle"));
		this.thumbnail.transform.gameObject.SetActive(false);
		this.selectPanelPicturebookIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelPicturebookIcon", base.gameObject).GetComponent<GUISelectPanelPicturebookIcon>();
		this.selectPanelPicturebookIcon.ScrollBarPosX = 580f;
		this.selectPanelPicturebookIcon.ScrollBarBGPosX = 580f;
		this.selectPanelPicturebookIcon.transform.localPosition = new Vector3(this.selectPanelPicturebookIcon.transform.localPosition.x, this.selectPanelPicturebookIcon.transform.localPosition.y, 20f);
		Vector3 localPosition = this.selectPanelPicturebookIcon.transform.localPosition;
		localPosition.x = -50f;
		GUICollider component = this.selectPanelPicturebookIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		if (this.goEFC_FOOTER != null)
		{
			this.selectPanelPicturebookIcon.transform.parent = this.goEFC_FOOTER.transform;
		}
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -480f;
		listWindowViewRect.xMax = 480f;
		listWindowViewRect.yMin = -297f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 180f + GUIMain.VerticalSpaceSize;
		this.selectPanelPicturebookIcon.ListWindowViewRect = listWindowViewRect;
		this.selectPanelPicturebookIcon.selectParts = GUIManager.LoadCommonGUI("ListParts/ListPartsThumbnail", null);
		this.selectPanelPicturebookIcon.selectParts.transform.SetParent(this.thumbnail.transform);
	}

	private APIRequestTask LoadPicturebookData(int TargetUserID)
	{
		GameWebAPI.RequestFA_MN_PicturebookExec request = new GameWebAPI.RequestFA_MN_PicturebookExec
		{
			SetSendData = delegate(GameWebAPI.MN_Req_Picturebook param)
			{
				param.targetUserId = TargetUserID;
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_Picturebook response)
			{
				this.haveMonsterCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), response.possessionNum, response.totalNum);
				GameWebAPI.RespDataMN_Picturebook.UserCollectionData[] userCollectionList = response.userCollectionList;
				this.collectionIdList = new List<string>();
				for (int i = 0; i < userCollectionList.Length; i++)
				{
					if ("1" == userCollectionList[i].collectionStatus)
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
		base.StartCoroutine(this.InitializeMonsterList());
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
				GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterM[i].monsterGroupId);
				if ("0" != monsterGroupMasterByMonsterGroupId.monsterCollectionId && !this.ExistCollectionId(monsterGroupMasterByMonsterGroupId.monsterCollectionId))
				{
					this.collectionIconMonsterDataList.Add(this.GetCollectionMonsterIconData(monsterGroupMasterByMonsterGroupId, monsterM[i]));
				}
			}
		}
		this.collectionIconMonsterDataList.Sort(new Comparison<PicturebookMonster>(CMD_Picturebook.CompareByCollectionId));
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

	private IEnumerator InitializeMonsterList()
	{
		this.selectPanelPicturebookIcon.initLocation = true;
		Vector3 iconScale = this.thumbnail.transform.localScale;
		this.selectPanelPicturebookIcon.useLocationRecord = true;
		yield return base.StartCoroutine(this.selectPanelPicturebookIcon.AllBuild(this.collectionIconMonsterDataList, iconScale, new Action<PicturebookMonster>(this.PressMIconShort)));
		UnityEngine.Object.Destroy(this.selectPanelPicturebookIcon.selectParts);
		this.selectPanelPicturebookIcon.selectParts = null;
		yield break;
	}

	public void PressMIconShort(PicturebookMonster monsterData)
	{
		CMD_PicturebookDetailedInfo.CreateDialog(base.gameObject, monsterData);
	}
}
