using System;
using System.Collections.Generic;
using UnityEngine;

public class ChipGashaController : MonoBehaviour
{
	private Action<int> endCallBack;

	[Header("動作ロケーターのゲームオブジェクト、のリスト")]
	[SerializeField]
	[Header("MAX １０個")]
	private List<GameObject> goLocatorList;

	[Header("MAX １０個")]
	[SerializeField]
	[Header("動作ロケーターライト用のゲームオブジェクト、のリスト")]
	private List<GameObject> goLightLocatorList;

	[Header("素材のゲームオブジェクト、のリスト、順番は以下")]
	[SerializeField]
	[Header("ライト")]
	private GameObject goPartsLight;

	[Header("青→青")]
	[SerializeField]
	private GameObject goPartsBlue;

	[Header("青→黄")]
	[SerializeField]
	private GameObject goPartsYellow;

	[SerializeField]
	[Header("青→虹")]
	private GameObject goPartsRainbow;

	[SerializeField]
	[Header("フェードアウト開始フレーム")]
	private int startFadeOutFrame;

	[SerializeField]
	[Header("メインカメラ")]
	public Camera mainCam;

	[SerializeField]
	[Header("2Dカメラ")]
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

	private int frameCT;

	private int frameLast = 80000;

	private bool fadeON;

	private UITexture chipGashaTex;

	private RenderTexture renderTex;

	public Action<int> EndCallBack
	{
		set
		{
			this.endCallBack = value;
		}
	}

	public GameWebAPI.RespDataGA_ExecChip.UserAssetList[] UserAssetList { get; set; }

	private void Awake()
	{
		base.name = "Cutscene";
	}

	private void Start()
	{
		this.ApplyParts();
	}

	public void SetChipGashaTex(UITexture tex)
	{
		this.renderTex = new RenderTexture(1200, 1200, 16);
		this.renderTex.antiAliasing = 2;
		this.chipGashaTex = tex;
		this.chipGashaTex.mainTexture = this.renderTex;
		this.chipGashaTex.width = this.renderTex.width;
		this.chipGashaTex.height = this.renderTex.height;
	}

	private void Update()
	{
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
			this.InActiveAllParts();
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

	private void ApplyParts()
	{
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		if (this.UserAssetList == null)
		{
			System.Random random = new System.Random();
			this.UserAssetList = new GameWebAPI.RespDataGA_ExecChip.UserAssetList[10];
			for (int i = 0; i < this.UserAssetList.Length; i++)
			{
				this.UserAssetList[i] = new GameWebAPI.RespDataGA_ExecChip.UserAssetList();
				int num = random.Next(1, 4);
				this.UserAssetList[i].effectType = num.ToString();
			}
		}
		for (int i = 0; i < this.UserAssetList.Length; i++)
		{
			string effectType = this.UserAssetList[i].effectType;
			GameObject item = null;
			string text = effectType;
			switch (text)
			{
			case "1":
				item = UnityEngine.Object.Instantiate<GameObject>(this.goPartsBlue);
				break;
			case "2":
				item = UnityEngine.Object.Instantiate<GameObject>(this.goPartsYellow);
				break;
			case "3":
				item = UnityEngine.Object.Instantiate<GameObject>(this.goPartsRainbow);
				break;
			}
			list.Add(item);
			item = UnityEngine.Object.Instantiate<GameObject>(this.goPartsLight);
			list2.Add(item);
		}
		Quaternion localRotation = Quaternion.Euler(0f, 0f, 0f);
		Vector3 localScale = Vector3.one;
		for (int i = 0; i < this.goLocatorList.Count; i++)
		{
			if (i < list.Count)
			{
				this.goLocatorList[i].SetActive(true);
				localScale = list[i].transform.localScale;
				list[i].transform.parent = this.goLocatorList[i].transform;
				list[i].transform.localPosition = Vector3.zero;
				list[i].transform.localRotation = localRotation;
				list[i].transform.localScale = localScale;
				list[i].SetActive(true);
			}
			else
			{
				this.goLocatorList[i].SetActive(false);
			}
		}
		for (int i = 0; i < this.goLightLocatorList.Count; i++)
		{
			if (i < list2.Count)
			{
				this.goLightLocatorList[i].SetActive(true);
				localScale = list2[i].transform.localScale;
				list2[i].transform.parent = this.goLightLocatorList[i].transform;
				list2[i].transform.localPosition = Vector3.zero;
				list2[i].transform.localRotation = localRotation;
				list2[i].transform.localScale = localScale;
				list2[i].SetActive(true);
			}
			else
			{
				this.goLightLocatorList[i].SetActive(false);
			}
		}
	}

	private void InActiveAllParts()
	{
		for (int i = 0; i < this.goLocatorList.Count; i++)
		{
			this.goLocatorList[i].SetActive(false);
		}
		for (int i = 0; i < this.goLightLocatorList.Count; i++)
		{
			this.goLightLocatorList[i].SetActive(false);
		}
	}
}
