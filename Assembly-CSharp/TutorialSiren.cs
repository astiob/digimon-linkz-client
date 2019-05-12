using System;
using UnityEngine;

public sealed class TutorialSiren : MonoBehaviour
{
	private UITexture projectionTexture;

	private FadeCircle fadeCircle;

	private void Start()
	{
		this.projectionTexture = base.GetComponent<UITexture>();
		GameObject original = AssetDataMng.Instance().LoadObject("UICommon/Render2D/FadeCircleROOT", null, true) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Resources.UnloadUnusedAssets();
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		this.fadeCircle = gameObject.GetComponent<FadeCircle>();
		UIPanel uipanel = GUIMain.GetUIPanel();
		this.fadeCircle.Initialize(uipanel.GetWindowSize());
		this.fadeCircle.StartAnimation();
		this.SetSirenEffectTexture(this.fadeCircle);
	}

	private void SetSirenEffectTexture(FadeCircle fadeCircle)
	{
		this.projectionTexture.width = Mathf.RoundToInt(fadeCircle.GetTextureWidth());
		this.projectionTexture.height = Mathf.RoundToInt(fadeCircle.GetTextureHeight());
		this.projectionTexture.mainTexture = fadeCircle.GetRenderTexture();
		this.projectionTexture.enabled = true;
	}

	private void OnDisable()
	{
		this.projectionTexture.enabled = false;
		this.projectionTexture.mainTexture = null;
		if (null != this.fadeCircle)
		{
			UnityEngine.Object.Destroy(this.fadeCircle.gameObject);
			UnityEngine.Object.Destroy(this.fadeCircle);
		}
		this.fadeCircle = null;
		Resources.UnloadUnusedAssets();
	}
}
