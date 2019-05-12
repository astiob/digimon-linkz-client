using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRender3DRT : MonoBehaviour
{
	private Camera cam;

	private RenderTexture renderTex;

	private GameObject goChara;

	private CharacterParams cpParams;

	private List<SkinnedMeshRenderer> smrList;

	private float rotY;

	[SerializeField]
	private float scrMaxX = 610f;

	[SerializeField]
	private float scrMaxY = 500f;

	[SerializeField]
	private float scrY = -210f;

	[SerializeField]
	private float charaZ = 0.7f;

	[SerializeField]
	private float partyScrMaxX = 615f;

	[SerializeField]
	private float partyScrMaxY = 310f;

	[SerializeField]
	private float partyScrY = -180f;

	[SerializeField]
	private float partyCharaZ = -0.2f;

	[SerializeField]
	private float n_partyScrMaxX = 615f;

	[SerializeField]
	private float n_partyScrMaxY = 310f;

	[SerializeField]
	private float n_partyScrY = -180f;

	[SerializeField]
	private float n_partyScrLimY = 200f;

	[SerializeField]
	private float n_partyCharaZ = -0.2f;

	[SerializeField]
	public CommonRender3DRT.PARTY_SHOW_TYPE party_show_type;

	[SerializeField]
	private float c_PartyChara_POSX;

	[SerializeField]
	private float c_PartyChara_POSY;

	[SerializeField]
	private float c_PartyChara_POSZ;

	[SerializeField]
	private float c_PartyChara_ROT;

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

	protected virtual void Start()
	{
	}

	protected virtual void OnDestroy()
	{
		if (this.renderTex != null)
		{
			UnityEngine.Object.Destroy(this.renderTex);
			this.renderTex = null;
		}
	}

	public Matrix4x4 projectionMatrix
	{
		get
		{
			return this.cam.projectionMatrix;
		}
		set
		{
			this.cam.projectionMatrix = value;
		}
	}

	public GameObject GetCameraGameObject()
	{
		return this.cam.gameObject;
	}

	public CharacterParams GetCharacterParams()
	{
		return this.cpParams;
	}

	public GameObject GetCharacterGameObject()
	{
		return this.goChara;
	}

	public void LoadChara(string path, float posX = 0f, float posY = 4000f, float cPosY = 0f, float cPosZ = 0f, bool useCamCont = true)
	{
		GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
		this.goChara = UnityEngine.Object.Instantiate<GameObject>(original);
		this.cpParams = this.goChara.GetComponent<CharacterParams>();
		if (null != this.cpParams)
		{
			this.cpParams.SetBillBoardCamera(this.cam);
			this.cpParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		}
		this.goChara.name = path;
		this.goChara.transform.SetParent(base.gameObject.transform);
		if (useCamCont)
		{
			if (null != this.cpParams)
			{
				this.cpParams.SetPreviewCamera(this.cam);
			}
		}
		else
		{
			this.goChara.transform.localPosition = new Vector3(0f, cPosY, cPosZ);
		}
		Quaternion localRotation = Quaternion.Euler(0f, -10f, 0f);
		this.goChara.transform.localRotation = localRotation;
		base.gameObject.transform.localPosition = new Vector3(posX, posY, 0f);
		this.smrList = CommonRender3DRT.GetCompoSMR(this.goChara);
		if (this.smrList != null)
		{
			this.SetSMRUpdateFlag(this.smrList);
		}
	}

	public void SetBillBoardCamera()
	{
		this.cpParams.SetBillBoardCamera(this.cam);
	}

	public void ResumeAnimation()
	{
		if (null != this.cpParams)
		{
			this.cpParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		}
	}

	public void LoadEgg(string path, float posX = 0f, float posY = 4000f, float cPosY = 0f)
	{
		GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
		this.goChara = UnityEngine.Object.Instantiate<GameObject>(original);
		this.goChara.name = path;
		this.goChara.transform.SetParent(base.gameObject.transform);
		Quaternion localRotation = Quaternion.Euler(0f, this.rotY, 0f);
		this.goChara.transform.localRotation = localRotation;
		base.gameObject.transform.localPosition = new Vector3(posX, posY, 0f);
	}

	public Vector3 GetCharacterCameraDistance()
	{
		if (null != this.cpParams)
		{
			return this.cpParams.GetPreviewCameraDifference();
		}
		return Vector3.zero;
	}

	public void LoadMonsterModel(string path, Vector3 characterLocalPosition, float characterLocalEulerAngleY)
	{
		GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
		this.goChara = UnityEngine.Object.Instantiate<GameObject>(original);
		this.goChara.name = path;
		this.goChara.transform.SetParent(base.gameObject.transform);
		this.goChara.transform.localPosition = characterLocalPosition;
		Quaternion localRotation = Quaternion.Euler(0f, characterLocalEulerAngleY, 0f);
		this.goChara.transform.localRotation = localRotation;
		this.rotY = characterLocalEulerAngleY;
		this.smrList = CommonRender3DRT.GetCompoSMR(this.goChara);
		if (this.smrList != null)
		{
			this.SetSMRUpdateFlag(this.smrList);
		}
		this.cpParams = this.goChara.GetComponent<CharacterParams>();
		if (null != this.cpParams)
		{
			this.cpParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		}
		this.SetBillBoardCamera();
	}

	public void DeleteCharacterModel()
	{
		if (null != this.goChara)
		{
			UnityEngine.Object.Destroy(this.goChara);
			this.goChara = null;
		}
	}

	public RenderTexture SetRenderTarget(int w, int h, int d = 16)
	{
		this.renderTex = new RenderTexture(w, h, d);
		this.renderTex.antiAliasing = 8;
		this.cam.targetTexture = this.renderTex;
		return this.renderTex;
	}

	public RenderTexture GetRenderTarget()
	{
		return this.renderTex;
	}

	public float RotY
	{
		get
		{
			return this.rotY;
		}
		set
		{
			this.rotY = value;
			Quaternion localRotation = Quaternion.Euler(0f, this.rotY, 0f);
			this.goChara.transform.localRotation = localRotation;
		}
	}

	public void AddCharacterRotationY(float addRotationEulerY)
	{
		this.rotY += addRotationEulerY;
		Quaternion localRotation = Quaternion.Euler(0f, this.rotY, 0f);
		this.goChara.transform.localRotation = localRotation;
	}

	public void SetAnimation(CharacterAnimationType type)
	{
		if (this.cpParams != null)
		{
			this.cpParams.PlayAnimationSmooth(type, SkillType.Attack, 0, null, null);
		}
	}

	private void SetSMRShader(List<SkinnedMeshRenderer> smrL, string shader)
	{
		Shader shader2 = Shader.Find(shader);
		for (int i = 0; i < smrL.Count; i++)
		{
			for (int j = 0; j < smrL[i].materials.Length; j++)
			{
				Material material = smrL[i].materials[j];
				if (material != null)
				{
					material.shader = shader2;
				}
			}
		}
	}

	private void SetSMRUpdateFlag(List<SkinnedMeshRenderer> smrL)
	{
		for (int i = 0; i < smrL.Count; i++)
		{
			smrL[i].updateWhenOffscreen = true;
		}
	}

	public void SetCharacterPositionForAll()
	{
		this.SetCharacterPosition(this.scrMaxX, this.scrMaxY, this.scrY, this.charaZ);
	}

	public void SetCharacterPositionForParty(float partyCharaPosX = 0f, float partyCharaPosY = 0f, float partyCharaPosZ = 0f, float partyCharaRotY = 0f)
	{
		switch (this.party_show_type)
		{
		case CommonRender3DRT.PARTY_SHOW_TYPE.STANDARD:
			this.SetCharacterPosition(this.partyScrMaxX, this.partyScrMaxY, this.partyScrY, this.partyCharaZ);
			break;
		case CommonRender3DRT.PARTY_SHOW_TYPE.LARGE:
		{
			this.SetCharacterPosition(this.n_partyScrMaxX, this.n_partyScrMaxY, this.n_partyScrY, this.n_partyCharaZ);
			float num = this.goChara.transform.localPosition.y + this.smrList[0].localBounds.size.y;
			float num2 = this.goChara.transform.localPosition.z - this.cam.gameObject.transform.localPosition.z;
			num2 += this.smrList[0].localBounds.size.z / 2f * 0.5f;
			float yzratioByScrY = this.GetYZRatioByScrY(this.n_partyScrLimY);
			float num3 = num2 * yzratioByScrY;
			Vector3 localPosition = this.goChara.transform.localPosition;
			if (num > num3)
			{
				localPosition.y = num3 - this.smrList[0].localBounds.size.y;
			}
			this.goChara.transform.localPosition = localPosition;
			break;
		}
		case CommonRender3DRT.PARTY_SHOW_TYPE.CUSTOM:
		{
			Vector3 localPosition = this.goChara.transform.localPosition;
			localPosition.x = this.c_PartyChara_POSX;
			localPosition.y = this.c_PartyChara_POSY;
			localPosition.z = this.c_PartyChara_POSZ;
			this.goChara.transform.localPosition = localPosition;
			Quaternion localRotation = Quaternion.Euler(0f, this.c_PartyChara_ROT, 0f);
			this.goChara.transform.localRotation = localRotation;
			break;
		}
		case CommonRender3DRT.PARTY_SHOW_TYPE.MASTER:
		{
			Vector3 localPosition = this.goChara.transform.localPosition;
			localPosition.x = partyCharaPosX;
			localPosition.y = partyCharaPosY;
			localPosition.z = partyCharaPosZ;
			this.goChara.transform.localPosition = localPosition;
			Quaternion localRotation = Quaternion.Euler(0f, partyCharaRotY, 0f);
			this.goChara.transform.localRotation = localRotation;
			break;
		}
		}
	}

	public void SetCharacterPosition(float scrMaxX, float scrMaxY, float scrY, float charaZ = 0.7f)
	{
		if (this.smrList != null)
		{
			float defZ = charaZ - this.cam.gameObject.transform.localPosition.z;
			float num = this.GetZPosByScrVol(scrMaxX, scrMaxY, defZ);
			float z = num + this.cam.gameObject.transform.localPosition.z;
			num -= this.smrList[0].localBounds.size.z / 2f * 0.7f;
			float yzratioByScrY = this.GetYZRatioByScrY(scrY);
			float y = num * yzratioByScrY;
			Vector3 localPosition = this.goChara.transform.localPosition;
			localPosition.y = y;
			localPosition.z = z;
			this.goChara.transform.localPosition = localPosition;
		}
	}

	private float GetZPosByScrVol(float scrMaxX, float scrMaxY, float defZ)
	{
		float scrZ = this.GetScrZ();
		float x = this.smrList[0].localBounds.size.x;
		float y = this.smrList[0].localBounds.size.y;
		float num = scrZ / scrMaxX * x;
		float num2 = scrZ / scrMaxY * y;
		float num3 = defZ;
		if (num > num3)
		{
			num3 = num;
		}
		if (num2 > num3)
		{
			num3 = num2;
		}
		return num3;
	}

	private float GetYZRatioByScrY(float scrY)
	{
		float scrZ = this.GetScrZ();
		return scrY / scrZ;
	}

	private float GetScrZ()
	{
		float result = 0f;
		if (this.cam != null)
		{
			float num = this.cam.fieldOfView / 2f;
			float num2 = Mathf.Cos(num * 0.0174532924f);
			float num3 = Mathf.Sin(num * 0.0174532924f);
			float num4 = num2 / num3;
			float num5 = (float)this.renderTex.height / 2f;
			float num6 = num5 * num4;
			result = num6;
		}
		return result;
	}

	public static List<SkinnedMeshRenderer> GetCompoSMR(GameObject go)
	{
		List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
		CommonRender3DRT.SearchCompoSMR(go, list);
		return list;
	}

	private static void SearchCompoSMR(GameObject go, List<SkinnedMeshRenderer> smrL)
	{
		IEnumerator enumerator = go.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				SkinnedMeshRenderer component = transform.gameObject.GetComponent<SkinnedMeshRenderer>();
				if (component != null)
				{
					smrL.Add(component);
				}
				else
				{
					CommonRender3DRT.SearchCompoSMR(transform.gameObject, smrL);
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

	public static List<Material> GetAllMaterialsInSMRS(List<SkinnedMeshRenderer> smrL)
	{
		List<Material> list = new List<Material>();
		for (int i = 0; i < smrL.Count; i++)
		{
			for (int j = 0; j < smrL[i].materials.Length; j++)
			{
				Material material = smrL[i].materials[j];
				if (material != null)
				{
					list.Add(material);
				}
			}
		}
		return list;
	}

	public static List<float> GetOutlineWidth(List<Material> matL)
	{
		List<float> list = new List<float>();
		for (int i = 0; i < matL.Count; i++)
		{
			if (matL[i].HasProperty("_OutlineWidth"))
			{
				float @float = matL[i].GetFloat("_OutlineWidth");
				list.Add(@float);
			}
			else
			{
				matL[i] = null;
				list.Add(0f);
			}
		}
		return list;
	}

	public enum PARTY_SHOW_TYPE
	{
		STANDARD,
		LARGE,
		CUSTOM,
		EDITOR,
		MASTER
	}
}
