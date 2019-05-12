using Master;
using MissionData;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MissionItem : GUIListPartBS
{
	[SerializeField]
	private PresentBoxItem csItem;

	[SerializeField]
	private UILabel conditionTitle;

	[SerializeField]
	private UISprite rewardIcon;

	[SerializeField]
	[Header("報酬アイテム名")]
	private UILabel rewardName;

	[SerializeField]
	private UILabel quantity;

	[SerializeField]
	private UILabel lbRewardDetail;

	[SerializeField]
	private GameObject GoOperatorMsg;

	[SerializeField]
	private UILabel achievementRate;

	[SerializeField]
	private UILabel rewardTagLabel;

	[SerializeField]
	private UILabel arTagLabel;

	private Action<MissionItem> onPushedButton;

	[SerializeField]
	private GameObject rightBlackPlate;

	[SerializeField]
	private UITable tblBtnLayout;

	[SerializeField]
	private UILabel hiddenRewardNum;

	[SerializeField]
	private GameObject rewardDetaildButton;

	[SerializeField]
	private GameObject pushedButton;

	[SerializeField]
	private UISprite pushedButtonSprite;

	[SerializeField]
	private UILabel pushedButtonLabel;

	[SerializeField]
	private UILabel debugLabel;

	[NonSerialized]
	public int missionId;

	[NonSerialized]
	public string missionCategoryId;

	[NonSerialized]
	public string lastStepFlg;

	private Action<Touch, Vector2, bool> lastAddedAction;

	private GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission missionInfo;

	protected override void Awake()
	{
		this.InitLabels();
		base.Awake();
	}

	private void InitLabels()
	{
	}

	public override void SetData()
	{
		CMD_Mission cmd_Mission = (CMD_Mission)base.GetInstanceCMD();
		this.missionInfo = cmd_Mission.GetMissionData(base.IDX);
	}

	public override void InitParts()
	{
		CMD_Mission @object = (CMD_Mission)base.GetInstanceCMD();
		this.SetDetail(this.missionInfo, new Action<MissionItem>(@object.OnPushedButton));
	}

	public override void RefreshParts()
	{
		CMD_Mission @object = (CMD_Mission)base.GetInstanceCMD();
		this.SetDetail(this.missionInfo, new Action<MissionItem>(@object.OnPushedButton));
	}

	public void SetDetail(GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission missionInfo, Action<MissionItem> touchEvent)
	{
		this.rewardTagLabel.text = StringMaster.GetString("Mission-09");
		this.arTagLabel.text = StringMaster.GetString("Mission-05");
		int.TryParse(missionInfo.detail.missionId, out this.missionId);
		this.missionCategoryId = missionInfo.missionCategoryId;
		this.lastStepFlg = missionInfo.lastStepFlg;
		int categoryId = int.Parse(this.missionCategoryId);
		this.onPushedButton = touchEvent;
		int missionType = -1;
		int.TryParse(missionInfo.displayGroup, out missionType);
		int status = missionInfo.status;
		this.SetButton(status, missionType, categoryId);
		int num = 0;
		foreach (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward reward2 in missionInfo.reward)
		{
			if (reward2.viewFlg == "1")
			{
				num++;
			}
		}
		if (num > 1)
		{
			this.rewardDetaildButton.SetActive(true);
			this.hiddenRewardNum.text = string.Format(StringMaster.GetString("MissionRewardHiddenNum"), num - 1);
		}
		else
		{
			this.rewardDetaildButton.SetActive(false);
			this.hiddenRewardNum.text = null;
		}
		this.tblBtnLayout.Reposition();
		this.rightBlackPlate.SetActive(true);
		this.conditionTitle.text = missionInfo.detail.missionDetail;
		string text = string.Empty;
		int num2 = 0;
		string text2 = string.Empty;
		string text3 = string.Empty;
		GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward[] reward3 = missionInfo.reward;
		foreach (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward reward4 in reward3)
		{
			if (reward4.viewFlg == "1")
			{
				int.TryParse(reward4.assetCategoryId, out num2);
				text = DataMng.Instance().GetAssetTitle(reward4.assetCategoryId, reward4.assetValue);
				text2 = reward4.assetValue;
				text3 = reward4.assetNum;
				break;
			}
		}
		AppCoroutine.Start(this.csItem.SetItemWithWaitASync(num2.ToString(), text2, text3, false, delegate
		{
		}), false);
		if (num2 == 19)
		{
			GameWebAPI.RespDataMA_TitleMaster.TitleM titleM;
			TitleDataMng.GetDictionaryTitleM().TryGetValue(int.Parse(text2), out titleM);
			this.lbRewardDetail.text = ((titleM == null) ? StringMaster.GetString("AlertDataErrorTitle") : titleM.name);
			this.lbRewardDetail.gameObject.SetActive(true);
			this.GoOperatorMsg.SetActive(false);
			this.quantity.gameObject.SetActive(false);
		}
		else
		{
			this.quantity.text = ((!(this.rewardIcon.spriteName == "Common02_Icon_Chip")) ? text3 : StringFormat.Cluster(text3));
			this.lbRewardDetail.gameObject.SetActive(false);
			this.GoOperatorMsg.SetActive(true);
			this.quantity.gameObject.SetActive(true);
		}
		int num3 = 0;
		string missionValue = missionInfo.detail.missionValue;
		int.TryParse(missionValue, out num3);
		int num4 = Mathf.Clamp(missionInfo.nowValue, 0, num3);
		if (status != 2)
		{
			if (num3 != 0)
			{
				this.achievementRate.text = string.Format(StringMaster.GetString("SystemFraction"), num4, missionValue);
			}
			else
			{
				this.achievementRate.text = StringMaster.GetString("SystemNoneHyphen");
			}
			this.achievementRate.spacingX = 0;
			this.achievementRate.color = Label.NOT_COMPLETE_COLOR;
		}
		else
		{
			this.achievementRate.text = StringMaster.GetString("Mission-06");
			this.achievementRate.spacingX = -1;
			this.achievementRate.color = Label.COMPLETE_COLOR;
		}
	}

	private void SetButton(int status, int missionType, int categoryId)
	{
		BoxCollider component = this.pushedButtonSprite.GetComponent<BoxCollider>();
		GUICollider component2 = this.pushedButtonSprite.GetComponent<GUICollider>();
		component.enabled = (1 == status);
		bool flag4 = false;
		if (status == 2)
		{
			this.pushedButtonSprite.enabled = false;
		}
		else
		{
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < FarmRoot.Instance.Scenery.farmObjects.Count; i++)
			{
				if (!FarmRoot.Instance.Scenery.farmObjects[i].IsConstruction())
				{
					if (FarmRoot.Instance.Scenery.farmObjects[i].facilityID == 4)
					{
						flag2 = true;
					}
					else if (FarmRoot.Instance.Scenery.farmObjects[i].facilityID == 5)
					{
						flag3 = true;
					}
				}
			}
			if (status == 0 && (categoryId == 1 || categoryId == 2 || categoryId == 5 || categoryId == 3 || categoryId == 4 || categoryId == 6 || (categoryId == 7 && flag2) || (categoryId == 8 && flag3) || categoryId == 201 || categoryId == 204 || categoryId == 205 || categoryId == 9 || categoryId == 10 || categoryId == 110 || categoryId == 111 || categoryId == 112 || categoryId == 113 || categoryId == 125 || categoryId == 126 || categoryId == 127 || categoryId == 120 || categoryId == 114 || categoryId == 115 || categoryId == 116 || categoryId == 117 || categoryId == 118 || categoryId == 119 || categoryId == 121 || categoryId == 122 || categoryId == 123 || categoryId == 161 || categoryId == 162 || categoryId == 163 || categoryId == 164 || categoryId == 131 || categoryId == 132 || categoryId == 133 || categoryId == 134 || categoryId == 144 || categoryId == 145 || categoryId == 146 || categoryId == 147 || categoryId == 154 || categoryId == 155 || categoryId == 156 || categoryId == 181))
			{
				status = 3;
				component.enabled = true;
				flag4 = true;
			}
			this.pushedButtonSprite.enabled = true;
			this.pushedButtonSprite.spriteName = Button.spriteNames[status];
		}
		this.pushedButtonLabel.color = Button.colors[status];
		if (this.lastAddedAction != null)
		{
			component2.onTouchEnded -= this.lastAddedAction;
			this.lastAddedAction = null;
		}
		if (flag4)
		{
			string text = string.Empty;
			switch (categoryId)
			{
			case 110:
			case 111:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 125:
			case 126:
			case 127:
			case 161:
			case 162:
			case 163:
			case 164:
				break;
			default:
				switch (categoryId)
				{
				case 1:
					break;
				case 2:
					goto IL_475;
				case 3:
					text = StringMaster.GetString("CaptureTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedGASHA();
					};
					goto IL_697;
				case 4:
					goto IL_571;
				case 5:
					text = StringMaster.GetString("ReinforcementTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedREINFORCE();
					};
					goto IL_697;
				case 6:
					goto IL_59B;
				case 7:
					text = StringMaster.GetString("SuccessionTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedSUCCESSION();
					};
					goto IL_697;
				case 8:
				case 10:
					text = StringMaster.GetString("LaboratoryTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedLABORATORY();
					};
					goto IL_697;
				case 9:
					text = StringMaster.GetString("ArousalTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedAROUSAL();
					};
					goto IL_697;
				default:
					switch (categoryId)
					{
					case 201:
						text = StringMaster.GetString("FriendTitle");
						this.onPushedButton = null;
						this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
						{
							this.OnClickedFRIEND();
						};
						goto IL_697;
					default:
						if (categoryId != 181)
						{
							goto IL_697;
						}
						goto IL_59B;
					case 204:
					case 205:
						text = StringMaster.GetString("ColosseumTitle");
						this.onPushedButton = null;
						this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
						{
							this.OnClicked_VS();
						};
						goto IL_697;
					}
					break;
				}
				break;
			case 131:
				goto IL_475;
			case 132:
				text = StringMaster.GetString("Mission-01");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedSTOREHOUSE();
				};
				goto IL_697;
			case 133:
				text = StringMaster.GetString("Mission-01");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRESTAURANT();
				};
				goto IL_697;
			case 134:
				text = StringMaster.GetString("Mission-01");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedTRAINING();
				};
				goto IL_697;
			case 144:
			case 145:
			case 146:
			case 147:
				goto IL_571;
			case 154:
			case 155:
			case 156:
				goto IL_59B;
			}
			text = StringMaster.GetString("QuestNormal");
			this.onPushedButton = null;
			this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnClickedQUEST();
			};
			goto IL_697;
			IL_475:
			text = StringMaster.GetString("Mission-01");
			this.onPushedButton = null;
			this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnClickedMEAT();
			};
			goto IL_697;
			IL_571:
			text = StringMaster.GetString("MealTitle");
			this.onPushedButton = null;
			this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnClickedMEAL();
			};
			goto IL_697;
			IL_59B:
			text = StringMaster.GetString("EvolutionTitle");
			this.onPushedButton = null;
			this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnClickedEVOLUTION();
			};
			IL_697:
			component2.onTouchEnded += this.lastAddedAction;
			this.pushedButtonLabel.text = text;
		}
		else
		{
			this.pushedButtonLabel.text = Button.texts[status];
		}
	}

	private void OnClickedQUEST()
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.OnClickedQuest();
		}
	}

	private void OnClickedMEAT()
	{
		GUIManager.CloseAllCommonDialog(new Action<int>(this.GoToMEAT));
	}

	private void GoToMEAT(int i)
	{
		this.GoToFacility(1);
	}

	private void OnClickedSTOREHOUSE()
	{
		GUIManager.CloseAllCommonDialog(new Action<int>(this.GoToSTOREHOUSE));
	}

	private void GoToSTOREHOUSE(int i)
	{
		this.GoToFacility(2);
	}

	private void OnClickedRESTAURANT()
	{
		GUIManager.CloseAllCommonDialog(new Action<int>(this.GoToRESTAURANT));
	}

	private void GoToRESTAURANT(int i)
	{
		this.GoToFacility(3);
	}

	private void OnClickedTRAINING()
	{
		GUIManager.CloseAllCommonDialog(new Action<int>(this.GoToTRAINING));
	}

	private void GoToTRAINING(int i)
	{
		this.GoToFacility(4);
	}

	private void GoToFacility(int facilityID)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			FarmScenery scenery = instance.Scenery;
			FarmObject[] array = scenery.farmObjects.Where((FarmObject x) => x.facilityID == facilityID).ToArray<FarmObject>();
			if (array != null && 0 < array.Length)
			{
				FarmObject farmObject = array[0];
				Vector3 baseGridPosition3D = farmObject.GetBaseGridPosition3D();
				GUICameraControll component = instance.Camera.GetComponent<GUICameraControll>();
				if (null != component)
				{
					AppCoroutine.Start(this.WaitMovedForFarmCamera(component, baseGridPosition3D), false);
				}
			}
		}
	}

	private IEnumerator WaitMovedForFarmCamera(GUICameraControll farmCamera, Vector3 position)
	{
		GUIMain.BarrierON(null);
		yield return AppCoroutine.Start(farmCamera.MoveCameraToLookAtPoint(position, 1f), false);
		GUIMain.BarrierOFF();
		yield break;
	}

	private void OnClickedREINFORCE()
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.OnClickedTraining();
		}
	}

	private void OnClickedAROUSAL()
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.OnClickedArousal();
		}
	}

	private void OnClickedGASHA()
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.OnClickedGacha();
		}
	}

	private void OnClickedMEAL()
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.OnClickedMeal();
		}
	}

	private void OnClickedEVOLUTION()
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.OnClickedEvo();
		}
	}

	private void OnClickedSUCCESSION()
	{
		GUIManager.CloseAllCommonDialog(delegate(int i)
		{
			GUIMain.ShowCommonDialog(null, "CMD_Succession");
		});
	}

	private void OnClickedLABORATORY()
	{
		GUIManager.CloseAllCommonDialog(delegate(int i)
		{
			GUIMain.ShowCommonDialog(null, "CMD_Laboratory");
		});
	}

	private void OnClickedFRIEND()
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.OnClickedFriend();
		}
	}

	private void OnClicked_VS()
	{
		FarmColosseum.ShowPvPTop();
	}

	private void OnPushedButton()
	{
		if (this.onPushedButton != null)
		{
			this.onPushedButton(this);
		}
	}

	private void OnRewardDetailButton()
	{
		CMD_MissionItemList.missionInfo = this.missionInfo;
		GUIMain.ShowCommonDialog(null, "CMD_MissionItemList");
	}
}
