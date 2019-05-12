using Master;
using Quest;
using System;
using UnityEngine;

public sealed class CMD_FirstLinkBonus : CMD
{
	[SerializeField]
	private UILabel firstClearMessage;

	[SerializeField]
	private GameObject rollCircle;

	[SerializeField]
	private GameObject rollInnerCircle;

	protected override void Awake()
	{
		base.Awake();
		GameWebAPI.RespData_WorldMultiResultInfoLogic respData_WorldMultiResultInfoLogic = ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic;
		GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LinkBonus[] linkBonus = respData_WorldMultiResultInfoLogic.dungeonReward.linkBonus;
		if (linkBonus == null || linkBonus.Length == 0)
		{
			GUICollider.DisableAllCollider("CMD_FirstClear");
			return;
		}
		int num = linkBonus.Length;
		this.firstClearMessage.text = string.Empty;
		for (int i = 0; i < num; i++)
		{
			GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LinkBonus linkBonus2 = linkBonus[i];
			int type = linkBonus2.type;
			if (linkBonus2.reward != null && linkBonus2.reward.Length != 0)
			{
				for (int j = 0; j < linkBonus2.reward.Length; j++)
				{
					GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LinkBonus.LinkBonusReward linkBonusReward = linkBonus2.reward[j];
					string assetTitle = DataMng.Instance().GetAssetTitle(linkBonusReward.assetCategoryId, linkBonusReward.assetValue.ToString());
					int num2 = type;
					if (num2 != 1)
					{
						if (num2 == 2)
						{
							UILabel uilabel = this.firstClearMessage;
							uilabel.text += string.Format(StringMaster.GetString("FirstLinkBonusFriend"), linkBonusReward.linkNum, assetTitle, linkBonusReward.assetNum);
						}
					}
					else
					{
						UILabel uilabel2 = this.firstClearMessage;
						uilabel2.text += string.Format(StringMaster.GetString("FirstLinkBonus"), linkBonusReward.linkNum, assetTitle, linkBonusReward.assetNum);
					}
					UILabel uilabel3 = this.firstClearMessage;
					uilabel3.text += "\n";
				}
			}
		}
		GUICollider.DisableAllCollider("CMD_FirstClear");
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		GUICollider.EnableAllCollider("CMD_FirstClear");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_305", 0f, false, true, null, -1);
	}

	protected override void Update()
	{
		base.Update();
		if (this.rollCircle)
		{
			this.rollCircle.transform.Rotate(new Vector3(0f, 0f, 1f));
		}
		if (this.rollInnerCircle)
		{
			this.rollInnerCircle.transform.Rotate(new Vector3(0f, 0f, -1f));
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
