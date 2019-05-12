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
		int categoryId = int.Parse(this.missionCategoryId);
		this.onPushedButton = touchEvent;
		int missionType = -1;
		int.TryParse(missionInfo.missionType, out missionType);
		int status = missionInfo.status;
		this.SetButton(status, missionType, categoryId);
		this.rightBlackPlate.SetActive(true);
		this.conditionTitle.text = missionInfo.detail.missionDetail;
		string text = string.Empty;
		int num = 0;
		string text2 = string.Empty;
		string text3 = string.Empty;
		GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward[] reward = missionInfo.reward;
		foreach (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward reward2 in reward)
		{
			if (reward2.viewFlg == "1")
			{
				int.TryParse(reward2.assetCategoryId, out num);
				text = DataMng.Instance().GetAssetTitle(reward2.assetCategoryId, reward2.assetValue);
				text2 = reward2.assetValue;
				text3 = reward2.assetNum;
				break;
			}
		}
		AppCoroutine.Start(this.csItem.SetItemWithWaitASync(num.ToString(), text2, text3, false, delegate
		{
		}), false);
		if (num == 19)
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
		int num2 = 0;
		string missionValue = missionInfo.detail.missionValue;
		int.TryParse(missionValue, out num2);
		int num3 = Mathf.Clamp(missionInfo.nowValue, 0, num2);
		if (status != 2)
		{
			if (num2 != 0)
			{
				this.achievementRate.text = string.Format(StringMaster.GetString("SystemFraction"), num3, missionValue);
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
			if (status == 0 && (categoryId == 1 || categoryId == 2 || categoryId == 5 || categoryId == 3 || categoryId == 4 || categoryId == 6 || (categoryId == 7 && flag2) || (categoryId == 8 && flag3) || categoryId == 201 || categoryId == 204 || categoryId == 9 || categoryId == 110 || categoryId == 111 || categoryId == 112 || categoryId == 113 || categoryId == 114 || categoryId == 115 || categoryId == 116 || categoryId == 117 || categoryId == 118 || categoryId == 119 || categoryId == 131 || categoryId == 132 || categoryId == 133 || categoryId == 134 || categoryId == 144 || categoryId == 145 || categoryId == 146 || categoryId == 154 || categoryId == 155 || categoryId == 156))
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
				break;
			default:
				switch (categoryId)
				{
				case 1:
					goto IL_364;
				case 2:
					goto IL_38E;
				case 3:
					text = StringMaster.GetString("CaptureTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedGASHA();
					};
					goto IL_5B0;
				case 4:
					break;
				case 5:
					text = StringMaster.GetString("ReinforcementTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedREINFORCE();
					};
					goto IL_5B0;
				case 6:
					goto IL_4B4;
				case 7:
					text = StringMaster.GetString("SuccessionTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedSUCCESSION();
					};
					goto IL_5B0;
				case 8:
					text = StringMaster.GetString("LaboratoryTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedLABORATORY();
					};
					goto IL_5B0;
				case 9:
					text = StringMaster.GetString("ArousalTitle");
					this.onPushedButton = null;
					this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedAROUSAL();
					};
					goto IL_5B0;
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
						goto IL_5B0;
					default:
						switch (categoryId)
						{
						case 144:
						case 145:
						case 146:
							break;
						default:
							switch (categoryId)
							{
							case 154:
							case 155:
							case 156:
								goto IL_4B4;
							default:
								goto IL_5B0;
							}
							break;
						}
						break;
					case 204:
						text = StringMaster.GetString("ColosseumTitle");
						this.onPushedButton = null;
						this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
						{
							this.OnClicked_VS();
						};
						goto IL_5B0;
					}
					break;
				}
				text = StringMaster.GetString("MealTitle");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedMEAL();
				};
				goto IL_5B0;
				IL_4B4:
				text = StringMaster.GetString("EvolutionTitle");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedEVOLUTION();
				};
				goto IL_5B0;
			case 131:
				goto IL_38E;
			case 132:
				text = StringMaster.GetString("Mission-01");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedSTOREHOUSE();
				};
				goto IL_5B0;
			case 133:
				text = StringMaster.GetString("Mission-01");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedRESTAURANT();
				};
				goto IL_5B0;
			case 134:
				text = StringMaster.GetString("Mission-01");
				this.onPushedButton = null;
				this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedTRAINING();
				};
				goto IL_5B0;
			}
			IL_364:
			text = StringMaster.GetString("QuestNormal");
			this.onPushedButton = null;
			this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnClickedQUEST();
			};
			goto IL_5B0;
			IL_38E:
			text = StringMaster.GetString("Mission-01");
			this.onPushedButton = null;
			this.lastAddedAction = delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnClickedMEAT();
			};
			IL_5B0:
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
}
