using Monster;
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class GashaController : MonoBehaviour
{
	public bool debugMode;

	public int[] target = new int[]
	{
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10
	};

	public int[] growStep = new int[]
	{
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1
	};

	public AudioSource efxSource;

	public AudioSource efxSource2;

	public AudioSource efxSource3;

	private GameObject obj_mons;

	[SerializeField]
	[Header("メインカメラ")]
	private GameObject obj_cam;

	[SerializeField]
	[Header("メインカメラリグ")]
	private GameObject obj_camrig;

	[Header("サブカメラ")]
	[SerializeField]
	private GameObject obj_cam2;

	[Header("UIカメラ")]
	[SerializeField]
	private GameObject obj_uicam;

	[Header("カメラスイッチャー")]
	[SerializeField]
	private GameObject obj_cs;

	[SerializeField]
	[Header("魔方陣")]
	private GameObject obj_mc;

	[SerializeField]
	private GameObject obj_mf;

	[SerializeField]
	private GameObject obj_mcpar1;

	[SerializeField]
	private GameObject obj_mcpar2;

	[SerializeField]
	private GameObject obj_mcpar3;

	[SerializeField]
	[Header("オーラ")]
	private GameObject obj_aura;

	[SerializeField]
	private GameObject obj_shock;

	[SerializeField]
	private GameObject obj_aura1;

	[SerializeField]
	private GameObject obj_aura2;

	[SerializeField]
	private GameObject obj_aura3;

	[SerializeField]
	[Header("電子球")]
	private GameObject obj_electron;

	[SerializeField]
	private GameObject obj_thunder;

	[SerializeField]
	private GameObject obj_star;

	[SerializeField]
	private GameObject obj_cluster;

	[Header("幼年期")]
	[SerializeField]
	private GameObject obj_n1;

	[SerializeField]
	private GameObject obj_n2;

	[SerializeField]
	private GameObject obj_n3;

	[SerializeField]
	private GameObject obj_n4;

	[SerializeField]
	private GameObject obj_n5;

	[SerializeField]
	[Header("成長期")]
	private GameObject obj_r1;

	[SerializeField]
	private GameObject obj_r2;

	[SerializeField]
	private GameObject obj_r3;

	[Header("成熟期")]
	[SerializeField]
	private GameObject obj_sr1;

	[SerializeField]
	private GameObject obj_sr2;

	[SerializeField]
	private GameObject obj_sr3;

	[Header("完全体")]
	[SerializeField]
	private GameObject obj_ssr1;

	[SerializeField]
	private GameObject obj_ssr2;

	[SerializeField]
	private GameObject obj_ssr3;

	[Header("究極体")]
	[SerializeField]
	private GameObject obj_lr1;

	[SerializeField]
	private GameObject obj_lr2;

	[SerializeField]
	private GameObject obj_lr3;

	[Header("アーマー体")]
	[SerializeField]
	private GameObject obj_sr1b;

	[SerializeField]
	private GameObject obj_sr2b;

	[SerializeField]
	private GameObject obj_sr3b;

	[SerializeField]
	private GameObject obj_sr4b;

	[SerializeField]
	private GameObject obj_sr5b;

	[Header("ハイブリッド体")]
	[SerializeField]
	private GameObject obj_HB1;

	[SerializeField]
	private GameObject obj_HB2;

	[SerializeField]
	private GameObject obj_HB3;

	[SerializeField]
	private GameObject obj_HB4;

	[SerializeField]
	private GameObject obj_HB5;

	[SerializeField]
	private GameObject obj_HB6;

	[SerializeField]
	private GameObject obj_HB7;

	[SerializeField]
	[Header("激レア")]
	private GameObject obj_sr1_r;

	[SerializeField]
	private GameObject obj_sr2_r;

	[SerializeField]
	private GameObject obj_sr3_r;

	[SerializeField]
	[Header("超激レア")]
	private GameObject obj_ssr1_r;

	[SerializeField]
	private GameObject obj_ssr2_r;

	[SerializeField]
	private GameObject obj_ssr3_r;

	[SerializeField]
	private GameObject obj_ssr4_r;

	[Header("超絶レア")]
	[SerializeField]
	private GameObject obj_lr1_r;

	[SerializeField]
	private GameObject obj_lr2_r;

	[SerializeField]
	private GameObject obj_lr3_r;

	[SerializeField]
	private GameObject obj_lr4_r;

	[Header("キラキラ")]
	[SerializeField]
	private GameObject obj_newfx10;

	[SerializeField]
	[Header("Nextボタン")]
	private GameObject obj_next;

	private ParticleSystem ps_cluster;

	private ParticleSystem ps_thunder;

	private ParticleSystem ps_aura1;

	private ParticleSystem ps_aura2;

	private ParticleSystem ps_aura3;

	private ParticleSystem ps_mf;

	private ParticleSystem ps_mcpar1;

	private ParticleSystem ps_mcpar2;

	private ParticleSystem ps_mcpar3;

	private ParticleSystem ps_shock;

	private ParticleSystem ps_newfx10;

	private CameraSwitcher cs_switcher;

	private Rigidbody rg_thunder;

	private Camera cam_cam;

	private Camera cam_cam2;

	private Camera cam_uicam;

	private CharacterParams cp_mons;

	private UIWidget uiw_next;

	private Color c_red;

	private Color c_black;

	private Color c_blue;

	private Color c_gold;

	private int skipFlg;

	private Coroutine playSceneCoroutine;

	private Action<int> endCallBack;

	public Action<int> EndCallBack
	{
		set
		{
			this.endCallBack = value;
		}
	}

	private void Awake()
	{
		base.name = "Cutscene";
		Physics.gravity = new Vector3(0f, -9.81f, 0f);
		int layer = LayerMask.NameToLayer("UI");
		int layer2 = LayerMask.NameToLayer("Cutscene");
		Physics.IgnoreLayerCollision(layer, layer2);
		global::Debug.Log(Screen.width + "×" + Screen.height);
	}

	private void Start()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/DirectionalLight"));
		gameObject.transform.SetParent(base.transform);
		gameObject.name = "SceneLight";
		gameObject.transform.position = new Vector3(0f, 3f, 0f);
		this.obj_mc.transform.position = new Vector3(0f, 0f, 0f);
		this.obj_mons = new GameObject("mons");
		this.obj_mons.transform.SetParent(base.transform);
		this.obj_mons.transform.position = new Vector3(0f, 0f, 0f);
		this.obj_camrig.transform.position = new Vector3(0f, 0f, 0f);
		MotionBlur component = this.obj_cam.GetComponent<MotionBlur>();
		component.blurAmount = 0.2f;
		global::Debug.Log("blurAmount：" + component.blurAmount);
		this.obj_cam2.transform.position = new Vector3(0f, 1f, -2f);
		this.obj_aura.transform.position = new Vector3(0f, 0f, 0f);
		this.obj_electron.transform.position = new Vector3(0f, 16f, 0f);
		this.cs_switcher = this.obj_cs.GetComponent<CameraSwitcher>();
		this.cs_switcher.targetName = "FxElectron";
		this.cs_switcher.switchFlg = 3;
		this.cs_switcher.rotationSpeed = 2f;
		this.cam_cam = this.obj_cam.GetComponent<Camera>();
		this.cam_cam2 = this.obj_cam2.GetComponent<Camera>();
		this.cam_uicam = this.obj_uicam.GetComponent<Camera>();
		this.ps_newfx10 = this.obj_newfx10.GetComponent<ParticleSystem>();
		this.ps_aura1 = this.obj_aura1.GetComponent<ParticleSystem>();
		this.ps_aura2 = this.obj_aura2.GetComponent<ParticleSystem>();
		this.ps_aura3 = this.obj_aura3.GetComponent<ParticleSystem>();
		this.ps_mf = this.obj_mf.GetComponent<ParticleSystem>();
		this.ps_mcpar1 = this.obj_mcpar1.GetComponent<ParticleSystem>();
		this.ps_mcpar2 = this.obj_mcpar2.GetComponent<ParticleSystem>();
		this.ps_mcpar3 = this.obj_mcpar3.GetComponent<ParticleSystem>();
		this.ps_shock = this.obj_shock.GetComponent<ParticleSystem>();
		this.uiw_next = this.obj_next.GetComponent<UIWidget>();
		this.ps_thunder = this.obj_thunder.GetComponent<ParticleSystem>();
		this.ps_cluster = this.obj_cluster.GetComponent<ParticleSystem>();
		this.rg_thunder = this.obj_electron.GetComponent<Rigidbody>();
		this.c_red = new Color(0.7254902f, 0.0431372561f, 0.3137255f, 1f);
		this.c_black = new Color(0f, 0f, 0f, 1f);
		this.c_blue = new Color(0.0235294122f, 0.003921569f, 0.345098048f, 1f);
		this.c_gold = new Color(1f, 0.843137264f, 0f, 1f);
		this.playSceneCoroutine = base.StartCoroutine(this.PlayScene());
	}

	private IEnumerator PlayScene()
	{
		for (int i = 0; i < this.target.Length; i++)
		{
			this.skipFlg = 0;
			yield return base.StartCoroutine(this.Init(i));
			this.skipFlg = 0;
			yield return base.StartCoroutine(this.CutA(i));
			if (this.skipFlg == 0)
			{
				yield return base.StartCoroutine(this.CutB(i));
			}
			else
			{
				this.ps_thunder.Stop();
				this.rg_thunder.useGravity = false;
				this.ps_mcpar3.Play();
			}
			this.skipFlg = 0;
			yield return base.StartCoroutine(this.CutC(i));
		}
		GC.Collect();
		Resources.UnloadUnusedAssets();
		if (this.endCallBack != null)
		{
			this.endCallBack(0);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield return 0;
		yield break;
	}

	private IEnumerator Init(int i)
	{
		this.obj_mons.SetActive(false);
		UnityEngine.Object.Destroy(this.obj_mons);
		this.ps_newfx10.Clear();
		this.ps_newfx10.Stop();
		this.obj_n1.GetComponent<UITexture>().alpha = 0f;
		this.obj_n2.GetComponent<UITexture>().alpha = 0f;
		this.obj_n3.GetComponent<UITexture>().alpha = 0f;
		this.obj_n4.GetComponent<UITexture>().alpha = 0f;
		this.obj_n5.GetComponent<UITexture>().alpha = 0f;
		this.obj_r1.GetComponent<UITexture>().alpha = 0f;
		this.obj_r2.GetComponent<UITexture>().alpha = 0f;
		this.obj_r3.GetComponent<UITexture>().alpha = 0f;
		this.obj_sr1.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr1, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr2.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr2, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr3.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr3, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_ssr1.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_ssr1, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_ssr2.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_ssr2, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_ssr3.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_ssr3, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_lr1.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_lr1, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_lr2.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_lr2, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_lr3.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_lr3, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr1b.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr1b, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr2b.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr2b, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr3b.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr3b, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr4b.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr4b, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr5b.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_sr5b, iTween.Hash(new object[]
		{
			"x",
			5,
			"y",
			5,
			"time",
			0f,
			"delay",
			0f
		}));
		this.obj_sr3_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_sr2_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_sr1_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr4_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr3_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr2_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr1_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr4_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr3_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr2_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr1_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_HB1.GetComponent<UITexture>().alpha = 0f;
		this.obj_HB2.GetComponent<UITexture>().alpha = 0f;
		this.obj_HB3.GetComponent<UITexture>().alpha = 0f;
		this.obj_HB4.GetComponent<UITexture>().alpha = 0f;
		this.obj_HB5.GetComponent<UITexture>().alpha = 0f;
		this.obj_HB6.GetComponent<UITexture>().alpha = 0f;
		this.obj_HB7.GetComponent<UITexture>().alpha = 0f;
		this.ps_thunder.Clear();
		this.ps_mf.Clear();
		this.ps_mcpar2.Stop();
		this.ps_mcpar1.Stop();
		this.ps_mcpar3.Clear();
		this.uiw_next.alpha = 0f;
		GC.Collect();
		Resources.UnloadUnusedAssets();
		Vector3 fx2_v = new Vector3(0f, 2f, 0f);
		if (i == 0)
		{
			fx2_v = new Vector3(0f, 16f, 0f);
		}
		this.obj_electron.transform.position = fx2_v;
		if (MonsterGrowStepData.IsGrowStepHigh(this.growStep[i]))
		{
			this.obj_star.SetActive(true);
		}
		else
		{
			this.obj_star.SetActive(false);
		}
		this.rg_thunder.useGravity = false;
		this.cam_cam2.fieldOfView = 40f;
		if (i == 0)
		{
			this.cam_cam2.fieldOfView = 12f;
		}
		this.obj_cam2.transform.position = new Vector3(0f, 1f, -2f);
		if (MonsterGrowStepData.IsRipeScope(this.growStep[i]))
		{
			this.cam_cam.backgroundColor = this.c_red;
			this.cam_cam2.backgroundColor = this.c_red;
		}
		else if (MonsterGrowStepData.IsGrowStepHigh(this.growStep[i]))
		{
			this.cam_cam.backgroundColor = this.c_black;
			this.cam_cam2.backgroundColor = this.c_black;
		}
		else
		{
			this.cam_cam.backgroundColor = this.c_blue;
			this.cam_cam2.backgroundColor = this.c_blue;
		}
		this.ps_shock.Stop();
		this.cs_switcher.targetName = "FxElectron";
		this.cs_switcher.switchFlg = 3;
		this.cs_switcher.rotationSpeed = 2f;
		if (this.debugMode)
		{
			this.obj_mons = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/" + this.target[i] + "/prefab"));
		}
		else
		{
			this.obj_mons = (GameObject)UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject("Characters/" + this.target[i] + "/prefab", null, true));
		}
		Camera cam = this.obj_cam.GetComponent<Camera>();
		CutsceneControllerBase.SetBillBoardCamera(this.obj_mons, cam);
		this.obj_mons.layer = LayerMask.NameToLayer("Cutscene");
		foreach (Transform j in this.obj_mons.GetComponentsInChildren<Transform>())
		{
			j.gameObject.layer = LayerMask.NameToLayer("Cutscene");
		}
		this.obj_mons.transform.SetParent(base.transform);
		this.obj_mons.name = "mons" + i;
		this.obj_mons.transform.position = new Vector3(0f, 0f, 0f);
		this.cp_mons = this.obj_mons.GetComponent<CharacterParams>();
		this.cp_mons.PlayIdleAnimation();
		this.obj_mons.SetActive(false);
		Time.timeScale = 1f;
		yield return 0;
		yield break;
	}

	private IEnumerator CutA(int i)
	{
		if (i == 0)
		{
			this.ps_cluster.Play();
			yield return new WaitForSeconds(1f);
		}
		this.ps_thunder.Play();
		if (i == 0)
		{
			yield return new WaitForSeconds(0.2f);
		}
		this.rg_thunder.useGravity = true;
		yield return new WaitForSeconds(0.5f);
		this.ps_cluster.Stop();
		if (i == 0)
		{
			if (!this.debugMode)
			{
				this.efxSource.volume = SoundMng.Instance().VolumeSE * 0.1f;
			}
			this.efxSource.Play();
		}
		this.ps_shock.Play();
		this.ps_mf.Play();
		if (i == 0)
		{
			yield return base.StartCoroutine(this.SkippableWait(1.5f));
			if (this.skipFlg == 1)
			{
				this.skipFlg = 0;
				yield break;
			}
		}
		else
		{
			yield return base.StartCoroutine(this.SkippableWait(0.5f));
			if (this.skipFlg == 1)
			{
				this.skipFlg = 0;
				yield break;
			}
		}
		global::Debug.Log(this.cam_cam2.fieldOfView);
		yield return 0;
		yield break;
	}

	private IEnumerator CutB(int i)
	{
		if (!this.debugMode)
		{
			this.efxSource2.volume = SoundMng.Instance().VolumeSE * 0.1f;
		}
		this.efxSource2.Play();
		if (MonsterGrowStepData.IsRipeScope(this.growStep[i]))
		{
			this.ps_aura2.Play();
		}
		else if (MonsterGrowStepData.IsGrowStepHigh(this.growStep[i]))
		{
			this.ps_aura3.Play();
		}
		else
		{
			this.ps_aura1.Play();
		}
		yield return new WaitForSeconds(0.9f);
		this.ps_thunder.Stop();
		this.rg_thunder.useGravity = false;
		yield return new WaitForSeconds(1.2f);
		this.ps_mcpar3.Play();
		yield return 0;
		yield break;
	}

	private IEnumerator CutC(int i)
	{
		this.cam_cam2.fieldOfView = 30f;
		this.obj_cam2.transform.position = new Vector3(2f, 1f, 0f);
		if (MonsterGrowStepData.IsUltimateScope(this.growStep[i]))
		{
			this.cam_cam.backgroundColor = this.c_gold;
			this.cam_cam2.backgroundColor = this.c_gold;
		}
		this.obj_mons.SetActive(true);
		this.cp_mons.PlayIdleAnimation();
		this.cs_switcher.targetName = "mons" + i;
		yield return new WaitForSeconds(1f);
		if (MonsterGrowStepData.IsRipeScope(this.growStep[i]))
		{
			if (!this.debugMode)
			{
				this.efxSource3.volume = SoundMng.Instance().VolumeSE * 0.1f;
			}
			this.efxSource3.Play();
			this.obj_sr1_r.GetComponent<UITexture>().alpha = 1f;
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.02f);
			this.obj_sr2_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_sr3_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.1f);
			yield return base.StartCoroutine(this.SkippableWait(1.2f));
			if (this.skipFlg == 1)
			{
				this.skipFlg = 0;
				yield break;
			}
		}
		if (MonsterGrowStepData.IsPerfectScope(this.growStep[i]))
		{
			if (!this.debugMode)
			{
				this.efxSource3.volume = SoundMng.Instance().VolumeSE * 0.1f;
			}
			this.efxSource3.Play();
			this.obj_ssr1_r.GetComponent<UITexture>().alpha = 1f;
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.02f);
			this.obj_ssr2_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_ssr3_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_ssr4_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.1f);
			yield return base.StartCoroutine(this.SkippableWait(1.2f));
			if (this.skipFlg == 1)
			{
				this.skipFlg = 0;
				yield break;
			}
		}
		if (MonsterGrowStepData.IsUltimateScope(this.growStep[i]))
		{
			if (!this.debugMode)
			{
				this.efxSource3.volume = SoundMng.Instance().VolumeSE * 0.1f;
			}
			this.efxSource3.Play();
			this.obj_lr1_r.GetComponent<UITexture>().alpha = 1f;
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.02f);
			this.obj_lr2_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_lr3_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_lr4_r.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.1f);
			yield return base.StartCoroutine(this.SkippableWait(1.2f));
			if (this.skipFlg == 1)
			{
				this.skipFlg = 0;
				yield break;
			}
		}
		if (!this.debugMode)
		{
			this.efxSource3.volume = SoundMng.Instance().VolumeSE * 0.1f;
		}
		this.efxSource3.Play();
		this.obj_sr3_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_sr2_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_sr1_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr4_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr3_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr2_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_ssr1_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr4_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr3_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr2_r.GetComponent<UITexture>().alpha = 0f;
		this.obj_lr1_r.GetComponent<UITexture>().alpha = 0f;
		if (MonsterGrowStepData.IsChildScope(this.growStep[i]))
		{
			this.obj_n1.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_n2.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_n3.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			if (MonsterGrowStepData.IsChild1Scope(this.growStep[i]))
			{
				this.obj_n4.GetComponent<UITexture>().alpha = 1f;
			}
			else
			{
				this.obj_n5.GetComponent<UITexture>().alpha = 1f;
			}
		}
		else if (MonsterGrowStepData.IsGrowingGroup(this.growStep[i]))
		{
			this.obj_r1.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_r2.GetComponent<UITexture>().alpha = 1f;
			yield return new WaitForSeconds(0.02f);
			this.obj_r3.GetComponent<UITexture>().alpha = 1f;
		}
		else if (MonsterGrowStepData.IsRipeGroup(this.growStep[i]))
		{
			this.obj_sr1.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr1, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr1, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.45f);
			this.obj_sr2.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr2, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr2, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_sr3.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr3, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr3, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
		}
		else if (MonsterGrowStepData.IsPerfectGroup(this.growStep[i]))
		{
			this.obj_ssr1.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_ssr1, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_ssr1, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.45f);
			this.obj_ssr2.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_ssr2, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_ssr2, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_ssr3.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_ssr3, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_ssr3, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
		}
		else if (MonsterGrowStepData.IsUltimateGroup(this.growStep[i]))
		{
			this.obj_lr1.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_lr1, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_lr1, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.45f);
			this.obj_lr2.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_lr2, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_lr2, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_lr3.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_lr3, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_lr3, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
		}
		else if (MonsterGrowStepData.IsArmorGroup(this.growStep[i]))
		{
			this.obj_sr1b.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr1b, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr1b, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.45f);
			this.obj_sr2b.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr2b, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr2b, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_sr3b.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr3b, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr3b, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_sr4b.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr4b, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr4b, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_sr5b.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_sr5b, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_sr5b, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
		}
		else if (MonsterGrowStepData.IsHybridGroup(this.growStep[i]))
		{
			this.obj_HB1.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_HB1, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_HB1, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			this.ps_newfx10.Play();
			yield return new WaitForSeconds(0.45f);
			this.obj_HB2.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_HB2, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_HB2, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_HB3.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_HB3, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_HB3, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_HB4.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_HB4, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_HB4, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_HB5.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_HB5, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_HB5, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_HB6.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_HB6, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_HB6, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
			yield return new WaitForSeconds(0.45f);
			this.obj_HB7.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_HB7, iTween.Hash(new object[]
			{
				"x",
				0.5,
				"y",
				0.5,
				"easetype",
				"easeOutQuart",
				"time",
				0.1f,
				"delay",
				0.2f
			}));
			iTween.ScaleTo(this.obj_HB7, iTween.Hash(new object[]
			{
				"x",
				1,
				"y",
				1,
				"easetype",
				"easeOutQuart",
				"time",
				0f,
				"delay",
				0.2f
			}));
		}
		this.cs_switcher.switchFlg = 2;
		this.cs_switcher.rotationSpeed = 8f;
		this.cp_mons.PlayAnimation(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
		yield return new WaitForSeconds(0.1f);
		yield return base.StartCoroutine(this.SkippableWait(1.5f));
		if (this.skipFlg == 1)
		{
			this.skipFlg = 0;
			yield break;
		}
		this.uiw_next.alpha = 1f;
		yield return base.StartCoroutine(this.SkippableWait(600f));
		if (this.skipFlg == 1)
		{
			this.skipFlg = 0;
			yield break;
		}
		yield return 0;
		yield break;
	}

	private IEnumerator Fade()
	{
		if (this.playSceneCoroutine != null)
		{
			base.StopCoroutine(this.playSceneCoroutine);
			this.playSceneCoroutine = null;
		}
		GC.Collect();
		Resources.UnloadUnusedAssets();
		if (this.endCallBack != null)
		{
			this.endCallBack(0);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield return 0;
		yield break;
	}

	private IEnumerator SkippableWait(float fSeconds)
	{
		float fStartTime = Time.time;
		while (!Input.GetMouseButtonUp(0) && Input.touchCount <= 0)
		{
			if (Time.time - fStartTime > fSeconds)
			{
				this.skipFlg = 0;
				IL_10A:
				yield break;
			}
			yield return 0;
		}
		global::Debug.Log("[ SKIP ]");
		if (this.cam_cam2.fieldOfView < 40f)
		{
			this.obj_electron.transform.position = new Vector3(0f, 2f, 0f);
		}
		this.efxSource.Stop();
		this.cam_cam2.fieldOfView = 40f;
		this.skipFlg = 1;
		goto IL_10A;
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0) || Input.touchCount > 0)
		{
			Ray ray = this.cam_uicam.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(ray, out raycastHit))
			{
				GameObject gameObject = raycastHit.collider.gameObject;
				global::Debug.Log(gameObject.name);
				if (gameObject.name == "Skip")
				{
					base.StartCoroutine(this.Fade());
				}
			}
		}
	}

	private void LateUpdate()
	{
		this.obj_electron.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		this.obj_electron.transform.position = new Vector3(0f, this.obj_electron.transform.position.y, 0f);
		if (this.cam_cam2.fieldOfView < 40f && this.rg_thunder.useGravity)
		{
			this.cam_cam2.fieldOfView += 0.8f;
			if (this.cam_cam2.fieldOfView > 40f)
			{
				this.cam_cam2.fieldOfView = 40f;
			}
		}
	}
}
