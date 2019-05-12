using System;
using System.Collections.Generic;
using UnityEngine;

public class TicketEfc : MonoBehaviour
{
	[Header("サムネイル TEX")]
	[SerializeField]
	public UITexture ngTICKET_THUMB;

	[SerializeField]
	[Header("チケット TEX")]
	public UITexture ngTICKET;

	[Header("パーティクル OBJ")]
	[SerializeField]
	public GameObject goPARTICLE;

	[Header("NEW スプライト")]
	[SerializeField]
	public UISprite spNew;

	[SerializeField]
	[Header("枚数ラベル")]
	public UILabel ngTXT_TICKET_NUM;

	private int playFrameCT;

	private bool isPlaying;

	private List<Vector3> vvList;

	private void Awake()
	{
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
			if (++this.playFrameCT > 8000)
			{
				this.playFrameCT = 8000;
			}
		}
	}

	public void Play()
	{
		if (!this.isPlaying)
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goPARTICLE);
				gameObject.SetActive(true);
				Vector3 localScale = gameObject.transform.localScale;
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = this.vvList[i];
				gameObject.transform.localScale = localScale;
			}
			this.isPlaying = true;
		}
	}
}
