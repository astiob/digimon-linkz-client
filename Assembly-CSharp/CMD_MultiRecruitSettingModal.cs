using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_MultiRecruitSettingModal : CMD
{
	public static CMD_MultiRecruitSettingModal instance;

	public static string inputedRecruitMessage;

	public static int selectedPublishedType = 1;

	public static int selectedMoodType = 1;

	[SerializeField]
	private List<GameObject> goPublishedBtnList;

	private List<GUICollider> clPublishedBtnList;

	private List<UISprite> spPublishedBtnList;

	private List<UILabel> lbPublishedBtnList;

	[SerializeField]
	private List<GameObject> goMoodBtnList;

	private List<GUICollider> clMoodBtnList;

	private List<UISprite> spMoodBtnList;

	private List<UILabel> lbMoodBtnList;

	[SerializeField]
	private UILabel ModalTitle;

	[SerializeField]
	private UILabel PublishdTitle;

	[SerializeField]
	private UILabel MoodTitle;

	[SerializeField]
	private UILabel submitButtonText;

	[SerializeField]
	private UIInput recruitMessageInput;

	[SerializeField]
	private UILabel recruitMessageLabel;

	private Action<GameWebAPI.RespData_MultiRoomCreate> callbackAction;

	private GameWebAPI.RespData_MultiRoomCreate MultiRoomCreateData;

	public int deckNum { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_MultiRecruitSettingModal.instance = this;
		this.SetupPublishedBtn();
		this.SetupMoodBtn();
		this.SetupLabel();
		this.OnClickedPublishedBtn(CMD_MultiRecruitSettingModal.selectedPublishedType - 1);
		this.OnClickedMoodBtn(CMD_MultiRecruitSettingModal.selectedMoodType - 1);
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
		if (CMD_MultiRecruitSettingModal.inputedRecruitMessage == null)
		{
			CMD_MultiRecruitSettingModal.inputedRecruitMessage = StringMaster.GetString("RecruitRule-09");
		}
		this.ModalTitle.text = StringMaster.GetString("RecruitRule-01");
		this.PublishdTitle.text = StringMaster.GetString("RecruitRule-02");
		this.MoodTitle.text = StringMaster.GetString("RecruitRule-05");
		this.submitButtonText.text = StringMaster.GetString("PartyRecruit");
		this.recruitMessageInput.value = CMD_MultiRecruitSettingModal.inputedRecruitMessage;
	}

	private void SetupPublishedBtn()
	{
		this.clPublishedBtnList = new List<GUICollider>();
		this.spPublishedBtnList = new List<UISprite>();
		this.lbPublishedBtnList = new List<UILabel>();
		for (int i = 0; i < this.goPublishedBtnList.Count; i++)
		{
			GUICollider component = this.goPublishedBtnList[i].GetComponent<GUICollider>();
			UISprite component2 = this.goPublishedBtnList[i].GetComponent<UISprite>();
			this.clPublishedBtnList.Add(component);
			this.spPublishedBtnList.Add(component2);
			int num;
			foreach (object obj in this.goPublishedBtnList[i].transform)
			{
				Transform transform = (Transform)obj;
				UILabel component3 = transform.gameObject.GetComponent<UILabel>();
				num = i;
				if (num != 0)
				{
					if (num == 1)
					{
						component3.text = StringMaster.GetString("RecruitRule-04");
					}
				}
				else
				{
					component3.text = StringMaster.GetString("RecruitRule-03");
				}
				this.lbPublishedBtnList.Add(component3);
			}
			num = i;
			if (num != 0)
			{
				if (num == 1)
				{
					component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedPublishedBtn(1);
					};
				}
			}
			else
			{
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedPublishedBtn(0);
				};
			}
		}
		this.SetupPublishedBtnColor(0);
	}

	private void SetupMoodBtn()
	{
		this.clMoodBtnList = new List<GUICollider>();
		this.spMoodBtnList = new List<UISprite>();
		this.lbMoodBtnList = new List<UILabel>();
		for (int i = 0; i < this.goMoodBtnList.Count; i++)
		{
			GUICollider component = this.goMoodBtnList[i].GetComponent<GUICollider>();
			UISprite component2 = this.goMoodBtnList[i].GetComponent<UISprite>();
			this.clMoodBtnList.Add(component);
			this.spMoodBtnList.Add(component2);
			foreach (object obj in this.goMoodBtnList[i].transform)
			{
				Transform transform = (Transform)obj;
				UILabel component3 = transform.gameObject.GetComponent<UILabel>();
				switch (i)
				{
				case 0:
					component3.text = StringMaster.GetString("RecruitRule-06");
					break;
				case 1:
					component3.text = StringMaster.GetString("RecruitRule-07");
					break;
				case 2:
					component3.text = StringMaster.GetString("RecruitRule-08");
					break;
				}
				this.lbMoodBtnList.Add(component3);
			}
			switch (i)
			{
			case 0:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedMoodBtn(0);
				};
				break;
			case 1:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedMoodBtn(1);
				};
				break;
			case 2:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedMoodBtn(2);
				};
				break;
			}
		}
		this.SetupMoodBtnColor(0);
	}

	private void OnClickedPublishedBtn(int idx)
	{
		CMD_MultiRecruitSettingModal.selectedPublishedType = idx + 1;
		this.SetupPublishedBtnColor(idx);
	}

	private void OnClickedMoodBtn(int idx)
	{
		CMD_MultiRecruitSettingModal.selectedMoodType = idx + 1;
		this.SetupMoodBtnColor(idx);
	}

	private void SetupPublishedBtnColor(int idx = 0)
	{
		for (int i = 0; i < this.goPublishedBtnList.Count; i++)
		{
			if (i == idx)
			{
				this.spPublishedBtnList[i].spriteName = "Common02_Btn_SupportRed";
				this.lbPublishedBtnList[i].color = Color.white;
			}
			else
			{
				this.spPublishedBtnList[i].spriteName = "Common02_Btn_SupportWhite";
				this.lbPublishedBtnList[i].color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
			}
		}
	}

	private void SetupMoodBtnColor(int idx = 0)
	{
		for (int i = 0; i < this.goMoodBtnList.Count; i++)
		{
			if (i == idx)
			{
				this.spMoodBtnList[i].spriteName = "Common02_Btn_SupportRed";
				this.lbMoodBtnList[i].color = Color.white;
			}
			else
			{
				this.spMoodBtnList[i].spriteName = "Common02_Btn_SupportWhite";
				this.lbMoodBtnList[i].color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
			}
		}
	}

	public void CheckInputContent()
	{
		this.recruitMessageInput.value = this.recruitMessageInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
		this.recruitMessageLabel.text = this.recruitMessageInput.value;
		this.recruitMessageInput.label.text = this.recruitMessageInput.value;
		CMD_MultiRecruitSettingModal.inputedRecruitMessage = this.recruitMessageInput.value;
	}

	public void OnClickedRecruitBtn()
	{
		if (this.recruitMessageInput.value.Length > 50)
		{
			this.ShowDaialogOverTheLength();
		}
		else if (TextUtil.SurrogateCheck(this.recruitMessageInput.value))
		{
			this.ShowDaialogForbiddenChar();
		}
		else
		{
			this.DispLoading(true);
			base.StartCoroutine(this.CreateMultiRecruitRoom());
		}
	}

	private IEnumerator CreateMultiRecruitRoom()
	{
		GameWebAPI.MultiRoomCreate request = new GameWebAPI.MultiRoomCreate
		{
			SetSendData = delegate(GameWebAPI.ReqData_MultiRoomCreate param)
			{
				param.worldDungeonId = int.Parse(CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldDungeonId);
				param.moodType = CMD_MultiRecruitSettingModal.selectedMoodType;
				param.announceType = CMD_MultiRecruitSettingModal.selectedPublishedType;
				param.introduction = this.recruitMessageInput.value;
				param.deckNum = this.deckNum;
			},
			OnReceived = delegate(GameWebAPI.RespData_MultiRoomCreate response)
			{
				this.MultiRoomCreateData = response;
			}
		};
		yield return base.StartCoroutine(request.RunOneTime(null, null, null));
		if (this.MultiRoomCreateData == null)
		{
			this.DispLoading(false);
			this.ClosePanel(true);
		}
		else
		{
			if (this.callbackAction != null)
			{
				this.callbackAction(this.MultiRoomCreateData);
			}
			this.DispLoading(false);
			this.ClosePanel(true);
		}
		yield break;
	}

	public void SetCallbackAction(Action<GameWebAPI.RespData_MultiRoomCreate> action)
	{
		this.callbackAction = action;
	}

	private void ShowDaialogOverTheLength()
	{
		CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(null, "CMD_Alert") as CMD_Alert;
		if (cmd_Alert == null)
		{
			return;
		}
		cmd_Alert.Title = StringMaster.GetString("MyProfile-15");
		cmd_Alert.Info = StringMaster.GetString("RecruitRule-10");
		cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
	}

	private void ShowDaialogForbiddenChar()
	{
		CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(null, "CMD_Alert") as CMD_Alert;
		if (cmd_Alert == null)
		{
			return;
		}
		cmd_Alert.Title = StringMaster.GetString("MyProfile-13");
		cmd_Alert.Info = StringMaster.GetString("MyProfile-14");
		cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
		this.recruitMessageInput.value = string.Empty;
	}

	private void DispLoading(bool isLoading)
	{
		if (isLoading)
		{
			if (!Loading.IsShow())
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			}
		}
		else if (Loading.IsShow())
		{
			RestrictionInput.EndLoad();
		}
	}
}
