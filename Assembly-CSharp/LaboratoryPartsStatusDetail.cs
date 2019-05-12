using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryPartsStatusDetail : MonoBehaviour
{
	[SerializeField]
	private UISprite charaIcon;

	[SerializeField]
	private UISprite digitamaFrame;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList resistanceList;

	[SerializeField]
	private MonsterStatusList statusList;

	[SerializeField]
	private MonsterMedalList medalList;

	[SerializeField]
	private MonsterEggMedalList eggMedalList;

	[SerializeField]
	private UISprite eggArousalIcon;

	[SerializeField]
	private TweenAlpha eggArousalIconTween;

	public void SetActiveIcon(bool isActive)
	{
		this.charaIcon.gameObject.SetActive(isActive);
	}

	public GameObject GetCharaIconObject()
	{
		return this.charaIcon.gameObject;
	}

	private void ClearDigimonStatus()
	{
		if (null != this.monsterBasicInfo)
		{
			this.monsterBasicInfo.ClearMonsterData();
		}
		this.statusList.ClearValues();
		if (this.medalList != null)
		{
			this.medalList.SetActive(false);
		}
		if (null != this.resistanceList)
		{
			this.resistanceList.ClearValues();
		}
	}

	public void ShowMATInfo(MonsterData monsterData)
	{
		if (monsterData == null)
		{
			this.ClearDigimonStatus();
		}
		else
		{
			if (null != this.monsterBasicInfo)
			{
				this.monsterBasicInfo.SetMonsterData(monsterData);
			}
			this.statusList.SetValues(monsterData, false);
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(monsterData.monsterM.resistanceId);
			List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceList = MonsterResistanceData.GetUniqueResistanceList(monsterData.GetResistanceIdList());
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM values = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceList);
			this.resistanceList.SetValues(values);
			if (this.medalList != null)
			{
				this.medalList.SetValues(monsterData.userMonster);
			}
		}
	}

	public void ClearDigitamaStatus()
	{
		this.statusList.ClearEggCandidateMedalValues();
		if (this.eggMedalList != null)
		{
			this.eggMedalList.SetActive(false);
		}
		this.charaIcon.spriteName = "Common02_Thumbnail_none";
		this.ClearArousalValue();
	}

	public void SetDigitamaStatus(MonsterEggStatusInfo eggStatus)
	{
		if (eggStatus == null)
		{
			this.ClearDigitamaStatus();
		}
		else
		{
			this.charaIcon.spriteName = "Common02_Thumbnail_Question";
			this.digitamaFrame.spriteName = "Common02_Thumbnail_wakuQ";
			this.statusList.FriendshipLabel.text = "0";
			this.statusList.LuckLabel.text = eggStatus.luck;
			if (this.eggMedalList != null)
			{
				this.eggMedalList.SetValues(eggStatus);
				this.statusList.SetEggCandidateMedalValues(eggStatus);
			}
			if (this.eggArousalIcon != null)
			{
				this.SetArousalValue(eggStatus.isArousal, eggStatus.isReturn, eggStatus.rare);
			}
		}
	}

	private void SetArousalValue(bool isArousal, bool isReturn, string rarity)
	{
		int num = int.Parse(rarity);
		if (isArousal)
		{
			num++;
			this.eggArousalIconTween.style = UITweener.Style.PingPong;
			this.eggArousalIconTween.PlayForward();
		}
		else if (isReturn)
		{
			num--;
			this.eggArousalIconTween.style = UITweener.Style.PingPong;
			this.eggArousalIconTween.PlayForward();
		}
		else
		{
			this.eggArousalIconTween.tweenFactor = 0f;
			this.eggArousalIconTween.style = UITweener.Style.Once;
			this.eggArousalIconTween.PlayReverse();
		}
		string arousalSpriteName = MonsterDetailUtil.GetArousalSpriteName(num);
		if (string.IsNullOrEmpty(arousalSpriteName))
		{
			this.eggArousalIcon.gameObject.SetActive(false);
		}
		else
		{
			this.eggArousalIcon.spriteName = arousalSpriteName;
			this.eggArousalIcon.gameObject.SetActive(true);
		}
	}

	private void ClearArousalValue()
	{
		this.eggArousalIcon.gameObject.SetActive(false);
	}
}
