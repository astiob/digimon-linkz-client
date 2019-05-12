using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeEfcDirector : MonoBehaviour
{
	private List<PrizeEfcDirector.EFC_TYPE> efcTypeList;

	private int efcTypeIDX;

	[SerializeField]
	[Header("フェードアウトのアルファ値")]
	private float valueA_Min;

	[SerializeField]
	[Header("フェードインのアルファ値")]
	private float valueA_Max = 1f;

	[SerializeField]
	[Header("フェードイン時間")]
	private float fadeInTime = 0.3f;

	[Header("オンしている時間")]
	[SerializeField]
	private float stayTime = 0.8f;

	[Header("フェードアウト時間")]
	[SerializeField]
	private float fadeOutTime = 0.3f;

	private float valueA;

	public float GetNowValue()
	{
		return this.valueA;
	}

	public PrizeEfcDirector.EFC_TYPE GetNowType()
	{
		if (this.efcTypeList.Count <= 0)
		{
			return PrizeEfcDirector.EFC_TYPE.NONE;
		}
		return this.efcTypeList[this.efcTypeIDX];
	}

	public int GetTypeCount()
	{
		return this.efcTypeList.Count;
	}

	public void SetTypeParam(List<MonsterData> mdL)
	{
		this.efcTypeIDX = 0;
		this.efcTypeList = new List<PrizeEfcDirector.EFC_TYPE>();
		for (int i = 0; i < mdL.Count; i++)
		{
			if (mdL[i].commonSkillM != null)
			{
				int num = int.Parse(mdL[i].commonSkillM.rank);
				if (num >= ConstValue.GASHA_INHARITANCE_PRIZE_LEVEL)
				{
					this.AddNewType(PrizeEfcDirector.EFC_TYPE.INHARITANCE);
				}
			}
			if (mdL[i].leaderSkillM != null)
			{
				int num2 = int.Parse(mdL[i].leaderSkillM.rank);
				if (num2 >= ConstValue.GASHA_LEADERSKILL_PRIZE_LEVEL)
				{
					this.AddNewType(PrizeEfcDirector.EFC_TYPE.LEADER_SKILL);
				}
			}
			if (mdL[i].GetHaveMedal().Count >= 1)
			{
				this.AddNewType(PrizeEfcDirector.EFC_TYPE.MEDAL);
			}
		}
		this.efcTypeList.Sort(new Comparison<PrizeEfcDirector.EFC_TYPE>(this.CompareEfcType));
		this.ResetEfc();
	}

	private int CompareEfcType(PrizeEfcDirector.EFC_TYPE xx, PrizeEfcDirector.EFC_TYPE yy)
	{
		if (xx < yy)
		{
			return -1;
		}
		if (xx > yy)
		{
			return 1;
		}
		return 0;
	}

	public void KickEfc()
	{
		this.valueA = this.valueA_Min;
		iTween.Stop(base.gameObject);
		iTween.Init(base.gameObject);
		this.StartUp_VA();
	}

	private void ResetEfc()
	{
		this.valueA = this.valueA_Min;
		iTween.Stop(base.gameObject);
		iTween.Init(base.gameObject);
	}

	private void AddNewType(PrizeEfcDirector.EFC_TYPE type)
	{
		for (int i = 0; i < this.efcTypeList.Count; i++)
		{
			if (this.efcTypeList[i] == type)
			{
				return;
			}
		}
		this.efcTypeList.Add(type);
	}

	private void StartUp_VA()
	{
		this.valueA = this.valueA_Min;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", this.valueA);
		hashtable.Add("to", this.valueA_Max);
		hashtable.Add("time", this.fadeInTime);
		hashtable.Add("onupdate", "UpdateValueA");
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("oncomplete", "EndValueA_WT");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void UpdateValueA(float vl)
	{
		this.valueA = vl;
	}

	private void EndValueA_WT(int id)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", this.valueA);
		hashtable.Add("to", this.valueA);
		hashtable.Add("time", this.stayTime);
		hashtable.Add("onupdate", "UpdateValueA");
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("oncomplete", "EndValueA_CHG");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void EndValueA_CHG(int id)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", this.valueA_Max);
		hashtable.Add("to", this.valueA_Min);
		hashtable.Add("time", this.fadeOutTime);
		hashtable.Add("onupdate", "UpdateValueA");
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("oncomplete", "NextUp_VA");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void NextUp_VA(int id)
	{
		this.efcTypeIDX++;
		if (this.efcTypeIDX >= this.efcTypeList.Count)
		{
			this.efcTypeIDX = 0;
		}
		this.StartUp_VA();
	}

	public enum EFC_TYPE
	{
		NONE = -1,
		INHARITANCE,
		LEADER_SKILL,
		MEDAL
	}
}
