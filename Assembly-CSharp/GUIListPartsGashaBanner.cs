using System;
using System.Collections;
using UnityEngine;

public class GUIListPartsGashaBanner : GUIListPartBS
{
	public GUISelectPanelGashaEdit selectPanelGasha;

	public int listNum;

	public string nameId;

	private Texture bannerTex;

	private UITexture uiTex;

	protected override void Awake()
	{
		base.Awake();
		if (base.gameObject.transform.parent != null)
		{
			this.parent = base.gameObject.transform.parent.gameObject.GetComponent<GUICollider>();
		}
		this.uiTex = base.gameObject.GetComponent<UITexture>();
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.TextView());
	}

	private IEnumerator TextView()
	{
		if (this.uiTex == null)
		{
			yield break;
		}
		if (this.bannerTex != null)
		{
			this.uiTex.mainTexture = this.bannerTex;
			yield break;
		}
		string downloadURL = AssetDataMng.GetWebAssetImagePath() + "/gasha/" + this.nameId;
		yield return TextureManager.instance.Load(downloadURL, new Action<Texture2D>(this.OnLoad), DownloadGashaTopTex.Instance.TimeoutSeconds, true);
		yield break;
	}

	private void OnLoad(Texture2D texture2D)
	{
		this.uiTex.mainTexture = texture2D;
		this.bannerTex = texture2D;
	}

	public void ReleaseTex()
	{
		Texture mainTexture = null;
		this.uiTex.mainTexture = mainTexture;
		this.bannerTex = mainTexture;
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this.beganPostion = pos;
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
		this.selectPanelGasha.timeCoun = this.selectPanelGasha.limitOrigin;
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
	}

	protected override void Update()
	{
		base.Update();
		if (base.gameObject.transform.position.x > 0.7f && base.gameObject.transform.position.x < 0.9f && this.selectPanelGasha.selectNum != this.listNum)
		{
			this.selectPanelGasha.selectNum = this.listNum;
		}
	}
}
