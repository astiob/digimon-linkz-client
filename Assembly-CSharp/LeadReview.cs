using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeadReview
{
	public static readonly string DISPLAY_DATETIME_PREFS_KEY = "DateTimeDisplayLeadReview";

	private static Action onFinishedAction;

	public bool DisplayDialog(MonsterData MonsterData)
	{
		if (!ConstValue.REVIEW_STOP_FLAG)
		{
			string @string = PlayerPrefs.GetString(LeadReview.DISPLAY_DATETIME_PREFS_KEY, string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				return false;
			}
			if (!this.JudgeHaveMedal(MonsterData.userMonster))
			{
				return false;
			}
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.ConfirmReaction), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("GashaReviewTitle");
			cmd_Confirm.Info = StringMaster.GetString("GashaReviewInfo");
			PlayerPrefs.SetString(LeadReview.DISPLAY_DATETIME_PREFS_KEY, ServerDateTime.Now.ToString());
		}
		return true;
	}

	public void DisplayDialog(List<MonsterData> MonsterDataList)
	{
		if (!ConstValue.REVIEW_STOP_FLAG)
		{
			string @string = PlayerPrefs.GetString(LeadReview.DISPLAY_DATETIME_PREFS_KEY, string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				return;
			}
			foreach (MonsterData monsterData in MonsterDataList)
			{
				if (this.DisplayDialog(monsterData))
				{
					break;
				}
			}
		}
	}

	public bool DeletePrefs(string CodeM)
	{
		string @string = PlayerPrefs.GetString(LeadReview.DISPLAY_DATETIME_PREFS_KEY, string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return false;
		}
		DateTime d = DateTime.Parse(@string);
		DateTime d2 = DateTime.Parse(CodeM);
		if ((d - d2).TotalSeconds < 0.0)
		{
			PlayerPrefs.DeleteKey(LeadReview.DISPLAY_DATETIME_PREFS_KEY);
			return true;
		}
		return false;
	}

	private bool JudgeHaveMedal(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList UserMonsterData)
	{
		return UserMonsterData.hpAbilityFlg == "1" || UserMonsterData.hpAbilityFlg == "2" || (UserMonsterData.attackAbilityFlg == "1" || UserMonsterData.attackAbilityFlg == "2") || (UserMonsterData.defenseAbilityFlg == "1" || UserMonsterData.defenseAbilityFlg == "2") || (UserMonsterData.spAttackAbilityFlg == "1" || UserMonsterData.spAttackAbilityFlg == "2") || (UserMonsterData.spDefenseAbilityFlg == "1" || UserMonsterData.spDefenseAbilityFlg == "2") || (UserMonsterData.speedAbilityFlg == "1" || UserMonsterData.speedAbilityFlg == "2");
	}

	private void ConfirmReaction(int index)
	{
		if (index == 0)
		{
			this.OpenReviewPage();
		}
	}

	private void OpenReviewPage()
	{
		Application.OpenURL(ConstValue.STORE_SITE_URL);
	}

	public static void ShowReviewConfirm(LeadReview.MessageType type, Action finishedAction = null, bool ignoreStopFlag = false)
	{
		if (!ConstValue.REVIEW_STOP_FLAG || ignoreStopFlag)
		{
			string @string;
			string string2;
			switch (type)
			{
			case LeadReview.MessageType.FIRST_CLEAR_AREA1_STAGE3:
				@string = StringMaster.GetString("LeadReview01Title");
				string2 = StringMaster.GetString("LeadReview01Info");
				break;
			case LeadReview.MessageType.TOTAL_LOGIN_COUNT_3DAYS:
				@string = StringMaster.GetString("LeadReview02Title");
				string2 = StringMaster.GetString("LeadReview02Info");
				break;
			case LeadReview.MessageType.FIRST_EVOLUTION:
				@string = StringMaster.GetString("LeadReview03Title");
				string2 = StringMaster.GetString("LeadReview03Info");
				break;
			case LeadReview.MessageType.FIRST_ULTIMA_EVOLUTION:
				@string = StringMaster.GetString("LeadReview04Title");
				string2 = StringMaster.GetString("LeadReview04Info");
				break;
			default:
				if (finishedAction != null)
				{
					finishedAction();
				}
				return;
			}
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(LeadReview.OnButtonReviewConfirm), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = @string;
			cmd_Confirm.Info = string2;
			LeadReview.onFinishedAction = finishedAction;
		}
		else if (finishedAction != null)
		{
			finishedAction();
		}
	}

	private static void OnButtonReviewConfirm(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			Application.OpenURL(ConstValue.STORE_SITE_URL);
		}
		if (LeadReview.onFinishedAction != null)
		{
			LeadReview.onFinishedAction();
			LeadReview.onFinishedAction = null;
		}
	}

	public enum MessageType
	{
		FIRST_CLEAR_AREA1_STAGE3,
		TOTAL_LOGIN_COUNT_3DAYS,
		FIRST_EVOLUTION,
		FIRST_ULTIMA_EVOLUTION
	}
}
