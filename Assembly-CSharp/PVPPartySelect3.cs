using Master;
using Monster;
using MonsterList.BaseSelect;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PVPPartySelect3 : MonoBehaviour
{
	private const string onButtonSprite = "Common02_Btn_Red";

	private const string offButtonSprite = "Common02_Btn_Gray_a";

	[SerializeField]
	private GameObject goMN_ICON_CHG;

	[SerializeField]
	private ChipBaseSelect chipBaseSelect;

	[SerializeField]
	private GUIMonsterIcon[] myMonsterIconList;

	[SerializeField]
	private GUIMonsterIcon[] enemyMonsterIconList;

	[SerializeField]
	private List<GameObject> selectNumObjList = new List<GameObject>();

	[SerializeField]
	private UILabel timerLabel;

	[SerializeField]
	private UILabel selectButtonLabel;

	[SerializeField]
	private UISprite selectButtonSprite;

	[SerializeField]
	private GUICollider selectButton;

	[SerializeField]
	private GameObject leaderObj;

	[Header("キャラクターのステータスPanel")]
	[SerializeField]
	private StatusPanel statusPanel;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill monsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill2;

	[SerializeField]
	private GameObject monsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject monsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject monsterSuccessionSkillGrayNA;

	[SerializeField]
	private GameObject goDetailedSkillPanel;

	[SerializeField]
	private MonsterResistanceList detailedMonsterResistanceList;

	[SerializeField]
	private MonsterLeaderSkill detailedMonsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill detailedMonsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill detailedMonsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill detailedMonsterSuccessionSkill2;

	[SerializeField]
	private GameObject detailedMonsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject detailedMonsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject detailedMonsterSuccessionSkillGrayNA;

	[SerializeField]
	private List<GameObject> goStatusPanelPage;

	[SerializeField]
	private GameObject switchSkillPanelBtn;

	[SerializeField]
	[Header("ステータス下のリーダースキル表示")]
	private MonsterLeaderSkill leaderSkill;

	private MonsterData DataChg;

	private GameObject goLeftLargeMonsterIcon;

	private GUIMonsterIcon leftLargeMonsterIcon;

	private BaseSelectIconGrayOut iconGrayOut;

	private int statusPage = 1;

	private List<MonsterData> myMonsterDataList = new List<MonsterData>();

	private MonsterData[] mySelectMonsterDataList = new MonsterData[3];

	private CMD_PvPMatchingWait pvpMatchingWait;

	private float selectLimitTime = (float)MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_PARTY_SELECT_TIME;

	private bool timerCountCheck;

	private bool selectDataSend;

	private bool isUpdate;

	public static PVPPartySelect3 CreateInstance(Transform parentTransform)
	{
		GameObject original = Resources.Load("UIPrefab/PvP/PVPPartySelect3") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Transform transform = gameObject.transform;
		transform.parent = parentTransform;
		transform.localScale = Vector3.one;
		transform.localPosition = new Vector3(0f, 0f, 0f);
		transform.localRotation = Quaternion.identity;
		return gameObject.GetComponent<PVPPartySelect3>();
	}

	public void SetData(CMD_PvPMatchingWait matching)
	{
		this.pvpMatchingWait = matching;
		this.iconGrayOut = new BaseSelectIconGrayOut();
		this.iconGrayOut.SetNormalAction(new Action<MonsterData>(this.ActMIconShort), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetBlockAction(null, new Action<MonsterData>(this.ActMIconLong));
		for (int i = 0; i < this.mySelectMonsterDataList.Length; i++)
		{
			this.mySelectMonsterDataList[i] = null;
		}
		this.DataChg = null;
		this.chipBaseSelect.ClearChipIcons();
		this.monsterMedalList.SetActive(false);
		this.selectLimitTime = (float)MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_PARTY_SELECT_TIME;
		this.timerLabel.text = Mathf.CeilToInt(this.selectLimitTime).ToString();
		this.timerCountCheck = true;
		this.selectButtonSprite.spriteName = "Common02_Btn_Red";
		this.selectButton.activeCollider = true;
		this.selectButtonLabel.text = StringMaster.GetString("PvPMonsterSelectButtonOn");
		base.gameObject.transform.localPosition = Vector3.zero;
	}

	public void Init()
	{
		this.timerCountCheck = false;
		this.isUpdate = true;
	}

	private void Update()
	{
		if (!this.isUpdate)
		{
			return;
		}
		this.TimeCounter();
	}

	private void TimeCounter()
	{
		if (!this.timerCountCheck)
		{
			return;
		}
		if (this.selectLimitTime < 0f)
		{
			this.selectLimitTime = 0f;
			this.timerLabel.text = "0";
			this.timerCountCheck = false;
			this.PartyDataSend();
		}
		else
		{
			this.selectLimitTime -= Time.deltaTime;
			this.timerLabel.text = Mathf.CeilToInt(this.selectLimitTime).ToString();
		}
	}

	public void SetMonsterData(List<MonsterData> myMonsterData, List<MonsterData> enemyMonsterData)
	{
		this.myMonsterDataList = myMonsterData;
		for (int i = 0; i < myMonsterData.Count; i++)
		{
			this.myMonsterIconList[i].Data = myMonsterData[i];
			this.myMonsterIconList[i].SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort));
		}
		for (int j = 0; j < enemyMonsterData.Count; j++)
		{
			this.enemyMonsterIconList[j].Data = enemyMonsterData[j];
		}
	}

	private void PartyDataSend()
	{
		if (this.selectDataSend)
		{
			return;
		}
		this.selectDataSend = true;
		if (this.GetMonsterListEmptyNum() != -1)
		{
			this.AutoPartySetting();
		}
		string[] array = new string[3];
		for (int i = 0; i < this.mySelectMonsterDataList.Length; i++)
		{
			array[i] = this.GetMonsterNumber(this.mySelectMonsterDataList[i]).ToString();
		}
		this.pvpMatchingWait.SendPvPEnemyData(array);
		this.selectButtonSprite.spriteName = "Common02_Btn_Gray_a";
		this.selectButton.activeCollider = false;
		this.selectButtonLabel.text = StringMaster.GetString("PvPMonsterSelectButtonOff");
	}

	private void AutoPartySetting()
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < this.myMonsterDataList.Count; i++)
		{
			list.Add(this.myMonsterDataList[i]);
		}
		for (int j = 0; j < this.mySelectMonsterDataList.Length; j++)
		{
			if (this.mySelectMonsterDataList[j] != null)
			{
				list.Remove(this.mySelectMonsterDataList[j]);
			}
		}
		for (int k = 0; k < this.mySelectMonsterDataList.Length; k++)
		{
			if (this.mySelectMonsterDataList[k] == null)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				MonsterData monsterData = list[index];
				int monsterNumber = this.GetMonsterNumber(monsterData);
				this.mySelectMonsterDataList[k] = monsterData;
				this.SetSelectNumberIcon(k, true, this.myMonsterIconList[monsterNumber].transform.localPosition);
				list.Remove(monsterData);
			}
		}
	}

	public void PartySelectButton()
	{
		if (this.GetMonsterListEmptyNum() == -1)
		{
			this.PartyDataSend();
		}
	}

	private void ActMIconShort(MonsterData tappedMonsterData)
	{
		if (this.selectDataSend)
		{
			return;
		}
		this.DataChg = tappedMonsterData;
		this.SetSelectedCharChg();
		int monsterNumber = this.GetMonsterNumber(tappedMonsterData);
		bool flag = this.SelectMonsterListSetCheck(tappedMonsterData);
		int monsterListEmptyNum = this.GetMonsterListEmptyNum();
		int num = this.SelectMonsterListSetNumber(tappedMonsterData);
		if (monsterNumber == -1)
		{
			return;
		}
		if (monsterListEmptyNum == -1)
		{
			if (flag && num != -1)
			{
				this.SetSelectNumberIcon(num, false, default(Vector3));
				this.mySelectMonsterDataList[num] = null;
			}
		}
		else if (!flag)
		{
			this.SetSelectNumberIcon(monsterListEmptyNum, true, this.myMonsterIconList[monsterNumber].transform.localPosition);
			this.mySelectMonsterDataList[monsterListEmptyNum] = tappedMonsterData;
		}
		else if (num != -1)
		{
			this.SetSelectNumberIcon(num, false, default(Vector3));
			this.mySelectMonsterDataList[num] = null;
		}
	}

	private void RemoveIcon(MonsterData md)
	{
		if (this.DataChg != null)
		{
			this.SetEmpty();
		}
	}

	public void SetEmpty()
	{
		this.DataChg = null;
		this.chipBaseSelect.ClearChipIcons();
		this.ShowChgInfo();
		if (this.goLeftLargeMonsterIcon != null)
		{
			UnityEngine.Object.Destroy(this.goLeftLargeMonsterIcon);
		}
		this.goMN_ICON_CHG.SetActive(true);
	}

	private void SetSelectedCharChg()
	{
		if (this.DataChg != null)
		{
			if (this.goLeftLargeMonsterIcon != null)
			{
				UnityEngine.Object.Destroy(this.goLeftLargeMonsterIcon);
			}
			if (this.goMN_ICON_CHG == null)
			{
				return;
			}
			Transform transform = this.goMN_ICON_CHG.transform;
			this.leftLargeMonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(this.DataChg, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.goLeftLargeMonsterIcon = this.leftLargeMonsterIcon.gameObject;
			this.goLeftLargeMonsterIcon.SetActive(true);
			this.leftLargeMonsterIcon.Data = this.DataChg;
			this.chipBaseSelect.SetSelectedCharChg(this.DataChg);
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.DataChg);
			this.iconGrayOut.SetSelect(icon);
			this.iconGrayOut.SelectIcon(this.leftLargeMonsterIcon);
			this.leftLargeMonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			this.leftLargeMonsterIcon.Lock = this.DataChg.userMonster.IsLocked;
			UIWidget component = this.goMN_ICON_CHG.GetComponent<UIWidget>();
			UIWidget component2 = this.leftLargeMonsterIcon.gameObject.GetComponent<UIWidget>();
			if (component != null && component2 != null)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = this.leftLargeMonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(this.leftLargeMonsterIcon.transform, add);
			}
			this.goMN_ICON_CHG.SetActive(false);
			this.ShowChgInfo();
		}
	}

	public void OnSwitchSkillPanelBtn()
	{
		this.switchDetailSkillPanel(!this.goDetailedSkillPanel.activeSelf);
	}

	private void ShowChgInfo()
	{
		this.statusPanel.SetEnable(this.DataChg != null);
		if (this.DataChg != null)
		{
			this.monsterBasicInfo.SetMonsterData(this.DataChg);
			this.monsterStatusList.SetValues(this.DataChg, false);
			this.monsterLeaderSkill.SetSkill(this.DataChg);
			this.detailedMonsterLeaderSkill.SetSkill(this.DataChg);
			this.leaderSkill.SetSkill(this.DataChg);
			this.monsterUniqueSkill.SetSkill(this.DataChg);
			this.detailedMonsterUniqueSkill.SetSkill(this.DataChg);
			this.monsterSuccessionSkill.SetSkill(this.DataChg);
			this.detailedMonsterSuccessionSkill.SetSkill(this.DataChg);
			this.monsterSuccessionSkillGrayReady.SetActive(false);
			this.monsterSuccessionSkillAvailable.SetActive(false);
			this.monsterSuccessionSkillGrayNA.SetActive(false);
			this.monsterSuccessionSkill2.SetSkill(this.DataChg);
			this.detailedMonsterSuccessionSkillGrayReady.SetActive(false);
			this.detailedMonsterSuccessionSkillAvailable.SetActive(false);
			this.detailedMonsterSuccessionSkillGrayNA.SetActive(false);
			this.detailedMonsterSuccessionSkill2.SetSkill(this.DataChg);
			if (MonsterStatusData.IsVersionUp(this.DataChg.GetMonsterMaster().Simple.rare))
			{
				if (this.DataChg.GetExtraCommonSkill() == null)
				{
					this.monsterSuccessionSkillGrayReady.SetActive(true);
					this.detailedMonsterSuccessionSkillGrayReady.SetActive(true);
				}
				else
				{
					this.monsterSuccessionSkillAvailable.SetActive(true);
					this.detailedMonsterSuccessionSkillAvailable.SetActive(true);
				}
			}
			else
			{
				this.monsterSuccessionSkillGrayNA.SetActive(true);
				this.detailedMonsterSuccessionSkillGrayNA.SetActive(true);
			}
			this.monsterResistanceList.SetValues(this.DataChg);
			this.detailedMonsterResistanceList.SetValues(this.DataChg);
			this.monsterMedalList.SetValues(this.DataChg.userMonster);
			this.StatusPageChange(false);
		}
		else
		{
			this.chipBaseSelect.ClearChipIcons();
			this.monsterBasicInfo.ClearMonsterData();
			this.monsterStatusList.ClearValues();
			this.monsterMedalList.SetActive(false);
			this.switchDetailSkillPanel(false);
			this.RequestStatusPage(1);
		}
	}

	private void StatusPageChange(bool pageChange)
	{
		if (this.DataChg != null)
		{
			if (pageChange)
			{
				if (this.statusPage < this.goStatusPanelPage.Count)
				{
					this.statusPage++;
				}
				else
				{
					this.statusPage = 1;
				}
			}
			int num = 1;
			foreach (GameObject gameObject in this.goStatusPanelPage)
			{
				if (num == this.statusPage)
				{
					gameObject.SetActive(true);
					this.switchSkillPanelBtn.SetActive(gameObject.name == "PartsStatusSkill");
				}
				else
				{
					gameObject.SetActive(false);
				}
				num++;
			}
		}
	}

	private void RequestStatusPage(int requestPage)
	{
		this.statusPage = requestPage;
		if (this.statusPage >= this.goStatusPanelPage.Count || this.statusPage < 1)
		{
			this.statusPage = 1;
		}
		int num = 1;
		foreach (GameObject gameObject in this.goStatusPanelPage)
		{
			if (num == this.statusPage)
			{
				gameObject.SetActive(true);
				this.switchSkillPanelBtn.SetActive(gameObject.name == "PartsStatusSkill");
			}
			else
			{
				gameObject.SetActive(false);
			}
			num++;
		}
	}

	public void StatusPageChangeTap()
	{
		this.switchDetailSkillPanel(false);
		this.StatusPageChange(true);
	}

	public void switchDetailSkillPanel(bool isOpen)
	{
		this.goDetailedSkillPanel.SetActive(isOpen);
		UISprite component = this.switchSkillPanelBtn.GetComponent<UISprite>();
		if (isOpen)
		{
			component.flip = UIBasicSprite.Flip.Vertically;
		}
		else
		{
			component.flip = UIBasicSprite.Flip.Nothing;
		}
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
	}

	private int GetMonsterListEmptyNum()
	{
		int result = -1;
		for (int i = 0; i < this.mySelectMonsterDataList.Length; i++)
		{
			if (this.mySelectMonsterDataList[i] == null)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private void SetSelectNumberIcon(int number, bool active, Vector3 pos)
	{
		if (number >= this.selectNumObjList.Count || number < 0)
		{
			return;
		}
		this.selectNumObjList[number].SetActive(active);
		if (number == 0)
		{
			this.leaderObj.SetActive(active);
		}
		if (pos != Vector3.zero)
		{
			this.selectNumObjList[number].transform.localPosition = pos;
			if (number == 0)
			{
				this.leaderObj.transform.localPosition = pos;
			}
		}
	}

	private int GetMonsterNumber(MonsterData md)
	{
		int result = -1;
		for (int i = 0; i < this.myMonsterDataList.Count; i++)
		{
			if (this.myMonsterDataList[i].userMonster.userMonsterId.Equals(md.userMonster.userMonsterId))
			{
				result = i;
			}
		}
		return result;
	}

	private bool SelectMonsterListSetCheck(MonsterData md)
	{
		bool result = false;
		for (int i = 0; i < this.mySelectMonsterDataList.Length; i++)
		{
			if (this.mySelectMonsterDataList[i] != null && this.mySelectMonsterDataList[i].userMonster.userMonsterId.Equals(md.userMonster.userMonsterId))
			{
				result = true;
			}
		}
		return result;
	}

	private int SelectMonsterListSetNumber(MonsterData md)
	{
		int result = -1;
		for (int i = 0; i < this.mySelectMonsterDataList.Length; i++)
		{
			if (this.mySelectMonsterDataList[i] != null && this.mySelectMonsterDataList[i].userMonster.userMonsterId.Equals(md.userMonster.userMonsterId))
			{
				result = i;
			}
		}
		return result;
	}

	public void ActMIconShort_0()
	{
		this.ActMIconShort(this.myMonsterIconList[0].Data);
	}

	public void ActMIconShort_1()
	{
		this.ActMIconShort(this.myMonsterIconList[1].Data);
	}

	public void ActMIconShort_2()
	{
		this.ActMIconShort(this.myMonsterIconList[2].Data);
	}

	public void ActMIconShort_3()
	{
		this.ActMIconShort(this.myMonsterIconList[3].Data);
	}

	public void ActMIconShort_4()
	{
		this.ActMIconShort(this.myMonsterIconList[4].Data);
	}

	public void ActMIconShort_5()
	{
		this.ActMIconShort(this.myMonsterIconList[5].Data);
	}
}
