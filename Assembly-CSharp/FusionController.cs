using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class FusionController : MonoBehaviour
{
	public bool debugMode;

	public int[] target = new int[]
	{
		1,
		2
	};

	public int eggMonsterGroupId = 1;

	public int rareUp;

	public AudioSource efxSource;

	public AudioSource efxSource2;

	private GameObject obj_mons1;

	private GameObject obj_mons2;

	private GameObject obj_mons3;

	private GameObject obj_egg;

	[Header("メインカメラ")]
	[SerializeField]
	private GameObject obj_cam;

	[Header("メインカメラリグ")]
	[SerializeField]
	private GameObject obj_camrig;

	[Header("サブカメラ")]
	[SerializeField]
	private GameObject obj_cam2;

	[Header("UIカメラ")]
	[SerializeField]
	private GameObject obj_uicam;

	[SerializeField]
	[Header("カメラスイッチャー")]
	private GameObject obj_cs;

	[SerializeField]
	[Header("魔方陣")]
	private GameObject obj_mc;

	[SerializeField]
	private GameObject obj_mcpar3;

	[Header("オーラ")]
	[SerializeField]
	private GameObject obj_aura1;

	[SerializeField]
	[Header("スピン")]
	private GameObject obj_spin;

	[Header("覚醒")]
	[SerializeField]
	private GameObject obj_k1;

	[SerializeField]
	private GameObject obj_k2;

	[SerializeField]
	[Header("キラキラ")]
	private GameObject obj_newfx10;

	[Header("Nextボタン")]
	[SerializeField]
	private GameObject obj_next;

	private ParticleSystem ps_aura1;

	private ParticleSystem ps_mcpar3;

	private ParticleSystem ps_spin;

	private ParticleSystem ps_newfx10;

	private CameraSwitcher cs_switcher;

	private Camera cam_cam2;

	private Camera cam_uicam;

	private UIWidget uiw_next;

	private Material[] elementsA;

	private Material[] elementsB;

	private int skipFlg;

	private int switchFlg;

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
	}

	private void Start()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/DirectionalLight"));
		gameObject.transform.SetParent(base.transform);
		gameObject.name = "SceneLight";
		gameObject.transform.position = new Vector3(0f, 3f, 0f);
		this.obj_mc.transform.position = new Vector3(0f, 0f, 0f);
		if (this.debugMode)
		{
			this.obj_mons1 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/" + this.target[0] + "/prefab"));
		}
		else
		{
			this.obj_mons1 = (GameObject)UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject("Characters/" + this.target[0] + "/prefab", null, true));
		}
		this.obj_mons1.layer = LayerMask.NameToLayer("Cutscene");
		foreach (Transform transform in this.obj_mons1.GetComponentsInChildren<Transform>())
		{
			transform.gameObject.layer = LayerMask.NameToLayer("Cutscene");
		}
		this.obj_mons1.transform.SetParent(base.transform);
		this.obj_mons1.name = "monsA";
		this.obj_mons1.transform.position = new Vector3(-1.5f, 0f, 0f);
		this.obj_mons1.GetComponent<CharacterParams>().PlayIdleAnimation();
		this.obj_mons1.SetActive(true);
		if (this.debugMode)
		{
			this.obj_mons2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/" + this.target[1] + "/prefab"));
		}
		else
		{
			this.obj_mons2 = (GameObject)UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject("Characters/" + this.target[1] + "/prefab", null, true));
		}
		this.obj_mons2.layer = LayerMask.NameToLayer("Cutscene");
		foreach (Transform transform2 in this.obj_mons2.GetComponentsInChildren<Transform>())
		{
			transform2.gameObject.layer = LayerMask.NameToLayer("Cutscene");
		}
		this.obj_mons2.transform.SetParent(base.transform);
		this.obj_mons2.name = "monsB";
		this.obj_mons2.transform.position = new Vector3(1.5f, 0f, 0f);
		this.obj_mons2.GetComponent<CharacterParams>().PlayIdleAnimation();
		this.obj_mons2.SetActive(true);
		string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(this.eggMonsterGroupId.ToString());
		GameObject original = AssetDataMng.Instance().LoadObject(monsterCharaPathByMonsterGroupId, null, true) as GameObject;
		this.obj_egg = UnityEngine.Object.Instantiate<GameObject>(original);
		this.obj_egg.layer = LayerMask.NameToLayer("Cutscene");
		foreach (Transform transform3 in this.obj_egg.GetComponentsInChildren<Transform>())
		{
			transform3.gameObject.layer = LayerMask.NameToLayer("Cutscene");
		}
		this.obj_egg.transform.SetParent(base.transform);
		this.obj_egg.name = "Egg";
		this.obj_egg.transform.position = new Vector3(0f, this.obj_mons1.GetComponent<CharacterParams>().characterCenterTarget.transform.position.y, 0f);
		base.transform.Find("Egg/Egg/Egg").gameObject.SetActive(false);
		this.obj_camrig.transform.position = new Vector3(0f, 0f, 0f);
		MotionBlur[] componentsInChildren4 = this.obj_cam.GetComponentsInChildren<MotionBlur>();
		componentsInChildren4[0].blurAmount = 0.2f;
		global::Debug.Log("blurAmount：" + componentsInChildren4[0].blurAmount);
		this.obj_cam2.transform.position = new Vector3(0f, -2f, 2f);
		this.cs_switcher = this.obj_cs.GetComponent<CameraSwitcher>();
		this.cs_switcher.targetName = "Egg/EggCenterTarget";
		this.cs_switcher.switchFlg = 3;
		this.cs_switcher.rotationSpeed = 2f;
		this.cam_cam2 = this.obj_cam2.GetComponent<Camera>();
		this.cam_uicam = this.obj_uicam.GetComponent<Camera>();
		this.ps_newfx10 = this.obj_newfx10.GetComponent<ParticleSystem>();
		this.ps_aura1 = this.obj_aura1.GetComponent<ParticleSystem>();
		this.ps_mcpar3 = this.obj_mcpar3.GetComponent<ParticleSystem>();
		this.ps_spin = this.obj_spin.GetComponent<ParticleSystem>();
		this.uiw_next = this.obj_next.GetComponent<UIWidget>();
		this.uiw_next.alpha = 0f;
		this.playSceneCoroutine = base.StartCoroutine(this.PlayScene());
	}

	private IEnumerator PlayScene()
	{
		yield return base.StartCoroutine(this.Init());
		this.skipFlg = 0;
		yield return base.StartCoroutine(this.CutA());
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

	private IEnumerator Init()
	{
		this.ps_newfx10.Clear();
		this.ps_newfx10.Stop();
		this.obj_k2.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_k2, iTween.Hash(new object[]
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
		this.obj_k1.GetComponent<UITexture>().alpha = 0f;
		iTween.ScaleTo(this.obj_k1, iTween.Hash(new object[]
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
		Time.timeScale = 1f;
		yield return 0;
		yield break;
	}

	private IEnumerator CutA()
	{
		this.cam_cam2.fieldOfView = 60f;
		this.cs_switcher.angle = -135f;
		this.ps_mcpar3.Play();
		yield return new WaitForSeconds(0.2f);
		if (!this.debugMode)
		{
			this.efxSource.volume = SoundMng.Instance().VolumeSE * 0.1f;
		}
		this.efxSource.Play();
		yield return new WaitForSeconds(1.5f);
		Transform trans = this.obj_mons1.transform;
		for (int i = 0; i < trans.childCount; i++)
		{
			SkinnedMeshRenderer[] smr = trans.GetChild(i).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p = 0; p < smr.Length; p++)
			{
				this.elementsA = smr[p].materials;
				for (int q = 0; q < this.elementsA.Length; q++)
				{
					this.elementsA[q] = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
				}
				smr[p].materials = this.elementsA;
			}
		}
		trans = this.obj_mons2.transform;
		for (int j = 0; j < trans.childCount; j++)
		{
			SkinnedMeshRenderer[] smr2 = trans.GetChild(j).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p2 = 0; p2 < smr2.Length; p2++)
			{
				this.elementsB = smr2[p2].materials;
				for (int q2 = 0; q2 < this.elementsB.Length; q2++)
				{
					this.elementsB[q2] = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
				}
				smr2[p2].materials = this.elementsB;
			}
		}
		yield return new WaitForSeconds(1.2f);
		trans = this.obj_mons1.transform;
		for (int k = 0; k < trans.childCount; k++)
		{
			SkinnedMeshRenderer[] smr3 = trans.GetChild(k).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p3 = 0; p3 < smr3.Length; p3++)
			{
				this.elementsA = smr3[p3].materials;
				for (int q3 = 0; q3 < this.elementsA.Length; q3++)
				{
					smr3[p3].sharedMesh.SetIndices(smr3[p3].sharedMesh.GetIndices(q3), MeshTopology.Lines, q3);
				}
			}
		}
		trans = this.obj_mons2.transform;
		for (int l = 0; l < trans.childCount; l++)
		{
			SkinnedMeshRenderer[] smr4 = trans.GetChild(l).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p4 = 0; p4 < smr4.Length; p4++)
			{
				this.elementsB = smr4[p4].materials;
				for (int q4 = 0; q4 < this.elementsB.Length; q4++)
				{
					smr4[p4].sharedMesh.SetIndices(smr4[p4].sharedMesh.GetIndices(q4), MeshTopology.Lines, q4);
				}
			}
		}
		yield return new WaitForSeconds(0.5f);
		iTween.MoveTo(this.obj_mons1, new Vector3(0f, 0f, 0f), 2.2f);
		iTween.MoveTo(this.obj_mons2, new Vector3(0f, 0f, 0f), 2.2f);
		yield return new WaitForSeconds(0.8f);
		this.ps_aura1.Play();
		yield return new WaitForSeconds(0.2f);
		this.obj_egg.transform.position = new Vector3(0f, 0f, 0f);
		this.cs_switcher.switchFlg = 2;
		this.cs_switcher.rotationSpeed = 8f;
		trans = this.obj_mons1.transform;
		for (int m = 0; m < trans.childCount; m++)
		{
			SkinnedMeshRenderer[] smr5 = trans.GetChild(m).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p5 = 0; p5 < smr5.Length; p5++)
			{
				this.elementsA = smr5[p5].materials;
				for (int q5 = 0; q5 < this.elementsA.Length; q5++)
				{
					smr5[p5].sharedMesh.SetIndices(smr5[p5].sharedMesh.GetIndices(q5), MeshTopology.Triangles, q5);
				}
			}
		}
		trans = this.obj_mons2.transform;
		for (int n = 0; n < trans.childCount; n++)
		{
			SkinnedMeshRenderer[] smr6 = trans.GetChild(n).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p6 = 0; p6 < smr6.Length; p6++)
			{
				this.elementsB = smr6[p6].materials;
				for (int q6 = 0; q6 < this.elementsB.Length; q6++)
				{
					smr6[p6].sharedMesh.SetIndices(smr6[p6].sharedMesh.GetIndices(q6), MeshTopology.Triangles, q6);
				}
			}
		}
		this.obj_mons1.SetActive(false);
		this.obj_mons2.SetActive(false);
		yield return new WaitForSeconds(2f);
		base.transform.Find("Egg/Egg/Egg").gameObject.SetActive(true);
		if (this.rareUp > 0)
		{
			yield return new WaitForSeconds(2f);
			if (!this.debugMode)
			{
				this.efxSource2.volume = SoundMng.Instance().VolumeSE * 0.1f;
			}
			this.efxSource2.Play();
			this.obj_k1.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_k1, iTween.Hash(new object[]
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
			iTween.ScaleTo(this.obj_k1, iTween.Hash(new object[]
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
			this.obj_k2.GetComponent<UITexture>().alpha = 1f;
			iTween.ScaleTo(this.obj_k2, iTween.Hash(new object[]
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
			iTween.ScaleTo(this.obj_k2, iTween.Hash(new object[]
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
			yield return new WaitForSeconds(0.5f);
			this.ps_spin.Play();
		}
		yield return new WaitForSeconds(1f);
		this.uiw_next.alpha = 1f;
		yield return base.StartCoroutine(this.SkippableWait(600f));
		if (this.skipFlg == 1)
		{
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
		Transform trans = this.obj_mons1.transform;
		for (int i = 0; i < trans.childCount; i++)
		{
			SkinnedMeshRenderer[] smr = trans.GetChild(i).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p = 0; p < smr.Length; p++)
			{
				this.elementsA = smr[p].materials;
				for (int q = 0; q < this.elementsA.Length; q++)
				{
					smr[p].sharedMesh.SetIndices(smr[p].sharedMesh.GetIndices(q), MeshTopology.Triangles, q);
				}
			}
		}
		trans = this.obj_mons2.transform;
		for (int j = 0; j < trans.childCount; j++)
		{
			SkinnedMeshRenderer[] smr2 = trans.GetChild(j).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p2 = 0; p2 < smr2.Length; p2++)
			{
				this.elementsB = smr2[p2].materials;
				for (int q2 = 0; q2 < this.elementsB.Length; q2++)
				{
					smr2[p2].sharedMesh.SetIndices(smr2[p2].sharedMesh.GetIndices(q2), MeshTopology.Triangles, q2);
				}
			}
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
				IL_C7:
				yield break;
			}
			yield return 0;
		}
		global::Debug.Log("[ SKIP ]");
		this.efxSource.Stop();
		this.cam_cam2.fieldOfView = 60f;
		this.skipFlg = 1;
		goto IL_C7;
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
		float y = this.obj_cam2.transform.position.y;
		if (this.obj_cam2.transform.position.y > 10f)
		{
			this.switchFlg = 1;
		}
		if (this.switchFlg == 0)
		{
			y = this.obj_cam2.transform.position.y + 1f * Time.deltaTime;
		}
		else
		{
			y = 10f;
		}
		this.obj_cam2.transform.position = new Vector3(this.obj_cam2.transform.position.x, y, this.obj_cam2.transform.position.z);
	}
}
