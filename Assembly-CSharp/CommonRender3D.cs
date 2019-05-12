using System;
using UnityEngine;

public class CommonRender3D : MonoBehaviour
{
	private GameObject goChara;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		base.gameObject.transform.localPosition = new Vector3(0f, 4000f, 0f);
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public void LoadChara(string path)
	{
		GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
		this.goChara = UnityEngine.Object.Instantiate<GameObject>(original);
		CharacterParams component = this.goChara.GetComponent<CharacterParams>();
		component.PlayIdleAnimation();
		this.goChara.name = path;
		this.goChara.transform.SetParent(base.gameObject.transform);
		this.goChara.transform.localPosition = new Vector3(0f, 0f, 0f);
		Quaternion localRotation = Quaternion.Euler(0f, 180f, 0f);
		this.goChara.transform.localRotation = localRotation;
	}
}
