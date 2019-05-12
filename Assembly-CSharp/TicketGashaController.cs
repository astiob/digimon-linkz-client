using Cutscene;
using Cutscene.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TicketGashaController : CutsceneBase
{
	[Header("カードアニメフレーム間隔")]
	[SerializeField]
	private int cardAnimIntervalFrame;

	[Header("メインカメラ")]
	[SerializeField]
	private Camera mainCam;

	[Header("カードエフェクト : 1:白, 2:黄色, 3:虹")]
	[SerializeField]
	private List<GameObject> goCardEfcList;

	[Header("カードアニメ : 1:白, 2:黄色, 3:虹")]
	[SerializeField]
	private List<GameObject> goCardAnimList;

	[SerializeField]
	private AllSkipButton allSkipButton;

	[SerializeField]
	private GameObject animationRoot;

	private Action<RenderTexture> endCallback;

	public string bgmFileName;

	private Vector2 backgroundSize;

	private string[] effectTypeList;

	private List<GameObject> goCardList = new List<GameObject>();

	private int startFadeOutFrame;

	private int cardEfcCT;

	private int cardFrameCT;

	private int frameCT;

	private TicketGashaController.EndState endState;

	private GameObject CreateEffectCard(string effectType)
	{
		int index = int.Parse(effectType) - 1;
		GameObject original = this.goCardAnimList[index];
		return UnityEngine.Object.Instantiate<GameObject>(original);
	}

	private GameObject CreateEffectStar(string effectType)
	{
		int index = int.Parse(effectType) - 1;
		GameObject original = this.goCardEfcList[index];
		return UnityEngine.Object.Instantiate<GameObject>(original);
	}

	private Vector3 GetPosition(GameObject card)
	{
		Vector3 localPosition = card.transform.localPosition;
		localPosition.z = 110f;
		return localPosition;
	}

	private Vector3 GetRandomPosition(GameObject card)
	{
		Vector3 localPosition = card.transform.localPosition;
		localPosition.z = 110f;
		float num = UnityEngine.Random.Range(-5f, 5f);
		float num2 = UnityEngine.Random.Range(-2f, 2f);
		localPosition.x += num;
		localPosition.y += num2;
		return localPosition;
	}

	private void KickEffectCard(GameObject card, Vector3 position)
	{
		Transform transform = card.transform;
		Vector3 localScale = transform.localScale;
		Quaternion localRotation = transform.localRotation;
		transform.parent = this.mainCam.gameObject.transform;
		transform.localPosition = position;
		transform.localScale = localScale;
		transform.localRotation = localRotation;
		this.goCardList.Add(card);
	}

	private void KickEffectStar(GameObject star, GameObject parentEffect)
	{
		Transform transform = star.transform;
		Vector3 localPosition = transform.localPosition;
		localPosition.z += 0.1f;
		Vector3 localScale = transform.localScale;
		Quaternion localRotation = transform.localRotation;
		transform.parent = parentEffect.transform.GetChild(0);
		transform.localPosition = localPosition;
		transform.localScale = localScale;
		transform.localRotation = localRotation;
	}

	private void UpdateCardEffect()
	{
		if (this.cardFrameCT % this.cardAnimIntervalFrame == 0 && this.cardEfcCT < this.effectTypeList.Length)
		{
			GameObject gameObject;
			if (this.effectTypeList.Length == 1)
			{
				gameObject = this.CreateEffectCard(this.effectTypeList[this.cardEfcCT]);
				Vector3 position = this.GetPosition(gameObject);
				this.KickEffectCard(gameObject, position);
			}
			else
			{
				gameObject = this.CreateEffectCard(this.effectTypeList[this.cardEfcCT]);
				Vector3 randomPosition = this.GetRandomPosition(gameObject);
				this.KickEffectCard(gameObject, randomPosition);
			}
			GameObject star = this.CreateEffectStar(this.effectTypeList[this.cardEfcCT]);
			this.KickEffectStar(star, gameObject);
			this.cardEfcCT++;
		}
		this.cardFrameCT++;
	}

	private void StartFadeOut()
	{
		this.endState = TicketGashaController.EndState.FADE_OUT;
		this.allSkipButton.Hide();
		this.fade.StartFadeOut(new Action(this.EndCutscene));
	}

	private void EndCutscene()
	{
		RenderTexture renderTexture = new RenderTexture((int)this.backgroundSize.x, (int)this.backgroundSize.y, 16);
		renderTexture.antiAliasing = 2;
		this.mainCam.targetTexture = renderTexture;
		for (int i = 0; i < this.goCardList.Count; i++)
		{
			UnityEngine.Object.Destroy(this.goCardList[i]);
		}
		this.endState = TicketGashaController.EndState.FINISH_RENDER_TEXTURE;
	}

	private void Finish()
	{
		if (this.endCallback != null)
		{
			this.endCallback(this.mainCam.targetTexture);
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
		this.UpdateCardEffect();
		if (this.startFadeOutFrame < this.frameCT)
		{
			if (this.endState == TicketGashaController.EndState.FINISH_ANIMATION)
			{
				this.StartFadeOut();
			}
			else if (this.endState == TicketGashaController.EndState.FINISH_RENDER_TEXTURE)
			{
				this.Finish();
			}
		}
		this.frameCT++;
	}

	public override void SetData(CutsceneDataBase data)
	{
		CutsceneDataTicketGasha cutsceneDataTicketGasha = data as CutsceneDataTicketGasha;
		if (cutsceneDataTicketGasha != null)
		{
			this.endCallback = cutsceneDataTicketGasha.endCallback;
			this.bgmFileName = cutsceneDataTicketGasha.bgmFileName;
			this.backgroundSize = cutsceneDataTicketGasha.backgroundSize;
			this.allSkipButton.Initialize();
			this.allSkipButton.AddAction(delegate
			{
				this.frameCT = this.startFadeOutFrame;
				this.StartFadeOut();
			});
			this.allSkipButton.Hide();
			this.startFadeOutFrame = this.cardAnimIntervalFrame * cutsceneDataTicketGasha.gashaResult.Length + 10;
			this.effectTypeList = new string[cutsceneDataTicketGasha.gashaResult.Length];
			for (int i = 0; i < cutsceneDataTicketGasha.gashaResult.Length; i++)
			{
				this.effectTypeList[i] = cutsceneDataTicketGasha.gashaResult[i].effectType;
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
