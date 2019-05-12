using System;
using System.Collections;
using UnityEngine;

public class AlphaTweener : MonoBehaviour
{
	private float valueA_Min = -1f;

	private float valueA_Max = 1f;

	private float changeTime = 0.4f;

	private float stayTime = 1.1f;

	private float valueA;

	private void Start()
	{
		this.StartUp_VA();
	}

	public float GetAlphaValue()
	{
		return this.valueA;
	}

	private void StartUp_VA()
	{
		this.valueA = 0f;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", this.valueA);
		hashtable.Add("to", this.valueA_Max);
		hashtable.Add("time", this.changeTime / 2f);
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
		if (this.valueA > 0.5f)
		{
			hashtable.Add("from", this.valueA_Max);
			hashtable.Add("to", this.valueA_Min);
		}
		else
		{
			hashtable.Add("from", this.valueA_Min);
			hashtable.Add("to", this.valueA_Max);
		}
		hashtable.Add("time", this.changeTime);
		hashtable.Add("onupdate", "UpdateValueA");
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("oncomplete", "EndValueA_WT");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}
}
