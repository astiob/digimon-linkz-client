using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TicketEfc : MonoBehaviour
{
	[Header("サムネイル TEX")]
	[SerializeField]
	public UITexture ngTICKET_THUMB;

	[SerializeField]
	[Header("チケット TEX")]
	private UITexture ngTICKET;

	[Header("パーティクル OBJ")]
	[SerializeField]
	private GameObject goPARTICLE;

	[SerializeField]
	[Header("NEW スプライト")]
	public UISprite spNew;

	[SerializeField]
	[Header("枚数ラベル")]
	public UILabel ngTXT_TICKET_NUM;

	private int playFrameCT;

	private bool isPlaying;

	private ParticleSystem[] ticketShowEffectList;

	private List<Vector3> vvList;

	private void Awake()
	{
		global::Debug.Assert(null != this.ngTXT_TICKET_NUM, "TicketEfc.ngTXT_TICKET_NUM == null");
		this.playFrameCT = 0;
		this.isPlaying = false;
		this.goPARTICLE.SetActive(false);
		this.ngTICKET.gameObject.SetActive(true);
		this.ngTICKET_THUMB.gameObject.SetActive(false);
		this.vvList = new List<Vector3>();
		Vector3 item = new Vector3(-40f, 0f, -1f);
		this.vvList.Add(item);
		item = new Vector3(0f, 0f, -1f);
		this.vvList.Add(item);
		item = new Vector3(40f, 0f, -1f);
		this.vvList.Add(item);
	}

	private void Update()
	{
		if (this.isPlaying)
		{
			if (this.playFrameCT == 20)
			{
				this.ngTICKET.gameObject.SetActive(false);
				this.ngTICKET_THUMB.gameObject.SetActive(true);
			}
			for (int i = 0; i < this.ticketShowEffectList.Length; i++)
			{
				if (null != this.ticketShowEffectList[i] && this.ticketShowEffectList[i].gameObject.activeSelf && !this.ticketShowEffectList[i].isPlaying)
				{
					this.ticketShowEffectList[i].gameObject.SetActive(false);
				}
			}
			this.playFrameCT++;
			if (8000 < this.playFrameCT)
			{
				this.playFrameCT = 8000;
			}
		}
	}

	public void Play()
	{
		if (!this.isPlaying)
		{
			this.ticketShowEffectList = new ParticleSystem[3];
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goPARTICLE);
				gameObject.SetActive(true);
				Vector3 localScale = gameObject.transform.localScale;
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = this.vvList[i];
				gameObject.transform.localScale = localScale;
				this.ticketShowEffectList[i] = gameObject.GetComponent<ParticleSystem>();
			}
			this.isPlaying = true;
		}
	}
}
