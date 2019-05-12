using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITweenFrameController : MonoBehaviour
{
	private List<float> mathList;

	private int frameCT;

	private Vector3 vScale;

	private void Awake()
	{
		this.frameCT = 0;
		this.mathList = new List<float>();
		this.vScale = Vector3.zero;
		this.MakeSpringTable();
	}

	private void MakeSpring(List<float> mList, float delay, float time)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", 0f);
		hashtable.Add("to", 1f);
		hashtable.Add("time", time);
		hashtable.Add("onupdate", "UpdateRate");
		hashtable.Add("easetype", iTween.EaseType.spring);
		hashtable.Add("oncomplete", "RateEnd");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void UpdateRate(float math)
	{
		this.mathList.Add(math);
	}

	private void RateEnd(int i)
	{
	}

	private void MakeSpringTable()
	{
		this.mathList.Add(0f);
		this.mathList.Add(0f);
		this.mathList.Add(0.1504258f);
		this.mathList.Add(0.281589f);
		this.mathList.Add(0.3973907f);
		this.mathList.Add(0.5015699f);
		this.mathList.Add(0.597003f);
		this.mathList.Add(0.6852779f);
		this.mathList.Add(0.7664802f);
		this.mathList.Add(0.8391378f);
		this.mathList.Add(0.9003115f);
		this.mathList.Add(0.9459141f);
		this.mathList.Add(0.9714976f);
		this.mathList.Add(0.973895f);
		this.mathList.Add(0.9539804f);
		this.mathList.Add(0.919872f);
		this.mathList.Add(0.8877106f);
		this.mathList.Add(0.8751218f);
		this.mathList.Add(0.8864f);
		this.mathList.Add(0.9037964f);
		this.mathList.Add(0.9071547f);
		this.mathList.Add(0.9f);
	}

	public float GetSpringMath()
	{
		if (this.mathList != null)
		{
			if (this.frameCT >= this.mathList.Count)
			{
				this.frameCT = this.mathList.Count - 1;
			}
			float result = this.mathList[this.frameCT];
			this.frameCT += 2;
			return result;
		}
		return 0f;
	}

	private void Update()
	{
		float springMath = this.GetSpringMath();
		this.vScale.x = springMath;
		this.vScale.y = springMath;
		this.vScale.z = 1f;
		base.gameObject.transform.localScale = this.vScale;
	}
}
