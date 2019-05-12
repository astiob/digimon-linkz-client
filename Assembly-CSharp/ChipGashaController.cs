using Cutscene;
using Cutscene.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChipGashaController : CutsceneBase
{
	[SerializeField]
	private GameObject lightLocatorRoot;

	[Header("動作ロケーターのゲームオブジェクト一覧")]
	[SerializeField]
	private List<GameObject> goLocatorList;

	[Header("動作ロケーター光弾用のゲームオブジェクト一覧")]
	[SerializeField]
	private List<GameObject> goLightLocatorList;

	[Header("光弾エフェクト")]
	[SerializeField]
	private GameObject goPartsLight;

	[Header("青色→青色")]
	[SerializeField]
	private GameObject goPartsBlue;

	[Header("青色→黄色")]
	[SerializeField]
	private GameObject goPartsYellow;

	[Header("青色→虹色")]
	[SerializeField]
	private GameObject goPartsRainbow;

	[Header("フェードアウト開始フレーム")]
	[SerializeField]
	private int startFadeOutFrame;

	[Header("メインカメラ")]
	[SerializeField]
	private Camera mainCam;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private GameObject animationRoot;

	private Action<RenderTexture> endCallback;

	private string bgmFileName;

	private Vector2 backgroundSize;

	private int frameCT;

	private ChipGashaController.EndState endState;

	private void SetEffectLocator(GameObject locator, GameObject effect)
	{
		Transform transform = effect.transform;
		Vector3 localScale = transform.localScale;
		transform.parent = locator.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = localScale;
		effect.SetActive(true);
	}

	private void StartFadeOut()
	{
		this.endState = ChipGashaController.EndState.FADE_OUT;
		this.allSkipButton.Hide();
		this.fade.StartFadeOut(new Action(this.EndCutscene));
	}

	private void EndCutscene()
	{
		RenderTexture renderTexture = new RenderTexture((int)this.backgroundSize.x, (int)this.backgroundSize.y, 16);
		renderTexture.antiAliasing = 2;
		this.mainCam.targetTexture = renderTexture;
		this.lightLocatorRoot.SetActive(false);
		this.endState = ChipGashaController.EndState.FINISH_RENDER_TEXTURE;
	}

	private void Finish()
	{
		if (this.endCallback != null)
		{
			this.endCallback(this.mainCam.targetTexture);
		}
		for (int i = 0; i < this.goLocatorList.Count; i++)
		{
			this.goLocatorList[i].SetActive(false);
		}
		for (int j = 0; j < this.goLocatorList.Count; j++)
		{
			this.goLightLocatorList[j].SetActive(false);
		}
		this.mainCam.targetTexture = null;
		UnityEngine.Object.Destroy(base.gameObject);
		GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	protected override void OnStartCutscene()
	{
		if (!this.animationRoot.gameObject.activeSelf)
		{
			this.animationRoot.gameObject.SetActive(true);
			this.allSkipButton.Show();
			SoundMng.Instance().PlayGameBGM(this.bgmFileName);
		}
	}

	protected override void OnUpdate()
	{
		if (this.startFadeOutFrame < this.frameCT)
		{
			if (this.endState == ChipGashaController.EndState.FINISH_ANIMATION)
			{
				this.StartFadeOut();
			}
			else if (this.endState == ChipGashaController.EndState.FINISH_RENDER_TEXTURE)
			{
				this.Finish();
			}
		}
		this.frameCT++;
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataChipGasha cutsceneDataChipGasha = data as CutsceneDataChipGasha;
		if (cutsceneDataChipGasha != null)
		{
			this.endCallback = cutsceneDataChipGasha.endCallback;
			this.bgmFileName = cutsceneDataChipGasha.bgmFileName;
			this.backgroundSize = cutsceneDataChipGasha.backgroundSize;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(delegate
			{
				this.frameCT = this.startFadeOutFrame;
				this.StartFadeOut();
			});
			this.allSkipButton.Hide();
			GameWebAPI.RespDataGA_ExecChip.UserAssetList[] gashaResult = cutsceneDataChipGasha.gashaResult;
			int i = 0;
			while (i < gashaResult.Length)
			{
				string effectType = gashaResult[i].effectType;
				if (effectType == null || effectType == "1")
				{
					goto IL_B7;
				}
				GameObject effect;
				if (!(effectType == "2"))
				{
					if (!(effectType == "3"))
					{
						goto IL_B7;
					}
					effect = UnityEngine.Object.Instantiate<GameObject>(this.goPartsRainbow);
				}
				else
				{
					effect = UnityEngine.Object.Instantiate<GameObject>(this.goPartsYellow);
				}
				IL_EA:
				this.SetEffectLocator(this.goLocatorList[i], effect);
				effect = UnityEngine.Object.Instantiate<GameObject>(this.goPartsLight);
				this.SetEffectLocator(this.goLightLocatorList[i], effect);
				i++;
				continue;
				IL_B7:
				effect = UnityEngine.Object.Instantiate<GameObject>(this.goPartsBlue);
				goto IL_EA;
			}
			for (int j = gashaResult.Length; j < this.goLocatorList.Count; j++)
			{
				this.goLocatorList[j].SetActive(false);
				this.goLightLocatorList[j].SetActive(false);
			}
		}
	}

	private enum EndState
	{
		FINISH_ANIMATION,
		FADE_OUT,
		FINISH_RENDER_TEXTURE
	}
}
