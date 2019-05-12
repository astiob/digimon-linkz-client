using System;
using UnityEngine;

public class TutorialShutdown : MonoBehaviour
{
	private UITexture projectionTexture;

	private TVFade tvFade;

	private Action onFinishedAnimation;

	public Action OnFinishedAnimation
	{
		set
		{
			this.onFinishedAnimation = value;
		}
	}

	private void Start()
	{
		this.projectionTexture = base.GetComponent<UITexture>();
		GameObject original = AssetDataMng.Instance().LoadObject("UICommon/Render2D/TVFadeROOT", null, true) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Resources.UnloadUnusedAssets();
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		this.tvFade = gameObject.GetComponent<TVFade>();
		this.tvFade.StartAnimation(this.onFinishedAnimation);
		this.SetEffectTexture(this.tvFade);
	}

	private void SetEffectTexture(TVFade effect)
	{
		this.projectionTexture.width = Mathf.RoundToInt(effect.GetTextureWidth());
		this.projectionTexture.height = Mathf.RoundToInt(effect.GetTextureHeight());
		this.projectionTexture.mainTexture = effect.GetRenderTexture();
		this.projectionTexture.enabled = true;
	}

	private void OnDisable()
	{
		this.projectionTexture.enabled = false;
		this.projectionTexture.mainTexture = null;
		if (null != this.tvFade)
		{
			UnityEngine.Object.Destroy(this.tvFade.gameObject);
			UnityEngine.Object.Destroy(this.tvFade);
		}
		this.tvFade = null;
		Resources.UnloadUnusedAssets();
	}
}
