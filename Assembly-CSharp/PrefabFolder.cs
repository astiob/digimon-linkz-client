using NGUI.Extensions;
using System;
using UnityEngine;

[AddComponentMenu("GUI/PrefabFolder")]
[ExecuteInEditMode]
public sealed class PrefabFolder : MonoBehaviour
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
			if (null != this.target)
			{
				UtilForCMD component = this.target.GetComponent<UtilForCMD>();
				if (null != component)
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

	private void ReplacePrefab()
	{
		if (null != this.goSRC_Prefab)
		{
			this.goSRC_Prv = this.goSRC_Prefab;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goSRC_Prefab);
			Vector3 localScale = gameObject.transform.localScale;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localScale = localScale;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = this.goSRC_Prefab.name;
			this.target = gameObject;
			IUISafeAreaChildren[] componentsInChildren = gameObject.GetComponentsInChildren<IUISafeAreaChildren>();
			if (componentsInChildren != null)
			{
				UISafeArea componentInParent = base.GetComponentInParent<UISafeArea>();
				if (null != componentInParent)
				{
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].SetAnchorTarget(componentInParent.transform);
					}
				}
			}
		}
	}
}
