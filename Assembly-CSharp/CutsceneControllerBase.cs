using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CutsceneControllerBase : MonoBehaviour
{
	private const string cutSceneName = "Cutscene";

	[SerializeField]
	protected bool debugMode;

	private Camera uiRootCamera;

	private Collider skipButton;

	private Collider endbutton;

	[Header("UI ROOT")]
	[SerializeField]
	private Transform UIroot;

	[SerializeField]
	[Header("UIカメラ")]
	private Camera UIcamera;

	[Header("スキップボタンコライダ")]
	[SerializeField]
	private Collider skipButtonCollider;

	[Header("エンドボタンコライダ")]
	[SerializeField]
	private Collider endButtonCollider;

	[Header("餌モンスターのレベル帯")]
	public int monsterLevelClass1;

	[Header("親モンスターのレベル帯")]
	public int monsterLevelClass2;

	[FormerlySerializedAs("target")]
	[SerializeField]
	private int[] _target = new int[0];

	protected CharacterParams character1Params;

	protected CharacterParams character2Params;

	[Header("進化前デジモンの親")]
	[SerializeField]
	protected GameObject character1Parent;

	[Header("進化後デジモンの親")]
	[SerializeField]
	protected GameObject character2Parent;

	protected GameObject monsA_instance;

	protected GameObject monsB_instance;

	protected Material[] elements;

	protected Material[] elementsA;

	protected Material[] elementsB;

	private SkinnedMeshRenderer smr;

	protected Material copyMaterial;

	protected Color materialAlpha;

	private Action<int> endCallBack;

	private List<string> isPlayingSE = new List<string>();

	public int[] target
	{
		get
		{
			return this._target;
		}
		set
		{
			this._target = value;
		}
	}

	public Action<int> EndCallBack
	{
		set
		{
			this.endCallBack = value;
		}
	}

	protected virtual void AwakeChild()
	{
	}

	private void Awake()
	{
		if (SoundMng.Instance() == null)
		{
			base.gameObject.AddComponent<SoundMng>();
			if (!base.gameObject.GetComponent<AudioListener>())
			{
				base.gameObject.AddComponent<AudioListener>();
			}
			SoundMng.Instance().Initialize();
		}
		base.name = "Cutscene";
		this.uiRootCamera = this.UIcamera;
		this.skipButton = this.skipButtonCollider;
		this.endbutton = this.endButtonCollider;
		int layer = LayerMask.NameToLayer("UI");
		int layer2 = LayerMask.NameToLayer("Cutscene");
		Physics.IgnoreLayerCollision(layer, layer2);
		this.AwakeChild();
	}

	protected void PlaySE(AudioClip ac)
	{
	}

	protected void PlaySE(string path, bool isFullPath = false)
	{
		string text = (!isFullPath) ? ("SEInternal/Cutscene/" + path) : path;
		if (!this.isPlayingSE.Contains(text.Trim()))
		{
			this.isPlayingSE.Add(text.Trim());
		}
		SoundMng.Instance().PlaySE(text, 0f, false, true, null, -1, 1f);
	}

	protected void StopAllSE()
	{
		foreach (string path in this.isPlayingSE)
		{
			SoundMng.Instance().TryStopSE(path, 0f, null);
		}
	}

	private void OnDestroy()
	{
		this.StopAllSE();
	}

	protected virtual void UpdateChild()
	{
	}

	public void monsPosAdjustment(int mlc, GameObject mons)
	{
		Vector3 zero;
		if (mlc == 4)
		{
			zero = new Vector3(0f, 0.3f, 0f);
		}
		else if (mlc <= 3)
		{
			zero = new Vector3(0f, 0.5f, 0f);
		}
		else
		{
			zero = Vector3.zero;
		}
		mons.transform.localPosition = zero;
	}

	protected Material[] MaterialKeeper(GameObject g)
	{
		return g.GetComponentInChildren<SkinnedMeshRenderer>().materials;
	}

	protected GameObject monsterInstantiater(GameObject mons, GameObject parent, CharacterParams cp, int t)
	{
		if (!this.debugMode)
		{
			mons = (GameObject)UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject("Characters/" + this.target[t] + "/prefab", null, true));
		}
		else
		{
			mons = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/" + this.target[t] + "/prefab"));
		}
		cp = mons.GetComponent<CharacterParams>();
		cp.PlayIdleAnimation();
		mons.transform.parent = parent.transform;
		mons.transform.localPosition = Vector3.zero;
		return mons;
	}

	private void Update()
	{
		this.UpdateChild();
		if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
		{
			Ray ray = this.uiRootCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(ray, out raycastHit))
			{
				Collider collider = raycastHit.collider;
				if (collider == this.skipButton)
				{
					base.StartCoroutine(this.NextPage());
					UnityEngine.Object.Destroy(this.skipButton);
					UnityEngine.Object.Destroy(this.endbutton);
				}
				else if (collider == this.endbutton)
				{
					base.StartCoroutine(this.EndPage());
					UnityEngine.Object.Destroy(this.endbutton);
				}
			}
		}
	}

	protected void OnWireFrameRenderer(GameObject g, Material m)
	{
		this.smr = g.GetComponentInChildren<SkinnedMeshRenderer>();
		this.elements = this.smr.GetComponentInChildren<SkinnedMeshRenderer>().materials;
		for (int i = 0; i < this.elements.Length; i++)
		{
			this.elements[i] = m;
		}
		g.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elements;
		for (int j = 0; j < this.elements.Length; j++)
		{
			this.smr.sharedMesh.SetIndices(this.smr.sharedMesh.GetIndices(j), MeshTopology.LineStrip, j);
		}
	}

	protected void OffWireFrameRenderer(GameObject g)
	{
		this.smr = g.GetComponentInChildren<SkinnedMeshRenderer>();
		this.elements = this.smr.GetComponentInChildren<SkinnedMeshRenderer>().materials;
		for (int i = 0; i < this.elements.Length; i++)
		{
			this.smr.sharedMesh.SetIndices(this.smr.sharedMesh.GetIndices(i), MeshTopology.Triangles, i);
		}
	}

	protected void OnMaterialChanger(Material m, GameObject g)
	{
		this.smr = g.GetComponentInChildren<SkinnedMeshRenderer>();
		this.elements = this.smr.GetComponentInChildren<SkinnedMeshRenderer>().materials;
		for (int i = 0; i < this.elements.Length; i++)
		{
			this.elements[i] = m;
		}
		g.GetComponentInChildren<SkinnedMeshRenderer>().materials = this.elements;
	}

	protected IEnumerator FadeoutCorutine(float fadecount)
	{
		while (this.materialAlpha.a > 0f)
		{
			yield return null;
			this.materialAlpha.a = this.materialAlpha.a - fadecount;
			this.copyMaterial.color = this.materialAlpha;
		}
		yield break;
	}

	protected virtual IEnumerator NextPageBefore()
	{
		yield break;
	}

	protected virtual IEnumerator NextPageAfter()
	{
		yield break;
	}

	private IEnumerator NextPage()
	{
		IEnumerator nextPageBefore = this.NextPageBefore();
		while (nextPageBefore.MoveNext())
		{
			object obj = nextPageBefore.Current;
			yield return obj;
		}
		yield return new WaitForSeconds(this.fadeWaitTime);
		IEnumerator nextPageAfter = this.NextPageAfter();
		while (nextPageAfter.MoveNext())
		{
			object obj2 = nextPageAfter.Current;
			yield return obj2;
		}
		Resources.UnloadUnusedAssets();
		if (this.endCallBack != null)
		{
			this.endCallBack(0);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	protected virtual IEnumerator EndPageBefore()
	{
		return this.NextPageBefore();
	}

	protected virtual IEnumerator EndPageAfter()
	{
		return this.NextPageAfter();
	}

	private IEnumerator EndPage()
	{
		IEnumerator endPageBefore = this.EndPageBefore();
		while (endPageBefore.MoveNext())
		{
			object obj = endPageBefore.Current;
			yield return obj;
		}
		yield return new WaitForSeconds(this.fadeWaitTime);
		IEnumerator endPageAfter = this.EndPageAfter();
		while (endPageAfter.MoveNext())
		{
			object obj2 = endPageAfter.Current;
			yield return obj2;
		}
		Resources.UnloadUnusedAssets();
		if (this.endCallBack != null)
		{
			this.endCallBack(0);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	protected virtual float fadeWaitTime
	{
		get
		{
			return 3f;
		}
	}
}
