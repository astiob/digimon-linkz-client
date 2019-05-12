using System;
using UnityEngine;

public class CMD_LoginAnimator : CMD_LoginBase
{
	private LoginBonusParam loginBonusParam;

	private Animator animator;

	private GameObject rewardUI;

	[SerializeField]
	private UITexture backGround;

	[SerializeField]
	private GameObject iconBase;

	[SerializeField]
	private GameObject rewardEffect;

	[SerializeField]
	private string rewardUIName = "UISPR_GET_";

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

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.HideDLG();
		Action<UnityEngine.Object> actEnd = delegate(UnityEngine.Object result)
		{
			if (result != null)
			{
				this.loginBonusParam = ((GameObject)result).GetComponent<LoginBonusParam>();
				this.ShowDLG();
				this.Show(f, sizeX, sizeY, aT);
				this.SetUp();
			}
			else
			{
				f(0);
				this.ClosePanel(false);
			}
		};
		AssetDataMng.Instance().LoadObjectASync("LoginBonus/" + this.loginBonus.loginBonusId + "/LoginBonus", actEnd);
	}

	private void SetUp()
	{
		this.SetUpWindow();
		this.SetUpIcon();
		this.SetUpAnimator();
	}

	private void SetUpWindow()
	{
		TextureManager.instance.Load(CMD_LoginCampaign.GetBgPathForFTP(this.loginBonus.backgroundImg), delegate(Texture2D texture)
		{
			if (texture != null)
			{
				this.backGround.mainTexture = texture;
			}
		}, 30f, true);
	}

	private void SetUpIcon()
	{
		string b = this.rewardUIName + this.loginBonus.loginCount;
		this.iconBase.SetActive(false);
		for (int i = 0; i < this.loginBonusParam.maxLoginCount; i++)
		{
			int num = i + 1;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.iconBase);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = this.rewardUIName + num;
			gameObject.SetActive(num < this.loginBonus.loginCount);
			if (gameObject.name == b)
			{
				this.rewardUI = gameObject;
			}
		}
	}

	private void SetUpAnimator()
	{
		this.animator = base.gameObject.AddComponent<Animator>();
		this.animator.runtimeAnimatorController = this.loginBonusParam.animatorController;
	}

	protected override void Update()
	{
		base.Update();
		if (this.rewardEffect != null && this.rewardUI != null)
		{
			this.rewardEffect.transform.position = this.rewardUI.transform.position;
		}
	}
}
