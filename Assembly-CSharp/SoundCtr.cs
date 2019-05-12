using System;
using System.Collections;
using UnityEngine;

public class SoundCtr : MonoBehaviour
{
	public string seName;

	public GameObject go;

	public AudioSource ads;

	public float value;

	public bool autoDel;

	public float time;

	public int selfVolume;

	public Action<int> act;

	private bool on;

	public void Play(bool flg)
	{
		this.on = flg;
		if (this.time == 0f)
		{
			if (this.on)
			{
				this.value = 1f;
				if (this.selfVolume >= 0)
				{
					this.ads.volume = this.value * (float)this.selfVolume * 0.1f;
				}
				else
				{
					this.ads.volume = this.value * SoundMng.Instance().VolumeSE * 0.1f;
				}
			}
			else
			{
				this.value = 0f;
				this.ads.volume = 0f;
			}
			if (this.act != null)
			{
				this.act(0);
			}
		}
		else
		{
			Hashtable hashtable = new Hashtable();
			if (flg)
			{
				this.value = 0f;
				this.ads.volume = 0f;
				hashtable.Add("from", 0f);
				hashtable.Add("to", 1f);
			}
			else
			{
				hashtable.Add("from", this.value);
				hashtable.Add("to", 0f);
				if (this.selfVolume >= 0)
				{
					this.ads.volume = this.value * (float)this.selfVolume * 0.1f;
				}
				else
				{
					this.ads.volume = this.value * SoundMng.Instance().VolumeSE * 0.1f;
				}
			}
			hashtable.Add("time", this.time);
			hashtable.Add("onupdate", "UpdateSnd");
			hashtable.Add("easetype", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "EndTime");
			hashtable.Add("oncompleteparams", 0);
			iTween.ValueTo(this.go, hashtable);
		}
		if (flg)
		{
			this.ads.Play();
		}
	}

	private void UpdateSnd(float v)
	{
		this.value = v;
		if (this.selfVolume >= 0)
		{
			this.ads.volume = this.value * (float)this.selfVolume * 0.1f;
		}
		else
		{
			this.ads.volume = this.value * SoundMng.Instance().VolumeSE * 0.1f;
		}
	}

	private void EndTime()
	{
		if (this.act != null)
		{
			this.act(0);
		}
	}

	private void Update()
	{
		if (!this.ads.loop)
		{
			if (this.autoDel)
			{
				if (!this.ads.isPlaying)
				{
					this.seName = string.Empty;
					this.ads.Stop();
					this.ads.clip = null;
				}
			}
			else if (!this.on && !this.ads.isPlaying)
			{
				this.seName = string.Empty;
				this.ads.Stop();
				this.ads.clip = null;
			}
		}
		else if (!this.on && this.value == 0f)
		{
			this.seName = string.Empty;
			this.ads.Stop();
			this.ads.clip = null;
		}
	}

	public void Stop()
	{
		this.seName = string.Empty;
		this.value = 0f;
		this.ads.Stop();
		this.ads.clip = null;
	}
}
