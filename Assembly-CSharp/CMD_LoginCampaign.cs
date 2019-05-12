using System;
using UnityEngine;

public class CMD_LoginCampaign : CMD_LoginBase
{
	public string BG_PATH = "UITEX_CAMPAIGNLOGIN";

	public string BG_TEXTURE_PATH = "CampaignLogin/thumb";

	private string PAST_STAMP_PREF_NAME = "UISPR_GET_";

	private UITexture bg;

	protected override void Awake()
	{
		base.Awake();
	}

	protected new void Start()
	{
		base.Start();
		this.bg = base.transform.Find(this.BG_PATH).GetComponent<UITexture>();
		TextureManager.instance.Load(CMD_LoginCampaign.GetBgPathForFTP(this.loginBonus.backgroundImg), delegate(Texture2D texture)
		{
			if (texture != null)
			{
				this.bg.mainTexture = texture;
			}
			else
			{
				NGUIUtil.ChangeUITextureFromFile(this.bg, this.BG_TEXTURE_PATH, false);
			}
		}, 30f, true);
	}

	protected override GameWebAPI.RespDataCM_LoginBonus.LoginBonus GetLoginBonus()
	{
		int showLoginBonusNumC = DataMng.Instance().ShowLoginBonusNumC;
		int num = DataMng.Instance().RespDataCM_LoginBonus.loginBonus.campaign.Length;
		if (num > 0)
		{
			return DataMng.Instance().RespDataCM_LoginBonus.loginBonus.campaign[showLoginBonusNumC];
		}
		return null;
	}

	protected override void GetStamps()
	{
		int num = 0;
		Transform transform;
		while (!this.IsAnimationStamp(num))
		{
			transform = base.transform.Find(this.PAST_STAMP_PREF_NAME + num);
			if (transform != null)
			{
				this.stamps.Add(transform);
			}
			num++;
		}
		transform = base.transform.Find(this.TODAY_STAMP_PREF_NAME + num);
		if (transform != null)
		{
			this.stamps.Add(transform);
		}
	}

	protected override void InitStamps(GameObject go, int index)
	{
		go.SetActive(index <= this.loginCount);
	}

	protected override bool IsAnimationStamp(int listIndex)
	{
		return listIndex == this.loginCount;
	}

	public static string GetBgPathForFTP(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return AssetDataMng.GetWebAssetImagePath() + "/login/";
		}
		return AssetDataMng.GetWebAssetImagePath() + "/login/" + path;
	}
}
