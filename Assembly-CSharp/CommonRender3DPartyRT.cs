using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRender3DPartyRT : MonoBehaviour
{
	private Camera cam;

	private RenderTexture renderTex;

	private List<GameObject> goCharaList;

	private List<CharacterParams> cpParamList;

	[Header("モンスター配置用Locater")]
	[SerializeField]
	private GameObject[] Locater;

	protected virtual void Awake()
	{
		IEnumerator enumerator = base.gameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.name == "Cam3D")
				{
					this.cam = transform.gameObject.GetComponent<Camera>();
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	protected virtual void OnDestroy()
	{
		if (this.cam != null && this.cam.targetTexture != null)
		{
			this.cam.targetTexture = null;
		}
		if (this.renderTex != null)
		{
			UnityEngine.Object.Destroy(this.renderTex);
		}
	}

	public void LoadCharas(List<string> pathList, float posX = 0f, float posY = 4000f)
	{
		this.goCharaList = new List<GameObject>();
		this.cpParamList = new List<CharacterParams>();
		int num = 0;
		foreach (string text in pathList)
		{
			GameObject original = AssetDataMng.Instance().LoadObject(text, null, true) as GameObject;
			this.goCharaList.Add(UnityEngine.Object.Instantiate<GameObject>(original));
			CharacterParams component = this.goCharaList[num].GetComponent<CharacterParams>();
			component.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
			this.cpParamList.Add(component);
			this.goCharaList[num].name = text;
			this.goCharaList[num].transform.SetParent(this.Locater[num].transform.parent);
			this.goCharaList[num].transform.transform.localPosition = Vector3.zero;
			this.goCharaList[num].transform.transform.localScale = Vector3.one;
			Vector3 previewCameraPosition = component.GetPreviewCameraPosition(this.cam.transform.parent);
			component.transform.position = this.cam.transform.position - previewCameraPosition + this.Locater[num].transform.position;
			component.transform.rotation = this.Locater[num].transform.rotation;
			num++;
		}
		base.gameObject.transform.localPosition = new Vector3(posX, posY, 0f);
	}

	public RenderTexture SetRenderTarget(int w, int h, int d = 16)
	{
		this.renderTex = new RenderTexture(w, h, d);
		this.renderTex.antiAliasing = 8;
		this.cam.targetTexture = this.renderTex;
		return this.renderTex;
	}

	public void SetAnimation(CharacterAnimationType type)
	{
		foreach (CharacterParams characterParams in this.cpParamList)
		{
			if (characterParams != null)
			{
				characterParams.PlayAnimationSmooth(type, SkillType.Attack, 0, null, null);
			}
		}
	}

	public GameObject GetChara(int index)
	{
		if (this.goCharaList.Count > index)
		{
			return this.goCharaList[index];
		}
		return null;
	}

	public void SetCameraBackgroundColor(Color color)
	{
		if (null != this.cam)
		{
			this.cam.backgroundColor = color;
		}
	}
}
