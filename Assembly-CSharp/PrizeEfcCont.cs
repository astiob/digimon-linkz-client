using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeEfcCont : MonoBehaviour
{
	[SerializeField]
	private UISprite spInharitance;

	[SerializeField]
	private UISprite spLeaderSkill;

	[SerializeField]
	private List<UISprite> spMedalList;

	[SerializeField]
	private UIWidget wdgMedalRoot;

	[SerializeField]
	private UISprite outerLuster;

	[SerializeField]
	private UISprite innerLuster;

	[SerializeField]
	private UISpriteAnimation shiningEffect;

	private List<UISpriteAnimation> shiningList = new List<UISpriteAnimation>();

	private List<Animation> medalAnimeList = new List<Animation>();

	public PrizeEfcDirector prizeEfcDir;

	private int PERFECT_HIT_NUM = 3;

	private Color nCol = new Color(1f, 1f, 1f, 0f);

	public MonsterData Data { get; set; }

	private void Update()
	{
		this.UpdateEfc();
	}

	public void SetParam()
	{
		this.SetupMedalAndEfc();
	}

	private void SetupMedalAndEfc()
	{
		List<string> strList = this.CheckHaveMedal(this.Data.userMonster);
		int num = 0;
		this.spInharitance.gameObject.SetActive(false);
		if (this.Data.commonSkillM != null)
		{
			int num2 = int.Parse(this.Data.commonSkillM.rank);
			if (num2 >= ConstValue.GASHA_INHARITANCE_PRIZE_LEVEL)
			{
				this.spInharitance.gameObject.SetActive(true);
				num++;
			}
		}
		this.spLeaderSkill.gameObject.SetActive(false);
		if (this.Data.leaderSkillM != null)
		{
			int num3 = int.Parse(this.Data.leaderSkillM.rank);
			if (num3 >= ConstValue.GASHA_LEADERSKILL_PRIZE_LEVEL)
			{
				this.spLeaderSkill.gameObject.SetActive(true);
				num++;
			}
		}
		int num4 = this.CheckMedalNum(strList, "Common02_Talent_Gold");
		int num5 = this.CheckMedalNum(strList, "Common02_Talent_Silver");
		for (int i = 0; i < this.spMedalList.Count; i++)
		{
			this.spMedalList[i].gameObject.SetActive(false);
		}
		bool flag = false;
		int num6 = 0;
		if (num4 > 0)
		{
			flag = true;
			this.spMedalList[num6].spriteName = "Common02_Talent_Gold";
			this.spMedalList[num6].gameObject.SetActive(true);
			Animation component = this.spMedalList[num6].gameObject.GetComponent<Animation>();
			this.medalAnimeList.Add(component);
			num6++;
		}
		if (num5 > 0)
		{
			flag = true;
			this.spMedalList[num6].spriteName = "Common02_Talent_Silver";
			this.spMedalList[num6].gameObject.SetActive(true);
			Animation component2 = this.spMedalList[num6].gameObject.GetComponent<Animation>();
			foreach (object obj in this.spMedalList[num6].gameObject.transform)
			{
				Transform transform = (Transform)obj;
				UILabel component3 = transform.gameObject.GetComponent<UILabel>();
				if (component3 != null)
				{
					component3.gameObject.SetActive(true);
					component3.text = StringMaster.GetString("MissionRewardKakeru") + num5.ToString();
					break;
				}
			}
			this.medalAnimeList.Add(component2);
			num6++;
		}
		if (flag)
		{
			num++;
		}
		this.shiningEffect.gameObject.SetActive(false);
		int growStep = int.Parse(this.Data.monsterMG.growStep);
		if (MonsterGrowStepData.IsPerfectScope(growStep) || MonsterGrowStepData.IsUltimateScope(growStep) || num >= this.PERFECT_HIT_NUM)
		{
			this.outerLuster.gameObject.SetActive(true);
			this.innerLuster.gameObject.SetActive(true);
		}
		else
		{
			this.outerLuster.gameObject.SetActive(false);
			this.innerLuster.gameObject.SetActive(false);
		}
		if (this.shiningList.Count > 0)
		{
			base.StartCoroutine(this.LoopMedalShine());
		}
	}

	private void UpdateEfc()
	{
		if (this.prizeEfcDir != null)
		{
			PrizeEfcDirector.EFC_TYPE nowType = this.prizeEfcDir.GetNowType();
			float a = this.prizeEfcDir.GetNowValue();
			int typeCount = this.prizeEfcDir.GetTypeCount();
			if (typeCount <= 1)
			{
				a = 1f;
			}
			this.spInharitance.color = this.nCol;
			this.spLeaderSkill.color = this.nCol;
			this.wdgMedalRoot.color = this.nCol;
			Color color = new Color(1f, 1f, 1f, a);
			switch (nowType)
			{
			case PrizeEfcDirector.EFC_TYPE.INHARITANCE:
				this.spInharitance.color = color;
				break;
			case PrizeEfcDirector.EFC_TYPE.LEADER_SKILL:
				this.spLeaderSkill.color = color;
				break;
			case PrizeEfcDirector.EFC_TYPE.MEDAL:
				this.wdgMedalRoot.color = color;
				break;
			}
		}
	}

	private List<string> CheckHaveMedal(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList UserMonsterData)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		list2.Add(UserMonsterData.hpAbilityFlg);
		list2.Add(UserMonsterData.attackAbilityFlg);
		list2.Add(UserMonsterData.defenseAbilityFlg);
		list2.Add(UserMonsterData.spAttackAbilityFlg);
		list2.Add(UserMonsterData.spDefenseAbilityFlg);
		list2.Add(UserMonsterData.speedAbilityFlg);
		int num = 0;
		foreach (string s in list2)
		{
			if (!int.TryParse(s, out num))
			{
				global::Debug.LogError("Error : int.TryParse for userMonsterData of AbilityFlg");
			}
			else
			{
				int num2 = num;
				if (num2 != 1)
				{
					if (num2 == 2)
					{
						list.Add("Common02_Talent_Silver");
					}
				}
				else
				{
					list.Add("Common02_Talent_Gold");
				}
			}
		}
		if (list.Count > 1)
		{
			list.Sort(delegate(string a, string b)
			{
				if (a == "Common02_Talent_Gold" && b != "Common02_Talent_Gold")
				{
					return -1;
				}
				if (a != "Common02_Talent_Gold" && b == "Common02_Talent_Gold")
				{
					return 1;
				}
				return 0;
			});
		}
		return list;
	}

	private int CheckMedalNum(List<string> strList, string strMedal)
	{
		int num = 0;
		for (int i = 0; i < strList.Count; i++)
		{
			if (strList[i] == strMedal)
			{
				num++;
			}
		}
		return num;
	}

	private IEnumerator LoopMedalShine()
	{
		for (;;)
		{
			foreach (Animation anime in this.medalAnimeList)
			{
				anime.Play();
			}
			foreach (UISpriteAnimation anim in this.shiningList)
			{
				anim.GetComponent<UISprite>().spriteName = "Common02_Thum_Luster1";
				anim.gameObject.SetActive(true);
				anim.Play();
			}
			float elapsed = 0f;
			while (this.shiningList.Count > 0 && this.shiningList[0].isPlaying)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
			foreach (UISpriteAnimation anim2 in this.shiningList)
			{
				anim2.gameObject.SetActive(false);
			}
			yield return new WaitForSeconds(2f - elapsed);
			foreach (Animation anime2 in this.medalAnimeList)
			{
				anime2.Stop();
			}
		}
		yield break;
	}
}
