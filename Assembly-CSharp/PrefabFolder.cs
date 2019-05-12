using System;
using UnityEngine;

[AddComponentMenu("GUI/PrefabFolder")]
[ExecuteInEditMode]
public class PrefabFolder : MonoBehaviour
{
	[SerializeField]
	private GameObject goSRC_Prefab;

	private GameObject goSRC_Prv;

	private GameObject target;

	public GameObject Target
	{
		get
		{
			return this.target;
		}
	}

	private void Awake()
	{
		this.goSRC_Prv = this.goSRC_Prefab;
		if (Application.isPlaying)
		{
			this.ReplacePrefab();
			if (this.target != null)
			{
				UtilForCMD component = this.target.GetComponent<UtilForCMD>();
				if (component != null)
				{
					component.SetParamToCMD();
				}
			}
		}
	}

	[ContextMenu("REFRESH PREFAB")]
	private void RefreshPrefab()
	{
		this.ReplacePrefab();
	}

	private void Update()
	{
		if (this.goSRC_Prv != this.goSRC_Prefab)
		{
			this.ReplacePrefab();
		}
	}

	private void OnInit()
	{
	}

	private void ReplacePrefab()
	{
		if (this.goSRC_Prefab != null)
		{
			this.goSRC_Prv = this.goSRC_Prefab;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				UnityEngine.Object.DestroyImmediate(transform.gameObject);
			}
			string name = this.goSRC_Prefab.name;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goSRC_Prefab);
			Vector3 localScale = gameObject.transform.localScale;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localScale = localScale;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			global::Debug.Log("ReplacePrefab");
			gameObject.name = name;
			this.target = gameObject;
		}
	}
}
