using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketGashaController : MonoBehaviour
{
	[SerializeField]
	[Header("カードアニメフレーム間隔")]
	private int cardAnimIntervalFrame;

	[SerializeField]
	[Header("メインカメラ")]
	public Camera mainCam;

	[Header("2Dカメラ")]
	[SerializeField]
	public Camera camUI;

	[SerializeField]
	[Header("白フェード板")]
	private GUISprite spFade;

	[Header("白フェード速度")]
	[SerializeField]
	private float fadeSpeed = 0.05f;

	[SerializeField]
	[Header("スキップ・コリダー")]
	private BoxCollider skipCollider;

	[Header("カードエフェクト : 1:白, 2:黄色, 3:虹")]
	[SerializeField]
	private List<GameObject> goCardEfcList;

	[Header("カードアニメ : 1:白, 2:黄色, 3:虹")]
	[SerializeField]
	private List<GameObject> goCardAnimList;

	private UITexture ticketGashaTex;

	private RenderTexture renderTex;

	private Action<int> endCallBack;

	private int cardEfcCT;

	private int cardFrameCT;

	private List<GameObject> goCardList = new List<GameObject>();

	private int frameCT;

	private int frameLast = 80000;

	private bool fadeON;

	private int startFadeOutFrame;

	private void Awake()
	{
		base.name = "Cutscene";
	}

	public void SetTicketGashaTex(UITexture tex)
	{
		this.renderTex = new RenderTexture(1100, 800, 16);
		this.renderTex.antiAliasing = 2;
		this.ticketGashaTex = tex;
		this.ticketGashaTex.mainTexture = this.renderTex;
	}

	public Action<int> EndCallBack
	{
		set
		{
			this.endCallBack = value;
		}
	}

	public GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList[] UserTicketList { get; set; }

	private void Start()
	{
		if (this.UserTicketList == null)
		{
			this.UserTicketList = new GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList[10];
			for (int i = 0; i < this.UserTicketList.Length; i++)
			{
				this.UserTicketList[i] = new GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList();
				this.UserTicketList[i].effectType = (i % 3 + 1).ToString();
			}
		}
		this.startFadeOutFrame = this.cardAnimIntervalFrame * this.UserTicketList.Length + 10;
	}

	private void UpdateCardEfc()
	{
		if (this.UserTicketList != null)
		{
			if (this.cardFrameCT % this.cardAnimIntervalFrame == 0 && this.cardEfcCT < this.UserTicketList.Length)
			{
				if (this.UserTicketList.Length == 1)
				{
					this.KickEfc(this.UserTicketList[this.cardEfcCT].effectType, false);
				}
				else
				{
					this.KickEfc(this.UserTicketList[this.cardEfcCT].effectType, true);
				}
				this.cardEfcCT++;
			}
			this.cardFrameCT++;
		}
	}

	private void KickEfc(string effectType, bool useRand = true)
	{
		int index = int.Parse(effectType) - 1;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goCardAnimList[index]);
		Vector3 localPosition = gameObject.transform.localPosition;
		localPosition.z = 110f;
		if (useRand)
		{
			float num = UnityEngine.Random.Range(-5f, 5f);
			float num2 = UnityEngine.Random.Range(-2f, 2f);
			localPosition.x += num;
			localPosition.y += num2;
		}
		Vector3 localScale = gameObject.transform.localScale;
		Quaternion localRotation = gameObject.transform.localRotation;
		gameObject.transform.parent = this.mainCam.gameObject.transform;
		gameObject.transform.localPosition = localPosition;
		gameObject.transform.localScale = localScale;
		gameObject.transform.localRotation = localRotation;
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.goCardEfcList[index]);
		localPosition = gameObject2.transform.localPosition;
		localPosition.z += 0.1f;
		localScale = gameObject2.transform.localScale;
		localRotation = gameObject2.transform.localRotation;
		using (IEnumerator enumerator = gameObject.transform.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Transform parent = (Transform)enumerator.Current;
				gameObject2.transform.parent = parent;
			}
		}
		gameObject2.transform.localPosition = localPosition;
		gameObject2.transform.localScale = localScale;
		gameObject2.transform.localRotation = localRotation;
		this.goCardList.Add(gameObject);
	}

	private void DestroyEfc()
	{
		for (int i = 0; i < this.goCardList.Count; i++)
		{
			UnityEngine.Object.Destroy(this.goCardList[i]);
		}
	}

	private void Update()
	{
		this.UpdateCardEfc();
		this.UpdateSkip();
		if (this.frameCT == this.startFadeOutFrame)
		{
			this.fadeON = true;
		}
		if (this.fadeON)
		{
			Color color = this.spFade.color;
			color.a += this.fadeSpeed;
			if (color.a >= 1f)
			{
				color.a = 1f;
				this.fadeON = false;
				this.frameLast = this.frameCT;
				this.DestroyEfc();
				this.mainCam.targetTexture = this.renderTex;
				if (this.endCallBack != null)
				{
					this.endCallBack(0);
				}
			}
			this.spFade.color = color;
		}
		if (this.frameCT == this.frameLast + 2)
		{
			this.mainCam.targetTexture = null;
			UnityEngine.Object.Destroy(base.gameObject);
			GC.Collect();
			Resources.UnloadUnusedAssets();
		}
		this.frameCT++;
	}

	private void DoSkip()
	{
		if (this.frameCT < this.startFadeOutFrame)
		{
			this.frameCT = this.startFadeOutFrame;
			this.fadeSpeed *= 4f;
		}
	}

	private void UpdateSkip()
	{
		List<Touch> touch = this.GetTouch();
		if (touch.Count > 0)
		{
			Ray ray = this.camUI.ScreenPointToRay(new Vector3(touch[0].position.x, touch[0].position.y, 0f));
			RaycastHit[] array = Physics.RaycastAll(ray);
			if (array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].collider == this.skipCollider)
					{
						this.DoSkip();
						global::Debug.Log("======================================== CUT SCENE SKIP !! ");
					}
				}
			}
		}
	}

	private List<Touch> GetTouch()
	{
		List<Touch> list = new List<Touch>();
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				list.Add(touch);
			}
		}
		return list;
	}
}
