using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class TrainingController : MonoBehaviour
{
	public bool debugMode;

	public int[] target = new int[]
	{
		1,
		1
	};

	public AudioSource efxSource;

	public AudioSource efxSource2;

	private GameObject obj_mons1;

	private GameObject obj_mons2;

	private GameObject obj_mons3;

	private GameObject obj_mons4;

	private GameObject obj_mons5;

	private GameObject obj_mons6;

	[Header("メインカメラ")]
	[SerializeField]
	private GameObject obj_cam;

	[Header("メインカメラリグ")]
	[SerializeField]
	private GameObject obj_camrig;

	[SerializeField]
	[Header("サブカメラ")]
	private GameObject obj_cam2;

	[SerializeField]
	[Header("UIカメラ")]
	private GameObject obj_uicam;

	[Header("カメラスイッチャー")]
	[SerializeField]
	private GameObject obj_cs;

	[SerializeField]
	[Header("魔方陣")]
	private GameObject obj_mc;

	[SerializeField]
	private GameObject obj_mcpar3;

	[SerializeField]
	[Header("オーラ")]
	private GameObject obj_aura2;

	[Header("∞エフェクト")]
	[SerializeField]
	private GameObject obj_infinity;

	private ParticleSystem ps_aura2;

	private ParticleSystem ps_mcpar3;

	private ParticleSystem ps_infinity;

	private Camera cam_cam2;

	private Camera cam_uicam;

	private CharacterParams cp_mons1;

	private CameraSwitcher cs_switcher;

	private GameObject fx3_instance;

	private GameObject fx4_instance;

	private GameObject fx5_instance;

	private GameObject fx6_instance;

	private GameObject fx7_instance;

	private Material[] elements;

	private Transform t;

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
		this.obj_mons1.transform.position = new Vector3(0f, 0f, 0f);
		this.cp_mons1 = this.obj_mons1.GetComponent<CharacterParams>();
		this.cp_mons1.PlayIdleAnimation();
		this.obj_mons1.SetActive(true);
		this.obj_camrig.transform.position = new Vector3(0f, 0f, 0f);
		MotionBlur[] componentsInChildren2 = this.obj_cam.GetComponentsInChildren<MotionBlur>();
		componentsInChildren2[0].blurAmount = 0.2f;
		global::Debug.Log("blurAmount：" + componentsInChildren2[0].blurAmount);
		this.obj_cam2.transform.position = new Vector3(-2f, 10f, 0f);
		this.t = this.obj_mons1.GetComponent<CharacterParams>().characterCenterTarget;
		this.obj_infinity.transform.position = this.t.position;
		this.obj_aura2.transform.position = this.t.position;
		this.fx3_instance = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/FxLaser"));
		this.fx3_instance.transform.SetParent(base.transform);
		this.fx3_instance.name = "FxLaserB";
		this.fx3_instance.transform.position = new Vector3(0f, this.t.position.y, 0f);
		this.fx3_instance.transform.Rotate(new Vector3(0f, 72f, 0f));
		if (this.target.Length >= 3)
		{
			this.fx4_instance = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/FxLaser"));
			this.fx4_instance.transform.SetParent(base.transform);
			this.fx4_instance.name = "FxLaserC";
			this.fx4_instance.transform.position = new Vector3(0f, this.t.position.y, 0f);
			if (this.target.Length == 3)
			{
				this.fx4_instance.transform.Rotate(new Vector3(0f, 252f, 0f));
			}
			else if (this.target.Length == 4)
			{
				this.fx4_instance.transform.Rotate(new Vector3(0f, 192f, 0f));
			}
			else if (this.target.Length == 5)
			{
				this.fx4_instance.transform.Rotate(new Vector3(0f, 162f, 0f));
			}
			else if (this.target.Length == 6)
			{
				this.fx4_instance.transform.Rotate(new Vector3(0f, 144f, 0f));
			}
		}
		if (this.target.Length >= 4)
		{
			this.fx5_instance = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/FxLaser"));
			this.fx5_instance.transform.SetParent(base.transform);
			this.fx5_instance.name = "FxLaserD";
			this.fx5_instance.transform.position = new Vector3(0f, this.t.position.y, 0f);
			if (this.target.Length == 4)
			{
				this.fx5_instance.transform.Rotate(new Vector3(0f, -48f, 0f));
			}
			else if (this.target.Length == 5)
			{
				this.fx5_instance.transform.Rotate(new Vector3(0f, 252f, 0f));
			}
			else if (this.target.Length == 6)
			{
				this.fx5_instance.transform.Rotate(new Vector3(0f, 216f, 0f));
			}
		}
		if (this.target.Length >= 5)
		{
			this.fx6_instance = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/FxLaser"));
			this.fx6_instance.transform.SetParent(base.transform);
			this.fx6_instance.name = "FxLaserE";
			this.fx6_instance.transform.position = new Vector3(0f, this.t.position.y, 0f);
			if (this.target.Length == 5)
			{
				this.fx6_instance.transform.Rotate(new Vector3(0f, -18f, 0f));
			}
			else if (this.target.Length == 6)
			{
				this.fx6_instance.transform.Rotate(new Vector3(0f, -72f, 0f));
			}
		}
		if (this.target.Length >= 6)
		{
			this.fx7_instance = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/FxLaser"));
			this.fx7_instance.transform.SetParent(base.transform);
			this.fx7_instance.name = "FxLaserF";
			this.fx7_instance.transform.position = new Vector3(0f, this.t.position.y, 0f);
			this.fx7_instance.transform.Rotate(new Vector3(0f, 0f, 0f));
		}
		this.cs_switcher = this.obj_cs.GetComponent<CameraSwitcher>();
		this.cs_switcher.targetName = "FxInfinity";
		this.cs_switcher.switchFlg = 3;
		this.cs_switcher.rotationSpeed = 2f;
		this.cam_cam2 = this.obj_cam2.GetComponent<Camera>();
		this.cam_uicam = this.obj_uicam.GetComponent<Camera>();
		this.ps_aura2 = this.obj_aura2.GetComponent<ParticleSystem>();
		this.ps_mcpar3 = this.obj_mcpar3.GetComponent<ParticleSystem>();
		this.ps_infinity = this.obj_infinity.GetComponent<ParticleSystem>();
		this.playSceneCoroutine = base.StartCoroutine(this.PlayScene());
	}

	private IEnumerator PlayScene()
	{
		yield return base.StartCoroutine(this.Init());
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
		Time.timeScale = 1f;
		yield return 0;
		yield break;
	}

	private IEnumerator CutA()
	{
		this.cs_switcher.angle = -180f;
		yield return new WaitForSeconds(0.5f);
		if (!this.debugMode)
		{
			this.efxSource.volume = SoundMng.Instance().VolumeSE * 0.1f;
		}
		this.efxSource.Play();
		yield return new WaitForSeconds(0.5f);
		iTween.MoveTo(base.transform.Find("FxLaserB/par2").gameObject, new Vector3(0f, this.t.position.y, 0f), 2.2f);
		if (this.target.Length >= 3)
		{
			iTween.MoveTo(base.transform.Find("FxLaserC/par2").gameObject, new Vector3(0f, this.t.position.y, 0f), 2.2f);
		}
		if (this.target.Length >= 4)
		{
			iTween.MoveTo(base.transform.Find("FxLaserD/par2").gameObject, new Vector3(0f, this.t.position.y, 0f), 2.2f);
		}
		if (this.target.Length >= 5)
		{
			iTween.MoveTo(base.transform.Find("FxLaserE/par2").gameObject, new Vector3(0f, this.t.position.y, 0f), 2.2f);
		}
		if (this.target.Length >= 6)
		{
			iTween.MoveTo(base.transform.Find("FxLaserF/par2").gameObject, new Vector3(0f, this.t.position.y, 0f), 2.2f);
		}
		yield return new WaitForSeconds(0.3f);
		Transform trans = this.obj_mons1.transform;
		for (int i = 0; i < trans.childCount; i++)
		{
			SkinnedMeshRenderer[] smr = trans.GetChild(i).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p = 0; p < smr.Length; p++)
			{
				this.elements = smr[p].materials;
				for (int q = 0; q < this.elements.Length; q++)
				{
					this.elements[q] = new Material(Shader.Find("Unlit/UnlitAlphaWithFade"));
				}
				smr[p].materials = this.elements;
			}
		}
		this.ps_infinity.Play();
		yield return new WaitForSeconds(0.3f);
		this.fx3_instance.SetActive(false);
		if (this.target.Length >= 3)
		{
			this.fx4_instance.SetActive(false);
		}
		if (this.target.Length >= 4)
		{
			this.fx5_instance.SetActive(false);
		}
		if (this.target.Length >= 5)
		{
			this.fx6_instance.SetActive(false);
		}
		if (this.target.Length >= 6)
		{
			this.fx7_instance.SetActive(false);
		}
		yield return new WaitForSeconds(0.8f);
		trans = this.obj_mons1.transform;
		for (int j = 0; j < trans.childCount; j++)
		{
			SkinnedMeshRenderer[] smr2 = trans.GetChild(j).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p2 = 0; p2 < smr2.Length; p2++)
			{
				this.elements = smr2[p2].materials;
				for (int q2 = 0; q2 < this.elements.Length; q2++)
				{
					smr2[p2].sharedMesh.SetIndices(smr2[p2].sharedMesh.GetIndices(q2), MeshTopology.Lines, q2);
				}
			}
		}
		yield return new WaitForSeconds(1.6f);
		this.ps_aura2.Play();
		yield return new WaitForSeconds(0.2f);
		this.ps_mcpar3.Play();
		yield return new WaitForSeconds(1.5f);
		trans = this.obj_mons1.transform;
		for (int k = 0; k < trans.childCount; k++)
		{
			SkinnedMeshRenderer[] smr3 = trans.GetChild(k).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p3 = 0; p3 < smr3.Length; p3++)
			{
				this.elements = smr3[p3].materials;
				for (int q3 = 0; q3 < this.elements.Length; q3++)
				{
					smr3[p3].sharedMesh.SetIndices(smr3[p3].sharedMesh.GetIndices(q3), MeshTopology.Triangles, q3);
				}
			}
		}
		this.obj_mons1.SetActive(false);
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
				this.elements = smr[p].materials;
				for (int q = 0; q < this.elements.Length; q++)
				{
					smr[p].sharedMesh.SetIndices(smr[p].sharedMesh.GetIndices(q), MeshTopology.Triangles, q);
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
				IL_1C8:
				yield break;
			}
			yield return 0;
		}
		global::Debug.Log("[ SKIP ]");
		this.efxSource.Stop();
		this.cam_cam2.fieldOfView = 40f;
		Transform trans = this.obj_mons1.transform;
		for (int i = 0; i < trans.childCount; i++)
		{
			SkinnedMeshRenderer[] smr = trans.GetChild(i).GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int p = 0; p < smr.Length; p++)
			{
				this.elements = smr[p].materials;
				for (int q = 0; q < this.elements.Length; q++)
				{
					smr[p].sharedMesh.SetIndices(smr[p].sharedMesh.GetIndices(q), MeshTopology.Triangles, q);
				}
			}
		}
		goto IL_1C8;
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
		if (this.obj_cam2.transform.position.y < 1f)
		{
			this.switchFlg = 1;
		}
		if (this.switchFlg == 0)
		{
			y = this.obj_cam2.transform.position.y - 3f * Time.deltaTime;
		}
		else
		{
			y = 1f;
		}
		this.obj_cam2.transform.position = new Vector3(this.obj_cam2.transform.position.x, y, this.obj_cam2.transform.position.z);
	}
}
