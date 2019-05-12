using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_LoginBase : CMD
{
	private int REWARD_TODAY;

	private int REWARD_NEXT = 1;

	protected string TODAY_STAMP_PREF_NAME = "UISPR_GET";

	public string NAVI_LABEL_PATH = "Parts_BALOON/TXT_LOGINMESSAGE";

	protected List<Transform> stamps = new List<Transform>();

	private List<Transform> animStamps = new List<Transform>();

	private UILabel naviLabel;

	private CustomTypewriterEffect twEff;

	protected GameWebAPI.RespDataCM_LoginBonus.LoginBonus loginBonus;

	protected int loginCount;

	protected override void Awake()
	{
		base.Awake();
		this.loginBonus = this.GetLoginBonus();
		if (this.loginBonus == null)
		{
			return;
		}
		this.loginCount = this.loginBonus.loginCount;
		this.GetStamps();
	}

	protected void Start()
	{
		if (this.loginBonus == null)
		{
			return;
		}
		int num = 0;
		foreach (Transform transform in this.stamps)
		{
			GameObject gameObject = transform.gameObject;
			this.InitStamps(gameObject, num);
			if (this.IsAnimationStamp(num))
			{
				this.animStamps.Add(transform);
			}
			num++;
		}
		this.naviLabel = base.transform.Find(this.NAVI_LABEL_PATH).gameObject.GetComponent<UILabel>();
		this.naviLabel.enabled = true;
		this.twEff = this.naviLabel.GetComponent<CustomTypewriterEffect>();
		this.twEff.customText = this.GetNaviMassage(this.REWARD_TODAY) + "\n" + this.GetNaviMassage(this.REWARD_NEXT);
		this.twEff.enabled = false;
		base.StartCoroutine(this.TextAnimationDelay());
	}

	protected override void WindowOpened()
	{
		if (this.loginBonus == null)
		{
			this.ClosePanel(true);
			return;
		}
		base.WindowOpened();
	}

	private IEnumerator TextAnimationDelay()
	{
		yield return new WaitForSeconds(1f);
		this.twEff.enabled = true;
		yield break;
	}

	protected virtual GameWebAPI.RespDataCM_LoginBonus.LoginBonus GetLoginBonus()
	{
		return null;
	}

	protected virtual bool IsAnimationStamp(int listIndex)
	{
		return false;
	}

	protected void SetStamps()
	{
		int num = 1;
		string name = this.TODAY_STAMP_PREF_NAME + num;
		Transform transform = base.transform.Find(name);
		while (transform != null)
		{
			name = this.TODAY_STAMP_PREF_NAME + num;
			this.stamps.Add(transform);
			num++;
			transform = base.transform.Find(name);
		}
	}

	protected virtual void GetStamps()
	{
	}

	protected virtual void InitStamps(GameObject go, int index)
	{
	}

	protected string GetNaviMassage(int type)
	{
		string text = string.Empty;
		GameWebAPI.RespDataCM_LoginBonus.LoginReward[] array = (type != this.REWARD_TODAY) ? this.loginBonus.nextRewardList : this.loginBonus.rewardList;
		if (array == null)
		{
			return text;
		}
		foreach (GameWebAPI.RespDataCM_LoginBonus.LoginReward reward in array)
		{
			if (string.IsNullOrEmpty(text))
			{
				if (type == 0)
				{
					text += this.CreateLoginRewardMessage(StringMaster.GetString("LoginBonus-01"), reward);
				}
				else
				{
					text += this.CreateLoginRewardMessage(StringMaster.GetString("LoginBonus-02"), reward);
				}
			}
			else
			{
				text += this.CreateLoginRewardMessage(StringMaster.GetString("LoginBonus-03"), reward);
			}
		}
		if (type == 0)
		{
			text += StringMaster.GetString("LoginBonus-04");
		}
		else
		{
			text += StringMaster.GetString("LoginBonus-05");
		}
		return text;
	}

	private string CreateLoginRewardMessage(string format, GameWebAPI.RespDataCM_LoginBonus.LoginReward reward)
	{
		int num = int.Parse(reward.assetCategoryId);
		string arg = string.Empty;
		MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)num;
		switch (assetCategory)
		{
		case MasterDataMng.AssetCategory.GATHA_TICKET:
			arg = StringMaster.GetString("SystemCountSheets");
			goto IL_86;
		case MasterDataMng.AssetCategory.MULTI_ITEM:
		case MasterDataMng.AssetCategory.MEAT:
		case MasterDataMng.AssetCategory.SOUL:
		case MasterDataMng.AssetCategory.NO_DATA_ID:
			break;
		default:
			switch (assetCategory)
			{
			case MasterDataMng.AssetCategory.MONSTER:
				arg = StringMaster.GetString("SystemCountBody");
				goto IL_86;
			case MasterDataMng.AssetCategory.DIGI_STONE:
			case MasterDataMng.AssetCategory.ITEM:
				break;
			case MasterDataMng.AssetCategory.LINK_POINT:
			case MasterDataMng.AssetCategory.TIP:
			case MasterDataMng.AssetCategory.EXP:
				goto IL_86;
			default:
				goto IL_86;
			}
			break;
		}
		arg = StringMaster.GetString("SystemCountNumber");
		IL_86:
		return string.Format(format, DataMng.Instance().GetAssetTitle(reward.assetCategoryId, reward.assetValue), reward.assetNum, arg);
	}
}
